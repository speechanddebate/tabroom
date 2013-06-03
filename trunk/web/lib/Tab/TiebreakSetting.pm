package Tab::TiebreakSetting;
use base 'Tab::DBI';
Tab::TiebreakSetting->table('tiebreak_setting');
Tab::TiebreakSetting->columns(All => qw/id tiebreak_set tag value timestamp/);
Tab::TiebreakSetting->has_a(tiebreak_set => 'Tab::TiebreakSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);

