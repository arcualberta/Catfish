//Declare MutationTypes
export var Mutations;
(function (Mutations) {
    Mutations["SET_TILES"] = "SET_TILES";
})(Mutations || (Mutations = {}));
//Create a mutation tree that implement all mutation interfaces
export const mutations = {
    [Mutations.SET_TILES](state, payload) {
        console.log('Payload: ', payload);
        state.searchResult = payload;
    },
};
//# sourceMappingURL=mutations.js.map