package Tab::RatingTier;
use base 'Tab::DBI';
Tab::RatingTier->table('rating_tier');
Tab::RatingTier->columns(Primary => qw/id/);
Tab::RatingTier->columns(Essential => qw/name tourn judge_group/);
Tab::RatingTier->columns(Others => qw/timestamp description strike type max min conflict/);

Tab::RatingTier->has_a(tourn => 'Tab::Tourn');
Tab::RatingTier->has_a(judge_group => 'Tab::JudgeGroup');

Tab::RatingTier->has_many(ratings => 'Tab::Rating', 'rating_tier');

__PACKAGE__->_register_datetimes( qw/timestamp/);

