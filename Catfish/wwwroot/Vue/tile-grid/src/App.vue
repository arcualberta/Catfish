<template>
    <CategoryFilter msg="Please select the following options to filter data."></CategoryFilter>
</template>

<script lang="ts">
    import { Options, Vue } from 'vue-class-component';
    import CategoryFilter from './components/CategoryFilter.vue';

    @Options({
        components: {
            CategoryFilter
        },
    })
    export default class App extends Vue { }
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 0px;
  border: solid 1px green;
}
</style>
