
alter table ballot drop created_at;

alter table ballot change side side bool after id;
alter table ballot change speakerorder speakerorder bool after side;
alter table ballot change speechnumber speechnumber bool after speakerorder;
alter table ballot change chair chair bool after speechnumber;
alter table ballot change bye bye bool after chair;
alter table ballot change forfeit forfeit bool after bye;
alter table ballot change seed seed bool after forfeit;
alter table ballot change pullup pullup bool after seed;
alter table ballot change tv tv bool after pullup;
alter table ballot change audit audit bool after tv;
alter table ballot change judge_started judge_started datetime after audit;
alter table ballot change collected collected datetime after judge_started;
alter table ballot change collected_by collected_by int after collected;
alter table ballot change entered_by entered_by int after collected_by;
alter table ballot change audited_by audited_by int after entered_by;
alter table ballot change cat_id cat_id int after hangout_admin; 
alter table ballot change timestamp timestamp timestamp after hangout_admin; 


alter table change_log change timestamp timestamp timestamp after round; 
alter table change_log change description description text after type; 
alter table change_log change new_panel new_panel int after fine; 
alter table change_log change person person int after description; 
alter table change_log drop created_at; 

alter table circuit drop created_at;
alter table circuit drop url;
alter table circuit change timestamp timestamp timestamp after webname;

alter table circuit_membership add timestamp timestamp;

alter table concession drop created_at;
alter table concession change tourn tourn int after school_cap;
alter table concession change timestamp timestamp timestamp after tourn;

alter table concession_purchase drop created_at;
alter table concession_purchase change school school int after fulfilled;
alter table concession_purchase change concession concession int after school;
alter table concession_purchase change timestamp timestamp timestamp after concession;

alter table conflict change type type varchar(15) after id;
alter table conflict drop judge; 
alter table conflict drop created_at; 

alter table conflict change added_by added_by int after chapter;
alter table conflict change timestamp timestamp timestamp after added_by;

alter table event_double drop created_at;
alter table event_double drop min;
alter table event_double change setting type tinyint after name;
alter table event_double change max max int after setting;
alter table event_double change exclude exclude int after max;
alter table event_double change tourn tourn int after exclude;

alter table event_double change timestamp timestamp timestamp after tourn;

alter table email change subject subject varchar(127) after id;
alter table email change content content text after subject;
alter table email change sent_to sent_to varchar(127) after content;
alter table email change sent_on sent_at datetime after sent_to;
alter table email change tourn tourn int after sender;
alter table email drop created_at;
alter table email drop region;
alter table email change timestamp timestamp timestamp after tourn;

alter table entry drop created_at;

alter table entry change code code varchar(63) after id;
alter table entry change name name varchar(127) after code;

alter table entry change dropped dropped bool after name;
alter table entry change waitlist waitlist bool after dropped;
alter table entry add unconfirmed bool after waitlist;
alter table entry change dq dq bool after unconfirmed;
alter table entry change ada ada bool after dq;
alter table entry change tba tba bool after ada;
alter table entry drop self_reg_by;
alter table entry change seed seed varchar(15) after tba;
alter table entry add registered_by int after event;

alter table event change name name varchar(127) after id;
alter table event change abbr abbr varchar(11) after name;
alter table event change type type varchar(15) after abbr;
alter table event change fee fee float after type;
alter table event change category category int after tourn;
alter table event drop created_at;
alter table event change timestamp timestamp timestamp after rating_subset; 

alter table file add webpage int;
alter table file drop public;
alter table file drop created_at;
alter table file change label label varchar(127) after id;
alter table file change name filename varchar(127) after label;
alter table file change posting posting bool after name;
alter table file change published published bool after posting;
alter table file change uploaded uploaded datetime after published;
alter table file change timestamp timestamp timestamp after webpage;

alter table school_fine drop region;
alter table school_fine drop judge;
alter table school_fine drop tourn;
alter table school_fine change reason reason varchar(63) after id;
alter table school_fine change amount amount float after reason;
alter table school_fine change payment payment bool after amount;

alter table school_fine change deleted deleted bool after tourn;
alter table school_fine change deleted_at deleted_at datetime after deleted; 
alter table school_fine change deleted_by deleted_by int after deleted_at; 

alter table school_fine change levied_on levied_at datetime after deleted_by; 
alter table school_fine change levied_by levied_by int after levied_at; 

alter table school_fine change timestamp timestamp timestamp after levied_by;
alter table school_fine drop created_at;
