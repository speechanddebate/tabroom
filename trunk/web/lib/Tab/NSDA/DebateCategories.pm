package Tab::NSDA::DebateCategories;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::DebateCategories->table('Debate_Categories');
Tab::NSDA::DebateCategories->columns(Essential => qw/category_id name/);

