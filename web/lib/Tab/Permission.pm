package Tab::Permission;
use base 'Tab::DBI';
Tab::Permission->table('permission');
Tab::Permission->columns(All   => qw/id tag person category event tourn
										chapter region district circuit
										created_by details timestamp/);

Tab::Permission->has_a(circuit    => "Tab::Circuit");
Tab::Permission->has_a(district   => "Tab::District");
Tab::Permission->has_a(tourn      => "Tab::Tourn");
Tab::Permission->has_a(region     => "Tab::Region");
Tab::Permission->has_a(category   => "Tab::Category");
Tab::Permission->has_a(event      => "Tab::Event");
Tab::Permission->has_a(chapter    => "Tab::Chapter");
Tab::Permission->has_a(person     => "Tab::Person");
Tab::Permission->has_a(created_by => "Tab::Person");

__PACKAGE__->_register_datetimes(qw/timestamp/);

sub get_details {
	my $self = shift;
	my $hashref;

	if ($self->details) {
		$hashref = JSON::decode_json($self->details);
	}

	return $hashref;
}

sub set_details {
	my ($self, $hashref) = @_;
	my $json = JSON::encode_json($hashref);

	if ($json && $json ne "null") {
		$self->details($json);
	} else {
		$self->details("");
	}
	$self->update();
}

