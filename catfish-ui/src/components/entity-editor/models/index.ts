import { Guid } from "guid-typescript"
import { eEntityType } from "../../shared/constants";
import { Form, FormEntry, FieldEntry } from "../../shared/form-models";

public enum eEntityType 
{
    Item,
    Collection
}
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
