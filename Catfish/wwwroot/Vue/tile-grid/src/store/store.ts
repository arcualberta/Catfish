import { InjectionKey } from 'vue'
import { createStore, Store } from 'vuex'
import { State } from './state'
import { Tile } from './tile'

// define injection key
export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
  state: {
    tiles: Array<Tile>()
  }
})