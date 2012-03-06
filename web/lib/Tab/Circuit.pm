package Tab::Circuit;
use base 'Tab::DBI';
Tab::Circuit->table('chapter');
Tab::Circuit->columns(Primary => qw/id/);
Tab::Circuit->columns(Essential => qw/name short_name timezone active state/);
Tab::Circuit->columns(Others => qw/url public_email hosted_site timestamp
						dues_to region_based track_bids last_change approved
						diocese_based site_theme header_file dues_amount apda_seeds 
						tourn_only full_members logo_file public_results invoice_message/);

Tab::Circuit->has_many(tourns => "Tab::Tourn");
Tab::Circuit->has_many(regions => "Tab::Region");
Tab::Circuit->has_many(sites => "Tab::Site");
Tab::Circuit->has_many(circuit_memberships => "Tab::CircuitMembership");
Tab::Circuit->has_many(chapter_chapters => "Tab::ChapterCircuit");

Tab::Circuit->has_a(dues_to => "Tab::Account");
Tab::Circuit->has_a(last_change => "Tab::Account");

__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::Circuit->set_sql(by_admin => "
				select distinct chapter.*
					from chapter,chapter_admin
					where chapter.id = chapter_admin.chapter
					and chapter_admin.account = ? ");

sub admins {
    my $self = shift;
    return sort {$a->last cmp $b->last} Tab::Account->search_by_chapter_admin($self->id);
}

sub coaches { 
	my $self = shift;
    return sort {$a->last cmp $b->last} Tab::Account->search_by_chapter_coach($self->id);
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
    return sort {$a->name cmp $b->name} Tab::Chapter->search_chapters($self->id);
}

sub non_members {
    my $self = shift;
	return sort {$a->name cmp $b->name} Tab::Chapter->search_chapter_and_membership($self->id, 0);
}

sub members {
    my $self = shift;
	return sort {$a->name cmp $b->name} Tab::Chapter->search_chapter_and_membership($self->id, 1);
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

Tab::Circuit->set_sql(chapters => " select distinct chapter.id 
		from chapter,chapter_chapter where chapter.id = chapter_chapter.chapter 
		and chapter_chapter.chapter = ?");

sub setting {

	my ($self, $tag, $value, $text) = @_;

	my @existing = Tab::CircuitSetting->search(  
		circuit => $self->id,
		tag => $tag
	);

    if ($value &! $value == 0) {

		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->text($text);
			$exists->update;

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} else {

			Tab::CircuitSetting->create({
				circuit => $self->id,
				tag => $tag,
				value => $value,
				text => $text
			});

		}

	} else {

		return unless @existing;

		my $setting = shift @existing;

		foreach my $other (@existing) { 
			$other->delete;
		}

		return $setting->text if $setting->value eq "text";
		return $setting->value;

	}

}
