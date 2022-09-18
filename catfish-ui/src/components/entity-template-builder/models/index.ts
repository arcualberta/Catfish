import { Guid } from "guid-typescript"
import { eState } from "../../shared/constants";
import { Form } from "../../shared/form-models";

export interface FormEntry {
    name: string;
    formId: Guid;
}

export interface EntityTemplate {
    id: Guid | null;
    name: string;
    description: string | null;
    state: eState;
    created: Date;
    updated: Date | null;
    forms: Form[];
    metadataForms: FormEntry[] | null;
    dataForms: FormEntry[] | null;
}
