<script lang="ts">
    interface Results {
        partialMatches: Tourn[],
        exactMatches: Tourn[],
        searchString: string,
    }
	interface Tourn {
		start: string,
		name: string,
		location: string,
		state: string,
		webname: string
	}

    let searchResults: Results;
    let resultsVisible = false;

    const getSearch = async () => {
        const searchInput = document.getElementById('searchtext') as HTMLInputElement;
        if (!searchInput || searchInput.value.length < 4) return;
        const response = await fetch(`${import.meta.env.VITE_API_URL}/public/search/all/${searchInput.value}`);
        searchResults = await response.json();
        resultsVisible = true;
    }
    const handleSubmit = async(e: Event) => {
        e.preventDefault();
        getSearch();
        window.location.href = `/search`;
    }
    const handleChangeInput = () => {
        getSearch();
    }
    const handleMouseLeave = () => {
        resultsVisible = false;
    }
</script>

<span id="search" class="searchform" title="Search for tournaments">
    <form on:submit={handleSubmit}>
        <input
            on:input       = {handleChangeInput}
            id             = "searchtext"
            type           = "text"
            maxlength      = "128"
            name           = "search"
            placeholder    = "SEARCH TOURNAMENTS"
            autocomplete   = "off"
            autocorrect    = "off"
            autocapitalize = "none"
            spellcheck     = "false"
            class          = "searchinput notfirst"
            tabindex       = "-1"
        >

        <button type="submit" class="searchbutton">
            <i class="fas fa-lg fa-magnifying-glass"></i>
        </button>
    </form>
</span>
{#if (searchResults?.exactMatches?.length > 0 || searchResults?.partialMatches?.length > 0) && resultsVisible}
    <div class="popup" role="listbox" tabindex="-1" on:mouseleave={handleMouseLeave}>
        {#each searchResults?.exactMatches as result}
            <p>{result.name}</p>
        {/each}
        {#each searchResults?.partialMatches as result}
            <p>{result.name}</p>
        {/each}
    </div>
{/if}

<style>
    .searchform {
        display          : inline-block;
        position         : relative;
        background-color : var(--darkest-blue);
        margin-left      : 4px;
        margin-right     : 4px;
		margin-top       : 6x;
        overflow         : hidden;
		vertical-align   : middle;
        padding          : 4px;
    }
    .searchinput {
        background    : none repeat scroll 0 0 transparent;
        color         : var(--background-white);
		border        : 1px solid var(--dark-blue);
		border-radius : 9px;
        width         : 75%;
		line-height   : 16px;
        font-size     : 16px;
		padding       : 4px;
		padding-left  : 1%;
		padding-right : 1%;
        display       : inline;
    }

	.searchbutton {
		padding       : 4px;
		padding-left  : 1%;
		padding-right : 1%;
		width         : 20%;
		background    : transparent;
		border        : none;
		color         : var(--neutral-gray);
	}

    .popup {
        position         : absolute;
        background-color : #f9f9f9;
        min-width        : 160px;
        box-shadow       : 0px 8px 16px 0px rgba(0,0,0,0.2);
        z-index          : 1;
        top              : 50px;
        right            : 200px;
    }
</style>
