package Tab::TournAdmin;
use base 'Tab::DBI';
Tab::TournAdmin->table('tourn_admin');
Tab::TournAdmin->columns(Primary => qw/id/);
Tab::TournAdmin->columns(Essential => qw/tourn account timestamp contact no_setup entry_only event/);
Tab::TournAdmin->has_a(tourn => 'Tab::Tourn');
Tab::TournAdmin->has_a(account => 'Tab::Account');

