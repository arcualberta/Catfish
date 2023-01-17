<script setup lang="ts">
    import { computed, watch } from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";

    import { useWorkflowBuilderStore } from './store';
    import { Workflow, WorkflowAction, FormSubmissionAction } from './models'
    import { default as WorkflowTemplate } from './components/Workflow.vue';
    

    const props = defineProps<{ piniaInstance: Pinia, repositoryRoot: string, workflowId?: Guid }>();

    const store = useWorkflowBuilderStore(props.piniaInstance);

    if (props.workflowId)
        store.loadWorkflow(props.workflowId)

    watch(() => store.transientMessage, async newMessage => {
        if (newMessage)
            setTimeout(() => {
                store.transientMessage = null;
            }, 2000)
    })

    const newWorkflow = () => {
        store.workflow = {
            id: Guid.EMPTY as unknown as Guid,
            name: "",
            description: "",
            states: [] as string[]
        } as Workflow;
    }

    const saveWorkflow = () => store.saveWorkflow()

    const disabled = computed(() => store.workflow ? false : true)

    const newFormSubmissionAction = () => {
        if (!store.workflow) {
            console.error('Cannot add action to null workflow');
            return;
        }

        const action = {
            id: Guid.create().toString() as unknown as Guid,
            title: "",
            description: "",
            formId: Guid.createEmpty(),
        } as unknown as FormSubmissionAction;

        if (!store.workflow.actions)
            store.workflow.actions = [action]
        else
            store.workflow.actions.push(action);
    }

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
    <WorkflowTemplate v-if="store.workflow" :model="store.workflow" />

    

</template>

