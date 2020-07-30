package Tab::Autoqueue;
use base 'Tab::DBI';
Tab::Autoqueue->table('autoqueue');
Tab::Autoqueue->columns(Primary => qw/id/);
Tab::Autoqueue->columns(Essential => qw/tag event round active_at message timestamp created_by created_at/);
Tab::Autoqueue->has_a(round => 'Tab::Round');
Tab::Autoqueue->has_a(event => 'Tab::Event');
Tab::Autoqueue->has_a(created_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp created_at active_at/);
