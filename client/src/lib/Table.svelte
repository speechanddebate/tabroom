<script>
    export let rows;
    export let columns;
</script>

<table id="table">
    <thead>
        <tr>
            {#each columns as c}
                <th>{c.header}</th>
            {/each}
        </tr>
    </thead>
    <tbody>
        {#each rows as r}
            <tr>
                {#each columns as c}
                    <td>{r[c.key]}</td>
                {/each}
            </tr>
        {/each}
    </tbody>
</table>