package Tab::Setting;
use base 'Tab::DBI';
Tab::Setting->table('setting');
Tab::Setting->columns(Essential => qw/id type tag value value_date value_text timestamp updated_at created_at/);
Tab::Setting->columns(Others => qw/tourn judge_group event person judge round pool school circuit tiebreak_set/);
Tab::Setting->has_a(tourn => 'Tab::Tourn');
Tab::Setting->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Setting->has_a(event => 'Tab::Event');
Tab::Setting->has_a(judge => 'Tab::Judge');
Tab::Setting->has_a(round => 'Tab::Round');
Tab::Setting->has_a(pool => 'Tab::Pool');
Tab::Setting->has_a(person => 'Tab::Person');
Tab::Setting->has_a(school => 'Tab::School');
Tab::Setting->has_a(circuit => 'Tab::Circuit');
Tab::Setting->has_a(tiebreak_set => 'Tab::TiebreakSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

