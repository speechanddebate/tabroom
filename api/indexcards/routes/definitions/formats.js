/* eslint-disable import/prefer-default-export */

// From https://coderwall.com/p/jmarug/regex-to-check-for-valid-mysql-datetime-format
// Accepts both YYYY-MM-DD and YYYY-MM-DD HH:mm:ss
export const mysqlDate = '^([0-9]{2,4})-([0-1][0-9])-([0-3][0-9])(?:( [0-2][0-9]):([0-5][0-9]):([0-5][0-9]))?$';
