package Tab::Permission;
use base 'Tab::DBI';
Tab::Permission->table('permission');
Tab::Permission->columns(All   => qw/id tag account judge_group tourn chapter region circuit timestamp/);

Tab::Permission->has_a(circuit => "Tab::Circuit");
Tab::Permission->has_a(tourn   => "Tab::Tourn");
Tab::Permission->has_a(judge_group   => "Tab::JudgeGroup");
Tab::Permission->has_a(chapter => "Tab::Chapter");
Tab::Permission->has_a(region  => "Tab::Region");
Tab::Permission->has_a(account => "Tab::Account");

__PACKAGE__->_register_datetimes( qw/timestamp/);

