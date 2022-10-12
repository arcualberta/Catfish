import { Guid } from "guid-typescript"
import { eEntityType } from "../../shared/constants";
import { FormData } from "../../shared/form-models";


export interface Entity{
    id: Guid
    templateId: Guid
    entityType: eEntityType
    data: FormData[]
    subjectRelationships: Relationship[]
    objectRelationships: Relationship[]
}

export interface TemplateEntry {
    //id: Guid | null;
    
    templateId: Guid
    templateName: string
}
export interface Relationship {
    subjectEntityId: Guid
    subjectEntity: Entity
    objectEntityId: Guid
    objectEntity: Entity
    name: string
    order: number
}
