package Tab::Region;
use base 'Tab::DBI';
Tab::Region->table('region');
Tab::Region->columns(Primary => qw/id/);
Tab::Region->columns(Essential => qw/circuit name code timestamp/);
Tab::Region->columns(Others => qw/diocese quota arch sweeps cooke_pts tourn/);
Tab::Region->columns(TEMP => qw/registered unregistered/);

Tab::Region->has_a(circuit => 'Tab::Circuit');
Tab::Region->has_a(tourn => 'Tab::Tourn');

Tab::Region->has_many(schools => 'Tab::School', 'region');
Tab::Region->has_many(fines => 'Tab::RegionFine', 'region');
Tab::Region->has_many(region_admins => 'Tab::RegionAdmin', 'region');
Tab::Region->has_many(admins => [ Tab::RegionAdmin => 'account']);
Tab::Region->has_many(chapters => [ Tab::ChapterCircuit => 'chapter']);

