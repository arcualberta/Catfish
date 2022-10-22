<template>
    <div :class="cssClass">
        <div v-if="title" class="title">{{title}}</div>
        <div v-if="description" class="description">{{description}}</div>
        <div v-bind:id="cidEl" class="reder-target">
        </div>
    </div>

</template>
<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { onMounted, toRef } from 'vue'
    import { AppletAttribute } from '@/components/shared/props'
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
        dataAttributes?: AppletAttribute | null,
        queryParameters?: AppletAttribute | null
     } > ();

     const _dataAttributes = toRef(props, 'dataAttributes')
     let _cssClass = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["css-class"] as string) : null;
     const cssClass =_cssClass ? "google-calendar " + _cssClass : "google-calendar";
     
     console.log(cssClass)
     const title = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["calendar-title"] as string): null;
     const description =_dataAttributes && _dataAttributes?.value? (_dataAttributes.value["calendar-description"] as string) : null
     const store = useGoogleCalendarStore();
    const cidEl=Guid.create().toString();
   
    // lifecycle hooks
    onMounted(() => {
      
        let calendarEl = document.getElementById(cidEl) as HTMLElement;
        let calIds: object[] = [] ;
        
        //get the calendar id(s) from the dataattributes)
        let gCalIds: string[] =_dataAttributes && _dataAttributes?.value?((_dataAttributes?.value["calendar-ids"]) as unknown as Array<string>): Array<string>();
        if(!gCalIds || !gCalIds.length)
           gCalIds = config.googleCalendarIds;

        gCalIds.map(function (cid) {  
            let calId= { googleCalendarId: cid.trim(), className:'gcal-event' }
            calIds?.push(calId);
        });
        let displayStyle = _dataAttributes && _dataAttributes?.value?(_dataAttributes?.value["display-style"] as string) : null;
        
        if(!displayStyle)
           displayStyle = config.initialView;

        const apikey =_dataAttributes && _dataAttributes?.value?(_dataAttributes?.value["api-key"] as string) : null;
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
<style scoped src="./style.css"></style>