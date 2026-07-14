export type ValueResult<TValue> = {
	succeeded: boolean
	data?: TValue
	errors: Map<string, string[]>
	allErrors: string[]
}

export type Result = {
	succeeded: boolean
	errors: Map<string, string[]>
	allErrors: string[]
}