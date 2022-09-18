import { Guid } from "guid-typescript"
import { eState } from "../../shared/constants";

export interface FormEntry {
    name: string;
    formId: Guid;
}

export interface EntityTemplate {
    id: Guid | null;
    created: Date | null;
    updated: Date | null;
    name: string | null;
    description: string | null;
    state: eState | null;
    metadataForms: FormEntry[] | null;
    dataForms: FormEntry[] | null;
}
