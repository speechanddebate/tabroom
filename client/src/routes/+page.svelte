<script lang="ts">
    import AdCarousel from '$lib/AdCarousel.svelte';
	import Table from '$lib/Table.svelte';
	import Sidebar from './Sidebar.svelte';

    import { tourns } from './stores';
    // export let data;
    // const tourns = data.tourns;

	interface Tourn {
		start    : string,
		end      : string,
		name     : string,
		location : string,
		state    : string,
		webname  : string
	};

    let tournList: Tourn[];
    tourns.subscribe((value: any) => {
		tournList = value;
	});

	interface Columns {
		header : string,
		key    : string,
		hover? : string,
		class? : string,
		url?   : string,
	};

	let columns: Columns[] = [
		{ 
			header : 'Dates',
			key    : 'dates',
			class  : 'semibold',
		},
		{
			header : 'Name',
			key    : 'name',
			url    : 'url'
		},
		{
			header : 'Location',
			key    : 'location',
			hover  : 'City or Platform',
		},
		{
			header : 'ST/TZ',
			key    : 'state',
			class  : 'center',
			hover  : 'State/Country for in-person, home timezone for online',
		}
	];

</script>

<div id="main">
	<AdCarousel />
	<h1 style='text-align: left;'>Upcoming Tournaments</h1>
	<Table rows="{tournList}" columns="{columns}" />
</div>

<div id="menu">
	<div class="sidenote">
		<h4 style='text-align: left;'>Actions</h4>
		<Sidebar />
	</div>
</div>
