package Tab::SweepEvent;
use base 'Tab::DBI';
Tab::SweepEvent->table('sweep_event');
Tab::SweepEvent->columns(All => qw/id sweep_set event timestamp/);
Tab::SweepEvent->has_a(event => 'Tab::Event');
Tab::SweepEvent->has_a(sweep_set => 'Tab::SweepSet');
__PACKAGE__->_register_datetimes( qw/timestamp/);

