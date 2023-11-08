import { tourns } from './stores';
import config from '../config';

/** @type {import('./$types').PageLoad} */
export const load = async () => {
    const response = await fetch(`${config.API_URL}/invite/upcoming`);
    const data = await response.json();
    tourns.update(() => data);
    return {
        tourns: data,
    };
};
