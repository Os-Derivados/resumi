import type { Result } from "../result";
import type { ReadResumeModel } from "~/data/api/read-resume-model";

/**
 * Efetua o cadastro de um currículo no back-end
 * @param title Título do currículo a ser criado
 * @returns Uma Promise contendo o resultado da operação
 */
export async function createResumeAsync(title: string): Promise<Result<ReadResumeModel>> {
	const { $clientApi } = useNuxtApp()

	const result = await $clientApi<Result<ReadResumeModel>>('/resumes', {
		method: 'POST',
		credentials: 'include',
		query: {
			title
		}
	})

	return result
}