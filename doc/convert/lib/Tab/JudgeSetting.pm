package Tab::JudgeSetting;
use base 'Tab::DBI';
Tab::JudgeSetting->table('judge_setting');
Tab::JudgeSetting->columns(All => qw/id judge tag value value_date value_text timestamp/);
Tab::JudgeSetting->has_a(judge => 'Tab::Judge');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

