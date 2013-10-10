package Tab::ResultValue;
use base 'Tab::DBI';
Tab::ResultValue->table('result_value');
Tab::ResultValue->columns(Primary => qw/id/);
Tab::ResultValue->columns(Essential => qw/result tag value priority sort_desc no_sort/);
Tab::ResultValue->columns(Others => qw/long_tag timestamp /);
Tab::ResultValue->has_a(result => 'Tab::Result');

__PACKAGE__->_register_datetimes( qw/timestamp/);

