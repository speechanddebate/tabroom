<script>
    import { tourns } from './stores.js';
    const getTourns = async (e) => {
        const response = await fetch(`http://local.tabroom.com/v1/invite/upcoming?state=${e.target.value}`);
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
