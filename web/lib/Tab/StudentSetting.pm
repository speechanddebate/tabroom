package Tab::StudentSetting;
use base 'Tab::DBI';
Tab::StudentSetting->table('student_setting');
Tab::StudentSetting->columns(All => qw/id student tag value value_date value_text setting timestamp/);
Tab::StudentSetting->has_a(student => 'Tab::Student');
Tab::StudentSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp value_date/);

