
alter table student add birthdate date;
alter table student add school_sid varchar(63);
alter table student add race varchar(63);
alter table round add note varchar(255);

alter table room add ada bool not null default 0;
alter table entry add flight int not null default 0;



