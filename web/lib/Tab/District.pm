package Tab::District;
use base 'Tab::DBI';
Tab::District->table('district');
Tab::District->columns(Primary => qw/id/);
Tab::District->columns(Essential => qw/name code location chair timestamp/);
Tab::District->columns(Others => qw/level realm/);

Tab::District->has_many(chapters => 'Tab::Chapter', 'district');

Tab::District->has_many(permissions => 'Tab::Permission', 'district');
Tab::District->has_many(admins => [ Tab::Permission => 'person']);

