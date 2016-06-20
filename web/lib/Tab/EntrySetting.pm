package Tab::EntrySetting;
use base 'Tab::DBI';
Tab::EntrySetting->table('entry_setting');
Tab::EntrySetting->columns(All => qw/id entry tag value value_date value_text setting timestamp/);
Tab::EntrySetting->has_a(entry => 'Tab::Entry');
Tab::EntrySetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

