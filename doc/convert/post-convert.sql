alter table account add change_deadline datetime null;
update judge set chapter_judge = 0 where chapter_judge is null;
update judge set account = 0 where account is null;

