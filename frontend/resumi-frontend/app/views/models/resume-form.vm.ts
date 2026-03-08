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
			location: "",
			ownerName: "",
			phoneNumber: "",
			description: "",
			keyword: "",
		})

		this.focusState = reactive<Record<keyof ResumeSchemaType, boolean>>({
			title: false,
			email: false,
			location: false,
			ownerName: false,
			phoneNumber: false,
			description: false,
			keyword: false,
		})
	}

	public readonly state
	public readonly schema
	public readonly focusState

	public setFocus(field: keyof typeof this.focusState, isFocused: boolean): void {
		this.focusState[field] = isFocused
	}

	public getVariant(field: keyof typeof this.focusState): "outline" | "none" {
		return this.focusState[field] ? "outline" : "none"
	}
}