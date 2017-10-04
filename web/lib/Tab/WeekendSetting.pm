package Tab::WeekendSetting;
use base 'Tab::DBI';
Tab::WeekendSetting->table('weekend_setting');
Tab::WeekendSetting->columns(All => qw/id weekend tag value value_date value_text setting timestamp/);
Tab::WeekendSetting->has_a(weekend => 'Tab::Weekend');
Tab::WeekendSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

