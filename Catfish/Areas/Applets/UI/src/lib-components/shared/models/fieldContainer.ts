import { Guid } from "guid-typescript";
import { Text, TextCollection} from "./textModels";

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
  values: Text[];
  name: TextCollection;
  required: boolean;
  allowMultipleValues: boolean;
  readonly: boolean;
  refId: string;
  description: TextCollection;
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