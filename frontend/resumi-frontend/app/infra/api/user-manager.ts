import type { CreateUserModel } from "~/data/api/create-user-model";
import type { ReadUserModel } from "~/data/api/read-user-model";
import type { LoginModel } from "~/data/api/login-model";
import type { AuthResponse } from "~/data/api/auth-response";
import { BackendUrl } from "./api.constants";
import { getEnvironmentVariable } from "~/infra/utils/environment-utils";
import type { ValueResult } from "~/infra/result";

export async function createUserAsync(backendUrl: string, model: CreateUserModel): Promise<ValueResult<ReadUserModel>> {
	try {
		const createUserResult = await useFetch(`${backendUrl}/users`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify(model),
		})

		return createUserResult.data.value as ValueResult<ReadUserModel>
	}
	catch {
		return {
			succeeded: false,
			errors: new Map<string, string[]>([
				["unknown", ["An unexpected error occurred while creating the user."]],
			]),
			allErrors: ["An unexpected error occurred while creating the user."],
		}
	}
}

export async function loginAsync(model: LoginModel): Promise<ValueResult<AuthResponse>> {
	try {
		const backendUrl = getEnvironmentVariable(BackendUrl)

		if (!backendUrl) throw new Error('Backend URL is not defined.')

		const loginResult = await useFetch(`${backendUrl}/users/login`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify(model),
			credentials: 'include'
		})

		return loginResult.data.value as ValueResult<AuthResponse>
	}
	catch {
		return {
			succeeded: false,
			errors: new Map<string, string[]>([
				["unknown", ["An unexpected error occurred while logging in."]],
			]),
			allErrors: ["An unexpected error occurred while logging in."],
		}
	}
}