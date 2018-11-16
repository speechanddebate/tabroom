
drop trigger if exists `insert_entry_active`;
drop trigger if exists `update_entry_active`;

update entry set active=1;

update entry set active=0 where ( waitlist = 1 
	OR dropped = 1 
	OR dq = 1 
	OR unconfirmed = 1
	OR unconfirmed = 2
);

delimiter $$ 

	create trigger `update_entry_active`
	before update on `entry`

	FOR EACH ROW
		BEGIN

			IF 
				NEW.dropped = 1 
				OR NEW.waitlist = 1 
				OR NEW.dq = 1 
				OR NEW.unconfirmed = 1
				OR NEW.unconfirmed = 2
			THEN
				set NEW.active = 0;
			ELSE
				set NEW.active = 1;


			END IF;

		END; $$

delimiter ;


delimiter $$ 

	create trigger `insert_entry_active`
	before insert on `entry`

	FOR EACH ROW
		BEGIN

			IF 
				NEW.dropped = 1 
				OR NEW.waitlist = 1 
				OR NEW.dq = 1 
				OR NEW.unconfirmed = 1
				OR NEW.unconfirmed = 2
			THEN
				set NEW.active = 0;
			ELSE
				set NEW.active = 1;

			END IF;

		END; $$

delimiter ;

