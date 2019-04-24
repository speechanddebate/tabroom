package Tab::School;
use base 'Tab::DBI';
Tab::School->table('school');
Tab::School->columns(Essential => qw/ id name code onsite tourn chapter state region district timestamp /);

Tab::School->has_a(tourn => 'Tab::Tourn');
Tab::School->has_a(chapter => 'Tab::Chapter');
Tab::School->has_a(region => 'Tab::Region');
Tab::School->has_a(district => 'Tab::District');

Tab::School->has_many(settings  => 'Tab::SchoolSetting'      , 'school');
Tab::School->has_many(purchases => 'Tab::ConcessionPurchase' , 'school');
Tab::School->has_many(entries   => 'Tab::Entry'              , 'school');
Tab::School->has_many(judges    => 'Tab::Judge'              , 'school');
Tab::School->has_many(fines     => 'Tab::Fine'               , 'school');
Tab::School->has_many(invoices  => 'Tab::Invoice'            , 'school');
Tab::School->has_many(hires     => 'Tab::JudgeHire'          , 'school');
Tab::School->has_many(files     => 'Tab::File'               , 'school');
Tab::School->has_many(strikes   => 'Tab::Strike'             , 'school');

Tab::School->has_many(followers => [Tab::Follower => 'follower']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub short_name {
	my ($self, $limit) = @_;
	return &Tab::short_name($self->name, $limit);
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = ""; # Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::SchoolSetting->search(  
		school => $self->id,
		tag    => $tag,
	)->first;

	if (defined $value) { 
			
		if ($existing) {

			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$existing->delete;
			} else { 
				$existing->value($value);
				$existing->value_text($blob) if $value eq "text";
				$existing->value_date($blob) if $value eq "date";
				$existing->update;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::SchoolSetting->create({
				school => $self->id,
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

		return $existing->value_text 
			if $existing->value eq "text";

		return $existing->value_date 
			if $existing->value eq "date";

		return $existing->value;

	}

}


sub all_settings { 

	my $self = shift;

	my %all_settings;

	my $dbh = Tab::DBI->db_Main();

    my $sth = $dbh->prepare("
		select setting.tag, setting.value, setting.value_date, setting.value_text
		from school_setting setting
		where setting.school = ? 
        order by setting.tag
    ");
    
    $sth->execute($self->id);
    
    while( my ($tag, $value, $value_date, $value_text)  = $sth->fetchrow_array() ) { 

		if ($value eq "date") { 

			my $dt = Tab::DBI::dateparse($value_date); 
			$all_settings{$tag} = $dt if $dt;

		} elsif ($value eq "text") { 

			$all_settings{$tag} = $value_text;

		} else { 

			$all_settings{$tag} = $value;

		}

	}

	return %all_settings;

}

