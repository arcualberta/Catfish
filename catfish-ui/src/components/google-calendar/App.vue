<template>
    <h3 style="color: green">Google Calendar</h3>
    <div>
        {{JSON.stringify(store.events)}}
    </div>
    <h3>Upcoming events</h3>
    <div>
        {{JSON.stringify(upcomingEvents)}}
    </div>
</template>
<script setup lang="ts">
    import { Pinia } from 'pinia'
    //import { Guid } from "guid-typescript";

    import { useGoogleCalendarStore } from './store';
    const props = defineProps < {
        piniaInstance: Pinia,
        
    } > ();

    const store = useGoogleCalendarStore(props.piniaInstance);

    const events = store.loadEvents();
    const upcomingEvents = store.getUpcomingEvents();
</script>