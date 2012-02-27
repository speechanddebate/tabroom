package Tab::TeamMember;
use base 'Tab::DBI';
Tab::TeamMember->table('team_member');
Tab::TeamMember->columns(All => qw/id comp timestamp student/);
Tab::TeamMember->has_a(student => 'Tab::Student');
Tab::TeamMember->has_a(comp => 'Tab::Comp');

