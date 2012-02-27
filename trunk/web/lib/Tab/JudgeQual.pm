package Tab::JudgeQual;
use base 'Tab::DBI';
Tab::JudgeQual->table('judge_qual');
Tab::JudgeQual->columns(Primary => qw/id/);
Tab::JudgeQual->columns(Essential => qw/timestamp judge qual/);
Tab::JudgeQual->has_a(judge => "Tab::Judge");
Tab::JudgeQual->has_a(qual => "Tab::Qual");

