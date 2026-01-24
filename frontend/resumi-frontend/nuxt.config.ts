// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    compatibilityDate: '2025-07-15',
    devtools: { enabled: true },

    modules: [
        '@nuxt/eslint',
        '@nuxt/ui',
        '@nuxt/test-utils',
        '@nuxt/image',
        '@nuxt/hints',
        '@pinia/nuxt'
    ],

    css: ['~/assets/css/main.css'],
    runtimeConfig: {
        public: {
            backendUrl: 'https://localhost:8081/api',
            environment: 'development'
        }
    },
    icon: {
        customCollections: [{
            prefix: "custom",
            dir: "./app/assets/icons"
        }]
    },
    plugins: [
        { src: '~/plugins/api.client.ts', mode: 'client' }
    ],
})