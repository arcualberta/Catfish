<script setup lang="ts">
    import { Workflow} from '../models/'
    import TabNav from '@/components/shared/components/TabNav/TabNav.vue';
    import {default as WorkflowActions} from './WorkflowActionList.vue'
    import {default as WorkflowStates} from './WorkflowStateList.vue'
    import {default as WorkflowRoles} from './WorkflowRoleList.vue'
    import {default as WorkflowTriggers} from './WorkflowTriggerList.vue'
    import { default as WorkflowEmailTemplates } from './WorkflowEmailTemplateList.vue';
    import { default as WorkflowPopups } from './WorkflowPopupList.vue';
    import { computed, onMounted, ref } from 'vue';
    import {useWorkflowBuilderStore} from '../store'
    import { SelectableOption } from '@/components/shared/components/form-field-selection-dropdown/models';


    const props = defineProps < { model: Workflow } > ();
    const store = useWorkflowBuilderStore();
    onMounted(() => {
         //load entity templates
        
         store.loadEntityTemplates();
    });
    
    const templateOptions=computed(()=>{
          const options = store.entityTemplates.map(template => {
                return {
                    value: template.id,
                    text: template.name
                } as SelectableOption
            });
        return options;
    });
 const workflow =computed(()=>store.workflow);
</script>

<template>
  <div class="col-sm-6">
    <b-input-group prepend="Name" class="mt-3">
      <b-form-input v-model="model.name" ></b-form-input>
    </b-input-group>
    <b-input-group prepend="Description" class="mt-3">
      <b-form-textarea v-model="model.description" rows="3" max-rows="6"></b-form-textarea>
    </b-input-group>
  </div>
  
  <b-row class="col-sm-6">
    <b-col class="col-sm-4 header-style">
      Entity Template:
    </b-col>
    <b-col class="col-sm-8 header-style">
      <b-form-select v-model="workflow!.entityTemplateId" :options="templateOptions"></b-form-select>
    </b-col>
  </b-row>
  <div class="tab-view">
    <TabNav :tabs="['States', 'Roles', 'Templates', 'Triggers', 'Pop-ups', 'Action']">
      <template v-slot:Action>
        <workflow-actions />
      </template>
      <template v-slot:States>
        <workflow-states />
      </template>
      <template v-slot:Roles>
        <workflow-roles />
      </template>
      <template v-slot:Templates>
        <workflow-email-templates />
      </template>
      <template v-slot:Triggers>
        <workflow-triggers />
      </template>
      <template v-slot:Pop-ups>
        <workflow-popups />
      </template>
    </TabNav>
  </div>
</template>