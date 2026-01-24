import { PUBLIC_PATHS } from "~/infra/auth/auth.constants"
import { getSessionUserAsync } from "~/infra/auth/session-user.context"
import { useSessionUserStore } from "~/stores/session-user.store"

export default defineNuxtRouteMiddleware(async (to) => {
    if (import.meta.server) return

    const session = useSessionUserStore()

    if (session.isAuthenticated || PUBLIC_PATHS.includes(to.path)) return

    try {
        const sessionUser = await getSessionUserAsync()
        session.setSessionUser(sessionUser)
    } catch {
        session.setSessionUser(undefined)
    }
})