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

	// Ignore SSL certificate errors in development
	hooks: {
		ready() {
			if (import.meta.env.NODE_ENV === 'development') {
				import.meta.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
			}
		}
	}
})