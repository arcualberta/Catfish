import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
//import { clearForm, FlattenedFormFiledState } from '../../shared/store/form-submission-utils'
//import { validateFields } from '../../shared/store/form-validators'

//Declare ActionTypes
export enum Actions {
    LOAD_FORM = "LOAD_FORM",
    SUBMIT_FORM = "SUBMIT_FORM"
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_FORM](store) {
       
        const api = window.location.origin +
            `/applets/api/itemtemplates/${store.state.itemTemplateId}/data-form/${store.state.formId}`;
       // console.log('Form Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                //console.log('Data:\n', JSON.stringify(data));
                store.commit(Mutations.SET_FORM, data);
            })
            .catch(error => {
                console.error('Actions.LOAD_FORM Error: ', error);
            });
    },
    [Actions.SUBMIT_FORM](store) {

        //Validating the form
        if (!store.state.form /*|| !validateFields(store.state.form)*/)
            return;

        store.commit(Mutations.SET_SUBMISSION_STATUS, "InProgress");

       
        const api = window.location.origin + `/applets/api/itemeditor/`;

        const formData = new FormData();
        formData.append('datamodel', JSON.stringify(store.state.form));

        fetch(api,
            {
                body: formData,
                method: "post"
            }).then(response =>
                response.json())
            .then(data => {
                console.log(JSON.stringify(data));
               // const flattenModel: FlattenedFormFiledState ={          
               //     flattenedOptionModels : store.state.flattenedOptionModels,
               //     flattenedTextModels : store.state.flattenedTextModels,
               // };
               // //clear the form content
               // clearForm(flattenModel);
               //store.commit(Mutations.SET_SUBMISSION_STATUS, "Success");
                
            })
            .catch(error => {
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Fail");
                console.log(error)
            });
      
    },
}

