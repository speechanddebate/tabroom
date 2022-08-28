import React from 'react';
import { unstable_HistoryRouter as Router, Route, Routes } from 'react-router-dom';
import { Provider } from 'react-redux';

import store from './redux/store';
import ErrorBoundary from './ErrorBoundary';
import Layout from './Layout';
import TestTable from './TestTable';

const App = () => {
	return (
		<Router>
			<Provider store={store}>
				<ErrorBoundary>
					<Routes>
						<Route exact path="/" element={<Layout><TestTable /></Layout>} />
						<Route exact path="/route" element={<Layout>{() => <p>I&apos;m a component, hear me roar!</p>}</Layout>} />
					</Routes>
				</ErrorBoundary>
			</Provider>
		</Router>
	);
};

export default App;
