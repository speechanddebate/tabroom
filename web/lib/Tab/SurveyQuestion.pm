package Tab::SurveyQuestion;
use base 'Tab::DBI';
Tab::SurveyQuestion->table('survey_question');
Tab::SurveyQuestion->columns(Essential => qw/id order question explanation answer_format 
								survey event category tourn timestamp/);

Tab::SurveyQuestion->has_a(survey => 'Tab::Survey');
Tab::SurveyQuestion->has_a(event => 'Tab::Event');
Tab::SurveyQuestion->has_a(category => 'Tab::Category');
Tab::SurveyQuestion->has_a(tourn => 'Tab::Tourn');

Tab::SurveyQuestion->has_many(answers => 'Tab::SurveyAnswer', 'survey_question');

__PACKAGE__->_register_datetimes( qw/timestamp/);
