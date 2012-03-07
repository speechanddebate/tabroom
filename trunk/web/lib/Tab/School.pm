package Tab::School;
use base 'Tab::DBI';
Tab::School->table('school');
Tab::School->columns(Essential => qw/id tourn chapter name region code/);

Tab::School->columns(Others => qw/sweeps_points timestamp score 
								disclaimed registered paid_amount contact_name
								contact_number concession_paid_amount entered
								registered_on noprefs/);

Tab::School->has_a(chapter => 'Tab::Chapter');
Tab::School->has_a(region => 'Tab::Region');
Tab::School->has_a(disclaimed => 'Tab::Account');
Tab::School->has_a(tourn => 'Tab::Tourn');
Tab::School->has_many(purchases => 'Tab::ConcessionPurchase', 'school');
Tab::School->has_many(entrys => 'Tab::Entry', 'school');
Tab::School->has_many(judges => 'Tab::Judge', 'school');
Tab::School->has_many(fines => 'Tab::SchoolFine', 'school');
Tab::School->has_many(hires => 'Tab::JudgeHire', 'school');
Tab::School->has_many(files => 'Tab::File', 'school');

__PACKAGE__->_register_datetimes( qw/entered/);
__PACKAGE__->_register_datetimes( qw/registered_on/);


Tab::School->set_sql(wipe_prefs => " delete from rating where school = ?");



