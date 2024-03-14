package Tab::Tourn;
use base 'Tab::DBI';
Tab::Tourn->table('tourn');
Tab::Tourn->columns(Primary => qw/id/);
Tab::Tourn->columns(Essential => qw/name city state country webname
						start end reg_start reg_end tz
						hidden timestamp
					/);

Tab::Tourn->columns(TEMP => qw/schoolid/);

Tab::Tourn->has_many(files          => 'Tab::File'         , 'tourn');
Tab::Tourn->has_many(rpools         => 'Tab::RPool'        , 'tourn');
Tab::Tourn->has_many(emails         => 'Tab::Email'        , 'tourn');
Tab::Tourn->has_many(hotels         => 'Tab::Hotel'        , 'tourn'   => {order_by  => 'name'} );
Tab::Tourn->has_many(events         => 'Tab::Event'        , 'tourn'   => { order_by => 'name'} );
Tab::Tourn->has_many(entries        => 'Tab::Entry'        , 'tourn');
Tab::Tourn->has_many(ratings        => 'Tab::Rating'       , 'tourn');
Tab::Tourn->has_many(regions        => 'Tab::Region'       , 'tourn');
Tab::Tourn->has_many(strikes        => 'Tab::Strike'       , 'tourn');
Tab::Tourn->has_many(settings       => 'Tab::TournSetting' , 'tourn');
Tab::Tourn->has_many(schools        => 'Tab::School'       , 'tourn'   => { order_by => 'name'} );
Tab::Tourn->has_many(webpages       => 'Tab::Webpage'      , 'tourn');
Tab::Tourn->has_many(judge_hires    => 'Tab::JudgeHire'    , 'tourn');
Tab::Tourn->has_many(followers      => 'Tab::Follower'     , 'tourn');
Tab::Tourn->has_many(groups         => 'Tab::Category'     , 'tourn'   => { order_by => 'name'} );
Tab::Tourn->has_many(timeslots      => 'Tab::Timeslot'     , 'tourn'   => { order_by => 'start'} );
Tab::Tourn->has_many(sweep_sets     => 'Tab::SweepSet'     , 'tourn'   => {order_by  => 'name'} );
Tab::Tourn->has_many(tourn_fees     => 'Tab::TournFee'     , 'tourn');
Tab::Tourn->has_many(result_sets    => "Tab::ResultSet"    , 'tourn');
Tab::Tourn->has_many(tourn_sites    => 'Tab::TournSite'    , 'tourn');
Tab::Tourn->has_many(concessions    => 'Tab::Concession'   , 'tourn'   => { order_by => 'name'} );
Tab::Tourn->has_many(permissions    => 'Tab::Permission'   , 'tourn');
Tab::Tourn->has_many(room_strikes   => 'Tab::RoomStrike'   , 'tourn');
Tab::Tourn->has_many(fines          => 'Tab::Fine'         , 'tourn');
Tab::Tourn->has_many(categories     => 'Tab::Category'     , 'tourn');
Tab::Tourn->has_many(change_logs    => 'Tab::ChangeLog'    , 'tourn');
Tab::Tourn->has_many(patterns       => 'Tab::Pattern'      , 'tourn');
Tab::Tourn->has_many(protocols  => 'Tab::Protocol'  , 'tourn');
Tab::Tourn->has_many(tourn_circuits => 'Tab::TournCircuit' , 'tourn');
Tab::Tourn->has_many(weekends       => 'Tab::Weekend'      , 'tourn');
Tab::Tourn->has_many(quizzes        => 'Tab::Quiz'         , 'tourn');

Tab::Tourn->has_many(admins => [ Tab::Permission => 'person']);
Tab::Tourn->has_many(sites => [Tab::TournSite => 'site']);
Tab::Tourn->has_many(circuits => [Tab::TournCircuit => 'circuit']);

__PACKAGE__->_register_datetimes( qw/start end reg_start reg_end timestamp/);

sub location {
	my $self = shift;
	my $location = $self->state."/" if $self->state;
	return $location.$self->country;
}

sub full_location {
	my $self = shift;
	my $location;
	$location = $self->city.", " if $self->city;
	$location .= $self->state."/" if $self->state;
	return $location.$self->country;
}

sub location_name {

	my $self = shift;

	my $state = $m->comp(
		"/funclib/state_translate.mas",
		state => $self->state
	) if $self->state;

	my $country = $m->comp(
		"/funclib/country_translate.mas",
		country => $self->country
	) if $self->country;

	$country = $state.", ".$country if $state;
	return $country;
}


sub setting {

	my ($self, $tag, $value, $blob) = @_;
	$/ = ""; #Remove all trailing newlines

	chomp $blob;

	my @existing = Tab::TournSetting->search(
		tourn => $self->id,
		tag   => $tag
	);

	my $existing = shift @existing if @existing;
	foreach (@existing) { $_->delete(); }

	if (defined $value) {

		if ($existing) {

			$existing->value($value);

			if ($value eq "text") {
				$existing->value_text($blob);
			} elsif ($value eq "date") {
				$existing->value_date($blob);
			} elsif ($value eq "json") {
				eval{
					$existing->value_text(JSON::encode_json($blob));
				};
			}

			$existing->update;

			if ($value eq "delete" || $value eq "" || $value eq "0") {
				$existing->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::TournSetting->create({
				tourn => $self->id,
				tag   => $tag,
				value => $value,
			});

			if ($value eq "text") {
				$existing->value_text($blob);
			} elsif ($value eq "date") {
				$existing->value_date($blob);
			} elsif ($value eq "json") {

				eval{
					$existing->value_text(JSON::encode_json($blob));
				};
			}

			$existing->update();
		}

	} else {

		return unless $existing;
		if ($existing->value eq "text") {
			return $existing->value_text
		} elsif ($existing->value eq "date") {
			return $existing->value_date
		} elsif ($existing->value eq "json") {
			return eval {
				return JSON::decode_json($existing->value_text);
			};
		}
		return $existing->value;
	}
}

sub all_settings {

	my $self = shift;
	my %all_settings;
	my $dbh = Tab::DBI->db_Main();

    my $sth = $dbh->prepare("
		select
			setting.id, setting.tag, setting.value, setting.value_date, setting.value_text vt
		from tourn_setting setting
			where setting.tourn = ?
        order by setting.tag
    ");

    $sth->execute($self->id);

	my $settings = $sth->fetchall_hash();

	foreach my $setting (@{$settings}) {

		if ($setting->{value} eq "date") {

			my $dt = Tab::DBI::dateparse($setting->{value_date});
			$all_settings{$setting->{tag}} = $dt if $dt;

		} elsif ($setting->{value} eq "text") {

			$all_settings{$setting->{tag}} = $setting->{vt};

		} elsif ($setting->{value} eq "json") {

			$all_settings{$setting->{tag}} = eval {
				return JSON::decode_json($setting->{vt});
			};

		} else {
			$all_settings{$setting->{tag}} = $setting->{value};
		}
	}
	return %all_settings;
}

