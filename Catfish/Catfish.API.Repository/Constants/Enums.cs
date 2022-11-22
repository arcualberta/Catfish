namespace Catfish.API.Repository.Constants
{
    public enum eState 
    {
        Draft = 0, 
        Active, 
        Archived, 
        Inactive, 
        Deleted
    }

    public enum eEntityType 
    {
        Item,
        Collection,
        Unknown
    }
    public enum eSearchTarget
    {
        Title,
        Description,
        TitleOrDescription
    }

    public enum eTextType
    {
        ShortAnswer,
        Paragraph,
        RichText,
        AttachmentField
    }

}
