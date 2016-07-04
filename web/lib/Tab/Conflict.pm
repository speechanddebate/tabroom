package Tab::Conflict;
use base 'Tab::DBI';
Tab::Conflict->table('conflict');
Tab::Conflict->columns(All => qw/id person conflicted chapter added_by timestamp/);
Tab::Conflict->has_a(added_by => 'Tab::Person');
Tab::Conflict->has_a(person => 'Tab::Person');
Tab::Conflict->has_a(conflicted => 'Tab::Person');
Tab::Conflict->has_a(chapter => 'Tab::Chapter');

