package Tab::JudgeClass;
use base 'Tab::DBI';
Tab::JudgeClass->table('judge_class');
Tab::JudgeClass->columns(Primary => qw/id/);
Tab::JudgeClass->columns(Essential => qw/timestamp judge class/);
Tab::JudgeClass->columns(Others => qw/qual tourn/);
Tab::JudgeClass->has_a(judge => "Tab::Judge");
Tab::JudgeClass->has_a(tourn => "Tab::Tourn");
Tab::JudgeClass->has_a(class => "Tab::Class");
Tab::JudgeClass->has_a(qual => "Tab::Qual");


