import { eFieldType } from '../shared/constants'
import {SearchFieldDefinition} from './model'

export const SolrFields:SearchFieldDefinition[] = [
    { name: "Mmovie_name_t", label: "Movie name", type: eFieldType.Text, options: [] },
    { name: "show_date_dt", label: "Show daye", type: eFieldType.Date, options: [] },
    { name: "showtimes_ts", label: "Show time (HH:mm)", type: eFieldType.Text, options: [] },
    { name: "showtime_minutes_is", label: "Show minutes", type: eFieldType.Integer, options: [] },
    { name: "show_passes_t", label: "Show passes", type: eFieldType.Radio, options:["Y", "N"]},
    { name: "show_festival_t", label: "Show festival", type: eFieldType.Radio, options: ["Y", "N"] },
    { name: "show_comments_ts", label: "Show comments", type: eFieldType.Text, options: [] },
    { name: "title_t", label: "Title", type: eFieldType.Text, options: [] },
    { name: "genres_ts", label: "Genres", type: eFieldType.Text, options: [] },
    //{ name: "pictures_ts", label: "Pictures", type: eFieldType.Text, options: [] },
    { name: "rating_t", label: "Rating", type: eFieldType.Text, options: [] },
    { name: "advisory_t", label: "Advisory", type: eFieldType.Text, options: [] },
    { name: "casts_ts", label: "Cast", type: eFieldType.Text, options: [] },
    { name: "directors_ts", label: "Directors", type: eFieldType.Text, options: [] },
    { name: "release_date_dt", label: "Release Date", type: eFieldType.Date, options: [] },
    { name: "release_notes_t", label: "Release Notes", type: eFieldType.Text, options: [] },
    { name: "running_time_i", label: "Running Time", type: eFieldType.Integer, options: [] },
    { name: "distributors_ts", label: "Distributes", type: eFieldType.Text, options: [] },
    { name: "writers_ts", label: "Writers", type: eFieldType.Text, options: [] },
    { name: "synopsis_t", label: "Synopsis", type: eFieldType.Text, options: [] },
    { name: "lang_t", label: "Language", type: eFieldType.Text, options: [] },
    { name: "intl_name_t", label: "Initial Name", type: eFieldType.Text, options: [] },
    { name: "intl_country_t", label: "Initial Country", type: eFieldType.Text, options: [] },
    { name: "intl_cert_t", label: "Initial Cert", type: eFieldType.Text, options: [] },
    { name: "intl_release_dt", label: "Initial Release Date", type: eFieldType.Date, options: [] },
    { name: "theater_name_t", label: "Theater Name", type: eFieldType.Text, options: [] },
    { name: "theater_address_t", label: "Theater Address", type: eFieldType.Text, options: [] },
    { name: "theater_city_t", label: "Theater City", type: eFieldType.Text, options: [] },
    { name: "theater_state_t", label: "Theater State", type: eFieldType.Text, options: [] },
    { name: "theater_zip_t", label: "Theater Zip", type: eFieldType.Text, options: [] },
    { name: "theater_phone_t", label: "Theater Phone", type: eFieldType.Text, options: [] },
    { name: "theater_ticketing_t", label: "Theater Ticketing", type: eFieldType.Text, options: [] },
    { name: "theater_location_t", label: "Theater Location", type: eFieldType.Text, options: [] },
    { name: "theater_screens_i", label: "Theater Screens", type: eFieldType.Integer, options: [] },
    { name: "theater_country_t", label: "Theater Country", type: eFieldType.Text, options: [] },
    { name: "theater_url_t", label: "Theater URL", type: eFieldType.Text, options: [] },


];

