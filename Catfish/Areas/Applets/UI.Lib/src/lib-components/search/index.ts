import { actions } from './actions';
import { mutations } from './mutations';
import { state } from './state';
import FreeTextSearch from './components/FreeTextSearch.vue';

export const searchModule =  {
    actions: actions,
    mutatios: mutations,
    state: state,
    components: { FreeTextSearch }
}

//export default {
//    Models: Models,
//    Actions: Actions,
//    Mutations: type Mutations,
//    State: State,
//    components: {
//        FreeTextSearch
//    }
//}