<%args>
	$round
</%args>
<%perl>

	our $debugme = 0; #1; #enable to show printed diagnostics


	my $now = DateTime->now(time_zone => "UTC");

	our $error_log = "Pairing ".$round->realname." of ".$round->event->tourn->name." at ".Tab::nicedt($now)." UTC\n\n";
	$error_log .= " Round ID is ".$round->id."\n\n";

	# To debug with this, go to panel/round/panel_master.mas and comment out
	# its redirect on line 284 or set a value for the $debugme value above

	use POSIX;

	my $event = $round->event;
	my $pullup_method = $event->setting("pullup_method");
	my $pullup_repeat = $event->setting("pullup_repeat");
	my $powermatch_method = $event->setting("powermatch");

	unless ($pullup_method) {
		$pullup_method="sop";
		$error_log .= "No pullup method set for this event; assuming SOP pullup method (opposition seeds).\n\n";		
	}

	unless ($powermatch_method) {
		$powermatch_method="sop";
		$error_log .= "No powermatch method set for this event; assuming SOP pullup method (seed + opposition seeds).\n\n";
	}

	if ($round->type eq "highhigh") { $powermatch_method = "highhigh"; }
	
	$error_log .= "Pairing using ".$powermatch_method." method for power-matching and ".$pullup_method." for pullups \n";	
		
	#erase any current round
	erase_current_round($round);
		
	my $oddround = 0;
	$oddround++ if ($round->name % 2); 
	$oddround = 1 if $event->setting("no_side_constraints");

	if ($round->type eq "highhigh" and $oddround == 0) {
		#send a message here
	}
	
	# use this round to seed

	my @round_dummy = Tab::Round->search( name => $round->name-1, event => $round->event );	

	my $round_for_seeding = Tab::Round->retrieve( $round_dummy[0]->id );

	print "Calling make_pairing_hash.mas\n\n" if $debugme;
	my ($entry_by_id_hash_ref, $precluded_hash_ref) = $m->comp("/funclib/make_pairing_hash.mas", round_id => $round->id);			

	my %entry_by_id = %{$entry_by_id_hash_ref};
	my %precluded = %{$precluded_hash_ref};

	my $entry_count = keys %entry_by_id; 

	#FIGURE OUT IF YOU NEED TO EVEN SIDES UP WITH TEAMS THAT HAD BYES IN THE PAST
	unless ($oddround) {

		my $aff; my $neg; my $sides_even; my $ctr; my $fixmarker;
	
		while ($sides_even == 0 and $ctr < 20 ) {

			undef $aff; 
			undef $neg; 
			undef $sides_even; 
			$ctr++;

			if ($ctr >= 20 ) { 
				last; 
				$error_log .= "bailed at 20 tries\n\n" ; 
			}

			foreach my $key (keys %entry_by_id ) {

				$aff++ if $entry_by_id{$key}{'sidedue'} == 1;
				$neg++ if $entry_by_id{$key}{'sidedue'} == 2;

			}
		
			#exit if things are even, or off by one (odd round with a bye)
			if (abs($aff-$neg) <= 1 ) { 
				undef $sides_even; 
				last; 
			}
			
			#find a team on the wrong side and switch if they've had a bye
			my $side_needed = 1; 
			undef $fixmarker;
			$side_needed = 2 if ($aff > $neg);

			foreach my $key (keys %entry_by_id ) {
				if ($fixmarker) { last; }
				if ($entry_by_id{$key}{'sidedue'} != $side_needed) {
					my @ballots = Tab::Ballot->search( entry => $key );
					foreach my $ballot (@ballots) {
						if ( $ballot->bye == 1 or $ballot->panel->bye == 1) {
							$entry_by_id{$key}{'sidedue'} = $side_needed;
							$error_log .= "Switching side due for ".$key." because sides need to be evened and they had a bye\n\n" ;
							$fixmarker=1;
							last;		
						}
					}
				}
			}
		}

		$error_log .= "$aff teams due aff and $neg teams due neg\n\n" ;

	}

	#BYE PROCESSING
	#	Figure out if you need a bye, and add it if you do; once it's in there it should just get
	#	treated like another team that's always last in the bracket and is only precluded from teams
	#	it's hit before
	
	if ( int($entry_count/2) != ($entry_count/2) ) {

		$entry_by_id{-1}{'code'} = "Bye";
		$entry_by_id{-1}{'wins'} = 0;
		$entry_by_id{-1}{'seed'} = $entry_count+1;
		$entry_by_id{-1}{'seed_nowins'} = $entry_count+1;
		$entry_by_id{-1}{'SOP'} = $entry_count*2;


		unless ($oddround) {
	
			my $aff; my $neg;
	
			foreach my $key (keys %entry_by_id ) {
				if ($entry_by_id{$key}{'sidedue'} == 1) { 
					$aff++;
				}
				if ($entry_by_id{$key}{'sidedue'} == 2) { 
					$neg++;
				}
			}

		$entry_by_id{-1}{'sidedue'} = 2;
		$entry_by_id{-1}{'sidedue'} = 1 if $aff < $neg;
		$entry_by_id{-1}{'side'} = $entry_by_id{-1}{'sidedue'};

		}
	}
	
