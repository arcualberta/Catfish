import { InjectionKey } from 'vue';
import { createStore, useStore as baseUseStore, Store } from 'vuex';
import { State, state } from './state'
import { mutations } from './mutations'
import { actions } from './actions'
import { getters } from './getters'

export const key: InjectionKey<Store<State>> = Symbol();

export const store = createStore<State>({
  state,
  mutations,
  actions,
  getters
});

// our own `useStore` composition function for types
export function useStore(): Store<State> {
  return baseUseStore(key);
}
