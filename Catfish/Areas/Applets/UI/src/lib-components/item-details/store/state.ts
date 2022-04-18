import { Guid } from 'guid-typescript'
import { Item } from '../../shared/models/item'
import { FlattenedFormFiledState } from '../../shared/store/flattened-form-field-state'


//Declare State interface
export interface State extends FlattenedFormFiledState {

    id: Guid | null;
    item: Item | null;
    permissionList: UserPermission[] | null;
}

export const state: State = {

    id: null,
    item: null,
    permissionList: null,
    flattenedTextModels: {},
    flattenedOptionModels: {},
    flattenedFileModels: {},
}

export interface UserPermission{
    permission : string | null;
}