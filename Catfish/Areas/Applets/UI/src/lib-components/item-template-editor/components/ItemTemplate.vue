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
    <div class="container row itemTemplate">

        <div class="col-md-4">
            <div class="col-12 menuLabel">
                Overview
            </div>
            <div class="col-12 menuLabel">
                Notification
            </div>
            <!-- METADATA SETS -->
            <div v-if="metadasets?.length > 0">
                <div>Metadasets</div> <!-- DEBUG -->
                <div v-for="ms in metadasets" :key="md.id">
                    <div v-if="ms.isTemplate == true" class="col-12 menuLabel">
                        {{ms.name.concatenatedContent}}
                    </div>
                </div>
            </div>
            <!-- FORMS -->
            <div class="col-12 menuLabel">
                <div class="sectionLabel">Forms</div>
            </div>
            <!-- DATA CONTAINER -->
            <div v-if="dataContainer?.length > 0">     
                <div v-for="form in dataContainer" :key="form.id" class="col-12 menuLabel">        
                        {{form.name.concatenatedContent}}    
                </div>
            </div>
         </div>
         <div class="col-md-8">
                <!-- Content Section -->

                <h5>Item Template JSON</h5>
                <p>{{JSON.stringify(template)}}</p>
         </div>
        
    </div>
</template>

<style scoped>
    .menuLabel{
        border: 1px solid Grey;
        margin: 10px;
        padding: 10px 10px;
    }
    .sectionLabel{
        font-weight: bold;
    }
</style>