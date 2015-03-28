package Tab::NSDA::EventCategories;
use base 'Tab::DBI';   #keep a local cache because they don't change much and boy is it slooooow.
Tab::NSDA::EventCategories->table('nsda_event_categories');
Tab::NSDA::EventCategories->columns(Essential => qw/id type name nsda_id timestamp/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

