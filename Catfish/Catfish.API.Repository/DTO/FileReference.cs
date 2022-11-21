namespace Catfish.API.Repository.DTO
{
    public class FileReference
    {
        public  Guid Id {get;set;}
        string FileName { get; set; }
        string OriginalFileName { get; set; }
        string Thumbnail { get; set; }
        string ContentType { get; set; }
        int Size { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    
        Guid FormDataId { get; set; }
        Guid FieldId { get; set; }
    }
}
