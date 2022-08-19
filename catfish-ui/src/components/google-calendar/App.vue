<template>
    <h3 style="color: green">Google Calendar</h3>
    
    <h3>Upcoming events</h3>
    <!--<div v-for="itm in upcomingEvents">
        <div>Title: {{itm.summary}}</div>
        <div>Description: {{itm.description}}</div>
        <div>Start Date: {{itm.start.dateTime}}</div>
    </div>-->
 
    <div id="es-calendar">
      
    </div>
    
</template>
<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onMounted } from 'vue'
    //import { Guid } from "guid-typescript";

    //ES6 fullcalendar
    import '@fullcalendar/core/vdom';
    import { Calendar } from '@fullcalendar/core';
    import googleCalendarPlugin from '@fullcalendar/google-calendar';
    import dayGridPlugin from '@fullcalendar/daygrid';
    import listViewPlugin from '@fullcalendar/list';
    import { useGoogleCalendarStore } from './store';
    import config from '../../appsettings'


    const props = defineProps < {
        piniaInstance: Pinia,
        
    } > ();

    const store = useGoogleCalendarStore(props.piniaInstance);

    const events = store.loadEvents();
    const upcomingEvents = computed(() => store.getUpcomingEvents());

    // lifecycle hooks
    onMounted(() => {
      
        let calendarEl = document.getElementById('es-calendar') as HTMLElement;
        let calIds: object[] = [] ;

      // calIds =  store.getCalendarIds;

        config.googleCalendarIds.map(function (cid) {
           
            let calId= { googleCalendarId: cid, className:'gcal-event' }
            calIds?.push(calId);
        });
       
        let calendar = new Calendar(calendarEl, {
            plugins: [googleCalendarPlugin, dayGridPlugin, listViewPlugin],
            googleCalendarApiKey: config.googleApiKey,

            eventSources: calIds,
            initialView:  'dayGridMonth', //'listMonth',
            //views: {
            //    listDay: { buttonText: 'list day' },
            //    listWeek: { buttonText: 'list week' },
            //    listMonth: { buttonText: 'list month' }
            //},
            
          
        });

        calendar.render();
    });
</script>