#	print "Here's what you're working with:\n\n" ;
#	foreach my $key ( sort { $entry_by_id{$a}->{'seed'} <=> $entry_by_id{$b}->{'seed'} } keys(%entry_by_id) ) {
#		print $entry_by_id{$key}{'seed'}." ".$entry_by_id{$key}{'code'}." wins:".$entry_by_id{$key}{'wins'}." due:".$entry_by_id{$key}{'sidedue'}."\n\n" ;
#	}
	
	#FIGURE OUT WHETHER NORMAL POWER MATCH IS POSSIBLE
	#count number of possible opponents for each team, and whether one school dominates the bracket
	my $min_possible_opponents = 999;
	my $school_ct = 0; 
	my $hi_school; 
	my %bracket;
	
	my %school_entries;
	foreach my $key ( keys %entry_by_id ) {
		$school_entries{ $entry_by_id{$key}{'school'} } ++;
		$bracket{$key}{'side'} = 0;
	}
	
	count_possible_opponents(\%entry_by_id, \%precluded, \$oddround, \$min_possible_opponents, \%bracket);
	
	#figure n teams from 1 school in bracket
	foreach my $key (keys %school_entries) {
		if ( $school_entries{$key} > $school_ct ) { $school_ct = $school_entries{$key}; $hi_school=$key; }
	}
	
	$error_log .= "Fewest opponents for any team is ".$min_possible_opponents."\n\n"; 

	$error_log .= "Largest school entry is $school_ct and total entries for event are $entry_count\n\n" ;

	#if it's zero bail to Palmer's disaster mode
	if ($min_possible_opponents == 0) {
		$m->print("This can't be paired by normal means; one team has no possible opponents.  Try pairing by hand; use the back button on your browser.\n\n");
		$m->abort;
	}
	
	#do a highhigh if not enough degrees of freedom for a power match
	if ($min_possible_opponents <=3 or (($school_ct/$entry_count) > .4) ) { 
		$powermatch_method = "highhigh"; 
		$error_log .= "\nToo few degrees of freedom -- doing a high-high\n\n" ;
	}
	

	# PAIR IT

	#Figure out how many brackets to pair
	my $hibracket=-1;

	foreach my $key ( keys %entry_by_id ) {
		$hibracket = $entry_by_id{$key}{'wins'} if ($entry_by_id{$key}{'wins'} > $hibracket) ;
	}
	
	# Loop through the brackets, set them, then pair them creates a global hash
	# called %bracket that can be used by both functions

	my $x = $hibracket;
	if ($powermatch_method eq "highhigh") { $x=0; }
	
	# this will (1) pair the bracket if it can, honoring constraints in the
	# bracket.  Exits on success.

	# (2) If it can't pair the bracket, will click into "just make it work"
	# mode, which will honor the bracket and produce a pairing with the right
	# teams in the right bracket but with skewing in the bracket, which should
	# be necessary since the clean pairing attempt failed

	# (3) pull up a different team if even that doesn't work, and try it all
	# again  

	# (4) if it still can't pair, the setbracket function will automatically
	# collapse it with the next lower bracket 

	# (5) if its the bottom bracket, it will collapse the teams into the next higher bracket
	# and re-pair that
	
	my $n_tries=0; my $collapse_up = 0;
	
	while ($x > -1) {

		$error_log .= "Now setting $x-win bracket...";

		%bracket = setbracket($round->type, $oddround, $x, $pullup_method, $pullup_repeat, \%entry_by_id, \%bracket, \%precluded);

#		if there's an odd number of teams in the bracket it must be the last one, so process the bye here
		my $dummy = keys %bracket;
		my $bye_school=0;

		$error_log .= "\nI have $dummy teams in this bracket:\n\n";
		foreach my $key (keys %bracket) { 
			$error_log .=  "\t".$key." ".$entry_by_id{$key}{'code'}." with ".$entry_by_id{$key}{'wins'}." wins\n";
		}

		if ( ($school_ct/$entry_count) > .4  ) { $bye_school=$hi_school; }

		#make the pullup list
		my @pullup_array = make_pullup_array($x, \%entry_by_id, \%bracket) ;
		
		#pair the bracket, and exit if it works.  if it doesn't, pull up other teams until there's one that works
		
		my $i=0; my $pullup_tries=0; my $last_try=0; 

		do {
			#infinite loop preventer
			$n_tries++;
			if ($n_tries > (keys %entry_by_id) ) { last };
			
			#set SOP and seed for sorting
			foreach my $key ( keys %bracket ) {
				$bracket{$key}{'SOP'} = $entry_by_id{$key}{'SOP'};
				$bracket{$key}{'seed'} = $entry_by_id{$key}{'seed'};
				$bracket{$key}{'seed_nowins'} = $entry_by_id{$key}{'seed_nowins'};
			}
			
			#initialize opponent to zero
			foreach my $key ( keys %bracket ) {
				$entry_by_id{$key}{'opponent'}=0;
			}

			#do the actual pairing
			my $outcome = pairbracket($oddround, $powermatch_method, \%entry_by_id, \%bracket, \%precluded);
		
			# if it fails try the "just pair it" bracket option

			if ($outcome eq "Fail") {
				my $try_ctr = 0;
				do {
					$try_ctr++;
					$outcome = justmakeitwork(\%bracket, \%entry_by_id, $oddround, \%precluded);
					$error_log .= "justmakeitwork returned $outcome on iteration $try_ctr\n\n";					
				} until ( $outcome ne "Fail" || $try_ctr > 20 );
				if ($outcome ne "Fail") { $i = scalar(@pullup_array) + 99 ; }
			} else { 
				$i = scalar(@pullup_array) + 99 ; 
			}

			# if you get this far you can't pair the bracket, so pull up a
			# different team; this will loop back to the top and try pairing
			# again

			if ($outcome eq "Fail") { 

				#set an opponent so it doesn't try to pull them up
				foreach my $key ( keys %bracket) {
					$entry_by_id{$key}{'opponent'}=-999;
				}

				#pull up a different team
				if ( scalar(@pullup_array) > 0 ) {
					my $team_trying_to_replace = $pullup_array[$i];
					unless($last_try) { $last_try = $team_trying_to_replace; }
					delete $bracket{$last_try}; $entry_by_id{$last_try}{'opponent'} = 0 ;
					$error_log .= "\n\ni is $i and scalar is ".scalar(@pullup_array)." \n\nNow  trying to pull up a different team; protecting $entry_by_id{$last_try}{'code'}\n\n";
					
					#id team to pull up and add to the bracket
					$last_try = pullup($entry_by_id{$team_trying_to_replace}{'sidedue'}, $pullup_method, $pullup_repeat, $last_try, \%entry_by_id, \%bracket, \%precluded);
					$bracket{$last_try}{'side'} = $entry_by_id{$last_try}{'sidedue'};
					$error_log .= "Now pulling up $entry_by_id{$last_try}{'code'}\n\n";
					
#					foreach my $key ( keys %bracket ) {
#						print "\n\n".$entry_by_id{$key}{'code'}." wins=".$entry_by_id{$key}{'wins'}." seed=".$entry_by_id{$key}{'seed_nowins'}." side=".$entry_by_id{$key}{'sidedue'};
#					}

					$pullup_tries++;

					if ( $last_try == 0 ) {
						$error_log .= "No other pullup works, so restoring $entry_by_id{$team_trying_to_replace}{'code'}";
						#there's nobody to pull up, so max out tries to escape the loop
						$pullup_tries = (keys %entry_by_id); 
						#and restore to the bracket the team you were trying to replace 
						$bracket{$team_trying_to_replace}{'side'} = $entry_by_id{$team_trying_to_replace}{'sidedue'}; 
					}
				}
			}

			if ( $pullup_tries > ((keys %entry_by_id)/2) ) { 
				$i++; 
				$pullup_tries = 0; 
				$last_try = 0; 
			}
		
			
		} until ($i >= scalar(@pullup_array) );

		#clean out the detritius
		for my $key ( keys %entry_by_id ) {
			if ( $key <=0 ) { delete($entry_by_id{$key}); }
		}
		
		if ( unpaired(\%entry_by_id, \%bracket) > 0 && $x > 0 ) { 
			$error_log .= "Couldn't pair the bracket; collapsing it with the next bracket down; current bracket is $x \n\n";  
		}		

		if ( unpaired(\%entry_by_id, \%bracket) > 0 && $x == 0 && $collapse_up < $hibracket ) { 
			$error_log .= "Couldn't pair the last bracket; collapsing it with the next bracket up\n\n";
			$x++; #note it will subtract one below, so this will keep it at zero
			$collapse_up++; #counting how many brackets its rolled back to
			#unpair the next bracket up
			foreach my $key ( keys %entry_by_id ) {
				if ($entry_by_id{$key}{'wins'} <= $collapse_up ) {
					$entry_by_id{$entry_by_id{$key}{'opponent'}}{'opponent'} = 0;
					$entry_by_id{$key}{'opponent'} = 0;
				}
			}	
			#When this loops back it should repeat with the collapsed brackets; note how sub setbracket works
		}		

		if ( unpaired(\%entry_by_id, \%bracket) == 0 ) { 
			$error_log .= "Successfully paired the $x-win bracket\n\n";  
		}		

		$x--;
	}


	# PAIRINGS ARE DONE, NOW MARK WHICH TEAM IS IN WHICH BRACKET

	my $bracket_value;
	foreach my $key ( keys %entry_by_id ) {
		$bracket_value = $entry_by_id{$key}{'wins'};

		if ($entry_by_id{$entry_by_id{$key}{'opponent'}}{'wins'} > $bracket_value ) { 
			$bracket_value = $entry_by_id{$entry_by_id{$key}{'opponent'}}{'wins'};
		}

		$bracket_value = 0 unless $bracket_value; 

		$entry_by_id{$key}{'bracket'} = $bracket_value;
		$entry_by_id{$entry_by_id{$key}{'opponent'}}{'bracket'} = $bracket_value;
	}	

	#SAVE

	#make sure there are sides assigned or it won't save
	foreach my $key (keys %entry_by_id) {
		if ( $entry_by_id{$key}{'sidedue'} == 0 ) {
			$entry_by_id{$key}{'sidedue'} = 1;
			$entry_by_id{$entry_by_id{$key}{'opponent'}}{'sidedue'} = 2;
		}
	}
	
	saveit($round, \%entry_by_id);

	$round->created($now);
	$round->update;

	# SET SIDES IF IT'S AN ODD NUMBERED ROUND USING THE SERPENTINE THING

	if ($oddround == 1) {
		$m->comp("/panel/manipulate/snake_sides.mas", round_id => $round->id, from => "autopair");
	}

	#return unless $debugme;

	sub setbracket {

		# receives a bracket to fill, returns a hash with teams in that bracket
		my ($round_type, $oddround, $winbracket, $pullup_method, $pullup_repeat, $entry_by_id_ref, $bracket_ref, $precluded_ref) = @_;

		# $error_log .= " odd round:".$oddround." winbracket=".$winbracket."\n\n" ;
		my %entry_by_id = %{$entry_by_id_ref};

		foreach my $key ( keys %entry_by_id ) {
			if ($key <= 0) { delete( $entry_by_id{$key} ); }
		}
		
		$error_log .= "Starting with this many teams in entry_by_id:".(keys %entry_by_id)."\n\n";
		my %team;

		#if a high-high round, dump them all in 1 bracket and be done with it
		if ($round_type eq "highhigh") {

			foreach my $key ( keys %entry_by_id ) {
				next unless $key;
				$team{$key}{'side'} = $entry_by_id{$key}{'sidedue'}; 
				$entry_by_id{$key}{'opponent'} = -999;
			}

			return %team;
		}
		
		my $totalentries = keys %entry_by_id;	#total teams in the event; necessary to test for the last bracket	
		
		#populate the bracket

		foreach my $key ( keys %entry_by_id ) {

			if ($key <= 0) { delete( $entry_by_id{$key} ); }	
			next unless $key;
			next unless $entry_by_id{$key};

			# $error_log .= $entry_by_id{$key}{'code'}." has ".$entry_by_id{$key}{'wins'}." wins 
			# and current opponent is ".$entry_by_id{$key}{'opponent'}." \n" ;

			if ($entry_by_id{$key}{'wins'} >= $winbracket && ($entry_by_id{$key}{'opponent'} == 0 || $entry_by_id{$key}{'opponent'} == -999)) { 
				$team{$key}{'side'} = $entry_by_id{$key}{'sidedue'}; 
				$entry_by_id{$key}{'opponent'} = -999;
			}
		}

		#loop and pull up until the bracket is even
		my $bracketeven = 0;
		my $brackettries = 0;

		while ( $bracketeven == 0 ) {
	
			#count the number of tries
			$brackettries++;
		
			#need to know how many teams have already been paired so you can see if this is the last bracket			
			my $paired_already = 0;

			foreach my $key ( keys %entry_by_id ) {
				$paired_already++ if $entry_by_id{$key}{'opponent'} != 0; 
			}

			#count total, aff, and neg teams and how many are from the same school	
			my $nteams = keys %team;

			my $aff; 
			my %affschool;

			my $neg; 
			my %negschool;

			$error_log .= "\n" ;

			foreach my $key ( keys %team ) {

				$aff++ if $team{$key}{'side'} == 1;
				$error_log .= $entry_by_id{$key}{'code'}." from ".$entry_by_id{$key}{'school'}." is due aff \n" if $team{$key}{'side'} == 1;

				$neg++ if $team{$key}{'side'} == 2;
				$error_log .= $entry_by_id{$key}{'code'}." from ".$entry_by_id{$key}{'school'}." is due neg \n" if $team{$key}{'side'} == 2;

				$affschool{$entry_by_id{$key}{'school'}}++ if $team{$key}{'side'} < 2;
				$negschool{$entry_by_id{$key}{'school'}}++ if $team{$key}{'side'} == 2;

			}

			$error_log .= "\n" ;
								
			#test for an even bracket in an odd round
			if ($oddround == 1 and (int($nteams/2) == $nteams/2)) { 
				$bracketeven = 1; 
			}

			#test for an even bracket in an even round
			if ($oddround == 0 and $aff == $neg) { 
				$bracketeven = 1; 
			}

			# Make sure the school composition of the bracket makes a pairing possible

			# NOTE: Probably need to change this to hasconflict rather than
			# just school but for now I can't think of how to do that without
			# pairing the bracket
			
			my $n_hischool_aff; #number of teams in the largest school entry in the bracket
			my $hischool_key_aff; #school number of the school with the most teams in the bracket
			my $fixed_pullup_side; #the side you need to pull from if its an even round
			
			#count school with most teams in aff bracket
			foreach my $key ( keys %affschool ) {
				if ( $affschool{$key} > $n_hischool_aff ) {
					$n_hischool_aff = $affschool{$key};
					$hischool_key_aff = $key;
				}
			}

			#repeat for neg
			my $n_hischool_neg; #number of teams in the largest school entry in the bracket
			my $hischool_key_neg; #school number of the school with the most teams in the bracket

			foreach my $key ( keys %negschool ) {
				if ( $negschool{$key} > $n_hischool_neg ) {
					$n_hischool_neg = $negschool{$key};
					$hischool_key_neg = $key;
				}
			}

			# count possible opponents in opposite bracket

			my $neg_opp_for_affhischool; #negative opponents who aren't from the school in the aff bracket with the most teams
			my $aff_opp_for_neghischool; #negative opponents who aren't from the school in the aff bracket with the most teams

			# Palmer fixed the below; it used to add an opponent per school by
			# using ++ but it should instead add one per opponent and thus the
			# += $negschool{$key}.  

			foreach my $key ( keys %negschool ) {
				$neg_opp_for_affhischool += $negschool{$key} if $key != $hischool_key_aff;
			}

			foreach my $key ( keys %affschool ) {
				$aff_opp_for_neghischool += $affschool{$key} if $key != $hischool_key_neg;
			}
			
			if ( $oddround ) {

				if ( ($nteams - $n_hischool_aff) < $n_hischool_aff ) { 
					$error_log .= "\n\nToo many teams from one school; pulling up more:";
					$bracketeven = 0;
				}

			} else {

				$error_log .= "\n\nToo many teams from one school on one side of the bracket; pulling up more:" ; 

				if ( $neg_opp_for_affhischool < $n_hischool_aff ) { 
					$bracketeven = 0; 
					$fixed_pullup_side = 2; 
					$error_log .= "\n\nI have $neg_opp_for_affhischool opponents on neg for $n_hischool_aff affs from $hischool_key_aff \n\n" ;
				}

				if ( $aff_opp_for_neghischool < $n_hischool_neg ) { 
					$bracketeven = 0; 
					$fixed_pullup_side = 1; 
					$error_log .= "\n\nI have $aff_opp_for_neghischool opponents on aff for $n_hischool_neg negs from $hischool_key_neg \n\n" ;
				}
			}

#			$error_log .= "oddround:".$oddround." aff:".$aff." neg:".$neg." bracketeven:".$bracketeven."\n\n" ;
			
			#test for this being the last bracket and needing a bye
			$bracketeven = 1 if $paired_already == $totalentries;
			
#			Tab::debuglog( "oddround:".$oddround." n teams:".$nteams." bracket even marker:".$bracketeven."");
			
			#exit if the bracket is even
			if ( $bracketeven == 1 ) { last; }
			
			#kick into "make it work" mode if can't even the bracket
			if ( $brackettries > 100 ) { last; }
		
			#bracket not even, so pull up
			my $sidetopull = 0;

			$sidetopull = 1 if ($oddround == 0 && $aff < $neg);
			$sidetopull = 2 if ($oddround == 0 && $aff > $neg);
			$sidetopull = $fixed_pullup_side if ($oddround == 0 && $fixed_pullup_side);
			
			my $pullup = pullup($sidetopull, $pullup_method, $pullup_repeat, 0, $entry_by_id_ref, $bracket_ref, $precluded_ref);

			$error_log .= "pulled up ". $pullup." ".$entry_by_id{$pullup}{'code'}."\n\n" ;

			$team{$pullup}{'side'} = $entry_by_id{$pullup}{'sidedue'} ;
			$entry_by_id{$pullup}{'opponent'} = -999; #stores a temporary opponent so they won't get pulled up again

		}

		$error_log .= "There are now ".(keys %team)." teams in the bracket ..." ; 
#		$error_log .= "exiting setbracket sub\n\n" ;		
		return %team;
	}

	sub pullup {

		#identifies which side you need to pull up; zero means either is OK
		my ($side, $pullup_method, $pullup_repeat, $pullup_protect, $entry_by_id_ref, $bracket_ref, $precluded_ref) = @_;
		my %entry_by_id = %{$entry_by_id_ref};
		
#		$error_log .= "Finding a pullup using method=".$pullup_method."\n\n" ;
#		$error_log .= "protecting $entry_by_id{$pullup_protect}{'code'} from pullup\n\n" ;

		#If pullup_protect is GT 0 it means that you tried pulling one team up and it didn't work
		#so this sets a flag that won't allow a pullup until you get past the last team you tried to pull up
		my $protect_flag = 0;
		unless ($pullup_protect) { 
			$protect_flag = 1; 
		}
		
		my @order;

		#default sortorder is sop
		@order = sort { $entry_by_id{$b}->{'wins'} <=> $entry_by_id{$a}->{'wins'} || $entry_by_id{$b}->{'oppseed'} <=> $entry_by_id{$a}->{'oppseed'} || $entry_by_id{$b}->{'seed'} <=> $entry_by_id{$a}->{'seed'} } keys(%entry_by_id);

		if ($pullup_method eq "oppwin") {
			@order = sort { $entry_by_id{$b}->{'wins'} <=> $entry_by_id{$a}->{'wins'} || $entry_by_id{$a}->{'opp_wins'} <=> $entry_by_id{$b}->{'opp_wins'} || $entry_by_id{$b}->{'seed_nowins'} <=> $entry_by_id{$a}->{'seed_nowins'} } keys(%entry_by_id);
		}
		
		if ($pullup_method eq "middle" or $pullup_method eq "lowseed" ) {
			@order = sort { $entry_by_id{$b}->{'wins'} <=> $entry_by_id{$a}->{'wins'} || $entry_by_id{$a}->{'seed'} <=> $entry_by_id{$b}->{'seed'} } keys(%entry_by_id);
		}

		my $oppwins=0; my $bracket=0;
		my @candidates; 
		
		foreach my $order_team ( @order ) {

			my $key=$order_team;
			
			#make sure they are on the right side of the bracket
			my $sidematch = 1;
			$sidematch = 0 if $side>0 and $entry_by_id{$key}{'sidedue'} != $side;

			#make sure they can debate at least one team in the bracekt
			my $allconflicts = 0;
			foreach my $key2 ( keys %{$bracket_ref} ) {
				my $oktodebate=1;
				if ( hasconflict($key, $key2, $precluded_ref) == 1 ) { $oktodebate = 0; }
				if ( $side > 0 and $entry_by_id{$key}{'sidedue'} == ${$bracket_ref}{$key2}{'side'} ) { $oktodebate = 0;  }
				if ( $oktodebate == 1 ) { $allconflicts = 0; last; }
			}
			
#			$error_log .= $entry_by_id{$key}{'code'}." wins:".$entry_by_id{$key}{'wins'}." Side match: ".$sidematch." opponent:".$entry_by_id{$key}{'opponent'}." all conflicts:".$allconflicts."\n\n" ;

			if ( $entry_by_id{$key}{'opponent'} == 0 and $sidematch == 1 and $allconflicts == 0 and $pullup_method eq "sop" and $protect_flag == 1 ) {
				$error_log .= "Pulled up ".$entry_by_id{$key}{'code'}." with ".$entry_by_id{$key}{'wins'}." wins ";
				$error_log .= " and ".$entry_by_id{$key}{'oppseed'}." avg opp seed\n\n"; 
				return $key;
			}

			#should only get this far if you are using the oppwin method

			#don't pull up a second time depending on settings; sop ignores this, so don't calcualte it unless you get this far
			#pullups are added and subtracted, so if you are pulled up and pulled down your setting is zero.  Positive numbers mean
			#you've been pulled up more than you've been pulled down.

			my $second_pullup=0;

			if ( $pullup_repeat and $entry_by_id{$key}{'pullup'} > 0 ) { 
				$error_log .= "Not pulling up $entry_by_id{$key}{'code'} b/c they were pulled up before\n\n" ;
				$second_pullup = 1; 
			}
		
			if ( $entry_by_id{$key}{'opponent'} == 0 and $sidematch == 1 and $allconflicts == 0 and $second_pullup == 0 and $pullup_method ne "sop" and $protect_flag == 1 ) {
				if ( $oppwins == 0 ) { $oppwins = $entry_by_id{$key}{'opp_wins'}; $bracket = $entry_by_id{$key}{'wins'}; }
				if ( $entry_by_id{$key}{'wins'} == $bracket and ( $entry_by_id{$key}{'opp_wins'} == $oppwins or $pullup_method ne 'sop' ) ) {
					$error_log .= "Marking ".$entry_by_id{$key}{'code'}." with ".$entry_by_id{$key}{'wins'}." wins ";
					$error_log .= " and ".$entry_by_id{$key}{'opp_wins'}." opp_wins as a pullup candidate\n\n"; 
					$candidates[scalar(@candidates)] = $key; 
				}
			}
		
			if ( $key == $pullup_protect ) { $protect_flag = 1; }		
			
		}
		
		#pulls from the middle if there are multiple candidates

#		For debugging only:
#		$error_log .= "\n\nCANDIDATES FOR PULLUP\n\n" ;
#		my $x;
#		foreach my $candidate (@candidates) {
#			$x++;
#			$error_log .= $x." ".$entry_by_id{$candidate}{'code'}." ".$entry_by_id{$candidate}{'code'}." ".$entry_by_id{$candidate}{'seed'}."\n\n" ;
#		}
		
		if ( $pullup_method eq "lowseed" ) {
			$error_log .= "...pulling up lowest seed who is ".$entry_by_id{$candidates[$#candidates]}{'code'}."\n\n" ;
			return $candidates[$#candidates];
		}

		if ( $pullup_method eq "oppwin" or $pullup_method eq "middle" ) {
			my $num = int((scalar(@candidates)/2) + .5); $num--;
			if ( scalar(@candidates) == 1 ) { $num = 0; }
			$error_log .= "There are ".scalar(@candidates)." teams with $oppwins opp wins in the $bracket win bracket" ; 
			$error_log .= "...pulling up ".$entry_by_id{$candidates[$num]}{'code'};
			$error_log .= " who has ".$entry_by_id{$candidates[$num]}{'wins'}." wins";
			$error_log .= " and ".$entry_by_id{$candidates[$num]}{'opp_wins'}." avg opp wins " ; 

			return $candidates[$num];
		}

		#if it gets this far something has failed
		$error_log .= "Exiting pullup sub without finding a team to pull up\n\n" ;
		return 0;
	}

	sub make_pullup_array {

		#receives a bracket list and full entry list, returns an array of teams pulled up
		
		my ($win_bracket, $entry_by_id_ref, $bracket_ref) = @_;

		my %entry_by_id = %{$entry_by_id_ref};
		
		my @pullup_list;
		
		foreach my $key ( keys (%{$bracket_ref}) ) {
			if ( ${$entry_by_id_ref}{$key}{'wins'} < $win_bracket ) {
				push @pullup_list, $key;
			}
		}
		
		return @pullup_list;

	}
		
	sub count_possible_opponents {
	
		#variables are passed by ref; this gives them identical local names
		my ($entry_by_id_ref, $precluded_ref, $oddround_ref, $min_possible_opponents_ref, $bracket_ref) = @_;
		my %entry_by_id = %{$entry_by_id_ref};
		my %precluded = %{$precluded_ref};
		my $oddround = ${$oddround_ref};
		my $min_possible_opponents = ${$min_possible_opponents_ref};
		my %bracket = %{$bracket_ref};
		
		foreach my $key ( keys %entry_by_id ) {
			$entry_by_id{$key}{'possible_opponents'} = 0;
		}
		
		foreach my $key ( keys %bracket ) {

			$entry_by_id{$key}{'random'} = int(rand(1000))+1;

			KEY:
			foreach my $key2 ( keys %bracket ) {
				next if (hasconflict($key, $key2, \%precluded) == 1);
				next if ($key == $key2);
				next if ( $oddround == 0 and $entry_by_id{$key}{'sidedue'} == $entry_by_id{$key2}{'sidedue'} );
				next if ( $entry_by_id{$key2}{'opponent'} > 0 );
				$entry_by_id{$key}{'possible_opponents'}++;
			}
		
			$entry_by_id{$key}{'possible_opponents'} = 0 unless $entry_by_id{$key}{'possible_opponents'};
		
			if ( $entry_by_id{$key}{'possible_opponents'} < $min_possible_opponents ) { 
				$min_possible_opponents = $entry_by_id{$key}{'possible_opponents'}; 
			}

		}

		${$min_possible_opponents_ref} = $min_possible_opponents;
		
	}
	
	sub pairbracket {

		my ($oddround, $powermatch_method, $entry_by_id_ref, $bracket_ref, $precluded_ref) = @_;

		my $nteams = keys %{$bracket_ref};	#figure out how many teams are in the bracket; you'll use this below	

		bracket_pair($oddround, $powermatch_method, 0, $entry_by_id_ref, $bracket_ref, $precluded_ref);
			
		#make sure they're all paired, and if they are, bail
		my $unpaired_teams = unpaired($entry_by_id_ref, $bracket_ref);
		
		if ($unpaired_teams == 0) { return "Success"; }

		#Now you know it got to the last 2 teams and got stuck, so you have to roll back some existing pairings
		#Sort the bracket in reverse order, and unpair one debate at a time.  Then pair the problem team, and then
		#finish the bracket with a normal hi-lo pair.

		my $problemchild; my $pc_sop = -100;
		foreach my $key ( sort { ${$bracket_ref}{$a}->{'SOP'} <=> ${$bracket_ref}{$b}->{'SOP'} } keys(%{$bracket_ref}) ) {
			if (${$entry_by_id_ref}{$key}{'opponent'} == 0 and ${$entry_by_id_ref}{$key}{'SOP'} > $pc_sop ) { 
				$problemchild = $key; $pc_sop = ${$entry_by_id_ref}{$key}{'SOP'};
				if ($debugme) { $error_log .= "Marked ".${$entry_by_id_ref}{$key}{'code'}." as problem child\n\n"; }
			}
		}

		if ($debugme) { $error_log .= "Problem child: ".${$entry_by_id_ref}{$problemchild}{'code'}." ".$problemchild."\n\n"; }
		
		my $n_bracket_tries = 0;
		my $n_problemchild_tries;
		
		my $ok_to_start = 0;
		
		foreach my $key ( sort { ${$bracket_ref}{$b}->{'SOP'} <=> ${$bracket_ref}{$a}->{'SOP'} } keys(%{$bracket_ref}) ) {
			
			if ($debugme) {
				$error_log .= "Problem child: ".$problemchild." ".${$entry_by_id_ref}{$problemchild}{'code'}." key=".$key." SOP=".${$entry_by_id_ref}{$key}{'SOP'} ;
				$error_log .= "trying to unpair:".${$entry_by_id_ref}{$key}{'code'} ;
				$error_log .= " ok_to_start marker:".$ok_to_start."\n\n" ;
			}
			
			if ( $problemchild == $key ){ $ok_to_start = 1; }

			if ($key != $problemchild and ${$entry_by_id_ref}{$key}{'opponent'} > 0 and $ok_to_start == 1 ) {
				if ($debugme) { 
					$error_log .= "Unpairing ".${$entry_by_id_ref}{$key}{'code'}." vs ".${$entry_by_id_ref}{${$entry_by_id_ref}{$key}{'opponent'}}{'code'}."\n\n"; 
				}

 				${$entry_by_id_ref}{${$entry_by_id_ref}{$key}{'opponent'}}{'opponent'} = 0;
				${$entry_by_id_ref}{$key}{'opponent'} = 0;
				
				#now pair the problem child		
				bracket_pair($oddround, $powermatch_method, $problemchild, $entry_by_id_ref, $bracket_ref, $precluded_ref);
				
				#now try to finish the bracket
				bracket_pair($oddround, $powermatch_method, 0, $entry_by_id_ref, $bracket_ref, $precluded_ref);
				
				#if it worked, bail, and otherwise loop back and undo the next pairing
				if (unpaired($entry_by_id_ref, $bracket_ref) == 0) { last; }			
			}
		
		}
			
		if (unpaired($entry_by_id_ref, $bracket_ref) == 0) { return "Success"; }			
		return "Fail";	
				
	}
	
	sub bracket_pair {
	
		my ($oddround, $powermatch_method, $problemchild, $entry_by_id_ref, $bracket_ref, $precluded_ref) = @_;	

		$error_log .= "In bracket pairing sub\n\n"; 
		$error_log .= "Problem child: $problemchild ${$entry_by_id_ref}{$problemchild}{'code'}" if $problemchild;
		
		# start from the top of the {$bracket_ref}, and then go backwards up
		# from the bottom of the {$bracket_ref} looking for an opponent who
		# fits pair if it's a match
		
		# major conceptual idea:  WHY do we start at the top of the AFF
		# {$bracket_ref}?  What if the top of the NEG {$bracket_ref} is a lower
		# seed?  In fact, why don't we do the high/low starting with the
		# highest unpaired seed REGARDLESS of whether they are AFF or NEG?  The
		# coding is easier AND its more consistent with the HiLo pair
		# philosophy
		 
		# for future reference:  see also Tie::SortHash
		# foreach my $key ( sort { ${$bracket_ref}{$a}->{'SOP'} <=> ${$bracket_ref}{$b}->{'SOP'} || ${$bracket_ref}{$a}->{'seed'} <=> ${$bracket_ref}{$b}->{'seed'} } keys(%{$bracket_ref}) ) 

		my @sorted_bracket;
		my @opp_bracket;

		if ($powermatch_method eq "sop") {
			@sorted_bracket = sort { ${$bracket_ref}{$a}->{'SOP'} <=> ${$bracket_ref}{$b}->{'SOP'} || ${$bracket_ref}{$a}->{'seed'} <=> ${$bracket_ref}{$b}->{'seed'} } keys(%{$bracket_ref});
			@opp_bracket = sort { ${$bracket_ref}{$b}->{'SOP'} <=> ${$bracket_ref}{$a}->{'SOP'} || ${$bracket_ref}{$b}->{'seed'} <=> ${$bracket_ref}{$a}->{'seed'} } keys(%{$bracket_ref});
	
		}
	
		if ($powermatch_method eq "seed") {
			@sorted_bracket = sort { ${$bracket_ref}{$a}->{'seed_nowins'} <=> ${$bracket_ref}{$b}->{'seed_nowins'} } keys(%{$bracket_ref});
			@opp_bracket = sort { ${$bracket_ref}{$b}->{'seed_nowins'} <=> ${$bracket_ref}{$a}->{'seed_nowins'} } keys(%{$bracket_ref});
		}

		if ($powermatch_method eq "highhigh") {
			@sorted_bracket = sort { ${$bracket_ref}{$a}->{'seed'} <=> ${$bracket_ref}{$b}->{'seed'} } keys(%{$bracket_ref});
			@opp_bracket = sort { ${$bracket_ref}{$a}->{'seed'} <=> ${$bracket_ref}{$b}->{'seed'} } keys(%{$bracket_ref});
		}
						

		#if there's a bye, assign it to the lowest seed before doing the hilo; otherwise, the top team in the
		#bracket will get the bye and not the bottom team
		if ( ${$bracket_ref}{-1} and ${$entry_by_id_ref}{-1}{'opponent'}==0 ) {
			$error_log .= "There's an unpaired bye in this here bracket!\n\n" ;
			foreach my $element (reverse @sorted_bracket) {
				if (${$entry_by_id_ref}{$element}{'opponent'} == 0 and $element != -1 and ${$entry_by_id_ref}{-1}{'sidedue'} != ${$entry_by_id_ref}{$element}{'sidedue'} and hasconflict($element, -1, $precluded_ref) == 0 ) {
					${$entry_by_id_ref}{$element}{'opponent'} = -1;
					${$entry_by_id_ref}{-1}{'opponent'} = $element;
					$error_log .= "Just assigned that there bye to ${$entry_by_id_ref}{$element}{'code'}\n\n" ;
					last;
				}
			}
		}
		
		foreach my $bracket_member ( @sorted_bracket ) {

			my $key = $bracket_member;
			
			#if it's an even round, only loop through the aff
			my $sidemarker = 1;
			$sidemarker = 0 if ($oddround == 0 and ${$bracket_ref}{$key}{'side'} == 2);

			#if there's a problem child, only find an opponnet for that team

			if ($problemchild) { 
				if ($key != $problemchild) { $sidemarker = 0; } 
			}
			
			if ( ${$entry_by_id_ref}{$key}{'opponent'} == 0 and $sidemarker == 1 and $key > 0) {

				$error_log .= "\n\nEntry: ".${$entry_by_id_ref}{$key}{'code'}." seed: ".${$entry_by_id_ref}{$key}{'seed'}." key:".$key." opponent:".${$entry_by_id_ref}{$key}{'opponent'}." sidemarker:".$sidemarker."\n\n" ;

				OPP:
				foreach my $opp_bracket_member ( @opp_bracket ) {

					$error_log .= "\t";

					next unless $opp_bracket_member;
					my $oppkey = $opp_bracket_member;
					next unless ${$entry_by_id_ref}{$oppkey};

					if (${$entry_by_id_ref}{$oppkey}{'opponent'} != 0) {
						$error_log .= ${$entry_by_id_ref}{$oppkey}{'code'}." already paired\n" ;
						next OPP;
					}

					if (hasconflict($key, $oppkey, $precluded_ref) == 1) {
						$error_log .= ${$entry_by_id_ref}{$oppkey}{'code'}." teams have conflict\n" ;
						next OPP;
					}

					if ($key == $oppkey) {
						$error_log .= " Same team.\n" ;
						next OPP;
					} 

					if ( $oddround == 0 and ${$bracket_ref}{$key}{'side'} == ${$bracket_ref}{$oppkey}{'side'} ) {
						$error_log .= ${$entry_by_id_ref}{$oppkey}{'code'}." sides don't match\n" ;
						next OPP;
					} 

					${$entry_by_id_ref}{$key}{'opponent'} = $oppkey;
					${$entry_by_id_ref}{$oppkey}{'opponent'} = $key;
					$error_log .= "WORKED!  Pairing ".${$entry_by_id_ref}{$key}{'code'}." key $oppkey vs ".${$entry_by_id_ref}{$oppkey}{'code'}."\n\n\n\n" ;
					last;
				}
			}
		}
	
	}

	sub unpaired {

		my ($entry_by_id_ref, $bracket_ref) = @_;
	
		my $returnvalue = 0;
		
		foreach my $key ( keys %{$bracket_ref} ) {
			if (${$entry_by_id_ref}{$key}{'opponent'} == 0 or ${$entry_by_id_ref}{$key}{'opponent'} == -999) { $returnvalue++; }
		}

		#Tab::debuglog( "unpaired: ".$returnvalue."");
		
		return $returnvalue;
		
	}

	sub hasconflict {

		my ($team1, $team2, $precluded_ref) = @_;	

		my $returnvalue = 0;

		if (${$precluded_ref}{$team1}{$team2} == 1) { $returnvalue = 1; }

		if (${$precluded_ref}{$team2}{$team1} == 1) { $returnvalue = 1; }
		
		#if ( $returnvalue == 1) { $error_log .= "conflict between ".$team1." and ".$team2."\n\n" ; }
		
		return $returnvalue;
		
	}
	
	sub erase_current_round {

		my $round = shift;

		#Out with the old.	

		Tab::BallotValue->set_sql( delete_round => "
			delete ballot_value from ballot_value, ballot, panel
			where panel.round = ?
			and panel.id = ballot.panel
			and ballot.id = ballot_value.ballot
		");

		Tab::Ballot->set_sql( delete_round => "
			delete ballot from ballot, panel
			where panel.round = ?
			and panel.id = ballot.panel
		");

		Tab::Panel->set_sql( delete_round => "
			delete from panel where round = ?
		");

		Tab::BallotValue->sql_delete_round->execute($round->id);	
		Tab::Ballot->sql_delete_round->execute($round->id);
		Tab::Panel->sql_delete_round->execute($round->id);

	}

	sub justmakeitwork {

		my ($bracket_ref, $entry_by_id_ref, $oddround, $precluded_ref) = @_;
		
		my %bracket = %{$bracket_ref};
		my %entry_by_id = %{$entry_by_id_ref};
		my %precluded = %{$precluded_ref};
	
		# NOTE: This counts the number of possible opponents and then pairs a
		# high-high based on that it may be possible to fuse this into another
		# routine, but there's no high-high pairing here now, so I'm not
		
		# normal powermatching has failed, so this just tries to pair the
		# bracket

		$error_log .= "\n\nin the 'just make it work' disaster pairing mode\n\n" ;
		$error_log .= " Bracket has ".scalar(keys %bracket)." teams\n\n" ;
	
		# clear any current opponents & count the number of possible opponents
		
		foreach my $key ( keys %bracket ) {
			$entry_by_id{$key}{'opponent'} = 0;
		}
		my $min_possible_opponents; my $pair_fail=0;
		
		#now pair it
		do {
			#re-count number of possible opponents
			count_possible_opponents(\%entry_by_id, \%precluded, \$oddround, \$min_possible_opponents, \%bracket);
	
			foreach my $key ( sort { $entry_by_id{$a}->{'possible_opponents'} <=> $entry_by_id{$b}->{'possible_opponents'} || $entry_by_id{$a}->{'random'} <=> $entry_by_id{$b}->{'random'} } keys(%bracket) ) {
	
				$error_log .= "Possible opponents for: ".$entry_by_id{$key}{'code'}." is ".$entry_by_id{$key}{'possible_opponents'}." \n\n";			
				next unless $key;
				next unless $entry_by_id{$key};
	
				#if it's an even round, only loop through the aff -- but WHY?  Disabled!  What are you doing, past self!
				#my $sidemarker = 1;
				#if ($oddround == 0 and $bracket{$key}{'side'} == 2) { $sidemarker = 0; }
				#if ( $entry_by_id{$key}{'opponent'} == 0 and $sidemarker == 1 ) {
	
				if ( $entry_by_id{$key}{'opponent'} == 0 ) {

					OPP:
					foreach my $oppkey (sort { $entry_by_id{$a}->{'possible_opponents'} <=> $entry_by_id{$b}->{'possible_opponents'} } keys(%bracket)) {
	
						next unless $oppkey;
						next unless $entry_by_id{$oppkey};
	
						$error_log .= "Trying to pair ".$entry_by_id{$key}{'code'}." vs ".$entry_by_id{$oppkey}{'code'}.": " ;
	
						if ($entry_by_id{$oppkey}{'opponent'} != 0) { 
							$error_log .= $entry_by_id{$oppkey}{'code'}." has opponent already.\n" ;
							next OPP;
						}
	
						if (hasconflict($key, $oppkey, $precluded_ref) == 1) { 
							$error_log .= "Teams have conflict.\n" ;
							next OPP;
						} 
	
						next OPP if ($key == $oppkey);
	
						if ( $oddround == 0 and $bracket{$key}{'side'} == $bracket{$oppkey}{'side'} ) { 
							$error_log .= "Teams on same side\n" ; 
							next OPP;
						} 
	
						$entry_by_id{$key}{'opponent'} = $oppkey;
						$entry_by_id{$oppkey}{'opponent'} = $key;
						$error_log .= "pairing WORKED!  Pairing ".$entry_by_id{$key}{'code'}." vs ".$entry_by_id{$oppkey}{'code'}."\n\n" ;
						last;
					}
					
					if ($entry_by_id{$key}{'opponent'} == 0 ) { 
						$error_log .= "\n\nFAILED TO FIND OPPONENT FOR ".$entry_by_id{$key}{'code'}."\n\n";
						$pair_fail = 1; }
				}
			}

		} until ( unpaired(\%entry_by_id, \%bracket) == 0 || $pair_fail == 1 ); #bail if all paired or couldn't pair last team
		
		if ( unpaired(\%entry_by_id, \%bracket) == 0 ) { return "Success"; }
		return "Fail";
	}

	sub saveit {

		my ($round, $entry_by_id_ref) = @_;
		my %entry_by_id = %{$entry_by_id_ref};

		my $letter = 1;
		$letter = "A" if $round->event->setting("panel_labels") eq "letters";

		my %done;

		foreach my $key ( keys %entry_by_id ) {		

			next unless $entry_by_id{$key}{'opponent'};
			next if $entry_by_id{$key}{'opponent'} == -999;
			next if $done{$key}++;

			if ( $key > -1 and ($entry_by_id{$key}{'sidedue'} == 1 or $entry_by_id{$key}{'opponent'} == -1 ) ) {

				my $bye = 0;
				$bye = 1 if $entry_by_id{$key}{'opponent'} == -1; 

				my $panel = Tab::Panel->create({
					round   => $round->id,
					bye     => $bye,
					letter  => $letter,
					flight  => 1,
					bracket => $entry_by_id{$key}{'bracket'}
				});
				
				#save aff ballot
				my $pullup = 0;
				my $opponent = $entry_by_id{$key}{'opponent'};
				$pullup = 1 if $entry_by_id{$key}{'wins'} < $entry_by_id{$opponent}{'wins'};
				
				Tab::Ballot->create({
					panel  => $panel->id,
					judge  => 0,
					bye    => $bye,
					audit  => $bye,
					entry  => $key,
					side   => $entry_by_id{$key}{'sidedue'},
					seed   => $entry_by_id{$key}{'seed'},
					pullup => $pullup
				});
				
				#save neg ballot

				if ( $bye == 0 and $opponent > 0 ) {

					my $pullup = 0;
					$pullup = 1 if $entry_by_id{$key}{'wins'} > $entry_by_id{$opponent}{'wins'}; 

					Tab::Ballot->create({
						panel  => $panel->id,
						judge  => 0,
						bye  => $bye,
						entry  => $opponent,
						side   => $entry_by_id{$opponent}{'sidedue'},
						seed   => $entry_by_id{$opponent}{'seed'},
						pullup => $pullup
					});
				}
				
				$letter++;

			}		
		}
	}

	$error_log .= "\n\n" ;

	if ($debugme) { 

		#printing the sorted seeds; just for debugging
		$m->print("<div class='blankfull'>");
		$m->print("<table id=\"bracketed\">");
		$m->print("<tr class=\"smallish yellowrow\"><th>key</th><th>code</th><th>school</th><th>wins</th><th>seed</th><th>oppseed</th><th>oppWins</th><th>SOP</th><th>sideddue</th><th>opponent</th><th>opp code</th></tr>");

		my $switch; 

		foreach my $key (sort {$entry_by_id{$a}->{'seed'} <=> $entry_by_id{$b}->{'seed'}} keys(%entry_by_id)) {

			if ($key and $key > -1) {
				print "<tr class=\"odd\">" if $switch % 2;
				print "<tr class=\"even\">" unless $switch++ % 2;
				print "<td>".$key."</td>";
				print "<td>".$entry_by_id{$key}{'code'}."</td>";
				print "<td>".$entry_by_id{$key}{'school'}."</td>";
				print "<td>".$entry_by_id{$key}{'wins'}."</td>";
				print "<td>".$entry_by_id{$key}{'seed'}."</td>";
				print "<td>".sprintf("%.3f", $entry_by_id{$key}{'oppseed'})."</td>";
				print "<td>".sprintf("%.3f", $entry_by_id{$key}{'opp_wins'})."</td>";
				print "<td>".sprintf("%.3f", $entry_by_id{$key}{'SOP'})."</td>";
				print "<td>".$entry_by_id{$key}{'sidedue'}."</td>";
				print "<td>".$entry_by_id{$key}{'opponent'}."</td>";
				print "<td>".$entry_by_id{$entry_by_id{$key}{'opponent'}}{'code'}."</td>";
				print "</tr>";
			}
		}

		print "</table>";
		print "\n\n";
		print "<table cellpadding='3' width='100%' class='tablesorter'>";
		print "<tr><td>Aff</td><td>W-Sd-SOP</td><td>Neg</td><td>W-Sd-SOP</td></tr>";

		foreach my $key (sort { $entry_by_id{$b}->{'bracket'} <=> $entry_by_id{$a}->{'bracket'} || $entry_by_id{$a}->{'SOP'} <=> $entry_by_id{$b}->{'SOP'} } keys(%entry_by_id)) {
			if ($key and ($entry_by_id{$key}{'sidedue'} == 1 or $entry_by_id{$key}{'opponent'} == -1 ) ) {
				print "<tr>"; 
				print "<td>".$entry_by_id{$key}{'code'}."</td>";
				print "<td>".$entry_by_id{$key}{'wins'}."-".$entry_by_id{$key}{'seed'}."-".sprintf("%.3f", $entry_by_id{$key}{'SOP'})."</td>" ;
				my $key2 = $entry_by_id{$key}{'opponent'};
				print "<td>".$entry_by_id{$key2}{'code'}."</td>";
				print "<td>".$entry_by_id{$key2}{'wins'}."-".$entry_by_id{$key2}{'seed'}."-".sprintf("%.3f", $entry_by_id{$key2}{'SOP'})."</td>";
				print "</tr>";
			}
		}
		print "</table>" ;

	}

	$error_log .= "Total spots paired including bye:".$entry_count."\n\n" ;
	$x = 0;

	foreach my $key ( keys %entry_by_id ) {
		if ( $entry_by_id{$key} != 0 ) { $x++; }
	}

	$error_log .= "Total teams paired with opponent including bye:".$x."\n\n" ;
	$error_log .= "All done and saved!\n\n" ;

	undef $oddround;

	# Test that it's worked, and it it hasn't, kick it over to Palmer's thing	
	my $allpaired = 1;
	$error_log .= "checking for round pair success.\n\n" ;

	foreach my $key ( keys %entry_by_id ) {

		$error_log .= $key." opponent is ".$entry_by_id{$key}{'opponent'}."\n\n" ;

		if ( $entry_by_id{$key}{'opponent'} == 0 or $entry_by_id{$key}{'opponent'} == -999 ) { 
			$allpaired = 0; 
		}
	}

#	Tab::debuglog("I am here. Allpaired is $allpaired and error log is $error_log");

	if ( $allpaired == 1 ) { 
		$error_log .= "<strong>Round successfully paired.</strong>\n\n"; 

		Tab::debuglog("Error log $error_log");

	} else { 

		$error_log .= "Round pair failed -- kicking into disaster mode\n\n";

		if ($debugme) {

			undef $debugme;
			$m->print("<h2>Error Log Output</h2>");
			$m->print($error_log);
			$m->print("</div>");
			undef $error_log;
			$m->flush_buffer;

		} else { 
			
			my $err = "Round pairing failed.  You must have some strange occurrence in your brackets.  Try resolving it with the manual powermatcher at right."; 

			Tab::debuglog("Emailing error handler with $error_log ");
	
			#Harass Bruschke about it so he feels others' pain: 

			my $mail = MIME::Lite->new
				( 	
					From    => 'error-handler@tabroom.com',
					To      => 'errors@tabroom.com,jbruschke@gmail.com',
					Subject => 'Dammit, Bruschke, your powermatcher screwed up again',
					Data    => $error_log,
				);

			$mail->send;

			undef $error_log;
			$m->redirect("/panel/schemat/show.mhtml?round_id=$round&err=$err");

		}
	}

	return;


</%perl>
