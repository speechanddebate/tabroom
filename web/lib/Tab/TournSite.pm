package Tab::TournSite;
use base 'Tab::DBI';
	Tab::TournSite->table('tourn_site');
	Tab::TournSite->columns(All => qw/id timestamp tourn site/);
	Tab::TournSite->has_a(site => 'Tab::Site');
	Tab::TournSite->has_a(tourn => 'Tab::Tourn');
