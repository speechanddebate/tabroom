<script lang="ts">
	import Login from '$lib/Login.svelte';
	import SearchBar from '$lib/SearchBar.svelte';
</script>

<div id="header">
	<span id="logo">
		<a
			tabindex = "-1"
			href     = "/"
		>
			<img
				src = "/images/tabroom-logo.png"
				alt = "Tabroom.com by the National Speech and Debate Association"
			>
		</a>
	</span>

	<span id="toprow">
		<Login />
		<SearchBar />
	</span>
</div>

<style>

	#header {
		width     : 88%;
		margin    : auto;
		padding   : 0;
		min-width : 970px;
	}

	#logo {
		display        : inline-block;
		width          : 22%;
		margin         : 0;
		padding        : 0;
		padding-bottom : 4px;
		padding-left   : 4px;
		max-height     : 48px;
	}

	#logo a {
		margin  : 0;
		padding : 0;
	}

	#logo a img {
		max-width  : 100%;
		max-height : 100%;
	}

</style>
