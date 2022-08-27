/* istanbul ignore file */
import React from 'react';
import PropTypes from 'prop-types';
import ErrorPage from './Error';

export class ErrorBoundary extends React.Component {
	constructor(props) {
		super(props);

		this.state = { error: null, errorInfo: null };
	}

	static getDerivedStateFromError(error) {
		return { error };
	}

	componentDidCatch(error, errorInfo) {
		this.setState({ error, errorInfo });
	}

	render() {
		if (this.state.error) {
			return (
				<ErrorPage
					message={`${this.state.error?.toString()} ${this.state.errorInfo?.componentStack}`}
				/>
			);
		}
		return this.props.children;
	}
}

ErrorBoundary.defaultProps = {
	children: {},
};

ErrorBoundary.propTypes = {
	children: PropTypes.any,
};

export default ErrorBoundary;
