package Tab::Person;
use base 'Tab::DBI';
Tab::Person->table('person');

Tab::Person->columns(Primary =>   qw/id/);
Tab::Person->columns(Essential => qw/email first last phone ualt_id provider site_admin multiple/);
Tab::Person->columns(Others =>    qw/alt_phone street city state zip country 
									 gender no_email tz started_judging diversity flags timestamp/);
Tab::Person->columns(TEMP => qw/prefs/);

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_dates( qw/started_judging/);

Tab::Person->has_many(sessions => 'Tab::Session', 'account');
Tab::Person->has_many(sites => 'Tab::Site', 'host');
Tab::Person->has_many(conflicts => 'Tab::AccountConflict', 'account');
Tab::Person->has_many(conflicteds => 'Tab::AccountConflict', 'conflict');

Tab::Person->has_many(tourn_admins => 'Tab::TournAdmin', 'account');
Tab::Person->has_many(chapter_admins => 'Tab::ChapterAdmin', 'account');
Tab::Person->has_many(circuit_admins => 'Tab::CircuitAdmin', 'account');
Tab::Person->has_many(region_admins => 'Tab::RegionAdmin', 'account');

Tab::Person->has_many(follows => 'Tab::Follower', 'account');
Tab::Person->has_many(followers => 'Tab::Follower', 'follower');
Tab::Person->has_many(chapter_judges => 'Tab::ChapterJudge', 'account');

Tab::Person->has_many(judges => 'Tab::Judge', 'account' => { order_by => 'id DESC'} );

Tab::Person->has_many(students => 'Tab::Student');

Tab::Person->has_many(ignores => [ Tab::TournIgnore => 'tourn']);
Tab::Person->has_many(circuits => [ Tab::CircuitAdmin => 'circuit']);
Tab::Person->has_many(tourns => [ Tab::TournAdmin => 'tourn']);
Tab::Person->has_many(chapters => [ Tab::ChapterAdmin => 'chapter']);
Tab::Person->has_many(regions => [ Tab::RegionAdmin => 'region']);

sub can_alter {

	my ($self,$school) = @_;

	return if $self->site_admin;
	return if Tab::ChapterAdmin->search( account => $self->id, chapter => $school->chapter->id );
	return if Tab::CircuitAdmin->search( account => $self->id, circuit => $school->tourn->circuit->id );

	$m->print("<p class=\"err\">You are not authorized to make changes to that school's entry.  Hit back and try again.</p>");
	$m->abort();
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::Setting->search(  
		person => $self->id,
		tag    => $tag,
		type   => "person"
	)->first;

	if (defined $value) { 
			
		if ($existing) {

			$existing->value($value);
			$existing->value_text($blob) if $value eq "text";
			$existing->value_date($blob) if $value eq "date";
			$existing->update;

			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$existing->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::Setting->create({
				person => $self->id,
				tag    => $tag,
				value  => $value,
				type   => "person"
			});

			if ($value eq "text") { 
				$existing->value_text($blob);
			}

			if ($value eq "date") { 
				$existing->value_date($blob);
			}

			$existing->update;

		}

	} else {

		return unless $existing;
		return $existing->value_text if $existing->value eq "text";
		return $existing->value_date if $existing->value eq "date";
		return $existing->value;

	}

}

