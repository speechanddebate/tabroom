package Tab::Person;
use base 'Tab::DBI';

Tab::Person->table('person');
Tab::Person->columns(Primary   => qw/id/);

Tab::Person->columns(Essential => qw/email first middle last phone ualt_id 
									 provider site_admin /);

Tab::Person->columns(Others    => qw/street city state zip country postal
                                   gender pronoun no_email tz diversity 
								   timestamp googleplus/);

Tab::Person->columns(TEMP => qw/prefs student_id judge_id/);

Tab::Person->has_many(logins => 'Tab::Login', 'person');

Tab::Person->has_many(settings => 'Tab::PersonSetting', 'person');
Tab::Person->has_many(sessions => 'Tab::Session', 'person');

Tab::Person->has_many(sites => 'Tab::Site', 'host');
Tab::Person->has_many(conflicts => 'Tab::Conflict', 'person');
Tab::Person->has_many(conflicteds => 'Tab::Conflict', 'conflicted');

Tab::Person->has_many(followers => 'Tab::Follower', 'person');
Tab::Person->has_many(follow_persons => 'Tab::Follower', 'follower');

Tab::Person->has_many(chapter_judges => 'Tab::ChapterJudge', 'person');
Tab::Person->has_many(judges => 'Tab::Judge', 'person' => { order_by => 'id DESC'} );

Tab::Person->has_many(students => 'Tab::Student', 'person');
Tab::Person->has_many(ignores => [ Tab::TournIgnore => 'tourn']);

Tab::Person->has_many(permissions => 'Tab::Permission', 'person');
Tab::Person->has_many(circuits => [ Tab::Permission => 'circuit']);
Tab::Person->has_many(tourns => [ Tab::Permission => 'tourn']);
Tab::Person->has_many(chapters => [ Tab::Permission => 'chapter']);
Tab::Person->has_many(regions => [ Tab::Permission => 'region']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::PersonSetting->search(  
		person => $self->id,
		tag    => $tag,
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

			my $existing = Tab::PersonSetting->create({
				person => $self->id,
				tag    => $tag,
				value  => $value,
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

sub all_settings { 

	my $self = shift;

	my @settings = $self->settings;

	my %all_settings;

	foreach my $setting (@settings) { 
		$all_settings{$setting->tag} = $setting->value;
		$all_settings{$setting->tag} = $setting->value_text if $all_settings{$setting->tag} eq "text";
		$all_settings{$setting->tag} = $setting->value_date if $all_settings{$setting->tag} eq "date";
	}

	return %all_settings;

}

