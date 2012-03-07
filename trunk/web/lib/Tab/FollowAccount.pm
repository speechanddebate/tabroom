package Tab::FollowAccount;
use base 'Tab::DBI';
Tab::FollowAccount->table('follow_account');
Tab::FollowAccount->columns(Primary => qw/id/);
Tab::FollowAccount->columns(Essential => qw/account cell email domain/);
Tab::FollowAccount->has_a(account => 'Tab::Account');

