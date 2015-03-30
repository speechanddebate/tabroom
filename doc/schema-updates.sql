
create table nsda_event_categories (
	id int auto_increment primary key,
	type char,
	name varchar(31),
	nsda_id int,
	nat_category bool,
	timestamp timestamp
);

