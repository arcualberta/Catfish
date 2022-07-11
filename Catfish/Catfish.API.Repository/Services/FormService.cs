namespace Catfish.API.Repository.Services
{
    public class FormService : IFormService
    {
        /// <summary>
        /// Get the list of field templates that has been used by forms.
        /// </summary>
        /// <returns></returns>
        public Task<List<Field>> GetTFieldemplates()
        {
            List<Field> fieldTemplates = new List<Field>()
            {
                new CheckboxField(),
                new DataListField(),
                new DateField(),
                new DecimalField(),
                new EmailField(),
                new InfoSection(),
                new IntegerField(),
                new RadioField(),
                new SelectField(),
                new TextField()
            };

            foreach(var field in (fieldTemplates.Where(field => field is OptionsField).Select(field => field as OptionsField)))
                field?.Options.Add(new Option() { IsExtendedOption = false, Value = new List<Text>() { new Text() { Language = "en", Value = "Option 1" }, new Text() { Language = "en", Value = "Option 2" } } });

            return Task.FromResult(fieldTemplates);
        }
    }
}
