package Tab::ConcessionType;
use base 'Tab::DBI';
Tab::ConcessionType->table('concession_type');
Tab::ConcessionType->columns(Essential => qw/id concession name description timestamp/);
Tab::ConcessionType->has_a(concession => 'Tab::Concession');

Tab::ConcessionType->has_many(options => 'Tab::ConcessionOption', 'concession_type');
Tab::ConcessionType->has_many(concession_options => 'Tab::ConcessionOption', 'concession_type');
__PACKAGE__->_register_datetimes( qw/timestamp/);

