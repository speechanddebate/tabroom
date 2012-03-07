package Tab::EventDouble;
use base 'Tab::DBI';
Tab::EventDouble->table('event_double');
Tab::EventDouble->columns(Primary =>qw/id/);
Tab::EventDouble->columns(Essential => qw/name tourn setting timestamp min max judge_group/);
Tab::EventDouble->has_many(events => "Tab::Event", 'class');
Tab::EventDouble->has_a(tourn => 'Tab::Tourn');
Tab::EventDouble->has_a(judge_group => 'Tab::JudgeGroup');
