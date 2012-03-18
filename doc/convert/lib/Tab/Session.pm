package Tab::Session;
use base 'Tab::DBI';
Tab::Session->table('session');
Tab::Session->columns(All => qw/id account authkey userkey timestamp ip limited entry league tournament school 
									director ie_annoy event/);
Tab::Session->has_a(account => 'Tab::Account');
Tab::Session->has_a(league => 'Tab::League');
Tab::Session->has_a(school => 'Tab::School');
Tab::Session->has_a(tournament => 'Tab::Tournament');
__PACKAGE__->_register_datetimes( qw/timestamp/);
