/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("card-block", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            var elName = e.target.name;

            //if (elName === "calendarId") {

            //    this.model.calendarId.value = e.target.value;
            //    var content = this.model.calendarId.value;
            //    if (content.length > 0) {
            //        this.$emit('update-content', {
            //            uid: this.uid,
            //            calendarId: content
            //        });
            //    }
            //} else if (elName == "pastDays") {
            //    this.model.daysRangePast.value = e.target.value;
            //    var content = this.model.daysRangePast.value;
            //    if (content.length > 0) {
            //        this.$emit('update-content', {
            //            uid: this.uid,
            //            daysRangePast: content
            //        });
            //    }
            //} else if (elName == "futureDays") {
            //    this.model.daysRangeFuture.value = e.target.value;
            //    var content = this.model.daysRangeFuture.value;
            //    if (content.length > 0) {
            //        this.$emit('update-content', {
            //            uid: this.uid,
            //            daysRangeFuture: content
            //        });
            //    }
            //}
            //else if (elName == "maxEvents") {
            //    this.model.maxEvents.value = e.target.value;
            //    var content = this.model.maxEvents.value;
            //    if (content.length > 0) {
            //        this.$emit('update-content', {
            //            uid: this.uid,
            //            maxEvents: content
            //        });
            //    }
            //}
            //else if (elName == "displayCalendarUI") {
            //    this.model.displayCalendarUI.value = e.target.value;
            //    var content = this.model.displayCalendarUI.value;
            //    if (content.length > 0) {
            //        this.$emit('update-content', {
            //            uid: this.uid,
            //            displayCalendarUI: content
            //        });
            //    }
            //}
            //else if (elName == "calendarStyle") {
            //    this.model.calendarStyle.value = e.target.value;
            //    var content = this.model.calendarStyle.value;
            //    if (content.length > 0) {
            //        this.$emit('update-content', {
            //            uid: this.uid,
            //            calendarStyle: content
            //        });
            //    }
            //}
        },

        /**
* Opens the Piranha-provided mediapicker
* */
        select() {
            if (this.model.body.media != null) {
                piranha.mediapicker.open(this.update, "Image", this.model.body.media.folderId);
            } else {
                piranha.mediapicker.openCurrentFolder(this.update, "Image");
            }
        },

        /**
     * Updates the image area with the chosen media
     * @param {any} media the chosen media
     */
        update(media) {
            if (media.type === "Image") {
                this.model.body.id = media.id;
                this.model.body.media = {
                    id: media.id,
                    folderId: media.folderId,
                    type: media.type,
                    filename: media.filename,
                    contentType: media.contentType,
                    publicUrl: media.publicUrl,
                };
                // Tell parent that title has been updated
                this.$emit('update-title', {
                    uid: this.uid,
                    title: this.model.body.media.filename
                });
            } else {
                console.log("No image was selected");
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
        isCardTitleEmpty: function () {
            return piranha.utils.isEmptyText(this.model.cardTitle.value);
        },

        isEmpty: function () {
            return this.model.body.media == null; //piranha.utils.isEmptyText(this.model.items.value);
        },

        /**
         * The url used to show an image/default image
         * */
        mediaUrl: function () {
            if (this.model.body.media != null) {
                return piranha.utils.formatUrl(this.model.body.media.publicUrl);
            } else {
                return piranha.utils.formatUrl("~/manager/assets/img/empty-image.png");
            }
        },
    },
    template: `
    <div class= 'block-body card-block'>

        <img class="rounded col-md-4" :src="mediaUrl">
        <div style="display: flex; flex-direction: column; flex: 1; justify-content: center;">
            <div class="lead row" style="top:10%;">
                <label class='form-label col-md-3'>Image: </label>
                <div class="btn-group float-right">
                    <button v-on:click.prevent="select" class="btn btn-primary text-center">
                        <i class="fas fa-plus"></i>
                    </button>
                    <button v-on:click.prevent="remove" class="btn btn-danger text-center">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
                <div class="card text-left">
                    <div class="card-body" v-if="isEmpty">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>

        <div class='lead row'><label class='form-label col-md-3 required'>Card Title: </label><input  class='form-control col-md-8' type='text' name='cardTitle' v-model='model.cardTitle.value' contenteditable='true' v-on:blur='onBlur' :class='{ requiredField: isCardTitleEmpty }' /></div>
        <div class='lead row' ><label class='form-label col-md-3'>Card Sub Title</label> <input type='text' class='form-control col-md-4' name='cardSubTitle' v-model='model.cardSubTitle.value' contenteditable='true' v-on:blur='onBlur' value='cardSubTitle' /></div>
        <input type="checkbox" id="has-modal-checkbox" v-model="model.hasAModal.value">
        <label for="has-modal-checkbox">Card has a popup when clicked?</label>
        <div class='lead row'><label class='form-label col-md-3'>Modal Size:</label>
            <select class='form-control col-md-4' name="modalSize" v-on:blur='onBlur' :value='model.modalSize.value' id="modal-size-select">
                <option value="0">Small</option>
                <option value="1">Large</option>
           </select>
        </div>
    </div>
    `
});