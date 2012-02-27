package Tab::ChapterAccess;
use base 'Tab::DBI';
Tab::ChapterAccess->table('chapter_access');
Tab::ChapterAccess->columns(All => qw/id chapter account timestamp/);
Tab::ChapterAccess->has_a(chapter => 'Tab::Chapter');
Tab::ChapterAccess->has_a(account => 'Tab::Account');


