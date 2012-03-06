
package Tab::LeagueAdmin;
use base 'Tab::DBI';
Tab::LeagueAdmin->table('league_admin');
Tab::LeagueAdmin->columns(All => qw/id league timestamp account/);
Tab::LeagueAdmin->has_a(league => "Tab::League");
Tab::LeagueAdmin->has_a(account => "Tab::Account");

