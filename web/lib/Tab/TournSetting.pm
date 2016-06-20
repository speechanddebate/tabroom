package Tab::TournSetting;
use base 'Tab::DBI';
Tab::TournSetting->table('tourn_setting');
Tab::TournSetting->columns(All => qw/id tourn tag value value_date value_text setting timestamp/);
Tab::TournSetting->has_a(tourn => 'Tab::Tourn');
Tab::TournSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

