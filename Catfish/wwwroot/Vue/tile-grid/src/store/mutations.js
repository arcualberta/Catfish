//Declare MutationTypes
export var MutationTypes;
(function (MutationTypes) {
    MutationTypes["SET_TILES"] = "SET_TILES";
})(MutationTypes || (MutationTypes = {}));
//Create a mutation tree that implement all mutation interfaces
export const mutations = {
    [MutationTypes.SET_TILES](state, payload) {
        state.tiles = payload;
    },
};
//# sourceMappingURL=mutations.js.map