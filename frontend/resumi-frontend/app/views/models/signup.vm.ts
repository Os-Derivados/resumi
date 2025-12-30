import type { FormSubmitEvent } from "@nuxt/ui";
import { z } from "zod";
import type { CreateUserModel } from "~/data/api/create-user-model";
import { createUserAsync } from "~/infra/api/user-service";

export class SignupViewModel {
	private readonly _runtimeConfig = useRuntimeConfig()

	constructor() {
		this.schema = z.object({
			fullName: z.string().min(1).max(128),
			email: z.email(),
			phoneNumber: z.string().regex(/^\+55 \(\d{2}\) 9 \d{4}-\d{4}$/),
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

	public requestUserCreationAsync = async (event: FormSubmitEvent<typeof this.state>): Promise<void> => {
		event.preventDefault()
		const toast = useToast()

		try {
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

			// if (result.succeeded) await useRouter().push('/login')
		}
		catch {
			toast.add({
				title: 'Cadastrar Usuário',
				description: 'Ocorreu um erro ao processar sua solicitação.',
				color: 'error',
				duration: 5000
			})

			await useRouter().push('/signup')
		}
	}
}