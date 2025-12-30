import { z } from "zod";
import type { CreateUserModel } from "~/data/api/create-user-model";
import { createUserAsync } from "~/infra/api/user-service";

export class SignupViewModel {
	private readonly _runtimeConfig = useRuntimeConfig()

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

	public readonly schema
	public readonly state

	public requestUserCreationAsync = async (event: MouseEvent): Promise<void> => {
		event.preventDefault()

		const toast = useToast()

		const newUser = this.schema.parse(this.state) as CreateUserModel
		const result = await createUserAsync(this._runtimeConfig.public.backendUrl, newUser)

		const resultDisplay = result.succeeded
			? 'Usuário criado com sucesso!'
			: 'Falha ao criar usuário'

		toast.add({
			title: 'Cadastrar Usuário',
			description: resultDisplay,
			color: result.succeeded ? 'success' : 'error',
			duration: 5000
		})
	}
}