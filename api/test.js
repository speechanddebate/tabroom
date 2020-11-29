
const express = require('express');
const path    = require('path');

const JWT = require('machinepack-jwt');

const secret = 'g2349df9023';

const encrypted = 'eyJhbGciOiJIUzI1NiJ9.eyJwdWJsaWNTZXJ2ZXIiOnRydWUsImRpc3BsYXlOYW1lIjoiVGFiIC0gQ2hyaXMgUGFsbWVyIiwic3VwcG9ydEVtYWlsIjoiaW5mb0BzcGVlY2hhbmRkZWJhdGUub3JnIiwic291cmNlIjoidGFicm9vbSIsInJvb21JbnN0cnVjdGlvbnMiOiJQcmVzcyB0aGUgY29ycmVjdCBidXR0b24gYW5kIG5vdCB0aGUgd3Jvbmcgb25lLiBEZWJhdGUgd2VsbC4gRG9uJ3QgY2xpcCBjYXJkcyBvciBmYWtlIGV2aWRlbmNlLiBEb24ndCBjb21wbGFpbiB0byBtZSB3aGVuIHRoZSB3aWtpIGdvZXMgZG93bi4iLCJyb2xlIjoidGFiIiwicm9vbU5hbWUiOiJDWCA5IiwicGVyc29uX2lkIjoxLCJ1dWlkIjoiZWYzYmUyNGM4YjFhZGYzZjAwMzEwOWVlY2U4ZTRjYWEifQ.gs0eRyqFMFsgFhC7MGW31b8pxWhtCwTxuE0hDvrEAA4';

	JWT.decode({
		secret    : 'g2349df9023',
		token     : encrypted,
		algorithm : 'HS256'
	}).exec({
		error: function (err) {
			console.log(err);
		},
		success: function (decodedToken) {
           	console.log(decodedToken);
        }
	});


