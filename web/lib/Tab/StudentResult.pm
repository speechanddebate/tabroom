
package Tab::StudentResult;
use base 'Tab::DBI';
Tab::StudentResult->table('student_result');
Tab::StudentResult->columns(Primary => qw/id/);
Tab::StudentResult->columns(Essential => qw/comp timestamp student event chapter/);
Tab::StudentResult->columns(Others => qw/results_bar bid rank tournament/);
Tab::StudentResult->has_a(student => 'Tab::Student');
Tab::StudentResult->has_a(tournament => 'Tab::Tournament');
Tab::StudentResult->has_a(event => 'Tab::Event');
Tab::StudentResult->has_a(comp => 'Tab::Comp');
Tab::StudentResult->has_a(chapter => 'Tab::Chapter');
