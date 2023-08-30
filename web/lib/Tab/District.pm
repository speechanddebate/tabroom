package Tab::District;
use base 'Tab::DBI';
Tab::District->table('district');
Tab::District->columns(Primary => qw/id/);
Tab::District->columns(Essential => qw/name code location timestamp/);
Tab::District->columns(Others => qw/level realm financials/);

Tab::District->has_many(chapters => 'Tab::Chapter', 'district');
Tab::District->has_many(schools => 'Tab::School', 'district');

Tab::District->has_many(permissions => 'Tab::Permission', 'district');
Tab::District->has_many(admins => [ Tab::Permission => 'person']);

sub financial {
	my ($self, $input) = @_;

	if ($input) {
		my $json = eval{
			return JSON::encode_json($input);
		};

		$self->financials($json);
		$self->update();
	} else {
		return eval {
			return JSON::decode_json($self->financials);
		};
	}
}
