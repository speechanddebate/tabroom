package Tab::Chapter;
use base 'Tab::DBI';
Tab::Chapter->table('chapter');
Tab::Chapter->columns(Primary => qw/id/);
Tab::Chapter->columns(Essential => qw/name street city state zip postal country/);
Tab::Chapter->columns(Others => qw/coaches self_prefs level naudl ipeds nces ceeb 
									nsda district timestamp/);
Tab::Chapter->columns(TEMP => qw/count prefs code member schoolid/);

Tab::Chapter->has_a(district => 'Tab::District');

Tab::Chapter->has_many(schools => 'Tab::School', 'chapter');
Tab::Chapter->has_many(students => 'Tab::Student', 'chapter');
Tab::Chapter->has_many(chapter_judges => 'Tab::ChapterJudge', 'chapter');
Tab::Chapter->has_many(chapter_circuits => 'Tab::ChapterCircuit', 'chapter');
Tab::Chapter->has_many(chapter_settings => 'Tab::ChapterSetting', 'chapter');

Tab::Chapter->has_many(admins => [ Tab::Permission => 'person']);
Tab::Chapter->has_many(persons => [ Tab::Permission => 'person']);
Tab::Chapter->has_many(circuits => [ Tab::ChapterCircuit => 'circuit']);
Tab::Chapter->has_many(permissions => 'Tab::Permission', 'chapter');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub location { 
	my $self = shift;
	my $location = $self->state."/" if $self->state;
	return $location.$self->country;
}

sub full_member {
    my ($self, $circuit) = @_;
	my @membership = Tab::ChapterCircuit->search( 
		chapter => $self->id, 
		circuit => $circuit->id 
	);

    return $membership[0]->full_member if @membership;
    return;
}

sub circuit_code {
    my ($self, $circuit, $code) = @_;
    my @membership = Tab::ChapterCircuit->search( 
		chapter => $self->id,
		circuit => $circuit->id
	);

	if ($code) { 
		$membership[0]->code($code);
		$membership[0]->update;
		return;
	}

    return $membership[0]->code if @membership;
    return;
}

sub region {
    my ($self, $circuit) = @_;
    my @membership = Tab::ChapterCircuit->search( 
		chapter => $self->id, 
		circuit => $circuit->id
	);
    return $membership[0]->region if @membership;
    return;
}

sub circuit_membership {
    my ($self, $circuit) = @_;
    my @membership = Tab::ChapterCircuit->search( 
		chapter => $self->id, 
		circuit => $circuit->id 
	);
    return $membership[0] if @membership;
    return;
}

sub short_name {

	my ($self, $limit) = @_;
	my $name = $self->name;

	$name = chomp $name;

	#screw these people.
	$name = "Thomas Jefferson" if $name eq "Thomas Jefferson High School of Science and Technology";
	$name = "Thomas Jefferson" if $name eq "Thomas Jefferson High School of Science & Technology";
	$name = "Bronx Science" if $name eq "The Bronx High School of Science";
	$name = "Whitney Young" if $name eq "Whitney M. Young Magnet High School";
	$name = "Lane Tech" if $name eq "Lane Tech College Prep H.S.";

	$name =~ s/College Prep H.S.//g;
	$name =~ s/College Prep HS//g;
	$name =~ s/College Prep High School//g;

	$name = "BC" if $name eq "Boston College";
	$name = "BU" if $name eq "Boston University";

	$name = "NYU" if $name eq "New York University";
	$name =~ s/of Math and Science$//g;
	$name =~ s/Academy$//g;
	$name =~ s/Regional\ High\ School$//g;
	$name =~ s/High\ School$//g;
	$name =~ s/Colleges$//g;
	$name =~ s/School$//g;
	$name =~ s/High$//g;
	$name =~ s/Preparatory$/Prep/g;
	$name =~ s/H.S.$//g;
	$name =~ s/HS.$//g;
	$name =~ s/HS$//g;
	$name =~ s/Regional$//g;
	$name =~ s/Public\ Charter//g;
	$name =~ s/Charter\ Public//g;
	$name =~ s/University\ of//g;
	$name =~ s/California State University,/CSU/g;
	$name =~ s/California State University/CSU/g;
	$name =~ s/California,/UC/g;
	$name =~ s/College\ Prep$/CP/g;
	$name = "College Prep" if $name eq "CP";  #Sometimes it's the whole school name.  Oops.
	$name =~ s/ CP //g;
	$name =~ s/University$//g;
	$name =~ s/College$//g;
	$name =~ s/State\ University,/State\ /g;
	$name =~ s/^The //g;
	$name =~ s/^Saint\ /St\ /g;
	$name =~ s/High\ School/HS/g;
	$name =~ s/ CP//g;
	$name = "Boston College" if $name eq "BC";
	$name = "Boston Uni" if $name eq "BU";
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

	my $existing = Tab::ChapterSetting->search(  
		chapter => $self->id,
		tag     => $tag,
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

			my $existing = Tab::ChapterSetting->create({
				chapter => $self->id,
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

