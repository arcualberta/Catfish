
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
    Text="Text",
    Date = "Date",
    Number = "Number",
    Checkbox = "Checkbox",
    Email="Email",
    DropDown="DropDown",
    Radio="Radio"
}