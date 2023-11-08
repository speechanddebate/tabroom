<script lang="ts">
    interface Row {
        [key: string]: any;
    }
    interface Column {
        header : string;
		hover  : string;
        key    : string;
		url?   : string;
		class? : string;
    }
    export let rows: Row[];
    export let columns: Column[];
</script>

<table id="table">
    <thead>
        <tr class='table-header-row'>
            {#each columns as c}
                <th title={c.hover}>
					{c.header}
				</th>
            {/each}
        </tr>
    </thead>
    <tbody>
        {#each rows as r}
            <tr class='table-data-row'>
                {#each columns as c}
					{#if c.url}
						<td class={c.class}>
							<a href={r[c.url]}>
								{r[c.key]}
							</a>
						</td>
					{:else}
						<td class={c.class}>
							{r[c.key] || ''}
						</td>
					{/if}
                {/each}
            </tr>
        {/each}
    </tbody>
</table>

<style>

	.table-data-row td {
		padding-left   : 4px;
		padding-right  : 4px;
		padding-top    : 6px;
		padding-bottom : 6px;
		font-size      : 12px;
	}
	
	.table-header-row  {
		background-color : var(--lightest-yellow);
		font-weight      : 600;
	}

	.table-header-row th {
		padding-left   : 4px;
		padding-right  : 4px;
		padding-top    : 4px;
		padding-bottom : 4px;
		margin         : 0;
		font-size      : 80%;
	}

	.semibold {
		font-weight: 600;
	}
</style>
