import { createStore } from "vuex";
import axios from "axios";

export default createStore({
    state: {
        keywords: [] as string[]
    },
    mutations: {},
    actions: {
        loadItems({ commit }) {
            axios
                .get('https://localhost:44385/api/tilegrid/keywords/block/f8d5815f-ccad-4b72-92ef-51b7c88ea0dd', {
                //    headers: {
                //        'Ocp-Apim-Subscription-Key': 'your key',
                //    }
                })
                .then(response => response.data)
                .then(items => {
                    console.log(items);
                    commit('SET_Items', items)
                })
        }
    },
    modules: {},
});
