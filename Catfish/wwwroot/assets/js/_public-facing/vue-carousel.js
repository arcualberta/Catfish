
﻿/**
 This is the public-facing carousel component.
 It is a slideshow-style set of images with text and a link (all optionally provided based on the manager side's input).
 */

Vue.component('vue-carousel', {
    props: ["model"],

    data: function () {
        return {
            elementId: "#" + this.model.Id,
            width: this.model.Width.Value !== null && this.model.Width.Value.length > 0 ? this.model.Width.Value : "1200",
            height: this.model.Height.Value !== null && this.model.Height.Value.length > 0 ? this.model.Height.Value : "400"
        }
    },
    mounted() {
        console.log(this.model);

        $('.carousel').carousel({
            interval: 20000000
        })
    },
    template:
        `<div>
            <div :id="this.model.Id" class="carousel slide" data-ride="carousel" v-bind:style="{'height': height + 'px'}">
                <ol class="carousel-indicators">
                    <li v-for="(item,index) in this.model.Items" :data-target="elementId" :data-slide-to="index" :class="{'active': index === 0}"></li>
                </ol>
            <div class="carousel-inner">
                <div v-for="(item,index) in this.model.Items" class="carousel-item" :class="{'active': index === 0}">

                <div class="flex-carousel-contents" :class="{'image-comes-second': !item.ImageComesFirst.Value}">    
                  <div v-bind:style="{ 'background-image': 'url(' + item.Body.Media.PublicUrl.replace(/^~/, '') + ')', 'width': width + 'px' }" 
                  class="d-block image-in-carousel" alt="...">
                  </div>
                  <div class="text-container">
                    <h2 class="title-text">
                        {{item.Title.Value}}
                    </h2>
                    <h5>
                        {{item.Description.Value}}
                    </h5>
                    <a v-if="item.LinkText.Value" role="button" class="btn btn-primary" :href="item.LinkUrl.Value">
                        {{item.LinkText.Value}}
                    </a>
                  </div>
                   </div>


                </div>
              </div>
              <a class="carousel-control-prev" :href="elementId" role="button" data-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                <span class="sr-only">Previous</span>
              </a>
              <a class="carousel-control-next" :href="elementId" role="button" data-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                <span class="sr-only">Next</span>
              </a>
            </div>
        </div>`
})