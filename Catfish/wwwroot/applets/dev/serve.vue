<script lang="ts">
import { defineComponent } from 'vue';
// Uncomment import and local "components" registration if library is not registered globally.
// import { AppletsSample } from '@/entry.esm';

export default defineComponent({
  name: 'ServeDev',
  // components: {
  //  AppletsSample,
  // }
});
</script>

<template>
  <div id="app">
    <applets-sample />
  </div>
</template>
