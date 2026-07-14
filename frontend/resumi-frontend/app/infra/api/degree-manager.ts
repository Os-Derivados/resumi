import type { ValueResult } from "../result";
import type { ReadDegreeModel, CreateDegreeModel as AddDegreeModel, UpdateDegreeModel } from "~/data/api/degree-models";

const route = '/degrees';

/**
 * Cria uma nova formação acadêmica
 * @param degree Dados da formação a ser criada
 * @returns Uma Promise contendo a formação criada
 */
export async function addDegreeAsync(
    degree: AddDegreeModel
): Promise<ValueResult<ReadDegreeModel>> {
    const { $clientApi } = useNuxtApp();

    const result = await $clientApi<ValueResult<ReadDegreeModel>>(
        route,
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
 * @param degreeId ID da formação
 * @param degree Dados atualizados da formação
 * @returns Uma Promise contendo a formação atualizada
 */
export async function updateDegreeAsync(
    degreeId: number,
    degree: UpdateDegreeModel
): Promise<ValueResult<ReadDegreeModel>> {
    const { $clientApi } = useNuxtApp();

    const result = await $clientApi<ValueResult<ReadDegreeModel>>(
        `${route}/${degreeId}`,
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
 * @param degreeId ID da formação a ser deletada
 * @returns Uma Promise contendo o resultado da operação
 */
export async function deleteDegreeAsync(
    degreeId: number
): Promise<ValueResult<boolean>> {
    const { $clientApi } = useNuxtApp();

    const result = await $clientApi<ValueResult<boolean>>(
        `${route}/${degreeId}`,
        {
            method: "DELETE",
            credentials: "include"
        }
    );

    return result;
}
