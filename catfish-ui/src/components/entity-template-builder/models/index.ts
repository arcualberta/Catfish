import { Guid } from "guid-typescript"
import { eState } from "../../shared/constants";
import { Form, FormEntry, FieldEntry } from "../../shared/form-models";

export interface EntityTemplateSettings{
    metadataForms: FormEntry[] | null;
    dataForms: FormEntry[] | null;
    titleField: FieldEntry | null;
    descriptionField: FieldEntry | null;
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
