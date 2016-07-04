package Tab::NSDA::EventCategory;
use base 'Tab::DBI';   #keep a local cache because they don't change much and boy is it slooooow.
Tab::NSDA::EventCategory->table('nsda_event_category');
Tab::NSDA::EventCategory->columns(Essential => qw/id type code name national timestamp/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

