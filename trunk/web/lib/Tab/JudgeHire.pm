package Tab::JudgeHire;
use base 'Tab::DBI';
Tab::JudgeHire->table('judge_hire');
Tab::JudgeHire->columns(Primary => qw/id/);
Tab::JudgeHire->columns(Essential => qw/judge tourn judge_group school rounds rounds_accepted covers accepted request_made timestamp /);

Tab::JudgeHire->has_a(tourn => "Tab::Tourn");
Tab::JudgeHire->has_a(judge => "Tab::Judge");
Tab::JudgeHire->has_a(school => "Tab::School");
Tab::JudgeHire->has_a(judge_group => "Tab::JudgeGroup");

__PACKAGE__->_register_datetimes( qw/request_made timestamp/ );

