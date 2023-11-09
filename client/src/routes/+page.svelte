<script lang="ts">
    import AdCarousel from '$lib/AdCarousel.svelte';
	import Table from '$lib/Table.svelte';
	import Sidebar from './Sidebar.svelte';

    import { tourns } from './stores';

	interface Tourn {
		start: string,
		name: string,
		location: string,
		state: string,
		webname: string
	}

    let tournList: Tourn[];
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
    tourns.subscribe((value: any) => {
		tournList = value;
	});

	let columns = [
		{ 
			header : 'Start Date',
			key    : 'start',
		},
		{
			header : 'Name',
			key    : 'name'
		},
		{
			header : 'City',
			key    : 'location'
		},
		{
			header : 'State',
			key    : 'state'
		},
		{
			header : 'Webname',
			key    : 'webname'
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
