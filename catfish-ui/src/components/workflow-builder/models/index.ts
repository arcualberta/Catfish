
import { eRecipientType, eTriggerType, eEmailType, eAuthorizedBy, eButtonTypes, eFormView } from "../../../components/shared/constants"

import { Guid } from "guid-typescript";

export interface Workflow {
    id: Guid;
    name: string;
    description: string;
    entityTemplateId: Guid;
    actions: WorkflowAction[];
    states: WorkflowState[];
    roles: WorkflowRole[];
    triggers: WorkflowTrigger[];
    emailTemplates: WorkflowEmailTemplate[];   
    popups: WorkflowPopup[];   
}
export interface WorkflowAction{
    id: Guid;
    name: string;
    description: string | null;
    formTemplate: Guid;
    formView: eFormView;
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
    currentStateId: Guid;
    authorizedBy: eAuthorizedBy;
    authorizedRoleId: Guid | null;
    authorizedDomain: string | null;
    authorizedFormId: Guid | null;
    authorizedFeildId: Guid | null;
    authorizedMetadataFormId: Guid | null;
    authorizedMetadataFeildId: Guid | null;
}
export interface Button{
    id: Guid;
    type: eButtonTypes;
    label: string;
    currentStateId: Guid;
    nextStateId: Guid;
    popupId: Guid | null;
    triggers: Array<Guid>;
}
export interface Recipient {
    id: Guid;
    emailType: eEmailType;
    recipienType:eRecipientType;
    roleId: Guid | null;
    email: string | null;
    FormId: Guid | null;
    FeildId: Guid | null;
    MetadataFormId: Guid | null;
    MetadataFeildId: Guid | null;
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
