import axios from 'axios';
import { createStore } from 'vuex';
export default createStore({
    state: {
        keywords: Array()
    },
    mutations: {
        setKeywords(state, keywords) {
            state.keywords = keywords;
        }
    },
    actions: {
        async loadKeywords(context, blockId) {
            let keywords = [];
            try {
                const { data } = await axios.get('/api/tilegrid/keywords/block/' + blockId);
                keywords = data.data;
            }
            catch (error) {
                console.log(error);
            }
            context.commit('setKeywords', keywords);
        }
    },
    modules: {}
});
//# sourceMappingURL=index.js.map