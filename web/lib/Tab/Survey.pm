package Tab::Survey;
use base 'Tab::DBI';
Tab::Survey->table('survey');
Tab::Survey->columns(Essential => qw/id name type condition start end tourn category event circuit timestamp/);

Tab::Survey->has_a(tourn => 'Tab::Tourn');
Tab::Survey->has_a(category => 'Tab::Category');
Tab::Survey->has_a(event => 'Tab::Event');
Tab::Survey->has_a(circuit => 'Tab::Circuit');

Tab::Survey->has_many(questions => 'Tab::SurveyQuestion', 'survey');
Tab::Survey->has_many(responses => 'Tab::SurveyResponse', 'survey');

__PACKAGE__->_register_datetimes( qw/start end timestamp/);
