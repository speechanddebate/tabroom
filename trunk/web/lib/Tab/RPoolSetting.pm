package Tab::RPoolSetting;
use base 'Tab::DBI';
Tab::RPoolSetting->table('rpool_setting');
Tab::RPoolSetting->columns(All => qw/id rpool tag value value_date value_text setting timestamp/);
Tab::RPoolSetting->has_a(rpool => 'Tab::RPool');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

