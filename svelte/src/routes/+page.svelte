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

<div class="centeralign auto">
	<div id="main" style="border: 1px solid black; width: 70%; display: inline-block;">
		<h1 style='text-align: left;'>Upcoming Tournaments</h1>
		<Table rows="{tournList}" columns="{columns}" />
	</div>

	<div id="menu" style="border: 1px solid black; width: 25%; display: inline-block; height: 100vh;">
		<h1 style='text-align: left;'>Actions</h1>
		<Sidebar />
	</div>
</div>
