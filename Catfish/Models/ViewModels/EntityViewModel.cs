using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.ViewModels
{
    public class EntityViewModel
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public ICollection<EntityViewModel> Children { get; set; }

        public ICollection<MetadataSetViewModel> MetadataSets { get; set; }
        
        public EntityViewModel()
        {
            Children = new List<EntityViewModel>();
            MetadataSets = new List<MetadataSetViewModel>();
        }

        public EntityViewModel(CFEntity entity, string[] languageCodes, IDictionary<string, EntityViewModel> previousEntities = null) : this()
        {
            this.Id = entity.Id;
            this.Guid = entity.Guid;

            // Added to prevent circular child members.
            if(previousEntities == null)
            {
                previousEntities = new Dictionary<string, EntityViewModel>();
            }
            previousEntities.Add(this.Guid, this);

            foreach(CFMetadataSet metadataset in entity.MetadataSets)
            {
                MetadataSets.Add(new MetadataSetViewModel(metadataset, languageCodes));
            }

            if (typeof(CFAggregation).IsAssignableFrom(entity.GetType()))
            {
                foreach (CFEntity member in ((CFAggregation)entity).ChildMembers)
                {
                    if (previousEntities.ContainsKey(member.Guid))
                    {
                        Children.Add(previousEntities[member.Guid]);
                    }
                    else
                    {
                        Children.Add(new EntityViewModel(member, languageCodes, previousEntities));
                    }
                }
            }
        }

        public IEnumerable<FormFieldViewModel> GetAllFormFields()
        {
            return MetadataSets.SelectMany(m => m.Fields);
        }
    }

    public class MetadataSetViewModel
    {
        public ICollection<FormFieldViewModel> Fields { get; set; }

        public MetadataSetViewModel()
        {
            Fields = new List<FormFieldViewModel>();
        }

        public MetadataSetViewModel(CFMetadataSet metadataset, string[] languageCodes) : this()
        {
            foreach(FormField field in metadataset.Fields)
            {
                Fields.Add(new FormFieldViewModel(field, languageCodes));
            }
        }
    }

    public class FormFieldViewModel
    {
        public IDictionary<string, string> Names { get; set; }
        public IDictionary<string, string> Values { get; set; }
        public string ModelType { get; set; }

        public FormFieldViewModel()
        {
            Names = new Dictionary<string, string>();
            Values = new Dictionary<string, string>();
        }

        public FormFieldViewModel(FormField field, string[] languageCodes) : this()
        {
            var names = field.GetNames(true);
            var values = field.GetValues(false);

            foreach(var name in names)
            {
                if (languageCodes.Contains(name.LanguageCode))
                {
                    Names.Add(name.LanguageCode, name.Value);
                }
            }

            foreach(var value in values)
            {
                if (languageCodes.Contains(value.LanguageCode))
                {
                    Values.Add(value.LanguageCode, value.Value);
                }
            }
        }
    }

    public class FileViewModel
    {
        public string MetadataType { get; set; }

        public string Guid { get; set; }
    }
}