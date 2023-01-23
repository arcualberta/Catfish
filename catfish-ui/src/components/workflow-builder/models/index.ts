
import { eRecipientType, eTriggerType, eEmailType } from "../../../components/shared/constants"

import { Guid } from "guid-typescript";
import { eButtonType  } from "./constants";

export interface Workflow {
    id: Guid;
    name: string;
    description: string;
    actions: WorkflowAction[];
    entityTemplateId: Guid;
    triggers: WorkflowTrigger[];
    roles: WorkflowRole[];
    emailTemplates: EmailTemplate[];   
    popups: object;   
}

export type WorkflowAction = FormSubmissionAction;

export interface FormSubmissionAction {
    id: Guid;
    name: string;
    description: string;
    formId: Guid;//not in the back end yet
    
    buttonType: eButtonType;
    buttonLabel: string;
    triggers: WorkflowTrigger[];
    permissions: WorkflowPermission[];
    frontEndStoreAction: string;
    frontEndViewTransition: object | null;
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

export interface WorkflowTrigger
{
    id: Guid;
    name: string;
    description: string | null;
    eTriggerType: eTriggerType;
}

export interface WorkflowPermission
{
    id: Guid;
    currentState: WorkflowState | null;
    newState: WorkflowState | null;
    isOwnerAuthorized: boolean;
    authorizedDomains: string[];
    authorizedRoles: WorkflowRole[];
}