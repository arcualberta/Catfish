////////import { InjectionKey } from 'vue';
////////import { createStore, useStore as baseUseStore, Store } from 'vuex';
////////import { State, state } from './defs/state'
////////import { mutations } from './defs/mutations'
////////import { actions } from './defs/actions'
////////import { getters } from './defs/getters'

////////// define injection key
////////export const key: InjectionKey<Store<State>> = Symbol()

////////export const store = createStore<State>({
////////  state,
////////  mutations,
////////  actions,
////////  getters
////////});

////////// our own `useStore` composition function for types
////////export function useStore(): Store<State> {
////////  return baseUseStore(key);
////////}
