package Tab::RatingTier;
use base 'Tab::DBI';
Tab::RatingTier->table('rating_tier');
Tab::RatingTier->columns(Primary => qw/id/);
Tab::RatingTier->columns(Essential => qw/name rating_subset category/);
Tab::RatingTier->columns(Others => qw/timestamp description strike type max min conflict start/);

Tab::RatingTier->has_a(category => 'Tab::Category');
Tab::RatingTier->has_a(rating_subset => 'Tab::RatingSubset');
Tab::RatingTier->has_many(ratings => 'Tab::Rating', 'rating_tier');

__PACKAGE__->_register_datetimes( qw/timestamp/);

