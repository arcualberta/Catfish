<script setup lang="ts">
    import { Workflow, TabNavigationDefinition } from '../models/'
    import TabNav from '@/components/shared/components/TabNav/TabNav.vue';
    import {default as WorkflowState} from './WorkflowState.vue'
    import {default as WorkflowRoles} from './WorkflowRoles.vue'
    import { default as EmailTemplate } from './EmailTemplate.vue';
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
      <workflow-state />
    </template>
    <template v-slot:Roles>
      <workflow-roles :model="model" />
    </template>
    <template v-slot:Templates>
      <email-template />
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