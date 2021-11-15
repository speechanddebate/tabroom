
update tourn_setting
	set tag="drop_deadline"
	where tag="drops_deadline"
	and not exists (select ts.id
			from tourn_setting ts
			where ts.tag = 'drop_deadline'
			and ts.tourn = tourn_setting.tourn
	);

delete from tourn_setting where tag in (
	"CrossEventEntry",
	"UseActualTime",
	"SuppressNavMessages",
	"CrossEventEntry",
	"googleplus",
	"online_nsda_approved",
	"online_public_approved",
	"online_room_limit",
	"TourneyType",
	"test_api",
	"student_claim",
	"DownloadSite",
	"weight",
	"hidden",
	"drops_deadline"
);

