/*
    Manager side for the carousel - basically the UI for the user to add the images/info.
*/
Vue.component("vue-carousel", {
    props: ["uid", "model"],

    methods: {
        onBlur: function (e) {
            this.model.slides.value = e.target.value;
        },
        
    },
    computed: {
        isEmpty: function () {
            //return piranha.utils.isEmptyText(this.model.items.value);
        },
        mediaUrl: function () {
            /*if (this.model.body.media != null) {
                return piranha.utils.formatUrl(this.model.body.media.publicUrl);
            } else {
                return piranha.utils.formatUrl("~/manager/assets/img/empty-image.png");
            }*/
        }

    },
    mounted() {
        console.log(this.model);
    },
    template: `
        <div>
            <div class="block-body has-media-picker rounded" :class="{ empty: isEmpty }">
                <div class="row">
                <img class="rounded col-md-4" :src="mediaUrl">
                    <div class="form-group col-md-8">
                            <label class="form-label">Title: </label><br/>
                            <label class="form-label">Description: </label><br/>
                            <label class="form-label">Link Text: </label><br/>
                            <label class="form-label">Link Url: </label>
                    </div>
                </div>
            </div>
            
            

            <textarea rows='4' cols='100' class='lead '
                v-html='' contenteditable='true' v-on:blur='onBlur' >
            </textarea>
        </div>
`
    /*
     
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
                  <div class='lead row' ><label class='form-label col-md-3'>Link Text:</label> <input type='text' class='form-control col-md-8' name='linkText' v-model='model.linkText.value' contenteditable='true' v-on:blur='onBlur' value='linkTextValue' /></div>
                   <div class='lead row'><label class='form-label col-md-3'>Link Url:</label> <input type='text' class='form-control col-md-8' name='linkUrl' v-model='model.linkUrl.value' contenteditable='true' v-on:blur='onBlur' value='linkUrlValue'  /></div>
                </div>
            </div>
        </div>
     
     */
});