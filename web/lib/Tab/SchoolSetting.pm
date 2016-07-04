package Tab::SchoolSetting;
use base 'Tab::DBI';
Tab::SchoolSetting->table('school_setting');
Tab::SchoolSetting->columns(All => qw/id school tag value value_date value_text setting timestamp/);
Tab::SchoolSetting->has_a(school => 'Tab::School');
Tab::SchoolSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp value_date/);

