<script setup lang="ts">
    import { default as FieldExpression } from './components/FieldExpression.vue'
    import { default as ResultTable } from './components/ResultTable.vue'
    import { useSolrSearchStore } from './store';
    import { computed, ref, watch } from 'vue';
    import { buildQueryString } from './helpers';
    import { eUiMode, SearchFieldDefinition, SolrEntryType, ParameterSweepProperties } from './models';
    import { copyToClipboard } from './helpers'
    import { VueDraggableNext as draggable } from 'vue-draggable-next'

    const props = defineProps<{
        searchFields?: SearchFieldDefinition[],
        sweepableFieldNames: string[],
        resultFieldNames: string[],
        entryTypeFieldName?: string,
        entryTypeFieldOptions?: SolrEntryType[],
        queryApi?: string,
        uiMode?: eUiMode
    }>();

    const store = useSolrSearchStore();
    store.selectedEntryType = props.entryTypeFieldOptions?.find(entry => entry.label == "Showtimes") as SolrEntryType;

    if(props.searchFields){
        store.searchFieldDefinitions = props.searchFields;
    }

    if(props.queryApi){
        store.queryApi = props.queryApi;
    }

    watch(() => props.queryApi, async newQueryApi => {
        if(newQueryApi){
            store.queryApi = newQueryApi;
        }
    })

    watch(() => props.uiMode, async newUiMode => {
        if(newUiMode === eUiMode.Raw){
            store.selectedEntryTypeBackup = store.selectedEntryType;
            store.selectedEntryType = null;
        }
        else{
            store.selectedEntryType = store.selectedEntryTypeBackup;
        }
    }) 

    //watch(() => props.)

    const uiMode = computed(() => props.uiMode ? props.uiMode : eUiMode.Default)

    const expression = computed(() => store.fieldExpression)
    const querySource = computed(() => store.querySource)

    const queryString = computed(() => {
        const q = buildQueryString(store.fieldExpression)
        if(q){
            if(props.entryTypeFieldName && store.selectedEntryType){
                return `(${props.entryTypeFieldName}:${store.selectedEntryType.name}) AND (${q})`;
            }
            else{
                return q;
            }
        }
        else{
            if(props.entryTypeFieldName && store.selectedEntryType){
                return `${props.entryTypeFieldName}:${store.selectedEntryType.name}`;
            }
            else{
                return '*:*';
            }
        }
    })

    const sweepableFields = computed((): SearchFieldDefinition[] => store.activeFieldList.filter(field => props.sweepableFieldNames.includes(field.name)))

    const rawQuery = ref("*:*")

    const pageSize = ref(100);

    const selectedEntryType = computed(() => store.selectedEntryType ? store.selectedEntryType.label : "All Entry Tyoes")

    function query() {
        store.queryResult = null;

        if(uiMode.value === eUiMode.Default){
            store.query(queryString.value, 0, Number(pageSize.value))
            const resultEntryTypes = store.selectedEntryType ? store.selectedEntryType.label : "All Entry Types"
            store.querySource = `Filter Result (${resultEntryTypes})`
        }
        else if(uiMode.value === eUiMode.Raw){
            if(rawQuery.value && rawQuery.value.trim().length > 0){
                store.query(rawQuery.value, 0, Number(pageSize.value))
                store.querySource = "Solr Query Result"
            }
            else{
                alert("Please specify a query")
            }
        }
    }

    function executeJob() {
        store.queryResult = null;
        
        if (uiMode.value === eUiMode.Default) {
            store.executeJob(queryString.value, email.value, label.value, batchSize.value, selectUniqueEntries.value, roundFloats.value, numDecimalPoints.value, frequencyArrayFields.value, uniqueExportFields.value)
           // const resultEntryTypes = store.selectedEntryType ? store.selectedEntryType.label : "All Entry Types"
            //store.querySource = `Filter Result (${resultEntryTypes})`
        }
        else if (uiMode.value === eUiMode.Raw) {
            if (rawQuery.value && rawQuery.value.trim().length > 0) {
                store.executeJob(rawQuery.value, email.value, label.value, batchSize.value, selectUniqueEntries.value, roundFloats.value, numDecimalPoints.value, frequencyArrayFields.value, uniqueExportFields.value)
               // store.querySource = "Solr Query Result"
            }
            else {
                alert("Please specify a query")
            }
        }

       // alert("Search job submitted. When it ready, notoification will be send to your email: " + email.value)
    }

    const isResultColumnsVisible = ref(false);
    const isExportConfigVisible = ref(false);
    const isParamSweepVisible = ref(false);
    const email = ref("");
    const label = ref("");
    const batchSize = ref(50000)
    const selectUniqueEntries = ref(false)
    const roundFloats = ref(false)
    const numDecimalPoints = ref(4)
    const frequencyArrayFields = ref([] as string[])
    const uniqueExportFields = ref([] as string[])
    const enableParameterSweep = ref(false)
    const sweepParameters = ref({} as ParameterSweepProperties);

    const isBatchButtonDisabled = computed(() => label.value.trim().length == 0 || batchSize.value <= 0);

