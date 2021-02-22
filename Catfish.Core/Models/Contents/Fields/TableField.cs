using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TableField : BaseField
    {
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

        public TableRow TableHead { get; set; }
        public XmlModelList<TableRow> TableData { get; set; }

        public TableField() : base() { DisplayLabel = "Table"; }
        public TableField(XElement data) : base(data) { DisplayLabel = "Table"; }
        public TableField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Table"; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            TableHead = new TableRow(GetElement("table-head", true));
            TableData = new XmlModelList<TableRow>(GetElement("table-data", true), true, TableRow.TagName);
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
        public TableRow AppendRow()
        {
            TableRow row = new TableRow();
            foreach (var cell in TableHead.Fields)
            {
                BaseField clone = cell.Clone() as BaseField;
                clone.Id = Guid.NewGuid();
                clone.RefId = cell.Id;
                row.AppendCell<BaseField>(clone);
            }
            TableData.Add(row);
            return row;
        }
        public TableField AppendRows(int count)
        {
            for (int i = 0; i < count; ++i)
                AppendRow();

            return this;
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

    }
}
