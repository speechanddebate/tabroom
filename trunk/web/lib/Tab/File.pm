package Tab::File;
use base 'Tab::DBI';
Tab::File->table('file');
Tab::File->columns(All => qw/id school label name tournament event uploaded posting result/);
Tab::File->has_a(school => 'Tab::School');
Tab::File->has_a(event => 'Tab::Event');
Tab::File->has_a(tournament => 'Tab::Tournament');

__PACKAGE__->_register_datetimes( qw/uploaded/);
