package Tab::SweepInclude;
use base 'Tab::DBI';
Tab::SweepInclude->table('sweep_include');
Tab::SweepInclude->columns(All => qw/id parent child timestamp/);
Tab::SweepInclude->has_a(parent => 'Tab::SweepSet');
Tab::SweepInclude->has_a(child => 'Tab::SweepSet');
__PACKAGE__->_register_datetimes( qw/timestamp/);

