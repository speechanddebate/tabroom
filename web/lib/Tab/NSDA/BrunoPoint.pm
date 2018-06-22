package Tab::NSDA::BrunoPoint;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::BrunoPoint->table('bruno_points');
Tab::NSDA::BrunoPoint->columns(Essential => qw/postID school_id postcode postDate tourn_year points comment user_id/);
Tab::NSDA::BrunoPoint->has_a(school_id => 'Tab::NSDA::School');

__PACKAGE__->_register_dates( qw/postDate/);
