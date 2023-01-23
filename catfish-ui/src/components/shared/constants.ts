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
export enum eTriggerType{
    Email = 1
}
export enum eRecipientType{
    Role = 1,
    Owner,
    FormField,
    MetadataField,
    Email
}
