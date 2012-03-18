package Tab::Result;
use base 'Tab::DBI';
Tab::Result->table('result');
Tab::Result->columns(Primary => qw/id/);
Tab::Result->columns(Essential => qw/entry sweeps bid label seed timestamp/);

Tab::Result->has_a(entry => 'Tab::Event');
Tab::Result->has_many(values => 'Tab::ResultValue', 'result');

__PACKAGE__->_register_dates( qw/timestamp/);

