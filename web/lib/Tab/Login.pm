package Tab::Login;
use base 'Tab::DBI';
Tab::Login->table('login');
Tab::Login->columns(Primary => qw/id/);
Tab::Login->columns(Essential => qw/username password salt name person/);
Tab::Login->columns(Others => qw/accesses last_access pass_changekey pass_timestamp pass_change_expires source timestamp/);

Tab::Login->has_a(person => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/last_access pass_change_expires timestamp/);


