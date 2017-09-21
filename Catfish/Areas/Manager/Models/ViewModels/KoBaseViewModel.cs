using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public abstract class KoBaseViewModel
    {
        public enum eStatus { Success = 1, Error, Warning }
        public int Id { get; set; }
        public string Message { get; set; }
        public eStatus Status { get; set; }
        public bool redirect { get; set; }
        public string url { get; set; }

        public KoBaseViewModel()
        {
            Message = "";
        }

        public static int GetBoundedArrayIndex(int candidate, int arraySize)
        {
            return (candidate < 0) ? 0 :
                (candidate < arraySize) ? candidate : arraySize - 1;
        }

        public object Error(string message)
        {
            Status = eStatus.Error;
            Message = message;
            return this;
        }

        public abstract void UpdateDataModel(object dataModel, CatfishDbContext db);
    }
}