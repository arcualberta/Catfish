export interface googleCalendarApi {
    key: string;
    googleCalendarId: string;
}

export interface eventDate {
    dateTime: string | null;
    timeZone: string | null;
}
export interface Item {
    start: eventDate | null;
    end: eventDate | null;
    summary: string | null;
    description: string | null;
    htmlLink: string | null;
}