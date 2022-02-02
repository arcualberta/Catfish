<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import dayjs from "dayjs";

    import { MonolingualTextField } from '../../models/fieldContainer'


    export default defineComponent({
        name: "DateField",
        props: {
            model: {
                type: null as PropType<MonolingualTextField> | null,
                required: true
           },
            isMultivalued: 
            {
                type: Boolean,
                required: false,
                default: false
            }

        },
        methods:{
            formatDate(dateString: string) {
                const date = dayjs(dateString);
                return date.format('MMM DD, YYYY');
            }
        }
       
    });
</script>

<template>
    <div v-for="val in model.values.$values">{{formatDate(val.value)}}</div>
</template>

