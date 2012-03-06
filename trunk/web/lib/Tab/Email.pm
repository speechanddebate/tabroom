package Tab::Email;
use base 'Tab::DBI';
Tab::Email->table('email');
Tab::Email->columns(Primary => qw/id/);
Tab::Email->columns(Essential => qw/timestamp subject league tourn senton sender sent_to text/);
Tab::Email->has_a(sender => 'Tab::Account');
Tab::Email->has_a(tourn => 'Tab::Tourn');
__PACKAGE__->_register_datetimes( qw/senton/);

