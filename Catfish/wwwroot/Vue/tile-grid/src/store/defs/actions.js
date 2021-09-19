import { Mutations } from './mutations';
//Declare ActionTypes
export var Actions;
(function (Actions) {
    Actions["FILTER_BY_KEYWORDS"] = "FILTER_BY_KEYWORDS";
})(Actions || (Actions = {}));
export const actions = {
    async [Actions.FILTER_BY_KEYWORDS](store, keywords) {
        const concatenatedKeywords = keywords.join('|');
        const api = `https://localhost:44385/api/tilegrid/?keywords=${concatenatedKeywords}`;
        const res = await fetch(api);
        const data = await res.json();
        store.commit(Mutations.SET_TILES, data);
    }
};
//# sourceMappingURL=actions.js.map