export default {
    googleApiKey: "Use dev credentials from https://docs.google.com/document/d/1N_y4aQupxPKPGh2eaxpOqCmc_75QionPp4U_MoY3gZQ/edit",
    googleCalendarIds: ["xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"],
    maxEvents: 10,
    initialView: 'dayGridMonth',
    dataRepositoryApiRoot: "https://localhost:5020"
}


import { eFieldType } from '@/components/shared/constants'
import {SearchFieldDefinition} from '@/components/solr-search-panel/models'

export enum eShowtimeEntryType {
    Movie = 1,
    Theater,
    Showtime
}


export const solrFields:SearchFieldDefinition[] = [
    { name: "entry_type_s", label: "Entry Type", type: eFieldType.Text, options: [], entryType: [eShowtimeEntryType.Movie, eShowtimeEntryType.Theater, eShowtimeEntryType.Showtime]},
    { name: "entry_src_s", label: "Source", type: eFieldType.Text, options: [], entryType: [eShowtimeEntryType.Movie, eShowtimeEntryType.Theater, eShowtimeEntryType.Showtime]},
    { name: "movie_id_i", label: "Movie ID", type: eFieldType.Integer, options: [], entryType: [eShowtimeEntryType.Movie, eShowtimeEntryType.Showtime] },
    { name: "parent_id_i", label: "Parent ID", type: eFieldType.Integer, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "title_t", label: "Title", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "pictures_ts", label: "Pictures", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "hipictures_ts", label: "Hi-pictures", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "rating_t", label: "Rating", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "advisory_t", label: "Advisory", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "genres_ts", label: "Genres", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "casts_ts", label: "Cast", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "directors_ts", label: "Directors", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "release_date_dt", label: "Release Date", type: eFieldType.Date, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "release_notes_t", label: "Release Notes", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "release_dvd_t", label: "Release DVD", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "running_time_i", label: "Running Time", type: eFieldType.Integer, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "official_site_s", label: "Official Site", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "distributors_ts", label: "Distributes", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "producers_ts", label: "Producers", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "writers_ts", label: "Writers", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "synopsis_t", label: "Synopsis", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "lang_s", label: "Language", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "intl_country_s", label: "International country", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "intl_name_t", label: "International name", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "intl_cert_t", label: "International cert", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "intl_advisory_t", label: "International advisory", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "intl_release_dt", label: "International release date", type: eFieldType.Date, options: [], entryType: eShowtimeEntryType.Movie },
    { name: "intl_poster_t", label: "International poster", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Movie },


    { name: "showtime_key_t", label: "Showtime composite ID", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "movie_name_t", label: "Movie name", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "show_date_dt", label: "Show date", type: eFieldType.Date, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "showtimes_ts", label: "Show time (HH:mm)", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "showtime_minutes_is", label: "Show minutes", type: eFieldType.Integer, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "show_attributes_ts", label: "Show attrbibutes", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "show_passes_t", label: "Show passes", type: eFieldType.Radio, options:["Y", "N"], entryType: eShowtimeEntryType.Showtime },
    { name: "show_festival_t", label: "Show festival", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "show_with_t", label: "Show with", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "show_sound_t", label: "Show sound", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },
    { name: "show_comments_ts", label: "Show comments", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Showtime },

    
    { name: "theater_id_i", label: "Theater ID", type: eFieldType.Integer, options: [], entryType: [eShowtimeEntryType.Theater, eShowtimeEntryType.Showtime] },
    { name: "theater_name_t", label: "Theater name", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_address_t", label: "Theater address", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_city_t", label: "Theater city", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_state_t", label: "Theater state", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_zip_t", label: "Theater zip", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_phone_t", label: "Theater phone", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_attributes_t", label: "Theater attributes", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_ticketing_t", label: "Theater ticketing", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_closed_reason_t", label: "Theater closed reason", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_area_t", label: "Theater area", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_location_t", label: "Theater location", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_market_t", label: "Theater market", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_screens_i", label: "Theater Screens", type: eFieldType.Integer, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_seating_t", label: "Theater seating", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_adult_t", label: "Theater adult", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_child_t", label: "Theater child", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_senior_t", label: "Theater senior", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_country_s", label: "Theater country", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_url_t", label: "Theater URL", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_chain_id_t", label: "Theater chain ID", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_adult_bargain_t", label: "Theater adult bargain", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_senior_bargain_t", label: "Theater senior bargain", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_child_bargain_t", label: "Theater child bargain", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_special_bargain_t", label: "Theater special bargain", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_adult_super_t", label: "Theater adult super", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_senior_super_t", label: "Theater senior super", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_chuld_super_t", label: "Theater child super", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_price_comment_t", label: "Theater price comment", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_extra_t", label: "Theater extra", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_desc_t", label: "Theater description", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_type_t", label: "Theater type", type: eFieldType.Text, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_lon_d", label: "Theater longitude", type: eFieldType.Decimal, options: [], entryType: eShowtimeEntryType.Theater },
    { name: "theater_lat_d", label: "Theater latitude", type: eFieldType.Decimal, options: [], entryType: eShowtimeEntryType.Theater },
];

