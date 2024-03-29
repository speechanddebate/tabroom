package Tab::SweepSet;
use base 'Tab::DBI';
Tab::SweepSet->table('sweep_set');
Tab::SweepSet->columns(All => qw/id tourn name sweep_award timestamp/);

Tab::SweepSet->has_a(tourn => 'Tab::Tourn');
Tab::SweepSet->has_a(sweep_award => 'Tab::SweepAward');

Tab::SweepSet->has_many(rules => 'Tab::SweepRule', 'sweep_set');
Tab::SweepSet->has_many(sweep_events => 'Tab::SweepEvent', 'sweep_set');
Tab::SweepSet->has_many(events => [Tab::SweepEvent => 'event']);
Tab::SweepSet->has_many(children => [Tab::SweepInclude => 'child'], 'parent');
Tab::SweepSet->has_many(parents => [Tab::SweepInclude => 'parent'], 'child');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub scopes { 

	my ($self, $value) = @_;

	if ($value) { 
	
		my $json = eval { 
			return JSON::encode_json($value);
		};

		$self->scope($json);
		$self->update();

	} else { 

		return eval { 
			return JSON::decode_json($self->scope);
		};
	}
}

sub rule {

	my ($self, $tag, $value) = @_;

	my @existing = Tab::SweepRule->search(  
		sweep_set => $self->id,
		tag       => $tag
	);

    if (defined $value) {

		if (@existing) {

			my $exists = shift @existing;
			if ($value eq "delete" || $value eq "" || $value eq 0) { 
				$exists->delete;
			} else { 
				$exists->value($value);
				$exists->update;
			}
			foreach my $other (@existing) { 
				$other->delete;
			}
			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $exists = Tab::SweepRule->create({
				sweep_set => $self->id,
				tag       => $tag,
				value     => $value,
			});

			$exists->update;
		}

	} else {
		return unless @existing;
		my $setting = shift @existing;
		return $setting->value;
	}

}
