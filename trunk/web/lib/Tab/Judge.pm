package Tab::Judge;
use base 'Tab::DBI';
Tab::Judge->table('judge');
Tab::Judge->columns(Primary => qw/id/);
Tab::Judge->columns(Essential => qw/school first last code active ada gender notes 
									judge_group alt_group covers chapter_judge obligation hired 
									account acct_request score tmp 
									reg_time timestamp /);

Tab::Judge->columns(TEMP => qw/tier pref panelid chair tourn avg diet ballotid accountid tab_rating
							   cjid schoolname schoolcode regname regcode region/);

Tab::Judge->has_a(judge_group   => 'Tab::JudgeGroup');
Tab::Judge->has_a(alt_group     => 'Tab::JudgeGroup');
Tab::Judge->has_a(covers        => 'Tab::JudgeGroup');
Tab::Judge->has_a(school        => 'Tab::School');
Tab::Judge->has_a(drop_by       => 'Tab::Account');
Tab::Judge->has_a(account       => 'Tab::Account');
Tab::Judge->has_a(acct_request  => 'Tab::Account');
Tab::Judge->has_a(chapter_judge => 'Tab::ChapterJudge');

Tab::Judge->has_many(ratings => 'Tab::Rating', 'judge');
Tab::Judge->has_many(strikes => 'Tab::Strike', 'judge');
Tab::Judge->has_many(ballots => 'Tab::Ballot', 'judge');
Tab::Judge->has_many(settings => "Tab::JudgeSetting", "judge");
Tab::Judge->has_many(hires => "Tab::JudgeHire", "judge");

Tab::Judge->has_many(jpools => [Tab::JPoolJudge => 'jpool']);

Tab::Judge->set_sql(highest_code => "select MAX(code) from judge where judge_group = ?");
Tab::Judge->set_sql(lowest_code => "select MIN(code) from judge where judge_group = ?");

__PACKAGE__->_register_datetimes( qw/drop_time reg_time/);

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

