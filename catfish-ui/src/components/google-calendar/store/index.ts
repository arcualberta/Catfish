import { defineStore } from 'pinia';
import  {Item } from '../models'
import config from '../../../appsettings'


export const useGoogleCalendarStore = defineStore('GoogleCalendarStore', {
    state: () => ({
      
        events: null as Item[] | null, //all events
       
        upcomingEvents: null as Item[] | null
        
    }),
    actions: {
        loadEvents() {

            let api = `https://www.googleapis.com/calendar/v3/calendars/` + config.googleCalendarId + `/events?key=` + config.googleApiKey;
            
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.events = data.items;
                    console.log("events" + this.events)
                })
                .catch((error) => {
                    console.error('Load google api Error:', error);
                });

        },
        getUpcomingEvents() {
            //console.log("max event" + config.maxEvents)
            //return this.events?.filter(this.checkCurrEvent)
            console.log("all event" + this.events);
            const futureEvents = this.events?.filter((ev) => this.checkCurrEvent(ev));
            console.log("filtered" + futureEvents);

            return futureEvents;
          
        },
        checkCurrEvent(ev:Item){
            if (ev.start?.dateTime && (new Date(ev.start?.dateTime) >= new Date())) {
                return ev;
            }
          }
    }
});