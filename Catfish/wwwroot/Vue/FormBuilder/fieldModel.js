﻿const fieldModel = {
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
            return this.index === this.$store.getters.selectedFieldIndex
        },
        fieldName() {
            return this.$store.getters.field(this.index).label
        },
        ...Vuex.mapState({
            field(state) {
                return state.form.fields[this.index]
            },
            isOptionField() {
                return this.field.componentType === "RadioButtonSet"
                    || this.field.componentType === "CheckboxSet"
                    || this.field.componentType === "DropDownMenu"
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
            this.$store.commit('setSelectedFieldIndex', idx - 1)
        },
        moveDown(idx) {
            this.$store.commit('moveFieldDown', idx)
            this.$store.commit('setSelectedFieldIndex', idx + 1)
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

            ['price', 'limit', 'startDate', 'endDate'].forEach(key => {
                if (this.activeTeleportModelChanges.hasOwnProperty(key))
                    this.setOptProperty(optIdx, key, this.activeTeleportModelChanges[key])
            });

            this.activeTeleportModelChanges = {};
        }
   }
    //template: '<h4>{{fieldModel}}</h4>',
}