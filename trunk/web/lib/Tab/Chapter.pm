package Tab::Chapter;
use base 'Tab::DBI';
Tab::Chapter->table('chapter');
Tab::Chapter->columns(Primary => qw/id/);
Tab::Chapter->columns(Essential => qw/name country state city timestamp coaches self_prefs district_id level naudl ipeds nces nsda/);
Tab::Chapter->columns(TEMP => qw/count prefs code member schoolid/);

Tab::Chapter->has_many(schools => 'Tab::School', 'chapter');
Tab::Chapter->has_many(students => 'Tab::Student', 'chapter');
Tab::Chapter->has_many(chapter_judges => 'Tab::ChapterJudge', 'chapter');
Tab::Chapter->has_many(chapter_circuits => 'Tab::ChapterCircuit', 'chapter');

Tab::Chapter->has_many(admins => [ Tab::Permission => 'account']);
Tab::Chapter->has_many(permissions => 'Tab::Permission', 'chapter');

Tab::Chapter->has_many(circuits => [ Tab::ChapterCircuit => 'circuit']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub location { 
	my $self = shift;
	my $location = $self->state."/" if $self->state;
	return $location.$self->country;
}

sub dues {
    my ($self, $circuit, $paid) = @_;

	if ($paid) { 
		my $school_year = &Tab::school_year;
	    my @dues = Tab::CircuitDues->search_where( chapter => $self->id, circuit => $circuit->id, paid_on => {">=", $school_year});
		my $total = 0;
		foreach (@dues) { $total += $_->amount; }
		return $total; 
	} else { 
	    return Tab::CircuitDues->search( chapter => $self->id, circuit => $circuit->id );
	}
}

sub full_member {
    my ($self, $circuit) = @_;
	my @membership = Tab::ChapterCircuit->search( chapter => $self->id, circuit => $circuit->id );
    return $membership[0]->full_member if @membership;
    return;
}

sub circuit_code {
    my ($self, $circuit, $code) = @_;
    my @membership = Tab::ChapterCircuit->search( chapter => $self->id, circuit => $circuit->id );

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
    my @membership = Tab::ChapterCircuit->search( chapter => $self->id, circuit => $circuit->id );
    return $membership[0]->region if @membership;
    return;
}

sub circuit_membership {
    my ($self, $circuit) = @_;
    my @membership = Tab::ChapterCircuit->search( chapter => $self->id, circuit => $circuit->id );
    return $membership[0] if @membership;
    return;
}

sub short_name {
	my ($self, $limit) = @_;
	my $name = $self->name;
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

