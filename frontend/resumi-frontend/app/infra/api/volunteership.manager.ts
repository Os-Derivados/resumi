import type { VolunteershipModel } from "~/data/api/models/volunteership.model";
import type { Result, ValueResult } from "../result";
import type { AddVolunteershipRequest } from "~/data/api/requests/add-volunteership.request";
import type { UpdateVolunteershipRequest } from "~/data/api/requests/update-volunteership.request";

const route = '/volunteerships'

export async function addVolunteershipAsync(model: AddVolunteershipRequest): Promise<ValueResult<VolunteershipModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<VolunteershipModel>>(route, {
		method: 'POST',
		body: model
	})
}

export async function updateVolunteershipAsync(model: UpdateVolunteershipRequest): Promise<ValueResult<VolunteershipModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<VolunteershipModel>>(`${route}/${model.id}`, {
		method: 'PUT',
		body: model
	})
}

export async function deleteVolunteershipAsync(id: number): Promise<Result> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<Result>(`${route}/${id}`, {
		method: 'DELETE'
	})
}