export function isDevelopment(): boolean {
	const runtimeConfig = useRuntimeConfig()

	return runtimeConfig.public.environment === 'development'
}

export function getEnvironmentVariable(key: string): string | undefined {
	const runtimeConfig = useRuntimeConfig()

	try {
		return runtimeConfig.public[key] as string
	}
	catch {
		return undefined
	}
}