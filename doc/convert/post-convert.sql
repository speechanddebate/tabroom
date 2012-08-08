
alter table panel add started datetime;

alter table judge_hire add rounds_accepted int;

create index account on judge(account);
create index account on student(account);
create index account on chapter_judge(account);
create index chapter_judge on judge(chapter_judge);
create index chapter on chapter_judge(chapter);


