package Tab::Judge;
use base 'Tab::DBI';
Tab::Judge->table('judge');
Tab::Judge->columns(Primary => qw/id/);
Tab::Judge->columns(Essential => qw/account chapter_judge school judge_group 
									first last code active obligation hired 
									covers special notes timestamp /);

Tab::Judge->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Judge->has_a(covers => 'Tab::JudgeGroup');

Tab::Judge->has_many(strikes => 'Tab::Strike', 'judge');
Tab::Judge->has_many(ballots => 'Tab::Ballot', 'judge');
Tab::Judge->has_many(settings => "Tab::JudgeSetting", "judge");

Tab::Judge->set_sql(highest_code => "select MAX(code) from judge where judge_group = ?");
Tab::Judge->set_sql(lowest_code => "select MIN(code) from judge where judge_group = ?");

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

sub name { 
	my $self = shift;
	return $self->first." ".$self->last;
}

sub region { 
	my $self = shift;
	return $self->school->region;
}

sub housing { 
	my ($self,$day) = @_;
	my @housings = Tab::Housing->search( judge => $self->id, night => $day->ymd) if $day;
	@housings = Tab::Housing->search( judge => $self->id) unless $day;
	return shift @housings;
}

sub pools {
	my $self = shift;
	return Tab::Pool->search_by_judge($self->id);
}

sub panels {
    my $self = shift;
    return Tab::Panel->search_by_judge($self->id);
}

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

		} elsif ($value ne "delete" && $value ne "" && $value != 0) {

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
