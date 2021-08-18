package Tab::Default;
use base 'Tab::DBI';
Tab::Default->table('tabroom_setting');
Tab::Default->columns(All => qw/id tag value value_date value_text timestamp/);

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

