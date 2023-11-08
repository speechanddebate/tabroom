<script lang="ts">
    import { onMount } from 'svelte';
    interface Ad {
        id: number,
        filename: string,
        url: string,
    }
	let ads: Ad[] = [];
    let currentAd: Ad = ads[0];
    let currentIndex = 0;

    onMount(() => {
        const fetchAds = async () => {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/public/ads`);
            ads = await response.json();
        };
        fetchAds();
        const interval = setInterval(() => {
            currentIndex = (currentIndex + 1) % ads.length;
            currentAd = ads[currentIndex];
        }, 10000);
        return () => clearInterval(interval);
    });
</script>

<div class="centeralign carousel row even">
    <a target="_blank" rel="noopener noreferrer" href="{currentAd?.url}" tabindex="0">
        <img alt="ad" src="{`${import.meta.env.VITE_S3_BASE}/ads/${currentAd?.id}/${currentAd?.filename}`}">
    </a>
</div>

<style>
    .carousel {
        width: 100%;
        display: inline-block;
        text-align: center;
        background-color: var(--dark-gray);
    }
</style>
