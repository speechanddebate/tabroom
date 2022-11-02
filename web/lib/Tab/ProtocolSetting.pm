package Tab::ProtocolSetting;
use base 'Tab::DBI';
Tab::ProtocolSetting->table('protocol_setting');
Tab::ProtocolSetting->columns(All => qw/id protocol tag value setting timestamp/);

Tab::ProtocolSetting->has_a(protocol => 'Tab::Protocol');
Tab::ProtocolSetting->has_a(setting  => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);

