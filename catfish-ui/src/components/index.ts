import GoogleCalendar from '../views/GoogleCalendar.vue';
import HelloWorldVue from './HelloWorld/HelloWorld.vue';

/* Shared form models module */
export * as FormModels from './shared/form-models';

/* Form Builder component */
export { default as FormBuilder } from './form-builder/App.vue';
export { useFormBuilderStore } from './form-builder/store';

/* Form Submission component */
export { default as FormSubmission } from './form-submission/App.vue';
export { useFormSubmissionStore } from './form-submission/store';

/* Login component */
export { default as Login } from './login/App.vue';
export { useLoginStore } from './login/store';

/* Workflow Builder component */
export { default as WorkflowBuilder } from './workflow-builder/App.vue';
export { useWorkflowBuilderStore } from './workflow-builder/store';

/* google calendar component */
export { default as GoogleCalendar } from './google-calendar/App.vue'
export { useGoogleCalendarStore } from './google-calendar/store'


/* Entity Template builder component */
export { default as EntityTemplateBuilder } from './entity-template-builder/App.vue'
export { useEntityTemplateBuilderStore } from './entity-template-builder/store'

/* Entity Editor component */
export { default as EntityEditor } from './entity-editor/App.vue'
export { useEntityEditorStore } from './entity-editor/store'


export {default as HelloWorld} from './hello-world/App.vue'
