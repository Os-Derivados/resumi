export type ReadDegreeModel = {
    id: number;
    name: string;
    description: string;
    institutionName: string;
    location?: string;
    isRemote: boolean;
    startDate: string;
    endDate?: string;
    stillEngaged: boolean;
    highlights?: string;
    level: "HighSchool" | "Technical" | "Technologist" | "Bachelor" | "Master" | "Doctorate";
    createdAt: string;
    updatedAt?: string;
};

export type CreateDegreeModel = {
    resumeId: number;
    name: string;
    description: string;
    institutionName: string;
    location?: string;
    isRemote: boolean;
    startDate: string;
    endDate?: string;
    stillEngaged: boolean;
    highlights?: string;
    degreeLevel: "HighSchool" | "Technical" | "Technologist" | "Bachelor" | "Master" | "Doctorate";
};

export type UpdateDegreeModel = {
    id: number;
    name?: string;
    description?: string;
    institutionName?: string;
    location?: string;
    isRemote?: boolean;
    startDate?: string;
    endDate?: string;
    stillEngaged?: boolean;
    highlights?: string;
    degreeLevel?: "HighSchool" | "Technical" | "Technologist" | "Bachelor" | "Master" | "Doctorate";
};
