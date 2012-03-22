package Tab::League;
use base 'Tab::DBI';
Tab::League->table('league');
Tab::League->columns(Primary => qw/id/);
Tab::League->columns(Essential => qw/name short_name timezone active state/);
Tab::League->columns(Others => qw/url public_email hosted_site timestamp
						dues_to region_based track_bids last_change approved
						diocese_based site_theme header_file dues_amount apda_seeds 
						tourn_only full_members logo_file public_results invoice_message/);
Tab::League->has_many(tournaments => "Tab::Tournament");
Tab::League->has_many(regions => "Tab::Region");
Tab::League->has_many(sites => "Tab::Site");
Tab::League->has_many(links => "Tab::Link");
Tab::League->has_many(news => "Tab::News");
Tab::League->has_many(methods => "Tab::Method");
Tab::League->has_many(memberships => "Tab::Membership");
Tab::League->has_many(chapter_leagues => "Tab::ChapterLeague");
Tab::League->has_a(dues_to => "Tab::Account");
Tab::League->has_a(last_change => "Tab::Account");
__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::League->set_sql(by_account => "
		select distinct league.*
		from league,league_admin
		where league.id = league_admin.league
		and league_admin.account = ? ");

sub admins {
    my $self = shift;
    return sort {$a->last cmp $b->last} Tab::Account->search_by_league_admin($self->id);
}

sub coaches { 
	my $self = shift;
    return sort {$a->last cmp $b->last} Tab::Account->search_by_league_coach($self->id);
}

sub accounts {
    my $self = shift;
    my @members;
	push (@members, $self->coaches);
	push (@members, $self->admins);
	@members = sort {$a->last cmp $b->last} @members;
	return @members;
}

sub chapters {
    my $self = shift;
    return sort {$a->name cmp $b->name} Tab::Chapter->search_leagues($self->id);
}

sub non_members {
    my $self = shift;
	return sort {$a->name cmp $b->name} Tab::Chapter->search_league_and_membership($self->id, 0);
}

sub members {
    my $self = shift;
	return sort {$a->name cmp $b->name} Tab::Chapter->search_league_and_membership($self->id, 1);
}

sub shorter_name {
	my $self = shift;
	my $name = $self->name;
	$name =~ s/Catholic Forensic League/CFL/;
	$name =~ s/Catholic Forensics League/CFL/;
	$name =~ s/Forensic League/FL/;
	$name =~ s/Forensics League/FL/;
	$name =~ s/Forensic Association/FA/;
	$name =~ s/Forensics Association/FA/;
	$name =~ s/Urban Debate League/UDL/;
	$name =~ s/High School Speech League/HSSL/;
	$name =~ s/High School Debate League/HSDL/;
	$name =~ s/Debate League/DL/;
	return $name;
}

Tab::League->set_sql(chapters => " select distinct league.id 
		from league,chapter_league where league.id = chapter_league.league 
		and chapter_league.chapter = ?");

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	my @existing = Tab::CircuitSetting->search(  
		circuit => $self->id,
		tag => $tag
	);

	if ($value &! $value == 0) { 

		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->value_text($blob) if $value eq "text";
			$exists->value_date($blob) if $value eq "date";
			$exists->update;

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} else {

			my $setting = Tab::CircuitSetting->create({
				circuit => $self->id,
				tag => $tag,
				value => $value,
			});

			$setting->value_text($blob) if $value eq "text";
			$setting->value_date($blob) if $value eq "date";
			$setting->update;

		}

	} else {

		return unless @existing;

		my $setting = shift @existing;

		foreach my $other (@existing) { 
			$other->delete;
		}

		return $setting->text if $setting->value eq "text";
		return $setting->datetime if $setting->value eq "date";
		return $setting->value;

	}
}
