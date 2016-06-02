package Tab::JPoolSetting;
use base 'Tab::DBI';
Tab::JPoolSetting->table('jpool_setting');
Tab::JPoolSetting->columns(All => qw/id jpool tag value value_date value_text setting timestamp/);
Tab::JPoolSetting->has_a(jpool => 'Tab::JPool');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

