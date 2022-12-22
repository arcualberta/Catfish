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
export const getFieldConstraintLabel = (val: eFieldConstraint): string => splitCamelCase(eFieldConstraint[val])
export const eFieldConstraintValues: eFieldConstraint[] = Object.keys(eFieldConstraint).filter(key => typeof eFieldConstraint[key as any] === 'number').sort().map(key => eFieldConstraint[key as any] as unknown as eFieldConstraint)


export enum eConstraintType{
    FieldConstraint = 1,
    FieldExpression
}