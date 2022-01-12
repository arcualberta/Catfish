﻿import { Guid } from "guid-typescript";
import { TextCollection, Text } from "./textModels";

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
    $type: string;
    modelType: string;
    name: TextCollection;
    required: boolean;
    allowMultipleValues: boolean;
    readonly: boolean;
    description: TextCollection;
    created: Date;
    updated: Date;
    cssClass: string;
    fieldCssClass: string;
}

export interface FieldContainer {
    id: Guid;
    templateId: Guid | null;
    $type: string;
    modelType: string;
    fields: {
        $type: string;
        $values: Field[];
    };
    isRoot: boolean | false;
    name: TextCollection | null;
    description: TextCollection | null;
    isTemplate: boolean | false;
    model: FieldContainerReference | null;
    source: FieldContainer[] | null;
}

export interface TextField extends Field {
    richText: boolean;
    rows: number;
    cols: number;
    maxWords: number;
    maxChars: number;
}

export interface MultilingualTextField extends TextField {
    values: {
        $type: string;
        $values: TextCollection[];
    } | null;
}

export interface MonolingualTextField extends TextField {
    values: Text[] | null;
}

export interface Option {
    id: Guid;
    $type: string;
    optionText: TextCollection | null;
    selected: boolean;
    extendedOption: boolean;
}
export interface OptionsField extends Field {
    options: Option[];
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

export interface FieldContainerReference extends Field {
    refId: Guid;
    refType: eRefType;
}

export interface FileReference {
    id: Guid;
    fileName: string;
    originalFileName: string;
    thumbnail: string;
    contentType: string;
    size: number;
    created: Date;
    updated: Date;
    cssClass: string;
    modelType: string;
    $type: string;
}

export interface AttachmentField extends Field {
    files: FileReference[];
    allowedExtensions: string[];
    maxFileSize: number;
}


export interface InfoSection extends Field {
    content: MonolingualTextField;
  
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

