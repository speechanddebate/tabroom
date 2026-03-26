
update event set type="speech", timestamp=timestamp where type="" and nsda_category LIKE "2%";
update event set type="wsdc", timestamp=timestamp where nsda_category = "105";
update event set type="debate", timestamp=timestamp where type="" and nsda_category LIKE "1%";
update event set type="speech", timestamp=timestamp where type="" and abbr IN ("DI" ,"HI", "EXT", "DUO", "USX", "IX", "OO" , "POI", "INF", "INFO", "IMP", "OI", "DEC", "RAD", "PRO", "POE");
update event set type="speech", timestamp=timestamp where type="" and abbr LIKE "Du%";
update event set type="congress", timestamp=timestamp where type="" and abbr IN ("HSE", "SEN", "CON", "HOU", "CONG", "SC", "CD");
update event set type="debate", timestamp=timestamp where type="" and abbr IN ("CX", "LD", "PF", "PFD", "POL");

update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "PF%";
update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "%PF%";
update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "%PF";

update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "LD%";
update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "%LD%";
update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "%LD";

update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "CX%";
update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "%CX%";
update event set type="debate", timestamp=timestamp where type="" and abbr LIKE "%CX";

alter table event modify type enum('speech','congress','debate','wudc','wsdc','attendee','mock_trial','academic');
update event set type="academic", timestamp=timestamp where type="";
alter table event modify type enum('speech','congress','debate','wudc','wsdc','attendee','mock_trial','academic') NOT NULL DEFAULT 'attendee';

