package Tab::Judge;
use base 'Tab::DBI';
Tab::Judge->table('judge');
Tab::Judge->columns(Primary => qw/id/);
Tab::Judge->columns(Essential => qw/school first last code dropped active special notes judge_group
									covers chapter_judge obligation elim_group alt_group drop_time drop_by reg_time
									cfl_parl account acct_request hired timestamp tmp score/);
Tab::Judge->columns(TEMP => qw/panel chair/);

Tab::Judge->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Judge->has_a(alt_group => 'Tab::JudgeGroup');
Tab::Judge->has_a(covers => 'Tab::JudgeGroup');
Tab::Judge->has_a(school => 'Tab::School');
Tab::Judge->has_a(drop_by => 'Tab::Account');
Tab::Judge->has_a(account => 'Tab::Account');
Tab::Judge->has_a(acct_request => 'Tab::Account');
Tab::Judge->has_a(chapter_judge => 'Tab::ChapterJudge');

Tab::Judge->has_many(strikes => 'Tab::Strike', 'judge');
Tab::Judge->has_many(ballots => 'Tab::Ballot', 'judge');
Tab::Judge->has_many(settings => "Tab::JudgeSetting", "judge");

Tab::Judge->set_sql(highest_code => "select MAX(code) from judge where judge_group = ?");
Tab::Judge->set_sql(lowest_code => "select MIN(code) from judge where judge_group = ?");

__PACKAGE__->_register_datetimes( qw/drop_time/);
__PACKAGE__->_register_datetimes( qw/reg_time/);

Tab::Judge->set_sql( by_group => "
        		select distinct judge.*
            	from judge
            	where judge.judge_group = ?
				and judge.active = 1");

Tab::Judge->set_sql( hired_by_group => "
        		select distinct judge.*
            		from judge 
				where judge.judge_group = ?
				and spare_pool = 1");

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	my @existing = Tab::JudgeSetting->search(  
		judge => $self->id,
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

			my $exists = Tab::JudgeSetting->create({
				judge => $self->id,
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

sub print_ratings {

	my ($self, $subset) = @_;

	my @ratings = Tab::Rating->search( judge => $self->id, subset => $subset->id, type => "coach" ) if $subset;
	@ratings = Tab::Rating->search( judge => $self->id, type => "coach") unless $subset;

	my $string;

	foreach my $rating (sort {$a->id cmp $b->id} @ratings) { 
		$string .= " ".$rating->qual->name if $rating->qual;
	}

	return $string;
}

sub rating { 

	my ($self, $subset) = @_;

	if ($subset) { 
		my @ratings = Tab::Rating->search( judge => $self->id, subset => $subset->id, type => "coach" );
		return shift @ratings if @ratings;
		return;
	} else { 
		my @ratings = Tab::Rating->search( judge => $self->id, type => "coach");
		return shift @ratings if @ratings;
		return;
	} 

}

