import { PropType } from 'vue';
import { Guid } from 'guid-typescript'
import { DataAttribute } from '../../models';

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