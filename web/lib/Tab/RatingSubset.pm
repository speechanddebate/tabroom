package Tab::RatingSubset;
use base 'Tab::DBI';
Tab::RatingSubset->table('rating_subset');
Tab::RatingSubset->columns(Primary => qw/id/);
Tab::RatingSubset->columns(Essential => qw/name judge_group timestamp/);

Tab::RatingSubset->has_a(judge_group => 'Tab::JudgeGroup');

Tab::RatingSubset->has_many(events => 'Tab::Event', "rating_subset");
Tab::RatingSubset->has_many(ratings => 'Tab::Rating', "rating_subset");

__PACKAGE__->_register_datetimes( qw/timestamp/);

