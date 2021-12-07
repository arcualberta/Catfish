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
}

export interface MultilingualTextInput extends Field{
    values: TextCollection[] | null;
}

export interface Option {
    id: Guid;
    optionText: TextCollection[] | null;
    selected: boolean;
    extendedOption: boolean;
}
export interface OptionsField extends Field {
    options: Option[] | null;
}
