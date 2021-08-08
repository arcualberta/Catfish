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
            state.form.Fields[fieldInfo.fieldIndex][fieldInfo.property] = fieldInfo.value;
        },
        moveFieldUp(state, index) {
            let tmp = state.form.Fields[index];
            state.form.Fields[index] = state.form.Fields[index - 1];
            state.form.Fields[index - 1] = tmp;
        },
        moveFieldDown(state, index) {
            let tmp = state.form.Fields[index];
            state.form.Fields[index] = state.form.Fields[index + 1];
            state.form.Fields[index + 1] = tmp;
        },
        deleteField(state, index) {
            state.form.Fields.splice(index, 1);
            state.selectedFieldIndex = index;
        },
        insertField(state, templateIndex) {
            if (state.selectedFieldIndex)
                ++state.selectedFieldIndex;
            else
                state.selectedFieldIndex = state.form.Fields.length;

            state.form.Fields.splice(state.selectedFieldIndex, 0, state.fieldTemplates[templateIndex]);
        },
        deleteOption(state, optInfo) {
            if (confirm("Do you really want to delete?"))
                state.form.Fields[optInfo.fieldIndex].Options.splice(optInfo.optionIndex, 1);
        },
        addOption(state, optInfo) {
            if (optInfo.positionIndex >= 0)
                state.form.Fields[optInfo.fieldIndex].Options.splice(optInfo.positionIndex + 1, 0, {});
            else
                state.form.Fields[optInfo.fieldIndex].Options.push({});
        },
        setOptionProperty(state, optInfo) {
            state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex][optInfo.property] = optInfo.value;
        },
        moveOptionUp(state, optInfo) {
            let tmp = state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex];
            state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex] = state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex - 1];
            state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex - 1] = tmp;
        },
        moveOptionDown(state, optInfo) {
            let tmp = state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex];
            state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex] = state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex + 1];
            state.form.Fields[optInfo.fieldIndex].Options[optInfo.optionIndex + 1] = tmp;
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
            return state.form?.Fields.length
        },
        selectedFieldIndex: state => {
            return state.selectedFieldIndex
        },
        field: state => index => {
            return state.form?.Fields[index]
        }
    },
    actions: {
        //Ref: https://next.vuex.vuejs.org/guide/actions.html
        loadForm(context, formId) {
            var url = "/api/forms/" + formId ?? 0;
            axios.get(url)
                .then(res => {
                    context.commit('setForm', res.data)
                })
                .catch(error => {
                    console.log(error.response)
                })
        },
        loadFieldTemplates(context) {
            var url = "/api/fieldtemplates";
            axios.get(url)
                .then(res => {
                    context.commit('setFieldTemplates', res.data)
                })
                .catch(error => {
                    console.log(error.response)
                })

        }
    }
})
