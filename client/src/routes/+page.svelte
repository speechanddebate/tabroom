<script lang="ts">
    import AdCarousel from '$lib/AdCarousel.svelte';
	import DataTable from '$lib/DataTable.svelte';
	// import Sidebar from './Sidebar.svelte';
	import type { PageData } from './$types';

    import { tourns } from './stores';

	export let data:PageData;

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
</script>

<div id="main">
	<AdCarousel />
	<h1 style='text-align: left;'>Upcoming Tournaments</h1>
	<DataTable data="{tournList}" columns="{data.columns}" />
</div>

<div id="menu">
	<div class="sidenote">
		<h4 style='text-align: left;'>Actions</h4>
		<!-- <Sidebar /> -->
	</div>
</div>
