package Tab::ChapterAdmin;
use base 'Tab::DBI';
Tab::ChapterAdmin->table('chapter_admin');
Tab::ChapterAdmin->columns(All => qw/id chapter account timestamp/);
Tab::ChapterAdmin->has_a(chapter => 'Tab::Chapter');
Tab::ChapterAdmin->has_a(account => 'Tab::Account');

