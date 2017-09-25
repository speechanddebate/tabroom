package Tab::TournWeekend;
use base 'Tab::DBI';
Tab::TournWeekend->table('tourn_weekend');
Tab::TournWeekend->columns(Primary => qw/id/);
Tab::TournWeekend->columns(Essential => qw/name 
						tourn city state
						start end reg_start reg_end 
						freeze_deadline drop_deadline judge_deadline fine_deadline
						timestamp /);

Tab::TournWeekend->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/start end reg_start reg_end timestamp/);
__PACKAGE__->_register_datetimes( qw/freeze_deadline drop_deadline judge_deadline fine_deadline/);
