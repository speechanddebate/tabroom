package Tab::ResultSet;
use base 'Tab::DBI';
Tab::ResultSet->table('result_set');
Tab::ResultSet->columns(Primary => qw/id/);
Tab::ResultSet->columns(Essential => qw/id tourn event bracket label generated timestamp published/);
Tab::ResultSet->has_a(tourn => 'Tab::Tourn');
Tab::ResultSet->has_a(event => 'Tab::Event' => { order_by => 'rank, id'} );
Tab::ResultSet->has_many(results => 'Tab::Result', 'result_set');

__PACKAGE__->_register_datetimes( qw/timestamp generated/);

