package Tab::Pattern;
use base 'Tab::DBI';
Tab::Pattern->table('pattern');
Tab::Pattern->columns(Primary =>qw/id/);
Tab::Pattern->columns(Essential => qw/name tourn type max exclude/);
Tab::Pattern->has_a(tourn => 'Tab::Tourn');
Tab::Pattern->has_a(exclude => 'Tab::Pattern');
Tab::Pattern->has_many(events => "Tab::Event", 'pattern');
