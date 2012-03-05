package Tab::TournSetting;
use base 'Tab::DBI';
Tab::TournSetting->table('tourn_setting');
Tab::TournSetting->columns(All => qw/id tourn tag value text timestamp/);
Tab::TournSetting->has_a(tourn => 'Tab::Tournament');

__PACKAGE__->_register_datetimes( qw/timestamp/);

