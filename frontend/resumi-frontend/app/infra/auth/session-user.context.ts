import type { ReadUserModel } from "~/data/api/read-user-model"
import type { Result } from "../result"

/**
 * Efetua a obtenção do usuário da sessão atual.
 * @returns Uma instância do usuário da sessão, ou undefined caso não haja um usuário autenticado.
 */
export async function getSessionUserAsync(): Promise<ReadUserModel | undefined> {
	const config = useRuntimeConfig()

	// On server-side, use $fetch directly since $clientApi plugin is client-only
	if (import.meta.server) {
		try {
			const event = useRequestEvent()
			const cookies = event?.headers.get('cookie') || ''

			const getSessionResult = await $fetch<Result<ReadUserModel>>('users/me', {
				baseURL: config.public.backendUrl,
				method: 'GET',
				credentials: 'include',
				headers: {
					cookie: cookies
				}
			})

			if (!getSessionResult?.succeeded) return undefined

			return getSessionResult.data
		} catch {
			return undefined
		}
	}

	// On client-side, use the $clientApi plugin
	const { $clientApi } = useNuxtApp()

	const getSessionResult = await $clientApi<Result<ReadUserModel>>('users/me', {
		method: 'GET',
		credentials: 'include'
	})

	if (!getSessionResult?.succeeded) return undefined

	return getSessionResult.data
}