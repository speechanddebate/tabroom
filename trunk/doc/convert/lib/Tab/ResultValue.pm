package Tab::ResultValue;
use base 'Tab::DBI';
Tab::ResultValue->table('result_value');
Tab::ResultValue->columns(Primary => qw/id/);
Tab::ResultValue->columns(Essential => qw/result label value priority sort_desc timestamp /);
Tab::ResultValue->has_a(result => 'Tab::Result');

__PACKAGE__->_register_dates( qw/timestamp/);

