import { PropType } from 'vue';
import { Guid } from "guid-typescript";

export default {
    pageId: {
        required: false,
        type: null as PropType<Guid> | null
    },
    blockId: {
        required: false,
        type: null as PropType<Guid> | null
    },
    appletTitle: {
        required: false,
        type: String
    },
    dataAttributes: {
        required: false,
        type: null as PropType<DataAttribute> | null
    },
    queryParameters: {
        required: false,
        type: null as PropType<QueryParameter> | null
    }
}

export interface DataAttribute {
    [key: string]: string | number | null;
}

export interface QueryParameter {
    [key: string]: string | number | null;
}
