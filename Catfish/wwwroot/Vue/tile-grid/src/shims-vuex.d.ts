import { Store } from 'vuex'
import { Tile } from 'src/store/tile'
import { State } from 'src/store/state'

declare module '@vue/runtime-core' {

  // provide typings for `this.$store`
  interface ComponentCustomProperties {
    $store: Store<State>
  }
}