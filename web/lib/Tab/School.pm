package Tab::School;
use base 'Tab::DBI';
Tab::School->table('school');
Tab::School->columns(Essential => qw/id tourn name chapter region code contact/);
Tab::School->columns(Others => qw/registered registered_on entered_on 
                                  paid contact_name contact_number individuals
                                  noprefs timestamp self_register self_reg_deadline /);

Tab::School->has_a(tourn => 'Tab::Tourn');
Tab::School->has_a(chapter => 'Tab::Chapter');
Tab::School->has_a(contact => 'Tab::Contact');
Tab::School->has_a(region => 'Tab::Region');

Tab::School->has_many(purchases => 'Tab::ConcessionPurchase', 'school');
Tab::School->has_many(entries => 'Tab::Entry', 'school');
Tab::School->has_many(judges => 'Tab::Judge', 'school');
Tab::School->has_many(fines => 'Tab::SchoolFine', 'school');
Tab::School->has_many(hires => 'Tab::JudgeHire', 'school');
Tab::School->has_many(files => 'Tab::File', 'school');

__PACKAGE__->_register_datetimes( qw/entered_on registered_on self_reg_deadline/);
__PACKAGE__->_register_datetimes( qw/timestamp/);


sub events { 
	my $self = shift;
	return Tab::Event->search_by_school($self->id);
}

sub students { 
	my $self = shift;
	return Tab::Student->search_by_school($self->id);
}

sub short_name {
	my ($self, $limit) = @_;
	my $name = $self->name;
	$name =~ s/of Math and Science$//g;
	$name =~ s/Academy$//g;
	$name =~ s/Regional\ High\ School$//g;
	$name =~ s/High\ School$//g;
	$name =~ s/School$//g;
	$name =~ s/High$//g;
	$name =~ s/Preparatory$/Prep/g;
	$name =~ s/College\ Prep$/CP/g;
	$name =~ s/HS$//g;
	$name =~ s/Regional$//g;
	$name =~ s/Public\ Charter//g;
	$name =~ s/Charter\ Public//g;
	$name =~ s/^The//g;
	$name =~ s/^Saint/St./g;
	$name = "College Prep" if $name eq "CP";  #Sometimes it's the whole school name.  Oops.
	$name =~ s/High\ School/HS/g;
	$name =~ s/^\s+//;  #leading spaces
	$name =~ s/\s+$//;  #trailing spaces

    if ($limit) { 
        return substr($name,0,$limit);
    } else { 
    	return $name;
    }
}

