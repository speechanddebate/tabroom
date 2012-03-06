package Tab::HousingSlots;
use base 'Tab::DBI';
Tab::HousingSlots->table('housing_slots');
Tab::HousingSlots->columns(Essential => qw/id tourn night slots/);
Tab::HousingSlots->has_a(tourn => 'Tab::Tourn');
__PACKAGE__->_register_dates( qw/night/);
