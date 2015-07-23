package Tab::SchoolSetting;
use base 'Tab::DBI';
Tab::SchoolSetting->table('school_setting');
Tab::SchoolSetting->columns(All => qw/id school tag value value_date value_text timestamp/);
Tab::SchoolSetting->has_a(school => 'Tab::School');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

