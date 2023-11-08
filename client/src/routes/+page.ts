import { tourns } from './stores';

/** @type {import('./$types').PageLoad} */
export const load = async () => {
    const response = await fetch(`${import.meta.env.VITE_API_URL}/invite/upcoming`);
    const data = await response.json();
    tourns.update(() => data);
    return {
        tourns: data,
    };
};
