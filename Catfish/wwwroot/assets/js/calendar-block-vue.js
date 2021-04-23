/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("calendar-block-vue", {
    props: ["uid", "toolbar", "model"],
    
    methods: {
        onBlur: function (e) {
            var elName = e.target.name;

            if (elName === "calendarId") {

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
            else if (elName == "displayCalendarUI") {
                this.model.displayCalendarUI.value = e.target.value;
                var content = this.model.displayCalendarUI.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        displayCalendarUI: content
                    });
                }
            }
            else if (elName == "calendarStyle") {
                this.model.calendarStyle.value = e.target.value;
                var content = this.model.calendarStyle.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        calendarStyle: content
                    });
                }
            }
        }
    },
    computed: {
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
        displayCalendarUIValue: {
            get: function () {
                return this.model.displayCalendarUI.value;
            }
        },
        calendarStyleValue: {
            get: function () {
                return this.model.calendarStyle.value;
            }
        },
        isCalendarIdEmpty: function () {
            return piranha.utils.isEmptyText(this.model.calendarId.value);
        }
    },
    template: `
    <div class= 'block-body calendar-block'>
        <div class='lead row'><label class='form-label col-md-3 required'>Calendar Id: </label><input  class='form-control col-md-8' type='text' name='calendarId' v-model='model.calendarId.value' contenteditable='true' v-on:blur='onBlur' value='cidValue'  :class='{ requiredField: isCalendarIdEmpty }' /></div>
        <div class='lead row' ><label class='form-label col-md-3'>Numb. Past Days:</label> <input type='number' class='form-control col-md-4' name='pastDays' v-model='model.daysRangePast.value' contenteditable='true' v-on:blur='onBlur' value='pastDaysValue' /></div>
        <div class='lead row'><label class='form-label col-md-3'>Number of Future Days:</label> <input type='number' class='form-control col-md-4' name='futureDays' v-model='model.daysRangeFuture.value' contenteditable='true' v-on:blur='onBlur' value='futureDaysValue'  /></div>
        <div class='lead row'><label class='form-label col-md-3'>Max Events:</label> <input type='number' class='form-control col-md-4' name='maxEvents' v-model='model.maxEvents.value' contenteditable='true' v-on:blur='onBlur' value='maxEventsValue'  /></div>
        <div class='lead row'><label class='form-label col-md-3'>Display a Calendar:</label>
            <select class='form-control col-md-4' name="displayCalendarUI" v-on:blur='onBlur' :value='displayCalendarUIValue' id="display-calendar-select">
                <option value="0">Do Not Display Calendar</option>
                <option value="1">Regular Calendar</option>
                <option value="2">Weekly Strip</option>
           </select>
        </div>
        <div class='lead row'><label class='form-label col-md-3'>Calendar Style:</label>
            <select class='form-control col-md-4' name="calendarStyle" v-on:blur='onBlur' :value='calendarStyleValue' id="calendar-style-select">
                <option value="0">Simple</option>
                <option value="1">Rounded</option>
           </select>
        </div>
    </div>
    `
});