package Tab::Region;
use base 'Tab::DBI';
Tab::Region->table('region');
Tab::Region->columns(Primary => qw/id/);
Tab::Region->columns(Essential => qw/circuit name code tourn timestamp/);
Tab::Region->columns(TEMP => qw/registered unregistered/);

Tab::Region->has_a(circuit => 'Tab::Circuit');
Tab::Region->has_a(tourn => 'Tab::Tourn');

Tab::Region->has_many(schools     => 'Tab::School', 'region');
Tab::Region->has_many(fines       => 'Tab::RegionFine', 'region');
Tab::Region->has_many(permissions => 'Tab::Permission', 'region');

Tab::Region->has_many(admins   => [ Tab::Permission     => 'person']);
Tab::Region->has_many(persons  => [ Tab::Permission     => 'person']);
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

	my %all_settings;

	my $dbh = Tab::DBI->db_Main();

    my $sth = $dbh->prepare("
		select setting.tag, setting.value, setting.value_date, setting.value_text
		from region_setting setting
		where setting.region = ? 
        order by setting.tag
    ");
    
    $sth->execute($self->id);
    
    while( my ($tag, $value, $value_date, $value_text)  = $sth->fetchrow_array() ) { 

		if ($value eq "date") { 

			my $dt = Tab::DBI::dateparse($value_date); 
			$all_settings{$tag} = $dt if $dt;

		} elsif ($value eq "text") { 

			$all_settings{$tag} = $value_text;

		} else { 

			$all_settings{$tag} = $value;

		}

	}

	return %all_settings;

}

