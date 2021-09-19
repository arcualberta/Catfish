import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { State } from './state'
import { Tile } from './tile'

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
  state: {
    tiles: Array<Tile>()
  }
})

// define your own `useStore` composition function
export function useStore(): Store<State> {
  return baseUseStore(key)
}