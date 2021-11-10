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
  appletName: {
    required: true,
    type: String
  },
  dataAttributes: {
    required: false,
    type: null as PropType<DataAttribute> | null
  }
}

export interface DataAttribute {
  name: string | null;
  value: string | null;
}
