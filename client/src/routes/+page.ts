import { tourns } from './stores';

/** @type {import('./$types').PageLoad} */
export const load = async () => {
    const response = await fetch('http://local.tabroom.com/v1/invite/upcoming');
    const data = await response.json();
    tourns.update(() => data);
    return {
        tourns: data,
    };
};
