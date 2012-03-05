package Tab::JudgeSetting;
use base 'Tab::DBI';
Tab::JudgeSetting->table('judge_setting');
Tab::JudgeSetting->columns(All => qw/id judge tag value text modified timestamp/);
Tab::JudgeSetting->has_a(judge => 'Tab::Judge');
Tab::JudgeSetting->has_a(modified => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/timestamp/);

