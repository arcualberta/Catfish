/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("extended-image-block", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            var elName = e.target.name;
            if (elName === "title") {

                this.model.title.value = e.target.value;
                var content = this.model.title.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        title: content
                    });
                }
            }
            else if (elName === "description") {

                this.model.description.value = e.target.value;
                var content = this.model.description.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        description: content
                    });
                }
            } else if (elName == "linkText") {
                this.model.linkText.value = e.target.value;
                var content = this.model.linkText.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        linkText: content
                    });
                }
            } else if (elName == "linkUrl") {
                this.model.linkUrl.value = e.target.value;
                var content = this.model.linkUrl.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        linkUrl: content
                    });
                }
            } else if (elName == "ImageComesFirst") {
                this.model.linkUrl.value = e.target.value;
                var content = this.model.imageComesFirst.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        linkUrl: content
                    });
                }
            }
           
        },
        clear: function () {
            // clear media from block
        },
        select: function () {
            if (this.model.body.media != null) {
                piranha.mediapicker.open(this.update, "Image", this.model.body.media.folderId);
            } else {
                piranha.mediapicker.openCurrentFolder(this.update, "Image");
            }
        },
        remove: function () {
            this.model.body.id = null;
            this.model.body.media = null;
        },
        update: function (media) {
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
        selectAspect: function (val) {
            this.model.aspect.value = val;
        },
        isAspectSelected(val) {
            return this.model.aspect.value === val;
        }
    },
    computed: {
        isEmpty: function () {
            return this.model.body.media == null;
        },
        mediaUrl: function () {
            if (this.model.body.media != null) {
                return piranha.utils.formatUrl(this.model.body.media.publicUrl);
            } else {
                return piranha.utils.formatUrl("~/manager/assets/img/empty-image.png");
            }
        },
        iconUrl: function () {
            if (this.model.aspect.value > 0) {
                if (this.model.aspect.value === 1 || this.model.aspect.value === 3) {
                    return piranha.utils.formatUrl("~/manager/assets/img/icons/img-landscape.svg");
                } else if (this.model.aspect.value == 2) {
                    return piranha.utils.formatUrl("~/manager/assets/img/icons/img-portrait.svg");
                } else if (this.model.aspect.value == 4) {
                    return piranha.utils.formatUrl("~/manager/assets/img/icons/img-square.svg");
                }
            }
            return null;
        },
        titleValue: {
            get: function () {
                return this.model.title.value;
            }
        },
        descriptionValue: {
            get: function () {
                return this.model.description.value;
            }
        },
        linkTextValue: {
            get: function () {
                return this.model.linkText.value;
            }
        },
        linkUrlValue: {
            get: function () {
                return this.model.linkUrl.value;
            }
        },
        imageComesFirstValue: {
            get: function () {
                return this.model.imageComesFirst.value;
            }
        }
    },
    mounted: function () {
        this.model.getTitle = function () {
            if (this.model.media != null) {
                return this.model.media.filename;
            } else {
                return "No image selected";
            }
        };
    },
    template:` <div class="block-body has-media-picker rounded" :class="{ empty: isEmpty }">
        <div class="row">
        <img class="rounded col-md-4" :src="mediaUrl">
         <div class="form-group col-md-8">
                 <label class="form-label">Title: </label> {{ model.title.value }}<br/>
                 <label class="form-label">Description: </label> {{ model.description.value }}<br/>
                 <label class="form-label">Link Text: </label> {{ model.linkText.value }}<br/>
                 <label class="form-label">Link Url: </label> {{ model.linkUrl.value }}<br/>
                <label class="form-label">Image Comes First?: </label> {{ model.imageComesFirst.value }}
          </div>
        </div>
        <div class="media-picker" style="top:10%;">
            <div class="btn-group float-right">
                <button :id="uid + '-aspect'" class="btn btn-info btn-aspect text-center" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i v-if="model.aspect.value === 0" class="fas fa-cog"></i>
                    <img v-else :src="iconUrl">
                </button>
                <div class="dropdown-menu aspect-menu" :aria-labelledby="uid + '-aspect'">
                    <label class="mb-0">{{ piranha.resources.texts.aspectLabel }}</label>
                    <div class="dropdown-divider"></div>
                    <a v-on:click.prevent="selectAspect(0)" class="dropdown-item" :class="{ active: isAspectSelected(0) }" href="#">
                        <img :src="piranha.utils.formatUrl('~/manager/assets/img/icons/img-original.svg')"><span>{{ piranha.resources.texts.aspectOriginal }}</span>
                    </a>
                    <a v-on:click.prevent="selectAspect(1)" class="dropdown-item" :class="{ active: isAspectSelected(1) }" href="#">
                        <img :src="piranha.utils.formatUrl('~/manager/assets/img/icons/img-landscape.svg')"><span>{{ piranha.resources.texts.aspectLandscape }}</span>
                    </a>
                    <a v-on:click.prevent="selectAspect(2)" class="dropdown-item" :class="{ active: isAspectSelected(2) }" href="#">
                        <img :src="piranha.utils.formatUrl('~/manager/assets/img/icons/img-portrait.svg')"><span>{{ piranha.resources.texts.aspectPortrait }}</span>
                    </a>
                    <a v-on:click.prevent="selectAspect(3)" class="dropdown-item" :class="{ active: isAspectSelected(3) }" href="#">
                        <img :src="piranha.utils.formatUrl('~/manager/assets/img/icons/img-landscape.svg')"><span>{{ piranha.resources.texts.aspectWidescreen }}</span>
                    </a>
                    <a v-on:click.prevent="selectAspect(4)" class="dropdown-item" :class="{ active: isAspectSelected(4) }" href="#">
                        <img :src="piranha.utils.formatUrl('~/manager/assets/img/icons/img-square.svg')"><span>{{ piranha.resources.texts.aspectSquare }}</span>
                    </a>
                </div>
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
                <div class="card-body" v-else>
                    {{ model.body.media.filename }}
                  <div class='lead row'><label class='form-label col-md-3'>Title: </label><input class='form-control col-md-8' type='text' name='title' v-model='model.title.value' contenteditable='true' v-on:blur='onBlur' value='titleValue'  /></div>
                  <div class='lead row'><label class='form-label col-md-3'>Description: </label><textarea row='4' cols='100' class='form-control col-md-8'  name='description' v-model='model.description.value' contenteditable='true' v-on:blur='onBlur' value='descriptionValue'></textarea></div>
                  <div class='lead row'><label class='form-label col-md-3'>Link Text:</label> <input type='text' class='form-control col-md-8' name='linkText' v-model='model.linkText.value' contenteditable='true' v-on:blur='onBlur' value='linkTextValue' /></div>
                  <div class='lead row'><label class='form-label col-md-3'>Link Url:</label> <input type='text' class='form-control col-md-8' name='linkUrl' v-model='model.linkUrl.value' contenteditable='true' v-on:blur='onBlur' value='linkUrlValue'  /></div>
                  <div class='lead row'><label class='form-label col-md-3'>Image Comes First?:</label> <input type='checkbox' class='form-control col-md-8' name='imageComesFirst' v-model='model.imageComesFirst.value' contenteditable='true' v-on:blur='onBlur' value='imageComesFirstValue'  /></div>
                </div>
            </div>
        </div>
    </div>`
   
});
