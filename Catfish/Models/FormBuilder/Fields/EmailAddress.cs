﻿using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class EmailAddress : ShortText
    {
        public EmailAddress() : base("Email") { }
        public override BaseField CreateDataFieldFor(Core.Models.Contents.Form dataModel)
            => CreateDataFieldFor<EmailField>(dataModel);
    }
}