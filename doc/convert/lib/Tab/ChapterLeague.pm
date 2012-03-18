package Tab::ChapterLeague;
use base 'Tab::DBI';
Tab::ChapterLeague->table('chapter_league');
Tab::ChapterLeague->columns(Primary => qw/id/);
Tab::ChapterLeague->columns(Essential => qw/league chapter code membership full_member active paid region/);
Tab::ChapterLeague->has_a(chapter => "Tab::Chapter");
Tab::ChapterLeague->has_a(league => "Tab::League");
Tab::ChapterLeague->has_a(region => "Tab::Region");
Tab::ChapterLeague->has_a(membership => "Tab::Membership");
