import { Guid } from "guid-typescript"
import { eState } from "../../shared/constants";
import { Form, FormEntry, FieldEntry } from "../../shared/form-models";
export interface Entity{
    id: Guid
    templateId: Guid
    entityType: eEntityType
    data: FormData[]
}

export interface TemplateEntry {
    id: Guid | null;
    
    templateId: Guid
    templateName: string
}
