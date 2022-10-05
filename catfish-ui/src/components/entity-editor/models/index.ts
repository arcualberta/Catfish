import { Guid } from "guid-typescript"
import { eEntityType } from "../../shared/constants";
import { FormData } from "../../shared/form-models";


export interface Entity{
    id: Guid
    templateId: Guid
    entityType: eEntityType
    data: FormData[]
}

export interface TemplateEntry {
    //id: Guid | null;
    
    templateId: Guid
    templateName: string
}
