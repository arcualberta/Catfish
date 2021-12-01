import { Guid } from "guid-typescript";
import { Text } from "./textModels";

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