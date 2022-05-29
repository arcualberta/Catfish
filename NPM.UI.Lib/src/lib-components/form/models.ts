import { Guid } from "guid-typescript"

import { eValidationStatus, eRefType } from './enumerations'

export interface Text {
    id: Guid;
    $type: string;
    modelType: string;
    value: string;
    format: string;
    language: string;
    rank: number;
    created: Date;
    updated: Date;

}

export interface TextCollection {
    id: Guid;
    $type: string;
    modelType: string;
    values: {
        $type: string;
        $values: Text[];
    };
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
    validationStatus: eValidationStatus | null;
}

export interface FieldContainerReference extends Field {
    refId: Guid;
    refType: eRefType;
}

export interface FieldContainer {
    id: Guid;
    templateId: Guid | null;
    parentId: Guid | null;
    $type: string;
    modelType: string;
    fields: {
        $type: string;
        $values: Field[];
    };
    childFieldContainers: {
        $type: string;
        $values: FieldContainer[];
    };
    isRoot: boolean | false;
    name: TextCollection | null;
    description: TextCollection | null;
    isTemplate: boolean | false;
    model: FieldContainerReference | null;
    source: FieldContainer[] | null;
    validationStatus: eValidationStatus | null;
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
    $type: string;
    modelType: string;
    values: {
        $type: string;
        $values: Text[];
    } | null;
}

export interface Option {
    id: Guid;
    $type: string;
    optionText: TextCollection | null;
    selected: boolean;
    extendedOption: boolean;
}
export interface OptionsField extends Field {
    options: {
        $type: string;
        $values: Option[];
    }
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
    files: {
        $type: string,
        $values: FileReference[];
    }
    allowedExtensions: string[];
    maxFileSize: number;
}

export interface AudioRecorderField extends Field {
    files: FileReference[];

    format: string;
}


export interface InfoSection extends Field {
    content: MonolingualTextField;

}


export interface FlattenedFiledSModel {
    modified: boolean;
    flattenedTextModels: { [key: string]: Text };
    flattenedOptionModels: { [key: string]: Option };
    flattenedFileModels: { [key: string]: File[] };
}
