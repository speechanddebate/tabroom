<script>
	import Header from './Header.svelte';
</script>


<div id="overlay">

	<Header />

	<div style="text-align: center;">
		<slot />
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

</style>
