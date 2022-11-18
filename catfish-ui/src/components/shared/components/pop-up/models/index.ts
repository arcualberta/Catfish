import { Guid } from "guid-typescript";

export interface ConfirmationPopUpModel {
    popupHeadder: string | null,
    popupBody: string | null,
    okButton: string | null,
    cancelButton: string | null,
    isVisible:boolean | false
};