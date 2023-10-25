"use client";

import * as z from "zod";
import { useState } from "react";
import { TextInput, Text, PasswordInput } from "@mantine/core";
import classes from "./FloatingLabelInput.module.css";
import {
	useForm,
	isNotEmpty,
	isEmail,
	isInRange,
	hasLength,
	matches,
} from "@mantine/form";
import { Button } from "@mantine/core";
const LoginForm = () => {
	const form = useForm({
		initialValues: {
			username: "",
			password: "",
		},
		validate: {
			username: hasLength({ min: 1 }, "Username is required"),
			password: hasLength({ min: 1 }, "Password is required"),
		},
	});
	const onSubmit = (data) => console.log(data);
	return (
		<div>
			<form className="space-y-5" onSubmit={form.onSubmit(() => {})}>
				<TextInput
					label="Username"
					placeholder="Username"
					name="username"
					id="username"
					{...form.getInputProps("username")}
				/>
				<PasswordInput
					label="Password"
					placeholder="Password"
					name="password"
					id="password"
					{...form.getInputProps("password")}
				/>
				<Button color="black" className="rounded-xl" type="submit">
					Submit
				</Button>
			</form>
		</div>
	);
};

export default LoginForm;
