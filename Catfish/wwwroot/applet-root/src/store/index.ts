import { createStore } from 'vuex'
import { state } from './defs/state'
import { mutations } from './defs/mutations'
import keywordSearchModule from '../applets/keyword-search/store';

export default createStore({
  state,
  mutations,
  actions: {
  },
  modules: {
    keywordSearchModule
  }
})
