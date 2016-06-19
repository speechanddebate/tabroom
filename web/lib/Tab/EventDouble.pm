package Tab::EventDouble;
use base 'Tab::DBI';
Tab::EventDouble->table('event_double');
Tab::EventDouble->columns(Primary =>qw/id/);
Tab::EventDouble->columns(Essential => qw/name tourn type max exclude/);
Tab::EventDouble->has_a(tourn => 'Tab::Tourn');
Tab::EventDouble->has_a(exclude => 'Tab::EventDouble');
Tab::EventDouble->has_many(events => "Tab::Event", 'event_double');
