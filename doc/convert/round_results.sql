
alter table round add post_primary tinyint after post_results;
alter table round add post_secondary tinyint after post_primary;
alter table round add post_feedback tinyint after post_secondary;


delete from round_setting where tag = "motion" and value="text" and value_text = "";
delete from round_setting where tag = "motion" and value="text" and value_text is NULL;
update round_setting set value="text" where tag="motion";

update round set post_primary = 1 where post_results > 0;
update round set post_primary = 3 where post_results > 1;

update round set post_secondary = 3 where post_results = 2;
update round set post_feedback = 2 where post_results = 2;

update round set post_feedback = 1 where exists (select rs.id from round_setting rs where rs.round = round.id and rs.tag = 'coach_feedback');
update round set post_feedback = 2 where exists (select rs.id from round_setting rs where rs.round = round.id and rs.tag = 'public_feedback');


