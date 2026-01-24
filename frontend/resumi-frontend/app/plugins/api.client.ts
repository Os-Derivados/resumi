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

    const config = useRuntimeConfig()

    const clientApi = $fetch.create({
        baseURL: config.public.backendUrl,
        credentials: 'include',
        ignoreResponseError: true,
        async onResponse({ response }) {
            if (response?.status !== 401) return

            const currentLocation = router.currentRoute.value

            if (!currentLocation || currentLocation.path === '/login') return

            await performRedirect({
                path: '/login',
                query: { redirect: currentLocation.fullPath },
            })
        },
    })

    return { provide: { clientApi } }
})