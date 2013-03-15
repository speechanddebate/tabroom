package Tab::Site;
use base 'Tab::DBI';
Tab::Site->table('site');
Tab::Site->columns(Primary => qw/id/);
Tab::Site->columns(Essential => qw/name circuit timestamp/);
Tab::Site->columns(Others => qw/host directions circuit dropoff/);

Tab::Site->has_a(host => 'Tab::Account');
Tab::Site->has_a(circuit => 'Tab::Circuit');
Tab::Site->has_many(rounds => 'Tab::Round', {order_by => "name"});
Tab::Site->has_many(rooms => 'Tab::Room', {order_by => "name"});

sub tourns {
    my $self = shift;
    return Tab::Tourn->search_by_site($self->id);
}

sub events { 
	my ($self,$tourn) = @_;
	return Tab::Event->search_by_site_and_tourn($self->id, $tourn->id);
}

sub panels { 
	my ($self,$tourn) = @_;
	return Tab::Panel->search_by_site_and_tourn($self->id, $tourn->id);
}

