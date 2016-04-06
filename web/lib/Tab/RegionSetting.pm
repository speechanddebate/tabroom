package Tab::RegionSetting;
use base 'Tab::DBI';
Tab::RegionSetting->table('region_setting');
Tab::RegionSetting->columns(All => qw/id region tag value value_date value_text event timestamp/);
Tab::RegionSetting->has_a(region => 'Tab::Region');
Tab::RegionSetting->has_a(event => 'Tab::Event');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

