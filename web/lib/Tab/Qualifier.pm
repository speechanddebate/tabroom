package Tab::Qualifier;
use base 'Tab::DBI';
Tab::Qualifier->table('qualifier');
Tab::Qualifier->columns(Primary => qw/id/);
Tab::Qualifier->columns(Essential => qw/entry name result tourn timestamp/);
Tab::Qualifier->has_a(tourn => 'Tab::Tournament');
Tab::Qualifier->has_a(entry => 'Tab::Comp');
