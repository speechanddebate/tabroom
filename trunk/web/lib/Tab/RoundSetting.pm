package Tab::RoundSetting;
use base 'Tab::DBI';
Tab::RoundSetting->table('round_setting');
Tab::RoundSetting->columns(All => qw/id round tag value value_date value_text timestamp/);
Tab::RoundSetting->has_a(round => 'Tab::Round');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

