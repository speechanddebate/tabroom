package Tab::TiebreakSetSetting;
use base 'Tab::DBI';
Tab::TiebreakSetSetting->table('tiebreak_set_setting');
Tab::TiebreakSetSetting->columns(All => qw/id tiebreak_set tag value setting timestamp/);

Tab::TiebreakSetSetting->has_a(tiebreak_set => 'Tab::TiebreakSet');
Tab::TiebreakSetSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);

