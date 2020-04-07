/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("calendar-block", {
    props: ["uid", "toolbar", "model"],
    
    methods: {
        onBlur: function (e) {
            var elName = e.target.name;
            if (elName === "apiKey") {

                this.model.apiKey.value = e.target.value;
                var content = this.model.apiKey.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        apiKey: content
                    });
                }
            }
            else if (elName === "calendarId") {

                this.model.calendarId.value = e.target.value;
                var content = this.model.calendarId.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        calendarId: content
                    });
                }
            } else if (elName == "pastDays") {
                this.model.daysRangePast.value = e.target.value;
                var content = this.model.daysRangePast.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        daysRangePast: content
                    });
                }
            } else if (elName == "futureDays") {
                this.model.daysRangeFuture.value = e.target.value;
                var content = this.model.daysRangeFuture.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        daysRangeFuture: content
                    });
                }
            }
            else if (elName == "maxEvents") {
                this.model.maxEvents.value = e.target.value;
                var content = this.model.maxEvents.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        maxEvents: content
                    });
                }
            }
        }
    },
    computed: {
        apiKeyValue: {
            get: function () {
                return this.model.apiKey.value;
            }
        },
        cidValue: {
            get: function () {
                return this.model.calendarId.value;
            }
        },
        pastDaysValue: {
            get: function () {
                return this.model.daysRangePast.value;
            }
        },
        futureDaysValue: {
            get: function () {
                return this.model.daysRangeFuture.value;
            }
        },
        maxEventsValue: {
            get: function () {
                return this.model.maxEvents.value;
            }
        },
        isApiKeyEmpty: function () {
            return piranha.utils.isEmptyText(this.model.apiKey.value);
        },
        isCalendarIdEmpty: function () {
            return piranha.utils.isEmptyText(this.model.calendarId.value);
        }
    },
    template: "<div  class= 'block-body calendar-block'>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>API Key: </label><input class='form-control col-md-8' type='text' name='apiKey' v-model='model.apiKey.value' contenteditable='true' v-on:blur='onBlur' value='apiKeyValue'  :class='{ requiredField: isApiKeyEmpty }' /></div>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>Calendar Id: </label><input  class='form-control col-md-8' type='text' name='calendarId' v-model='model.calendarId.value' contenteditable='true' v-on:blur='onBlur' value='cidValue'  :class='{ requiredField: isCalendarIdEmpty }' /></div>" +
        "<div class='lead row' ><label class='form-label col-md-3'>Numb. Past Days:</label> <input type='number' class='form-control col-md-4' name='pastDays' v-model='model.daysRangePast.value' contenteditable='true' v-on:blur='onBlur' value='pastDaysValue' /></div>" +
        "<div class='lead row'><label class='form-label col-md-3'>Number of Future Days:</label> <input type='number' class='form-control col-md-4' name='futureDays' v-model='model.daysRangeFuture.value' contenteditable='true' v-on:blur='onBlur' value='futureDaysValue'  /></div>" +
        "<div class='lead row'><label class='form-label col-md-3'>Max Events:</label> <input type='number' class='form-control col-md-4' name='maxEvents' v-model='model.maxEvents.value' contenteditable='true' v-on:blur='onBlur' value='maxEventsValue'  /></div>" +
      
    "</div>"
});