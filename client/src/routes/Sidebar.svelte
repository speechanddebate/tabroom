<script lang="ts">
    import { tourns } from './stores';
    const getTourns = async (e: Event) => {
        const target = e.target as HTMLSelectElement;
        const response = await fetch(`${import.meta.env.VITE_API_URL}/invite/upcoming?state=${target.value}`);
        const data = await response.json();
        tourns.update( () => data );
    }
</script>

<div>
    <select on:change={getTourns}>
        <option value="TX">Texas</option>
        <option value="IA">Iowa</option>
        <option value="CO">Colorado</option>
        <option value="UT">Utah</option>
    </select>
</div>
