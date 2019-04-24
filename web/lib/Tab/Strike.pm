package Tab::Strike;
use base 'Tab::DBI';
Tab::Strike->table('strike');
Tab::Strike->columns(Primary => qw/id/); 
Tab::Strike->columns(Essential => qw/type start end registrant conflict conflictee 
							tourn judge event entry school district region timeslot shift 
							entered_by created_at timestamp/);

Tab::Strike->has_a(tourn      => 'Tab::Tourn');
Tab::Strike->has_a(judge      => 'Tab::Judge');
Tab::Strike->has_a(event      => 'Tab::Event');
Tab::Strike->has_a(entry      => 'Tab::Entry');
Tab::Strike->has_a(school     => 'Tab::School');
Tab::Strike->has_a(region     => 'Tab::Region');
Tab::Strike->has_a(district   => 'Tab::District');
Tab::Strike->has_a(timeslot   => 'Tab::Timeslot');
Tab::Strike->has_a(shift      => 'Tab::JudgeShift');
Tab::Strike->has_a(entered_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/start end timestamp created_at/);


