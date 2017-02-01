package Tab::Judge;
use base 'Tab::DBI';
Tab::Judge->table('judge');
Tab::Judge->columns(Primary => qw/id/);
Tab::Judge->columns(Essential => qw/school first middle last code active ada category person chapter_judge score tmp/);

Tab::Judge->columns(Others => qw / alt_category covers obligation hired person_request timestamp /);

# Wow, that's a lot. 
Tab::Judge->columns(TEMP => qw/tier pref panelid chair hangout_admin tourn avg eventtype eventid
							   diet ballotid personid tab_rating cjid schoolname 
							   schoolcode regname regcode region standby site neutral diversity/);

Tab::Judge->has_a(category    => 'Tab::Category');
Tab::Judge->has_a(alt_category      => 'Tab::Category');
Tab::Judge->has_a(covers         => 'Tab::Category');
Tab::Judge->has_a(school         => 'Tab::School');
Tab::Judge->has_a(person         => 'Tab::Person');
Tab::Judge->has_a(person_request => 'Tab::Person');
Tab::Judge->has_a(chapter_judge  => 'Tab::ChapterJudge');

Tab::Judge->has_many(ratings => 'Tab::Rating', 'judge');
Tab::Judge->has_many(strikes => 'Tab::Strike', 'judge');
Tab::Judge->has_many(ballots => 'Tab::Ballot', 'judge');
Tab::Judge->has_many(settings => "Tab::JudgeSetting", "judge");
Tab::Judge->has_many(hires => "Tab::JudgeHire", "judge");

Tab::Judge->has_many(jpools => [Tab::JPoolJudge => 'jpool']);

Tab::Judge->set_sql(highest_code => "select MAX(code) from judge where category = ?");
Tab::Judge->set_sql(lowest_code => "select MIN(code) from judge where category = ?");

__PACKAGE__->_register_datetimes( qw/timestamp /);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::JudgeSetting->search(  
		judge => $self->id,
		tag   => $tag,
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

			my $existing = Tab::JudgeSetting->create({
				judge => $self->id,
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

