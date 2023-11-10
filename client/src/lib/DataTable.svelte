<script lang="ts">
	import {TabulatorFull as Tabulator} from 'tabulator-tables';
	import {onMount} from 'svelte';

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	export let data: any[];
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	export let columns: any[] = [];
	let tableComponent:HTMLDivElement;

	onMount(() => {
		new Tabulator(tableComponent, {
			data,
			columns,
			layout            : 'fitColumns',
			reactiveData      : true,
			headerSortElement : '<i class="fas fas-sm fa-arrow-up"></i>',
			autoColumns       : columns.length > 0 ? false : true,
			pagination        : true,
			paginationSize    : 50,
		});
	});
	</script>
  
	<div bind:this={tableComponent}></div>
  
	<svelte:head>
<!--	<script 
			type="text/javascript" 
			src="https://oss.sheetjs.com/sheetjs/xlsx.full.min.js"
		></script>
-->
		<link
			href="/css/tabulator.min.css" 
			rel="stylesheet"
		>
		<link href="/css/tabulator.css" rel="stylesheet">
	</svelte:head>