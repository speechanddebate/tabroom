package Tab::JPoolJudge;
use base 'Tab::DBI';
Tab::JPoolJudge->table('jpool_judge');
Tab::JPoolJudge->columns(Primary => qw/id/);
Tab::JPoolJudge->columns(Essential => qw/jpool judge timestamp/);
Tab::JPoolJudge->has_a(judge => "Tab::Judge");
Tab::JPoolJudge->has_a(jpool => "Tab::JPool");

__PACKAGE__->_register_datetimes( qw/timestamp/);
