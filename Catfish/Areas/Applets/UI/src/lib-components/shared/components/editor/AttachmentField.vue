<script lang="ts">
    
    import { defineComponent, PropType, ref} from 'vue'
    //import { useStore } from 'vuex';

    import { AttachmentField} from '../../models/fieldContainer'

    export default defineComponent({
        name: "AttachmentField",
        components: {
           
        },
        props: {
            model: {
                type: null as PropType<AttachmentField> | null,
                required: true
            },


        },
        setup() {
            const file = ref(null); //ref(p.model.files);

            const handleFileUpload = () => {
                // debugger;
                console.log("selected file " + JSON.stringify(file));
                //Upload to server


                return {
                    handleFileUpload,
                    file
                }
            }
        }
           
    });
</script>

<template>
    <div>Attachment Field</div>
    <div>{{JSON.stringify(model)}}</div>

   
    <!--<div v-for="f in this.model.files" >-->
    <!--<input type="file" ref="file"  @change="handleFileUpload()" />-->

  
    <!--</div>-->
</template>

