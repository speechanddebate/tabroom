package Tab::Session;
use base 'Tab::DBI';
Tab::Session->table('session');
Tab::Session->columns(All    => qw/id person userkey timestamp ip tourn event category weekend su defaults/);
Tab::Session->has_a(person   => 'Tab::Person');
Tab::Session->has_a(su       => 'Tab::Person');
Tab::Session->has_a(tourn    => 'Tab::Tourn');
Tab::Session->has_a(event    => 'Tab::Event');
Tab::Session->has_a(weekend  => 'Tab::Weekend');
Tab::Session->has_a(category => 'Tab::Category');

sub account {
	my $self = shift;
	return $self->person;
}

__PACKAGE__->_register_datetimes( qw/timestamp/);

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
