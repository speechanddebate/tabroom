package Tab::Region;
use base 'Tab::DBI';
Tab::Region->table('region');
Tab::Region->columns(Primary => qw/id/);
Tab::Region->columns(Essential => qw/name code/);
Tab::Region->columns(Others => qw/director timestamp tourn diocese 
								  quota arch league sweeps cooke_pts/);
Tab::Region->has_a(director => 'Tab::Account');
Tab::Region->has_a(league => 'Tab::League');
Tab::Region->has_many(schools => 'Tab::School', 'region');
Tab::Region->has_many(fines => 'Tab::Fine', 'region');

Tab::Region->set_sql(by_tourn => "select distinct region.*
						from region,school
						where school.tourn = ? 
						and school.region = region.id
						");

Tab::Region->set_sql(by_school_league => "select distinct region.* 
							from region,chapter_league,school
							where school.id = ? 
							and school.chapter = chapter_league.chapter
							and chapter_league.league = ?
							and chapter_league.region = region.id ");

Tab::Region->set_sql(by_event => "select distinct region.* 
							from region,school,entry
							where entry.event = ? 
							and entry.school = school.id
							and region.id = school.region
							");

Tab::Region->set_sql(by_admin => "select distinct region.*
						from region,region_admin
						where region_admin.account = ? 
						and region.admin.region = region.id "); 

sub paid { 

	my ($self, $tourn) = @_;

	my $paid;

	#Payments should always be negative.
	foreach my $school ($self->schools($tourn)) { 
		$paid -= $school->paid_amount;
	}

	foreach my $fine (Tab::Fine->search( region => $self->id, reason => "Payment" )) { 
		$paid += $fine->amount;
	}

	return $paid;

}

sub events {
	my ($self, $tourn) = @_;
	return Tab::Event->search_by_region_and_tourn($self->id, $tourn->id);
}

sub event_entrys {
	my ($self, $event) = @_;
	my @entrys = sort {$a->id <=> $b->id} Tab::Entry->search_by_region_and_event($self->id, $event->id);
	return @entrys;
}

sub active_event_entrys {
	my ($self, $event) = @_;
	return Tab::Entry->search_active_by_region_and_event($self->id, $event->id);
}

sub judges {
    my ($self,$tourn) = @_;
	return Tab::Judge->search_by_region($self->id, $tourn->id);
}


sub group_coverage { 

    my ($self,$group) = @_;
	@judges = $self->judges_by_group;
	@covers = $self->covers_by_group;

	my @keepers;

	foreach my $judge (@judges) { 
		
		push (@keepers, $judge) unless $judge->covers && $judge->covers->id != $group->id;

	}

	push (@keepers, @covers);

    my %seen = ();
	@keepers = grep { ! $seen{$_->id} ++ } @keepers;

	return @keepers;
}

sub judges_by_group {
    my ($self,$group) = @_;
	return Tab::Judge->search_by_region_and_group($self->id, $group->id);
} 


sub covers_by_group { 
    my ($self,$group) = @_;
	return Tab::Judge->search_covers_by_region_and_group($self->id, $group->id);
}

sub judges_by_alt_group {
    my ($self,$group) = @_;
	return Tab::Judge->search_by_region_and_alt_group($self->id, $group->id);
}

sub elim_pool_rounds { 
	my ($self,$group) = @_;
	return Tab::PoolJudge->search_elim_by_group_and_diocese($group->id, $self->id);
}

sub entrys {
    my ($self,$tourn) = @_;
	return Tab::Entry->search_by_region($self->id, $tourn->id);	
}

sub active_entrys {
    my ($self,$tourn) = @_;
	return Tab::Entry->search_active_by_region($self->id, $tourn->id);	
}

sub entry_event_count { 
	my ($self,$event) = @_;
	Tab::Entry->set_sql(region_event_count => "select count(distinct entry.id) from entry,school
										where entry.school = school.id
										and entry.event = ?
										and school.region = ?");
	return Tab::Entry->sql_region_event_count->select_val($event->id, $self->id);
}

sub entry_count { 
	my ($self,$tourn) = @_;
	Tab::Entry->set_sql(region_count => "select count(distinct entry.id) from entry,school
										where entry.school = school.id
										and school.tourn = ?
										and school.region = ?");
	return Tab::Entry->sql_region_count->select_val($tourn->id, $self->id);
}


sub judge_group_count { 
	my ($self,$group) = @_;
	Tab::Judge->set_sql(region_group_count => "select count(distinct judge.id) from judge,school
										where judge.school = school.id
										and judge.judge_group = ?
										and school.region = ?");
	return Tab::Judge->sql_region_group_count->select_val($group->id, $self->id);
}

sub judge_count { 
	my ($self,$tourn) = @_;
	Tab::Judge->set_sql(region_count => "select count(distinct judge.id) from judge,school
										where judge.school = school.id
										and school.tourn = ?
										and school.region = ?");
	return Tab::Judge->sql_region_count->select_val($tourn->id, $self->id);
}


sub group_entrys {
    my ($self,$group) = @_;
	return Tab::Entry->search_by_region_and_group($self->id, $group->id);	
}

sub chapters {
    my $self = shift;
    my @memberships = Tab::ChapterLeague->search( region => $self->id );
    my @members;
    foreach my $ms (@memberships) {
        push (@members, $ms->chapter);
    }
    return @members;
}
