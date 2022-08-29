import React from 'react';
import PropTypes from 'prop-types';

const Error = ({ statusCode = null, message = '', is404 = false }) => {
	if (statusCode === 404) { is404 = true; }

	if (is404) {
		statusCode = 404;
		message = "That page doesn't exist";
	}

	return (
		<div className="error">
			<div>
				<h3>Error {statusCode}</h3>
				<p className="message">{message}</p>
				<p>
					{/* <a href="#" onClick={() => navigate(-1)}>Back</a> */}
					<span> | </span>
					<a href="/" />
				</p>
			</div>
		</div>
	);
};

Error.propTypes = {
	statusCode: PropTypes.number,
	message: PropTypes.string,
	is404: PropTypes.bool,
};

export default Error;
