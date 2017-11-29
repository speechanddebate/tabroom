
delete from session where timestamp < "2017-07-01 00:00:00";

alter table session add weekend int after category;

