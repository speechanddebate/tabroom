package Tab::SurveyAnswer;
use base 'Tab::DBI';
Tab::SurveyAnswer->table('survey_answer');
Tab::SurveyAnswer->columns(Essential => qw/id value survey_response survey_question timestamp/);

Tab::SurveyAnswer->has_a(survey_response => 'Tab::SurveyResponse');
Tab::SurveyAnswer->has_a(survey_question => 'Tab::SurveyQuestion');

__PACKAGE__->_register_datetimes( qw/timestamp/);
