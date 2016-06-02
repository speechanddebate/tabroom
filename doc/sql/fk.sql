
delete from room_group_room where room_group not in (select distinct id from room_group);
alter table room_group_room add foreign key(room_group) references room_group(id) on delete cascade on update cascade;

delete from room_group_round where room_group not in (select distinct id from room_group);
alter table room_group_round add foreign key(room_group) references room_group(id) on delete cascade on update cascade;
