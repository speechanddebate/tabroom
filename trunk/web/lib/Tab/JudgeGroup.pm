package Tab::JudgeGroup;
use base 'Tab::DBI';
Tab::JudgeGroup->table('judge_group');
Tab::JudgeGroup->columns(Primary => qw/id/);
Tab::JudgeGroup->columns(Essential => qw/tourn name abbr timestamp/);

Tab::JudgeGroup->has_a(tourn => "Tab::Tourn");

Tab::JudgeGroup->has_many(pools => "Tab::Pool", "judge_group");
Tab::JudgeGroup->has_many(judges => "Tab::Judge", "judge_group" => { order_by => 'code'} );
Tab::JudgeGroup->has_many(events => "Tab::Event", "judge_group");
Tab::JudgeGroup->has_many(hires => 'Tab::JudgeHire', 'judge_group');
Tab::JudgeGroup->has_many(rating_tiers => "Tab::RatingTier", "judge_group");
Tab::JudgeGroup->has_many(strike_times => "Tab::StrikeTime", "judge_group");
Tab::JudgeGroup->has_many(settings => "Tab::Setting", "judge_group");
Tab::JudgeGroup->has_many(rating_subsets => "Tab::RatingSubset", "judge_group");

__PACKAGE__->_register_datetimes( qw/timestamp/);

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

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::Setting->search(  
		judge_group => $self->id,
		tag         => $tag,
		type        => "class"
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
				judge_group => $self->id,
				tag         => $tag,
				value       => $value,
				type        => "class"
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

