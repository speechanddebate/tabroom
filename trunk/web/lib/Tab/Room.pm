package Tab::Room;
use base 'Tab::DBI';
Tab::Room->table('room');
Tab::Room->columns(Primary => qw/id/);
Tab::Room->columns(Essential => qw/name inactive quality site/);
Tab::Room->columns(Others => qw/timestamp capacity notes/);
Tab::Room->has_a(site => 'Tab::Site');
Tab::Room->has_many(blocks => 'Tab::RoomBlock', 'room');
Tab::Room->has_many(panels => 'Tab::Panel', 'room');

Tab::Room->set_sql(tourn_rooms => "select distinct room.id 
								from room,tournament_site
								where room.site = tournament_site.site 
								and tournament_site.tournament = ?");		

Tab::Room->set_sql(by_event_pool => "select distinct room.id
										from room,room_pool
										where room.id = room_pool.room
										and room_pool.event = ?");

Tab::Room->set_sql(clean_rooms_by_round => "
            select distinct room.* from room,round,tournament,timeslot
            where room.site = ?
            and round.id = ?
			and round.site = room.site
            and tournament.id = timeslot.tournament
            and round.timeslot = timeslot.id
            and not exists (
                select p2.id from panel as p2,round as r2,timeslot as t2
                where t2.start < timeslot.end
                and t2.end > timeslot.start
                and p2.room = room.id
                and p2.round = r2.id
                and r2.timeslot  = t2.id )
            and not exists (
                select rb.id from roomblock as rb,timeslot as tb
                where rb.room = room.id
                and tb.id = rb.timeslot
                and tb.start < timeslot.end
                and tb.end > timeslot.start  )
			and not exists (
				select rb2.id from roomblock as rb2
				where rb2.room = room.id
				and rb2.type = \"special\")
			order by room.name ");

Tab::Room->set_sql(clean_rooms_by_timeslot => "
            select distinct room.* from room,tournament,timeslot
			where timeslot.id = ? 	
            and room.site = ?
            and tournament.id = timeslot.tournament
            and not exists (
                select p2.id from panel as p2,round as r2,timeslot as t2
                where t2.start < timeslot.end
                and t2.end > timeslot.start
                and p2.room = room.id
                and p2.round = r2.id
                and r2.timeslot  = t2.id )
            and not exists (
                select rb.id from roomblock as rb,timeslot as tb
                where rb.room = room.id
                and tb.id = rb.timeslot
                and tb.start < timeslot.end
                and tb.end > timeslot.start  )
			and not exists (
				select rb2.id from roomblock as rb2
				where rb2.room = room.id
				and rb2.type = \"special\")
			order by room.name ");

sub event_pools {
	my $self = shift;
	return Tab::Event->search_by_room_pool($self->id);
}

sub reserved { 
    my $self = shift;
	return Tab::Event->search_by_reserved($self->id);
}

sub reserved_to {
	my ($self, $event) = @_;
	my @pools = Tab::RoomPool->search(	room => $self->id, 
										event => $event->id);
	return 1 if @pools;
	return;
}

sub exclusive_to {
	my ($self, $event) = @_;
	my @pools = Tab::RoomPool->search(	room => $self->id, 
										reserved => 1,
										event => $event->id );
	return 1 if @pools;
	return;
}


