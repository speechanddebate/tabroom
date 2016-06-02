package Tab::SettingLabel;
use base 'Tab::DBI';
Tab::SettingLabel->table('setting_label');
Tab::SettingLabel->columns(Primary => qw/id/); 
Tab::SettingLabel->columns(Essential => qw/lang label guide options setting/);
Tab::SettingLabel->columns(Others => qw/timestamp/);

Tab::SettingLabel->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);

