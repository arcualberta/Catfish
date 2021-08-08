const appModel = {
    props: {
        id: {
            type: Number,
            required: false
        }
    },
    //======================================================================================================
    //Defining a set of computed properties that are mapped from the Vuex based store. These properties are
    //used by the app. Since these are computed properties mapped from the store, they are reactive, thus 
    //they trigger DOM updates automatically to update interface elements when the source properties in 
    //the store are modified.
    computed: Vuex.mapState({
        fieldTemplates: state => state.fieldTemplates,
        formName: state => state.form?.name,
        formDescription: state => state.form?.description,
        linkText: state => state.form?.linkText,
        fields: state => state.form?.fields
    }),
    //======================================================================================================
    //Defining a set of methods for updating the Vuex data store through store commits.
    methods: {
        setFormProperty(name, val) { this.$store.commit('setFormProperty', { name: name, val: val }) },
        insertField(templateIndex) { this.$store.commit('insertField', templateIndex) }
    },
    //======================================================================================================
    //Initializing the app
    created() {

        //Ref: https://next.vuex.vuejs.org/guide/actions.html#dispatching-actions
        this.$store.dispatch('loadFieldTemplates')
        this.$store.dispatch('loadForm', this.id)
    }
};
