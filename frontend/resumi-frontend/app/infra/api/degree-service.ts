import type { Result } from "../result";
import type { ReadDegreeModel, CreateDegreeModel, UpdateDegreeModel } from "~/data/api/degree-models";

/**
 * Recupera todas as formações acadêmicas de um currículo
 * @param resumeId ID do currículo
 * @returns Uma Promise contendo a lista de formações
 */
export async function getDegreesAsync(resumeId: number): Promise<Result<ReadDegreeModel[]>> {
    const { $clientApi } = useNuxtApp();

    const result = await $clientApi<Result<ReadDegreeModel[]>>(
        `/resumes/${resumeId}/degrees`,
        {
            method: "GET",
            credentials: "include"
        }
    );

    return result;
}

/**
 * Cria uma nova formação acadêmica
 * @param resumeId ID do currículo
 * @param degree Dados da formação a ser criada
 * @returns Uma Promise contendo a formação criada
 */
export async function createDegreeAsync(
    resumeId: number,
    degree: CreateDegreeModel
): Promise<Result<ReadDegreeModel>> {
    const { $clientApi } = useNuxtApp();

    const result = await $clientApi<Result<ReadDegreeModel>>(
        `/resumes/${resumeId}/degrees`,
        {
            method: "POST",
            credentials: "include",
            body: degree
        }
    );

    return result;
}

/**
 * Atualiza uma formação acadêmica existente
 * @param resumeId ID do currículo
 * @param degreeId ID da formação
 * @param degree Dados atualizados da formação
 * @returns Uma Promise contendo a formação atualizada
 */
export async function updateDegreeAsync(
    resumeId: number,
    degreeId: number,
    degree: UpdateDegreeModel
): Promise<Result<ReadDegreeModel>> {
    const { $clientApi } = useNuxtApp();

    const result = await $clientApi<Result<ReadDegreeModel>>(
        `/resumes/${resumeId}/degrees/${degreeId}`,
        {
            method: "PUT",
            credentials: "include",
            body: degree
        }
    );

    return result;
}

/**
 * Deleta uma formação acadêmica
 * @param resumeId ID do currículo
 * @param degreeId ID da formação a ser deletada
 * @returns Uma Promise contendo o resultado da operação
 */
export async function deleteDegreeAsync(
    resumeId: number,
    degreeId: number
): Promise<Result<boolean>> {
    const { $clientApi } = useNuxtApp();

    const result = await $clientApi<Result<boolean>>(
        `/resumes/${resumeId}/degrees/${degreeId}`,
        {
            method: "DELETE",
            credentials: "include"
        }
    );

    return result;
}
