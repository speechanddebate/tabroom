package Tab::AccountAccess;
use base 'Tab::DBI';
Tab::AccountAccess->table('account_access');
Tab::AccountAccess->columns(Primary => qw/id/);
Tab::AccountAccess->columns(Essential => qw/tournament account timestamp contact nosetup entry/);
Tab::AccountAccess->has_a(tournament => 'Tab::Tournament');
Tab::AccountAccess->has_a(account => 'Tab::Account');

