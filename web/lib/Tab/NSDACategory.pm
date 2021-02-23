package Tab::NSDACategory;
use base 'Tab::DBI';   #keep a local cache because they don't change much and boy is it slooooow.
Tab::NSDACategory->table('nsda_category');
Tab::NSDACategory->columns(Essential => qw/id type code name national timestamp/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

