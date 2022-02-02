<script lang="ts">
    
    import { defineComponent, PropType/*, ref*/} from 'vue'
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
        methods: {
         
             handleFileUpload(e: any) {
                //const target = <HTMLInputElement>e.target;

                //if (target !== null) {
                //  //  const url = URL.createObjectURL(target.files && target.files[0]);
                //    var file = target.files && target;//.files[0];
                //    console.log("selected file: " + JSON.stringify(file));
                //}
                console.log(e.target.files[0]);
            }
        },
        setup() {
            //const file = ref(null); //ref(p.model.files);

           
              
                //Upload to server


                return {
                    //handleFileUpload,
                    //file
                }
            }
        
           
    });
</script>

<template>
    <!--<div>Attachment Field</div>
    <div>{{JSON.stringify(model)}}</div>-->

   
    <!--<div v-for="f in this.model.files" >-->
    <input type="file" @change="handleFileUpload($event)" />

  
    <!--</div>-->
</template>

