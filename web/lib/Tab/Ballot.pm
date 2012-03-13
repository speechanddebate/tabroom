package Tab::Ballot;
use base 'Tab::DBI';
Tab::Ballot->table('ballot');
Tab::Ballot->columns(Primary => qw/id/);
Tab::Ballot->columns(Essential => qw/judge panel entry win rank points side speakerorder bye audit timestamp/);
Tab::Ballot->columns(Others => qw/countmenot speechnumber topic seed collected collected_by tv noshow chair/);

Tab::Ballot->has_a(judge => 'Tab::Judge');
Tab::Ballot->has_a(panel => 'Tab::Panel');
Tab::Ballot->has_a(entry => 'Tab::Entry');
Tab::Ballot->has_a(collected_by => 'Tab::Account');

__PACKAGE__->_register_dates( qw/timestamp collected/);

