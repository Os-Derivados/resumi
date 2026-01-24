import { PUBLIC_PATHS } from "~/infra/auth/auth.constants"
import { getSessionUserAsync } from "~/infra/auth/session-user.context"
import { useSessionUserStore } from "~/stores/session-user.store"

export default defineNuxtRouteMiddleware(async () => {
    const route = useRoute()
    const session = useSessionUserStore()

    if (session.isAuthenticated || PUBLIC_PATHS.includes(route.path)) return

    try {
        const sessionUser = await getSessionUserAsync()

        session.setSessionUser(sessionUser)
    } catch {
        session.setSessionUser()
    }
})