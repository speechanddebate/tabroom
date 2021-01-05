package Tab::Login;
use base 'Tab::DBI';
Tab::Login->table('login');
Tab::Login->columns(Primary => qw/id/);
Tab::Login->columns(Essential => qw/username sha512 person accesses last_access/);
Tab::Login->columns(Others => qw/pass_changekey pass_timestamp pass_change_expires timestamp/);

Tab::Login->has_a(person => 'Tab::Person');

sub account {
	my $login = shift;
	return $login->person;
}

__PACKAGE__->_register_datetimes( qw/last_access pass_change_expires timestamp/);


