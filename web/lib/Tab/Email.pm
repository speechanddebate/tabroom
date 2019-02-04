package Tab::Email;
use base 'Tab::DBI';
Tab::Email->table('email');
Tab::Email->columns(Primary => qw/id/);
Tab::Email->columns(Essential => qw/sender content metadata subject sent_at circuit sent_to tourn timestamp/);
Tab::Email->has_a(sender => 'Tab::Person');
Tab::Email->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/sent_at timestamp/);

