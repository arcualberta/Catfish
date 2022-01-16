﻿import { Guid } from "guid-typescript";

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
    concatenatedContent: string;
}
