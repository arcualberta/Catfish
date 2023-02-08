namespace Catfish.API.Repository.Models.Workflow
{
    public enum eTriggerType
    {
        Email
    }
    public enum eEmailType
    {
        To,
        Cc,
        Bcc
    }
    public enum eRecipientType
    {
        Role,
        Owner,
        FormField,
        MetadataField,
        Email
    }
    public enum eFormView
    {
        EntrySlip,
        ItemDetails,
        ItemEditView,
        ChildFormEntrySlip,
        ChildFormEditView
    }
    public enum eButtonTypes
    {
        Button,
        Link
    }
    public enum eAuthorizedBy
    {
        Role,
        Owner,
        Domain,
        FormField,
        MetadataField
    }
}
