package Tab::NSDA::Event;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Event->table('auto_events');
Tab::NSDA::Event->columns(Essential => qw/alt_id instance_id event_id type size name category nfl_cat_id nfl_subcat_id/);
Tab::NSDA::Event->has_a(instance_id => 'Tab::NSDA::Instance');

