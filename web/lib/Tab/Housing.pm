package Tab::Housing;
use base 'Tab::DBI';
Tab::Housing->table('housing');
Tab::Housing->columns(Primary => "id");
Tab::Housing->columns(Essential => qw/tourn student tba judge type waitlist night requested account school timestamp/);

Tab::Housing->has_a(tourn => 'Tab::Tourn');
Tab::Housing->has_a(judge => 'Tab::Judge');
Tab::Housing->has_a(school => 'Tab::School');
Tab::Housing->has_a(account => 'Tab::Account');
Tab::Housing->has_a(student => 'Tab::Student');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/requested/);
__PACKAGE__->_register_dates( qw/night/);

