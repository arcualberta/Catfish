"use strict";
var _a;
exports.__esModule = true;
exports.mutations = exports.Mutations = void 0;
//Declare MutationTypes
var Mutations;
(function (Mutations) {
    Mutations["SET_TILES"] = "SET_TILES";
})(Mutations = exports.Mutations || (exports.Mutations = {}));
//Create a mutation tree that implement all mutation interfaces
exports.mutations = (_a = {},
    _a[Mutations.SET_TILES] = function (state, payload) {
        state.items = payload;
        console.log('Payload: ', payload);
    },
    _a);
