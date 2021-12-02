<script lang="ts">
    import {defineComponent, computed , ref} from "vue";
    import {useStore} from 'vuex';
    import dayjs from "dayjs";

    import NotificationEditor from "./NotificationEditor.vue"
    import FormEditor from "./FormEditor.vue"

    export default defineComponent({
        name: "ItemTemplate",
        components: { NotificationEditor, FormEditor},
        props: { },
        setup() {
            const store = useStore()

            const activePanel = ref(null as null | string)
           
            return {
                template: computed(() => store.state.template),
                metadataSets: computed(() => store.state.template?.metadataSets),
                dataContainer: computed(() => store.state.template?.dataContainer),
                activePanel
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
            <div class="col-12 menuEntry " @click="activePanel = 'overview'" v-bind:class= "activePanel == 'overview' || activePanel == null ?'active':''">
                <div class="sectionLabel">Overview</div>
            </div>

            <!-- NOTIFICATIONS -->
            <div class="col-12 menuEntry" @click="activePanel = 'notifications'" v-bind:class="activePanel == 'notifications' ?'active':''">
                <div class="sectionLabel">Notifications</div>
            </div>
            <div v-for="ms in metadataSets?.filter(m => m.isTemplate == true)" :key="ms.id" class="col-12 menuEntry" @click="activePanel = ms.id" v-bind:class= "activePanel == ms.id?'active':''">
                {{ms.name.concatenatedContent}}
                
            </div>

            <!-- DATA FORMS -->
            <div class="col-12 menuEntry" @click="activePanel = 'forms'" >
                <div class="sectionLabel">Forms</div>
            </div>
            <div v-for="form in dataContainer" :key="form.id" class="col-12 menuEntry" @click="activePanel = form.id" v-bind:class = "activePanel == form.id?'active':''">
                {{form.name.concatenatedContent}}
            </div>

            <!-- METADATA FORMS -->
            <div class="col-12 menuEntry" @click="activePanel = 'metadata-forms'">
                <div class="sectionLabel">Metadata Forms</div>
            </div>
            <div v-for="ms in metadataSets?.filter(m => m.isTemplate == false)" :key="ms.id" class="col-12 menuEntry" @click="activePanel = ms.id" v-bind:class = "activePanel == ms.id?'active':''">
                {{ms.name.concatenatedContent}}
            </div>

        </div>

        <div class="col-md-8">
            <div class="col-12 wrapper" v-if="activePanel == null || activePanel == 'overview'">
                <h4>Overview</h4>
            </div>

            <!-- NOTIFICATIONS -->
            <div class="col-12 wrapper" v-if="activePanel == 'notifications'">
                <h4>Notifications</h4>
            </div>
            <div v-for="ms in metadataSets?.filter(m => m.isTemplate == true)" :key="ms.id" class="col-12 wrapper">
                <div v-if="activePanel == ms.id">
                    <!--<h4>{{ms.name.concatenatedContent}}</h4>-->
                    <NotificationEditor :title="ms.name.concatenatedContent" :container="ms" ></NotificationEditor>
                </div>
            </div>
            <!-- DATA FORMS -->
            <div class="col-12 wrapper" v-if="activePanel == 'forms'">
                <h4>Forms</h4>
            </div>
            <div v-for="form in dataContainer" :key="form.id" class="col-12 wrapper">
                <div v-if="activePanel == form.id">
                    <!--{{form.name.concatenatedContent}}-->
                    <FormEditor :title="form.name.concatenatedContent" :form="form"></FormEditor>
                </div>

            </div>
            <!-- METADATA FORMS -->
            <div class="col-12 wrapper" v-if="activePanel == 'metadata-forms'">
                <h4>Metadata Forms</h4>
            </div>
            <div v-for="ms in metadataSets?.filter(m => m.isTemplate == false)" :key="ms.id" class="col-12 wrapper " >
                <div v-if="activePanel == ms.id">
                    {{ms.name.concatenatedContent}}
                </div>
               
            </div>
        </div>

    </div>
</template>

<style scoped>
    .menuEntry{
        border: 1px solid Grey;
        margin: 10px;
        padding: 10px 10px;
    }
        .menuEntry.active {
            background-color: #BBBCAA;
        }
    .sectionLabel{
        font-weight: bold;
    }
    .wrapper{
        margin: 0;
        padding: 0;
    }
</style>