</script>
<template>
    <div class="mb-2">
        <span v-if="entryTypeFieldName">
            Entry Type:
            <select v-model="store.selectedEntryType">
                <option value="">ALL</option>
                <option v-for="val in entryTypeFieldOptions" :value="val" >{{val.label}}</option>
            </select>
            &nbsp;&nbsp;&nbsp;
        </span>
        Page Size:
        <select v-model="pageSize" class="page-size-dropdown">
            <option value="50">50</option>
            <option value="100" selected>100</option>
            <option value="250">250</option>
            <option value="500">500</option>
            <option value="1000">1000</option>
            <option value="2500">2500</option>
            <option value="5000">5000</option>
        </select>
    </div>
    <div v-if="uiMode === eUiMode.Default" class="query-wrapper">
        <FieldExpression :model="expression"></FieldExpression>
    </div>
    <div v-if="uiMode === eUiMode.Default" class="mt-3 query alert alert-success">
        <div>
            <b>Query String</b>
            <font-awesome-icon icon="fa-solid fa-copy"
                               class="fa-icon btn"
                               @click="copyToClipboard(queryString)"
                               v-b-tooltip.hover :title="'Copy the query string to clipboard.'" />
        </div>
        <div>{{ queryString }}</div>
    </div>
    <div v-if="uiMode === eUiMode.Raw">
        <textarea v-model="rawQuery" class="col-12"></textarea>
    </div>

    <div class="accordion pb-3" role="tablist">
        <b-card no-body class="mb-1">
            <b-card-header header-tag="header" class="p-0 card-header" role="tab">
                <b-button block data-bs-target="accordion-3" data-bs-toggle="visible" variant="success" @click="isResultColumnsVisible = !isResultColumnsVisible">
                    Result Columns
                    <font-awesome-icon icon="fa-chevron-down" class="fa-icon down-arrow" v-if="!isResultColumnsVisible" />
                    <font-awesome-icon icon="fa-chevron-up" class="fa-icon up-arrow" v-if="isResultColumnsVisible" />
                </b-button>
            </b-card-header>
            <b-collapse id="accordion-3" accordion="my-accordion" role="tabpanel" :class="!isResultColumnsVisible? '' : 'show'">
                <b-card-body>
                    <b-card-text>
                        <div class="row">
                            <div v-for="field in store.activeFieldList" :key="field.name" class="col-md-3 result-field-option">
                                <input type="checkbox" :value="field.name" v-model="store.resultFieldNames" /> {{field.label}}
                            </div>
                        </div>
                    </b-card-text>
                    <div>
                        <draggable class="dragArea list-group" :list="store.resultFieldNames">
                            <b-button v-for="fieldName in store.activeSelectedResultFieldNames" :key="fieldName" class="column-handle">{{ store.activeFieldList.find( fd => fd.name === fieldName)?.label }}</b-button>
                        </draggable>
                    </div>
                </b-card-body>
            </b-collapse>
        </b-card>
    </div>
   
    <div class="mt-12 mb-12 panel-search container row">
        <div class="mt-3 mb-3 panel-live-seacrh col-md-6">
            <h3>Live Search</h3>
            <button @click="query" class="btn btn-danger">Search</button>
        </div>
        <div class="mt-3 mb-3 panel-search-bg col-md-6">
            <h3>Background Search</h3>
            <!--  <div>Notification Email : <input type="text" v-model="email" placeholder="email address" /> </div> -->
            <div>Job Label : <input type="text" v-model="label" placeholder="label for the job" /></div>
            <div>Batch Size: <input type="number" v-model="batchSize" placeholder="Batch Size" /></div>
            
            <div class="accordion pb-3 mt-3" role="tablist">
                <b-card no-body class="mb-1">
                    <b-card-header header-tag="header" class="p-0 card-header" role="tab">
                        <b-button block data-bs-target="export-config-accordion" data-bs-toggle="visible" variant="success" @click="isExportConfigVisible = !isExportConfigVisible">
                            Export Configuration
                            <font-awesome-icon icon="fa-chevron-down" class="fa-icon down-arrow" v-if="!isExportConfigVisible" />
                            <font-awesome-icon icon="fa-chevron-up" class="fa-icon up-arrow" v-if="isExportConfigVisible" />
                        </b-button>
                    </b-card-header>
                    <b-collapse id="export-config-accordion" accordion="export-config-accordion" role="tabpanel" :class="!isExportConfigVisible? '' : 'show'">
                        <b-card-body>
                            <b-card-text>
                                <div class="row">
                                    <div><input type="checkbox" v-model="selectUniqueEntries" /> Select unique entries</div>
                                    <div v-if="selectUniqueEntries">
                                        <input type="checkbox" v-model="roundFloats" /> Round decimal values<span v-if="roundFloats"> to: <input type="number" v-model="numDecimalPoints" style="width: 60px" /> decimal places</span>
                                        <div v-if="store.resultArrayFields?.length > 0">
                                            <h5 class="mt-2">Select arrays where number of elements counted towards frequency.</h5>
                                            <div v-for="field in store.resultArrayFields" :id="field" style="margin-left:15px">
                                                <input  type="checkbox" :value="field" v-model="frequencyArrayFields" name="field"> {{ searchFields?.find(sf => sf.name == field)?.label }}
                                            </div>
                                        </div>
                                        <div v-if="store.resultFieldNames.length > 0">
                                            <h5 class="mt-2">Limit export fields to (select none to export all result fields):</h5>
                                            <div class="alert alert-info">
                                                Note: unique entries are identified based on the combination of values in exported fields.
                                            </div>
                                            <div v-if="store.resultFieldNames.length > 0">
                                                <div v-for="field in store.resultFieldNames" :id="field" style="margin-left:15px">
                                                    <input  type="checkbox" :value="field" v-model="uniqueExportFields" name="field" > {{ searchFields?.find(sf => sf.name == field)?.label }}
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </b-card-text>
                        </b-card-body>
                    </b-collapse>
                </b-card>
            </div>            
            
            <!--
            <div class="mt-3 mb-3">
                <h4>Export Configuration</h4>
                <div><input type="checkbox" v-model="selectUniqueEntries" /> Select unique entries</div>
                <div v-if="selectUniqueEntries">
                    <input type="checkbox" v-model="roundFloats" />Round floats <span v-if="roundFloats">to: <input type="number" v-model="numDecimalPoints" style="width: 60px" /> decimal places</span>
                    <div v-if="store.resultArrayFields?.length > 0">
                        <h5 class="mt-2">Select arrays where number of elements counted towards frequency.</h5>
                        <div v-for="field in store.resultArrayFields" :id="field" style="margin-left:15px">
                            <input  type="checkbox" :value="field" v-model="frequencyArrayFields" name="field"> {{ searchFields?.find(sf => sf.name == field)?.label }}
                        </div>
                    </div>
                    <div v-if="store.resultFieldNames.length > 0">
                        <h5 class="mt-2">Limit export fields to (select none to export all result fields):</h5>
                        <div class="alert alert-info">
                            Note: unique entries are identified based on the combination of values in ALL exported fields.
                        </div>
                        <div v-if="store.resultFieldNames.length > 0">
                            <div v-for="field in store.resultFieldNames" :id="field" style="margin-left:15px">
                                <input  type="checkbox" :value="field" v-model="uniqueExportFields" name="field" > {{ searchFields?.find(sf => sf.name == field)?.label }}
                            </div>
                        </div>
                    </div>
                </div>
            </div> -->

            <div class="accordion pb-3" role="tablist">
                <b-card no-body class="mb-1">
                    <b-card-header header-tag="header" class="p-0 card-header" role="tab">
                        <b-button block data-bs-target="param-sweep-accordion" data-bs-toggle="visible" variant="success" @click="isParamSweepVisible = !isParamSweepVisible">
                            Parameter Sweep
                            <font-awesome-icon icon="fa-chevron-down" class="fa-icon down-arrow" v-if="!isParamSweepVisible" />
                            <font-awesome-icon icon="fa-chevron-up" class="fa-icon up-arrow" v-if="isParamSweepVisible" />
                        </b-button>
                    </b-card-header>
                    <b-collapse id="param-sweep-accordion" accordion="param-sweep-accordion" role="tabpanel" :class="!isParamSweepVisible? '' : 'show'">
                        <b-card-body>
                            <b-card-text>
                                <input type="checkbox" v-model="enableParameterSweep" /> Enable
                                <div class="col-mb-2" v-if="enableParameterSweep">
                                    <div class="form-group row">
                                        <label for="sweepField" class="col-md-3">Sweep on</label>
                                        <select v-model="store.sweepField"  class="form-control" style="width:250px;" id="sweepField">
                                            <option v-for="val in sweepableFields" :value="val" >{{val.label}}</option>
                                        </select>
                                    </div>
                                    <div v-if="store.sweepField?.name.endsWith('_dt')">
                                        <div class="form-group mt-3" >
                                            <label for="sweepFrom" class="col-md-3">From</label>
                                            <input type="date" v-bind="sweepParameters.startDate" name="sweepFrom"/>
                                        </div>
                                        <div class="form-group mt-3" >
                                            <label for="sweepTo" class="col-md-3">To</label>
                                            <input type="date" v-bind="sweepParameters.endDate" name="sweepTo"/>
                                        </div>
                                        <div class="form-group mt-3" >
                                            <label for="sweepStep" class="col-md-3">Step</label>
                                            <input type="number" v-bind="sweepParameters.step" style="100px;" name="sweepStep"/>
                                            in 
                                            <select v-model="sweepParameters.stepType">
                                                <option v-for="val in ['days', 'weeks', 'months', 'years']" :value="val" >{{val}}</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div v-if="store.sweepField?.name.endsWith('_s')">
                                        <div class="form-group mt-3" >
                                            <label for="sweepValues" class="col-12">Sweep over values (specify separated by commas)</label>
                                            <input type="text" v-bind="sweepParameters.values" class="col-12" name="sweepValues"/>
                                        </div>                                        
                                    </div>
                               </div>
                            </b-card-text>
                        </b-card-body>
                    </b-collapse>
                </b-card>
            </div>
            <button @click="executeJob" class="btn btn-success" :disabled='isBatchButtonDisabled'>Submit Search Job</button>
        </div>
    </div>
    <div v-if="store.isLoadig" class="mt-2">
        <b-spinner variant="primary" label="Spinning"></b-spinner>
    </div>
    <div v-if="store.isLoadingFailed" class="alert alert-danger">
        Data loading failed!
    </div>
    <div class="mt-3 mb-3" v-if="store.queryResult">
        <div class="mt-3">
            <h4>{{ querySource }}</h4>
            Query time: {{store.queryTime}} seconds<br /><br />
            <ResultTable :model="store.queryResult" />
        </div>
    </div>
</template>

<style scoped>
.query-wrapper{
    margin-left: -15px;
}

.fa-icon.btn{
    width:15px;
    height:15px;
    padding: 3px;
}
.copy-link:hover {
    cursor: pointer;
}
.accordion .card {
    padding-bottom: 10px;;
}
.card-header{
    margin-top:-30px;
    margin-left:-30px;
    background-color: transparent !important;
    border:none
}
.btn[aria-expanded="false"] .up-arrow{
    display: none;
}
.btn[aria-expanded="true"] .down-arrow{
    display: none;
}

.column-handle {
  border: solid 1px;
  margin-right: 5px;
}

.dragArea.list-group {
    overflow-x: scroll;
    overflow: auto;
    flex-direction: row;
}

</style>