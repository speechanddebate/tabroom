create table region_setting (
	id int auto_increment primary key,
	region int,
	tag varchar(32),
	value varchar(127),
	value_text text,
	value_date datetime,
	timestamp timestamp,
	created_at datetime,
	setting int,
	event int
);

create index event on region_setting(event);
create index region on region_setting(region);
