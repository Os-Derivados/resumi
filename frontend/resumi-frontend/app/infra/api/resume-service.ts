import type { CreateResumeModel } from "~/data/api/create-resume-model";
import type { Result } from "../result";
import { getEnvironmentVariable } from "../utils/environment-utils";
import { BackendUrl } from "./api.constants";
import { error } from "#build/ui";
import { string } from "zod";


export async function createResumeAsync(model: CreateResumeModel): Promise<Result<undefined>> {
    try 
    {
        const apiKey = getEnvironmentVariable(BackendUrl)

        if (!apiKey) throw new Error("Backend URL is not defined.")

        const createResume = await useFetch(`${apiKey}/resumes`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(model)
        })

        return {
            succeeded: true,
            errors: {},
        } as Result<undefined>
    } 
    
    catch
    {
        return {
            succeeded: false,
            errors: new Map<string, string[]>([
                ["Uknown", ["An unexpected error ocurred while creating a new resume"]]
            ]),
            allErrors: ["An unexpected error ocurred while creating a new resume"]
        } as Result<undefined>
    }
}