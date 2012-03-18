package Tab::TournSite;
use base 'Tab::DBI';
	Tab::TournSite->table('tournament_site');
	Tab::TournSite->columns(All => qw/id timestamp tournament site/);
	Tab::TournSite->has_a(site => 'Tab::Site');
	Tab::TournSite->has_a(tournament => 'Tab::Tournament');
