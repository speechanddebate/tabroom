package Tab::Region;
use base 'Tab::DBI';
Tab::Region->table('region');
Tab::Region->columns(Primary => qw/id/);
Tab::Region->columns(Essential => qw/circuit name code timestamp/);
Tab::Region->columns(Others => qw/diocese quota arch sweeps cooke_pts tourn/);
Tab::Region->columns(TEMP => qw/registered unregistered/);

Tab::Region->has_a(circuit => 'Tab::Circuit');
Tab::Region->has_a(tourn => 'Tab::Tourn');

Tab::Region->has_many(schools => 'Tab::School', 'region');
Tab::Region->has_many(fines => 'Tab::RegionFine', 'region');
Tab::Region->has_many(permissions => 'Tab::Permission', 'region');
Tab::Region->has_many(admins => [ Tab::Permission => 'account']);
Tab::Region->has_many(chapters => [ Tab::ChapterCircuit => 'chapter']);


sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = ""; # Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::RegionSetting->search(  
		region => $self->id,
		tag    => $tag,
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

			my $existing = Tab::RegionSetting->create({
				region => $self->id,
				tag    => $tag,
				value  => $value,
			});

			if ($value eq "region") { 
				$existing->event($blob);
			}

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

