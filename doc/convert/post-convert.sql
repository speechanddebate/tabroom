alter table account add change_deadline datetime null;
update judge set chapter_judge = 0 where chapter_judge is null;
update judge set account = 0 where account is null;

alter table judge_hire add rounds int;

alter table ballot modify side char;
update ballot set side=1 where side="A";
update ballot set side=2 where side="N";
alter table ballot modify side tinyint;

alter table tiebreak_set add type varchar(15);
update tiebreak_set set type="Team" where type is null;

