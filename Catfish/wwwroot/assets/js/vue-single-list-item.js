/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("vue-single-list-item", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur(e) {
            //this.model.items.value = e.target.value;

        },

        isAspectSelected(val) {
            return false;//this.model.aspect.value === val;
        },

        selectAspect(val) {
            this.model.aspect.value = val;
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
    mounted() {
        console.log(this.model);

        tinymce.init({
            selector: 'textarea#' + this.uid + '-tiny-content-editor',   //'textarea#basic-example',
            height: 500,
            menubar: false,
            plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime media table paste code help wordcount'
            ],
            toolbar: 'undo redo | formatselect | ' +
                'bold italic backcolor | alignleft aligncenter ' +
                'alignright alignjustify | bullist numlist outdent indent | ' +
                'removeformat | help',
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
        });
    },
    computed: {
        isEmpty: function () {
            return ''; //piranha.utils.isEmptyText(this.model.items.value);
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
        <div>
            <div class="row">
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
                <div class='lead row'>
                    <label class='form-label col-md-3'>Title: </label>
                    <input class='form-control col-md-8' type='text' name='title' v-model='model.itemTitle.value' contenteditable='true' v-on:blur='onBlur' value='model.itemTitle.value'  />
                </div>
                <div class='lead row'>
                    <label class='form-label col-md-3'>Subtitle: </label>
                    <input type="text" class='form-control col-md-8'  name='description' v-model='model.itemSubtitle.value' contenteditable='true' v-on:blur='onBlur' value='model.itemSubtitle.value' />
                </div>
            </div>
</div>
            <div class='lead col'>
                <label class='form-label col-md-3'>Content:</label>
                <textarea rows='3' cols='100' :id="uid + '-tiny-content-editor'" class='form-control' name='linkText' v-model='model.itemContents.value' contenteditable='true' v-on:blur='onBlur' value='model.itemContents.value'></textarea>
            </div>
        </div>`
});