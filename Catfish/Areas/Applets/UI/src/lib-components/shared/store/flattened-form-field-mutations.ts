﻿import { Guid } from 'guid-typescript'
import { MutationTree } from 'vuex';
import { FlattenedFormFiledState as State } from './flattened-form-field-state';
import { flattenFieldInputs } from './form-submission-utils'
import { FieldContainer, /*eValidationStatus,*/ MonolingualTextField, MultilingualTextField } from '../../shared/models/fieldContainer';
import { TextCollection, Text } from '../models/textModels';
//import { validateFields } from '../../shared/store/form-validators';

export enum FlattenedFormFiledMutations {
    SET_TEXT_VALUE = 'SET_TEXT_VALUE',
    SET_OPTION_VALUE = 'SET_OPTION_VALUE',
    ADD_FILE = 'ADD_FILE',
    REMOVE_FILE = 'REMOVE_FILE',
    CLEAR_FIELD_DATA = 'CLEAR_FIELD_DATA',
    REMOVE_FIELD_CONTAINERS = 'REMOVE_FIELD_CONTAINERS',
    APPEND_FIELD_DATA = 'APPEND_FIELD_DATA',
    APPEND_MONOLINGUAL_VALUE = 'APPEND_MONOLINGUAL_VALUE',
    APPEND_MULTILINGUAL_VALUE ='APPEND_MULTILINGUAL_VALUE'
}

//Create a mutation tree that implement all mutation interfaces
export const mutations: MutationTree<State> = {
    [FlattenedFormFiledMutations.SET_TEXT_VALUE](state: State, payload: { id: Guid; val: string }) {
        state.flattenedTextModels[payload.id.toString()].value = payload.val;
        state.modified = true;

    ////    //Re-validating the forms
    ////    state.fieldContainers?.forEach(fc => {
    ////        if (fc?.validationStatus === eValidationStatus.INVALID)
    ////        validateFields(fc);
    ////    })
    },
    [FlattenedFormFiledMutations.SET_OPTION_VALUE](state: State, payload: { id: Guid; isSelected: boolean }) {
        state.flattenedOptionModels[payload.id.toString()].selected = payload.isSelected;
        state.modified = true;
    },
    [FlattenedFormFiledMutations.ADD_FILE](state: State, payload: { id: Guid; val: File }) {
        if (!state.flattenedFileModels[payload.id.toString()])
            state.flattenedFileModels[payload.id.toString()] = [] as File[];
        state.flattenedFileModels[payload.id.toString()].push(payload.val);
        state.modified = true;
    },
    [FlattenedFormFiledMutations.REMOVE_FILE](state: State, payload: { id: Guid; index: number }) {
        state.flattenedFileModels[payload.id.toString()]?.splice(payload.index, 1);
        state.modified = true;
    },
    [FlattenedFormFiledMutations.CLEAR_FIELD_DATA](state: State) {
        //Iterate through all Text elements in state.flattenedTextModels 
        Object.keys(state.flattenedTextModels).forEach(function (key) {
            state.flattenedTextModels[key].value = '';
        });

        // Iterate through all Option elements in state.flattenedOptionModels
        Object.keys(state.flattenedOptionModels).forEach(function (key) {
            state.flattenedOptionModels[key].selected = false;
        });

        // Iterate through attachment in state.flattenedOptionModels
        Object.keys(state.flattenedFileModels).forEach(function (key) {
            state.flattenedFileModels[key] = [] as File[];
        });

        //Since this mutation is meant to reset forms, reset modified flag to false
        state.modified = false;
    },
    [FlattenedFormFiledMutations.REMOVE_FIELD_CONTAINERS](state: State) {
        state.flattenedTextModels = {};
        state.flattenedOptionModels = {};
        state.flattenedFileModels = {}

        //Since this mutation is meant to reset forms, reset modified flag to false
        state.modified = false;
    },
    [FlattenedFormFiledMutations.APPEND_FIELD_DATA](state: State, payload: FieldContainer) {
        //console.log('SET_FORM payload:\n', JSON.stringify(payload));
        flattenFieldInputs(payload, state)
    },
    [FlattenedFormFiledMutations.APPEND_MONOLINGUAL_VALUE](state: State, target: MonolingualTextField) {
       
        const newText = {
            id: Guid.create().toString() as unknown as Guid,
            $type: "Catfish.Core.Models.Contents.Text",
        } as Text;

        target.values?.$values.push(newText);
        state.flattenedTextModels[newText.id.toString()] = newText;
    },
    [FlattenedFormFiledMutations.APPEND_MULTILINGUAL_VALUE](state: State, target: MultilingualTextField) {
      
        var newTextCollection = {
            id: Guid.create().toString() as unknown as Guid,
            $type: "Catfish.Core.Models.Contents.MultilingualValue",
            values: {
                $type: "Catfish.Core.Models.Contents.XmlModelList`1[[Catfish.Core.Models.Contents.Text, Catfish.Core]], Catfish.Core",
                $values: [] as Text[]
            }
        } as TextCollection

        if (target.values?.$values[0]) {
            target.values.$values.forEach((txt: Text | any) => {
                const newTxt: Text = {
                    id: Guid.create().toString() as unknown as Guid,
                    language: txt.language
                } as Text;

                newTextCollection.values.$values.push(newTxt);
                state.flattenedTextModels[newTxt.id.toString()] = newTxt;
            })
        }
        else {
            const newTxt: Text = {
                id: Guid.create().toString() as unknown as Guid,
                language: "en"
            } as Text;

            newTextCollection.values.$values.push(newTxt);
            state.flattenedTextModels[newTxt.id.toString()] = newTxt;
        }

        target.values?.$values.push(newTextCollection);
    },
}
