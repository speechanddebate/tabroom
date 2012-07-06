package Tab::Chapter;
use base 'Tab::DBI';
Tab::Chapter->table('chapter');
Tab::Chapter->columns(Primary => qw/id/);
Tab::Chapter->columns(Essential => qw/name country state timestamp coaches/);
Tab::Chapter->columns(TEMP => qw/count/);

Tab::Chapter->has_many(schools => 'Tab::School', 'chapter');
Tab::Chapter->has_many(students => 'Tab::Student', 'chapter');
Tab::Chapter->has_many(chapter_judges => 'Tab::ChapterJudge', 'chapter');
Tab::Chapter->has_many(chapter_circuits => 'Tab::ChapterCircuit', 'chapter');

Tab::Chapter->set_sql(by_tourn => "select distinct chapter.* from chapter, school
						where chapter.id = school.chapter
						and school.tourn = ?");

Tab::Chapter->set_sql(by_admin => "
						select distinct chapter.*
						from chapter,chapter_admin
						where chapter.id = chapter_admin.chapter 
						and chapter_admin.account = ?");

sub circuits {
    my $self = shift;
    return sort {$a->name cmp $b->name } Tab::Circuit->search_by_chapter($self->id);
}

Tab::Circuit->set_sql(by_chapter => "	
					select distinct circuit.* 
					from circuit,chapter_circuit
					where circuit.id = chapter_circuit.circuit
					and chapter_circuit.chapter = ? 
					order by circuit.name ");

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

sub admins {
    my $self = shift;
    return Tab::Account->search_by_chapter_admin($self->id);
}

sub full_member {
    my ($self, $circuit) = @_;
	my @membership = Tab::ChapterCircuit->search( chapter => $self->id, circuit => $circuit->id );
    return $membership[0]->full_member if @membership;
    return;
}

sub code {
    my ($self, $circuit) = @_;
    my @membership = Tab::ChapterCircuit->search( chapter => $self->id, circuit => $circuit->id );
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

