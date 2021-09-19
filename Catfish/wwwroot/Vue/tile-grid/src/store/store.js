import { createStore, useStore as baseUseStore } from 'vuex';
import { state } from './state';
// define injection key
export const key = Symbol();
export const store = createStore({
    state
});
// define your own `useStore` composition function
export function useStore() {
    return baseUseStore(key);
}
//# sourceMappingURL=store.js.map