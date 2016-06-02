ALTER TABLE person ADD googleplus varchar(127) DEFAULT NULL;
ALTER TABLE tourn ADD googleplus tinyint DEFAULT NULL;
ALTER TABLE panel
	ADD g_event varchar(255) DEFAULT NULL,
	ADD room_ext_id varchar(255) DEFAULT NULL,
	ADD invites_sent tinyint(4) NOT NULL DEFAULT '0';
ALTER TABLE ballot ADD hangout_admin tinyint(4) NOT NULL DEFAULT '0';
INSERT INTO room (id,name) VALUES(-1,'Google Hangout');
INSERT INTO room (id,name) VALUES(-2,'Hangout On Air');
