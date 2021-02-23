
delete from session where timestamp < "2018-07-01 00:00:00";

alter table session add defaults text after ip;
alter table session add weekend int after category;

