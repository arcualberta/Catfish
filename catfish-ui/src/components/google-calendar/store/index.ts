import { defineStore } from 'pinia';
import  {Item } from '../models'
import config from '../../../appsettings'

export const useGoogleCalendarStore = defineStore('GoogleCalendarStore', {
    state: () => ({
      
        events: null as Item[] | null,
        maxEvents: null as number | null,
        
    }),
    actions: {
        loadEvents() {

            let api = `https://www.googleapis.com/calendar/v3/calendars/` + config.googleCalendarId + `/events?key=` + config.googleApiKey;
            console.log(api)
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
        getUpcomingEvents(maxEvents: number) {

        }
    }

});