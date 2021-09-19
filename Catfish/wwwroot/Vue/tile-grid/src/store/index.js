import { createStore, useStore as baseUseStore } from 'vuex';
import { state } from './defs/state';
import { mutations } from './defs/mutations';
import { actions } from './defs/actions';
import { getters } from './defs/getters';
export const key = Symbol();
export const store = createStore({
    state,
    mutations,
    actions,
    getters
});
// our own `useStore` composition function for types
export function useStore() {
    return baseUseStore(key);
}
//# sourceMappingURL=index.js.map