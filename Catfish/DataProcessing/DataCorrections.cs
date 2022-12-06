using Catfish.API.Repository.Constants;
using Catfish.Test.Helpers;
using CatfishExtensions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class DataCorrections
    {
        public readonly TestHelper _testHelper;
        public DataCorrections()
        {
            _testHelper = new TestHelper();
        }

        [Fact]
        public void ChangeStringFieldTypesToNumericEnums()
        {
            var db = _testHelper.Db;

            foreach(var form in db.Forms)
            {
                foreach (var enumVal in Enum.GetValues(typeof(FieldType))){
                    var str = string.Format("\"{0}\"", enumVal.ToString());
                    form.SerializedFields = form.SerializedFields.Replace(str, ((int)enumVal).ToString(), StringComparison.OrdinalIgnoreCase);
                }
                form.SerializedFields = form.SerializedFields.Replace("\"SingleLine\"", ((int)FieldType.ShortAnswer).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Short Answer\"", ((int)FieldType.ShortAnswer).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Rich Text\"", ((int)FieldType.RichText).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Date Time\"", ((int)FieldType.DateTime).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Check Boxes\"", ((int)FieldType.Checkboxes).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Data List\"", ((int)FieldType.DataList).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Radio Buttons\"", ((int)FieldType.RadioButtons).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Drop Down\"", ((int)FieldType.DropDown).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Info Section\"", ((int)FieldType.InfoSection).ToString());
                form.SerializedFields = form.SerializedFields.Replace("\"Attachment Field\"", ((int)FieldType.AttachmentField).ToString());
            }

            db.SaveChanges();

        }
    }
}
