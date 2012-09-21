package Tab::SweepSet;
use base 'Tab::DBI';
Tab::SweepSet->table('sweep_set');
Tab::SweepSet->columns(All => qw/id tourn name timestamp/);
Tab::SweepSet->has_a(tourn => 'Tab::Tourn');
Tab::SweepSet->has_many(sweep_rules => 'Tab::SweepRule', 'sweep_set');
Tab::SweepSet->has_many(sweep_events => 'Tab::SweepEvent', 'sweep_set');
Tab::SweepSet->has_many(events => [Tab::SweepEvent => 'event']);
__PACKAGE__->_register_datetimes( qw/timestamp/);
