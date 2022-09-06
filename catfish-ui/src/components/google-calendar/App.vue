<template>
    <h3 style="color: green">Google Calendar</h3>
    
    <h3>Upcoming events</h3>
   
    <div v-bind:id="cidEl" :class="cssClass">
      
    </div>
    
</template>
<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { onMounted, toRef } from 'vue'
    import { AppletAttribute } from '../shared/props'
    import { Guid } from "guid-typescript";

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
        dataAttributes: AppletAttribute,
        queryParameters: AppletAttribute | null
     } > ();

     const _dataAttributes = toRef(props, 'dataAttributes')
     
     const cssClass = _dataAttributes.value["css-class"] as string;
    
     const store = useGoogleCalendarStore(props.piniaInstance);
    const cidEl=Guid.create().toString() as unknown as Guid
   // const events = store.loadEvents();
   // const upcomingEvents = computed(() => store.getUpcomingEvents());
   
    // lifecycle hooks
    onMounted(() => {
      
        let calendarEl = document.getElementById(cidEl) as HTMLElement;
        let calIds: object[] = [] ;
        
        //get the calendar id(s) from the dataattributes)
        let gCalIds: string[] = (_dataAttributes.value["calendar-ids"]) as unknown as Array<string>;
        gCalIds.map(function (cid) {
           
            let calId= { googleCalendarId: cid.trim(), className:'gcal-event' }
            calIds?.push(calId);
        });
        let displayStyle = _dataAttributes.value["display-style"] as string;
        
        const apikey =_dataAttributes.value["api-key"] as string;
         //use the api key from the data attribute if existed, if not use the one in the appsettings
        let gApiKey = apikey? apikey: config.googleApiKey;

        let calendar = new Calendar(calendarEl, {
            plugins: [googleCalendarPlugin, dayGridPlugin, listViewPlugin],
            googleCalendarApiKey: gApiKey, //config.googleApiKey,

            eventSources: calIds,
            initialView: displayStyle //config.initialView,//'dayGridMonth', //'listMonth',
            //views: {
            //    listDay: { buttonText: 'list day' },
            //    listWeek: { buttonText: 'list week' },
            //    listMonth: { buttonText: 'list month' }
            //},
        });

        calendar.render();
    });
</script>