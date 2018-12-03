package Tab::Ad;
use base 'Tab::DBI';
Tab::Ad->table('ad');
Tab::Ad->columns(Primary => qw/id/);
Tab::Ad->columns(Essential => qw/tag filename url sort_order start end approved approved_by person created_at timestamp/);
Tab::Ad->has_a(person => 'Tab::Person');
Tab::Ad->has_a(approved_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/start end timestamp created_at/);
