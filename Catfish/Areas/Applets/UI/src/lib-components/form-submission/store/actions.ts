import { ActionTree } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { FlattenedFormFiledMutations } from '../../shared/store/flattened-form-field-mutations'
import { validateFields } from '../../shared/store/form-validators'

//Declare ActionTypes
export enum Actions {
    LOAD_FORM = "LOAD_FORM",
    SUBMIT_FORM = "SUBMIT_FORM"
}

export const actions: ActionTree<State, any> = {

    [Actions.LOAD_FORM](store) {
       
        const api = window.location.origin +
            `/applets/api/itemtemplates/${store.state.itemTemplateId}/data-form/${store.state.formId}`;
        //console.log('Form Load API: ', api)

        fetch(api)
            .then(response => response.json())
            .then(data => {
                //console.log('Data:\n', JSON.stringify(data));
                store.commit(Mutations.SET_FORM, data);
                store.commit(FlattenedFormFiledMutations.APPEND_FIELD_DATA, data);
            })
            .catch(error => {
                console.error('Actions.LOAD_FORM Error: ', error);
            });
    },
    [Actions.SUBMIT_FORM](store) {
        const form = store.state.form;

        //Validating the form
        if (!form || !validateFields(form))
            return;
        store.commit(Mutations.SET_SUBMISSION_STATUS, "InProgress");
       
        const api = window.location.origin + `/applets/api/items/?itemTemplateId=${store.state.itemTemplateId}&groupId=${store.state.groupId ? store.state.groupId : ""}&collectionId=${store.state.collectionId}`;

        const formData = new FormData();

        //Setting the serialized JSON form model to the datamodel variable in formData
        formData.append('datamodel', JSON.stringify(form));

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
            .then(() => {
                //console.log(JSON.stringify(data));
                store.commit(FlattenedFormFiledMutations.REMOVE_FIELD_CONTAINERS);
                store.commit(Mutations.SET_FORM, null);
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Success");

            })
            .catch(error => {
                store.commit(Mutations.SET_SUBMISSION_STATUS, "Fail");
                console.log(error)
            });
      
    },
}

