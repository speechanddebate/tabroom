package Tab::Email;
use base 'Tab::DBI';
Tab::Email->table('email');
Tab::Email->columns(Primary => qw/id/);
Tab::Email->columns(Essential => qw/sender content subject sent_on circuit sent_to tourn timestamp/);
Tab::Email->has_a(sender => 'Tab::Account');
Tab::Email->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/sent_on timestamp/);

