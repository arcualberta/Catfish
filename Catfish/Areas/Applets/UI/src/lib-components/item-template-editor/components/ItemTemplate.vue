<script lang="ts">
    import {defineComponent, computed /*, ref*/} from "vue";
    import {useStore} from 'vuex';
    import dayjs from "dayjs";

    //import { ItemTemplate } from "../models/itemTemplate.vue";

    export default defineComponent({
        name: "ItemTemplate",

        props: { },
        setup() {
            const store = useStore()
           
            //const template = ref(store.state.template?.value.templateName);
           // console.log("ItemTemplate component - template name : " + templateName)
            return {
                template: computed(() => store.state.template),
                metadatasets: computed(() => store.state.template?.metadataSets),
                dataContainer: computed(() => store.state.template?.dataContainer)
            }
        },

        methods: {
        formatDate(dateString: string) {
                const date = dayjs(dateString);
                return date.format('MMM DD, YYYY');
            }
        }
    });
</script>

<template>
    <h3>{{template?.templateName}}</h3>
    <div v-if="metadatasets?.length > 0">
        <h5>Metadata Sets</h5>
    </div>

    <div v-if="dataContainer?.length > 0">
        <h5>Data Container</h5>
        
            <div v-for="df in dataContainer" :key="df.id">
                <div v-if="df.fields.length > 0">
                    <div>Field</div>
                    <div v-for="fc in df.fields" :key="fc.id">
                        <div>{{fc.id}} : {{fc.modelType }}</div>
                         <div v-if="fc.values?.length > 0">
                             <div v-for="v in fc.values" :key="v.id">
                                 <div>{{v.id}} : {{v.value}}</div>
                             </div>

                         </div>

                    </div>
                </div>
            </div>
        
    </div>

</template>