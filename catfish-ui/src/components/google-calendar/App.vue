<template>
    <h3 style="color: green">Google Calendar</h3>
    
    <h3>Upcoming events</h3>
    <div v-for="itm in upcomingEvents">
        <div>Title: {{itm.summary}}</div>
        <div>Description: {{itm.description}}</div>
        <div>Start Date: {{itm.start.dateTime}}</div>
      

    </div>
    
</template>
<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed } from 'vue'
    //import { Guid } from "guid-typescript";

    import { useGoogleCalendarStore } from './store';
    const props = defineProps < {
        piniaInstance: Pinia,
        
    } > ();

    const store = useGoogleCalendarStore(props.piniaInstance);

    const events = store.loadEvents();
    const upcomingEvents = computed(() => store.getUpcomingEvents());
</script>