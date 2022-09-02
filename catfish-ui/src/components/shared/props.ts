import { PropType } from 'vue';
import { Guid } from "guid-typescript";

/**
 * A*/
export interface DataAttribute {
    [key: string]: string | number | null;
}

export interface QueryParameter {
    [key: string]: string | number | null;
}
