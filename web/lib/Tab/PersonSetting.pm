package Tab::PersonSetting;
use base 'Tab::DBI';
Tab::PersonSetting->table('person_setting');
Tab::PersonSetting->columns(All => qw/id person tag value value_date value_text setting timestamp/);
Tab::PersonSetting->has_a(person => 'Tab::Person');
Tab::PersonSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