Tab::School->set_sql(schools => "select distinct school.id 
		from school,entry where entry.event= ? and entry.school = school.id" );

Tab::School->set_sql(speech_schools_by_tourn => "select distinct school.*
							from school
							where school.tourn = ?
							and exists (
								select entry.id
								from entry, event
								where entry.school = school.id
								and entry.event = event.id
								and event.type = \"speech\" 
								limit 1) ");

Tab::School->set_sql(debate_congress_schools_by_tourn => "select distinct school.*
							from school
							where school.tourn = ?
							and exists (
								select entry.id
								from entry, event
								where entry.school = school.id
								and entry.event = event.id
								and event.type != \"speech\" 
								limit 1) ");

Tab::School->set_sql(by_group => "select distinct school.id
							from school,entry,event,class
							where class.judge_group = ?
							and event.class = class.id
							and entry.event = event.id
							and entry.school = school.id");

Tab::School->set_sql(by_event => "select distinct school.id
							from school,entry
							where school.id = entry.school
							and entry.dropped != 1
							and entry.event = ? ");

Tab::School->set_sql(by_waitlist_event => "select distinct school.id
							from school,entry
							where school.id = entry.school
							and entry.dropped != 1
							and entry.waitlist = 1
							and entry.event = ? ");

Tab::School->set_sql(members_by_tourn => "select distinct school.id
							from school, chapter, chapter_circuit,tourn
                        	where tourn.id = ?
							and school.tourn = tourn.id
							and school.chapter = chapter.id
							and chapter.id = chapter_circuit.chapter 
                        	and chapter_circuit.circuit = tourn.circuit
                        	and chapter_circuit.full_member = 1");

sub free_ubers { 
	my $self = shift;
	return Tab::Uber->search_free_by_school($self->id);
}

sub is_conflicted  { 

	my ($self, $judge) = @_;

	my @strikes = Tab::Strike->search(	
					school => $self->id,
					judge => $judge->id,
					strike => 0 );

	my $strike = shift @strikes;

	foreach my $spare (@strikes) { 
		$spare->delete;
	}

	return $strike;

}

sub conflicts { 

	my ($self, $group) = @_;

	my $tourn =  $self->tourn;

	my @strikes = Tab::Strike->search(	
					school => $self->id,
					strike => 0 );

	return @strikes unless $group;

	# If a group is specified, comb out only those judges in the group.  This
	# is better done in sql but in practice it's so lightly used it's not worth
	# the effort.

	my @filtered_strikes;

	foreach $strike (@strikes) {
		next unless $strike->judge->judge_group->id == $group->id;
		push (@filtered_strikes, $strike);
	}

	return @filtered_strikes;
}

sub strikes { 

	my ($self, $group) = @_;

	my $tourn =  $self->tourn;

	my @strikes = Tab::Strike->search(	
					school => $self->id,
					strike => 1 );

	return @strikes unless $group;

	# If a group is specified, comb out only those judges in the group.  This
	# is better done in sql but in practice it's so lightly used it's not worth
	# the effort.

	my @filtered_strikes;

	foreach $strike (@strikes) {
		next unless $strike->judge->judge_group->id == $group->id;
		push (@filtered_strikes, $strike);
	}

	return @filtered_strikes;
}

sub chapter_region { 

	my ($self,$region) = @_;

	if ($region) { 
		my $circuit = $self->tourn->circuit;

		my @circuit_mems = Tab::ChapterCircuit->search( chapter => $self->chapter->id,
						  								circuit => $circuit->id);

		my $circuit_mem = shift @circuit_mems if @circuit_mems;

		if ($circuit_mem) { 
			$circuit_mem->region($region->id);
			$circuit_mem->update;
		}

		return;

	} else { 
		my $circuit = $self->tourn->circuit;
		my @regs = Tab::Region->search_by_school_circuit($self->id, $circuit->id);
		my $region = shift @regs;
		return $region;
	}
}

sub requests_by_group { 

	my ($self, $group, $requested) = @_;
	my @requests = Tab::JudgeHire->search( school => $self->id, judge_group => $group->id );
	my $request = shift @requests; 
	foreach my $req (@requests) { $_->delete if $_; }  # Kill the spare, Wormtail.

	return $request;

}


Tab::School->set_sql(highest_code => "select MAX(code) from school where tourn = ?");

sub housing_reqs { 
	my $self = shift;
	my @reqs = Tab::Housing->search_by_school($self->id);
	push (@reqs, Tab::Housing->search_judge_by_school($self->id));
	return @reqs;
}

sub housing { 
	my $self = shift;
	my @housed = Tab::Student->search_housed_by_school($self->id);
	push (@housed, Tab::Student->search_housed_partners_by_school($self->id));
	push (@housed, Tab::Judge->search_housed($self->id));
	return @housed;
}

sub truncate_name {
	my $self = shift;
	my $name = $self->name;
	$name =~ s/Academy$//g;
	$name =~ s/High\ School$//g;
	$name =~ s/School$//g;
	$name =~ s/High$//g;
	$name =~ s/Preparatory$/Prep/g;
	$name =~ s/College\ Prep$/CP/g;
	$name =~ s/HS$//g;
	$name =~ s/Regional$//g;
	$name =~ s/^The//g;
	$name =~ s/^Saint/St./g;
	$name = "College Prep" if $name eq "CP";
	$name =~ s/High\ School/HS/g;
	$name =~ s/\s+$//;
	return $name;
}

sub short_name {
	my $self = shift;
	my $name = $self->name;
	$name =~ s/Academy$//g;
	$name =~ s/High\ School$//g;
	$name =~ s/School$//g;
	$name =~ s/High$//g;
	$name =~ s/Preparatory$/Prep/g;
	$name =~ s/College\ Prep$/CP/g;
	$name =~ s/HS$//g;
	$name =~ s/Regional$//g;
	$name =~ s/^The//g;
	$name =~ s/^Saint/St./g;
	$name = "College Prep" if $name eq "CP";
	$name =~ s/High\ School/HS/g;
	$name =~ s/\s+$//;
	$name =~ s/^\s+//;
	return substr ($name, 0, 15);
}

sub event_count{
    my ($self, $event_id) = @_;
    my @count = Tab::Entry->search( school => $self->id, event => $event_id, waitlist => 0);
    return scalar @count;
}

sub group_entries { 
    my ($self, $group) = @_;
    return Tab::Entry->search_by_group_school($self->id, $group->id,);
}


sub finalists{ 
	my $self = shift;
	my $tourn = $self->tourn; 	
	my @events = $tourn->events;
	my @finalists = Tab::Entry->search_finals_by_school($self->id);
	return @finalists;
}

sub events { 
	my $self = shift;
	return Tab::Event->search_by_school($self->id);
}

sub strike_time_school_search {
	my ($self,$group,$strike_time) = @_;
	return Tab::Judge->search_by_school_and_strike_time($self->id, $group->id, $strike_time->id);
}

# Relating to concessions


sub concession_count { 
	my ($self, $concession) = @_;
	my @purchases = Tab::ConcessionPurchase->search({ school => $self->id, concession => $concession->id });
	my $quantity;
	foreach (@purchases) {  $quantity += $_->quantity; };
	return $quantity;
}

sub concession_cost { 
	my ($self, $concession) = @_;
	my $quantity = $self->concession_count($concession);
	my $cost = $quantity * $concession->price;
	return $cost;
}

sub concession_total { 
	
	my $self = shift; 
	my $tourn = $self->tourn;
	my $total;

	foreach my $concession ($tourn->concessions)  {
		$total += $self->concession_cost($concession);
	}

	return $total;
}

sub students { 
	
	my $self = shift;
	my @students;

	push (@students, Tab::Student->search_by_school($self->id));
	push (@students, Tab::Student->search_team_members_by_school($self->id));

	#uniq
	my %seen = ();
	@students = grep { ! $seen{$_->id} ++ } @students;

	return @students;
}

