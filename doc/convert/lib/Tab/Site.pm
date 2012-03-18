
package Tab::Site;
use base 'Tab::DBI';
Tab::Site->table('site');
Tab::Site->columns(All => qw/id name host timestamp directions league dropoff/);
Tab::Site->has_a(host => 'Tab::Account');
Tab::Site->has_many(rooms => 'Tab::Room', {order_by => "quality"});
Tab::Site->has_a(league => 'Tab::League');

Tab::Site->set_sql(by_tournament => "
        select distinct site.id
        from site,tournament_site
        where site.id = tournament_site.site
        and tournament_site.tournament = ?" );

Tab::Site->set_sql(by_event => "
        select distinct site.id
        from site,round
        where site.id = round.site
		and round.event = ?");


sub tournaments {
    my $self = shift;
    return Tab::Tournament->search_by_site($self->id);
}

sub rounds { 
	my ($self,$tourn) = @_;
	return Tab::Round->search_by_site_and_tourn($self->id, $tourn->id);
}

sub events { 
	my ($self,$tourn) = @_;
	return Tab::Event->search_by_site_and_tourn($self->id, $tourn->id);
}

sub panels { 
	my ($self,$tourn) = @_;
	return Tab::Panel->search_by_site_and_tourn($self->id, $tourn->id);
}

sub timeslots_with_panels { 
	my ($self,$tourn) = @_;
	return Tab::Timeslot->search_with_panels_by_site_and_tourn($self->id, $tourn->id);
}
