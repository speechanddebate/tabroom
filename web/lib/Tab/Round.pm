package Tab::Round;
use base 'Tab::DBI';
Tab::Round->table('round');
Tab::Round->columns(Primary => qw/id/);
Tab::Round->columns(Essential => qw/name label event type timeslot site pool/);
Tab::Round->columns(Others => qw/post_results published listed created completed 
									blasted timestamp tb_set motion judges cat_id 
									flighted start_time note/);
Tab::Round->columns(TEMP => qw/speaks/);

Tab::Round->has_a(event => 'Tab::Event');
Tab::Round->has_a(site => 'Tab::Site');
Tab::Round->has_a(pool => 'Tab::Pool');
Tab::Round->has_a(timeslot => 'Tab::Timeslot');
Tab::Round->has_a(tb_set => 'Tab::TiebreakSet');

Tab::Round->has_many(panels => 'Tab::Panel', 'round' => { order_by => 'letter'} );
Tab::Round->has_many(results => 'Tab::Result', 'round');

__PACKAGE__->_register_datetimes( qw/blasted/);
__PACKAGE__->_register_datetimes( qw/created/);
__PACKAGE__->_register_datetimes( qw/completed/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/start_time/);

sub realname { 
	my $self = shift;
	return $self->label if $self->label && $self->label ne $self->name;
	return "Round ".$self->name;
}

sub shortname { 
	my $self = shift;
	return $self->label if $self->label && $self->label ne $self->name;
	return "Rd ".$self->name;
}


