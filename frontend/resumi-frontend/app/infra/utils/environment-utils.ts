export function isDevelopment(): boolean {
	return import.meta.env.NODE_ENV === 'development'
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