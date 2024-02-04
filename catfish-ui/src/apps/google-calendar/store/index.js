import { defineStore } from 'pinia';
export const useGoogleCalendarStore = defineStore('GoogleCalendarStore', {
    state: () => ({
        id: null,
        events: null,
        upcomingEvents: null,
        calendarIds: null
    }),
    actions: {
    // loadEvents() {
    //     let api = `https://www.googleapis.com/calendar/v3/calendars/` + config.googleCalendarIds[0] + `/events?key=` + config.googleApiKey;  
    //     fetch(api, {
    //         method: 'GET'
    //     })
    //         .then(response => response.json())
    //        .then(data => {
    //            this.events = data.items;
    //console.log("events" + this.events)
    //         })
    //         .catch((error) => {
    //             console.error('Load google api Error:', error);
    //         });
    //  },
    //  getUpcomingEvents() {
    //      const futureEvents = this.events?.filter((ev) => this.checkCurrEvent(ev));
    // console.log("filtered" + futureEvents);
    //       return futureEvents;
    //    },
    //    checkCurrEvent(ev:Item){
    //       if (ev.start?.dateTime && (new Date(ev.start?.dateTime) >= new Date())) {
    //          return ev;
    //      }
    //  },
    },
});
//# sourceMappingURL=index.js.map