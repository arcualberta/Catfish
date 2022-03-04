import { Guid } from "guid-typescript";

import { ComponentLayout } from "./componentLayout";

export interface FieldLayout extends ComponentLayout {
    formId: Guid;
    fieldId: Guid;
    label: "Form Field";
}