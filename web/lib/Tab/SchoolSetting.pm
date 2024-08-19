package Tab::SchoolSetting;
use base 'Tab::DBI';
Tab::SchoolSetting->table('school_setting');
Tab::SchoolSetting->columns(All => qw/id school tag value value_date value_text last_changed setting timestamp/);
Tab::SchoolSetting->has_a(school       => 'Tab::School');
Tab::SchoolSetting->has_a(setting      => 'Tab::Setting');
Tab::SchoolSetting->has_a(last_changed => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp value_date/);

