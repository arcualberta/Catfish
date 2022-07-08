Vue.component('news-feed-block-vue', {
    props: ["uid", "model"],

    data: function () {
        return {
            //modalSizeSmall: null,
            //modalSizeLarge: null,
            //isModalCenteredOnTheScreen: null
        }
    },

    mounted: function () {
        //use nextTick => to wait until the entire view has been rendered
        this.$nextTick(function () {
            
                if (document.getElementById('twitterScript')) { return }

                let src = 'https://platform.twitter.com/widgets.js';
                let script = document.createElement('script');
                script.setAttribute('src', src);
                script.setAttribute('type', 'text/javascript');
                script.setAttribute('id', 'twitterScript');
                document.head.appendChild(script);
        })
    }
   
})