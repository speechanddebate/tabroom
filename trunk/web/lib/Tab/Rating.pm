package Tab::Rating;
use base 'Tab::DBI';
Tab::Rating->table('rating');
Tab::Rating->columns(Primary => qw/id/);
Tab::Rating->columns(Essential => qw/tourn judge type rating_tier subset entry school entered name/);
Tab::Rating->columns(TEMP => qw/rating_tier_val/);

Tab::Rating->has_a(judge => 'Tab::Judge');
Tab::Rating->has_a(tourn => 'Tab::Tourn');
Tab::Rating->has_a(entry => 'Tab::Entry');
Tab::Rating->has_a(school => 'Tab::School');
Tab::Rating->has_a(rating_tier => 'Tab::RatingTier');

__PACKAGE__->_register_datetimes( qw/entered/ );

Tab::Rating->set_sql(school_ratings_by_group => "
						select distinct rating.*,rating_tier.name as rating_tier_val from rating,judge,rating_tier
						where rating.judge = judge.id
						and judge.judge_group = ?
						and rating.rating_tier = rating_tier.id
						and rating_tier.type = \"rating_tier\"");

Tab::Rating->set_sql(entry_ratings_by_group => "
						select distinct rating.*,rating_tier.name as rating_tier_val from rating,judge,rating_tier
						where rating.judge = judge.id
						and judge.judge_group = ?
						and rating.rating_tier = rating_tier.id
						and rating_tier.type = \"mpj\"");

