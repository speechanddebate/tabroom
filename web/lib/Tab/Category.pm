package Tab::Category;
use base 'Tab::DBI';
Tab::Category->table('category');
Tab::Category->columns(Primary => qw/id/);
Tab::Category->columns(Essential => qw/tourn name abbr timestamp/);

Tab::Category->has_a(tourn => "Tab::Tourn");

Tab::Category->has_many(jpools => "Tab::JPool", "category");
Tab::Category->has_many(judges => "Tab::Judge", "category" => { order_by => 'code'} );
Tab::Category->has_many(events => "Tab::Event", "category");
Tab::Category->has_many(hires => 'Tab::JudgeHire', 'category');
Tab::Category->has_many(rating_tiers => "Tab::RatingTier", "category");
Tab::Category->has_many(strike_times => "Tab::StrikeTime", "category");
Tab::Category->has_many(settings => "Tab::CategorySetting", "category");
Tab::Category->has_many(rating_subsets => "Tab::RatingSubset", "category");

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
        $code++ if $code eq "666";
        $code++ if $code eq "69";
        $code++ if $code eq "420";
    }

    return $code;
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::CategorySetting->search(  
		category => $self->id,
		tag         => $tag,
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

			my $existing = Tab::CategorySetting->create({
				category => $self->id,
				tag         => $tag,
				value       => $value,
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

