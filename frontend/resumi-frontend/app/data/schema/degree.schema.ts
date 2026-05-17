import { z, type ZodObject } from "zod";

/**
 * Define o esquema de validação para uma formação acadêmica, utilizando a biblioteca Zod.
 * Este esquema garante que os dados da formação estejam no formato correto.
 * @returns Um objeto ZodObject que representa o esquema de validação da formação acadêmica.
 */
export function degreeSchema(): ZodObject {
	return z.object({
		name: z.string()
			.min(3, "O nome do curso deve ter no mínimo 3 caracteres")
			.max(128, "O nome do curso não pode exceder 128 caracteres"),
		description: z.string()
			.min(5, "A descrição deve ter no mínimo 5 caracteres")
			.max(256, "A descrição não pode exceder 256 caracteres"),
		institutionName: z.string()
			.min(3, "O nome da instituição deve ter no mínimo 3 caracteres")
			.max(128, "O nome da instituição não pode exceder 128 caracteres"),
		location: z.string()
			.max(64, "A localização não pode exceder 64 caracteres")
			.optional(),
		isRemote: z.boolean(),
		startDate: z.date({ message: "Data de início é obrigatória" }),
		endDate: z.date().optional(),
		stillEngaged: z.boolean(),
		highlights: z.string()
			.max(1000, "Os destaques não podem exceder 1000 caracteres")
			.optional(),
		degreeLevel: z.enum([
			"HighSchool",
			"Technical",
			"Technologist",
			"Bachelor",
			"Master",
			"Doctorate"
		], { message: "Nível de formação inválido" })
	})
}

/**
 * Schema para criação de formação acadêmica (sem ID)
 */
export function createDegreeSchema(): ZodObject {
	return degreeSchema().extend({
		resumeId: z.number().int().positive("ID do currículo inválido")
	})
}

/**
 * Schema para atualização de formação acadêmica
 */
export function updateDegreeSchema(): ZodObject {
	return degreeSchema().partial({
		name: true,
		description: true,
		institutionName: true,
		startDate: true
	}).extend({
		id: z.number().int().positive("ID da formação inválido")
	})
}
