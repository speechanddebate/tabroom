import { createStore, applyMiddleware, compose } from 'redux';
import thunk from 'redux-thunk';
import api from './apiMiddleware.js';
import rootReducer from './rootReducer.js';

// Create the redux store
/* eslint-disable no-underscore-dangle */
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
export default createStore(rootReducer, composeEnhancers(applyMiddleware(thunk, api)));
/* eslint-enable */
