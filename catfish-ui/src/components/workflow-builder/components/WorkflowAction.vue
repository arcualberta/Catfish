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
            <select>
                <option value="b01e8c95-abef-4db2-ac3a-9ab35c4f6e11">TO DO Form 1</option>
                <option value="139e7084-20ba-4268-9396-61bac78cda52">TO DO Form 2</option>
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