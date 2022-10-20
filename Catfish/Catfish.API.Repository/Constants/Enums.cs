namespace Catfish.API.Repository.Constants
{
    public enum eState 
    { 
        Published,
        UnPublished,
        Archived,
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

}
