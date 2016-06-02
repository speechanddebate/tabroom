package Tab::Setting;
use base 'Tab::DBI';
Tab::Setting->table('setting');
Tab::Setting->columns(Primary => qw/id/); 
Tab::Setting->columns(Essential => qw/tag type subtype value_type conditions/);
Tab::Setting->columns(Others => qw/timestamp/);

Tab::Setting->has_many( setting_labels => 'Tab::SettingLabel', 'setting');

Tab::Setting->has_many( account_settings => 'Tab::AccountSetting', 'setting');
Tab::Setting->has_many( circuit_settings => 'Tab::CircuitSetting', 'setting');
Tab::Setting->has_many( entry_settings => 'Tab::EntrySetting', 'setting');
Tab::Setting->has_many( event_settings => 'Tab::EventSetting', 'setting');
Tab::Setting->has_many( jpool_settings => 'Tab::JPoolSetting', 'setting');
Tab::Setting->has_many( judge_group_settings => 'Tab::JudgeGroupSetting', 'setting');
Tab::Setting->has_many( judge_settings => 'Tab::JudgeSetting', 'setting');
Tab::Setting->has_many( region_settings => 'Tab::RegionSetting', 'setting');
Tab::Setting->has_many( round_settings => 'Tab::RoundSetting', 'setting');
Tab::Setting->has_many( rpool_settings => 'Tab::RPoolSetting', 'setting');
Tab::Setting->has_many( school_settings => 'Tab::SchoolSetting', 'setting');
Tab::Setting->has_many( tiebreak_settings => 'Tab::TiebreakSetting', 'setting');
Tab::Setting->has_many( tourn_settings => 'Tab::TournSetting', 'setting');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub label {
	my ($self, $lang) = @_;
	return Tab::SettingLabel->search( setting => $self->id, lang => $lang)->first;
}
