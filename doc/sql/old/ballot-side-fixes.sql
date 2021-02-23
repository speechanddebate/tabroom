
	alter table ballot modify speakerorder int not null default 0;
	alter table ballot modify entry int not null default 0;
	alter table ballot modify panel int not null default 0;

	update score, ballot            
		set score.speech = ballot.speechnumber          
		where score.speech is null              
			and ballot.speechnumber > 0
			and ballot.id = score.ballot;

    update IGNORE
        score, ballot, ballot b2
        set score.ballot = ballot.id
        where score.ballot = b2.id
			and b2.entry = ballot.entry
			and b2.judge = ballot.judge
			and b2.panel = ballot.panel
			and b2.id > ballot.id;

    delete score
        from score, ballot, score s2, ballot b2
        where score.ballot = ballot.id
            and s2.ballot = b2.id
            and b2.id < ballot.id
            and b2.judge = ballot.judge
            and b2.entry = ballot.entry
            and score.tag = s2.tag
            and score.student = s2.student
            and score.id != s2.id
			and ballot.id != b2.id;

	delete from panel where round = 0;
	delete from panel where round is null;
	
	delete from panel  where not exists (select round.id from round where round.id = panel.round);
	delete from ballot where not exists (select panel.id from panel where panel.id = ballot.panel);
	delete from ballot where not exists (select entry.id from entry where entry.id = ballot.entry);
	delete from score  where not exists (select ballot.id from ballot where ballot.id = score.ballot);

    delete ballot.*
    from ballot, ballot b2
    where ballot.entry = b2.entry
		and ballot.judge = b2.judge
		and ballot.panel = b2.panel
		and ballot.id != b2.id
		and not exists (
			select score.id from score 
			where score.ballot = ballot.id
		);


