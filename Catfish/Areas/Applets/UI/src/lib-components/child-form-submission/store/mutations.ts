import { MutationTree } from 'vuex';
import { Guid } from 'guid-typescript';

import { State } from './state';
import { FieldContainer } from '../../shared/models/fieldContainer';
import { mutations as formSubmissionMutations } from '../../form-submission/store/mutations';

//Declare MutationTypes
export enum Mutations  {
    SET_SUBMISSIONS = 'SET_SUBMISSIONS',
    SET_PATENT_ITEM_ID = 'SET_PATENT_ITEM_ID',
    APPEND_CHILD_INSTANCE ='APPEND_CHILD_INSTANCE'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    [Mutations.SET_SUBMISSIONS](state: State, payload: FieldContainer[]) {
        state.formInstances = payload
    },
    [Mutations.SET_PATENT_ITEM_ID](state: State, payload: Guid) {
        state.itemInstanceId = payload
    },
    [Mutations.APPEND_CHILD_INSTANCE](state: State, payload: FieldContainer) {
        console.log("append child instance")
        console.log(state.formInstances)
        //  console.log("count before insert: ", state.formInstances?.length)
        var tempArray = [];
        tempArray.push(payload)

        //for (var i = 0; i < state.formInstances?.length; i++) {
        //    tempArray.push(state.formInstances[i]);
        //}
       // state.formInstances = [payload, ...state.formInstances ];
        // state.formInstances.splice(0, 0, payload);
        state.formInstances = [payload].concat(state.formInstances);
       // state.formInstances = tempArray;
       

        console.log("count after insert: ", state.formInstances?.length)
        console.log(state.formInstances)
    },


    ...formSubmissionMutations
}
