import { Guid } from "guid-typescript"
import { eEntityType } from "../../shared/constants";
import { FormData } from "../../shared/form-models";


export interface EntityData{
    id: Guid
    templateId: Guid
    entityType: eEntityType
    data: FormData[]
    subjectRelationships: Relationship[]
    objectRelationships: Relationship[]
    files: File[] | null,
    created: Date,
    updated?: Date | null,
    title: string,
    description: string | null
}

export interface TemplateEntry {
    //id: Guid | null;
    
    templateId: Guid
    templateName: string
}

export interface Relationship {
    subjectEntityId: Guid
    subjectEntity: EntityData
    objectEntityId: Guid
    objectEntity: EntityData
    name: string
    order: number
}






