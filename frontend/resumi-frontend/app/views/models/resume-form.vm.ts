import type z from "zod"
import { resumeSchema } from "~/data/schema/resume.schema"

export class ResumeFormViewModel {
	private readonly _resumeId: number

	constructor(resumeId: number) {
		this._resumeId = resumeId

		this.schema = resumeSchema()

		type ResumeSchemaType = z.infer<typeof this.schema>

		this.state = reactive<Partial<ResumeSchemaType>>({
			title: "",
			email: "",
			keyword: "",
			location: "",
			ownerName: "",
			phoneNumber: ""
		})
	}

	public readonly state
	public readonly schema
}