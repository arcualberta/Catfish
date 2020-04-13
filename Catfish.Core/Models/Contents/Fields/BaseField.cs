using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class BaseField : XmlModel
    {
        public const string FieldTagName = "field";

        public MultilingualText Name { get; protected set; }
        public MultilingualText Description { get; protected set; }

        public BaseField() : base(FieldTagName) { }
        public BaseField(XElement data) : base(data) { }

        public BaseField(string name, string desc, string lang = null)
            : base(FieldTagName)
        {
            Name = new MultilingualText(GetElement(Entity.NameTag, true));
            Name.SetContent(name, lang);

            Description = new MultilingualText(GetElement("description", true));
            Description.SetContent(desc, lang);
        }

        public override void Initialize()
        {

        }

        /*
                /// <summary>
                /// Name in specific languages
                /// </summary>
                protected List<Text> mName = new List<Text>();
                public void SetName(string name, string lang = null)
                {
                    if (string.IsNullOrEmpty(lang))
                        lang = ConfigHelper.DefaultLanguageCode;

                    Text nameInGivenLanguage = mName.Where(n => n.Language == lang).FirstOrDefault();
                    if (nameInGivenLanguage == null)
                    {
                        nameInGivenLanguage = new Text(name, lang);
                        GetElement(Entity.NameTag, true).Add(nameInGivenLanguage.Data);
                        mName.Add(nameInGivenLanguage);
                    }
                    else
                        nameInGivenLanguage.Data.Value = name;
                }

                public string GetName(string lang)
                {
                    if (string.IsNullOrEmpty(lang))
                        lang = ConfigHelper.DefaultLanguageCode;

                    Text selected = mName.Where(n => n.Language == lang).FirstOrDefault();
                    if (selected == null)
                        selected = mName.Where(n => !string.IsNullOrEmpty(n.Value)).FirstOrDefault();

                    return selected != null ? selected.Value : null;
                }
        */

        /// <summary>
        /// Description in specific languages
        /// </summary>
        protected List<Text> mDesc = new List<Text>();
        public void SetDescription(string name, string lang = null)
        {
            if (string.IsNullOrEmpty(lang))
                lang = ConfigHelper.DefaultLanguageCode;

            Text descInGivenLanguage = mDesc.Where(n => n.Language == lang).FirstOrDefault();
            if (descInGivenLanguage == null)
            {
                descInGivenLanguage = new Text(name, lang);
                GetElement(Entity.DescriptionTag, true).Add(descInGivenLanguage.Data);
                mDesc.Add(descInGivenLanguage);
            }
            else
                descInGivenLanguage.Data.Value = name;
        }

        public string GetDescription(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                lang = ConfigHelper.DefaultLanguageCode;

            Text selected = mDesc.Where(n => n.Language == lang).FirstOrDefault();
            if (selected == null)
                selected = mDesc.Where(n => !string.IsNullOrEmpty(n.Value)).FirstOrDefault();

            return selected != null ? selected.Value : null;
        }
    }
}
