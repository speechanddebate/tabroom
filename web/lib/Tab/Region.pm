package Tab::Region;
use base 'Tab::DBI';
Tab::Region->table('region');
Tab::Region->columns(Primary => qw/id/);
Tab::Region->columns(Essential => qw/circuit name code timestamp/);
Tab::Region->columns(Others => qw/diocese quota arch sweeps cooke_pts/);

Tab::Region->has_a(circuit => 'Tab::Circuit');

Tab::Region->has_many(schools => 'Tab::School', 'region');
Tab::Region->has_many(fines => 'Tab::RegionFine', 'region');
Tab::Region->has_many(region_admins => 'Tab::RegionAdmin', 'region');

sub admins {
    my $self = shift;
    return Tab::Account->search_by_region_admin($self->id);
}

sub events {
	my ($self, $tourn) = @_;
	return Tab::Event->search_by_region_and_tourn($self->id, $tourn->id);
}

sub entries {
    my ($self,$tourn) = @_;
	return Tab::Entry->search_by_region_and_tourn($self->id, $tourn->id);	
}

sub judges {
    my ($self,$tourn) = @_;
	return Tab::Judge->search_by_region($self->id, $tourn->id);
}

sub event_entries {
	my ($self, $event) = @_;
	my @entries = sort {$a->id <=> $b->id} Tab::Entry->search_by_region_and_event($self->id, $event->id);
	return @entries;
}

sub group_entries {
    my ($self,$group) = @_;
	return Tab::Entry->search_by_region_and_group($self->id, $group->id);	
}

sub chapters {
    my $self = shift;
    return Tab::Chapter->search_by_region($self->id);
}

