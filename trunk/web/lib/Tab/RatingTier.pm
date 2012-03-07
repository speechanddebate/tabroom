package Tab::RatingTier;
use base 'Tab::DBI';
Tab::RatingTier->table('rating_tier');
Tab::RatingTier->columns(Primary => qw/id/);
Tab::RatingTier->columns(Essential => qw/name tourn judge_group/);
Tab::RatingTier->columns(Others => qw/timestamp description strike type max min conflict/);
Tab::RatingTier->has_a(tourn => 'Tab::Tourn');
Tab::RatingTier->has_a(judge_group => 'Tab::JudgeGroup');

Tab::RatingTier->set_sql(subset_rating_tier => "select rating_tier.* from rating_tier,rating
						where rating.judge = ?
						and rating.subset = ? 
						and rating.type = \"coach\"
						and rating.rating_tier = rating_tier.id");

Tab::RatingTier->set_sql(rating_tier => "select rating_tier.* from rating_tier,rating
						where rating.judge = ?
						and rating.rating_tier = rating_tier.id
						and rating.type = \"coach\"");
