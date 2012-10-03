package Tab::FollowAccount;
use base 'Tab::DBI';
Tab::FollowAccount->table('follow_account');
Tab::FollowAccount->columns(Primary => qw/id/);
Tab::FollowAccount->columns(Essential => qw/account follower parent request timestamp/);
Tab::FollowAccount->has_a(account => 'Tab::Account');
Tab::FollowAccount->has_a(follower => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/timestamp/);
