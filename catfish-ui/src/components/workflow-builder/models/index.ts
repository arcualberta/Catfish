import { Guid } from "guid-typescript"
import { eRecipientType, eTriggerType, eEmailType } from "../../../components/shared/constants"
export interface Workflow {
    id: Guid;
    name: string;
    description: string;
    triggers: WorkflowTrigger[];
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

export interface WorkflowState {
    id: Guid;
    name: string;
    description: string | null;
}

export interface WorkflowRole {
    id: Guid;
    name: string;
    description: string | null;
}

export interface EmailTemplate {
    id: Guid;
    name: string;
    description: string | null;
    emailSubject: string;
    emailBody: string;
}

export interface Recipient {
    id: Guid;
    emailType: eEmailType;
    recipienType:eRecipientType;
    role: string | null;
    email: string | null;
}
export interface WorkflowTrigger {
    id: Guid;
    type: eTriggerType;
    name: string;
    description: string | null;
    templateId:Guid;
    recipients:Recipient[]
}
export interface TabNavigationDefinition {
    name: string;
    title: string;
}
