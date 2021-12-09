import { Guid } from "guid-typescript";
import { TextCollection} from "./textModels";

//export enum eFieldType {
//  AttachmentField = "AttachmentField",
//  CheckboxField = "CheckboxField",
//  CompositeField = "CompositeField",
//  DateField = "DateField",
//  DecimalField = "DecimalField",
//  EmailField = "EmailField",
//  FieldContainerReference = "FieldContainerReference",
//  InfoSection = "InfoSection",
//  IntegerField = "IntegerField",
//  MonolingualTextField = "MonolingualTextField",
//  RadioField = "RadioField",
//  SelectField = "SelectField",
//  TableField = "TableField",
//  TextArea = "TextArea",
//  TextField = "TextField",
//}

export enum eRefType { undefined, data, metadata }

export enum eFieldType {
  AttachmentField,
  CheckboxField,
  CompositeField,
  DateField,
  DecimalField,
  EmailField,
  FieldContainerReference,
  InfoSection,
  IntegerField,
  MonolingualTextField,
  RadioField,
  SelectField,
  TableField,
  TextArea,
  TextField,
}

export interface Field {
  id: Guid;
  modelType: string;
  name: TextCollection;
  required: boolean;
  allowMultipleValues: boolean;
  readonly: boolean;
  description: TextCollection;
  created: Date;
  updated: Date;
  cssClass: string;
}

export interface FieldContainer {
    id: Guid;
    modelType: string;
    fields: Field[];
    isRoot: boolean | false;
    name: TextCollection | null;
    description: TextCollection | null;
    isTemplate: boolean | false;
    model: FieldContainerReference | null;
    source: FieldContainer[] | null;
}

export interface MultilingualTextField extends Field {
    values: TextCollection[] | null;
}

export interface MonolingualTextField extends Field {
    values: Text[] | null;
}

export interface Option {
    id: Guid;
    optionText: TextCollection | null;
    selected: boolean;
    extendedOption: boolean;
}
export interface OptionsField extends Field {
    options: Option[];
}

export interface FieldContainerReference extends Field {
    ReferenceId: Guid;
    RefType: eRefType;
}
export class OptionsFieldMethods {

    public static getSelectedFieldLabels(options: Option[]) {
        return options?.filter(opt => opt.selected)
            .map(opt => opt.optionText?.values
                .map(txt => txt.value)
                .join(" / ")
            )
            .join(", ")
    }
}

export class EntityModelMethods {

    public static getSource(source: FieldContainer[]) {
        return source;
    }
    public static getMetadataset(metadatasets: FieldContainer[], id: Guid) {
        return metadatasets.find(m => m.id == id);
    }

    public static getDataContainer(datacontainers: FieldContainer[], id: Guid) {
        return datacontainers.find(d => d.id == id);
    }

}

