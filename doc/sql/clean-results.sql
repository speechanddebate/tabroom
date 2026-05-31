
delete from result_value rv where rv.result_key IS NOT NULL and rv.result_key != 0 and NOT EXISTS (select rk.id from result_key rk where rk.id = rv.result_key);

