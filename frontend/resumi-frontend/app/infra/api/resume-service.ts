import type { CreateResumeModel } from "~/data/api/create-resume-model";
import type { Result } from "../result";
import type { ReadResumeModel } from "~/data/api/read-resume-model";

/**
 * Efetua o cadastro de um currículo no back-end
 * @param model Modelo contendo os dados do currículo a ser criado
 * @returns Uma Promise contendo o resultado da operação
 */
export async function createResumeAsync(model: CreateResumeModel): Promise<Result<ReadResumeModel>> {
    const { $clientApi } = useNuxtApp()

    const result = await $clientApi<Result<ReadResumeModel>>('/resumes', {
        method: 'POST',
        credentials: 'include',
        body: model
    })

    return result
}