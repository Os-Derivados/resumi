import { PUBLIC_PATHS } from "~/infra/auth/auth.constants"
import { getSessionUserAsync } from "~/infra/auth/session-user.context"
import { useSessionUserStore } from "~/stores/session-user.store"

export default defineNuxtRouteMiddleware(async (to) => {
    // Skip on server side since $clientApi is only available on client
    if (import.meta.server) return

    const session = useSessionUserStore()

    if (session.isAuthenticated || PUBLIC_PATHS.includes(to.path)) return

    // Try to get session user - if it returns 401, the API client will handle redirect
    const sessionUser = await getSessionUserAsync()

    session.setSessionUser(sessionUser)
})