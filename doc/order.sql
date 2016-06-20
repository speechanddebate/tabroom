
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

alter table conflict add type varchar(15) after id;
update tabroom.conflict set type = "chapter" where chapter > 0;
update tabroom.conflict set type = "individual" where conflict > 0;

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

alter table follower drop created_at;
alter table follower drop updated_at;
alter table follower change type type varchar(8) after id;
alter table follower change follower follower int after email;

alter table hotel change tourn tourn int after multiple;
alter table hotel drop created_at;
alter table hotel drop updated_at;

alter table housing change type type varchar(7) after id;
alter table housing change night night date after type;
alter table housing change waitlist waitlist bool after night;
alter table housing change tba tba bool after waitlist;
alter table housing change requested requested datetime after tba;
alter table housing change person requestor int after requested;
alter table housing change timestamp timestamp timestamp after school;
alter table housing drop created_at;

alter table housing_slots change tourn tourn int after slots;
alter table housing_slots drop created_at; 

alter table jpool_judge drop created_at; 
alter table jpool_judge drop type;

alter table jpool_round drop created_at; 

alter table jpool change name name varchar(63) after id;
alter table jpool change category category int after name;
alter table jpool drop tourn;
alter table jpool drop created_at; 

alter table judge_hire change covers entries_requested int after id; 
alter table judge_hire change accepted entries_accepted int after entries_requested; 
alter table judge_hire change rounds rounds_requested int after entries_accepted; 
alter table judge_hire change rounds_accepted rounds_accepted int after rounds_requested; 
alter table judge_hire drop created_at; 
alter table judge_hire change request_made requested_at datetime after rounds_accepted;
alter table judge_hire change requestor requestor int after requested_at;

alter table judge change code code int after id; 
alter table judge change first first varchar(63) after code;
alter table judge add middle varchar(63) after first;
alter table judge change last last varchar(63) after middle;
alter table judge change active active bool after last; 
alter table judge change ada ada bool after active;
alter table judge change obligation obligation smallint after ada; 
alter table judge change hired hired smallint after obligation; 
alter table judge drop created_at;

alter table judge change timestamp timestamp timestamp after person_request;

alter table login drop name;
alter table login drop salt;
alter table login drop created_at;

alter table login change password password varchar(63) after last_access;
alter table login change sha512 sha512 char(128) after password;
alter table login change spinhash spinhash char(128) after sha512;
alter table login change pass_timestamp pass_timestamp datetime after spinhash;
alter table login change person person int after source;

alter table login change timestamp timestamp timestamp after ualt_id;

update person set state="MA" where state="Mass";
update person set state="MA" where state="Massachuset";
update person set state="NY" where state="New York";
update person set state="WA" where state="Washington";
update person set state="UT" where state="ut";
update person set state="Ohio" where state="OH";

alter table person change middle middle varchar(63) after first; 
alter table person drop alt_phone; 
alter table person drop multiple;
alter table person drop flags;
alter table person drop created_at;
alter table person drop started_judging;
alter table person change state state char(4) after city;
alter table person change postal postal varchar(15) after zip; 
alter table person change tz tz varchar(63) after country;
alter table person change phone phone varchar(31) after tz;
alter table person change provider provider varchar(63) after phone;

alter table person change gender gender char after last;
alter table person change pronoun pronoun varchar(63) after gender;
alter table person change no_email no_email bool after pronoun;
alter table person change ualt_id ualt_id int after googleplus;
alter table person change timestamp timestamp timestamp after ualt_id;

alter table qualifier change name name varchar(63) after id;
alter table qualifier change result result varchar(127) after name;
alter table qualifier change qualified_at qualified_at int after tourn;
alter table qualifier drop created_at; 

alter table rating_subset drop created_at;

alter table rating_tier change type type enum('coach', 'mpj') after id;
alter table rating_tier change strike strike bool after description;
alter table rating_tier change conflict conflict bool after strike;
alter table rating_tier change min min float after conflict;
alter table rating_tier change max max float after min;
alter table rating_tier change start start bool after max;
alter table rating_tier drop created_at;
alter table rating_tier drop tourn;
alter table rating_tier change timestamp timestamp timestamp after rating_subset;

delete from rating where timestamp < "2015-07-01 00:00:00";

alter table rating change type type enum('school', 'entry', 'coach') after id;
alter table rating add draft bool;
alter table rating change draft draft bool after type;
alter table rating change entered entered datetime after draft;
alter table rating change ordinal ordinal int after entered;
alter table rating change percentile percentile float after ordinal;
alter table rating drop created_at; 

alter table region change circuit circuit int after sweeps;
alter table region change timestamp timestamp timestamp after tourn;

alter table region drop active;
alter table region drop created_at;

alter table result_set drop created_at; 

alter table result_set change label label varchar(255) after id;
alter table result_set change bracket bracket bool after label;
alter table result_set change published published bool after bracket;
alter table result_set change generated generated datetime after published;

alter table result_value change tag tag varchar(15) after id;
alter table result_value change value value varchar(255) after tag;
alter table result_value change priority priority smallint after value;
alter table result_value change long_tag long_tag varchar(63) after priority;
alter table result_value change no_sort no_sort bool after long_tag;
alter table result_value change sort_desc sort_desc bool after no_sort;
alter table result_value change result result int after sort_desc;
alter table result_value drop created_at; 


