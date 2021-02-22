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
            throw new NotImplementedException();
        }

        public TableField AppendColumn<T>(string columnName, string lang) where T : BaseField
        {
            BaseField cell = Activator.CreateInstance(typeof(T)) as BaseField;
            cell.SetName(columnName, lang);
            TableHead.AppendCell(cell);
            return this;
        }

        public TableRow AppendRow()
        {
            TableRow row = new TableRow();
            foreach (var cell in TableHead.Cells)
                row.AppendCell<BaseField>(cell.Clone() as BaseField);
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

    public class TableRow : XmlModel
    {
        public static readonly string TagName = "table-row";
        public XmlModelList<BaseField> Cells { get; set; }

        public TableRow() : base() { }
        public TableRow(XElement data) : base(data) { }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Cells = new XmlModelList<BaseField>(GetElement("cells", true), true, null);
        }

        public TableRow AppendCell<T>(T cellContent) where T : BaseField
        {
            Cells.Add(cellContent);
            return this;
        }

    }
}
