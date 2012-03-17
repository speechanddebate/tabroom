package Tab::RegionFine;
use base 'Tab::DBI';
Tab::RegionFine->table('region_fine');
Tab::RegionFine->columns(All => qw/id region tourn amount reason levied_on levied_by timestamp/);

Tab::RegionFine->has_a(region => 'Tab::Region');
Tab::RegionFine->has_a(levied_by => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/levied_on/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
