import { Guid } from "guid-typescript";
import { FieldContainer } from "../../shared/models/fieldContainer";
import { Text } from "../../shared/models/textModels";

export interface State{
    itemTemplateID: Guid | null;
    collectionID: Guid | null;
    groupId: Guid | null;
    reportFields: ReportField[] | null;
    reportData: Report | null;
    detailedViewUrl: string | null;
    templateStatus: SystemStatus[] | null;
    id: Guid | null;
    offset: number;
    pageSize: number;
    siteUrl: string | null;
}

export const state: State = {

    itemTemplateID: null,
    collectionID: null,
    groupId: null,
    reportFields: null,
    reportData: null,
    detailedViewUrl: null,
    templateStatus: null,
    id: null,
    offset: 0,
    pageSize: 25,
    siteUrl: null
}

export interface ReportField {
    formTemplateId: Guid | null;
    fieldId: Guid | null;
    formName: string | null;
    itemId: Guid | null;
    created: string | null;
    status: string | null;
    templateId: Guid | null;
    dataContainer: FieldContainer | null;
    metadataSets: FieldContainer | null;
}

//IMPORTANT: The following report TypeScript models do not match one-to-one in the "typed" JSON serialized c# models.
export interface ReportRow {
    cells: {
        itemId: Guid | null;
        created: string | null;
        status: string | null;
        values: ReportCell[];
    };
}

export interface Report {
    offset: number;
    total: number;
    rows: ReportRow[] | null;
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

export interface SystemStatus {
    id: Guid;
    entityTemplateId: Guid;
    status: string;
    normalizedStatus: string;
}
export interface SearchParams {
    startDate: Date | null;
    endDate: Date | null;
    freeText: string | null;
    status: Guid | null;
}
