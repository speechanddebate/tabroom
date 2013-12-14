package Tab::Account;
use base 'Tab::DBI';
Tab::Account->table('account');

Tab::Account->columns(Primary =>   qw/id/);
Tab::Account->columns(Essential => qw/email passhash site_admin multiple/);
Tab::Account->columns(Others =>    qw/first last phone street city state zip country hotel idebate_site
									provider paradigm paradigm_timestamp started_judging gender timestamp help_admin
									no_email change_deadline change_pass_key password_timestamp tz/);
Tab::Account->columns(TEMP => qw/prefs/);

__PACKAGE__->_register_datetimes( qw/paradigm_timestamp password_timestamp change_deadline/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::Account->has_many(sessions => 'Tab::Session', 'account');
Tab::Account->has_many(sites => 'Tab::Site', 'host');
Tab::Account->has_many(conflicts => 'Tab::AccountConflict', 'account');
Tab::Account->has_many(conflicteds => 'Tab::AccountConflict', 'conflict');

Tab::Account->has_many(tourn_admins => 'Tab::TournAdmin', 'account');
Tab::Account->has_many(chapter_admins => 'Tab::ChapterAdmin', 'account');
Tab::Account->has_many(circuit_admins => 'Tab::CircuitAdmin', 'account');
Tab::Account->has_many(region_admins => 'Tab::RegionAdmin', 'account');

Tab::Account->has_many(followers => 'Tab::FollowAccount', 'account');
Tab::Account->has_many(follow_accounts => 'Tab::FollowAccount', 'follower');
Tab::Account->has_many(chapter_judges => 'Tab::ChapterJudge', 'account');

Tab::Account->has_many(judges => 'Tab::Judge', 'account');
Tab::Account->has_many(follow_judge => 'Tab::FollowJudge', 'account');

Tab::Account->has_many(entries => 'Tab::Entry', 'account');
Tab::Account->has_many(students => 'Tab::Student');
Tab::Account->has_many(follow_entry => 'Tab::FollowEntry', 'entry');

Tab::Account->has_many(ignores => [ Tab::TournIgnore => 'tourn']);
Tab::Account->has_many(circuits => [ Tab::CircuitAdmin => 'circuit']);
Tab::Account->has_many(tourns => [ Tab::TournAdmin => 'tourn']);
Tab::Account->has_many(chapters => [ Tab::ChapterAdmin => 'chapter']);
Tab::Account->has_many(regions => [ Tab::RegionAdmin => 'region']);

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

	my @existing = Tab::AccountSetting->search(  
		account => $self->id,
		tag => $tag
	);

	if (defined $value) { 
			
		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->value_text($blob) if $value eq "text";
			$exists->value_date($blob) if $value eq "date";
			$exists->update;


			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$exists->delete;
			}

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $exists = Tab::AccountSetting->create({
				account => $self->id,
				tag => $tag,
				value => $value,
			});

			if ($value eq "text") { 
				$exists->value_text($blob);
			}

			if ($value eq "date") { 
				$exists->value_date($blob);
			}

			$exists->update;

		}

	} else {

		return unless @existing;
		my $setting = shift @existing;
		return $setting->value_text if $setting->value eq "text";
		return $setting->value_date if $setting->value eq "date";
		return $setting->value;

	}

}

