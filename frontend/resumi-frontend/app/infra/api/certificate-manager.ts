import type { AddCertificateModel } from "~/data/api/add-certificate-model";
import type { ReadCertificateModel } from "~/data/api/read-certificate-model";
import type { ValueResult } from "~/infra/result";
import { getEnvironmentVariable } from "~/infra/utils/environment-utils";
import { BackendUrl } from "./api.constants";
import type { UpdateCertificateModel } from "~/data/api/update-certificate-model";

const backendUrl = getEnvironmentVariable(BackendUrl);
const route = `${backendUrl}/certificates`;

export async function createCertificateAsync(model: AddCertificateModel): Promise<ValueResult<ReadCertificateModel>> {
  try {
    if (!backendUrl) throw new Error("Backend URL is not defined.");

    const result = await useFetch(`${route}/certificates`, {
      method: "POST",
      credentials: "include",
      body: JSON.stringify(model),
      headers: {
        "Content-Type": "application/json",
      },
    });

    return result.data.value as ValueResult<ReadCertificateModel>;
  } catch {
    return {
      succeeded: false,
      errors: new Map<string, string[]>([["unknown", ["Erro ao criar certificado."]]]),
      allErrors: ["Erro ao criar certificado."],
    };
  }
}

export async function updateCertificateAsync(id: number, model: UpdateCertificateModel): Promise<ValueResult<ReadCertificateModel>> {
  try {
    if (!backendUrl) throw new Error("Backend URL is not defined.");

    const result = await useFetch(`${route}/${id}`, {
      method: "PUT",
      credentials: "include",
      body: JSON.stringify(model),
      headers: {
        "Content-Type": "application/json",
      },
    });

    return result.data.value as ValueResult<ReadCertificateModel>;
  } catch {
    return {
      succeeded: false,
      errors: new Map<string, string[]>([["unknown", ["Erro ao atualizar certificado."]]]),
      allErrors: ["Erro ao atualizar certificado."],
    };
  }
}

export async function deleteCertificateAsync(id: number): Promise<ValueResult<null>> {
  try {
    if (!backendUrl) throw new Error("Backend URL is not defined.");

    const result = await useFetch(`${route}/${id}`, {
      method: "DELETE",
      credentials: "include",
    });

    return result.data.value as ValueResult<null>;
  } catch {
    return {
      succeeded: false,
      errors: new Map<string, string[]>([["unknown", ["Erro ao excluir certificado."]]]),
      allErrors: ["Erro ao excluir certificado."],
    };
  }
}
