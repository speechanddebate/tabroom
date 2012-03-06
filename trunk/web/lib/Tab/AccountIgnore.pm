package Tab::AccountIgnore;
use base 'Tab::DBI';
Tab::AccountIgnore->table('account_ignore');
Tab::AccountIgnore->columns(Primary => qw/id/);
Tab::AccountIgnore->columns(Essential => qw/account tourn/);
Tab::AccountIgnore->has_a(account => "Tab::Account");
Tab::AccountIgnore->has_a(tourn => "Tab::Tourn");
