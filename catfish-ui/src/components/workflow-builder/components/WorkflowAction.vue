<script setup lang="ts">
    import { WorkflowAction } from '../models/'

    defineProps<{ model: WorkflowAction }>();

</script>

<template>
    <div class="action">
        Workflow Action: {{model.id}}
        <div>
            <h4>{{model.name}}</h4>
            Name: <input type="text" v-model="model.name" />
        </div>
        <div>
            <p>{{model.description}}</p>
            Description: <textarea v-model="model.description" />
        </div>

        <div>
            <p>{{model.description}}</p>
            Form:
            <select v-model="model.formId">
                <option>TO DO Form 1</option>
                <option>TO DO Form 2</option>
            </select>
        </div>
    </div>
</template>

<style scoped>
    .action{
        border: solid 1px #8888;
        margin: 15px;
        padding: 15px;
    }
</style>