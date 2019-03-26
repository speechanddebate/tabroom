package Tab::SweepRule;
use base 'Tab::DBI';
Tab::SweepRule->table('sweep_rule');

Tab::SweepRule->columns(All => qw/
		id sweep_set tag 
		value place count 
		tiebreak_set timestamp
	/);

Tab::SweepRule->has_a(sweep_set => 'Tab::SweepSet');
Tab::SweepRule->has_a(tiebreak_set => 'Tab::TiebreakSet');
__PACKAGE__->_register_datetimes( qw/timestamp/);

