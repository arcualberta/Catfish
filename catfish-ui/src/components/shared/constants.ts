
export enum eState { 
    Draft = "Draft", 
    Active = "Active", 
    Archived = "Archived",  
    Inactive="Inactive",
    Deleted = "Deleted" }

export enum eEntityType 
{
    Item="Item",
    Collection="Collection",
    Unknown="Unknown"
}

export enum eSearchTarget 
{
    Title="Title",
    Description="Description",
    TitleOrDescription="TitleOrDescription"
}

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
export enum eFieldConstraint {
    Contains,
    Equals,
    NotEquals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}