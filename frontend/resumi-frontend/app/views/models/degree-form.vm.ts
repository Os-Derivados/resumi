import { z } from "zod";
import { degreeSchema, updateDegreeSchema } from "~/data/schema/degree.schema";
import type { ReadDegreeModel, UpdateDegreeModel } from "~/data/api/degree-models";

const degreeLevelOptions = [
    { value: "HighSchool", label: "Ensino Médio" },
    { value: "Technical", label: "Técnico" },
    { value: "Technologist", label: "Tecnólogo" },
    { value: "Bachelor", label: "Bacharelado" },
    { value: "Master", label: "Mestrado" },
    { value: "Doctorate", label: "Doutorado" }
];

export class DegreeFormViewModel {
    public readonly schema;
    public readonly state;
    public readonly focusState;
    public readonly degreeLevelOptions = degreeLevelOptions;
    public isLoading = false;
    public errors: Record<string, string> = {};
    public isEditMode = false;
    private readonly _existingDegree?: ReadDegreeModel;

    constructor(degree?: ReadDegreeModel) {
        this._existingDegree = degree;
        this.isEditMode = !!degree;

        // Use update schema in edit mode (partial fields required)
        this.schema = this.isEditMode ? updateDegreeSchema() : degreeSchema();

        type DegreeSchemaType = z.infer<typeof this.schema>;

        this.state = reactive<Partial<DegreeSchemaType>>({
            name: degree?.name ?? "",
            description: degree?.description ?? "",
            institutionName: degree?.institutionName ?? "",
            location: degree?.location ?? "",
            isRemote: degree?.isRemote ?? false,
            startDate: degree ? new Date(degree.startDate) : undefined,
            endDate: degree?.endDate ? new Date(degree.endDate) : undefined,
            stillEngaged: degree?.stillEngaged ?? true,
            highlights: degree?.highlights ?? "",
            degreeLevel: degree?.level ?? "Bachelor"
        });

        if (this.isEditMode) {
            (this.state as any).id = degree!.id;
        }

        this.focusState = reactive<Record<string, boolean>>({
            name: false,
            description: false,
            institutionName: false,
            location: false,
            startDate: false,
            endDate: false,
            highlights: false,
            degreeLevel: false
        });
    }

    public setFocus(field: string, isFocused: boolean): void {
        if (this.focusState.hasOwnProperty(field)) {
            this.focusState[field] = isFocused;
        }
    }

    public getVariant(field: string): "outline" | "none" {
        return this.focusState[field] ? "outline" : "none";
    }

    public setError(field: string, message: string): void {
        this.errors[field] = message;
    }

    public clearErrors(): void {
        this.errors = {};
    }

    public clearError(field: string): void {
        delete this.errors[field];
    }

    public hasError(field: string): boolean {
        return !!this.errors[field];
    }

    public getError(field: string): string | undefined {
        return this.errors[field];
    }

    public validate(): boolean {
        this.clearErrors();

        try {
            const dataToValidate = {
                ...this.state,
                startDate: this.state.startDate instanceof Date 
                    ? this.state.startDate 
                    : new Date(this.state.startDate as string),
                endDate: this.state.endDate instanceof Date 
                    ? this.state.endDate 
                    : this.state.endDate 
                        ? new Date(this.state.endDate as string)
                        : undefined
            };

            this.schema.parse(dataToValidate);
            return true;
        } catch (error: unknown) {
            if (error instanceof z.ZodError) {
                error.issues.forEach(issue => {
                    const path = issue.path[0] as string;
                    this.errors[path] = issue.message;
                });
            }
            return false;
        }
    }

    public getFormData(): any {
        return {
            ...this.state,
            startDate: this.state.startDate instanceof Date
                ? this.state.startDate.toISOString().split('T')[0]
                : this.state.startDate,
            endDate: this.state.endDate instanceof Date
                ? this.state.endDate.toISOString().split('T')[0]
                : this.state.endDate || null
        };
    }

    public reset(): void {
        this.state.name = this._existingDegree?.name ?? "";
        this.state.description = this._existingDegree?.description ?? "";
        this.state.institutionName = this._existingDegree?.institutionName ?? "";
        this.state.location = this._existingDegree?.location ?? "";
        this.state.isRemote = this._existingDegree?.isRemote ?? false;
        this.state.startDate = this._existingDegree 
            ? new Date(this._existingDegree.startDate) 
            : undefined;
        this.state.endDate = this._existingDegree?.endDate
            ? new Date(this._existingDegree.endDate)
            : undefined;
        this.state.stillEngaged = this._existingDegree?.stillEngaged ?? true;
        this.state.highlights = this._existingDegree?.highlights ?? "";
        this.state.degreeLevel = this._existingDegree?.level ?? "Bachelor";
        this.clearErrors();
    }
}
