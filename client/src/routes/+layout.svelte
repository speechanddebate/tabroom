<script lang="ts">
	import Header from './Header.svelte';
</script>

<div id="overlay">
	<Header />
	<div id="wrapper">
		<div id="content">
			<slot />
		</div>
	</div>
</div>

<style>

	#overlay {
		background-image    : url("/images/lens-flair.png");
		background-repeat   : no-repeat;
		background-position : -256px -206px;
		padding             : 0;
		margin              : 0;
	}

	#wrapper {
		width      : 88%;
		min-width  : 970px;
		max-width  : 2536px;
		min-height : 650px;
		margin     : auto;
		padding    : 0;
	}
	
	#content {
		position         : relative;
		color            : black;
		background-color : var(--lightest-cool-gray);
		margin           : 0;
		padding          : 0;
		min-width        : 1180px;
	}
	
</style>
