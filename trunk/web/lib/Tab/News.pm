package Tab::News;
use base 'Tab::DBI';
Tab::News->table('news');
Tab::News->columns(All => qw/id timestamp title posted league edited_by author pinned
							text file link active main_site sitewide edited_on tournament 
							display_order special/);

Tab::News->has_a(author => 'Tab::Account');
Tab::News->has_a(edited_by => 'Tab::Account');
Tab::News->has_a(link => 'Tab::Link');
Tab::News->has_a(league => 'Tab::League');
Tab::News->has_a(tournament => 'Tab::Tournament');

__PACKAGE__->_register_datetimes( qw/posted/);
__PACKAGE__->_register_datetimes( qw/edited_on/);

