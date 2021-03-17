using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class FieldVM
    {
        public Guid Id { get; set; }
        public bool AllowMultipleValues { get; set; }
        public string ModelType { get; set; }
        public List<DisplayTextVM> Name { get; set; } = new List<DisplayTextVM>();
        public List<DisplayTextVM> Description { get; set; } = new List<DisplayTextVM>();
        public List<Guid> ValueIds { get; set; } = new List<Guid>();
        public Dictionary<Guid, List<FieldValue>> ValueGroups { get; set; } = new Dictionary<Guid, List<FieldValue>>();

        public FieldVM() { }

        /// <summary>
        /// Constructure for TextFieds and TextAreas
        /// </summary>
        /// <param name="src"></param>
        public FieldVM(TextField src)
        {
            Init(src);
            UpdateTextValues(src);
        }

        /// <summary>
        /// Constructor for RadioField, SelectField, and CheckboxField
        /// </summary>
        /// <param name="src"></param>
        public FieldVM(OptionsField src)
        {
            Init(src);

            //TODO: Figure out how to represent radio options in this view-model structure
        }
        //this needs to go, too generic... but then how am i making a new field from incoming ItemVM?
        //src coming in as any of the subclasses, but then cant use above field constructors due to this, dont know how to get around this
        public FieldVM(BaseField src)
        {
            Init(src);

            if (typeof(TextField).IsAssignableFrom(src.GetType()) )
            {
                UpdateTextValues(src as TextField);

            }else if (typeof(DateField).IsAssignableFrom(src.GetType()) )
            { 
                UpdateTextValues(src as DateField);
            }
            
        }

        public void Init(BaseField src)
        {
            Id = src.Id;
            ModelType = src.GetType().ToString();
            AllowMultipleValues = src.AllowMultipleValues;
        }

        public void UpdateTextValues(TextField src)
        {
            ValueIds = new List<Guid>();
            ValueGroups = new Dictionary<Guid, List<FieldValue>>();

            Name = new List<DisplayTextVM>();
            Description = new List<DisplayTextVM>();

            //expect to need to loop these - but the superclass is a single item for each...
            foreach(Text txt in src.Name.Values)
            {
                Name.Add(new DisplayTextVM(txt));
            }

            foreach (Text txt in src.Description.Values)
            {
                Description.Add(new DisplayTextVM(txt));
            }
            //Name.Add(new DisplayTextVM(src.Name));
            //Description.Add(new DisplayTextVM(src.Description));

            //TODO: go over all values in 
            foreach (MultilingualValue multiLingualVal in src.Values)
            {
                //Create a new list of FieldValue objects
                var tmp = new List<FieldValue>();

                //Iterate throuh all individual languages in multiLingualVal and
                //create a new field value object for that language and add it to the
                //above list of FieldValue objects
                foreach(Text text in multiLingualVal.Values)
                {
                    tmp.Add(new FieldValue(text));
                }


                //Insert the list of field value objects into the dictionary
                ValueGroups.Add(multiLingualVal.Id, tmp);
                ValueIds.Add(multiLingualVal.Id);
            }
        }
        public void UpdateDataModel(BaseField targetField)
        {
            if (typeof(TextField).IsAssignableFrom(targetField.GetType()))
                UpdateDataModelValues(targetField as TextField); //This will capture text field and text areas
            else if (typeof(MonolingualTextField).IsAssignableFrom(targetField.GetType()))
                UpdateDataModelValues(targetField as MonolingualTextField);
            else if (typeof(OptionsField).IsAssignableFrom(targetField.GetType()))
                UpdateDataModelValues(targetField as OptionsField);
            else if (typeof(AttachmentField).IsAssignableFrom(targetField.GetType()))
                UpdateDataModelValues(targetField as AttachmentField);
            else if (typeof(TableField).IsAssignableFrom(targetField.GetType()))
                UpdateDataModelValues(targetField as TableField);
            else if (typeof(CompositeField).IsAssignableFrom(targetField.GetType()))
                UpdateDataModelValues(targetField as CompositeField);
            else
                throw new Exception(string.Format("UpdateDataModel is not implemented for {0}", targetField.GetType().FullName));

        }

        public void UpdateDataModelValues(TextField targetField)
        {
            //Iterating through all items in the data model and identifying the one that are not
            //within the view model. We need to delete them from the data model.
            var toBeDeleted = targetField.Values.Where(val => !ValueIds.Contains(val.Id)).ToList();
            foreach (var val in toBeDeleted)
                targetField.Values.Remove(val);

            //Iterating through values in the view model and updating corresponding existing ones
            //or inserting new ones to the data model
            foreach (Guid id in ValueIds)
            {
                //Find the corresponding Text model from the target field
                var targetMultilingualValue = targetField.Values.Where(txt => txt.Id == id).FirstOrDefault();

                if (targetMultilingualValue != null)
                {
                    var valueTranslations = ValueGroups[id];

                    //we iterate through each value in the valueTranslations, find the 
                    //corresponding entry in the targetMultilingualValue and update it.
                    foreach(var fieldVal in valueTranslations)
                    {
                        var targetTxt = targetMultilingualValue.Values.Where(txt => txt.Id == fieldVal.Id).FirstOrDefault();
                        
                        if (targetTxt == null)
                            throw new Exception(string.Format("No value exist with ID {0} in the field {1}", fieldVal.Id, targetMultilingualValue.Id));

                        targetTxt.Value = fieldVal.Value;
                    }
                }
                else
                {
                    //Here, we have a new value. We need to insert it.
                    var valueTranslations = ValueGroups[id];

                    targetMultilingualValue = new MultilingualValue() { Id = id };
                    targetField.Values.Add(targetMultilingualValue);

                    //We iterate through each value in the valueTranslations, find the 
                    //corresponding entry in the targetMultilingualValue and update it.
                    foreach (var fieldVal in valueTranslations)
                    {
                        var targetTxt = new Text()
                        {
                            Id = fieldVal.Id,
                            Value = fieldVal.Value
                        };
                        targetMultilingualValue.Values.Add(targetTxt);
                    }
                }
            }
        }

        public void UpdateDataModelValues(MonolingualTextField targetField)
        {
            //Iterating through all items in the data model and identifying the one that are not
            //within the view model. We need to delete them from the data model.
            List<Text> toBeDeleted = targetField.Values.Where(val => !ValueIds.Contains(val.Id)).ToList();
            foreach (var val in toBeDeleted)
                targetField.Values.Remove(val);

            //Iterating through values in the view model and updating corresponding existing ones
            //or inserting new ones to the data model
            foreach (Guid id in ValueIds)
            {
                //Find the corresponding Text model from the target field
                var targetTxt = targetField.Values.Where(txt => txt.Id == id).FirstOrDefault();

                if(targetTxt != null)
                {
                    var valueTranslations = ValueGroups[id];

                    //Since this is a case of a monolingual text field, we should only have one value
                    //in the above valueTranslations.
                    targetTxt.Value = valueTranslations.Select(fv => fv.Value).FirstOrDefault();
                }
                else
                {
                    //Here, we have a new value. We need to insert it.
                    var valueTranslations = ValueGroups[id];

                    targetField.Values.Add(new Text() { Value = valueTranslations.Select(fv => fv.Value).FirstOrDefault() });
                }
            }
        }
        public void UpdateDataModelValues(OptionsField targetField)
        {
            throw new NotImplementedException(string.Format("Not yet implemented for {0}", targetField.GetType().FullName));
        }

        public void UpdateDataModelValues(AttachmentField targetField)
        {
            throw new NotImplementedException(string.Format("Not yet implemented for {0}", targetField.GetType().FullName));
        }

        public void UpdateDataModelValues(TableField targetField)
        {
            throw new NotImplementedException(string.Format("Not yet implemented for {0}", targetField.GetType().FullName));
        }

        public void UpdateDataModelValues(CompositeField targetField)
        {
            throw new NotImplementedException(string.Format("Not yet implemented for {0}", targetField.GetType().FullName));
        }


        public void UpdateTextValues(DateField src)
        {
            ValueIds = new List<Guid>();
            ValueGroups = new Dictionary<Guid, List<FieldValue>>();

            Name = new List<DisplayTextVM>();
            Description = new List<DisplayTextVM>();
            foreach(Text txt in src.Name.Values)
            {
                Name.Add(new DisplayTextVM(txt));
            }

            foreach (Text txt in src.Description.Values)
            {
                Description.Add(new DisplayTextVM(txt));
            }
            //Name.Add(new DisplayTextVM(src.Name));
            //Description.Add(new DisplayTextVM(src.Description));

            //this is setting up for the valudId/ValueGroup reference to use the same id as the text
            //there is no containing object for the text because there is only 1 value for this field
            var tmpList = new List<FieldValue>();
            foreach (Text text in src.Values)
            {
                ValueIds.Add(text.Id);
                tmpList.Add(new FieldValue(text) );
                //this loop will only complete once because it is a monolingual value so this should be ok
                ValueGroups.Add(text.Id, tmpList);
            }
        }


        public BaseField ToDataModel()
        {
            throw new NotImplementedException();
        }
    }
}
