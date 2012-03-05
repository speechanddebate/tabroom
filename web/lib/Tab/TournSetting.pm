package Tab::TournSetting;
use base 'Tab::DBI';
Tab::TournSetting->table('tourn_setting');
Tab::TournSetting->columns(All => qw/id tourn tag value text modified timestamp/);
Tab::TournSetting->has_a(tourn => 'Tab::Tournament');
Tab::TournSetting->has_a(modified => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/timestamp/);

