import React from 'react';
import { unstable_HistoryRouter as Router, Route, Routes } from 'react-router-dom';
import { Provider } from 'react-redux';
import { createBrowserHistory } from 'history';

import store from './redux/store';
import ErrorBoundary from './common/ErrorBoundary';
import Error from './common/Error';
import Layout from './common/Layout';
import TestTable from './common/table/TestTable';
import TournList from './public/TournList';
import TournListMenu from './public/TournListMenu';
import TestForm from './samples/TestForm';

import './normalize.css';

const App = () => {

	const history = createBrowserHistory({ window });

	return (
		<Router history={history}>
			<Provider store={store}>
				<ErrorBoundary>
					<Routes>
						<Route exact path="/" element={<Layout main={TournList} menu={TournListMenu} />} />
						<Route exact path="/route/:name" element={<Layout><TestTable /><TestForm /></Layout>} />
						<Route exact path="/route" element={<Layout><p>I&apos;m a component, hear me roar!</p></Layout>} />
						<Route path="*" element={<Layout><Error is404 /></Layout>} />
					</Routes>
				</ErrorBoundary>
			</Provider>
		</Router>
	);
};

export default App;
