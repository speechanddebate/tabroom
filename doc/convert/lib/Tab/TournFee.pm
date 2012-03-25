package Tab::TournFee;
use base 'Tab::DBI';
Tab::TournFee->table('tourn_fee');
Tab::TournFee->columns(All => qw/id tourn amount reason start end timestamp/);

Tab::TournFee->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/start end/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
