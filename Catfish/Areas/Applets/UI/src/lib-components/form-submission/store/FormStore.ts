import { defineStore } from 'pinia';

//import router from '../router'
import { search, form } from '@arcualberta/catfish-ui';

//import config from '../appsettings';
import { FieldContainer } from '@arcualberta/catfish-ui/dist/types/src/lib-components/form/models';
import { Guid } from 'guid-typescript';

export const useFormStore = defineStore('FormStore', {
    state: () => ({
        form: null as form.models.FieldContainer | null,
        query: null as search.SolrQuery.QueryModel | null,
        submissionFailed: false,
        itemTemplateId: null as Guid | null,
        formId:null as  Guid | null,
        collectionId : null as Guid | null,
        groupId: null as Guid | null,
        apiRoot: null as string | null
    }),
    getters: {
       
    },
    actions: {
        fetchData() {
         
            //const api = 'https://catfish-test.artsrn.ualberta.ca/applets/api/itemtemplates/bd35d406-3399-40af-bc72-c7b5813ee9b1/data-form/49a7a1d3-0194-4703-b3d8-747acbf3bbfa'
          //  const api = `${config.dataServiceApiRoot}itemtemplates/${config.dataAttributes.templateId}/data-form/${config.dataAttributes.formId}`

            const api = this.apiRoot + "/applets/api/itemtemplates/" + this.itemTemplateId + "/data-form/" + this.formId;
          
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    //console.log(JSON.stringify(data))
                    this.form = data;
                    
                })
                .catch((error) => {
                    this.submissionFailed = true;
                    console.error('Item Load API Error:', error);
                });
        },
        submitForm() {
            const api = this.apiRoot + "/applets/api/items?itemTemplateId=" + this.itemTemplateId + "&groupId=" + this.groupId + "&collectionId=" + this.collectionId;
            console.log('submitForm:')
            console.log('datamodel - ', JSON.stringify(this.form))

            const formData = new FormData();
            //Setting the serialized JSON form model to the datamodel variable in formData
            formData.append('datamodel', JSON.stringify(this.form));
         

            //Adding all attachments uploaded to the files variable in formData
            const fileReferences = form.helpers.getFileReferences(this.form as FieldContainer);
            fileReferences.forEach(fileRef => {
                formData.append('files', fileRef.file);
                formData.append('fileKeys', fileRef.fieldId.toString() as unknown as string);
           })

            fetch(api,
                {
                    body: formData,
                    method: "post",
                    headers: {
                        "encType": "multipart/form-data"
                    }
                })
                .then(response => response.json())
                .then(data => {
                    console.log(JSON.stringify(data))
                   // this.submissionFailed = data !== "Success";
                   // router.push({name:'joinConfirmation'})
                    this.submissionFailed = false;
                })
                .catch((error) => {
                    this.submissionFailed = true;
                    console.error('Form submission failed:', error);
                });

        },
        setFormId(formId: Guid) {
            this.formId = formId;
        },
        setTemplateId(templateId: Guid) {
            this.itemTemplateId = templateId;
        },
        setCollectionId(collectionId: Guid) {
            this.collectionId = collectionId;
        },
        setGroupId(groupId: Guid) {
            this.groupId = groupId;
        },
        setApiRoot(apiRoot: string) {
            this.apiRoot = apiRoot;
        }
    }
});