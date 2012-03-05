package Tab::JudgeGroupSetting;
use base 'Tab::DBI';
Tab::JudgeGroupSetting->table('judge_group_setting');
Tab::JudgeGroupSetting->columns(All => qw/id judge_group tag value text modified timestamp/);
Tab::JudgeGroupSetting->has_a(judge_group => 'Tab::JudgeGroup');
Tab::JudgeGroupSetting->has_a(modified => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/timestamp/);

