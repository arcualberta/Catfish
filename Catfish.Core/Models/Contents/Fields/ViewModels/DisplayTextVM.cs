using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class DisplayTextVM
    {
        public Guid Id { get; set; }
        public string Format { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }

        public DisplayTextVM() { }

        //doesnt have format or language
        //public DisplayTextVM(MultilingualText src)
        //{
        //    Id = src.Id;
        //    Value = src.ConcatenatedContent;
        //}
        public DisplayTextVM(Text src)
        {
            Id = src.Id;
            Format = src.Format.ToString();
            Language = src.Language;
            Value = src.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void UpdateFieldValues(Text text)
        {
            text.Id = Id;
            text.Value = Value;
        }
    }
}
