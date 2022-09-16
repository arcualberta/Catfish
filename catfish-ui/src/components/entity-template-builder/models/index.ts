import { Guid } from "guid-typescript"
import { eState } from "../../shared/constants";

export interface formEntry {
    name: string;
    formId: Guid;
}

export interface entityTemplate {
    id: Guid | null;
    created: Date | null;
    updated: Date | null;
    name: string | null;
    description: string | null;
    state: eState | null;
    metadateForms: formEntry[] | null;
    dataForms: formEntry[] | null;
}
