import { Guid } from "guid-typescript";

import { ComponentLayout } from "./componentLayout";

export interface FieldLayout extends ComponentLayout {
    formTemplateId: Guid;
    fieldId: Guid;
    label: "Form Field";
    $type: string;
}