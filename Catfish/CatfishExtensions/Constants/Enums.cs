using Newtonsoft.Json.Converters;

namespace CatfishExtensions.Constants
{
   // [JsonConverter(typeof(StringEnumConverter))]
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
        Item=0,
        Collection,
        Unknown
    }
    public enum eSearchTarget
    {
        Title=0,
        Description,
        TitleOrDescription
    }

    public enum eTextType
    {
        ShortAnswer=0,
        Paragraph,
        RichText,
        AttachmentField
    }

    public enum FieldType
    {
        ShortAnswer=0,
        Paragraph,
        RichText,
        Date,
        DateTime,
        Decimal,
        Integer,
        Email,
        Checkboxes,
        DataList,
        RadioButtons,
        DropDown,
        InfoSection,
        AttachmentField
    }

}
