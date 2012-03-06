package Tab::Event;
use base 'Tab::DBI';
Tab::Event->table('event');
Tab::Event->columns(Primary => qw/id/);
Tab::Event->columns(Essential => qw/name abbr tourn team judge_group/);
Tab::Event->columns(Others =>  qw/
				class
				min_entry
				max_entry
				timestamp
				code 
				type
				supp
				cap 
				school_cap 
				blurb 
				deadline 
				fee 
				ballot 
				alumni 
				field_report
				ballot_type 
				allow_judge_own 
				waitlist 
				waitlist_all
				no_codes
				reg_codes
				initial_codes
				bids
				qual_subset
				no_judge_burden 
				live_updates
				omit_sweeps
				ask_for_titles /);

__PACKAGE__->_register_datetimes( qw/deadline/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::Event->has_a(tourn => 'Tab::Tourn');
Tab::Event->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Event->has_a(class => 'Tab::Class');
Tab::Event->has_a(qual_subset => 'Tab::QualSubset');
Tab::Event->has_many(entrys => 'Tab::Entry', 'event' => { order_by => 'code'} );
Tab::Event->has_many(rounds => 'Tab::Round', 'event' => { order_by => 'name'}  );
Tab::Event->has_many(results => 'Tab::StudentResult', 'event' );
Tab::Event->has_many(room_pools => 'Tab::RoomPool', 'event');
Tab::Event->has_many(panels => 'Tab::Panel', 'event'  => { order_by => 'letter'} );

Tab::Event->set_sql(reported_by_tourn => " 
							select distinct event.* from event
								where event.tourn = ? 
								and event.field_report = 1
								order by event.name");

Tab::Event->set_sql(liveupdates_by_tourn => " 
							select distinct event.* from event
								where event.tourn = ? 
								and event.live_updates = 1
								order by event.name");

Tab::Event->set_sql(by_pool => "select distinct round.event as id
									from round
									where round.pool = ? ");


Tab::Event->set_sql(by_site_and_tourn => "select distinct event.* 
                                            from round,event
                                            where round.site = ? 
                                            and round.event = event.id
                                            and event.tourn = ?");

Tab::Event->set_sql(by_room_pool => "select distinct event.* 
										from event,room_pool
										where room_pool.event = event.id
										and room_pool.room = ?");

Tab::Event->set_sql(by_group => "select distinct event.* from event where event.judge_group = ?");

Tab::Event->set_sql(blockable_by_group => "select distinct event.* 
										from event
										where event.judge_group = ? 
										and event.reg_blockable = 1");

Tab::Event->set_sql(by_region_and_tourn => "
						select distinct event.* from event,entry,school
						where event.id = entry.event 
						and entry.school = school.id
						and entry.dropped != 1
						and school.region = ?
						and school.tourn = ? ");

Tab::Event->set_sql(by_school => "select distinct event.* from event,entry
						where event.id = entry.event 
						and entry.school = ? ");

Tab::Event->set_sql(with_published => "select distinct event.* from event, round
						where event.tourn = ? 
						and event.round = round.id
						and round.published = 1");

Tab::Event->set_sql(ok_to_break => "    
    select distinct event.* from event
    where event.tourn = ?
	and event.type = \"speech\"
	and exists (
		select p2.id from panel as p2,round as r2
		where p2.round = r2.id
		and r2.event = event.id )
    and not exists (
    select ballot.id from ballot,entry,panel,round
        where panel.event = event.id
		and panel.round = round.id
		and round.preset != 1 
        and ballot.panel = panel.id
		and ballot.entry = entry.id
		and entry.dropped = 0
        and ballot.audit = 0
		)
    ");

sub sites {
	my $self = shift;
	return Tab::Site->search_by_event($self->id);
}

sub room_pool { 
	my $self = shift;
	return	Tab::Room->search_by_event_pool($self->id);
}

sub largest_panel {

	my $self = shift;

	Tab::Event->set_sql(large_panel => "select max(number) from (
            select count(distinct entry.id) as number
            from ballot,panel,entry
           	where panel.event = ".$self->id."
            and ballot.panel = panel.id
            and ballot.entry = entry.id
            and entry.dropped != 1
            and panel.type=\"prelim\"
            group by panel.id ) as panel_numbers");

	return Tab::Event->sql_large_panel->select_val;

}

sub count_active_entrys { 
	my $self = shift;
	return Tab::Entry->sql_count_active_by_event->select_val($self->id);
}

sub active_entrys { 
	my $self = shift;
	return Tab::Entry->search_active_by_event($self->id);
}

sub count_waitlist_entrys { 
	my $self = shift;
	return Tab::Entry->sql_count_waitlist_by_event->select_val($self->id);
}

sub waitlist_entrys { 
	my $self = shift;
	return Tab::Entry->search_waitlist_by_event($self->id);
}

sub entrys_by_timeslot_and_order { 
	my ($self, $timeslot,$order) = @_;
	return Tab::Entry->search_by_event_order_and_timeslot($self->id, $order, $timeslot->id);
}

sub smallest_panel {

	my $self = shift;

	Tab::Event->set_sql(small_panel => "select min(number) from (
            select count(distinct entry.id) as number
            from ballot,panel,entry
           	where panel.event = ".$self->id."
            and ballot.panel = panel.id
            and ballot.entry = entry.id
            and entry.dropped != 1
            and panel.type=\"prelim\"
            group by panel.id ) as panel_numbers");
	return Tab::Event->sql_small_panel->select_val;

}

Tab::Event->set_sql(head_count => "
		select count(distinct entry.id)
		from entry
		where entry.event = ?
		and entry.dropped != 1
		and entry.waitlist != 1
		and entry.dq != 1");

sub count { 
	my $self = shift;
	return Tab::Event->sql_head_count->select_val($self->id);
}

sub judges {
    my $self = shift;
    my @judges = Tab::Judge->search_has_judged($self->id);
    return @judges;
}

sub schools {
    my $self = shift;
    return Tab::School->search_by_event($self->id);
}
sub dioceses {
	my $self = shift;
    return Tab::Region->search_by_event($self->id);
}
sub letters {
    my $self = shift;
    return Tab::Panel->search_letters($self->id);
}

sub prelims {
    my $self = shift;
    return Tab::Panel->search_prelims($self->id);
}

sub next_code {

    my $self = shift;
    my @existing_entrys = Tab::Entry->search( event => $self->id, {order_by => "code DESC"} );
	my $code = $self->code; 
	
	while (defined $self->tourn->entry_with_code($code)) {
		$code++;
		$code++ if $code == 666;
		$code++ if $code == 69;
	}

	return $code;

}

sub last_non_preset { 
	my $self = shift;
	Tab::Round->set_sql(hi_round => "select max(name) from round where preset != 1 and event = ".$self->id);
	return Tab::Round->sql_hi_round($self->id)->select_val;
}

sub last_round { 
	my $self = shift;
	Tab::Round->set_sql(hi_round => "select max(name) from round where event = ".$self->id);
	return Tab::Round->sql_hi_round($self->id)->select_val;
}

sub honorable_mentions { 

	my $self = shift;

	Tab::Entry->set_sql(hon_mens => "
		select entry.id,entry.code from entry 
		where entry.event = ?  	
		and entry.id not in  	 	
		(select cdb.id  		
			from entry as cdb, ballot as bdb, panel as pdb 		
			where pdb.type = \"final\" 	    
			and pdb.id = bdb.panel 	    
			and bdb.entry = cdb.id 		
			and cdb.event = ? 		
		)   	
		and     	
		(select sum(bc.rank)  		
			from ballot as bc,panel as pc 		
			where bc.entry = entry.id 		
			and bc.panel = pc.id 		
			and pc.type = \"prelim\" 	
		) = 
		(select max(cume) from  	  		
			(select sum(b1.rank) as cume 			
			from ballot as b1,entry as c1,panel as p1
			where b1.entry = c1.id 	 			
			and b1.panel = p1.id 	 			
			and c1.event = ? 			
			and p1.type = \"prelim\" 	 			
			and c1.id in 
				(select cb.id  				
				from entry as cb, ballot as bb, panel as pb 				
				where pb.type = \"final\" 				
				and pb.id = bb.panel 				
				and bb.entry = cb.id ) 			
			group by c1.id) 
		as cume_score
		)
	");

	return Tab::Entry->search_hon_mens($self->id,$self->id,$self->id);

}

sub finalists { 
	my $self = shift;
	return Tab::Entry->search_finals_by_event($self->id);
}

sub ballots { 
	my $self = shift;
	return Tab::Ballot->search_ballots_by_event($self->id);
}

sub prelim_ballots { 
	my $self = shift;
	return Tab::Ballot->search_prelim_ballots_by_event($self->id);
}

sub students { 
	my $self = shift;
	return if $self->team == 3;
	my @students;
	push (@students, Tab::Student->search_event_students( $self->id ));
	push (@students, Tab::Student->search_event_partners( $self->id )) if $self->team == 2;
	return @students;
}

sub partners { 
	my $self = shift;
	return if $self->team == 3;
	return (@students, Tab::Student->search_event_partners( $self->id ));
}

sub students_only { 
	my $self = shift;
	return if $self->team == 3;
	return (@students, Tab::Student->search_event_students( $self->id ));
}

sub strike {
	my ($self, $judge) = @_;
	my @cons = Tab::Strike->search(	
				event => $self->id, 
				judge => $judge->id );
	return shift @cons;
}

sub panels_by_timeslot {
	my ($self, $timeslot) = @_;
	return Tab::Panel->search_by_event_and_timeslot($self->id, $timeslot->id);
}

sub waitlist_schools {
    my $self = shift;
    return Tab::School->search_by_waitlist_event($self->id);
}

Tab::Event->set_sql(by_reserved => " select distinct event.id from event, room_pool
			where room_pool.reserved = 1
			and room_pool.event = event.id
			and room_pool.room = ?  ");

sub setting {

	my ($self, $tag, $value, $text) = @_;

	my @existing = Tab::EventSetting->search(  
		event => $self->id,
		tag => $tag
	);

    if ($value &! $value == 0) {

		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->text($text);
			$exists->update;

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} else {

			Tab::EventSetting->create({
				event => $self->id,
				tag => $tag,
				value => $value,
				text => $text
			});

		}

	} else {

		return unless @existing;

		my $setting = shift @existing;

		foreach my $other (@existing) { 
			$other->delete;
		}

		return $setting->text if $setting->value eq "text";
		return $setting->value;

	}

}
