package Tab::ChapterAdmin;
use base 'Tab::DBI';
Tab::ChapterAdmin->table('tourn_admin');
Tab::ChapterAdmin->columns(Primary => qw/id/);
Tab::ChapterAdmin->columns(Essential => qw/tourn account timestamp contact nosetup entry/);
Tab::ChapterAdmin->has_a(tourn => 'Tab::Tourn');
Tab::ChapterAdmin->has_a(account => 'Tab::Account');

