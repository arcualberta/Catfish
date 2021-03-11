using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

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

            //Here, we update the table data as follows:
            //  1. If the source data does NOT contain some of the rows in "this" object,
            //     then we delete those extra rows from "this" object
            //  2. If the source data contains rows that matches with Ids in "this" object, 
            //     then we update those rows
            //  3. If the source data contains "new" rows that do not exist in "this" object,
            //     then we inser them

            //Step #1
            var srcRowIds = src.TableData.Select(tr => tr.Id).ToList();
            var toBeDeleted = TableData.Where(tr => !srcRowIds.Contains(tr.Id)).ToList();
            foreach (var del in toBeDeleted)
                TableData.Remove(del);

            //Ste #2 and #3
            foreach (var srcRow in src.TableData)
            {
                //Finding the matching row
                var dstRow = TableData.Where(tr => tr.Id == srcRow.Id).FirstOrDefault();

                if (dstRow != null)
                    UpdateValues(srcRow, dstRow); //Step #2
                else
                    InsertValues(srcRow, eRowTarget.Data); //Step #3
            }


            //Table footer is not meant to be replaced in whole but we only change the values
            //of cells in each row.
            for (var r = 0; r< Math.Min(src.TableFooter.Count, TableFooter.Count); ++r)
                UpdateValues(src.TableFooter[r], TableFooter[r]);
        }

        /// <summary>
        /// This method has no meaning for the table field.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lang"></param>
        public override void SetValue(string value, string lang) { }

        public TableRow InsertValues(TableRow srcValues, eRowTarget target = eRowTarget.Data)
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

            if (target == eRowTarget.Data)
                TableData.Add(dstRow);
            else if (target == eRowTarget.Footer)
                TableFooter.Add(dstRow);

            return dstRow;
        }

        public void UpdateValues(TableRow src, TableRow dst)
        {
            for (var c = 0; c < Math.Min(src.Fields.Count, dst.Fields.Count); ++c)
            {
                dst.Fields[c].UpdateValues(src.Fields[c]);
            }
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

                //////Updating row-model references in value expressions with the new ID
                ////clone.ValueExpression.ResolveDataModelIdReferences(clone.Id);

                row.AppendCell<BaseField>(clone);
            }

            //Updating all references to table header's model IDs in value expressions
            //in the row with the new GUIDs of corresponding cells
            for (int i = 0; i < row.Fields.Count; ++i)
            {
                var cell = row.Fields[i];
                if (cell.HasValueExpression)
                {
                    for(int h = 0; h<TableHead.Fields.Count; ++h)
                    {
                        var oldGuid = TableHead.Fields[h].Id;
                        var newGuid = row.Fields[h].Id;
                        cell.ValueExpression.ReplaceReferences(oldGuid, newGuid);
                    }
                }
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

        /// <summary>
        /// We have not implemented this method for TableField
        /// </summary>
        /// <param name="srcField"></param>
        public override void CopyValue(BaseField srcField, bool overwrite = false)
        {

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
