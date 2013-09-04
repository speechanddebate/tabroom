package Tab::Stats;
use base 'Tab::DBI';
Tab::Stats->table('stats');
Tab::Stats->columns(All => qw/id event tag value label taken timestamp/);
Tab::Stats->has_a(event => 'Tab::Event');

__PACKAGE__->_register_datetimes( qw/taken timestamp/);

