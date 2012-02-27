package Tab::Change;
use base 'Tab::DBI';
Tab::Change->table('changes');
Tab::Change->columns(Primary => qw/id/);
Tab::Change->columns(Essential => qw/type tournament timestamp/);
Tab::Change->columns(Others => qw/comp event panel judge fine moved_from regline school/);
Tab::Change->columns(TEMP => "qw/sortpanel/");
Tab::Change->has_a(panel => "Tab::Panel");
Tab::Change->has_a(moved_from => "Tab::Panel");
Tab::Change->has_a(comp => "Tab::Comp");
Tab::Change->has_a(school => "Tab::School");
Tab::Change->has_a(tournament => "Tab::Tournament");
Tab::Change->has_a(event => "Tab::Event");
Tab::Change->has_a(judge => "Tab::Judge");
Tab::Change->has_a(fine => "Tab::Fine");

__PACKAGE__->_register_datetimes( qw/timestamp/);

