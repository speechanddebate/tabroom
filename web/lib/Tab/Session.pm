package Tab::Session;
use base 'Tab::DBI';
Tab::Session->table('session');
Tab::Session->columns(All    => qw/id person userkey timestamp ip su defaults created_at agent_data geoip push_notify push_active last_access/);
Tab::Session->has_a(person   => 'Tab::Person');
Tab::Session->has_a(su       => 'Tab::Person');

sub account {
	my $self = shift;
	return $self->person;
}

__PACKAGE__->_register_datetimes( qw/timestamp created_at last_access push_active/);

sub default {
	my ($self, $input) = @_;

	if ($input) {
		my $json = eval{
			return JSON::encode_json($input);
		};

		$self->defaults($json);
		$self->update();
	} else {
		return eval {
			return JSON::decode_json($self->defaults);
		};
	}
}

sub location {
	my ($self, $input) = @_;

	if ($input) {
		my $json = eval{
			return JSON::encode_json($input);
		};

		$self->geoip($json);
		$self->update();
	} else {
		return eval {
			return JSON::decode_json($self->geoip);
		};
	}
}

sub agent {
	my ($self, $input) = @_;

	if ($input) {
		my $json = eval{
			return JSON::encode_json($input);
		};

		$self->agent_data($json);
		$self->update();
	} else {
		return eval {
			return JSON::decode_json($self->agent_data);
		};
	}
}
