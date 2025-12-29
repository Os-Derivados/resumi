export type Result<TValue> = {
	succeeded: boolean
	data?: TValue
	errors: Map<string, string[]>
	allErrors: string[]
}