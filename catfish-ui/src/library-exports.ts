
/**
 * Exporting Components
 */
 export * as Components from './components';

/**
 * Exporting Applets
 */
 export * as Apps from './apps';

/**
 * Exporting the external Bootstrap Components
 */
export {BootstrapVue3} from './external/bootstrap-vue-3/BootstrapVue'
export {BootstrapVue3Icons} from './external/bootstrap-vue-3-icons/BootstrapVueIcons'

/**
 * Exporting Font Awesome librarty
 */
import * as faIcons from '@fortawesome/free-solid-svg-icons'
import { library } from '@fortawesome/fontawesome-svg-core'
library.add(faIcons.faCircleCheck)
library.add(faIcons.faCircleXmark)
library.add(faIcons.faPenToSquare)
library.add(faIcons.faCirclePlus)
library.add(faIcons.faQuestionCircle)
library.add(faIcons.faThList)
library.add(faIcons.faArrowLeft)
export {FontAwesomeIcon} from '@fortawesome/vue-fontawesome'
