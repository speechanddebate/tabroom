package Tab::SweepAward;
use base 'Tab::DBI';
Tab::SweepAward->table('sweep_award');
Tab::SweepAward->columns(All => qw/id name description circuit target count min_entries min_schools period timestamp/);

Tab::SweepAward->has_a(circuit => 'Tab::Circuit');

Tab::SweepAward->has_many(sweep_sets => 'Tab::SweepSet', 'sweep_award');
Tab::SweepAward->has_many(result_sets => 'Tab::ResultSet', 'sweep_award');

__PACKAGE__->_register_datetimes( qw/timestamp/);

