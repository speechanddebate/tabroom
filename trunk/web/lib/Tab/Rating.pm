package Tab::Rating;
use base 'Tab::DBI';
Tab::Rating->table('rating');
Tab::Rating->columns(Primary => qw/id/);
Tab::Rating->columns(Essential => qw/tourn judge value type rating_tier rating_subset entry school entered timestamp/);

Tab::Rating->has_a(tourn => 'Tab::Tourn');
Tab::Rating->has_a(judge => 'Tab::Judge');
Tab::Rating->has_a(entry => 'Tab::Entry');
Tab::Rating->has_a(school => 'Tab::School');
Tab::Rating->has_a(rating_tier => 'Tab::RatingTier');
Tab::Rating->has_a(rating_subset => 'Tab::RatingSubset');

__PACKAGE__->_register_datetimes( qw/entered/ );
__PACKAGE__->_register_datetimes( qw/timestamp/);

