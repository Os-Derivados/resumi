import type { CreateResumeModel } from "~/data/api/create-resume-model";
import type { Result } from "../result";
import { getEnvironmentVariable } from "../utils/environment-utils";
import { BackendUrl } from "./api.constants";
import { error } from "#build/ui";
import { string } from "zod";


export async function createResumeAsync(model: CreateResumeModel): Promise<Result<void>> {
    try 
    {
        const apiKey = getEnvironmentVariable(BackendUrl)

        if (!apiKey) throw new Error("Backend URL is not defined.")
        if (!model.token) throw new Error("Token is not defined")

        const createResume = await useFetch(`${apiKey}/resumes`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": model.token
            },
            body: JSON.stringify(model.title)
        })

        return {
            succeeded: true,
            errors: {},
        } as Result<void>
    } 
    
    catch
    {
        return {
            succeeded: false,
            errors: new Map<string, string[]>([
                ["Uknown", ["An unexpected error ocurred while creating a new resume"]]
            ]),
            allErrors: ["An unexpected error ocurred while creating a new resume"]
        } as Result<void>
    }
}