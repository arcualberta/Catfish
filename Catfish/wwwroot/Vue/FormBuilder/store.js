const store = Vuex.createStore({
    state() {
        return {
            form: null,
            selectedFieldIndex: null,
            fieldTemplates: [],
            modalVisible: false,
            modalComponent: null
        }
    },
    mutations: {
        setForm(state, val) {
            state.form = val
        },
        setFieldTemplates(state, val) {
            state.fieldTemplates = val
        },
        setFormProperty(state, formInfo) {
            state.form[formInfo.name] = formInfo.val
        },
        setSelectedFieldIndex(state, idx) {
            state.selectedFieldIndex = idx
        },
        setFieldProperty(state, fieldInfo) {
            state.form.fields[fieldInfo.fieldIndex][fieldInfo.property] = fieldInfo.value;
        },
        moveFieldUp(state, index) {
            let tmp = state.form.fields[index];
            state.form.fields[index] = state.form.fields[index - 1];
            state.form.fields[index - 1] = tmp;
        },
        moveFieldDown(state, index) {
            let tmp = state.form.fields[index];
            state.form.fields[index] = state.form.fields[index + 1];
            state.form.fields[index + 1] = tmp;
        },
        deleteField(state, index) {
            state.form.fields.splice(index, 1);
            state.selectedFieldIndex = index;
        },
        insertField(state, templateIndex) {
            if (state.selectedFieldIndex)
                ++state.selectedFieldIndex;
            else
                state.selectedFieldIndex = state.form.fields.length;

            state.form.fields.splice(state.selectedFieldIndex, 0, state.fieldTemplates[templateIndex]);
        },
        deleteOption(state, optInfo) {
            if (confirm("Do you really want to delete?"))
                state.form.fields[optInfo.fieldIndex].options.splice(optInfo.optionIndex, 1);
        },
        addOption(state, optInfo) {
            if (optInfo.positionIndex >= 0)
                state.form.fields[optInfo.fieldIndex].options.splice(optInfo.positionIndex + 1, 0, {});
            else
                state.form.fields[optInfo.fieldIndex].options.push({});
        },
        setOptionProperty(state, optInfo) {
            state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex][optInfo.property] = optInfo.value;
        },
        moveOptionUp(state, optInfo) {
            let tmp = state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex];
            state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex] = state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex - 1];
            state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex - 1] = tmp;
        },
        moveOptionDown(state, optInfo) {
            let tmp = state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex];
            state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex] = state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex + 1];
            state.form.fields[optInfo.fieldIndex].options[optInfo.optionIndex + 1] = tmp;
        },
        editFieldSettings(state, field) {
            //edit field settings
        },
        showModal(state, optInfo) {
            state.modalVisible = true;
            state.modalComponent = optInfo.fieldType;
        },
        hideModal(state) {
            state.modalVisible = false; 
        }
       
    },
    getters: {
        fieldCount: state => {
            return state.form?.fields.length
        },
        selectedFieldIndex: state => {
            return state.selectedFieldIndex
        },
        field: state => index => {
            return state.form?.fields[index]
        }
    },
    actions: {
        //Ref: https://next.vuex.vuejs.org/guide/actions.html
        loadForm(context, formId) {
            var url = "/manager/api/forms/" + formId;// ?? null;
            axios.get(url)
                .then(res => {
                    context.commit('setForm', res.data)
                })
                .catch(error => {
                    console.log(error.response)
                })
        },
        loadFieldTemplates(context) {
            var url = "/manager/api/fieldtemplates";
            axios.get(url)
                .then(res => {
                    context.commit('setFieldTemplates', res.data)
                })
                .catch(error => {
                    console.log(error.response)
                })
        },
        saveForm(context) {
            //var postData = JSON.stringify(this.state.form);
            //var formData = new FormData();
            //formData.append('formStr', postData);

            axios.post('/manager/api/forms/', context.state.form)
                .then(function (response) {
                    if (response.data.created) {
                        let url = window.location.href.replace(/\/$/, '') + "/" + response.data.id;
                        window.location.href = url;
                    }
                    console.log(response);
                })
                .catch(function (error) {
                    console.log(error);
                });
        }
    }
})
