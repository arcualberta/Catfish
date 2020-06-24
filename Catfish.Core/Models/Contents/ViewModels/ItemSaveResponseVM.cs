using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.ViewModels
{
    public class ItemSaveResponseVM
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Item Model { get; set; }

        public ItemSaveResponseVM()
        {

        }

        public ItemSaveResponseVM(string status, string message)
            : this(status, message, null)
        {

        }

        public ItemSaveResponseVM(string status, string message, Item model)
        {
            Status = status;
            Message = message;
            Model = model;
        }
    }
}
