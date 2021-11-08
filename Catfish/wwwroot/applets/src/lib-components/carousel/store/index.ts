////////import { InjectionKey } from 'vue';
////////import { createStore, useStore as baseUseStore, Store } from 'vuex';
////////import { State, state } from './defs/state'

////////// define injection key
////////export const key: InjectionKey<Store<State>> = Symbol()

////////export const store = createStore<State>({
////////  state
////////});

////////// our own `useStore` composition function for types
////////export function useStore(): Store<State> {
////////  return baseUseStore(key);
////////}
