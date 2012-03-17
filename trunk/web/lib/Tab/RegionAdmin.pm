package Tab::RegionAdmin;
use base 'Tab::DBI';
Tab::RegionAdmin->table('region_admin');
Tab::RegionAdmin->columns(All => qw/id region timestamp account/);
Tab::RegionAdmin->has_a(region => "Tab::Region");
Tab::RegionAdmin->has_a(account => "Tab::Account");

