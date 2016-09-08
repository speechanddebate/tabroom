package Tab::File;
use base 'Tab::DBI';
Tab::File->table('file');
Tab::File->columns(All => qw/id label filename type posting published uploaded tourn school event result circuit webpage/);
Tab::File->has_a(school => 'Tab::School');
Tab::File->has_a(event => 'Tab::Event');
Tab::File->has_a(tourn => 'Tab::Tourn');
Tab::File->has_a(result => 'Tab::Result');
Tab::File->has_a(circuit => 'Tab::Circuit');
Tab::File->has_a(webpage => 'Tab::Webpage');

__PACKAGE__->_register_datetimes( qw/uploaded/);

