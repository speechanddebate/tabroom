package Tab::Round;
use base 'Tab::DBI';
Tab::Round->table('round');
Tab::Round->columns(Primary => qw/id/);
Tab::Round->columns(Essential => qw/type name label flighted published post_results created start_time 
							 		event timeslot site tiebreak_set timestamp/);
Tab::Round->columns(TEMP => qw/speaks/);

Tab::Round->has_a(site => 'Tab::Site');
Tab::Round->has_a(event => 'Tab::Event');
Tab::Round->has_a(tiebreak_set => 'Tab::TiebreakSet');
Tab::Round->has_a(timeslot => 'Tab::Timeslot');

Tab::Round->has_many(jpools => [Tab::JPoolRound => 'jpool']);
Tab::Round->has_many(rpools => [Tab::RPoolRound => 'rpool']);
Tab::Round->has_many(settings => 'Tab::RoundSetting', 'round');

Tab::Round->has_many(panels => 'Tab::Panel', 'round' => { order_by => 'letter'} );
Tab::Round->has_many(results => 'Tab::Result', 'round');

__PACKAGE__->_register_datetimes( qw/created start_time timestamp/);

sub realname { 
	my $self = shift;
	return $self->label if $self->label && $self->label ne $self->name;
	return "Round ".$self->name;
}

sub shortname { 
	my $self = shift;
	return $self->label if $self->label && $self->label ne $self->name;
	return "Rd ".$self->name;
}


sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = ""; # Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::RoundSetting->search(  
		round => $self->id,
		tag   => $tag,
	)->first;

	if (defined $value) { 
			
		if ($existing) {

			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$existing->delete;
			} else { 
				$existing->value($value);
				$existing->value_text($blob) if $value eq "text";
				$existing->value_date($blob) if $value eq "date";
				$existing->update;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::RoundSetting->create({
				round => $self->id,
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

