import { Guid } from "guid-typescript"
import { eState } from "../../shared/constants";
import { FormTemplate, FieldEntry } from "../../shared/form-models";
import { FormEntry } from "../../shared";

export interface EntityTemplateSettings{
    metadataForms: FormEntry[];
    dataForms: FormEntry[];
    titleField: FieldEntry | null;
    descriptionField: FieldEntry | null;
    mediaField: FieldEntry | null;
}

export interface EntityTemplate {
    id: Guid | null;
    name: string;
    description: string;
    state: eState;
    created: Date;
    updated: Date | null;
    entityTemplateSettings: EntityTemplateSettings;
    forms: FormTemplate[];
}
