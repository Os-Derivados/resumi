import type { FormSubmitEvent } from "@nuxt/ui"
import z from "zod"
import type { LoginModel } from "~/data/api/login-model"
import { loginAsync } from "~/infra/api/user-service"
import { isDevelopment } from "~/infra/utils/environment-utils"

export class LoginViewModel {
	constructor() {
		this.schema = z.object({
			email: z.email(),
			password: z.string().min(8).max(128)
		})

		type LoginSchema = z.infer<typeof this.schema>

		this.state = reactive<Partial<LoginSchema>>({
			email: "",
			password: ""
		})
	}

	public readonly schema
	public readonly state

	public requestLoginAsync = async (event: FormSubmitEvent<typeof this.state>): Promise<void> => {
		event.preventDefault()

		const toast = useToast()

		try {
			const loginModel = this.schema.parse(this.state) as LoginModel
			const result = await loginAsync(loginModel)
			const token = result.data?.token

			const resultDisplay = result.succeeded
				? 'Login realizado com sucesso!'
				: 'Falha ao realizar login'

			toast.add({
				title: 'Login',
				description: resultDisplay,
				color: result.succeeded ? 'success' : 'error',
				duration: 5000
			})

			if (result.succeeded) {
				
				if (token) useCookie<string>("auth", { default: () => token })

				const router = useRouter()
				await router.push('/home')
			}
		}
		catch (error) {
			if (isDevelopment()) console.error(error)

			toast.add({
				title: 'Login',
				description: 'Ocorreu um erro ao processar sua solicitação.',
				color: 'error',
				duration: 5000
			})
		}
	}
}