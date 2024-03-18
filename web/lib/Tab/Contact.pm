package Tab::Contact;
use base 'Tab::DBI';
Tab::Contact->table('contact');
Tab::Contact->columns(All => qw/id school person official onsite email blast nsda book timestamp created_at created_by/);

Tab::Contact->has_a(school     => 'Tab::School');
Tab::Contact->has_a(person     => 'Tab::Person');
Tab::Contact->has_a(created_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp created_at/);
