/*
    Javascript for the Calendar block.
    Note that this is javascript solely for the visual calendar UI element,
    the javascript does not interact with the events listed.
    See CalendarBlock.cshtml or CalendarBlock.cs for more info.
 */

import dayjs from "dayjs";
const weekday = require("dayjs/plugin/weekday");
const weekOfYear = require("dayjs/plugin/weekOfYear");

Vue.component('calendar-block-vue', {
    props: ["uid", "model"],

    data: function () {
        return {
            WEEKDAYS: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            initialYear: null,
            initialMonth: null,
            days: [],
            currentMonthDays: [],
            previousMonthDays: [],
            nextMonthDays: [],
            selectedMonth: null,
            today: null
        }
    },

    methods: {

        //code from Css-Tricks monthly Calendar with real data
        //https://css-tricks.com/how-to-make-a-monthly-calendar-with-real-data/
        //Accessed April 8 2021
        getDaysInMonth(year, month) {
            return [...Array(this.getNumberOfDaysInMonth(year, month))].map((day, index) => {
                return {
                    date: dayjs(`${year}-${month}-${index + 1}`).format("YYYY-MM-DD"),
                    dayOfMonth: index + 1,
                    isCurrentMonth: true
                };
            });
        },

        getNumberOfDaysInMonth(year, month) {
            return dayjs(`${year}-${month}-01`).daysInMonth();
        },

        //returns what day of the week the date is (Mon/Tues/Weds/etc)
        getWeekday(date) {
            return dayjs(date).weekday()
        },

        createDaysForCurrentMonth(year, month) {
            return [...Array(this.getNumberOfDaysInMonth(year, month))].map((day, index) => {
                return {
                    date: dayjs(`${year}-${month}-${index + 1}`).format("YYYY-MM-DD"),
                    dayOfMonth: index + 1,
                    isCurrentMonth: true
                };
            });
        },

        createDaysForPreviousMonth(year, month) {
            const firstDayOfTheMonthWeekday = this.getWeekday(this.currentMonthDays[0].date);

            const previousMonth = dayjs(`${year}-${month}-01`).subtract(1, "month");

            // Account for first day of the month on a Sunday (firstDayOfTheMonthWeekday === 0)
            const visibleNumberOfDaysFromPreviousMonth = firstDayOfTheMonthWeekday; //? firstDayOfTheMonthWeekday - 1 : 6


            const previousMonthLastMondayDayOfMonth = dayjs(
                  this.currentMonthDays[0].date
              ).subtract(visibleNumberOfDaysFromPreviousMonth, "day").date();

            return [...Array(visibleNumberOfDaysFromPreviousMonth)].map((day, index) => {
                return {
                    date: dayjs(
                        `${previousMonth.year()}-${previousMonth.month() + 1}-${previousMonthLastMondayDayOfMonth + index}`
                    ).format("YYYY-MM-DD"),
                    dayOfMonth: previousMonthLastMondayDayOfMonth + index,
                    isCurrentMonth: false
                };
            });
        },

        createDaysForNextMonth(year, month) {
            const lastDayOfTheMonthWeekday = this.getWeekday(`${year}-${month}-${this.currentMonthDays.length}`)


            const visibleNumberOfDaysFromNextMonth = lastDayOfTheMonthWeekday ? 6 - lastDayOfTheMonthWeekday : lastDayOfTheMonthWeekday


            return [...Array(visibleNumberOfDaysFromNextMonth)].map((day, index) => {
                return {
                    date: dayjs(`${year}-${Number(month) + 1}-${index + 1}`).format("YYYY-MM-DD"),
                    dayOfMonth: index + 1,
                    isCurrentMonth: false
                }
            })
        },

        createCalendar(year = this.initialYear, month = this.initialMonth) {
            this.days = [];
            this.currentMonthDays = this.createDaysForCurrentMonth(
                year,
                month,
                dayjs(`${year}-${month}-01`).daysInMonth()
            );

            this.previousMonthDays = this.createDaysForPreviousMonth(year, month);
            this.nextMonthDays = this.createDaysForNextMonth(year, month);
            this.days = [...this.previousMonthDays, ...this.currentMonthDays, ...this.nextMonthDays];
        },

        goToPreviousMonth() {
            this.selectedMonth = dayjs(this.selectedMonth).subtract(1, "month");
            this.createCalendar(this.selectedMonth.format("YYYY"), this.selectedMonth.format("M"));
        },

        goToNextMonth() {
            this.selectedMonth = dayjs(this.selectedMonth).add(1, "month");
            this.createCalendar(this.selectedMonth.format("YYYY"), this.selectedMonth.format("M"));
        },

        goToPresentMonth() {
            this.selectedMonth = dayjs(new Date(this.initialYear, this.initialMonth - 1, 1));
            this.createCalendar(this.selectedMonth.format("YYYY"), this.selectedMonth.format("M"));

        }
    },

    created () {
        dayjs.extend(weekday);
        dayjs.extend(weekOfYear);

        this.initialYear = dayjs().format("YYYY");
        this.initialMonth = dayjs().format("M");
        this.selectedMonth = dayjs(new Date(this.initialYear, this.initialMonth - 1, 1));

        this.currentMonthDays = this.createDaysForCurrentMonth(this.initialYear, this.initialMonth);
        this.previousMonthDays = this.createDaysForPreviousMonth(this.initialYear, this.initialMonth, this.currentMonthDays[0]);
        this.nextMonthDays = this.createDaysForNextMonth(this.initialYear, this.initialMonth);
        this.today = dayjs().format("YYYY-MM-DD");


        this.days = [...this.previousMonthDays, ...this.currentMonthDays, ...this.nextMonthDays]

        console.log("dayjs working?", this.days);
    }
});