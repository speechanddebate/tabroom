package Tab::SweepRule;
use base 'Tab::DBI';
Tab::SweepRule->table('sweep_rule');
Tab::SweepRule->columns(All => qw/id sweep_set tag value timestamp/);
Tab::SweepRule->has_a(sweep_set => 'Tab::SweepSet');
__PACKAGE__->_register_datetimes( qw/timestamp/);

