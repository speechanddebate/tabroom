package Tab::School;
use base 'Tab::DBI';
Tab::School->table('school');
Tab::School->columns(Essential => qw/id tourn name chapter region code contact/);
Tab::School->columns(Others => qw/registered timestamp/);

Tab::School->has_a(tourn => 'Tab::Tourn');
Tab::School->has_a(chapter => 'Tab::Chapter');
Tab::School->has_a(region => 'Tab::Region');

Tab::School->has_many(purchases => 'Tab::ConcessionPurchase', 'school');
Tab::School->has_many(entries => 'Tab::Entry', 'school');
Tab::School->has_many(judges => 'Tab::Judge', 'school');
Tab::School->has_many(fines => 'Tab::SchoolFine', 'school');
Tab::School->has_many(hires => 'Tab::JudgeHire', 'school');
Tab::School->has_many(files => 'Tab::File', 'school');
Tab::School->has_many(followers => [Tab::Follower => 'follower']);

__PACKAGE__->_register_datetimes( qw/timestamp/);


sub events { 
	my $self = shift;
	return Tab::Event->search_by_school($self->id);
}

sub students { 
	my $self = shift;
	return Tab::Student->search_by_school($self->id);
}

sub short_name {
	my ($self, $limit) = @_;
	my $name = $self->name;
	$name = "NYU" if $name eq "New York University";
	$name =~ s/of Math and Science$//g;
	$name =~ s/Academy$//g;
	$name =~ s/Regional\ High\ School$//g;
	$name =~ s/High\ School$//g;
	$name =~ s/Colleges$//g;
	$name =~ s/School$//g;
	$name =~ s/High$//g;
	$name =~ s/Preparatory$/Prep/g;
	$name =~ s/College\ Prep$/CP/g;
	$name =~ s/HS$//g;
	$name =~ s/Regional$//g;
	$name =~ s/Public\ Charter//g;
	$name =~ s/Charter\ Public//g;
	$name =~ s/University\ of//g;
	$name =~ s/California State University,/CSU/g;
	$name =~ s/California State University/CSU/g;
	$name =~ s/University$//g;
	$name =~ s/State\ University,/State\ /g;
	$name =~ s/^The//g;
	$name =~ s/^Saint\ /St\ /g;
	$name = "College Prep" if $name eq "CP";  #Sometimes it's the whole school name.  Oops.
	$name =~ s/High\ School/HS/g;
	$name =~ s/^\s+//;  #leading spaces
	$name =~ s/\s+$//;  #trailing spaces

    if ($limit) { 
        return substr($name,0,$limit);
    } else { 
    	return $name;
    }
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = ""; # Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::SchoolSetting->search(  
		school => $self->id,
		tag    => $tag,
	)->first;

	if ($value) { 
			
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

			my $existing = Tab::SchoolSetting->create({
				school => $self->id,
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

