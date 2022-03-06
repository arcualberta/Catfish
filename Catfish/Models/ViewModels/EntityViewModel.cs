using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Catfish.Core.Models.Data.CFDataFile;

namespace Catfish.Models.ViewModels
{
    public class EntityViewModel
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public ICollection<EntityViewModel> Children { get; set; }

        public ICollection<MetadataSetViewModel> MetadataSets { get; set; }

        public ICollection<DataFileViewModel> Files { get; set; }

        public string[] LanguageCodes { get; private set; }

        public string EntityTypeName { get; private set; }

        public DateTime Created { get; private set; }
        
        public EntityViewModel()
        {
            Children = new List<EntityViewModel>();
            MetadataSets = new List<MetadataSetViewModel>();
            Files = new List<DataFileViewModel>();
        }

        public EntityViewModel(CFEntity entity, string[] languageCodes, IDictionary<string, EntityViewModel> previousEntities = null) : this()
        {
            this.Id = entity.Id;
            this.Guid = entity.Guid;
            this.LanguageCodes = languageCodes;
            this.EntityTypeName = entity.EntityType.Name;

            this.Created = entity.Created;

            Type entityType = entity.GetType();

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

            if (typeof(CFItem).IsAssignableFrom(entityType))
            {
                foreach(CFDataObject dataObject in ((CFItem)entity).DataObjects)
                {
                    if (typeof(CFDataFile).IsAssignableFrom(dataObject.GetType()))
                    {
                        Files.Add(new DataFileViewModel((CFDataFile)dataObject, entity.Id));
                    }
                }
            }
            
            if (typeof(CFAggregation).IsAssignableFrom(entityType))
            {
                
                foreach (CFEntity member in ((CFAggregation)entity).ChildMembers)
                {
                    if (!previousEntities.ContainsKey(member.Guid))
                    {
                        EntityViewModel child = new EntityViewModel(member, languageCodes, previousEntities);
                        Children.Add(child);
                    }
                }
            }
        }

        public IEnumerable<EntityViewModel> GetParents()
        {
            CatfishDbContext context = new CatfishDbContext();
            EntityService es = new EntityService(context);

            return es.GetEntityParents(Id).ToList().Select(e => new EntityViewModel(e, LanguageCodes));
        }

        public IEnumerable<FormFieldViewModel> GetAllFormFields()
        {
            return MetadataSets.SelectMany(m => m.Fields);
        }

        public IEnumerable<string> GetSelectedOptions(string name, string languageCode = null)
        {
            IEnumerable<string> results = GetAllFormFields().SelectMany((f) => {
                if (languageCode == null)
                {
                    var keys = f.Names.Where(n => n.Value == name).Select(n => n.Key);
                    return f.SelectedOptions.Where(v => keys.Contains(v.Key)).SelectMany(v => v.Value);
                }
                else
                {
                    var result = new List<string>();
                    if (f.Names[languageCode] == name)
                    {
                        result.AddRange(f.SelectedOptions[languageCode]);
                    }

                    return result;
                }
            });

            return results;
        }

        public IEnumerable<string> GetFieldValuesByName(string name, string languageCode = null)
        {
            IEnumerable<string> results = GetAllFormFields().SelectMany((f) => {
                if(languageCode == null)
                {
                    var keys = f.Names.Where(n => n.Value == name).Select(n => n.Key);
                    return f.Values.Where(v => keys.Contains(v.Key)).Select(v => v.Value);
                }
                else
                {
                    var result = new List<string>();
                    if (f.Names.Where(n => n.Value == name && n.Key == languageCode).Any())
                    {
                        result.AddRange(f.ValuesList.Where(v => v.ContainsKey(languageCode)).Select(v => v[languageCode]));
                    }

                    return result;
                }
            });

            return results;
        }

        public DataFileViewModel GetFirstDataFile(MimeType type, ref int depth, int maxDepth = int.MaxValue)
        {
            if(depth >= maxDepth)
            {
                return null;
            }

            foreach (DataFileViewModel file in Files)
            {
                if (file.MimeType == type)
                {
                    return file;
                }
            }

            // Perform a bredth first search.
            int minDepth = maxDepth;
            DataFileViewModel result = null;
            foreach (EntityViewModel child in Children)
            {
                int checkDepth = depth + 1;
                DataFileViewModel checkResult = child.GetFirstDataFile(type, ref checkDepth, minDepth);

                if (checkResult != null)
                {
                    minDepth = checkDepth;
                    result = checkResult;
                }
            }

            depth = minDepth;
            return result;
        }

        public DataFileViewModel GetFirstImage()
        {
            int depth = 0;
            return GetFirstDataFile(MimeType.Image, ref depth);
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
        public List<IDictionary<string, string>> ValuesList { get; set; }
        public IDictionary<string, List<string>> SelectedOptions { get; set; }
        public string ModelType { get; set; }

        public FormFieldViewModel()
        {
            Names = new Dictionary<string, string>();
            Values = new Dictionary<string, string>();
            ValuesList = new List<IDictionary<string, string>>();
            SelectedOptions = new Dictionary<string, List<string>>();

            ValuesList.Add(Values);
        }

        public FormFieldViewModel(FormField field, string[] languageCodes) : this()
        {
            var names = field.GetNames(true);
            var values = field.GetValues(false);

            IEnumerable<Option> options = typeof(OptionsField).IsAssignableFrom(field.GetType()) ? ((OptionsField)field).Options : new List<Option>();

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
                    if (Values.ContainsKey(value.LanguageCode))
                    {
                        Values = new Dictionary<string, string>();
                        ValuesList.Add(Values);

                        Values.Add(value.LanguageCode, value.Value);
                    }
                    else
                    {
                        Values.Add(value.LanguageCode, value.Value);
                    }
                }
            }


            foreach(string code in languageCodes)
            {
                SelectedOptions.Add(code, new List<string>());
            }
            
            foreach(var option in options)
            {
                if (option.Selected)
                {
                    foreach (var optionVal in option.Value)
                    {
                        if (languageCodes.Contains(optionVal.LanguageCode))
                        {
                            SelectedOptions[optionVal.LanguageCode].Add(optionVal.Value);
                        }
                    }
                }
            }
        }
    }

    public class DataFileViewModel
    {
        public int ParentId { get; set; }
        public MimeType MimeType { get; set; }

        public string Guid { get; set; }

        public DataFileViewModel()
        {

        }

        public DataFileViewModel(CFDataFile dataFile, int parentId) : this()
        {
            ParentId = parentId;
            Guid = dataFile.Guid;
            MimeType = dataFile.TopMimeType;
        }
    }
}