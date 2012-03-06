package Tab::Membership;
use base 'Tab::DBI';
Tab::Membership->table('membership');
Tab::Membership->columns(Primary => qw/id/);
Tab::Membership->columns(Essential => qw/name text league dues dues_start dues_expire approval/);
Tab::Membership->has_a(league => "Tab::League");
Tab::Membership->has_many(chapter_memberships => "Tab::ChapterLeague", "membership");
__PACKAGE__->_register_datetimes( qw/dues_start/);
__PACKAGE__->_register_datetimes( qw/dues_expire/);
