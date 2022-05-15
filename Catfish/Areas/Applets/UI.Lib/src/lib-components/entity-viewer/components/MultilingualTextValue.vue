<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'

    import { MultilingualTextField, TextCollection } from '../../shared/entity/';

    export default defineComponent({
        name: "MultilingualTextValue",
        props: {
            model: {
                required: false,
                type: null as PropType<MultilingualTextField> | null
            },
            lang: {
                required: false,
                type: null as PropType<string> | null,
                default: "en"
            },
        },
        setup(p) {

            const values = computed((): TextCollection[] | null => {
                if (p.lang === "*") {
                    return p.model?.values?.$values as TextCollection[] | null
                }
                else {
                    return p.model?.values?.$values.map(textCollection => {
                        const clone = JSON.parse(JSON.stringify(textCollection)) as TextCollection;

                        //removing all languages but the specified one by p.lang
                        clone.values.$values = clone.values.$values.filter(txt => txt.language === p.lang);
                        return clone;
                    }) as TextCollection[] | null;
                }
            })

            return {
                values,
            };
        },
    });
</script>

<template>
    <div v-for="value in values" :key="value.id" class="ml-value-set">
        <div class="ml-vlaue">
            <div v-for="txt in value.$values" :key="txt.id" :class="'lang lang-'+txt.language">
                {{txt.value}}
            </div>
        </div>
    </div>
</template>

