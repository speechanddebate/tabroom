package Tab::Hotel;
use base 'Tab::DBI';
Tab::Hotel->table('hotel');
Tab::Hotel->columns(Primary => "id");
Tab::Hotel->columns(Essential => qw/tourn name multiple timestamp/);
Tab::Hotel->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/timestamp/);

