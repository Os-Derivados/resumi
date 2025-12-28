import { z } from "zod";

export class SignupViewModel {
	public readonly schema
	public readonly state

	constructor() {
		this.schema = z.object({
			fullName: z.string().min(1).max(128),
			email: z.email(),
			phoneNumber: z.string().regex(/^\+?[1-9]\d{1,14}$/),
			password: z.string().min(8).max(128)
		})

		type SignupSchema = z.infer<typeof this.schema>

		this.state = reactive<Partial<SignupSchema>>({
			fullName: "",
			email: "",
			phoneNumber: "",
			password: ""
		})
	}

	public requestUseCreationAsync(): Promise<void> {
		return Promise.resolve(console.table(this.state))
	}
}