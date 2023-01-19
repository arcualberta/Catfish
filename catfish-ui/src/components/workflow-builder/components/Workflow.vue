<script setup lang="ts">
    import { Workflow, TabNavigationDefinition } from '../models/'
    import TabNav from '@/components/shared/components/TabNav/TabNav.vue';
    import {default as WorkflowStates} from './WorkflowStateList.vue'
    import {default as WorkflowRoles} from './WorkflowRoleList.vue'
    import { default as EmailTemplates } from './EmailTemplateList.vue';
    import { default as  tabMenu} from "@/appsettings";
    import { computed, ref } from 'vue';

    const props = defineProps < { model: Workflow } > ();
</script>

<template>
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