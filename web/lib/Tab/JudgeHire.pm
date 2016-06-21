package Tab::JudgeHire;
use base 'Tab::DBI';
Tab::JudgeHire->table('judge_hire');
Tab::JudgeHire->columns(Primary => qw/id/);
Tab::JudgeHire->columns(Essential => qw/entries_requested entries_accepted rounds_requested rounds_accepted requested_at 
										judge tourn category school requestor timestamp/);

Tab::JudgeHire->has_a(requestor => "Tab::Person");
Tab::JudgeHire->has_a(tourn => "Tab::Tourn");
Tab::JudgeHire->has_a(judge => "Tab::Judge");
Tab::JudgeHire->has_a(school => "Tab::School");
Tab::JudgeHire->has_a(category => "Tab::Category");

__PACKAGE__->_register_datetimes( qw/requested_at timestamp/ );

