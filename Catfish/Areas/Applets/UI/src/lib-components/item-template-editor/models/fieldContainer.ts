import { Guid } from "guid-typescript";


export interface Text {
    id: Guid;
    modelType: string;
    value: string;
    format: string;
    language: string;
    rank: number;
    created: Date;
    updated: Date;
}

export interface Field {
    id: Guid;
    modelType: string;
    values: Text[];
}

export interface FieldContainer {
    id: Guid;
    modelType: string;
    fields: Field[];

}