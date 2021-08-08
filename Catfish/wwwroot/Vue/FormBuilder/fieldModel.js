const fieldModel = {
    template: '#field-template',
    props: {
        index: {
            type: Number,
            required: true
        }
    },
    data() {
        return {
            isSelected: false,
            isError: false,
            showOptionSettingsModel: [],
            isShowModal: false,
            activeTeleportModelId: null,
            activeTeleportModelChanges: {}
        }
    },
    computed: {
        isHighlighted() {
            return this.index == this.$store.getters.selectedFieldIndex
        },
        fieldName() {
            return this.$store.getters.field(this.index).Label
        },
        ...Vuex.mapState({
            field(state) {
                return state.form.Fields[this.index]
            },
            isOptionField() {
                return this.field.ComponentType === "RadioButtonSet"
                    || this.field.ComponentType === "CheckboxSet"
                    || this.field.ComponentType === "DropDownMenu"
            }
        })
    },
    methods: {
        setFieldProperty(name, val) {
            this.$store.commit('setFieldProperty', { fieldIndex: this.index, property: name, value: val })
        },
        deleteField(idx) {
            this.$store.commit('deleteField', idx)
        },
        moveUp(idx) {
            this.$store.commit('moveFieldUp', idx)
        },
        moveDown(idx) {
            this.$store.commit('moveFieldDown', idx)
        },
        selectField() {
            this.$store.commit('setSelectedFieldIndex', this.index)
        },
        deleteOption(optIndex) {
            this.$store.commit('deleteOption', { fieldIndex: this.index, optionIndex: optIndex })
        },
        addOption(positionIdx) {
            this.$store.commit('addOption', { fieldIndex: this.index, positionIndex: positionIdx })
        },
        setOptProperty(optIndex, name, val) {
            this.$store.commit('setOptionProperty', { fieldIndex: this.index, optionIndex: optIndex, property: name, value: val })
        },
        moveOpt(optIndex, dirUp) {
            this.$store.commit(dirUp ? 'moveOptionUp' : 'moveOptionDown',
                { fieldIndex: this.index, optionIndex: optIndex })
        },
        showTeleportModel(id){
            this.activeTeleportModelId = id;
            this.activeTeleportModelChanges = {};
        },
        cancelTeleportModelChanges() {
            this.activeTeleportModelId = null;
            this.activeTeleportModelChanges = {};
        },
        updateOptionSettings(optIdx) {
            this.activeTeleportModelId = null;

            ['Price', 'Limit', 'StartDate', 'EndDate'].forEach(key => {
                if (this.activeTeleportModelChanges.hasOwnProperty(key))
                    this.setOptProperty(optIdx, key, this.activeTeleportModelChanges[key])
            });

            this.activeTeleportModelChanges = {};
        }
   }
    //template: '<h4>{{fieldModel}}</h4>',
}