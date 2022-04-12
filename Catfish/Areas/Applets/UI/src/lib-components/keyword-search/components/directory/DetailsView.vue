<script lang="ts">
    import { Guid } from 'guid-typescript'
    import { defineComponent, computed, onMounted } from "vue";
    import { useStore } from 'vuex';

    import props, { QueryParameter } from '../../../shared/props'

    import { Mutations as ItemViewerMutations } from '../../../item-viewer/store/mutations';
    import { Actions as ItemViewerActions } from '../../../item-viewer/store/actions';
    import { /*state,*/ State } from '../../store/state';

    export default defineComponent({
        name: "DetailsView",
        props,
        setup(p) {
            const store = useStore();

            const queryParameters = p.queryParameters as QueryParameter;
            if (queryParameters && (queryParameters["iid"] as string)?.length > 0)
                store.commit(ItemViewerMutations.SET_ID, queryParameters["iid"] as unknown as Guid);

            const itemId = computed(() => (store.state as State).id);
           // var items = [];
            const items= computed(() => store.state.searchResult?.items);
           
            onMounted(() => {
                if (itemId)
                    store.dispatch(ItemViewerActions.LOAD_ITEM, itemId);
            })

            return {
                itemId,
                items,
                item: computed(() => store.getters.getItem(itemId))
                //item: computed(() => (store.state as State).item),
            }
        },
    });
</script>

<template>
    <h2>Details View</h2>
    <div class="row">
        <div v-for="item in items" :key="item.id" >
            <div v-if="item.id === itemId" class=" row ">
                <div class="col-md-6 grey-BG">
                    <div class="row ">

                        <!--<img src="#" />-->
                        <i class="fas fa-image profileHeadshot"></i>

                        <div class="profileInfo">
                            <h5 class="item-title">
                                <a href="#" @click="viewDetails(item.id)">{{item.subtitle}}</a>
                            </h5>
                        </div>

                    </div>
                    <div class="content profileDetails">{{item.content}}</div>

                    <button class="contactMeBtn btn btn-link">Contact Me!</button>
                </div>
                <!--  related Researchers tabs style -->
                <div class="col-md-5">
                    <div class="explore-related">
                        <div class="related-title">Explore related researchers</div>

                        <div class="related-scroll">

                         
                            <div v-for="item in items" :key="item.id" class="related">
                                <i class="fas fa-image related-image"></i>
                                <div class="related-results">
                                    {{item.subtitle}}
                                </div>
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
       
    </div>
    <!--Item ID: {{JSON.stringify(itemId)}}
    <br />
    <br />
    {{JSON.stringify(items)}}-->
</template>

<style scoped>
    .fa-image{
        font-size:500%;
    }
    .explore-related {
        display: inline-block;
        position: relative;
        margin: 20px;
        vertical-align: top;
    }
    .related-title {
        background-color: #ececec;
        font-weight: 700;
        text-decoration: underline;
        position: relative;
        width: 230px;
        padding: 15px;
        margin-left: 30px;
        border-radius: 20px 20px 0px 0px;
        font-size: large;
    }
    .related-scroll {
        background-color: #ececec;
        border-radius: 30px;
        padding: 20px;
        width: 450px;
        height: 150px;
        overflow-x: scroll;
        overflow-y: hidden;
        white-space: nowrap;
        display: inline-block;
    }
    .related {
        display: inline-block;
    }
    .related-image {
        position: relative;
        display: inline-block;
        float: left;
        width: 100px;
    }
    .related-results {
        display: inline-block;
        position: relative;
        margin: 20px;
        margin-top: 10px;
    }
    .contactMeBtn{
        float: right;
        background: White;
        border-radius: 25px;
    }
    
    .grey-BG {
        background-color: #ececec;
        border-radius: 30px;
        height: 500px;
        position: relative;
        width: 600px;
        width: 700px;
        margin-left: 20px;
        margin-top: 10px;
        margin-bottom: 20px;
        display: inline-block;
        overflow-y: hidden;
    }

    .profileInfo {
        margin-left: 10px;
        display: inline-block;
        margin-top: 22px;
        max-width: 275px;
        font-size: large;
    }

    .profileHeadshot {
        font-size: 500%;
        margin: 10px;
    }
</style>