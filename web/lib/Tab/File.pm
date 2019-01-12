package Tab::File;
use base 'Tab::DBI';
Tab::File->table('file');
Tab::File->columns(All => qw/id label filename tag type published coach uploaded 
								tourn school entry event district circuit webpage parent person timestamp/);

Tab::File->has_a(school   => 'Tab::School');
Tab::File->has_a(event    => 'Tab::Event');
Tab::File->has_a(entry    => 'Tab::Entry');
Tab::File->has_a(tourn    => 'Tab::Tourn');
Tab::File->has_a(parent   => 'Tab::File');
Tab::File->has_a(person   => 'Tab::Person');
Tab::File->has_a(circuit  => 'Tab::Circuit');
Tab::File->has_a(district => 'Tab::District');
Tab::File->has_a(webpage  => 'Tab::Webpage');

Tab::File->has_many(children => 'Tab::File', 'parent');

__PACKAGE__->_register_datetimes( qw/uploaded timestamp/);

