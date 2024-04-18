package Tab::ResultSet;
use base 'Tab::DBI';
Tab::ResultSet->table('result_set');
Tab::ResultSet->columns(Primary => qw/id/);
Tab::ResultSet->columns(Essential => qw/id tag label tourn event circuit qualifier sweep_set sweep_award
										bracket published coach code generated timestamp cache/);

Tab::ResultSet->has_a(tourn       => 'Tab::Tourn');
Tab::ResultSet->has_a(event       => 'Tab::Event');
Tab::ResultSet->has_a(circuit     => 'Tab::Circuit');
Tab::ResultSet->has_a(sweep_set   => 'Tab::SweepSet');
Tab::ResultSet->has_a(sweep_award => 'Tab::SweepAward');

Tab::ResultSet->has_many(results => 'Tab::Result', 'result_set', { order_by => 'rank, id'} );
Tab::ResultSet->has_many(result_keys => 'Tab::ResultKey', 'result_set');

__PACKAGE__->_register_datetimes( qw/timestamp generated/);

