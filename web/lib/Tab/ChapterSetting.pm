package Tab::ChapterSetting;
use base 'Tab::DBI';
Tab::ChapterSetting->table('chapter_setting');
Tab::ChapterSetting->columns(All => qw/id chapter tag value value_date value_text setting timestamp/);
Tab::ChapterSetting->has_a(chapter => 'Tab::Chapter');
Tab::ChapterSetting->has_a(setting => 'Tab::Setting');

__PACKAGE__->_register_datetimes( qw/timestamp value_date/);

