import { Mutations } from './mutations';
//Declare ActionTypes
export var Actions;
(function (Actions) {
    Actions["FILTER_BY_KEYWORDS"] = "FILTER_BY_KEYWORDS";
    Actions["NEXT_PAGE"] = "NEXT_PAGE";
    Actions["PREVIOUS_PAGE"] = "PREVIOUS_PAGE";
})(Actions || (Actions = {}));
export const actions = {
    async [Actions.FILTER_BY_KEYWORDS](store, params) {
        //If search params is not specified, try to load it from the Local Storage. If it is not null,
        //save it into the Local Storage
        if (params === undefined) {
            params = (localStorage.keywordSearchParams)
                ? JSON.parse(localStorage.keywordSearchParams)
                : { keywords: [], offset: 0, max: 0 };
        }
        localStorage.searchParams = JSON.stringify(params);
        const api = window.location.origin +
            `/api/tilegrid/?keywords=${params?.keywords?.join('|')}&offset=${params?.offset}&max=${params?.max}`;
        console.log("Item Load API: ", api);
        const res = await fetch(api);
        const data = await res.json();
        store.commit(Mutations.SET_TILES, data);
    }
};
//# sourceMappingURL=actions.js.map