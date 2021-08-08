using Catfish.Models.FormBuilder.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder
{
    public class Form
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int? ForeignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LinkText { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();

        public T AppendField<T>(int? foreignId, string fieldName, string fieldDescription = null, bool isRequired = false) where T:Field, new()
        {
            T field = new T()
            {
                ForeignId = foreignId,
                Name = fieldName,
                Description = fieldDescription,
                IsRequired = isRequired,
            };
            Fields.Add(field);
            return field;
        }

        public Field AppendField<T>(int? foreignId, string fieldName, string[] options, string fieldDescription = null, bool isRequired = false) where T : OptionField, new()
        {
            OptionField field = new T()
            {
                ForeignId = foreignId,
                Name = fieldName,
                Description = fieldDescription,
                IsRequired = isRequired
            }
            .AppendOptions(options);

            Fields.Add(field);
            return field;
        }

    }
}