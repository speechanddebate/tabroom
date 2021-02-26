package Tab::Result;
use base 'Tab::DBI';
Tab::Result->table('result');
Tab::Result->columns(Primary => qw/id/);
Tab::Result->columns(Essential => qw/rank place result_set entry student school round panel/);
Tab::Result->columns(Others => qw/timestamp percentile/);
Tab::Result->columns(TEMP => qw/value/);

Tab::Result->has_a(entry => 'Tab::Entry');
Tab::Result->has_a(round => 'Tab::Round');
Tab::Result->has_a(student => 'Tab::Student');
Tab::Result->has_a(result_set => 'Tab::ResultSet');

Tab::Result->has_many(result_values => 'Tab::ResultValue', 'result');
Tab::Result->has_many(values => 'Tab::ResultValue', 'result');

__PACKAGE__->_register_datetimes( qw/timestamp/);

