import type { AddCertificateModel } from "~/data/api/add-certificate-model";
import type { ReadCertificateModel } from "~/data/api/read-certificate-model";
import type { Result } from "~/infra/result";
import { getEnvironmentVariable } from "~/infra/utils/environment-utils";
import { BackendUrl } from "./api.constants";

export async function getCertificatesAsync(resumeId: number): Promise<Result<ReadCertificateModel[]>> {
  try {
    const backendUrl = getEnvironmentVariable(BackendUrl);

    if (!backendUrl) throw new Error("Backend URL is not defined.");

    const result = await useFetch(`${backendUrl}/resumes/${resumeId}/certificates`, {
      method: "GET",
      credentials: "include",
    });

    return result.data.value as Result<ReadCertificateModel[]>;
  } catch {
    return {
      succeeded: false,
      errors: new Map<string, string[]>([["unknown", ["Erro ao buscar certificados."]]]),
      allErrors: ["Erro ao buscar certificados."],
    };
  }
}

export async function createCertificateAsync(resumeId: number, model: AddCertificateModel): Promise<Result<ReadCertificateModel>> {
  try {
    const backendUrl = getEnvironmentVariable(BackendUrl);

    if (!backendUrl) throw new Error("Backend URL is not defined.");

    const result = await useFetch(`${backendUrl}/resumes/${resumeId}/certificates`, {
      method: "POST",
      credentials: "include",
      body: JSON.stringify(model),
      headers: {
        "Content-Type": "application/json",
      },
    });

    return result.data.value as Result<ReadCertificateModel>;
  } catch {
    return {
      succeeded: false,
      errors: new Map<string, string[]>([["unknown", ["Erro ao criar certificado."]]]),
      allErrors: ["Erro ao criar certificado."],
    };
  }
}

export async function updateCertificateAsync(resumeId: number, id: number, model: unknown): Promise<Result<ReadCertificateModel>> {
  try {
    const backendUrl = getEnvironmentVariable(BackendUrl);

    if (!backendUrl) throw new Error("Backend URL is not defined.");

    const result = await useFetch(`${backendUrl}/resumes/${resumeId}/certificates/${id}`, {
      method: "PUT",
      credentials: "include",
      body: JSON.stringify(model),
      headers: {
        "Content-Type": "application/json",
      },
    });

    return result.data.value as Result<ReadCertificateModel>;
  } catch {
    return {
      succeeded: false,
      errors: new Map<string, string[]>([["unknown", ["Erro ao atualizar certificado."]]]),
      allErrors: ["Erro ao atualizar certificado."],
    };
  }
}

export async function deleteCertificateAsync(resumeId: number, id: number): Promise<Result<null>> {
  try {
    const backendUrl = getEnvironmentVariable(BackendUrl);

    if (!backendUrl) throw new Error("Backend URL is not defined.");

    const result = await useFetch(`${backendUrl}/resumes/${resumeId}/certificates/${id}`, {
      method: "DELETE",
      credentials: "include",
    });

    return result.data.value as Result<null>;
  } catch {
    return {
      succeeded: false,
      errors: new Map<string, string[]>([["unknown", ["Erro ao excluir certificado."]]]),
      allErrors: ["Erro ao excluir certificado."],
    };
  }
}
