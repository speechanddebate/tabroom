package Tab::NSDA::MundtPoint;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::MundtPoint->table('mundt_points');
Tab::NSDA::MundtPoint->columns(Essential => qw/postID school_id postcode postDate tourn_year points comment user_id/);
Tab::NSDA::MundtPoint->has_a(school_id => 'Tab::NSDA::School');

__PACKAGE__->_register_dates( qw/postDate/);
