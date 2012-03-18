package Tab::NoInterest;
use base 'Tab::DBI';
Tab::NoInterest->table('no_interest');
Tab::NoInterest->columns(Primary => qw/id/);
Tab::NoInterest->columns(Essential => qw/account tournament/);
Tab::NoInterest->has_a(account => "Tab::Account");
Tab::NoInterest->has_a(tournament => "Tab::Tournament");
