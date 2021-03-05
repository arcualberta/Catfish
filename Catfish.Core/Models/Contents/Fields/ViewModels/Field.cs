using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class Field
    {
        public Guid Id { get; set; }
        public bool AllowMultipleValues { get; set; }
        public string ModelType { get; set; }
        public List<DisplayTextVM> Name { get; set; } = new List<DisplayTextVM>();
        public List<DisplayTextVM> Description { get; set; } = new List<DisplayTextVM>();
        public List<Guid> ValueIds { get; set; } = new List<Guid>();
        public Dictionary<Guid, List<FieldValue>> ValueGroups { get; set; } = new Dictionary<Guid, List<FieldValue>>();

        public Field() { }

        /// <summary>
        /// Constructure for TextFieds and TextAreas
        /// </summary>
        /// <param name="src"></param>
        public Field(TextField src)
        {
            Init(src);
            UpdateTextValues(src);
        }

        /// <summary>
        /// Constructor for RadioField, SelectField, and CheckboxField
        /// </summary>
        /// <param name="src"></param>
        public Field(OptionsField src)
        {
            Init(src);

            //TODO: Figure out how to represent radio options in this view-model structure
        }
        //this needs to go, too generic... but then how am i making a new field from incoming ItemVM?
        //src coming in as any of the subclasses, but then cant use above field constructors due to this, dont know how to get around this
        public Field(BaseField src)
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
            Name.Add(new DisplayTextVM(src.Name));
            Description.Add(new DisplayTextVM(src.Description));

            //TODO: go over all values in 
            foreach(MultilingualValue multiLingualVal in src.Values)
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

        
        public void UpdateTextValues(DateField src)
        {
            ValueIds = new List<Guid>();
            ValueGroups = new Dictionary<Guid, List<FieldValue>>();

            //expect to need to loop these - but the superclass is a single item for each...
            Name.Add(new DisplayTextVM(src.Name));
            Description.Add(new DisplayTextVM(src.Description));

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
