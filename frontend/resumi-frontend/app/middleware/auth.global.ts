import { PUBLIC_PATHS } from "~/infra/auth/auth.constants"
import { getSessionUserAsync } from "~/infra/auth/session-user.context"
import { useSessionUserStore } from "~/stores/session-user.store"

export default defineNuxtRouteMiddleware(async (to) => {
    const session = useSessionUserStore()

    // If already authenticated or on public path, allow navigation
    if (session.isAuthenticated || PUBLIC_PATHS.includes(to.path)) return

    // On server side, redirect to login if not authenticated and not on public path
    if (import.meta.server) {
        return navigateTo({
            path: '/login',
            query: { redirect: to.fullPath }
        })
    }

    // On client side, try to get session user - if it returns 401, the API client will handle redirect
    const sessionUser = await getSessionUserAsync()

    session.setSessionUser(sessionUser)
})