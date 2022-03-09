import { Guid } from "guid-typescript";
import { FieldContainer } from "../../shared/models/fieldContainer";

export interface State{
    itemTemplateID: Guid | null;
    collectionID: Guid | null;
    groupId: Guid | null;
    reportFields: ReportField[] | null;

}
export interface ReportField {
    formId: Guid | null;
    fieldId: Guid | null;
    formName: string | null;
    itemId: Guid|null;
    templateId: Guid | null;
    dataContainer:FieldContainer | null;
    metadataSets:FieldContainer | null;
}
export const state: State = {

    itemTemplateID: null,
    collectionID: null,
    groupId: null,
    reportFields: null
}