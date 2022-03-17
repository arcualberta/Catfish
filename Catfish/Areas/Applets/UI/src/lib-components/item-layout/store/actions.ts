import { ActionTree } from 'vuex';
import { State } from './state';
import {actions as itemActions } from "../../item-viewer/store/actions"

////Declare ActionTypes
//export enum Actions{
//}

export const actions: ActionTree<State, any> = {
   ...itemActions
}

