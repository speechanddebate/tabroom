package Tab::Concession;
use base 'Tab::DBI';
Tab::Concession->table('concession');
Tab::Concession->columns(Primary => qw/id/);
Tab::Concession->columns(Essential => qw/name price tourn deadline/);
Tab::Concession->columns(Other => qw/description timestamp/);

Tab::Concession->has_a(tourn => 'Tab::Tourn');
Tab::Concession->has_a(event => 'Tab::Event');

Tab::Concession->has_many(concession_purchases => 'Tab::ConcessionPurchase', 'concession');
__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/deadline/);
