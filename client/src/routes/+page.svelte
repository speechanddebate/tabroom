<script>
	import Table from '$lib/Table.svelte';
	import Sidebar from './Sidebar.svelte';

    import { tourns } from './stores.js';
    // export let data;
    // const tourns = data.tourns;

    let tournList;
    tourns.subscribe((value) => {
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
		<h1 style='text-align: left;'>Upcoming Tournaments</h1>
		<Table rows="{tournList}" columns="{columns}" />
	</div>

	<div id="menu">
		<div class="sidenote">
			<h4 style='text-align: left;'>Actions</h4>
			<Sidebar />
		</div>
	</div>
