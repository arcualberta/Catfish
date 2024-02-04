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
export { BootstrapVue3 } from 'bootstrap-vue-3';
import IconsPlugin from 'bootstrap-vue-3';
export { IconsPlugin };
import FloatingVue from 'floating-vue';
export { FloatingVue };
/**
 * Exporting Font Awesome librarty
 */
import * as faIcons from '@fortawesome/free-solid-svg-icons';
import { library } from '@fortawesome/fontawesome-svg-core';
library.add(faIcons.faCircleCheck);
library.add(faIcons.faCircleXmark);
library.add(faIcons.faPenToSquare);
library.add(faIcons.faCirclePlus);
library.add(faIcons.faQuestionCircle);
library.add(faIcons.faThList);
library.add(faIcons.faArrowLeft);
library.add(faIcons.faArrowUpRightFromSquare);
library.add(faIcons.faCopy);
library.add(faIcons.faArrowDown);
library.add(faIcons.faChevronDown);
library.add(faIcons.faChevronRight);
library.add(faIcons.faChevronUp);
export { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
//# sourceMappingURL=library-exports.js.map