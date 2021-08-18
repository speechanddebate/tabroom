package Tab::TabroomSetting;
use base 'Tab::DBI';
Tab::TabroomSetting->table('tabroom_setting');
Tab::TabroomSetting->columns(All => qw/id tag value value_date value_text person timestamp/);

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

Tab::TabroomSetting->has_a(person => 'Tab::Person');

