import config from '../../config/config'
import db from '../models';

import attendance from './status';
import { assert } from 'chai';

describe ("Status Dashboard API Functions", () => {

	it("Shows correct status on the status board", async () => {

		let req = {
			db      : db,
			config  : config,
			params  : {
				tourn_id : 1,
				round_id : 1
			}
		};

		const status = await(attendance(req));
		console.log(status);

	});

});

import { assert } from 'chai';

