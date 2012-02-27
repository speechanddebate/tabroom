
package Tab::Flight;
use base 'Tab::DBI';
Tab::Flight->table('flight');
Tab::Flight->columns(Primary => qw/id/);
Tab::Flight->columns(Essential => qw/name tournament/);
Tab::Flight->has_many(events => 'Tab::Event', 'flight');
Tab::Flight->has_a(tournament => 'Tab::Tournament');

