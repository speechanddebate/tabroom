package Tab::ResultKey;
use base 'Tab::DBI';
Tab::ResultKey->table('result_key');
Tab::ResultKey->columns(Primary => qw/id/);
Tab::ResultKey->columns(Essential => qw/id result_set tag description no_sort sort_desc timestamp/);

Tab::ResultKey->has_a(result_set => 'Tab::ResultSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);

