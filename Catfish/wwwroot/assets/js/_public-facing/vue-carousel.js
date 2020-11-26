Vue.component('vue-carousel', {
    props: ["uid", "model"],

    data: function () {
        return {
            elementId: "#" + this.model.Id,
            width: this.model.Width.Value !== null && this.model.Width.Value.length > 0 ? this.model.Width.Value : "1200",
            hieght: this.model.Height.Value !== null && this.model.Height.Value.length > 0 ? this.model.Height.Value : "400"
        }
    },
    template:
        `<div>
            <div :id="this.model.Id" class="carousel slide" data-ride="carousel">
              <ol class="carousel-indicators">
                <li v-for="(item,index) in this.model.Items" :data-target="elementId" :data-slide-to="index" :class="{'active': index === 0}"></li>
              </ol>
              <div class="carousel-inner">
                <div v-for="(item,index) in this.model.Items" class="carousel-item" :class="{'active': index === 0}">
                  <img :width="width" :height="hieght" :src="item.Body.Media.PublicUrl.replace(/^~/, '')" class="d-block w-100" alt="...">
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
