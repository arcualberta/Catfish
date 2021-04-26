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
    props: ["uid", "model", "googlecalendardata", "calendardisplay"],

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
            today: null,

            //for weekly strip
            daysSection: [],
            weekSliceNum: 7,
            //not the best solution, figure out something else
            //0 = neutral, -1 = backwards, 1 = forwards
            lastAction: 0,
        }
    },

    methods: {

        //code from Css-Tricks monthly Calendar with real data
        //https://css-tricks.com/how-to-make-a-monthly-calendar-with-real-data/
        //Accessed April 8 2021

        getNumberOfDaysInMonth(year, month) {
            return dayjs(`${year}-${month}-01`).daysInMonth();
        },

        //returns what day of the week the date is (Mon/Tues/Weds/etc)
        getWeekday(date) {
            return dayjs(date).weekday()
        },

        //note: index is the day (index + 1), day is undefined
        createDaysForCurrentMonth(year, month) {
            return [...Array(this.getNumberOfDaysInMonth(year, month))].map((day, index) => {
                return {
                    date: dayjs(`${year}-${month}-${index + 1}`).format("YYYY-MM-DD"),
                    dayOfMonth: index + 1,
                    isCurrentMonth: true,
                    hasEvent: this.checkIfEventDay(index + 1, month, year)
                };
            });
        },

        //note: index is the day (index + 1), day is undefined
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
                    isCurrentMonth: false,
                    hasEvent: this.checkIfEventDay(index + 1, month, year)
                };
            });
        },

        //note: index is the day (index + 1), day is undefined
        createDaysForNextMonth(year, month) {
            const lastDayOfTheMonthWeekday = this.getWeekday(`${year}-${month}-${this.currentMonthDays.length}`)


            const visibleNumberOfDaysFromNextMonth = lastDayOfTheMonthWeekday ? 6 - lastDayOfTheMonthWeekday : 6//lastDayOfTheMonthWeekday;


            return [...Array(visibleNumberOfDaysFromNextMonth)].map((day, index) => {
                return {
                    date: dayjs(`${year}-${Number(month) + 1}-${index + 1}`).format("YYYY-MM-DD"),
                    dayOfMonth: index + 1,
                    isCurrentMonth: false,
                    hasEvent: this.checkIfEventDay(index + 1, month, year)
                }
            })
        },

        //for now only works for start day, not multiple days
        checkIfEventDay(day, month, year) {
            for (let calendarEvent of this.googlecalendardata) {
                if (calendarEvent.StartDay == day.toString() && calendarEvent.StartMonth == dayjs(month).format('MMM') && calendarEvent.StartYear == year) {
                    return true;
                }
            }
            return false;
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

        },

        goToNextWeek() {
            //if week exceeds the current month
            let lastWeekInMonth = this.days.slice(-7);
            let isSameWeek = this.compareWeeks(lastWeekInMonth);
            if (isSameWeek) {
                this.goToNextMonth();
                this.weekSliceNum = 7;
            } else if (this.lastAction < 0) {
                this.weekSliceNum -= 7;
            }

            this.weekSliceNum += 7;
            this.daysSection = this.days.slice((this.weekSliceNum - 7), this.weekSliceNum);
            this.lastAction = 1;
        },

        goToPreviousWeek() {
            //if week exceeds the current month
            let firstWeekInMonth = this.days.slice(0, 7);
            let isSameWeek = this.compareWeeks(firstWeekInMonth);
            if (isSameWeek) {
                this.goToPreviousMonth();
                this.weekSliceNum = this.days.length;
                console.log(this.weekSliceNum);
            } else if(this.lastAction > 0) {
                this.weekSliceNum -= 7;
            }

            this.weekSliceNum -= 7;
            this.daysSection = this.days.slice(this.weekSliceNum, this.weekSliceNum + 7);
            this.lastAction = -1;
        },

        /**
         * Object comparison of day objects
         * @returns boolean true/false value
         * */
        compareWeeks(week) {
            for (const [i, day] of this.daysSection.entries()) {
                if (day !== week[i]) {
                    return false;
                }
            }
            return true;
        }
    },

    created () {
        dayjs.extend(weekday);
        dayjs.extend(weekOfYear);

        this.initialYear = dayjs().format("YYYY");
        this.initialMonth = dayjs().format("M");
        this.selectedMonth = dayjs(new Date(this.initialYear, this.initialMonth - 1, 1));

        /*
         CalendarUI Enum values
         0 - do not display calendar
         1 - regular calendar
         2 - weekyl strip calendar
         */
        //switch (this.calendarDisplay) {
        //    //no calendar
        //    case 0:
        //        break;
        //    //regular calendar
        //    case 1:
        //        this.currentMonthDays = this.createDaysForCurrentMonth(this.initialYear, this.initialMonth);
        //        this.previousMonthDays = this.createDaysForPreviousMonth(this.initialYear, this.initialMonth, this.currentMonthDays[0]);
        //        this.nextMonthDays = this.createDaysForNextMonth(this.initialYear, this.initialMonth);
        //        break;
        //    //weekly strip calendar
        //    case 2:
        //        //this.currentWeekDays = this.createDaysForCurrentWeek(this.initialYear, this.initialMonth, this.initialDay);
        //        //this.previousMonthDays = this.
        //        break;
        //    //no calendar
        //    default:
        //        break;
        //}
        

        this.currentMonthDays = this.createDaysForCurrentMonth(this.initialYear, this.initialMonth);
        this.previousMonthDays = this.createDaysForPreviousMonth(this.initialYear, this.initialMonth, this.currentMonthDays[0]);
        this.nextMonthDays = this.createDaysForNextMonth(this.initialYear, this.initialMonth);
        this.today = dayjs().format("YYYY-MM-DD");

        this.days = [...this.previousMonthDays, ...this.currentMonthDays, ...this.nextMonthDays];

        if (this.calendardisplay == 2) {
            this.daysSection = this.days.slice(0, this.weekSliceNum);
        }
    },

});