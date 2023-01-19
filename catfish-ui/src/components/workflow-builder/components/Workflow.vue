<script setup lang="ts">
    import { Workflow, TabNavigationDefinition } from '../models/'
    import TabNav from '@/components/shared/components/TabNav/TabNav.vue';
    import {default as WorkflowStates} from './WorkflowStateList.vue'
    import {default as WorkflowRoles} from './WorkflowRoleList.vue'
    import { default as EmailTemplates } from './EmailTemplateList.vue';
    import { default as  tabMenu} from "@/appsettings";
    import { computed, onMounted, ref } from 'vue';
    import {useWorkflowBuilderStore} from '../store'
import { SelectableOption } from '@/components/shared/components/form-field-selection-dropdown/models';

    const props = defineProps < { model: Workflow } > ();
    const store = useWorkflowBuilderStore();
    onMounted(() => {
         //load entity templates
         store.loadEntityTemplates();
    });
    const selectedTemplateId =ref(store.workflow?.entityTemplateId); 
    const templateOptions=computed(()=>{
          const options = store.entityTemplates.map(template => {
                return {
                    value: template.id,
                    text: template.name
                } as SelectableOption
            });
        return options;
    });

</script>

<template>
   <b-row>
     <h5 class="col-2">Entity Templates</h5>
      <div class="col-4">
            <b-form-select v-model="selectedTemplateId" :options="templateOptions"></b-form-select>
        selectedTemplateId: {{selectedTemplateId}}
      </div>
      
   </b-row>
   <div>
  <TabNav :tabs="['Action', 'States', 'Roles', 'Templates', 'Triggers', 'Pop-ups']">
    <template v-slot:Action>
      Please select new Action.
    </template>
    <template v-slot:States>
      <workflow-states />
    </template>
    <template v-slot:Roles>
      <workflow-roles />
    </template>
    <template v-slot:Templates>
      <email-templates />
    </template>
    <template v-slot:Triggers>
      Please select new Triggers.
    </template>
    <template v-slot:Pop-ups>
      Please select new Pop-ups.
    </template>
  </TabNav>
</div>
</template>