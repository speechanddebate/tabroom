package Tab::Judge;
use base 'Tab::DBI';
Tab::Judge->table('judge');
Tab::Judge->columns(Primary => qw/id/);
Tab::Judge->columns(Essential => qw/school first last code active ada
									special notes judge_group alt_group gender
									timestamp covers chapter_judge obligation 
									account acct_request dropped drop_time 
									reg_time drop_by hired score tmp standby
									hire_offer hire_approved cat_id diverse/);

Tab::Judge->columns(TEMP => qw/tier pref panelid chair tourn avg diet ballotid accountid tab_rating
							   cjid schoolname schoolcode regname regcode region/);

Tab::Judge->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Judge->has_a(alt_group => 'Tab::JudgeGroup');
Tab::Judge->has_a(covers => 'Tab::JudgeGroup');
Tab::Judge->has_a(school => 'Tab::School');
Tab::Judge->has_a(drop_by => 'Tab::Account');
Tab::Judge->has_a(account => 'Tab::Account');
Tab::Judge->has_a(acct_request => 'Tab::Account');
Tab::Judge->has_a(chapter_judge => 'Tab::ChapterJudge');

Tab::Judge->has_many(ratings => 'Tab::Rating', 'judge');
Tab::Judge->has_many(strikes => 'Tab::Strike', 'judge');
Tab::Judge->has_many(ballots => 'Tab::Ballot', 'judge');
Tab::Judge->has_many(settings => "Tab::JudgeSetting", "judge");
Tab::Judge->has_many(hires => "Tab::JudgeHire", "judge");

Tab::Judge->has_many(groups => [Tab::JudgeGroupJudge => 'judge_group']);

Tab::Judge->has_many(jpools => [Tab::JPoolJudge => 'jpool']);

Tab::Judge->set_sql(highest_code => "select MAX(code) from judge where judge_group = ?");
Tab::Judge->set_sql(lowest_code => "select MIN(code) from judge where judge_group = ?");

__PACKAGE__->_register_datetimes( qw/drop_time reg_time/);

sub judge_group { 
	my $self = shift;
	return $self->groups->first;
}

sub group { 
	my $self = shift;
	return $self->groups->first;
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::JudgeSetting->search(  
		judge => $self->id,
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

