import { eFieldType } from '../shared/constants'
import {SearchFieldDefinition} from './model'

export const SolrFields:SearchFieldDefinition[] = [
    { name: "Mmovie_name_t", label: "Movie name", type: eFieldType.Text },
    { name: "show_date_dt", label: "Show daye", type: eFieldType.Date },
    { name: "showtimes_ts", label: "Show time (HH:mm)", type: eFieldType.Text },
    { name: "show_passes_ss", label: "Show passes", type: eFieldType.Radio, options:["YES", "NO"]},
];



/*
{
    MovieName = "movie_name_t",
    SHOWTIMEDATE = "show_date_dt",
    SHOWTIMES = "showtimes_ts",
    SHOWTIMEMINUTES = "showtime_minutes_is",
    SHOWPASSES = "show_passes_ss",
    SHOWFESTIVAL = "show_festival_ss",
    SHOWCOMMENTS = "show_comments_ts",
    TITLE = "title_t",
    GENRES = "genres_ts",
    PICTURES = "pictures_ts",
    RATING = "rating_t",
    ADVISORY = "advisory_t",
    CAST = "casts_ts",
    DIRECTORS = "directors_ts",
    RELEASEDATE = "release_date_dt",
    RELEASENOTE = "release_notes_t", 
    RUNNINGTIME = "running_time_i", 
    DISTRIBUTORS = "distributors_ts", 
    WRITERS = "writers_ts", 
    SYNOPSIS = "synopsis_t", 
    LANGUAGE = "lang_t", 
    INITIALNAME = "intl_name_t", 
    INITIALCOUNTRY = "intl_country_t", 
    INITIALCERT = "intl_cert_t", 
    INITIALRELEASEDATE = "intl_release_dt", 
    THEATERNAME = "theater_name_t", 
    THEATERADDRESS = "theater_address_t",
    THEATERCITY = "theater_city_t",  
    THEATERSTATE = "theater_state_t", 
    THEATERZIP = "theater_zip_t", 
    THEATERPHONE = "theater_phone_t", 
    THEATERTICKTING = "theater_ticketing_t", 
    THEATERLOCATION = "theater_location_t",
    THEATERSCREENS = "theater_screens_i",
    THEATERCOUNTRY = "theater_country_t",
    THEATERURL = "theater_url_t"
}
*/