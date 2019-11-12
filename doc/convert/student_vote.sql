
alter table student_vote add tag enum("nominee", "rank") not null default "rank" after id;
alter table student_vote change rank value int;

update tiebreak set name = "student_rank" where name = "student_vote";
update tiebreak set name = "student_rank" where name = "student_ranks";
update tiebreak set name = "student_nominee" where name = "student_ballot";


