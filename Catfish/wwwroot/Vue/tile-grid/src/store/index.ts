import axios from 'axios';
import { createStore } from 'vuex'

export default createStore({
    state: {
        keywords: Array<string>()
    },
    mutations: {
        setKeywords(state, keywords: Array<string>) {
            state.keywords = keywords;
        }
    },
    actions: {
        async loadKeywords(context, blockId: string) {
            let keywords = [] as string[];
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
    modules: {
    }

})
