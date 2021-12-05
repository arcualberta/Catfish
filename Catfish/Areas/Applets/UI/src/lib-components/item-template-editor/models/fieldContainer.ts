import { Guid } from "guid-typescript";
import { Text, TextCollection} from "./textModels";

export interface Field {
    id: Guid;
    modelType: string;
    values: Text[];
    name: TextCollection | null;
    required: boolean;
    allowMultipleValues: boolean;
    readonly: boolean;
    refId: string;
    description: TextCollection | null;
}

export interface FieldContainer {
    id: Guid;
    modelType: string;
    fields: Field[];
    isRoot: boolean | false;
    name: TextCollection | null;
    description: TextCollection | null;
    isTemplate: boolean | false;
}