using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TableField : BaseField
    {
        public enum eRowTarget { Data, Footer }
        public int MinRows
        {
            get => GetAttribute("min-rows", 0);
            set => SetAttribute("min-rows", value);
        }

        public int? MaxRows
        {
            get => GetAttribute("max-rows", null as int?);
            set => SetAttribute("max-rows", value);
        }
        public bool AllowAddRows
        {
            get => GetAttribute("allow-add-rows", true);
            set => SetAttribute("allow-add-rows", value);
        }

        public TableRow TableHead { get; set; }
        public XmlModelList<TableRow> TableData { get; set; }
        public XmlModelList<TableRow> TableFooter { get; set; }

        //public bool ShowRowSum
        //{
        //    get => GetAttribute("show-row-sum", false);
        //    set => SetAttribute("show-row-sum", value);
        //}

        //public bool ShowColSum
        //{
        //    get => GetAttribute("show-col-sum", false);
        //    set => SetAttribute("show-col-sum", value);
        //}

        //private TableRow mRowSum;
        //public TableRow RowSum 
        //{ 
        //    get 
        //    { 
        //        if(mRowSum == null) 
        //            mRowSum = new TableRow(GetElement("row-sum", true));
        //        return mRowSum;
        //    } 
        //}
        //private TableRow mColSum;
        //public TableRow ColSum
        //{
        //    get
        //    {
        //        if (mColSum == null)
        //            mColSum = new TableRow(GetElement("col-sum", true));
        //        return mColSum;
        //    }
        //}

    public TableField() : base() { DisplayLabel = "Table"; }
        public TableField(XElement data) : base(data) { DisplayLabel = "Table"; }
        public TableField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Table"; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            TableHead = new TableRow(GetElement("table-head", true));         
            TableData = new XmlModelList<TableRow>(GetElement("table-data", true), true, TableRow.TagName);
            TableFooter = new XmlModelList<TableRow>(GetElement("table-footer", true), true, TableRow.TagName);
        }
        public override void UpdateValues(BaseField srcField)
        {
            TableField src = srcField as TableField;
            if (src == null)
                throw new Exception("The source field is null or is not a TableField");

            //Clearing all children exist in the destination, if any
            TableData.Clear();
            foreach (var row in src.TableData)
                InsertValues(row);

            //foreach (var srcChild in src.Children)
            //{
            //    var dstChild = ChildTemplate.Clone() as DataItem;
            //    dstChild.TemplateId = ChildTemplate.Id;
            //    dstChild.Id = srcChild.Id;
            //    dstChild.UpdateFieldValues(srcChild);
            //    Children.Add(dstChild);
            //}
        }

        /// <summary>
        /// This method has no meaning for the table field.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lang"></param>
        public override void SetValue(string value, string lang) { }

        public TableRow InsertValues(TableRow srcValues)
        {
            TableRow dstRow = new TableRow();
            for(int i=0; i<srcValues.Fields.Count; ++i)
            {
                BaseField src = srcValues.Fields[i];
                BaseField clone = TableHead.Fields[i].Clone() as BaseField;
                clone.RefId = TableHead.Fields[i].Id;
                clone.Id = src.Id;
                clone.UpdateValues(src);
                dstRow.Fields.Add(clone);
            }

            TableData.Add(dstRow);
            return dstRow;
        }
        public TableRow AppendRow(eRowTarget target = eRowTarget.Data)
        {
            TableRow row = new TableRow();
            foreach (var cell in TableHead.Fields)
            {
                BaseField clone = cell.Clone() as BaseField;
                clone.Id = Guid.NewGuid();

                // Set the reference ID of the clone to the corresponding cells in the header
                clone.RefId = cell.Id;

                //Updating row-model references in value expressions with the new ID
                clone.ValueExpression.ResolveDataModelIdReferences(clone.Id);

                row.AppendCell<BaseField>(clone);
            }
            switch(target)
            {
                case eRowTarget.Data:
                    TableData.Add(row);
                    break;
                case eRowTarget.Footer:
                    TableFooter.Add(row);
                    break;
            }

            return row;
        }
        public TableField AppendRows(int count, eRowTarget target = eRowTarget.Data)
        {
            for (int i = 0; i < count; ++i)
                AppendRow(target);

            return this;
        }

        public void SetColumnValues(int columnIndex, string[] values, string lang, bool markReadOnly = false)
        {
            //Make sure we have at least number of rows equal to the nimber of values
            if (TableData.Count < values.Length)
                AppendRows(values.Length - TableData.Count);

            for (int i = 0; i < values.Length; ++i)
            {
                TableData[i].Fields[columnIndex].SetValue(values[i], lang);
                TableData[i].Fields[columnIndex].Readonly = markReadOnly;
            }
        }
    }

    public class TableRow : FieldContainerBase
    {
        public static readonly string TagName = "table-row";
        //public XmlModelList<BaseField> Cells { get; set; }

        public TableRow() : base(TagName) { }
        public TableRow(XElement data) : base(data) { }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            //Cells = new XmlModelList<BaseField>(GetElement("cells", true), true, null);
        }

        public TableRow AppendCell<T>(T cellContent) where T : BaseField
        {
            Fields.Add(cellContent);
            //Cells.Add(cellContent);
            return this;
        }

        public void SetReadOnly(bool val = true, int[] cellIndices = null)
        {
            if (cellIndices == null)
                foreach (var field in Fields)
                    field.Readonly = val;
            else
                foreach (var idx in cellIndices)
                    Fields[idx].Readonly = val;
        }
    }
}
