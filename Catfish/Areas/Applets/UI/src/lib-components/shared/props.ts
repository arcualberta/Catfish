import { PropType } from 'vue';
import { Guid } from "guid-typescript";

export default {
  pageId: {
    required: true,
    type: null as PropType<Guid> | null
  },
  blockId: {
    required: true,
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
  name: string | null;
  value: string | null;
}

export interface QueryParameter {
  name: string | null;
  value: string | number | null;
}
