import { defineStore } from 'pinia';
import  {Item } from '../models'
import config from '../../../appsettings'
import moment from 'moment'

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
        currentDateTime() {
            const current = new Date();
            const date = current.getFullYear() + '-' + (current.getMonth() + 1) + '-' + current.getDate();
            const time = current.getHours() + ":" + current.getMinutes() + ":" + current.getSeconds();
            const dateTime = date + ' ' + time;

            return dateTime;
        },
        getUpcomingEvents() {

            console.log("max event" + config.maxEvents)
            
            let count = 1;
            //this.events?.forEach((ev) => {
            //    let end = moment(ev.end?.dateTime).format("DD-MM-YYYY")
            //    let curr = moment(this.currentDateTime()).format("DD-MM-YYYY")
            //    if (end > curr) {
                   
            //        if (count <= config.maxEvents) {
                       
            //            this.upcomingEvents?.push(ev);
            //            count++;
            //        }  
            //    }
            //});
            return this.events?.filter(this.checkCurrEvent)
            //console.log("upcoming events:")
            //console.log(JSON.stringify(this.upcomingEvents))
          
        },
        checkCurrEvent(ev:Item){
            let end = moment(ev.end?.dateTime).format("DD-MM-YYYY")
            let curr = moment(this.currentDateTime()).format("DD-MM-YYYY")
            if (end > curr) {
                return ev;
            }
          }
    }

});