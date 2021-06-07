
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
            height: this.model.Height.Value !== null && this.model.Height.Value.length > 0 ? this.model.Height.Value : "400",
            timeValue: 0,
        }
    },
    mounted() {

        $('carousel').ready(() => { this.timeValue = 100;});

        $('.carousel').on('slide.bs.carousel', () => { 
            if (this.timeValue == 100) {
                this.timeValue = 0;
            } else {
                this.timeValue = 100;
            }
        });

        $('.carousel').on('slid.bs.carousel', () => {
            this.timeValue = 100;
        });
    },
    template:
        `<div class="vue-carousel row">
            <div :id="this.model.Id" class="carousel slide" data-ride="carousel" :style="{'height': height + 'px' }">
                <ol class="carousel-indicators">
                    <li v-for="(item,index) in this.model.Items" :data-target="elementId" :data-slide-to="index" :class="{'active': index === 0}"></li>
                </ol>
            <div class="carousel-inner">
                <div v-for="(item,index) in this.model.Items" class="carousel-item" :class="{'active': index === 0}">

                <div class="flex-carousel-contents" :class="{'image-comes-second': !item.ImageComesFirst.Value}">    
                  <div v-bind:style="{ 'background-image': 'url(' + item.Body.Media.PublicUrl.replace(/^~/, '') + ')', 'width': '100%' }" 
                  class="d-block image-in-carousel" alt="...">
                  </div>
                  <div v-if="item.Title.Value" class="text-container">
                    <h1 class="title-text">
                        {{item.Title.Value}}
                    </h1>
                    <h4 v-if="item.Description.Value" class="desc-in-carousel">
                        {{item.Description.Value}}
                    </h4>
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
            <div class="progress">
                <div class="progress-bar" :class="{'time-grow': timeValue == 100 }" role="progressbar" v-bind:style="{'width': timeValue + '%' }" :aria-valuenow="timeValue" aria-valuemin="0" aria-valuemax="100"></div>
            </div>
        </div>`
})