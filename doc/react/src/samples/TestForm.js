import React, { useEffect } from 'react';
import { useForm, useWatch } from 'react-hook-form';

export const TestForm = () => {
	const {
		register,
		control,
		formState: { errors, isValid },
		handleSubmit,
	} = useForm({
		mode: 'all',
		defaultValues: {
			name: '',
			setting: false,
		},
	});

	const watchedFields = useWatch({ control });

	useEffect(() => {
		// console.log(watchedFields);
	}, [watchedFields]);

	const formHandler = async (data) => {
		try {
			await fetch('http://local.tabroom.com:10010/v1/status', { method: 'POST', body: data });
		} catch (err) {
			console.log(err);
		}
	};

	return (
		<form onSubmit={handleSubmit(formHandler)}>
			<input
				name         = "name"
				type         = "text"
				defaultValue = "Hardy"
				{...register('name', { required: true, maxLength: 5 })}
			/>
			<input
				name="setting"
				type="checkbox"
				{...register('setting')}
			/>
			<button type="submit" disabled={!isValid}>Submit</button>
			<div className="error">
				{errors.name?.type === 'required' && <p>This field is required</p>}
			</div>
		</form>
	);
};

export default TestForm;
