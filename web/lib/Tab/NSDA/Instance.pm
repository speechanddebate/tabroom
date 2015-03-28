package Tab::NSDA::Instance;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Instance->table('auto_instance');
Tab::NSDA::Instance->columns(Essential => qw/instance_id status tourn_id source start_date end_date tournament tstamp location state type/);

__PACKAGE__->_register_datetimes( qw/tstamp/);
__PACKAGE__->_register_dates( qw/start_date end_date/);

