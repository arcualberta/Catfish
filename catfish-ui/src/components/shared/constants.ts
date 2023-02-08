const splitCamelCase = (val: string): string => val.replace(/([a-z](?=[A-Z]))/g, '$1 ');


export enum eState { 
    Draft = "Draft", 
    Active = "Active", 
    Archived = "Archived",  
    Inactive="Inactive",
    Deleted = "Deleted" 
}
export const getStateLabel = (val: eState): string => splitCamelCase(eState[val])

export enum eEntityType 
{
    Item="Item",
    Collection="Collection",
    Unknown="Unknown"
}
export const getEntityTypeLabel = (val: eEntityType): string => splitCamelCase(eEntityType[val])

export enum eSearchTarget 
{
    Title="Title",
    Description="Description",
    TitleOrDescription="TitleOrDescription"
}
export const getSearchTargetLabel = (val: eSearchTarget): string => splitCamelCase(eSearchTarget[val])

export enum eFieldType{
    Text = 1,
    Date,
    Integer,
    Decimal,
    Email,
    Checkbox,
    DropDown,
    Radio
}
export const getFieldTypeLabel = (val: eFieldType): string => splitCamelCase(eFieldType[val])

export enum eFieldConstraint {
    Contains = 1,
    Equals,
    NotEquals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}
export const getFieldConstraintLabel = (val: eFieldConstraint): string => {
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
    return splitCamelCase(eFieldConstraint[val])
}
export const getFieldConstraintToolTip = (val: eFieldConstraint): string => {
    switch(val){
        case eFieldConstraint.Contains: return "Contains: filters entries containing any word in the input value."
        case eFieldConstraint.Equals:   return "Equals: filters entries containing the exact phrase specified by the input value."
        case eFieldConstraint.NotEquals: return "Not equals: filters entries that does not contain the phrase in the input value."
        case eFieldConstraint.GreaterThan: return "Greater than: filters entries containing a value greater than the input value."
        case eFieldConstraint.GreaterThanOrEqual: return "Greater than or equal: filters entries containing a value greater than or equal to the input value"
        case eFieldConstraint.LessThan: return "Less than: filters entries containing a value less than the input value"
        case eFieldConstraint.LessThanOrEqual: return "Less than or equal: filters entries containing a value less than or equal to the input value"
        default: return "Unknwon constraint"
    }
}
export const eFieldConstraintValues: eFieldConstraint[] = Object.keys(eFieldConstraint).filter(key => typeof eFieldConstraint[key as any] === 'number').sort().map(key => eFieldConstraint[key as any] as unknown as eFieldConstraint)


export enum eConstraintType{
    FieldConstraint = 1,
    FieldExpression
}
export enum eEmailType{
    To = 1,
    Cc,
    Bcc
}
export const eEmailTypeValues: eEmailType[] = Object.keys(eEmailType).filter(key => typeof eEmailType[key as any] === 'number').sort().map(key => eEmailType[key as any] as unknown as eEmailType)
export const getEmailTypeLabel = (val: eEmailType): string => splitCamelCase(eEmailType[val])
export enum eTriggerType{
    Email = 1
}
export const eTriggerTypeValues: eTriggerType[] = Object.keys(eTriggerType).filter(key => typeof eTriggerType[key as any] === 'number').sort().map(key => eTriggerType[key as any] as unknown as eTriggerType)
export const getTriggerTypeLabel = (val: eTriggerType): string => splitCamelCase(eTriggerType[val])
export enum eRecipientType{
    Role = 1,
    Owner,
    FormField,
    MetadataField,
    Email
}
export const eRecipientTypeValues: eRecipientType[] = Object.keys(eRecipientType).filter(key => typeof eRecipientType[key as any] === 'number').sort().map(key => eRecipientType[key as any] as unknown as eRecipientType)
export const getRecipientTypeLabel = (val: eRecipientType): string => splitCamelCase(eRecipientType[val])
export enum eButtonReturnType{
    true = 1,
    false
}
export const eByttonReturnTypeValues: eButtonReturnType[] = Object.keys(eButtonReturnType).filter(key => typeof eButtonReturnType[key as any] === 'number').sort().map(key => eButtonReturnType[key as any] as unknown as eButtonReturnType)
export const getButtonReturnTypeLabel = (val: eButtonReturnType): string => splitCamelCase(eButtonReturnType[val])

export enum eAuthorizedBy{
    Role = 1,
    Owner,
    Domain,
    FormField,
    MetadataField
}
export const eAuthorizedByValues: eAuthorizedBy[] = Object.keys(eAuthorizedBy).filter(key => typeof eAuthorizedBy[key as any] === 'number').sort().map(key => eAuthorizedBy[key as any] as unknown as eAuthorizedBy)
export const getAuthorizedByLabel = (val: eAuthorizedBy): string => splitCamelCase(eAuthorizedBy[val])
export enum eButtonTypes{
    Button = 1,
    Link
}
export const eByttonTypeValues: eButtonTypes[] = Object.keys(eButtonTypes).filter(key => typeof eButtonTypes[key as any] === 'number').sort().map(key => eButtonTypes[key as any] as unknown as eButtonTypes)
export const getButtonTypeLabel = (val: eButtonTypes): string => splitCamelCase(eButtonTypes[val])

export enum eFormView{
    EntrySlip = 1,
    ItemDetails,
    ItemEditView,
    ChildFormEntrySlip,
    ChildFormEditView
}
export const eFormViewValues: eFormView[] = Object.keys(eFormView).filter(key => typeof eFormView[key as any] === 'number').sort().map(key => eFormView[key as any] as unknown as eFormView)
export const getFormViewLabel = (val: eFormView): string => splitCamelCase(eFormView[val])
