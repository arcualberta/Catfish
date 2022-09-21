<script setup lang="ts">
    import { Form } from '../../shared/form-models';

    const props = defineProps<{ model: Form }>();

</script>

<template>
    <div class="col-sm-10">
        <br />
        <div class="toggle-button-cover">
            <div class="button-cover">
                <div class="button r" id="button-1">
                    <input v-model="model.isMultiValued" type="checkbox" class="checkbox" />
                    <div class="knobs"></div>
                    <div class="layer"></div>
                </div>
            </div>
        </div>

    </div>
</template>