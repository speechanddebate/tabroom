import React from 'react';
import { useLocation } from 'react-router-dom';
import PropTypes from 'prop-types';

const Error = ({ statusCode = null, message = '', is404 = false }) => {

	if (statusCode === 404) {
		is404 = true;
	}

	const location = useLocation();
	const pathName = location.pathname;

	if (is404) {
		statusCode = 404;
		message = `The page ${pathName} doesn't exist`;
	}

	return (
		<div className="error">
			<div>
				<h2>Error {statusCode}</h2>
				<p className="message bigger">{ message }</p>
				<p>
					{/* <a href="#" onClick={() => navigate(-1)}>Back</a> */}
					<a href="/" />
				</p>
			</div>
		</div>
	);
};

Error.propTypes = {
	statusCode : PropTypes.number,
	message    : PropTypes.string,
	is404      : PropTypes.bool,
};

export default Error;
