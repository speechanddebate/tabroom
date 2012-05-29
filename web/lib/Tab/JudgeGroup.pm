package Tab::JudgeGroup;
use base 'Tab::DBI';
Tab::JudgeGroup->table('judge_group');
Tab::JudgeGroup->columns(Primary => qw/id/);
Tab::JudgeGroup->columns(Essential => qw/tourn name abbr judge_per deadline timestamp/);

Tab::JudgeGroup->has_a(tourn => "Tab::Tourn");

Tab::JudgeGroup->has_many(pools => "Tab::Pool", "judge_group");
Tab::JudgeGroup->has_many(judges => "Tab::Judge", "judge_group");
Tab::JudgeGroup->has_many(events => "Tab::Judge", "judge_group");
Tab::JudgeGroup->has_many(hires => 'Tab::JudgeHire', 'judge_group');
Tab::JudgeGroup->has_many(strike_times => "Tab::StrikeTime", "judge_group");
Tab::JudgeGroup->has_many(event_doubles => "Tab::EventDouble", "judge_group");
Tab::JudgeGroup->has_many(settings => "Tab::JudgeGroupSetting", "judge_group");
Tab::JudgeGroup->has_many(rating_subsets => "Tab::RatingSubset", "judge_group");

__PACKAGE__->_register_datetimes( qw/deadline timestamp/);

sub next_code { 
	
    my $self = shift;

	my %judges_by_code = ();
	foreach my $judge ($self->tourn->judges) { 
		$judges_by_code{$judge->code}++;
	}

    my $code = 100;

    while (defined $judges_by_code{$code}) { 
        $code++;
        $code++ if $code == 666;
        $code++ if $code == 69;
    }

    return $code;
}


sub entries {
	my $self = shift;
	return Tab::Entry->search_by_group($self->id);
}

sub schools { 
	my $self = shift; 
	return Tab::School->search_by_group($self->id);
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	my @existing = Tab::JudgeGroupSetting->search(  
		judge_group => $self->id,
		tag         => $tag
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

			my $exists = Tab::JudgeGroupSetting->create({
				judge_group => $self->id,
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
