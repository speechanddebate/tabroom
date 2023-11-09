<script lang="ts">
    import AdCarousel from '$lib/AdCarousel.svelte';
	import Table from '$lib/Table.svelte';
	import Sidebar from './Sidebar.svelte';

    import { tourns } from './stores';

	import type { Column } from '../types/Column';

	interface Tourn {
		start    : string,
		end      : string,
		name     : string,
		location : string,
		state    : string,
		webname  : string
	}

    let tournList: Tourn[];
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
    tourns.subscribe((value: any) => {
		tournList = value;
	});

	let columns: Column[] = [
		{
			header : 'Dates',
			key    : 'dates'
		},
		{
			header : 'Name',
			key    : 'name',
			url    : 'url'
		},
		{
			header : 'City/Platform',
			key    : 'location',
			hover  : 'City or Online Platform',
		},
		{
			header   : 'ST/TZ',
			key      : 'state',
			class    : 'center',
			hover    : 'State/Country for in-person, home timezone for online',
			hoverkey : 'tzHover',
		},
		{
			header : 'Judge Signup',
			key    : 'signup'
		},
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
