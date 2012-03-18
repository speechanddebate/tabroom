
package Tab::Coach;
use base 'Tab::DBI';
Tab::Coach->table('coach');
Tab::Coach->columns(Primary => qw/id/);
Tab::Coach->columns(Essential => qw/name chapter timestamp/);
Tab::Coach->has_a(chapter => 'Tab::Chapter');

