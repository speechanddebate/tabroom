package Tab::Link;
use base 'Tab::DBI';
Tab::Link->table('link');
Tab::Link->columns(Primary => qw/id/);
Tab::Link->columns(Essential => qw/id name timestamp text league active display_order/);
Tab::Link->has_a(league => 'Tab::League');
Tab::Link->has_many(news => 'Tab::News', "link");

return 1;

