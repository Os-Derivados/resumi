import { z, type ZodObject } from "zod";

/**
 * Define o esquema de validação para um currículo, utilizando a biblioteca Zod.
 * Este esquema é utilizado para garantir que os dados do currículo estejam no formato correto antes de serem processados ou armazenados.
 * @returns Um objeto ZodObject que representa o esquema de validação do currículo.
 */
export function resumeSchema(): ZodObject {
	return z.object({
		title: z.string().min(10).max(255),
		ownerName: z.string().min(3).max(255),
		location: z.string().max(255).optional(),
		email: z.email().max(255),
		phoneNumber: z.string().max(20),
		description: z.string().max(1024).optional(),
		keyword: z.string().max(255).optional()
	})
}