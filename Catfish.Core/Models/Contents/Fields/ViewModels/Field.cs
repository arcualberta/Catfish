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
        public List<DisplayText> Name { get; set; } = new List<DisplayText>();
        public List<DisplayText> Description { get; set; } = new List<DisplayText>();
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
            UpdateTextValyes(src);
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

        public void Init(BaseField src)
        {
            Id = src.Id;
            ModelType = src.GetType().ToString();
            AllowMultipleValues = src.AllowMultipleValues;
        }

        public void UpdateTextValyes(TextField src)
        {
            ValueIds = new List<Guid>();
            ValueGroups = new Dictionary<Guid, List<FieldValue>>();

            //TODO: go over all values in 
            foreach(MultilingualValue multiLingualVal in src.Values)
            {
                //Add the ID of the multiLingualVal to ValueIds


                //Create a new list of FieldValue objects

                //Iterate throuh all individual languages in multiLingualVal and
                //create a new field value object for that langiage and add it to the
                //above list of FieldValue objects


                //Inser the list of field value objects into the dictionary
            }
        }


        public BaseField ToDataModel()
        {
            throw new NotImplementedException();
        }
    }
}
