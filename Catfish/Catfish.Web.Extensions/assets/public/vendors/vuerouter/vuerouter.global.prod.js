/*!
  * vue-router v4.1.5
  * (c) 2022 Eduardo San Martin Morote
  * @license MIT
  */
var VueRouter=function(e,t){"use strict";const n="undefined"!=typeof window;function r(e){return e.__esModule||"Module"===e[Symbol.toStringTag]}const o=Object.assign;function a(e,t){const n={};for(const r in t){const o=t[r];n[r]=s(o)?o.map(e):e(o)}return n}const c=()=>{},s=Array.isArray,i=/\/$/;function l(e,t,n="/"){let r,o={},a="",c="";const s=t.indexOf("#");let i=t.indexOf("?");return s<i&&s>=0&&(i=-1),i>-1&&(r=t.slice(0,i),a=t.slice(i+1,s>-1?s:t.length),o=e(a)),s>-1&&(r=r||t.slice(0,s),c=t.slice(s,t.length)),r=function(e,t){if(e.startsWith("/"))return e;if(!e)return t;const n=t.split("/"),r=e.split("/");let o,a,c=n.length-1;for(o=0;o<r.length;o++)if(a=r[o],"."!==a){if(".."!==a)break;c>1&&c--}return n.slice(0,c).join("/")+"/"+r.slice(o-(o===r.length?1:0)).join("/")}(null!=r?r:t,n),{fullPath:r+(a&&"?")+a+c,path:r,query:o,hash:c}}function u(e,t){return t&&e.toLowerCase().startsWith(t.toLowerCase())?e.slice(t.length)||"/":e}function f(e,t){return(e.aliasOf||e)===(t.aliasOf||t)}function p(e,t){if(Object.keys(e).length!==Object.keys(t).length)return!1;for(const n in e)if(!h(e[n],t[n]))return!1;return!0}function h(e,t){return s(e)?d(e,t):s(t)?d(t,e):e===t}function d(e,t){return s(t)?e.length===t.length&&e.every(((e,n)=>e===t[n])):1===e.length&&e[0]===t}var m,g;!function(e){e.pop="pop",e.push="push"}(m||(m={})),function(e){e.back="back",e.forward="forward",e.unknown=""}(g||(g={}));function v(e){if(!e)if(n){const t=document.querySelector("base");e=(e=t&&t.getAttribute("href")||"/").replace(/^\w+:\/\/[^\/]+/,"")}else e="/";return"/"!==e[0]&&"#"!==e[0]&&(e="/"+e),e.replace(i,"")}const y=/^[^#]+#/;function b(e,t){return e.replace(y,"#")+t}const w=()=>({left:window.pageXOffset,top:window.pageYOffset});function E(e){let t;if("el"in e){const n=e.el,r="string"==typeof n&&n.startsWith("#"),o="string"==typeof n?r?document.getElementById(n.slice(1)):document.querySelector(n):n;if(!o)return;t=function(e,t){const n=document.documentElement.getBoundingClientRect(),r=e.getBoundingClientRect();return{behavior:t.behavior,left:r.left-n.left-(t.left||0),top:r.top-n.top-(t.top||0)}}(o,e)}else t=e;"scrollBehavior"in document.documentElement.style?window.scrollTo(t):window.scrollTo(null!=t.left?t.left:window.pageXOffset,null!=t.top?t.top:window.pageYOffset)}function R(e,t){return(history.state?history.state.position-t:-1)+e}const O=new Map;let k=()=>location.protocol+"//"+location.host;function P(e,t){const{pathname:n,search:r,hash:o}=t,a=e.indexOf("#");if(a>-1){let t=o.includes(e.slice(a))?e.slice(a).length:1,n=o.slice(t);return"/"!==n[0]&&(n="/"+n),u(n,"")}return u(n,e)+r+o}function j(e,t,n,r=!1,o=!1){return{back:e,current:t,forward:n,replaced:r,position:window.history.length,scroll:o?w():null}}function C(e){const t=function(e){const{history:t,location:n}=window,r={value:P(e,n)},a={value:t.state};function c(r,o,c){const s=e.indexOf("#"),i=s>-1?(n.host&&document.querySelector("base")?e:e.slice(s))+r:k()+e+r;try{t[c?"replaceState":"pushState"](o,"",i),a.value=o}catch(e){console.error(e),n[c?"replace":"assign"](i)}}return a.value||c(r.value,{back:null,current:r.value,forward:null,position:t.length-1,replaced:!0,scroll:null},!0),{location:r,state:a,push:function(e,n){const s=o({},a.value,t.state,{forward:e,scroll:w()});c(s.current,s,!0),c(e,o({},j(r.value,e,null),{position:s.position+1},n),!1),r.value=e},replace:function(e,n){c(e,o({},t.state,j(a.value.back,e,a.value.forward,!0),n,{position:a.value.position}),!0),r.value=e}}}(e=v(e)),n=function(e,t,n,r){let a=[],c=[],s=null;const i=({state:o})=>{const c=P(e,location),i=n.value,l=t.value;let u=0;if(o){if(n.value=c,t.value=o,s&&s===i)return void(s=null);u=l?o.position-l.position:0}else r(c);a.forEach((e=>{e(n.value,i,{delta:u,type:m.pop,direction:u?u>0?g.forward:g.back:g.unknown})}))};function l(){const{history:e}=window;e.state&&e.replaceState(o({},e.state,{scroll:w()}),"")}return window.addEventListener("popstate",i),window.addEventListener("beforeunload",l),{pauseListeners:function(){s=n.value},listen:function(e){a.push(e);const t=()=>{const t=a.indexOf(e);t>-1&&a.splice(t,1)};return c.push(t),t},destroy:function(){for(const e of c)e();c=[],window.removeEventListener("popstate",i),window.removeEventListener("beforeunload",l)}}}(e,t.state,t.location,t.replace);const r=o({location:"",base:e,go:function(e,t=!0){t||n.pauseListeners(),history.go(e)},createHref:b.bind(null,e)},t,n);return Object.defineProperty(r,"location",{enumerable:!0,get:()=>t.location.value}),Object.defineProperty(r,"state",{enumerable:!0,get:()=>t.state.value}),r}function x(e){return"string"==typeof e||"symbol"==typeof e}const $={path:"/",name:void 0,params:{},query:{},hash:"",fullPath:"/",matched:[],meta:{},redirectedFrom:void 0},S=Symbol("");var A;function L(e,t){return o(new Error,{type:e,[S]:!0},t)}function M(e,t){return e instanceof Error&&S in e&&(null==t||!!(e.type&t))}e.NavigationFailureType=void 0,(A=e.NavigationFailureType||(e.NavigationFailureType={}))[A.aborted=4]="aborted",A[A.cancelled=8]="cancelled",A[A.duplicated=16]="duplicated";const q="[^/]+?",B={sensitive:!1,strict:!1,start:!0,end:!0},T=/[.+*?^${}()[\]/\\]/g;function _(e,t){let n=0;for(;n<e.length&&n<t.length;){const r=t[n]-e[n];if(r)return r;n++}return e.length<t.length?1===e.length&&80===e[0]?-1:1:e.length>t.length?1===t.length&&80===t[0]?1:-1:0}function G(e,t){let n=0;const r=e.score,o=t.score;for(;n<r.length&&n<o.length;){const e=_(r[n],o[n]);if(e)return e;n++}if(1===Math.abs(o.length-r.length)){if(F(r))return 1;if(F(o))return-1}return o.length-r.length}function F(e){const t=e[e.length-1];return e.length>0&&t[t.length-1]<0}const D={type:0,value:""},I=/[a-zA-Z0-9_]/;function K(e,t,n){const r=function(e,t){const n=o({},B,t),r=[];let a=n.start?"^":"";const c=[];for(const t of e){const e=t.length?[]:[90];n.strict&&!t.length&&(a+="/");for(let r=0;r<t.length;r++){const o=t[r];let s=40+(n.sensitive?.25:0);if(0===o.type)r||(a+="/"),a+=o.value.replace(T,"\\$&"),s+=40;else if(1===o.type){const{value:e,repeatable:n,optional:i,regexp:l}=o;c.push({name:e,repeatable:n,optional:i});const u=l||q;if(u!==q){s+=10;try{new RegExp(`(${u})`)}catch(t){throw new Error(`Invalid custom RegExp for param "${e}" (${u}): `+t.message)}}let f=n?`((?:${u})(?:/(?:${u}))*)`:`(${u})`;r||(f=i&&t.length<2?`(?:/${f})`:"/"+f),i&&(f+="?"),a+=f,s+=20,i&&(s+=-8),n&&(s+=-20),".*"===u&&(s+=-50)}e.push(s)}r.push(e)}if(n.strict&&n.end){const e=r.length-1;r[e][r[e].length-1]+=.7000000000000001}n.strict||(a+="/?"),n.end?a+="$":n.strict&&(a+="(?:/|$)");const i=new RegExp(a,n.sensitive?"":"i");return{re:i,score:r,keys:c,parse:function(e){const t=e.match(i),n={};if(!t)return null;for(let e=1;e<t.length;e++){const r=t[e]||"",o=c[e-1];n[o.name]=r&&o.repeatable?r.split("/"):r}return n},stringify:function(t){let n="",r=!1;for(const o of e){r&&n.endsWith("/")||(n+="/"),r=!1;for(const e of o)if(0===e.type)n+=e.value;else if(1===e.type){const{value:a,repeatable:c,optional:i}=e,l=a in t?t[a]:"";if(s(l)&&!c)throw new Error(`Provided param "${a}" is an array but it is not repeatable (* or + modifiers)`);const u=s(l)?l.join("/"):l;if(!u){if(!i)throw new Error(`Missing required param "${a}"`);o.length<2&&(n.endsWith("/")?n=n.slice(0,-1):r=!0)}n+=u}}return n||"/"}}}(function(e){if(!e)return[[]];if("/"===e)return[[D]];if(!e.startsWith("/"))throw new Error(`Invalid path "${e}"`);function t(e){throw new Error(`ERR (${n})/"${l}": ${e}`)}let n=0,r=n;const o=[];let a;function c(){a&&o.push(a),a=[]}let s,i=0,l="",u="";function f(){l&&(0===n?a.push({type:0,value:l}):1===n||2===n||3===n?(a.length>1&&("*"===s||"+"===s)&&t(`A repeatable param (${l}) must be alone in its segment. eg: '/:ids+.`),a.push({type:1,value:l,regexp:u,repeatable:"*"===s||"+"===s,optional:"*"===s||"?"===s})):t("Invalid state to consume buffer"),l="")}function p(){l+=s}for(;i<e.length;)if(s=e[i++],"\\"!==s||2===n)switch(n){case 0:"/"===s?(l&&f(),c()):":"===s?(f(),n=1):p();break;case 4:p(),n=r;break;case 1:"("===s?n=2:I.test(s)?p():(f(),n=0,"*"!==s&&"?"!==s&&"+"!==s&&i--);break;case 2:")"===s?"\\"==u[u.length-1]?u=u.slice(0,-1)+s:n=3:u+=s;break;case 3:f(),n=0,"*"!==s&&"?"!==s&&"+"!==s&&i--,u="";break;default:t("Unknown state")}else r=n,n=4;return 2===n&&t(`Unfinished custom RegExp for param "${l}"`),f(),c(),o}(e.path),n),a=o(r,{record:e,parent:t,children:[],alias:[]});return t&&!a.record.aliasOf==!t.record.aliasOf&&t.children.push(a),a}function U(e,t){const n=[],r=new Map;function a(e,n,r){const l=!r,u=function(e){return{path:e.path,redirect:e.redirect,name:e.name,meta:e.meta||{},aliasOf:void 0,beforeEnter:e.beforeEnter,props:H(e),children:e.children||[],instances:{},leaveGuards:new Set,updateGuards:new Set,enterCallbacks:{},components:"components"in e?e.components||null:e.component&&{default:e.component}}}(e);u.aliasOf=r&&r.record;const f=z(t,e),p=[u];if("alias"in e){const t="string"==typeof e.alias?[e.alias]:e.alias;for(const e of t)p.push(o({},u,{components:r?r.record.components:u.components,path:e,aliasOf:r?r.record:u}))}let h,d;for(const t of p){const{path:o}=t;if(n&&"/"!==o[0]){const e=n.record.path,r="/"===e[e.length-1]?"":"/";t.path=n.record.path+(o&&r+o)}if(h=K(t,n,f),r?r.alias.push(h):(d=d||h,d!==h&&d.alias.push(h),l&&e.name&&!W(h)&&s(e.name)),u.children){const e=u.children;for(let t=0;t<e.length;t++)a(e[t],h,r&&r.children[t])}r=r||h,i(h)}return d?()=>{s(d)}:c}function s(e){if(x(e)){const t=r.get(e);t&&(r.delete(e),n.splice(n.indexOf(t),1),t.children.forEach(s),t.alias.forEach(s))}else{const t=n.indexOf(e);t>-1&&(n.splice(t,1),e.record.name&&r.delete(e.record.name),e.children.forEach(s),e.alias.forEach(s))}}function i(e){let t=0;for(;t<n.length&&G(e,n[t])>=0&&(e.record.path!==n[t].record.path||!Q(e,n[t]));)t++;n.splice(t,0,e),e.record.name&&!W(e)&&r.set(e.record.name,e)}return t=z({strict:!1,end:!0,sensitive:!1},t),e.forEach((e=>a(e))),{addRoute:a,resolve:function(e,t){let a,c,s,i={};if("name"in e&&e.name){if(a=r.get(e.name),!a)throw L(1,{location:e});s=a.record.name,i=o(V(t.params,a.keys.filter((e=>!e.optional)).map((e=>e.name))),e.params&&V(e.params,a.keys.map((e=>e.name)))),c=a.stringify(i)}else if("path"in e)c=e.path,a=n.find((e=>e.re.test(c))),a&&(i=a.parse(c),s=a.record.name);else{if(a=t.name?r.get(t.name):n.find((e=>e.re.test(t.path))),!a)throw L(1,{location:e,currentLocation:t});s=a.record.name,i=o({},t.params,e.params),c=a.stringify(i)}const l=[];let u=a;for(;u;)l.unshift(u.record),u=u.parent;return{name:s,path:c,params:i,matched:l,meta:N(l)}},removeRoute:s,getRoutes:function(){return n},getRecordMatcher:function(e){return r.get(e)}}}function V(e,t){const n={};for(const r of t)r in e&&(n[r]=e[r]);return n}function H(e){const t={},n=e.props||!1;if("component"in e)t.default=n;else for(const r in e.components)t[r]="boolean"==typeof n?n:n[r];return t}function W(e){for(;e;){if(e.record.aliasOf)return!0;e=e.parent}return!1}function N(e){return e.reduce(((e,t)=>o(e,t.meta)),{})}function z(e,t){const n={};for(const r in e)n[r]=r in t?t[r]:e[r];return n}function Q(e,t){return t.children.some((t=>t===e||Q(e,t)))}const X=/#/g,Y=/&/g,Z=/\//g,J=/=/g,ee=/\?/g,te=/\+/g,ne=/%5B/g,re=/%5D/g,oe=/%5E/g,ae=/%60/g,ce=/%7B/g,se=/%7C/g,ie=/%7D/g,le=/%20/g;function ue(e){return encodeURI(""+e).replace(se,"|").replace(ne,"[").replace(re,"]")}function fe(e){return ue(e).replace(te,"%2B").replace(le,"+").replace(X,"%23").replace(Y,"%26").replace(ae,"`").replace(ce,"{").replace(ie,"}").replace(oe,"^")}function pe(e){return null==e?"":function(e){return ue(e).replace(X,"%23").replace(ee,"%3F")}(e).replace(Z,"%2F")}function he(e){try{return decodeURIComponent(""+e)}catch(e){}return""+e}function de(e){const t={};if(""===e||"?"===e)return t;const n=("?"===e[0]?e.slice(1):e).split("&");for(let e=0;e<n.length;++e){const r=n[e].replace(te," "),o=r.indexOf("="),a=he(o<0?r:r.slice(0,o)),c=o<0?null:he(r.slice(o+1));if(a in t){let e=t[a];s(e)||(e=t[a]=[e]),e.push(c)}else t[a]=c}return t}function me(e){let t="";for(let n in e){const r=e[n];if(n=fe(n).replace(J,"%3D"),null==r){void 0!==r&&(t+=(t.length?"&":"")+n);continue}(s(r)?r.map((e=>e&&fe(e))):[r&&fe(r)]).forEach((e=>{void 0!==e&&(t+=(t.length?"&":"")+n,null!=e&&(t+="="+e))}))}return t}function ge(e){const t={};for(const n in e){const r=e[n];void 0!==r&&(t[n]=s(r)?r.map((e=>null==e?null:""+e)):null==r?r:""+r)}return t}const ve=Symbol(""),ye=Symbol(""),be=Symbol(""),we=Symbol(""),Ee=Symbol("");function Re(){let e=[];return{add:function(t){return e.push(t),()=>{const n=e.indexOf(t);n>-1&&e.splice(n,1)}},list:()=>e,reset:function(){e=[]}}}function Oe(e,n,r){const o=()=>{e[n].delete(r)};t.onUnmounted(o),t.onDeactivated(o),t.onActivated((()=>{e[n].add(r)})),e[n].add(r)}function ke(e,t,n,r,o){const a=r&&(r.enterCallbacks[o]=r.enterCallbacks[o]||[]);return()=>new Promise(((c,s)=>{const i=e=>{var i;!1===e?s(L(4,{from:n,to:t})):e instanceof Error?s(e):"string"==typeof(i=e)||i&&"object"==typeof i?s(L(2,{from:t,to:e})):(a&&r.enterCallbacks[o]===a&&"function"==typeof e&&a.push(e),c())},l=e.call(r&&r.instances[o],t,n,i);let u=Promise.resolve(l);e.length<3&&(u=u.then(i)),u.catch((e=>s(e)))}))}function Pe(e,t,n,o){const a=[];for(const s of e)for(const e in s.components){let i=s.components[e];if("beforeRouteEnter"===t||s.instances[e])if("object"==typeof(c=i)||"displayName"in c||"props"in c||"__vccOpts"in c){const r=(i.__vccOpts||i)[t];r&&a.push(ke(r,n,o,s,e))}else{let c=i();a.push((()=>c.then((a=>{if(!a)return Promise.reject(new Error(`Couldn't resolve component "${e}" at "${s.path}"`));const c=r(a)?a.default:a;s.components[e]=c;const i=(c.__vccOpts||c)[t];return i&&ke(i,n,o,s,e)()}))))}}var c;return a}function je(e){const n=t.inject(be),r=t.inject(we),o=t.computed((()=>n.resolve(t.unref(e.to)))),a=t.computed((()=>{const{matched:e}=o.value,{length:t}=e,n=e[t-1],a=r.matched;if(!n||!a.length)return-1;const c=a.findIndex(f.bind(null,n));if(c>-1)return c;const s=xe(e[t-2]);return t>1&&xe(n)===s&&a[a.length-1].path!==s?a.findIndex(f.bind(null,e[t-2])):c})),i=t.computed((()=>a.value>-1&&function(e,t){for(const n in t){const r=t[n],o=e[n];if("string"==typeof r){if(r!==o)return!1}else if(!s(o)||o.length!==r.length||r.some(((e,t)=>e!==o[t])))return!1}return!0}(r.params,o.value.params))),l=t.computed((()=>a.value>-1&&a.value===r.matched.length-1&&p(r.params,o.value.params)));return{route:o,href:t.computed((()=>o.value.href)),isActive:i,isExactActive:l,navigate:function(r={}){return function(e){if(e.metaKey||e.altKey||e.ctrlKey||e.shiftKey)return;if(e.defaultPrevented)return;if(void 0!==e.button&&0!==e.button)return;if(e.currentTarget&&e.currentTarget.getAttribute){const t=e.currentTarget.getAttribute("target");if(/\b_blank\b/i.test(t))return}e.preventDefault&&e.preventDefault();return!0}(r)?n[t.unref(e.replace)?"replace":"push"](t.unref(e.to)).catch(c):Promise.resolve()}}}const Ce=t.defineComponent({name:"RouterLink",compatConfig:{MODE:3},props:{to:{type:[String,Object],required:!0},replace:Boolean,activeClass:String,exactActiveClass:String,custom:Boolean,ariaCurrentValue:{type:String,default:"page"}},useLink:je,setup(e,{slots:n}){const r=t.reactive(je(e)),{options:o}=t.inject(be),a=t.computed((()=>({[$e(e.activeClass,o.linkActiveClass,"router-link-active")]:r.isActive,[$e(e.exactActiveClass,o.linkExactActiveClass,"router-link-exact-active")]:r.isExactActive})));return()=>{const o=n.default&&n.default(r);return e.custom?o:t.h("a",{"aria-current":r.isExactActive?e.ariaCurrentValue:null,href:r.href,onClick:r.navigate,class:a.value},o)}}});function xe(e){return e?e.aliasOf?e.aliasOf.path:e.path:""}const $e=(e,t,n)=>null!=e?e:null!=t?t:n;function Se(e,t){if(!e)return null;const n=e(t);return 1===n.length?n[0]:n}const Ae=t.defineComponent({name:"RouterView",inheritAttrs:!1,props:{name:{type:String,default:"default"},route:Object},compatConfig:{MODE:3},setup(e,{attrs:n,slots:r}){const a=t.inject(Ee),c=t.computed((()=>e.route||a.value)),s=t.inject(ye,0),i=t.computed((()=>{let e=t.unref(s);const{matched:n}=c.value;let r;for(;(r=n[e])&&!r.components;)e++;return e})),l=t.computed((()=>c.value.matched[i.value]));t.provide(ye,t.computed((()=>i.value+1))),t.provide(ve,l),t.provide(Ee,c);const u=t.ref();return t.watch((()=>[u.value,l.value,e.name]),(([e,t,n],[r,o,a])=>{t&&(t.instances[n]=e,o&&o!==t&&e&&e===r&&(t.leaveGuards.size||(t.leaveGuards=o.leaveGuards),t.updateGuards.size||(t.updateGuards=o.updateGuards))),!e||!t||o&&f(t,o)&&r||(t.enterCallbacks[n]||[]).forEach((t=>t(e)))}),{flush:"post"}),()=>{const a=c.value,s=e.name,i=l.value,f=i&&i.components[s];if(!f)return Se(r.default,{Component:f,route:a});const p=i.props[s],h=p?!0===p?a.params:"function"==typeof p?p(a):p:null,d=t.h(f,o({},h,n,{onVnodeUnmounted:e=>{e.component.isUnmounted&&(i.instances[s]=null)},ref:u}));return Se(r.default,{Component:d,route:a})||d}}});function Le(e){return e.reduce(((e,t)=>e.then((()=>t()))),Promise.resolve())}return e.RouterLink=Ce,e.RouterView=Ae,e.START_LOCATION=$,e.createMemoryHistory=function(e=""){let t=[],n=[""],r=0;function o(e){r++,r===n.length||n.splice(r),n.push(e)}const a={location:"",state:{},base:e=v(e),createHref:b.bind(null,e),replace(e){n.splice(r--,1),o(e)},push(e,t){o(e)},listen:e=>(t.push(e),()=>{const n=t.indexOf(e);n>-1&&t.splice(n,1)}),destroy(){t=[],n=[""],r=0},go(e,o=!0){const a=this.location,c=e<0?g.back:g.forward;r=Math.max(0,Math.min(r+e,n.length-1)),o&&function(e,n,{direction:r,delta:o}){const a={direction:r,delta:o,type:m.pop};for(const r of t)r(e,n,a)}(this.location,a,{direction:c,delta:e})}};return Object.defineProperty(a,"location",{enumerable:!0,get:()=>n[r]}),a},e.createRouter=function(e){const r=U(e.routes,e),i=e.parseQuery||de,u=e.stringifyQuery||me,h=e.history,d=Re(),g=Re(),v=Re(),y=t.shallowRef($);let b=$;n&&e.scrollBehavior&&"scrollRestoration"in history&&(history.scrollRestoration="manual");const k=a.bind(null,(e=>""+e)),P=a.bind(null,pe),j=a.bind(null,he);function C(e,t){if(t=o({},t||y.value),"string"==typeof e){const n=l(i,e,t.path),a=r.resolve({path:n.path},t),c=h.createHref(n.fullPath);return o(n,a,{params:j(a.params),hash:he(n.hash),redirectedFrom:void 0,href:c})}let n;if("path"in e)n=o({},e,{path:l(i,e.path,t.path).path});else{const r=o({},e.params);for(const e in r)null==r[e]&&delete r[e];n=o({},e,{params:P(e.params)}),t.params=P(t.params)}const a=r.resolve(n,t),c=e.hash||"";a.params=k(j(a.params));const s=function(e,t){const n=t.query?e(t.query):"";return t.path+(n&&"?")+n+(t.hash||"")}(u,o({},e,{hash:(f=c,ue(f).replace(ce,"{").replace(ie,"}").replace(oe,"^")),path:a.path}));var f;const p=h.createHref(s);return o({fullPath:s,hash:c,query:u===me?ge(e.query):e.query||{}},a,{redirectedFrom:void 0,href:p})}function S(e){return"string"==typeof e?l(i,e,y.value.path):o({},e)}function A(e,t){if(b!==e)return L(8,{from:t,to:e})}function q(e){return T(e)}function B(e){const t=e.matched[e.matched.length-1];if(t&&t.redirect){const{redirect:n}=t;let r="function"==typeof n?n(e):n;return"string"==typeof r&&(r=r.includes("?")||r.includes("#")?r=S(r):{path:r},r.params={}),o({query:e.query,hash:e.hash,params:"path"in r?{}:e.params},r)}}function T(e,t){const n=b=C(e),r=y.value,a=e.state,c=e.force,s=!0===e.replace,i=B(n);if(i)return T(o(S(i),{state:"object"==typeof i?o({},a,i.state):a,force:c,replace:s}),t||n);const l=n;let h;return l.redirectedFrom=t,!c&&function(e,t,n){const r=t.matched.length-1,o=n.matched.length-1;return r>-1&&r===o&&f(t.matched[r],n.matched[o])&&p(t.params,n.params)&&e(t.query)===e(n.query)&&t.hash===n.hash}(u,r,n)&&(h=L(16,{to:l,from:r}),Q(r,r,!0,!1)),(h?Promise.resolve(h):G(l,r)).catch((e=>M(e)?M(e,2)?e:z(e):N(e,l,r))).then((e=>{if(e){if(M(e,2))return T(o({replace:s},S(e.to),{state:"object"==typeof e.to?o({},a,e.to.state):a,force:c}),t||l)}else e=D(l,r,!0,s,a);return F(l,r,e),e}))}function _(e,t){const n=A(e,t);return n?Promise.reject(n):Promise.resolve()}function G(e,t){let n;const[r,o,a]=function(e,t){const n=[],r=[],o=[],a=Math.max(t.matched.length,e.matched.length);for(let c=0;c<a;c++){const a=t.matched[c];a&&(e.matched.find((e=>f(e,a)))?r.push(a):n.push(a));const s=e.matched[c];s&&(t.matched.find((e=>f(e,s)))||o.push(s))}return[n,r,o]}(e,t);n=Pe(r.reverse(),"beforeRouteLeave",e,t);for(const o of r)o.leaveGuards.forEach((r=>{n.push(ke(r,e,t))}));const c=_.bind(null,e,t);return n.push(c),Le(n).then((()=>{n=[];for(const r of d.list())n.push(ke(r,e,t));return n.push(c),Le(n)})).then((()=>{n=Pe(o,"beforeRouteUpdate",e,t);for(const r of o)r.updateGuards.forEach((r=>{n.push(ke(r,e,t))}));return n.push(c),Le(n)})).then((()=>{n=[];for(const r of e.matched)if(r.beforeEnter&&!t.matched.includes(r))if(s(r.beforeEnter))for(const o of r.beforeEnter)n.push(ke(o,e,t));else n.push(ke(r.beforeEnter,e,t));return n.push(c),Le(n)})).then((()=>(e.matched.forEach((e=>e.enterCallbacks={})),n=Pe(a,"beforeRouteEnter",e,t),n.push(c),Le(n)))).then((()=>{n=[];for(const r of g.list())n.push(ke(r,e,t));return n.push(c),Le(n)})).catch((e=>M(e,8)?e:Promise.reject(e)))}function F(e,t,n){for(const r of v.list())r(e,t,n)}function D(e,t,r,a,c){const s=A(e,t);if(s)return s;const i=t===$,l=n?history.state:{};r&&(a||i?h.replace(e.fullPath,o({scroll:i&&l&&l.scroll},c)):h.push(e.fullPath,c)),y.value=e,Q(e,t,r,i),z()}let I;function K(){I||(I=h.listen(((e,t,r)=>{if(!J.listening)return;const a=C(e),s=B(a);if(s)return void T(o(s,{replace:!0}),a).catch(c);b=a;const i=y.value;var l,u;n&&(l=R(i.fullPath,r.delta),u=w(),O.set(l,u)),G(a,i).catch((e=>M(e,12)?e:M(e,2)?(T(e.to,a).then((e=>{M(e,20)&&!r.delta&&r.type===m.pop&&h.go(-1,!1)})).catch(c),Promise.reject()):(r.delta&&h.go(-r.delta,!1),N(e,a,i)))).then((e=>{(e=e||D(a,i,!1))&&(r.delta&&!M(e,8)?h.go(-r.delta,!1):r.type===m.pop&&M(e,20)&&h.go(-1,!1)),F(a,i,e)})).catch(c)})))}let V,H=Re(),W=Re();function N(e,t,n){z(e);const r=W.list();return r.length?r.forEach((r=>r(e,t,n))):console.error(e),Promise.reject(e)}function z(e){return V||(V=!e,K(),H.list().forEach((([t,n])=>e?n(e):t())),H.reset()),e}function Q(r,o,a,c){const{scrollBehavior:s}=e;if(!n||!s)return Promise.resolve();const i=!a&&function(e){const t=O.get(e);return O.delete(e),t}(R(r.fullPath,0))||(c||!a)&&history.state&&history.state.scroll||null;return t.nextTick().then((()=>s(r,o,i))).then((e=>e&&E(e))).catch((e=>N(e,r,o)))}const X=e=>h.go(e);let Y;const Z=new Set,J={currentRoute:y,listening:!0,addRoute:function(e,t){let n,o;return x(e)?(n=r.getRecordMatcher(e),o=t):o=e,r.addRoute(o,n)},removeRoute:function(e){const t=r.getRecordMatcher(e);t&&r.removeRoute(t)},hasRoute:function(e){return!!r.getRecordMatcher(e)},getRoutes:function(){return r.getRoutes().map((e=>e.record))},resolve:C,options:e,push:q,replace:function(e){return q(o(S(e),{replace:!0}))},go:X,back:()=>X(-1),forward:()=>X(1),beforeEach:d.add,beforeResolve:g.add,afterEach:v.add,onError:W.add,isReady:function(){return V&&y.value!==$?Promise.resolve():new Promise(((e,t)=>{H.add([e,t])}))},install(e){e.component("RouterLink",Ce),e.component("RouterView",Ae),e.config.globalProperties.$router=this,Object.defineProperty(e.config.globalProperties,"$route",{enumerable:!0,get:()=>t.unref(y)}),n&&!Y&&y.value===$&&(Y=!0,q(h.location).catch((e=>{})));const r={};for(const e in $)r[e]=t.computed((()=>y.value[e]));e.provide(be,this),e.provide(we,t.reactive(r)),e.provide(Ee,y);const o=e.unmount;Z.add(e),e.unmount=function(){Z.delete(e),Z.size<1&&(b=$,I&&I(),I=null,y.value=$,Y=!1,V=!1),o()}}};return J},e.createRouterMatcher=U,e.createWebHashHistory=function(e){return(e=location.host?e||location.pathname+location.search:"").includes("#")||(e+="#"),C(e)},e.createWebHistory=C,e.isNavigationFailure=M,e.loadRouteLocation=function(e){return e.matched.every((e=>e.redirect))?Promise.reject(new Error("Cannot load a route that redirects.")):Promise.all(e.matched.map((e=>e.components&&Promise.all(Object.keys(e.components).reduce(((t,n)=>{const o=e.components[n];return"function"!=typeof o||"displayName"in o||t.push(o().then((t=>{if(!t)return Promise.reject(new Error(`Couldn't resolve component "${n}" at "${e.path}". Ensure you passed a function that returns a promise.`));const o=r(t)?t.default:t;e.components[n]=o}))),t}),[]))))).then((()=>e))},e.matchedRouteKey=ve,e.onBeforeRouteLeave=function(e){const n=t.inject(ve,{}).value;n&&Oe(n,"leaveGuards",e)},e.onBeforeRouteUpdate=function(e){const n=t.inject(ve,{}).value;n&&Oe(n,"updateGuards",e)},e.parseQuery=de,e.routeLocationKey=we,e.routerKey=be,e.routerViewLocationKey=Ee,e.stringifyQuery=me,e.useLink=je,e.useRoute=function(){return t.inject(we)},e.useRouter=function(){return t.inject(be)},e.viewDepthKey=ye,Object.defineProperty(e,"__esModule",{value:!0}),e}({},Vue);
