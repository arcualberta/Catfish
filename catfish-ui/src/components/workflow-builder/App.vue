<script setup lang="ts">
    import { computed, watch } from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";
    import {useRoute} from 'vue-router'
    import { useWorkflowBuilderStore } from './store';
    import { Workflow } from './models'
    import { default as WorkflowTemplate } from './components/Workflow.vue';
    /* import the fontawesome core */
    import { library } from '@fortawesome/fontawesome-svg-core'

    /* import font awesome icon component */
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

    /* import specific icons */
    import * as faIcons from '@fortawesome/free-solid-svg-icons'
    /* add icons to the library */
    library.add(faIcons.faCircleCheck)
    library.add(faIcons.faCircleXmark)
    library.add(faIcons.faPenToSquare)
    library.add(faIcons.faCirclePlus)
    library.add(faIcons.faQuestionCircle)
    library.add(faIcons.faThList)
    library.add(faIcons.faArrowLeft)

    const props = defineProps<{ piniaInstance: Pinia, 
                                repositoryRoot: string, 
                                workflowId?: Guid }>();

    const store = useWorkflowBuilderStore(props.piniaInstance);
    const route = useRoute()
   
    const workflowId = props.workflowId? props.workflowId : route.params.id as unknown as Guid;
  
    if (workflowId)
        store.loadWorkflow(workflowId)

    watch(() => store.transientMessage, async newMessage => {
        if (newMessage)
            setTimeout(() => {
                store.transientMessage = null;
            }, 2000)
    })

    const newWorkflow = () => {
        /*store.workflow = {
            id: Guid.EMPTY as unknown as Guid,
            name: "",
            description: "",
            states: [] as WorkflowState[],
            actions: [] as WorkflowAction[]
        } as Workflow;*/
        store.createNewWorkflow();
    }

    const saveWorkflow = () => store.saveWorkflow()

    const disabled = computed(() => store.workflow ? false : true)

    

</script>
<style scoped src="./styles.css"></style>

<template>
    <transition name="fade">
        <p v-if="store.transientMessage" :class="'alert alert-' + store.transientMessageClass">{{store.transientMessage}}</p>
    </transition>
    <h2>Workflow Builder</h2>
    <div class="control">
        <button type="button" class="btn btn-primary" :disabled="!disabled" @click="newWorkflow">New Workflow</button>
        <button type="button" class="btn btn-success" :disabled="disabled" @click="saveWorkflow">Save</button>
    </div>
    
    <hr />
    <WorkflowTemplate v-if="store.workflow" :model="(store.workflow as Workflow)" />
</template>
<style>
.header-style{
    padding-top: 25px;
    font-size: 24px;
    font-family: "Architects Daughter";
}
.btn-close {
    position: absolute;
    top: 0;
    right: 0;
    border: none;
    font-size: 20px;
    padding: 10px;
    cursor: pointer;
    font-weight: bold;
    color: #db2424;
    background: transparent;
}
.modal-add-btn{
    background-color: #007bff;
    color: #fdfdfd;
    border: 1px solid #007bff;
    border-radius: 5px;
    font-weight: bold;
    font-size: 16px;
    cursor: pointer;
    line-height: 1.15385;
    padding: 8px .8em;
}
.plus-btn{
    background-color: #1ca5b8;
}
.list-item{
padding-top: 40px;
max-width: 40%;
}
.list-recipient{
max-width: 75%;
}
.title-recipient{
padding-top: 20px;
}
.text-editor{
    width: 780;
}
.tab-view{
    padding-top: 40px;
}
.panel-delete{
    padding-bottom: 20px;
}
.trigger-add{
    padding-left: 10px;
    padding-top: 10px;
}
.content-style{
    padding-top: 10px;
    font-size: 18px;
    font-family: "Architects Daughter";
}
.popup-list-item{
padding-top: 10px;
padding-bottom: 10px;
max-width: 80%;
}
.accordion-button{
    width: 30px;
    height: 30px;
    float: right;
}
.one-space{
    margin-right: 10px;
}
.accordion-header{
    padding-top: 5px;
    font-size: 18px;
    font-family: "Architects Daughter";  
}
.left-space{
    margin-left: 60px;
}
</style>
