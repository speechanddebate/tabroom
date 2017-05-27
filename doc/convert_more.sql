
alter table circuit_membership modify name varchar(64);

alter table circuit_membership modify circuit int after description;

alter table district modify level tinyint;

alter table district modify location varchar(16);
alter table district modify code varchar(16);
alter table district modify name varchar(64);
alter table district modify realm varchar(8);

alter table email modify circuit int after tourn;

alter table entry drop seed;
alter table entry drop chapter;
alter table entry modify registered_by int after dq;
alter table entry modify event int after dq;
alter table entry modify school int after dq;
alter table entry modify tourn int after dq;
alter table entry modify created_at datetime after dq;
alter table entry modify ada bool after name;

alter table entry_setting drop type;

alter table event_setting modify event int not null after value_date;
alter table file modify type varchar(16) after id;
alter table file drop posting;
alter table file drop result;

alter table fine modify deleted_by int after levied_by;
alter table fine modify deleted_at datetime after levied_by;
alter table fine modify deleted bool after levied_by;

alter table jpool_setting drop type;

alter table judge_setting modify judge int after value_date;
alter table person_setting modify person int after value_date;
alter table region_setting modify region int after value_date;
alter table tiebreak_set_setting modify tiebreak_set int after value_date;
alter table tiebreak modify child smallint after highlow_count;

alter table webpage modify parent int after tourn;

alter table judge_hire add region int after school;
alter table fine modify deleted bool not null default 0;
