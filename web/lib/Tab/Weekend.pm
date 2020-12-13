package Tab::Weekend;
use base 'Tab::DBI';
Tab::Weekend->table('weekend');
Tab::Weekend->columns(Primary => qw/id/);
Tab::Weekend->columns(Essential => qw/name
						tourn site city state
						start end reg_start reg_end
						freeze_deadline drop_deadline judge_deadline fine_deadline
						timestamp /);

Tab::Weekend->has_a(tourn => 'Tab::Tourn');
Tab::Weekend->has_a(site => 'Tab::Site');

__PACKAGE__->_register_datetimes( qw/start end reg_start reg_end timestamp/);
__PACKAGE__->_register_datetimes( qw/freeze_deadline drop_deadline judge_deadline fine_deadline/);

