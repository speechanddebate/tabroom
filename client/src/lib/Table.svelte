<script lang="ts">
	import type { Column } from '../types/Column';
    interface Row {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        [key: string]: any;
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
						<td class="{c.class || ""} nospace">
							<a href={r[c.url]} class="full">
								{r[c.key]}
							</a>
						</td>
					{:else}
						<td
							class="{c.class || ''} {c.hoverkey ? r[c.hoverkey] : 'hover'}"
							title={c.hoverkey ? r[c.hoverkey] : '' }
						>
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
		padding        : 4px;
		padding-left   : 8px;
		margin         : 0;
		font-size      : 11px;
		line-height    : 14px;
		vertical-align : middle;
		font-weight     : 600;
	}

	.semibold {
		font-weight: 600;
	}
</style>
