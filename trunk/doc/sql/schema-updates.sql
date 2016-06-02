
alter table login add ualt_id int;
alter table login add nsda_login_id int;
alter table student add ualt_id int;
alter table chapter add city varchar(63);

create table nsda_event_categories (
	id int auto_increment primary key,
	type char,
	name varchar(31),
	nsda_id int,
	nat_category bool,
	timestamp timestamp
);

