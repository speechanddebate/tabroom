package Tab::HousingSlots;
use base 'Tab::DBI';
Tab::HousingSlots->table('housing_slots');
Tab::HousingSlots->columns(Essential => qw/id tournament night slots/);
Tab::HousingSlots->has_a(tournament => 'Tab::Tournament');
__PACKAGE__->_register_dates( qw/night/);
