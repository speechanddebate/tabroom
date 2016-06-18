package Tab::TournIgnore;
use base 'Tab::DBI';
Tab::TournIgnore->table('tourn_ignore');
Tab::TournIgnore->columns(Primary => qw/id/);
Tab::TournIgnore->columns(Essential => qw/person tourn timestamp/);
Tab::TournIgnore->has_a(person => "Tab::Tourn");
Tab::TournIgnore->has_a(tourn => "Tab::Tourn");

__PACKAGE__->_register_datetimes( qw/timestamp/);
