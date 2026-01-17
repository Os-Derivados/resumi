import type { FormSubmitEvent } from "@nuxt/ui";
import z from "zod";
import type { CreateResumeModel } from "~/data/api/create-resume-model";
import { createResumeAsync } from "~/infra/api/resume-service";
import { isDevelopment } from "~/infra/utils/environment-utils";

export class CreateResumeViewModel {
    constructor() {
        this.schema = z.object({
            title: z.string().min(10).max(255),
            authtoken: z.string()
        })

        type CreateResumeSchema = z.infer<typeof this.schema>

        this.state = reactive<Partial<CreateResumeSchema>>({
            title: "",
            authtoken: ""
        })
    }

    public readonly schema
    public readonly state 

    public requestCreateResume = async (event: FormSubmitEvent<typeof this.state>): Promise<void> => {
        const toast = useToast()
        const authCookie = useCookie<string>("auth").value
        this.state.authtoken = authCookie

        event.preventDefault();

        
        try {
            const createResumeModel = this.schema.parse(this.state) as CreateResumeModel
            const result = await createResumeAsync(createResumeModel)

            const resultDisplay = result.succeeded 
                ? "Curriculo criado com sucesso!"
                : "Falha ao criar curriculo!"
            
            toast.add({
                title: 'Criação de curriculo',
                description: resultDisplay,
                color: result.succeeded ? "success" : "error",
                duration: 5000
            })

            if (result.succeeded) {
                await useRouter().push("/resume-editor")
            }
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