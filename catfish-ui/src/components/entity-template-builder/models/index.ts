import { Guid } from "guid-typescript"
import { eState } from "../../shared/constants";
import { Form } from "../../shared/form-models";

export interface FormEntry {
    /**
     * A unique form-entry ID used for the UI purposes
     * */
    id: Guid;
    name: string;
    formId: Guid;
}
export interface EntityTemplateSettings{
    metadataForms: FormEntry[] | null;
    dataForms: FormEntry[] | null;
}

export interface EntityTemplate {
    id: Guid | null;
    name: string;
    description: string | null;
    state: eState;
    created: Date;
    updated: Date | null;
    entityTemplateSettings: EntityTemplateSettings;
    forms: Form[] | null;
}
