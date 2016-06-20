package Tab::CategorySetting;
use base 'Tab::DBI';
Tab::CategorySetting->table('category_setting');
Tab::CategorySetting->columns(All => qw/id category tag value value_date value_text setting timestamp/);

Tab::CategorySetting->has_a(category => 'Tab::Category');
Tab::CategorySetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);
