package Tab::Webpage;
use base 'Tab::DBI';
Tab::Webpage->table('webpage');
Tab::Webpage->columns(Primary => qw/id/);
Tab::Webpage->columns(Essential => qw/tourn circuit page_order title content last_editor posted_on timestamp active sitewide special parent help/);

Tab::Webpage->has_a(tourn => 'Tab::Tourn');
Tab::Webpage->has_a(circuit => 'Tab::Circuit');
Tab::Webpage->has_a(last_editor => 'Tab::Account');
Tab::Webpage->has_a(parent => 'Tab::Webpage');

__PACKAGE__->_register_datetimes( qw/posted_on/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

