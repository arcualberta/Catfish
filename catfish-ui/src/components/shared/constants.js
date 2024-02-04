const splitCamelCase = (val) => val.replace(/([a-z](?=[A-Z]))/g, '$1 ');
export var eState;
(function (eState) {
    eState["Draft"] = "Draft";
    eState["Active"] = "Active";
    eState["Archived"] = "Archived";
    eState["Inactive"] = "Inactive";
    eState["Deleted"] = "Deleted";
})(eState || (eState = {}));
export const getStateLabel = (val) => splitCamelCase(eState[val]);
export var eEntityType;
(function (eEntityType) {
    eEntityType["Item"] = "Item";
    eEntityType["Collection"] = "Collection";
    eEntityType["Unknown"] = "Unknown";
})(eEntityType || (eEntityType = {}));
export const getEntityTypeLabel = (val) => splitCamelCase(eEntityType[val]);
export var eSearchTarget;
(function (eSearchTarget) {
    eSearchTarget["Title"] = "Title";
    eSearchTarget["Description"] = "Description";
    eSearchTarget["TitleOrDescription"] = "TitleOrDescription";
})(eSearchTarget || (eSearchTarget = {}));
export const getSearchTargetLabel = (val) => splitCamelCase(eSearchTarget[val]);
export var eFieldType;
(function (eFieldType) {
    eFieldType[eFieldType["Text"] = 1] = "Text";
    eFieldType[eFieldType["Date"] = 2] = "Date";
    eFieldType[eFieldType["Integer"] = 3] = "Integer";
    eFieldType[eFieldType["Decimal"] = 4] = "Decimal";
    eFieldType[eFieldType["Email"] = 5] = "Email";
    eFieldType[eFieldType["Checkbox"] = 6] = "Checkbox";
    eFieldType[eFieldType["DropDown"] = 7] = "DropDown";
    eFieldType[eFieldType["Radio"] = 8] = "Radio";
})(eFieldType || (eFieldType = {}));
export const getFieldTypeLabel = (val) => splitCamelCase(eFieldType[val]);
export var eFieldConstraint;
(function (eFieldConstraint) {
    eFieldConstraint[eFieldConstraint["Contains"] = 1] = "Contains";
    eFieldConstraint[eFieldConstraint["Equals"] = 2] = "Equals";
    eFieldConstraint[eFieldConstraint["NotEquals"] = 3] = "NotEquals";
    eFieldConstraint[eFieldConstraint["GreaterThan"] = 4] = "GreaterThan";
    eFieldConstraint[eFieldConstraint["GreaterThanOrEqual"] = 5] = "GreaterThanOrEqual";
    eFieldConstraint[eFieldConstraint["LessThan"] = 6] = "LessThan";
    eFieldConstraint[eFieldConstraint["LessThanOrEqual"] = 7] = "LessThanOrEqual";
})(eFieldConstraint || (eFieldConstraint = {}));
export const getFieldConstraintLabel = (val) => {
    /*switch(val){
        case eFieldConstraint.Contains: return "Containts"
        case eFieldConstraint.Equals:   return "="
        case eFieldConstraint.NotEquals: return "!="
        case eFieldConstraint.GreaterThan: return ">"
        case eFieldConstraint.GreaterThanOrEqual: return ">="
        case eFieldConstraint.LessThan: return "<"
        case eFieldConstraint.LessThanOrEqual: return "<="
        default: return ""
    }*/
    return splitCamelCase(eFieldConstraint[val]);
};
export const getFieldConstraintToolTip = (val) => {
    switch (val) {
        case eFieldConstraint.Contains: return "Contains: filters entries containing any word in the input value.";
        case eFieldConstraint.Equals: return "Equals: filters entries containing the exact phrase specified by the input value.";
        case eFieldConstraint.NotEquals: return "Not equals: filters entries that does not contain the phrase in the input value.";
        case eFieldConstraint.GreaterThan: return "Greater than: filters entries containing a value greater than the input value.";
        case eFieldConstraint.GreaterThanOrEqual: return "Greater than or equal: filters entries containing a value greater than or equal to the input value";
        case eFieldConstraint.LessThan: return "Less than: filters entries containing a value less than the input value";
        case eFieldConstraint.LessThanOrEqual: return "Less than or equal: filters entries containing a value less than or equal to the input value";
        default: return "Unknwon constraint";
    }
};
export const eFieldConstraintValues = Object.keys(eFieldConstraint).filter(key => typeof eFieldConstraint[key] === 'number').sort().map(key => eFieldConstraint[key]);
export var eConstraintType;
(function (eConstraintType) {
    eConstraintType[eConstraintType["FieldConstraint"] = 1] = "FieldConstraint";
    eConstraintType[eConstraintType["FieldExpression"] = 2] = "FieldExpression";
})(eConstraintType || (eConstraintType = {}));
export var eEmailType;
(function (eEmailType) {
    eEmailType[eEmailType["To"] = 1] = "To";
    eEmailType[eEmailType["Cc"] = 2] = "Cc";
    eEmailType[eEmailType["Bcc"] = 3] = "Bcc";
})(eEmailType || (eEmailType = {}));
export const eEmailTypeValues = Object.keys(eEmailType).filter(key => typeof eEmailType[key] === 'number').sort().map(key => eEmailType[key]);
export const getEmailTypeLabel = (val) => splitCamelCase(eEmailType[val]);
export var eTriggerType;
(function (eTriggerType) {
    eTriggerType[eTriggerType["Email"] = 1] = "Email";
})(eTriggerType || (eTriggerType = {}));
export const eTriggerTypeValues = Object.keys(eTriggerType).filter(key => typeof eTriggerType[key] === 'number').sort().map(key => eTriggerType[key]);
export const getTriggerTypeLabel = (val) => splitCamelCase(eTriggerType[val]);
export var eRecipientType;
(function (eRecipientType) {
    eRecipientType[eRecipientType["Role"] = 1] = "Role";
    eRecipientType[eRecipientType["Owner"] = 2] = "Owner";
    eRecipientType[eRecipientType["FormField"] = 3] = "FormField";
    eRecipientType[eRecipientType["MetadataField"] = 4] = "MetadataField";
    eRecipientType[eRecipientType["EmailcurrentState"] = 5] = "EmailcurrentState";
})(eRecipientType || (eRecipientType = {}));
export const eRecipientTypeValues = Object.keys(eRecipientType).filter(key => typeof eRecipientType[key] === 'number').sort().map(key => eRecipientType[key]);
export const getRecipientTypeLabel = (val) => splitCamelCase(eRecipientType[val]);
export var eButtonReturnType;
(function (eButtonReturnType) {
    eButtonReturnType[eButtonReturnType["true"] = 1] = "true";
    eButtonReturnType[eButtonReturnType["false"] = 2] = "false";
})(eButtonReturnType || (eButtonReturnType = {}));
export const eByttonReturnTypeValues = Object.keys(eButtonReturnType).filter(key => typeof eButtonReturnType[key] === 'number').sort().map(key => eButtonReturnType[key]);
export const getButtonReturnTypeLabel = (val) => splitCamelCase(eButtonReturnType[val]);
export var eAuthorizedBy;
(function (eAuthorizedBy) {
    eAuthorizedBy[eAuthorizedBy["Role"] = 1] = "Role";
    eAuthorizedBy[eAuthorizedBy["Owner"] = 2] = "Owner";
    eAuthorizedBy[eAuthorizedBy["Domain"] = 3] = "Domain";
    eAuthorizedBy[eAuthorizedBy["FormField"] = 4] = "FormField";
    eAuthorizedBy[eAuthorizedBy["MetadataField"] = 5] = "MetadataField";
})(eAuthorizedBy || (eAuthorizedBy = {}));
export const eAuthorizedByValues = Object.keys(eAuthorizedBy).filter(key => typeof eAuthorizedBy[key] === 'number').sort().map(key => eAuthorizedBy[key]);
export const getAuthorizedByLabel = (val) => splitCamelCase(eAuthorizedBy[val]);
export var eButtonTypes;
(function (eButtonTypes) {
    eButtonTypes[eButtonTypes["Button"] = 1] = "Button";
    eButtonTypes[eButtonTypes["Link"] = 2] = "Link";
})(eButtonTypes || (eButtonTypes = {}));
export const eByttonTypeValues = Object.keys(eButtonTypes).filter(key => typeof eButtonTypes[key] === 'number').sort().map(key => eButtonTypes[key]);
export const getButtonTypeLabel = (val) => splitCamelCase(eButtonTypes[val]);
export var eFormView;
(function (eFormView) {
    eFormView[eFormView["EntrySlip"] = 1] = "EntrySlip";
    eFormView[eFormView["ItemDetails"] = 2] = "ItemDetails";
    eFormView[eFormView["ItemEditView"] = 3] = "ItemEditView";
    eFormView[eFormView["ChildFormEntrySlip"] = 4] = "ChildFormEntrySlip";
    eFormView[eFormView["ChildFormEditView"] = 5] = "ChildFormEditView";
})(eFormView || (eFormView = {}));
export const eFormViewValues = Object.keys(eFormView).filter(key => typeof eFormView[key] === 'number').sort().map(key => eFormView[key]);
export const getFormViewLabel = (val) => splitCamelCase(eFormView[val]);
//# sourceMappingURL=constants.js.map