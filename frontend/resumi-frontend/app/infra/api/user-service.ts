import type { CreateUserModel } from "~/data/api/create-user-model";
import type { ReadUserModel } from "~/data/api/read-user-model";
import type { Result } from "../result";

export async function createUserAsync(backendUrl: string, model: CreateUserModel): Promise<Result<ReadUserModel>> {
	try {
		const createUserResult = await useFetch(`${backendUrl}/users`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify(model),
		})

		return createUserResult.data.value as Result<ReadUserModel>
	}
	catch {
		return {
			succeeded: false,
			errors: new Map<string, string[]>([
				["unknown", ["An unexpected error occurred while creating the user."]],
			]),
			allErrors: ["An unexpected error occurred while creating the user."],
		}
	}
}