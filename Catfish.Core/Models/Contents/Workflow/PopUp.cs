using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class PopUp : XmlModel
    {
        public static readonly string TagName = "pop-up";
        public static readonly string TitleAtt = "title";
        public static readonly string BodyAtt = "body";
        public static readonly string MessageAtt = "message";
        public XmlModelList<Button> Buttons { get; set; }
        public string Title
        {
            get => GetAttribute(TitleAtt, null as string);
            set => SetAttribute(TitleAtt, value);
        }
        public string Body
        {
            get => GetAttribute(BodyAtt, null as string);
            set => SetAttribute(BodyAtt, value);
        }

        public string Message
        {
            get => GetAttribute(MessageAtt, null as string);
            set => SetAttribute(MessageAtt, value);
        }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Initializing the buttons list
            XElement buttonsistDefinition = GetElement("buttons", true);
            Buttons = new XmlModelList<Button>(buttonsistDefinition, true, "button");

        }

        public PopUp(XElement data)
            : base(data)
        {

        }
        public PopUp()
            : base(new XElement(TagName))
        {

        }
        public Button AddButtons(string text, string returnVal)
        {
            if (Buttons.FindByAttribute(Button.TextAtt, text) != null)
                throw new Exception(string.Format("Button {0} already exists.", text));

            Button newButton = new Button() { Text = text, Return = returnVal };
            Buttons.Add(newButton);
            return newButton;
        }
    }
}
