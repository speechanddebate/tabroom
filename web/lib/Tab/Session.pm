package Tab::Session;
use base 'Tab::DBI';
Tab::Session->table('session');
Tab::Session->columns(All    => qw/id person userkey timestamp ip su defaults created_at/);
Tab::Session->has_a(person   => 'Tab::Person');
Tab::Session->has_a(su       => 'Tab::Person');

sub account {
	my $self = shift;
	return $self->person;
}

__PACKAGE__->_register_datetimes( qw/timestamp created_at/);

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
