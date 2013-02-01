package Tab::Result;
use base 'Tab::DBI';
Tab::Result->table('result');
Tab::Result->columns(Primary => qw/id/);
Tab::Result->columns(Essential => qw/result_set entry student school round timestamp honor/);

Tab::Result->has_a(entry => 'Tab::Entry');
Tab::Result->has_a(round => 'Tab::Round');
Tab::Result->has_a(student => 'Tab::Student');
Tab::Result->has_a(result_set => 'Tab::ResultSet');

Tab::Result->has_many(values => 'Tab::ResultValue', 'result');

__PACKAGE__->_register_dates( qw/timestamp/);

