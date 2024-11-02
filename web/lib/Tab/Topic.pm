package Tab::Topic;
use base 'Tab::DBI';
Tab::Topic->table('topic');
Tab::Topic->columns(Primary => qw/id/);
Tab::Topic->columns(Essential => qw/tag source event_type topic_text school_year sort_order pattern created_by created_at timestamp/);

__PACKAGE__->_register_datetimes( qw/created_at timestamp/);

