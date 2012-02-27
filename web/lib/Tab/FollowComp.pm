package Tab::FollowComp;
use base 'Tab::DBI';
Tab::FollowComp->table('follow_comp');
Tab::FollowComp->columns(Primary => qw/id/);
Tab::FollowComp->columns(Essential => qw/comp cell email domain/);
Tab::FollowComp->has_a(comp => 'Tab::Comp');

