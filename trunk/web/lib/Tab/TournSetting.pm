package Tab::TournSetting;
use base 'Tab::DBI';
Tab::TournSetting->table('tourn_setting');
Tab::TournSetting->columns(All => qw/id tourn tag value value_date text timestamp/);
Tab::TournSetting->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

