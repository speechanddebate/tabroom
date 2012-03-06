package Tab::AccountAccess;
use base 'Tab::DBI';
Tab::AccountAccess->table('tourn_admin');
Tab::AccountAccess->columns(Primary => qw/id/);
Tab::AccountAccess->columns(Essential => qw/tourn account timestamp contact nosetup entry/);
Tab::AccountAccess->has_a(tourn => 'Tab::Tourn');
Tab::AccountAccess->has_a(account => 'Tab::Account');

