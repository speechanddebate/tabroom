package Tab::File;
use base 'Tab::DBI';
Tab::File->table('file');
Tab::File->columns(All => qw/id label name school tourn event uploaded posting result timestamp/);
Tab::File->has_a(school => 'Tab::School');
Tab::File->has_a(event => 'Tab::Event');
Tab::File->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/uploaded/);
