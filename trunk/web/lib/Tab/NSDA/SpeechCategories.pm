package Tab::NSDA::SpeechCategories;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::SpeechCategories->table('NEW_SPEECH_CATEGORIES');
Tab::NSDA::SpeechCategories->columns(Essential => qw/category_id name category_type_id active/);

