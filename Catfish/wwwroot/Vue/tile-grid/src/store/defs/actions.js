import { Mutations } from './mutations';
//Declare ActionTypes
export var Actions;
(function (Actions) {
    Actions["FILTER_BY_KEYWORDS"] = "FILTER_BY_KEYWORDS";
})(Actions || (Actions = {}));
export const actions = {
    async [Actions.FILTER_BY_KEYWORDS](store, keywords) {
        if (typeof keywords !== "string")
            keywords = keywords.join('|');
        localStorage.selectedKeywords = keywords;
        const api = window.location.origin + `/api/tilegrid/?keywords=${keywords}`;
        const res = await fetch(api);
        const data = await res.json();
        store.commit(Mutations.SET_TILES, data);
    }
};
//# sourceMappingURL=actions.js.map