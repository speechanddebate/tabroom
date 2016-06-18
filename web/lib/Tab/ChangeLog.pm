package Tab::TournChange;
use base 'Tab::DBI';
Tab::TournChange->table('tourn_change');
Tab::TournChange->columns(Primary => qw/id/);
Tab::TournChange->columns(Essential => qw/type tourn school account/);
Tab::TournChange->columns(Others => qw/judge entry event new_panel old_panel fine text timestamp/);

Tab::TournChange->has_a(account => "Tab::Account");
Tab::TournChange->has_a(tourn => "Tab::Tourn");
Tab::TournChange->has_a(event => "Tab::Event");
Tab::TournChange->has_a(entry => "Tab::Entry");
Tab::TournChange->has_a(school => "Tab::School");
Tab::TournChange->has_a(judge => "Tab::Judge");
Tab::TournChange->has_a(fine => "Tab::SchoolFine");

Tab::TournChange->has_a(new_panel => "Tab::Panel");
Tab::TournChange->has_a(old_panel => "Tab::Panel");

__PACKAGE__->_register_datetimes( qw/timestamp/);

