package Tab::SurveyResponse;
use base 'Tab::DBI';
Tab::SurveyResponse->table('survey_response');
Tab::SurveyResponse->columns(Essential => qw/id start survey person tourn judge entry school timestamp/);

Tab::SurveyResponse->has_a(survey => 'Tab::Survey');
Tab::SurveyResponse->has_a(person => 'Tab::Person');
Tab::SurveyResponse->has_a(tourn => 'Tab::Tourn');
Tab::SurveyResponse->has_a(judge => 'Tab::Judge');
Tab::SurveyResponse->has_a(entry => 'Tab::Entry');
Tab::SurveyResponse->has_a(school => 'Tab::School');

Tab::SurveyResponse->has_many(answers => 'Tab::SurveyAnswer', 'survey_response');

__PACKAGE__->_register_datetimes( qw/start timestamp/);
