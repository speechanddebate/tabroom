package Tab::Weekend;
use base 'Tab::DBI';
Tab::Weekend->table('weekend');
Tab::Weekend->columns(Primary => qw/id/);
Tab::Weekend->columns(Essential => qw/name 
						tourn site city state
						start end reg_start reg_end 
						freeze_deadline drop_deadline judge_deadline fine_deadline
						timestamp /);

Tab::Weekend->has_a(tourn => 'Tab::Tourn');
Tab::Weekend->has_a(site => 'Tab::Site');

__PACKAGE__->_register_datetimes( qw/start end reg_start reg_end timestamp/);
__PACKAGE__->_register_datetimes( qw/freeze_deadline drop_deadline judge_deadline fine_deadline/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::TournSetting->search(  
		tourn => $self->id,
		tag    => $tag,
	)->first;

	if (defined $value) { 
			
		if ($existing) {

			$existing->value($value);
			$existing->value_text($blob) if $value eq "text";
			$existing->value_date($blob) if $value eq "date";
			$existing->update;

			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$existing->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::TournSetting->create({
				tourn => $self->id,
				tag    => $tag,
				value  => $value,
			});

			if ($value eq "text") { 
				$existing->value_text($blob);
			}

			if ($value eq "date") { 
				$existing->value_date($blob);
			}

			$existing->update;

		}

	} else {

		return unless $existing;
		return $existing->value_text if $existing->value eq "text";
		return $existing->value_date if $existing->value eq "date";
		return $existing->value;

	}

}

sub all_settings { 

	my $self = shift;

	my @settings = $self->settings;

	my %all_settings;

	foreach my $setting (@settings) { 
		$all_settings{$setting->tag} = $setting->value;
		$all_settings{$setting->tag} = $setting->value_text if $all_settings{$setting->tag} eq "text";
		$all_settings{$setting->tag} = $setting->value_date if $all_settings{$setting->tag} eq "date";
	}

	return %all_settings;

}

