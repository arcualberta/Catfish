import { Guid } from "guid-typescript";
import { FieldContainer } from  "./fieldContainer";

export interface ItemTemplate {
    id: Guid;
    status: string;
    templateName: string;
    modelType: string;
    metadatSets: FieldContainer[];
    dataContainer: FieldContainer[];
}