import { Guid } from 'guid-typescript'
import { Item } from '../../shared/models/item'
//Declare State interface
export interface State {

    id: Guid | null;
    item: Item | null;
    permissionList: UserPermission[] | null;
}

export const state: State = {

    id: null,
    item: null,
    permissionList: null
}
export interface UserPermission{
    permission : string | null;
}