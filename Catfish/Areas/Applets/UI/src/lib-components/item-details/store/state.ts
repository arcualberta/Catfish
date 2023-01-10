import { Guid } from 'guid-typescript'
import { Item } from '../../shared/models/item'
import { FlattenedFormFiledState, flattenedFormFiledState } from '../../shared/store/flattened-form-field-state'


//Declare State interface
export interface State extends FlattenedFormFiledState {

    id: Guid | null;
    item: Item | null;
    permissionList: UserPermission[] | null;
    siteUrl: string | null;
}

export const state: State = {
    ...flattenedFormFiledState,
    id: null,
    item: null,
    permissionList: null,
    siteUrl: null
}

export interface UserPermission{
    formId: Guid | null;
    formType: string | null;
    permissions: Permission[] | null;
}
export interface Permission {
    action: string | null;
}