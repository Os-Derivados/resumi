export default defineNuxtPlugin(() => {
    const router = useRouter()

    let isRedirecting = false

    const performRedirect = async (to: { path: string; query?: Record<string, string> }) => {
        if (isRedirecting) return

        isRedirecting = true
        try {
            await router.isReady()
            await router.replace(to)
        } finally {
            isRedirecting = false
        }
    }

    const clientApi = $fetch.create({
        credentials: 'include',
        ignoreResponseError: true,
        onResponse({ response }) {
            if (!import.meta.client || response?.status !== 401) return

            const currentLocation = router.currentRoute.value

            if (!currentLocation || currentLocation.path === '/login') return

            performRedirect({
                path: '/login',
                query: { redirect: currentLocation.fullPath },
            })
        },
    })

    return { provide: { clientApi } }
})