package Tab::TournSite;
use base 'Tab::DBI';
Tab::TournSite->table('tourn_site');
Tab::TournSite->columns(All => qw/id tourn site timestamp/);

Tab::TournSite->has_a(site => 'Tab::Site');
Tab::TournSite->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/timestamp/);

