import { defineStore } from 'pinia';
import { Item, GoogleCalendarId } from '../models'
import config from '../../../appsettings'
import { Guid } from 'guid-typescript';


export const useGoogleCalendarStore = defineStore('GoogleCalendarStore', {
    state: () => ({
        id: null as Guid | null,
        events: null as Item[] | null, //all events
        upcomingEvents: null as Item[] | null,
        calendarIds: null as  [] | null
        
    }),
    actions: {
        loadEvents() {

            let api = `https://www.googleapis.com/calendar/v3/calendars/` + config.googleCalendarIds[0] + `/events?key=` + config.googleApiKey;
            
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.events = data.items;
                    //console.log("events" + this.events)
                })
                .catch((error) => {
                    console.error('Load google api Error:', error);
                });

        },
        getUpcomingEvents() {
            //console.log("max event" + config.maxEvents)
            //return this.events?.filter(this.checkCurrEvent)
           // console.log("all event" + this.events);
            const futureEvents = this.events?.filter((ev) => this.checkCurrEvent(ev));
           // console.log("filtered" + futureEvents);

            return futureEvents;
          
        },
        checkCurrEvent(ev:Item){
            if (ev.start?.dateTime && (new Date(ev.start?.dateTime) >= new Date())) {
                return ev;
            }
        },
       
    },
    getters: {
        getCalendarIds: (state) => {
           // let calIds: object[] = [];
            config.googleCalendarIds.map(function (cid) {
                let calId = { googleCalendarId: cid, className: 'gcal-event' }
               // calIds.push(calId);
                state.calendarIds?.push(calId);
            });
           return state.calendarIds;
        }
            
        
    }

   
});