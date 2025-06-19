package Tab::Email;
use base 'Tab::DBI';
Tab::Email->table('email');
Tab::Email->columns(Primary => qw/id/);
Tab::Email->columns(Essential => qw/sender person sender_raw content metadata subject sent_at sent_to hidden circuit tourn timestamp/);
Tab::Email->has_a(sender => 'Tab::Person');
Tab::Email->has_a(person => 'Tab::Person');
Tab::Email->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/sent_at timestamp/);

