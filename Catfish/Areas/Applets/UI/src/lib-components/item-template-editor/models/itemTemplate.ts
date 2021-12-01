import { Guid } from "guid-typescript";
import { FieldContainer } from "./fieldContainer";
import { TextCollection } from "./textModels"

export interface ItemTemplate {
    id: Guid;
    status: string;
    templateName: string;
    modelType: string;
    metadatSets: FieldContainer[];
    dataContainer: FieldContainer[];
    name: TextCollection | null;
    description: TextCollection | null;
    statusId: Guid | null;
}