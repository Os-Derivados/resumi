import { PUBLIC_PATHS } from "~/infra/auth/auth.constants"
import { getSessionUserAsync } from "~/infra/auth/session-user.context"
import { useSessionUserStore } from "~/stores/session-user.store"

export default defineNuxtRouteMiddleware(async (to) => {
	// Allow access to public paths without authentication
	if (PUBLIC_PATHS.includes(to.path)) return

	// On server-side, check authentication via API
	if (import.meta.server) {
		const sessionUser = await getSessionUserAsync()

		if (!sessionUser) {
			return navigateTo('/login', { redirectCode: 302 })
		}

		return
	}

	// On client-side, check store first, then API if needed
	const session = useSessionUserStore()

	if (session.isAuthenticated) return

	try {
		const sessionUser = await getSessionUserAsync()

		if (!sessionUser) {
			return navigateTo('/login')
		}

		session.setSessionUser(sessionUser)
	} catch {
		session.setSessionUser(undefined)
		return navigateTo('/login')
	}
})