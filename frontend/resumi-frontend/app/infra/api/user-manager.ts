import type { CreateUserModel } from "~/data/api/create-user-model";
import type { ReadUserModel } from "~/data/api/read-user-model";
import type { LoginModel } from "~/data/api/login-model";
import type { AuthResponse } from "~/data/api/auth-response";
import type { ValueResult } from "~/infra/result";
import { ApiPagination } from "~/data/api/models/api-pagination";
import type { UpdateUserRequest } from "~/data/api/requests/update-user.request";

const route = '/users'

export async function createUserAsync(model: CreateUserModel): Promise<ValueResult<ReadUserModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadUserModel>>(route, {
		method: 'POST',
		body: model
	})
}

export async function readAllUsersAsync(pagination: ApiPagination = new ApiPagination()): Promise<ValueResult<ReadUserModel[]>> {
	const { $clientApi } = useNuxtApp()
	
	return await $clientApi<ValueResult<ReadUserModel[]>>(route, {
		method: 'GET',
		query: pagination.isDefault() ? { } : pagination
	})
}

export async function aboutMeAsync(): Promise<ValueResult<ReadUserModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadUserModel>>(`${route}/me`, {
		method: 'GET'
	})
}

export async function findUserAsync(id: number): Promise<ValueResult<ReadUserModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadUserModel>>(`${route}/${id}`, {
		method: 'GET'
	})
}

export async function loginAsync(model: LoginModel): Promise<ValueResult<AuthResponse>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<AuthResponse>>(`${route}/login`, {
		method: 'POST',
		body: model,
		credentials: 'omit'
	})
}

export async function updateUserAsync(id: number, model: UpdateUserRequest): Promise<ValueResult<ReadUserModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadUserModel>>(`${route}/${id}`, {
		method: 'PUT',
		body: model
	})
}