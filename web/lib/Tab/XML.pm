
package Tab::XML; # custom Tabroom XML::Simple subclass 

use base 'XML::Simple';
use Tab::General;

# Overriding the method here to sort the Tabroom XML data the way the schema
# wants.  This matters only one some data sets functionality wise.

sub sorted_keys {

	my ($self, $name, $hashref) = @_;

	if ($name eq 'JUDGE') { 

		my @keys = keys %$hashref;  # Damn you, Bruschke.
		my @event_keys;
		my @ts_keys;

		my $keylog;
		
		foreach my $key (sort @keys) { 
			push @event_keys, $key if (index($key, "Event") != -1);
			push @ts_keys, $key if (index($key, "Timeslot") != -1);
			$keylog .= $key;
		} 
		
		return ('ID', 'DOWNLOADRECORD','SCHOOL','FIRST','LAST','OBLIGATION','HIRED','TABRATING','STOPSCHEDULING','ADA','DIVERSE','NOTES','EMAIL', @event_keys, @ts_keys);
		
	} elsif ($name eq "TOURN") { 
		return ('ID', 'TOURNNAME', 'STARTDATE','ENDDATE','DOWNLOADSITE');
	} elsif ($name eq "ENTRY") { 
		return ('ID', 'SCHOOL', 'EVENT', 'RATING', 'CODE', 'FULLNAME', 'DROPPED', 'WAITLIST', 'ADA', 'TUBDISABILITY');
	} elsif ($name eq "ENTRY_STUDENT") { 
		return ('ID', 'DOWNLOADRECORD', 'SCHOOL', 'ENTRY', 'FIRST', 'LAST');
	} elsif ($name eq "SCHOOL") { 
		return ('ID', 'DOWNLOADRECORD', 'CODE', 'SCHOOLNAME', 'COACHES', 'CHAPTER', 'NSDA');
	} elsif ($name eq "ROUND") { 
		return ('ID', 'EVENT', 'TIMESLOT', 'TB_SET', 'RD_NAME', 'LABEL', 'FLIGHTING', 'JUDGESPERPANEL', 
			'JUDGEPLACESCHEME', 'PAIRINGSCHEME', 'RUNOFF', 'TOPIC', 'CREATEDOFFLINE');
	} elsif ($name eq "PANEL") { 
		return ('ID', 'ROUND', 'ROOM', 'FLIGHT', 'BYE');
	} elsif ($name eq "BALLOT") { 
		return ('ID', 'JUDGE', 'PANEL', 'ENTRY', 'SIDE', 'ROOM', 'BYE', 'NOSHOW', 'CHAIR');
	} elsif ($name eq "BALLOT_SCORE") { 
		return ('ID', 'BALLOT', 'RECIPIENT', 'SCORE_ID', 'SPEECH', 'SCORE');
	} elsif ($name eq "SCORES") { 
		return ('ID', 'SCORE_NAME', 'SCOREFOR', 'SORTORDER');
	} elsif ($name eq "SCORE_SETTING") { 
		return ('ID', 'TB_SET', 'SCORE', 'MAX', 'MIN', 'DUPESOK', 'DECIMALINCREMENTS');
	} elsif ($name eq "TIEBREAK") { 
		return ('ID', 'SortOrder', 'SCOREID', 'DROPS', 'FOROPPONENT', 'LABEL', 'TAG', 'TB_SET');
	} elsif ($name eq "ELIMSEED") { 
		return ('ID', 'EVENT', 'ROUND', 'ENTRY', 'SEED');
	} elsif ($name eq "TIMESLOT") { 
		return ('ID', 'TIMESLOTNAME', 'END', 'START');
	} elsif ($name eq "JUDGEPREF") { 
		return ('ID', 'TEAM', 'JUDGE', 'RATING', 'ORDPCT');
	} elsif ($name eq "TOURN_SETTING") { 
		return ('TAG', 'VALUE');
	} elsif ($name eq "ROOM") { 

		my @keys = keys %$hashref;  # Damn you, Bruschke.
		my @event_keys;
		my @ts_keys;

		my $keylog;

		foreach my $key (sort @keys) { 
			push @event_keys, $key if (index($key, "EVENT") != -1);
			push @ts_keys, $key if (index($key, "TIMESLOT") != -1);
			$keylog .= $key;
		} 
		
		return ('ID', 'BUILDING', 'ROOMNAME', 'QUALITY', 'CAPACITY', 'INACTIVE', 'NOTES', @event_keys, @ts_keys);

	}
	

   return $self->SUPER::sorted_keys($name, $hashref); 

}

return 1;

