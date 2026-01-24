import type { $Fetch } from 'ofetch'

declare module '#app' {
    interface NuxtApp {
        $clientApi: $Fetch
    }
}

declare module 'vue' {
    interface ComponentCustomProperties {
        $clientApi: $Fetch
    }
}

export { }