<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
	import { useStore } from 'vuex';
	import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "Text",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
            },
            isRequired:
            {
                type: Boolean,
                required: false,
                default: false
            }
        },
        setup(p) {

            const store = useStore();
            const model = p.model;

			console.log(store.state.form?.id);
            console.log(model.id);

            return {
				content: computed(() => store.state.itemTemplateId.toString())
			}
            //return {
            //    childForm: computed(() => {
            //        return {
            //            get(): string {
            //                return model.value;
            //            },
            //            set(value: string): void {
            //                store.commit('SET_TEXT_VALUE', { id: model.id, val: value });
            //            }
            //        }
            //    }
            //};
        },
    });
</script>

<template>
    <input v-model="content" required="{isRequired ? 'required' : ''}" class="form-control" />
</template>

