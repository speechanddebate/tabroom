package Tab::PanelSetting;
use base 'Tab::DBI';
Tab::PanelSetting->table('panel_setting');
Tab::PanelSetting->columns(All => qw/id panel tag value value_date value_text setting timestamp/);
Tab::PanelSetting->has_a(panel => 'Tab::Panel');
Tab::PanelSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

