export type ResumeNodeModel = {
	id: number
	resumeId: number
	name?: string
	description?: string
	institutionName?: string
	location?: string
	isRemote: boolean
	startDate: Date
	endDate?: Date
	stillEngaged: boolean
}