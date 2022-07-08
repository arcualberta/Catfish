/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("card-block-vue", {
    props: ["uid", "toolbar", "model"],
    data: function() {
        return {
            selectedImg: null
        }
    },
    methods: {
        onBlur: function (e) {
            var elName = e.target.name;

            if (elName == "modalSize") {
                this.model.modalSize.value = e.target.value;
                var content = this.model.modalSize.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        modalSize: content
                    });
                }
            }
            else if (elName == "imagePosition") {
                this.model.imagePosition.value = e.target.value;
                var content = this.model.imagePosition.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        imagePosition: content
                    });
                }
            }
        },

        /**
         * Clears the image selection 
         **/
        remove(selected) {
            if (selected == 'card') {
                this.model.cardImage.id = null;
                this.model.cardImage.media = null;
            } else if (selected == 'modal') {
                this.model.modalImage.id = null;
                this.model.modalImage.media = null;
            }
        },

        /**
        * Opens the Piranha-provided mediapicker
        * */
        select(selected) {
            this.selectedImg = selected;
            if (selected == 'card') {
                if (this.model.cardImage.media != null) {
                    piranha.mediapicker.open(this.update, "Image", this.model.cardImage.media.folderId);
                } else {
                    piranha.mediapicker.openCurrentFolder(this.update, "Image");
                }
            } else if (selected == 'modal') {
                if (this.model.modalImage.media != null) {
                    piranha.mediapicker.open(this.update, "Image", this.model.modalImage.media.folderId);
                } else {
                    piranha.mediapicker.openCurrentFolder(this.update, "Image");
                }
            }

            
        },

        /**
     * Updates the image area with the chosen media
     * @param {any} media the chosen media
     */
        update(media) {
            if (media.type === "Image") {
                //card image or modal image
                if (this.selectedImg == 'card') {
                    this.model.cardImage.id = media.id;
                    this.model.cardImage.media = {
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
                        title: this.model.cardImage.media.filename
                    });
                } else if (this.selectedImg == 'modal') {
                    this.model.modalImage.id = media.id;
                    this.model.modalImage.media = {
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
                        title: this.model.modalImage.media.filename
                    });
                }
                this.selectedImg = null;
                
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

        isCardImageEmpty: function () {
            return this.model.cardImage.media == null; //piranha.utils.isEmptyText(this.model.items.value);
        },

        isModalImageEmpty: function () {
            return this.model.modalImage.media == null; //piranha.utils.isEmptyText(this.model.items.value);
        },

        /**
         * The url used to show an image/default image for the card
         * */
        cardImageMediaUrl: function () {
            if (this.model.cardImage.media != null) {
                return piranha.utils.formatUrl(this.model.cardImage.media.publicUrl);
            } else {
                return piranha.utils.formatUrl("~/manager/assets/img/empty-image.png");
            }
        },

        /**
        * The url used to show an image/default image for the pop-up/modal
        * */
        modalImageMediaUrl: function () {
            if (this.model.modalImage.media != null) {
                return piranha.utils.formatUrl(this.model.modalImage.media.publicUrl);
            } else {
                return piranha.utils.formatUrl("~/manager/assets/img/empty-image.png");
            }
        }
    },
    template: `
    <div class= 'block-body card-block'>
        <h4>Card Contents</h4>
        <p>The card is the base content that viewers of the website can click to open a pop-up.</p>
        <div style="display: flex; flex-direction: column; flex: 1; justify-content: center;">
            <div class="lead row" style="top:10%;">
                <label class='form-label col-md-3'>Image: </label>
                <img class="rounded col-md-4" :src="cardImageMediaUrl">
                <div class="btn-group float-right" style="align-items: baseline;">
                    <button v-on:click.prevent="select('card')" class="btn btn-primary text-center">
                        <i class="fas fa-plus"></i>
                    </button>
                    <button v-on:click.prevent="remove('card')" class="btn btn-danger text-center">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
                <div class="card text-left">
                    <div class="card-body" v-if="isCardImageEmpty">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>

        <div class='lead row'>
            <label class='form-label col-md-3 required'>Card Title: </label>
            <input  class='form-control col-md-8' type='text' name='cardTitle' v-model='model.cardTitle.value' contenteditable='true' v-on:blur='onBlur' :class='{ requiredField: isCardTitleEmpty }' />
        </div>
        <div class='lead row'>
            <label class='form-label col-md-3'>Card Sub Title</label> 
            <input type='text' class='form-control col-md-4' name='cardSubTitle' v-model='model.cardSubTitle.value' contenteditable='true' v-on:blur='onBlur' value='cardSubTitle' />
        </div>
        <div class='lead row' >
            <label class='form-label col-md-3' for="has-modal-checkbox">Card has a popup when clicked?</label>        
            <input type="checkbox" id="has-modal-checkbox" v-model="model.hasAModal.value">
        </div>

        <hr>
        <h4>Pop-up Contents</h4>
        <p>The pop-up, if enabled in the above options, will appear when viewers of the website click the card.</p>
        <div class='lead row'><label class='form-label col-md-3'>Modal Size:</label>
            <select class='form-control col-md-4' name="modalSize" v-on:blur='onBlur' :value='model.modalSize.value' id="modal-size-select">
                <option value="0">Small</option>
                <option value="1">Large</option>
           </select>
        </div>
        <div class='lead row'>
            <label class='form-label col-md-3' for="center-modal-checkbox">Is the popup centered on the screen?</label>        
            <input type="checkbox" id="center-modal-checkbox" v-model="model.isModalCenteredOnTheScreen.value">
        </div>

        <div style="display: flex; flex-direction: column; flex: 1; justify-content: center;">
            <div class="lead row" style="top:10%;">
                <label class='form-label col-md-3'>Image: </label>
                <img class="rounded col-md-4" :src="modalImageMediaUrl">
                <div class="btn-group float-right" style="align-items: baseline;">
                    <button v-on:click.prevent="select('modal')" class="btn btn-primary text-center">
                        <i class="fas fa-plus"></i>
                    </button>
                    <button v-on:click.prevent="remove('modal')" class="btn btn-danger text-center">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
                <div class="card text-left">
                    <div class="card-body" v-if="isModalImageEmpty">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>

       <div class='lead row'><label class='form-label col-md-3'>Image Positioning:</label>
           <select class='form-control col-md-4' name="imagePosition" v-on:blur='onBlur' :value='model.imagePosition.value' id="image-position-select">
               <option value="0">Left</option>
               <option value="1">Right</option>
               <option value="2">Top</option>
               <option value="3">Bottom</option>
          </select>
       </div>

        <div class='lead row'>
            <label class='form-label col-md-3'>Popup Title: </label>
            <input class='form-control col-md-8' type='text' name='popupTitle' v-model='model.modalTitle.value' contenteditable='true' v-on:blur='onBlur' />
        </div>
        <div class='lead row'>
            <label class='form-label col-md-3'>Popup Sub Title: </label>
            <input class='form-control col-md-8' type='text' name='popupSubTitle' v-model='model.modalSubTitle.value' contenteditable='true' v-on:blur='onBlur' />
        </div>
        <div class='lead row'>
            <label class='form-label col-md-3'>Popup Description: </label>
            <textarea value='model.modalDescription.value' v-model='model.modalDescription.value' class="form-control col-md-8" rows="6"></textarea>
        </div>
        <div class='lead row'>
            <label class='form-label col-md-3'>Email Address: </label>
            <input class='form-control col-md-8' type='text' name='popupEmail' v-model='model.emailAddress.value' contenteditable='true' v-on:blur='onBlur' />
        </div>
        <br>
        <h5>
            An optional  button can be added to the pop-up that links to another site. 
            <br>You can add the URL to navigate to, as well as the text that will be displayed in the button.
        </h5>
        <br>
        <div class='lead row'>
            <label class='form-label col-md-3'>Button Link Url: </label>
            <input class='form-control col-md-8' type='text' name='buttonLinkUrl' v-model='model.buttonLink.value' contenteditable='true' v-on:blur='onBlur' />
        </div>
        <div class='lead row'>
            <label class='form-label col-md-3'>Button Text: </label>
            <input class='form-control col-md-8' type='text' name='buttonText' v-model='model.buttonText.value' contenteditable='true' v-on:blur='onBlur' />
        </div>
        <div class='lead row'>
            <label class='form-label col-md-3'>Button Color: </label>
            <input type="color" v-model="model.buttonColor.value">
        </div>
        <br>
        <div class='lead row'>
            <label class='form-label col-md-3' for="prevent-outside-clicks-checkbox">Prevent popup from closing when clicking outside of it?</label>        
            <input type="checkbox" id="prevent-outside-clicks-checkbox" v-model="model.preventUserFromExitingOnOutsideClick.value">
        </div>
    </div>
    `
});