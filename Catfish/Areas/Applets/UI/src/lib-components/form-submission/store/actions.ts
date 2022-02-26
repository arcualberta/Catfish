import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { clearForm, FlattenedFormFiledState } from '../../shared/store/form-submission-utils';
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

       
        const api = window.location.origin + `/applets/api/itemeditor/?itemTemplateId=${store.state.itemTemplateId}&groupId=${store.state.groupId ? store.state.groupId : ""}&collectionId=${store.state.collectionId}`;

        const formData = new FormData();

        //Setting the serialized JSON form model to the datamodel variable in formData
        formData.append('datamodel', JSON.stringify(store.state.form));

        //Adding all attachments uploaded to the files variable in formData
        for (const key in store.state.flattenedFileModels) {
            if (store.state.flattenedFileModels[key].length > 0) {
                store.state.flattenedFileModels[key].forEach(file => {
                    console.log("File: ", file.name)
                    formData.append('files', file);
                    formData.append('fileKeys', key);
				})
			}
        }

        fetch(api,
            {
                body: formData,
                method: "post",
                headers: {
                    //"Content-Type": "multipart/form-data"
                    "encType": "multipart/form-data"
                }
            }).then(response =>
                response.json())
            .then(data => {
                console.log(JSON.stringify(data));
                const flattenModel: FlattenedFormFiledState = {
                    flattenedOptionModels: store.state.flattenedOptionModels,
                    flattenedTextModels: store.state.flattenedTextModels,
                    flattenedFileModels: store.state.flattenedFileModels
                };

                //clear the form content
                clearForm(flattenModel);
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Success");

            })
            .catch(error => {
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Fail");
                console.log(error)
            });
      
    },
}

