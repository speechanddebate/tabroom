package Tab::SweepAwardEvent;
use base 'Tab::DBI';
Tab::SweepAwardEvent->table('sweep_award_event');
Tab::SweepAwardEvent->columns(All => qw/id name abbr level sweep_set timestamp/);
Tab::SweepAwardEvent->has_a(sweep_set => 'Tab::SweepSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);

