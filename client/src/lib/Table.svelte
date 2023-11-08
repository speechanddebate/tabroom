<script lang="ts">
    interface Row {
        [key: string]: any;
    }
    interface Column {
        header: string;
        key: string;
    }
    export let rows: Row[];
    export let columns: Column[];
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
