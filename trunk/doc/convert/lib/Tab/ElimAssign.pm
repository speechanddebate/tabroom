
package Tab::ElimAssign;
use base 'Tab::DBI';
Tab::ElimAssign->table('elim_assign');
Tab::ElimAssign->columns(Primary => qw/id/);
Tab::ElimAssign->columns(Essential => qw/timestamp judge judge_group round/);
Tab::ElimAssign->has_a(judge => "Tab::Judge");
Tab::ElimAssign->has_a(judge_group => "Tab::JudgeGroup");

