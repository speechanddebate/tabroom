
alter table student_vote add tag enum("nominee", "rank") not null default "rank" after id;
alter table student_vote change rank value int;

update tiebreak set name = "student_rank" where name = "student_vote";
update tiebreak set name = "student_rank" where name = "student_ranks";
update tiebreak set name = "student_nominee" where name = "student_ballot";


    delete tiebreak.*, tiebreak_set.*

        from tiebreak, tiebreak_set, tourn
        where tourn.start > "2019-09-01 00:00:00"
        and tourn.id = tiebreak_set.tourn
        and tiebreak_set.name = "Debate Final NSDA Points"
        and tiebreak_set.id = tiebreak.tiebreak_set

