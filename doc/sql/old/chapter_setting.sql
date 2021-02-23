	delete cs2.*  
	from chapter_setting cs1, chapter_setting cs2  
	where cs1.chapter = cs2.chapter 
		and cs1.tag = cs2.tag
		and cs1.id > cs2.id
	;

	alter table chapter_setting add constraint uk_chapter_tag UNIQUE(tag,chapter);

