package Tab::EventSetting;
use base 'Tab::DBI';
Tab::EventSetting->table('event_setting');
Tab::EventSetting->columns(All => qw/id event tag value value_date value_text timestamp/);
Tab::EventSetting->has_a(event => 'Tab::Event');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

