/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("news-feed-block-vue", {
    props: ["uid", "toolbar", "model"],
    //data: function() {
    //    return {
    //    //    selectedImg: null
    //    }
    //},
    methods: {
        onBlur: function (e) {
            var elName = e.target.name;

            if (elName == "newsSource") {
                this.model.newsSource.value = e.target.value;
                var content = this.model.newsSource.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        newsSource: content
                    });
                }
            }
            else if (elName == "embedOption") {
                this.model.embedOption.value = e.target.value;
                var content = this.model.embedOption.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        embedOption: content
                    });
                }
            }
        },


    },
    computed: {
        //cidValue: {
        //    get: function () {
        //        return this.model.calendarId.value;
        //    }
        //},
        //pastDaysValue: {
        //    get: function () {
        //        return this.model.daysRangePast.value;
        //    }
        //},
        //futureDaysValue: {
        //    get: function () {
        //        return this.model.daysRangeFuture.value;
        //    }
        //},
        //maxEventsValue: {
        //    get: function () {
        //        return this.model.maxEvents.value;
        //    }
        //},
        //displayCalendarUIValue: {
        //    get: function () {
        //        return this.model.displayCalendarUI.value;
        //    }
        //},
        //calendarStyleValue: {
        //    get: function () {
        //        return this.model.calendarStyle.value;
        //    }
        //},
        //isCardTitleEmpty: function () {
        //    return piranha.utils.isEmptyText(this.model.cardTitle.value);
        //},

        //isCardImageEmpty: function () {
        //    return this.model.cardImage.media == null; //piranha.utils.isEmptyText(this.model.items.value);
        //},

        //isModalImageEmpty: function () {
        //    return this.model.modalImage.media == null; //piranha.utils.isEmptyText(this.model.items.value);
        //}

    },
    template: `
    <div class= 'block-body news-feed-block'>
        <h4>News Feed</h4>
        <p>This block allow you to embed Twitter or FB timeline.</p>
       

        <div class='lead row'>
            <label class='form-label col-md-3'>Block Title: </label>
            <input  class='form-control col-md-8' type='text' name='blockTitle' v-model='model.blockTitle.value' contenteditable='true' v-on:blur='onBlur'  />
        </div>
        <div class='lead row' >
            <label class='form-label col-md-3' for="is-display-title-checkbox">Is Display Block Title?</label>
            <input type="checkbox" name ="isDisplayBlockTitle" id="is-display-title-checkbox" v-model="model.isDisplayBlockTitle.value">
        </div>

        <h4>Embed Option</h4>
        
        <div class='lead row'><label class='form-label col-md-3'>Option:</label>
            <select class='form-control col-md-4' name="embedOption" v-on:blur='onBlur' :value='model.embedOption.value' id="embed-option-select">
                <option value="0">Embed</option>
               <!-- <option value="1">Shared Button</option> -->
           </select>
        </div>
       
      <div class='lead row'><label class='form-label col-md-3'>Source:</label>
            <select class='form-control col-md-4' name="newsSource" v-on:blur='onBlur' :value='model.newsSource.value' id="news-source-select">
                <option value="0">Twitter</option>
                <option value="1">Facebook</option>
           </select>
        </div>

       <div class='lead row'>
            <label class='form-label col-md-3 required'>Source Url: </label>
            <input  class='form-control col-md-8' type='text' name='referenceUrl' v-model='model.referenceUrl.value' contenteditable='true' v-on:blur='onBlur'  />
        </div>

    </div>
    `
});