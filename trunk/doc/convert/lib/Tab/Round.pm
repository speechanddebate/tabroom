package Tab::Round;
use base 'Tab::DBI';
Tab::Round->table('round');
Tab::Round->columns(Primary => qw/id/);
Tab::Round->columns(Essential => qw/name event timeslot type label site pool preset/);
Tab::Round->columns(Others => qw/published listed created completed no_first_year score/);

__PACKAGE__->_register_dates( qw/created/);
__PACKAGE__->_register_dates( qw/completed/);

Tab::Round->has_a(event => 'Tab::Event');
Tab::Round->has_a(site => 'Tab::Site');
Tab::Round->has_a(pool => 'Tab::Pool');
Tab::Round->has_a(timeslot => 'Tab::Timeslot');
Tab::Round->has_many(panels => 'Tab::Panel', 'round');
Tab::Round->set_sql(high_round => "SELECT MAX(name) FROM __TABLE__ WHERE event = ?");

sub realname { 
	my $self = shift;
	return $self->label if $self->label;
	return "Round ".$self->name;
}

sub empties { 
	my $self = shift;
	return Tab::Ballot->search_empty_by_round($self->id);
}


sub unbalanced { 
	my $self = shift;
	return unless $self->panels;
	my $smallest_panel = $self->smallest_panel;
	my $largest_panel = $self->largest_panel;
	return 1 if ($largest_panel - $smallest_panel > 1);
	return;
}

sub smallest_panel {
	my $self = shift;
	return Tab::Round->sql_small_panel->select_val($self->id);
}

Tab::Round->set_sql(small_panel => "select min(number) from (
			select count(distinct comp.id) as number
			from ballot,panel,comp
		   	where panel.round = ?
			and ballot.panel = panel.id
			and ballot.comp = comp.id
			and comp.dropped != 1
			and panel.type=\"prelim\"
			group by panel.id ) as panel_numbers");

sub largest_panel {
	my $self = shift;
	return Tab::Round->sql_large_panel->select_val($self->id);
}

Tab::Round->set_sql(large_panel => "select max(number) from (
			select count(distinct comp.id) as number
			from ballot,panel,comp
			where panel.round = ?
			and ballot.panel = panel.id
			and ballot.comp = comp.id
			and comp.dropped != 1
            and panel.type=\"prelim\"
            group by panel.id ) as panel_numbers");

sub rooms { 

	my $self = shift; 

	my @raw_rooms =  Tab::Room->search_clean_rooms_by_round(	$self->site->id, 
																$self->id);
	my @rooms;
	my %room_events;
	my %reservations;
	my %exclusives;

	ROOM:
	foreach my $room (@raw_rooms) { 

		my $count;
		foreach my $event ($room->event_pools) { 
			$count++;
			$room_events{$room->id."-".$event->id}++;
		}

		$reservations{$room->id} = $count;

		my $reserved;

		my @exclusive_events = $room->reserved($self->event->tournament);

		foreach my $ev (@exclusive_events) { 
			$reserved++;
			$exclusives{$room->id}++ if $room_events{$room->id."-".$self->event->id};
		}

		#put all rooms in except the rooms reserved only to other events
		push (@rooms, $room) unless ($reserved && $exclusives{$room->id} < 1);

	}

	# De-prefer bad quality rooms 
	@rooms = sort{$a->quality <=> $b->quality} @rooms;

	# De-prefer rooms pooled to many other events
	@rooms = sort{$reservations{$a} <=> $reservations{$b}} @rooms;

	# Prefer rooms that are pooled to this event
	@rooms = sort{$room_events{$b->id."-".$self->event->id} <=> $room_events{$a->id."-".$self->event->id}} @rooms;

	# Prefer rooms reserved to this event
	@rooms = sort{$exclusives{$b->id} <=> $exclusives{$a}} @rooms;

}

sub comps { 
	my $self = shift;
	return Tab::Comp->search_by_round($self->id);
}

sub judges { 
	my $self = shift;
	return Tab::Judge->search_by_round($self->id);
}

sub debate_judges { 
	my $self = shift;
	return Tab::Judge->search_by_debate_round($self->id);
}

sub ballots { 
	my $self = shift;
	return Tab::Ballot->search_by_round($self->id);
}

Tab::Round->set_sql(by_timeslot_and_event => "select distinct * from round
											where timeslot = ?
											and event = ?  ");

Tab::Round->set_sql(by_site_and_tourn => "select distinct round.* 
											from round,event
											where round.site = ? 
											and round.event = event.id
											and event.tournament = ?");

Tab::Round->set_sql(latest_by_comp => "select distinct round.* 
											from round,panel,ballot
											where round.id = panel.round
											and panel.id = ballot.panel
											and ballot.comp = ? 
											order by round.name DESC limit 1");

Tab::Round->set_sql(with_judges => "select distinct round.* 
								from round
								where round.event = ? 
								and exists ( select judge.id 
								from judge,panel,ballot
								where judge.id = ballot.judge
								and ballot.panel = panel.id
								and panel.round = round.id)
								");

Tab::Round->set_sql(with_comps => "select distinct round.* 
								from round
								where round.event = ? 
								and exists ( select comp.id 
								from comp,panel,ballot
								where comp.id = ballot.comp
								and ballot.panel = panel.id
								and panel.round = round.id)
								");

Tab::Round->set_sql(finals_by_comp => "select distinct round.*
								from round,ballot,panel,comp
								where comp.id = ballot.comp
								and round.id = panel.round
								and comp.id = ? 
								and ballot.panel = panel.id
								and panel.type = \"final\" ");

Tab::Round->set_sql(elims_by_comp => "select distinct round.*
								from round,ballot,panel,comp
								where comp.id = ballot.comp
								and round.id = panel.round
								and comp.id = ? 
								and ballot.panel = panel.id
								and panel.type = \"elim\" ");

Tab::Round->set_sql(by_tournament => "select distinct round.*
								from round, event
								where round.event = event.id
								and event.tournament = ? ");

Tab::Round->set_sql(prelims_by_tournament => "select distinct round.*
								from round, event
								where round.event = event.id
								and round.type = \"prelim\"
								and event.tournament = ? ");

Tab::Round->set_sql(published_by_tournament => "select distinct round.*
								from round, event
								where round.event = event.id
								and round.published = \"1\"
								and event.tournament = ? ");

Tab::Round->set_sql(listed_by_tournament => "select distinct round.*
								from round, event
								where round.event = event.id
								and round.listed = \"1\"
								and event.tournament = ? ");


Tab::Round->set_sql(max_round => "select distinct round.id
									from round
									where round.event= ? 
									and round.preset != 1
									and round.type =?
									order by round.name DESC
									limit 1");
