package Tab::AccountSetting;
use base 'Tab::DBI';
Tab::AccountSetting->table('account_setting');
Tab::AccountSetting->columns(All => qw/id account tag value value_date value_text timestamp/);
Tab::AccountSetting->has_a(account => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

