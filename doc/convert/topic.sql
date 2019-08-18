create table topic (
	id int auto_increment primary key,
	tag varchar(31),
	source varchar(15),
	event_type varchar(31),
	topic_text text,
	school_year int,
	sort_order smallint,
	timestamp timestamp,
	created_at datetime,
	created_by int not null default 0
);

