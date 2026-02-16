import type { FormSubmitEvent } from "@nuxt/ui";
import z from "zod";
import { createResumeAsync } from "~/infra/api/resume-service";
import { isDevelopment } from "~/infra/utils/environment-utils";

export class CreateResumeViewModel {
	constructor() {
		this.schema = z.object({
			title: z.string().min(10).max(255)
		})

		type CreateResumeSchema = z.infer<typeof this.schema>

		this.state = reactive<Partial<CreateResumeSchema>>({
			title: ""
		})
	}

	public readonly schema
	public readonly state

	public handleCreateResumeAsync = async (event: FormSubmitEvent<typeof this.state>): Promise<void> => {
		const toast = useToast()

		event.preventDefault();


		try {
			const title = this.state.title || ""
			const creationResult = await createResumeAsync(title)

			const resultDisplay = creationResult.succeeded
				? "Curriculo criado com sucesso!"
				: "Falha ao criar curriculo!"

			toast.add({
				title: 'Criação de curriculo',
				description: resultDisplay,
				color: creationResult.succeeded ? "success" : "error",
				duration: 5000
			})
		}
		catch (error) {
			if (isDevelopment()) console.error(error)

			toast.add({
				title: 'Criação de curriculo',
				description: "Ocorreu um erro ao criar o seu curriculo.",
				color: "error",
				duration: 5000
			})
		}
	}
}