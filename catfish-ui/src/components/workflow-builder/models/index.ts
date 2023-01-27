
import { eRecipientType, eTriggerType, eEmailType, eAuthorizedBy, eButtonTypes } from "../../../components/shared/constants"

import { Guid } from "guid-typescript";

export interface Workflow {
    id: Guid;
    name: string;
    description: string;
    states: WorkflowState[];
    actions: WorkflowAction[];
    entityTemplateId: Guid;
    triggers: WorkflowTrigger[];
    roles: WorkflowRole[];
    emailTemplates: WorkflowEmailTemplate[];   
    popups: WorkflowPopup[];   
}
export interface WorkflowAction{
    id: Guid;
    name: string;
    description: string | null;
    formTemplate: Guid;
    formView: string;
    buttons: Button[];
    authorizations: Authorization[];
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
export interface WorkflowEmailTemplate {
    id: Guid;
    name: string;
    description: string | null;
    emailSubject: string;
    emailBody: string;
}
export interface WorkflowTrigger {
    id: Guid;
    type: eTriggerType;
    name: string;
    description: string | null;
    templateId:Guid;
    recipients:Recipient[]
}
export interface WorkflowPopup {
    id: Guid;
    title: string;
    Message: string;
    buttons:PopupButton[]
}
export interface Authorization{
    id: Guid;
    currentState: Guid;
    authorizedBy: eAuthorizedBy;
    authorizedRole: string | null;
    authorizedDomain: string | null;
    authorizedFormId: Guid | null;
    authorizedFeildId: Guid | null;
    authorizedMetadataFormId: Guid | null;
    authorizedMetadataFeildId: Guid | null;
}
export interface Button{
    id: Guid;
    type: eButtonTypes;
    lable: string;
    currentStateId: Guid;
    nextStateId: Guid;
    popupId: Guid | null;
    triggers: Array<Guid>;
}
export interface Recipient {
    id: Guid;
    emailType: eEmailType;
    recipienType:eRecipientType;
    role: string | null;
    email: string | null;
}
export interface PopupButton {
    id: Guid;
    text: string;
    returnValue: string;
}
export interface TabNavigationDefinition {
    name: string;
    title: string;
}
