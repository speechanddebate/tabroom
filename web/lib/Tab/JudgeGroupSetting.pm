package Tab::JudgeGroupSetting;
use base 'Tab::DBI';
Tab::JudgeGroupSetting->table('judge_group_setting');
Tab::JudgeGroupSetting->columns(All => qw/id judge_group tag value value_date text timestamp/);
Tab::JudgeGroupSetting->has_a(judge_group => 'Tab::JudgeGroup');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);
