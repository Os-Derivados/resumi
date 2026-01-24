import type { ReadUserModel } from "~/data/api/read-user-model"
import type { Result } from "../result"

/**
 * Efetua a obtenção do usuário da sessão atual.
 * @returns Uma instância do usuário da sessão, ou undefined caso não haja um usuário autenticado.
 */
export async function getSessionUserAsync(): Promise<ReadUserModel | undefined> {
    const { $clientApi } = useNuxtApp()

    const getSessionResult = await $clientApi<Result<ReadUserModel>>('users/me', {
        method: 'GET',
        credentials: 'include'
    })

    if (!getSessionResult?.succeeded) return undefined

    return getSessionResult.data
}