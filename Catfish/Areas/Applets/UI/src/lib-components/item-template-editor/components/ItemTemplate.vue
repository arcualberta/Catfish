<script lang="ts">
    import {defineComponent, computed /*, ref*/} from "vue";
    import {useStore} from 'vuex';
    import dayjs from "dayjs";

    export default defineComponent({
        name: "ItemTemplate",

        props: { },
        setup() {
            const store = useStore()
           
            return {
                template: computed(() => store.state.template),
                metadataSets: computed(() => store.state.template?.metadataSets),
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
            <div class="col-12 menuEntry">
                <div class="sectionLabel">Overview</div>
            </div>

            <!-- NOTIFICATIONS -->
            <div class="col-12 menuEntry">
                <div class="sectionLabel">Notifications</div>
            </div>
            <div v-for="ms in metadataSets.filter(m => m.isTemplate == true)" :key="ms.id" class="col-12 menuEntry">
                {{ms.name.concatenatedContent}}
            </div>

            <!-- DATA FORMS -->
            <div class="col-12 menuEntry">
                <div class="sectionLabel">Forms</div>
            </div>
            <div v-for="form in dataContainer" :key="form.id" class="col-12 menuEntry">
                {{form.name.concatenatedContent}}
            </div>

            <!-- METADATA FORMS -->
            <div class="col-12 menuEntry">
                <div class="sectionLabel">Metadata Forms</div>
            </div>
            <div v-for="ms in metadataSets.filter(m => m.isTemplate == false)" :key="ms.id" class="col-12 menuEntry">
                {{ms.name.concatenatedContent}}
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
    .menuEntry{
        border: 1px solid Grey;
        margin: 10px;
        padding: 10px 10px;
    }
    .sectionLabel{
        font-weight: bold;
    }
</style>