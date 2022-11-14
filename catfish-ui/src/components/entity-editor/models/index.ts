import { Guid } from "guid-typescript"
import { eEntityType } from "../../shared/constants";
import { FormData } from "../../shared/form-models";
import { EntityEntry, ListEntry } from '@/components/shared'

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

export interface TemplateEntry extends ListEntry {
    //id: Guid | null;
    
   // templateId: Guid
    //templateName: string
}

export interface Relationship {
    subjectEntityId: Guid
    subjectEntity: EntityData
    objectEntityId: Guid
    objectEntity: EntityData
    name: string
    order: number
}


export interface EntitySearchResult
{
    result: EntityEntry[];
    offset: number
    total: number
}



