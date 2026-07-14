import type { UpdateResumeRequest } from "~/data/api/requests/update-resume.request";
import type { ValueResult } from "../result";
import type { ReadResumeModel } from "~/data/api/read-resume-model";

const route = '/resumes';

/**
 * Efetua o cadastro de um currículo no back-end
 * @param title Título do currículo a ser criado
 * @returns Uma Promise contendo o resultado da operação
 */
export async function createResumeAsync(title: string): Promise<ValueResult<ReadResumeModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadResumeModel>>(route, {
		method: 'POST',
		query: { title }
	})
}

export async function findResumeAsync(id: number): Promise<ValueResult<ReadResumeModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadResumeModel>>(`${route}/${id}`, {
		method: 'GET',
	})
}

export async function readAllResumeAsync(userId: number): Promise<ValueResult<ReadResumeModel[]>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadResumeModel[]>>(route, {
		method: 'GET',
		query: {
			userId
		}
	})
}

export async function updateResumeAsync(resumeId: number, model: UpdateResumeRequest)
: Promise<ValueResult<ReadResumeModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadResumeModel>>(`${route}/${resumeId}`, {
		method: 'PUT',
		credentials: 'include',
		body: model
	})
}


export async function deleteResumeAsync(resumeId: number): Promise<ValueResult<ReadResumeModel>> {
	const { $clientApi } = useNuxtApp()

	return await $clientApi<ValueResult<ReadResumeModel>>(`${route}/${resumeId}`, {
		method: 'DELETE',
		credentials: 'include'
	})
}