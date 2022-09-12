import { Guid } from "guid-typescript";

export interface Workflow {
    id: Guid;
    name: string;
    description: string;
    states: string[];
    actions: WorkflowAction[];
}

export type WorkflowAction = FormSubmissionAction;

export interface FormSubmissionAction {
    id: Guid;
    name: string;
    description: string;
    formId: Guid;

}

export interface SubmissionOption {
    actionButton: string;
    validateForm: boolean;
    preState: string | null;
    postState: string;
}