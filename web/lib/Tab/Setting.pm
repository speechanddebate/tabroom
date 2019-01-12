package Tab::Setting;
use base 'Tab::DBI';
Tab::Setting->table('setting');
Tab::Setting->columns(Primary => qw/id/); 
Tab::Setting->columns(Essential => qw/tag sort_order answer_type setting_header /);
Tab::Setting->columns(Others => qw/answer conditional_on conditional_value timestamp/);

Tab::Setting->has_a(setting_header => 'Tab::SettingHeader');

Tab::Setting->has_many( person_settings       => 'Tab::PersonSetting'      , 'setting');
Tab::Setting->has_many( circuit_settings      => 'Tab::CircuitSetting'     , 'setting');
Tab::Setting->has_many( entry_settings        => 'Tab::EntrySetting'       , 'setting');
Tab::Setting->has_many( event_settings        => 'Tab::EventSetting'       , 'setting');
Tab::Setting->has_many( jpool_settings        => 'Tab::JPoolSetting'       , 'setting');
Tab::Setting->has_many( category_settings     => 'Tab::CategorySetting'    , 'setting');
Tab::Setting->has_many( judge_settings        => 'Tab::JudgeSetting'       , 'setting');
Tab::Setting->has_many( region_settings       => 'Tab::RegionSetting'      , 'setting');
Tab::Setting->has_many( round_settings        => 'Tab::RoundSetting'       , 'setting');
Tab::Setting->has_many( rpool_settings        => 'Tab::RPoolSetting'       , 'setting');
Tab::Setting->has_many( school_settings       => 'Tab::SchoolSetting'      , 'setting');
Tab::Setting->has_many( tiebreak_set_settings => 'Tab::TiebreakSetSetting' , 'setting');
Tab::Setting->has_many( tourn_settings        => 'Tab::TournSetting'       , 'setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub label {
	my ($self, $lang) = @_;
	return Tab::SettingLabel->search( 
		setting => $self->id,
		lang    => $lang
	)->first;
}


