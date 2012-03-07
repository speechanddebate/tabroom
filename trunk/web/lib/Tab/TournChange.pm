package Tab::TournChange;
use base 'Tab::DBI';
Tab::TournChange->table('changes');
Tab::TournChange->columns(Primary => qw/id/);
Tab::TournChange->columns(Essential => qw/type tourn timestamp/);
Tab::TournChange->columns(Others => qw/entry event panel judge fine moved_from regline school/);
Tab::TournChange->columns(TEMP => "qw/sortpanel/");
Tab::TournChange->has_a(panel => "Tab::Panel");
Tab::TournChange->has_a(moved_from => "Tab::Panel");
Tab::TournChange->has_a(entry => "Tab::Entry");
Tab::TournChange->has_a(school => "Tab::School");
Tab::TournChange->has_a(tourn => "Tab::Tourn");
Tab::TournChange->has_a(event => "Tab::Event");
Tab::TournChange->has_a(judge => "Tab::Judge");
Tab::TournChange->has_a(fine => "Tab::SchoolFine");

__PACKAGE__->_register_datetimes( qw/timestamp/);

