import React from 'react';
import { unstable_HistoryRouter as Router, Route, Routes } from 'react-router-dom';
import { Provider } from 'react-redux';
import { createBrowserHistory } from 'history';

import store from './redux/store';
import ErrorBoundary from './ErrorBoundary';
import Layout from './Layout';
import TestTable from './TestTable';
import TestForm from './TestForm';

import './normalize.css';

const App = () => {
	const history = createBrowserHistory({ window });

	return (
		<Router history={history}>
			<Provider store={store}>
				<ErrorBoundary>
					<Routes>
						<Route exact path="/" element={<Layout><TestTable /><TestForm /></Layout>} />
						<Route exact path="/route/:name" element={<Layout><TestTable /></Layout>} />
						<Route exact path="/route" element={<Layout><p>I&apos;m a component, hear me roar!</p></Layout>} />
						<Route path="*" element={<Layout><p>I am 404.  Hear me roar.</p></Layout>} />
					</Routes>
				</ErrorBoundary>
			</Provider>
		</Router>
	);
};

export default App;
