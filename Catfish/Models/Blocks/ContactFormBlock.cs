
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;


namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Contact Form Block", Category = "Content", Component = "contact-block", Icon = "fas fa-envelope")]
    public class ContactFormBlock : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ContactFormBlock>();

        [Field(Title = "Recipient", Placeholder = "Where the email will be send to")]
        public StringField SendTo { get; set; }
        [Field(Title = "Subject", Placeholder = "Subject of the email that you will receive from Contact form")]
        public StringField Subject { get; set; }
        [Field(Title = "Sucess Message", Placeholder = "Message to display to user after successfully send out the contact email")]
        public StringField SuccessMessage { get; set; }
    }
}
