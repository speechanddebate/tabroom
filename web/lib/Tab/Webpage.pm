package Tab::Webpage;
use base 'Tab::DBI';
Tab::Webpage->table('webpage');
Tab::Webpage->columns(Primary => qw/id/);
Tab::Webpage->columns(Essential => qw/title content published sitewide special page_order tourn last_editor parent timestamp/);

Tab::Webpage->has_a(tourn => 'Tab::Tourn');
Tab::Webpage->has_a(last_editor => 'Tab::Person');
Tab::Webpage->has_a(parent => 'Tab::Webpage');

__PACKAGE__->_register_datetimes( qw/timestamp/);

