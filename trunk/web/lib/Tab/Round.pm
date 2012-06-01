package Tab::Round;
use base 'Tab::DBI';
Tab::Round->table('round');
Tab::Round->columns(Primary => qw/id/);
Tab::Round->columns(Essential => qw/name label event type timeslot site pool/);
Tab::Round->columns(Others => qw/published listed created entryleted timestamp tb_set/);

Tab::Round->has_a(event => 'Tab::Event');
Tab::Round->has_a(site => 'Tab::Site');
Tab::Round->has_a(pool => 'Tab::Pool');
Tab::Round->has_a(timeslot => 'Tab::Timeslot');
Tab::Round->has_a(tb_set => 'Tab::TiebreakSet');

Tab::Round->has_many(panels => 'Tab::Panel', 'round');

__PACKAGE__->_register_dates( qw/created/);
__PACKAGE__->_register_dates( qw/entryleted/);
__PACKAGE__->_register_dates( qw/timestamp/);

sub realname { 
	my $self = shift;
	return $self->label if $self->label;
	return "Round ".$self->name;
}

sub entries { 
	my $self = shift;
	return Tab::Entry->search_by_round($self->id);
}

sub judges { 
	my $self = shift;
	return Tab::Judge->search_by_round($self->id);
}

sub ballots { 
	my $self = shift;
	return Tab::Ballot->search_by_round($self->id);
}

