import { Guid } from "guid-typescript";
import { FieldContainer } from "../../shared/models/fieldContainer";
import { Text } from "../../shared/models/textModels";

export interface State{
    itemTemplateID: Guid | null;
    collectionID: Guid | null;
    groupId: Guid | null;
    reportFields: ReportField[] | null;
    reportData: ReportRow[] | null;
    detailedViewUrl: string | null;
    id: Guid | null;
}

export const state: State = {

    itemTemplateID: null,
    collectionID: null,
    groupId: null,
    reportFields: null,
    reportData: null,
    detailedViewUrl: null,
    id: null
}

export interface ReportField {
    formTemplateId: Guid | null;
    fieldId: Guid | null;
    formName: string | null;
    itemId: Guid|null;
    templateId: Guid | null;
    dataContainer: FieldContainer | null;
    metadataSets: FieldContainer | null;
}

//IMPORTANT: The following report TypeScript models do not match one-to-one in the "typed" JSON serialized c# models.
export interface ReportRow {
    cells: {
        itemId: Guid | null;
        values: ReportCell[];
    };
}

export interface ReportCell {
    formTemplateId: Guid | null;
    fieldId: Guid | null;
    values: ReportCellValue[];
}

export interface ReportCellValue {
    fieldType: string;
    formInstanceId: Guid | null;
    values: Text[];
}
