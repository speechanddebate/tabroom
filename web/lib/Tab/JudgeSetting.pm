package Tab::JudgeSetting;
use base 'Tab::DBI';
Tab::JudgeSetting->table('judge_setting');
Tab::JudgeSetting->columns(All => qw/id judge tag value value_date value_text setting conditional timestamp/);
Tab::JudgeSetting->has_a(judge => 'Tab::Judge');
Tab::JudgeSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp value_date/);

