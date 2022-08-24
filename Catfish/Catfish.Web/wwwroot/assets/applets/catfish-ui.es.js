import { getCurrentInstance as Qa, inject as Gs, markRaw as lt, ref as Jn, watch as Qn, reactive as js, effectScope as Ys, isRef as nt, isReactive as so, toRef as hr, toRaw as qs, nextTick as No, computed as he, onUnmounted as Zs, toRefs as Ho, defineComponent as K, h as Fo, resolveComponent as ge, openBlock as S, createElementBlock as x, unref as _, createElementVNode as C, toDisplayString as se, withDirectives as Ee, vModelText as xe, createCommentVNode as Z, Fragment as q, renderList as Ce, createBlock as pe, createVNode as B, withCtx as Be, Transition as lo, normalizeClass as uo, pushScopeId as er, popScopeId as tr, createTextVNode as He, vModelSelect as ei, vModelRadio as Xs, onMounted as ti } from "vue";
var Ot = /* @__PURE__ */ ((e) => (e.ShortAnswer = "Short Answer", e.Paragraph = "Paragraph", e.RichText = "Rich Text", e))(Ot || {}), nr = /* @__PURE__ */ ((e) => (e.Date = "Date", e.DateTime = "Date Time", e.Decimal = "Decimal", e.Integer = "Integer", e.Email = "Email", e))(nr || {}), rr = /* @__PURE__ */ ((e) => (e.Checkboxes = "Checkboxes", e.DataList = "Data List", e.RadioButtons = "Radio Buttons", e.DropDown = "Drop Down", e))(rr || {}), co = /* @__PURE__ */ ((e) => (e.InfoSection = "Info Section", e))(co || {});
const z = { ...Ot, ...nr, ...rr, ...co };
var or = /* @__PURE__ */ ((e) => (e[e.None = 0] = "None", e[e.Optional = 1] = "Optional", e[e.Required = 2] = "Required", e))(or || {});
const Bm = /* @__PURE__ */ Object.freeze(/* @__PURE__ */ Object.defineProperty({
  __proto__: null,
  TextType: Ot,
  MonolingualFieldType: nr,
  OptionFieldType: rr,
  InfoSectionType: co,
  FieldTypes: z,
  ExtensionType: or
}, Symbol.toStringTag, { value: "Module" }));
var Ks = function() {
  function e(t) {
    if (!t)
      throw new TypeError("Invalid argument; `value` has no value.");
    this.value = e.EMPTY, t && e.isGuid(t) && (this.value = t);
  }
  return e.isGuid = function(t) {
    var n = t.toString();
    return t && (t instanceof e || e.validator.test(n));
  }, e.create = function() {
    return new e([e.gen(2), e.gen(1), e.gen(1), e.gen(1), e.gen(3)].join("-"));
  }, e.createEmpty = function() {
    return new e("emptyguid");
  }, e.parse = function(t) {
    return new e(t);
  }, e.raw = function() {
    return [e.gen(2), e.gen(1), e.gen(1), e.gen(1), e.gen(3)].join("-");
  }, e.gen = function(t) {
    for (var n = "", r = 0; r < t; r++)
      n += ((1 + Math.random()) * 65536 | 0).toString(16).substring(1);
    return n;
  }, e.prototype.equals = function(t) {
    return e.isGuid(t) && this.value === t.toString();
  }, e.prototype.isEmpty = function() {
    return this.value === e.EMPTY;
  }, e.prototype.toString = function() {
    return this.value;
  }, e.prototype.toJSON = function() {
    return {
      value: this.value
    };
  }, e.validator = new RegExp("^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$", "i"), e.EMPTY = "00000000-0000-0000-0000-000000000000", e;
}(), ee = Ks;
function pn(e, t, n) {
  return Array.isArray(e) ? (e.length = Math.max(e.length, t), e.splice(t, 1, n), n) : (e[t] = n, n);
}
function vr(e, t) {
  if (Array.isArray(e)) {
    e.splice(t, 1);
    return;
  }
  delete e[t];
}
/*!
  * pinia v2.0.18
  * (c) 2022 Eduardo San Martin Morote
  * @license MIT
  */
let Br;
const An = (e) => Br = e, Js = Symbol("pinia");
function pt(e) {
  return e && typeof e == "object" && Object.prototype.toString.call(e) === "[object Object]" && typeof e.toJSON != "function";
}
var jt;
(function(e) {
  e.direct = "direct", e.patchObject = "patch object", e.patchFunction = "patch function";
})(jt || (jt = {}));
const Wt = typeof window < "u";
function ni(e, t) {
  for (const n in t) {
    const r = t[n];
    if (!(n in e))
      continue;
    const o = e[n];
    pt(o) && pt(r) && !nt(r) && !so(r) ? e[n] = ni(o, r) : e[n] = r;
  }
  return e;
}
const Qs = () => {
};
function Vo(e, t, n, r = Qs) {
  e.push(t);
  const o = () => {
    const a = e.indexOf(t);
    a > -1 && (e.splice(a, 1), r());
  };
  return !n && Qa() && Zs(o), o;
}
function Et(e, ...t) {
  e.slice().forEach((n) => {
    n(...t);
  });
}
function Wr(e, t) {
  for (const n in t) {
    if (!t.hasOwnProperty(n))
      continue;
    const r = t[n], o = e[n];
    pt(o) && pt(r) && e.hasOwnProperty(n) && !nt(r) && !so(r) ? e[n] = Wr(o, r) : e[n] = r;
  }
  return e;
}
const el = Symbol("pinia:skipHydration");
function tl(e) {
  return !pt(e) || !e.hasOwnProperty(el);
}
const { assign: Ie } = Object;
function Uo(e) {
  return !!(nt(e) && e.effect);
}
function Lo(e, t, n, r) {
  const { state: o, actions: a, getters: i } = t, s = n.state.value[e];
  let l;
  function u() {
    !s && !r && (n.state.value[e] = o ? o() : {});
    const c = r ? Ho(Jn(o ? o() : {}).value) : Ho(n.state.value[e]);
    return Ie(c, a, Object.keys(i || {}).reduce((d, f) => (f in c && console.warn(`[\u{1F34D}]: A getter cannot have the same name as another state property. Rename one of them. Found with "${f}" in store "${e}".`), d[f] = lt(he(() => {
      An(n);
      const p = n._s.get(e);
      return i[f].call(p, p);
    })), d), {}));
  }
  return l = $r(e, u, t, n, r, !0), l.$reset = function() {
    const d = o ? o() : {};
    this.$patch((f) => {
      Ie(f, d);
    });
  }, l;
}
function $r(e, t, n = {}, r, o, a) {
  let i;
  const s = Ie({ actions: {} }, n);
  if (!r._e.active)
    throw new Error("Pinia destroyed");
  const l = {
    deep: !0
  };
  l.onTrigger = (M) => {
    u ? p = M : u == !1 && !R._hotUpdating && (Array.isArray(p) ? p.push(M) : console.error("\u{1F34D} debuggerEvents should be an array. This is most likely an internal Pinia bug."));
  };
  let u, c, d = lt([]), f = lt([]), p;
  const h = r.state.value[e];
  !a && !h && !o && (r.state.value[e] = {});
  const b = Jn({});
  let E;
  function y(M) {
    let I;
    u = c = !1, p = [], typeof M == "function" ? (M(r.state.value[e]), I = {
      type: jt.patchFunction,
      storeId: e,
      events: p
    }) : (Wr(r.state.value[e], M), I = {
      type: jt.patchObject,
      payload: M,
      storeId: e,
      events: p
    });
    const N = E = Symbol();
    No().then(() => {
      E === N && (u = !0);
    }), c = !0, Et(d, I, r.state.value[e]);
  }
  const D = () => {
    throw new Error(`\u{1F34D}: Store "${e}" is built using the setup syntax and does not implement $reset().`);
  };
  function k() {
    i.stop(), d = [], f = [], r._s.delete(e);
  }
  function T(M, I) {
    return function() {
      An(r);
      const N = Array.from(arguments), j = [], oe = [];
      function Ue(ce) {
        j.push(ce);
      }
      function at(ce) {
        oe.push(ce);
      }
      Et(f, {
        args: N,
        name: M,
        store: R,
        after: Ue,
        onError: at
      });
      let ye;
      try {
        ye = I.apply(this && this.$id === e ? this : R, N);
      } catch (ce) {
        throw Et(oe, ce), ce;
      }
      return ye instanceof Promise ? ye.then((ce) => (Et(j, ce), ce)).catch((ce) => (Et(oe, ce), Promise.reject(ce))) : (Et(j, ye), ye);
    };
  }
  const U = /* @__PURE__ */ lt({
    actions: {},
    getters: {},
    state: [],
    hotState: b
  }), O = {
    _p: r,
    $id: e,
    $onAction: Vo.bind(null, f),
    $patch: y,
    $reset: D,
    $subscribe(M, I = {}) {
      const N = Vo(d, M, I.detached, () => j()), j = i.run(() => Qn(() => r.state.value[e], (oe) => {
        (I.flush === "sync" ? c : u) && M({
          storeId: e,
          type: jt.direct,
          events: p
        }, oe);
      }, Ie({}, l, I)));
      return N;
    },
    $dispose: k
  }, R = js(Ie(
    Wt ? {
      _customProperties: lt(/* @__PURE__ */ new Set()),
      _hmrPayload: U
    } : {},
    O
  ));
  r._s.set(e, R);
  const A = r._e.run(() => (i = Ys(), i.run(() => t())));
  for (const M in A) {
    const I = A[M];
    if (nt(I) && !Uo(I) || so(I))
      o ? pn(b.value, M, hr(A, M)) : a || (h && tl(I) && (nt(I) ? I.value = h[M] : Wr(I, h[M])), r.state.value[e][M] = I), U.state.push(M);
    else if (typeof I == "function") {
      const N = o ? I : T(M, I);
      A[M] = N, U.actions[M] = I, s.actions[M] = I;
    } else
      Uo(I) && (U.getters[M] = a ? n.getters[M] : I, Wt && (A._getters || (A._getters = lt([]))).push(M));
  }
  Ie(R, A), Ie(qs(R), A), Object.defineProperty(R, "$state", {
    get: () => o ? b.value : r.state.value[e],
    set: (M) => {
      if (o)
        throw new Error("cannot set hotState");
      y((I) => {
        Ie(I, M);
      });
    }
  });
  {
    R._hotUpdate = lt((I) => {
      R._hotUpdating = !0, I._hmrPayload.state.forEach((N) => {
        if (N in R.$state) {
          const j = I.$state[N], oe = R.$state[N];
          typeof j == "object" && pt(j) && pt(oe) ? ni(j, oe) : I.$state[N] = oe;
        }
        pn(R, N, hr(I.$state, N));
      }), Object.keys(R.$state).forEach((N) => {
        N in I.$state || vr(R, N);
      }), u = !1, c = !1, r.state.value[e] = hr(I._hmrPayload, "hotState"), c = !0, No().then(() => {
        u = !0;
      });
      for (const N in I._hmrPayload.actions) {
        const j = I[N];
        pn(R, N, T(N, j));
      }
      for (const N in I._hmrPayload.getters) {
        const j = I._hmrPayload.getters[N], oe = a ? he(() => (An(r), j.call(R, R))) : j;
        pn(R, N, oe);
      }
      Object.keys(R._hmrPayload.getters).forEach((N) => {
        N in I._hmrPayload.getters || vr(R, N);
      }), Object.keys(R._hmrPayload.actions).forEach((N) => {
        N in I._hmrPayload.actions || vr(R, N);
      }), R._hmrPayload = I._hmrPayload, R._getters = I._getters, R._hotUpdating = !1;
    });
    const M = {
      writable: !0,
      configurable: !0,
      enumerable: !1
    };
    Wt && ["_p", "_hmrPayload", "_getters", "_customProperties"].forEach((I) => {
      Object.defineProperty(R, I, {
        value: R[I],
        ...M
      });
    });
  }
  return r._p.forEach((M) => {
    if (Wt) {
      const I = i.run(() => M({
        store: R,
        app: r._a,
        pinia: r,
        options: s
      }));
      Object.keys(I || {}).forEach((N) => R._customProperties.add(N)), Ie(R, I);
    } else
      Ie(R, i.run(() => M({
        store: R,
        app: r._a,
        pinia: r,
        options: s
      })));
  }), R.$state && typeof R.$state == "object" && typeof R.$state.constructor == "function" && !R.$state.constructor.toString().includes("[native code]") && console.warn(`[\u{1F34D}]: The "state" must be a plain object. It cannot be
	state: () => new MyClass()
Found in store "${R.$id}".`), h && a && n.hydrate && n.hydrate(R.$state, h), u = !0, c = !0, R;
}
function sn(e, t, n) {
  let r, o;
  const a = typeof t == "function";
  typeof e == "string" ? (r = e, o = a ? n : t) : (o = e, r = e.id);
  function i(s, l) {
    const u = Qa();
    if (s = s || u && Gs(Js), s && An(s), !Br)
      throw new Error(`[\u{1F34D}]: getActivePinia was called with no active Pinia. Did you forget to install pinia?
	const pinia = createPinia()
	app.use(pinia)
This will fail in production.`);
    s = Br, s._s.has(r) || (a ? $r(r, t, o, s) : Lo(r, o, s), i._pinia = s);
    const c = s._s.get(r);
    if (l) {
      const d = "__hot:" + r, f = a ? $r(d, t, o, s, !0) : Lo(d, Ie({}, o), s, !0);
      l._hotUpdate(f), delete s.state.value[d], s._s.delete(d);
    }
    if (Wt && u && u.proxy && !l) {
      const d = u.proxy, f = "_pStores" in d ? d._pStores : d._pStores = {};
      f[r] = c;
    }
    return c;
  }
  return i.$id = r, i;
}
const ar = sn("FormBuilderStore", {
  state: () => ({
    lang: ["en", "fr"],
    form: null,
    transientMessage: null,
    transientMessageClass: null
  }),
  actions: {
    loadForm(e) {
      let t = `https://localhost:5020/api/forms/${e}`;
      console.log(t), fetch(t, {
        method: "GET"
      }).then((n) => n.json()).then((n) => {
        this.form = n;
      }).catch((n) => {
        console.error("Load Form API Error:", n);
      });
    },
    saveForm() {
      var r, o;
      if (!this.form) {
        console.error("Cannot save null form.");
        return;
      }
      const e = ((o = (r = this.form) == null ? void 0 : r.id) == null ? void 0 : o.toString()) === ee.EMPTY;
      let t = "https://localhost:5020/api/forms", n = "";
      e ? (console.log("Saving new form."), this.form.id = ee.create().toString(), n = "POST") : (console.log("Updating existing form."), t = `${t}/${this.form.id}`, n = "PUT"), fetch(
        t,
        {
          body: JSON.stringify(this.form),
          method: n,
          headers: {
            encType: "multipart/form-data",
            "Content-Type": "application/json"
          }
        }
      ).then((a) => {
        if (a.ok)
          this.transientMessage = "The form saved successfully", this.transientMessageClass = "success";
        else
          switch (e && this.form && (this.form.id = ee.EMPTY), this.transientMessageClass = "danger", a.status) {
            case 400:
              this.transientMessage = "Bad request. Failed to save the form";
              break;
            case 404:
              this.transientMessage = "Form not found";
              break;
            case 500:
              this.transientMessage = "An internal server error occurred. Failed to save the form";
              break;
            default:
              this.transientMessage = "Unknown error occured. Failed to save the form";
              break;
          }
      }).catch((a) => {
        e && this.form && (this.form.id = ee.EMPTY), this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("Form Save API Error:", a);
      });
    }
  }
}), At = (e) => {
  const t = {
    id: ee.create().toString(),
    values: []
  };
  return (typeof e == "string" ? [e] : e).forEach((r) => {
    t.values.push({
      id: ee.create().toString(),
      lang: r
    });
  }), t;
}, ri = (e) => {
  const t = {
    id: ee.create().toString()
  };
  return e && (t.lang = e), t;
}, Bo = () => ee.create().toString(), oi = (e, t, n) => {
  var o, a;
  let r;
  return t ? r = (o = e == null ? void 0 : e.values) == null ? void 0 : o.filter((i) => i.lang === t).map((i) => i.value) : r = (a = e == null ? void 0 : e.values) == null ? void 0 : a.map((i) => i.value), n ? r.join(n) : r;
}, nl = (e) => {
  const t = JSON.parse(JSON.stringify(e));
  return t.id = Bo(), t.values.forEach((n) => {
    n.id = Bo();
  }), t;
};
function ai(e, t) {
  return {
    id: ee.create().toString(),
    isExtendedInput: or.None,
    optionText: t || At(e)
  };
}
const Rt = (e, t) => rl(e.optionText, t);
function rl(e, t) {
  var n, r, o;
  return t ? (r = (n = e == null ? void 0 : e.values) == null ? void 0 : n.filter((a) => a.lang === t).map((a) => a.value)) == null ? void 0 : r.at(0) : (o = e == null ? void 0 : e.values) == null ? void 0 : o.map((a) => a.value);
}
const fo = (e) => Object.values(rr).map((t) => t).includes(e.type), ii = (e) => Object.values(Ot).map((t) => t).includes(e.type), si = (e) => Object.values(nr).map((t) => t).includes(e.type), ol = (e, t) => oi(e.title, t)[0], li = (e, t) => oi(e.description, t)[0], al = (e, t) => {
  var r;
  const n = {
    id: ee.create().toString(),
    fieldId: e.id
  };
  if (fo(e))
    n.selectedOptionIds = [], e.allowCustomOptionValues && (n.customOptionValues = []), (r = e.options) != null && r.find((o) => o.isExtendedInput) && (n.extendedOptionValues = []);
  else if (ii(e)) {
    const o = typeof t == "string" ? [t] : t;
    n.multilingualTextValues = [At(o)];
  } else
    si(e) && (n.monolingualTextValues = [ri(null)]);
  return n;
}, il = (e, t) => {
  var r;
  const n = {
    id: ee.EMPTY,
    formId: e.id,
    fieldData: []
  };
  return (r = e.fields) == null || r.forEach((o) => {
    const a = al(o, t);
    n.fieldData.push(a);
  }), n;
};
/*!
  * vue-draggable-next v2.1.0
  * (c) 2021 Anish George
  * @license MIT
  */
/**!
 * Sortable 1.14.0
 * @author	RubaXa   <trash@rubaxa.org>
 * @author	owenm    <owen23355@gmail.com>
 * @license MIT
 */
function Wo(e, t) {
  var n = Object.keys(e);
  if (Object.getOwnPropertySymbols) {
    var r = Object.getOwnPropertySymbols(e);
    t && (r = r.filter(function(o) {
      return Object.getOwnPropertyDescriptor(e, o).enumerable;
    })), n.push.apply(n, r);
  }
  return n;
}
function Fe(e) {
  for (var t = 1; t < arguments.length; t++) {
    var n = arguments[t] != null ? arguments[t] : {};
    t % 2 ? Wo(Object(n), !0).forEach(function(r) {
      sl(e, r, n[r]);
    }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(e, Object.getOwnPropertyDescriptors(n)) : Wo(Object(n)).forEach(function(r) {
      Object.defineProperty(e, r, Object.getOwnPropertyDescriptor(n, r));
    });
  }
  return e;
}
function Sn(e) {
  return typeof Symbol == "function" && typeof Symbol.iterator == "symbol" ? Sn = function(t) {
    return typeof t;
  } : Sn = function(t) {
    return t && typeof Symbol == "function" && t.constructor === Symbol && t !== Symbol.prototype ? "symbol" : typeof t;
  }, Sn(e);
}
function sl(e, t, n) {
  return t in e ? Object.defineProperty(e, t, {
    value: n,
    enumerable: !0,
    configurable: !0,
    writable: !0
  }) : e[t] = n, e;
}
function $e() {
  return $e = Object.assign || function(e) {
    for (var t = 1; t < arguments.length; t++) {
      var n = arguments[t];
      for (var r in n)
        Object.prototype.hasOwnProperty.call(n, r) && (e[r] = n[r]);
    }
    return e;
  }, $e.apply(this, arguments);
}
function ll(e, t) {
  if (e == null)
    return {};
  var n = {}, r = Object.keys(e), o, a;
  for (a = 0; a < r.length; a++)
    o = r[a], !(t.indexOf(o) >= 0) && (n[o] = e[o]);
  return n;
}
function ul(e, t) {
  if (e == null)
    return {};
  var n = ll(e, t), r, o;
  if (Object.getOwnPropertySymbols) {
    var a = Object.getOwnPropertySymbols(e);
    for (o = 0; o < a.length; o++)
      r = a[o], !(t.indexOf(r) >= 0) && (!Object.prototype.propertyIsEnumerable.call(e, r) || (n[r] = e[r]));
  }
  return n;
}
var cl = "1.14.0";
function We(e) {
  if (typeof window < "u" && window.navigator)
    return !!/* @__PURE__ */ navigator.userAgent.match(e);
}
var je = We(/(?:Trident.*rv[ :]?11\.|msie|iemobile|Windows Phone)/i), ln = We(/Edge/i), $o = We(/firefox/i), Yt = We(/safari/i) && !We(/chrome/i) && !We(/android/i), ui = We(/iP(ad|od|hone)/i), dl = We(/chrome/i) && We(/android/i), ci = {
  capture: !1,
  passive: !1
};
function G(e, t, n) {
  e.addEventListener(t, n, !je && ci);
}
function $(e, t, n) {
  e.removeEventListener(t, n, !je && ci);
}
function Nn(e, t) {
  if (!!t) {
    if (t[0] === ">" && (t = t.substring(1)), e)
      try {
        if (e.matches)
          return e.matches(t);
        if (e.msMatchesSelector)
          return e.msMatchesSelector(t);
        if (e.webkitMatchesSelector)
          return e.webkitMatchesSelector(t);
      } catch {
        return !1;
      }
    return !1;
  }
}
function fl(e) {
  return e.host && e !== document && e.host.nodeType ? e.host : e.parentNode;
}
function Pe(e, t, n, r) {
  if (e) {
    n = n || document;
    do {
      if (t != null && (t[0] === ">" ? e.parentNode === n && Nn(e, t) : Nn(e, t)) || r && e === n)
        return e;
      if (e === n)
        break;
    } while (e = fl(e));
  }
  return null;
}
var zo = /\s+/g;
function De(e, t, n) {
  if (e && t)
    if (e.classList)
      e.classList[n ? "add" : "remove"](t);
    else {
      var r = (" " + e.className + " ").replace(zo, " ").replace(" " + t + " ", " ");
      e.className = (r + (n ? " " + t : "")).replace(zo, " ");
    }
}
function F(e, t, n) {
  var r = e && e.style;
  if (r) {
    if (n === void 0)
      return document.defaultView && document.defaultView.getComputedStyle ? n = document.defaultView.getComputedStyle(e, "") : e.currentStyle && (n = e.currentStyle), t === void 0 ? n : n[t];
    !(t in r) && t.indexOf("webkit") === -1 && (t = "-webkit-" + t), r[t] = n + (typeof n == "string" ? "" : "px");
  }
}
function kt(e, t) {
  var n = "";
  if (typeof e == "string")
    n = e;
  else
    do {
      var r = F(e, "transform");
      r && r !== "none" && (n = r + " " + n);
    } while (!t && (e = e.parentNode));
  var o = window.DOMMatrix || window.WebKitCSSMatrix || window.CSSMatrix || window.MSCSSMatrix;
  return o && new o(n);
}
function di(e, t, n) {
  if (e) {
    var r = e.getElementsByTagName(t), o = 0, a = r.length;
    if (n)
      for (; o < a; o++)
        n(r[o], o);
    return r;
  }
  return [];
}
function Ne() {
  var e = document.scrollingElement;
  return e || document.documentElement;
}
function ie(e, t, n, r, o) {
  if (!(!e.getBoundingClientRect && e !== window)) {
    var a, i, s, l, u, c, d;
    if (e !== window && e.parentNode && e !== Ne() ? (a = e.getBoundingClientRect(), i = a.top, s = a.left, l = a.bottom, u = a.right, c = a.height, d = a.width) : (i = 0, s = 0, l = window.innerHeight, u = window.innerWidth, c = window.innerHeight, d = window.innerWidth), (t || n) && e !== window && (o = o || e.parentNode, !je))
      do
        if (o && o.getBoundingClientRect && (F(o, "transform") !== "none" || n && F(o, "position") !== "static")) {
          var f = o.getBoundingClientRect();
          i -= f.top + parseInt(F(o, "border-top-width")), s -= f.left + parseInt(F(o, "border-left-width")), l = i + a.height, u = s + a.width;
          break;
        }
      while (o = o.parentNode);
    if (r && e !== window) {
      var p = kt(o || e), h = p && p.a, b = p && p.d;
      p && (i /= b, s /= h, d /= h, c /= b, l = i + c, u = s + d);
    }
    return {
      top: i,
      left: s,
      bottom: l,
      right: u,
      width: d,
      height: c
    };
  }
}
function Go(e, t, n) {
  for (var r = Ke(e, !0), o = ie(e)[t]; r; ) {
    var a = ie(r)[n], i = void 0;
    if (n === "top" || n === "left" ? i = o >= a : i = o <= a, !i)
      return r;
    if (r === Ne())
      break;
    r = Ke(r, !1);
  }
  return !1;
}
function Nt(e, t, n, r) {
  for (var o = 0, a = 0, i = e.children; a < i.length; ) {
    if (i[a].style.display !== "none" && i[a] !== V.ghost && (r || i[a] !== V.dragged) && Pe(i[a], n.draggable, e, !1)) {
      if (o === t)
        return i[a];
      o++;
    }
    a++;
  }
  return null;
}
function po(e, t) {
  for (var n = e.lastElementChild; n && (n === V.ghost || F(n, "display") === "none" || t && !Nn(n, t)); )
    n = n.previousElementSibling;
  return n || null;
}
function Te(e, t) {
  var n = 0;
  if (!e || !e.parentNode)
    return -1;
  for (; e = e.previousElementSibling; )
    e.nodeName.toUpperCase() !== "TEMPLATE" && e !== V.clone && (!t || Nn(e, t)) && n++;
  return n;
}
function jo(e) {
  var t = 0, n = 0, r = Ne();
  if (e)
    do {
      var o = kt(e), a = o.a, i = o.d;
      t += e.scrollLeft * a, n += e.scrollTop * i;
    } while (e !== r && (e = e.parentNode));
  return [t, n];
}
function pl(e, t) {
  for (var n in e)
    if (!!e.hasOwnProperty(n)) {
      for (var r in t)
        if (t.hasOwnProperty(r) && t[r] === e[n][r])
          return Number(n);
    }
  return -1;
}
function Ke(e, t) {
  if (!e || !e.getBoundingClientRect)
    return Ne();
  var n = e, r = !1;
  do
    if (n.clientWidth < n.scrollWidth || n.clientHeight < n.scrollHeight) {
      var o = F(n);
      if (n.clientWidth < n.scrollWidth && (o.overflowX == "auto" || o.overflowX == "scroll") || n.clientHeight < n.scrollHeight && (o.overflowY == "auto" || o.overflowY == "scroll")) {
        if (!n.getBoundingClientRect || n === document.body)
          return Ne();
        if (r || t)
          return n;
        r = !0;
      }
    }
  while (n = n.parentNode);
  return Ne();
}
function hl(e, t) {
  if (e && t)
    for (var n in t)
      t.hasOwnProperty(n) && (e[n] = t[n]);
  return e;
}
function mr(e, t) {
  return Math.round(e.top) === Math.round(t.top) && Math.round(e.left) === Math.round(t.left) && Math.round(e.height) === Math.round(t.height) && Math.round(e.width) === Math.round(t.width);
}
var qt;
function fi(e, t) {
  return function() {
    if (!qt) {
      var n = arguments, r = this;
      n.length === 1 ? e.call(r, n[0]) : e.apply(r, n), qt = setTimeout(function() {
        qt = void 0;
      }, t);
    }
  };
}
function vl() {
  clearTimeout(qt), qt = void 0;
}
function pi(e, t, n) {
  e.scrollLeft += t, e.scrollTop += n;
}
function hi(e) {
  var t = window.Polymer, n = window.jQuery || window.Zepto;
  return t && t.dom ? t.dom(e).cloneNode(!0) : n ? n(e).clone(!0)[0] : e.cloneNode(!0);
}
var we = "Sortable" + new Date().getTime();
function ml() {
  var e = [], t;
  return {
    captureAnimationState: function() {
      if (e = [], !!this.options.animation) {
        var r = [].slice.call(this.el.children);
        r.forEach(function(o) {
          if (!(F(o, "display") === "none" || o === V.ghost)) {
            e.push({
              target: o,
              rect: ie(o)
            });
            var a = Fe({}, e[e.length - 1].rect);
            if (o.thisAnimationDuration) {
              var i = kt(o, !0);
              i && (a.top -= i.f, a.left -= i.e);
            }
            o.fromRect = a;
          }
        });
      }
    },
    addAnimationState: function(r) {
      e.push(r);
    },
    removeAnimationState: function(r) {
      e.splice(pl(e, {
        target: r
      }), 1);
    },
    animateAll: function(r) {
      var o = this;
      if (!this.options.animation) {
        clearTimeout(t), typeof r == "function" && r();
        return;
      }
      var a = !1, i = 0;
      e.forEach(function(s) {
        var l = 0, u = s.target, c = u.fromRect, d = ie(u), f = u.prevFromRect, p = u.prevToRect, h = s.rect, b = kt(u, !0);
        b && (d.top -= b.f, d.left -= b.e), u.toRect = d, u.thisAnimationDuration && mr(f, d) && !mr(c, d) && (h.top - d.top) / (h.left - d.left) === (c.top - d.top) / (c.left - d.left) && (l = yl(h, f, p, o.options)), mr(d, c) || (u.prevFromRect = c, u.prevToRect = d, l || (l = o.options.animation), o.animate(u, h, d, l)), l && (a = !0, i = Math.max(i, l), clearTimeout(u.animationResetTimer), u.animationResetTimer = setTimeout(function() {
          u.animationTime = 0, u.prevFromRect = null, u.fromRect = null, u.prevToRect = null, u.thisAnimationDuration = null;
        }, l), u.thisAnimationDuration = l);
      }), clearTimeout(t), a ? t = setTimeout(function() {
        typeof r == "function" && r();
      }, i) : typeof r == "function" && r(), e = [];
    },
    animate: function(r, o, a, i) {
      if (i) {
        F(r, "transition", ""), F(r, "transform", "");
        var s = kt(this.el), l = s && s.a, u = s && s.d, c = (o.left - a.left) / (l || 1), d = (o.top - a.top) / (u || 1);
        r.animatingX = !!c, r.animatingY = !!d, F(r, "transform", "translate3d(" + c + "px," + d + "px,0)"), this.forRepaintDummy = gl(r), F(r, "transition", "transform " + i + "ms" + (this.options.easing ? " " + this.options.easing : "")), F(r, "transform", "translate3d(0,0,0)"), typeof r.animated == "number" && clearTimeout(r.animated), r.animated = setTimeout(function() {
          F(r, "transition", ""), F(r, "transform", ""), r.animated = !1, r.animatingX = !1, r.animatingY = !1;
        }, i);
      }
    }
  };
}
function gl(e) {
  return e.offsetWidth;
}
function yl(e, t, n, r) {
  return Math.sqrt(Math.pow(t.top - e.top, 2) + Math.pow(t.left - e.left, 2)) / Math.sqrt(Math.pow(t.top - n.top, 2) + Math.pow(t.left - n.left, 2)) * r.animation;
}
var _t = [], gr = {
  initializeByDefault: !0
}, un = {
  mount: function(t) {
    for (var n in gr)
      gr.hasOwnProperty(n) && !(n in t) && (t[n] = gr[n]);
    _t.forEach(function(r) {
      if (r.pluginName === t.pluginName)
        throw "Sortable: Cannot mount plugin ".concat(t.pluginName, " more than once");
    }), _t.push(t);
  },
  pluginEvent: function(t, n, r) {
    var o = this;
    this.eventCanceled = !1, r.cancel = function() {
      o.eventCanceled = !0;
    };
    var a = t + "Global";
    _t.forEach(function(i) {
      !n[i.pluginName] || (n[i.pluginName][a] && n[i.pluginName][a](Fe({
        sortable: n
      }, r)), n.options[i.pluginName] && n[i.pluginName][t] && n[i.pluginName][t](Fe({
        sortable: n
      }, r)));
    });
  },
  initializePlugins: function(t, n, r, o) {
    _t.forEach(function(s) {
      var l = s.pluginName;
      if (!(!t.options[l] && !s.initializeByDefault)) {
        var u = new s(t, n, t.options);
        u.sortable = t, u.options = t.options, t[l] = u, $e(r, u.defaults);
      }
    });
    for (var a in t.options)
      if (!!t.options.hasOwnProperty(a)) {
        var i = this.modifyOption(t, a, t.options[a]);
        typeof i < "u" && (t.options[a] = i);
      }
  },
  getEventProperties: function(t, n) {
    var r = {};
    return _t.forEach(function(o) {
      typeof o.eventProperties == "function" && $e(r, o.eventProperties.call(n[o.pluginName], t));
    }), r;
  },
  modifyOption: function(t, n, r) {
    var o;
    return _t.forEach(function(a) {
      !t[a.pluginName] || a.optionListeners && typeof a.optionListeners[n] == "function" && (o = a.optionListeners[n].call(t[a.pluginName], r));
    }), o;
  }
};
function bl(e) {
  var t = e.sortable, n = e.rootEl, r = e.name, o = e.targetEl, a = e.cloneEl, i = e.toEl, s = e.fromEl, l = e.oldIndex, u = e.newIndex, c = e.oldDraggableIndex, d = e.newDraggableIndex, f = e.originalEvent, p = e.putSortable, h = e.extraEventProperties;
  if (t = t || n && n[we], !!t) {
    var b, E = t.options, y = "on" + r.charAt(0).toUpperCase() + r.substr(1);
    window.CustomEvent && !je && !ln ? b = new CustomEvent(r, {
      bubbles: !0,
      cancelable: !0
    }) : (b = document.createEvent("Event"), b.initEvent(r, !0, !0)), b.to = i || n, b.from = s || n, b.item = o || n, b.clone = a, b.oldIndex = l, b.newIndex = u, b.oldDraggableIndex = c, b.newDraggableIndex = d, b.originalEvent = f, b.pullMode = p ? p.lastPutMode : void 0;
    var D = Fe(Fe({}, h), un.getEventProperties(r, t));
    for (var k in D)
      b[k] = D[k];
    n && n.dispatchEvent(b), E[y] && E[y].call(t, b);
  }
}
var El = ["evt"], be = function(t, n) {
  var r = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : {}, o = r.evt, a = ul(r, El);
  un.pluginEvent.bind(V)(t, n, Fe({
    dragEl: w,
    parentEl: te,
    ghostEl: L,
    rootEl: J,
    nextEl: ut,
    lastDownEl: wn,
    cloneEl: ne,
    cloneHidden: Xe,
    dragStarted: $t,
    putSortable: de,
    activeSortable: V.active,
    originalEvent: o,
    oldIndex: Tt,
    oldDraggableIndex: Zt,
    newIndex: Se,
    newDraggableIndex: Ze,
    hideGhostForTarget: yi,
    unhideGhostForTarget: bi,
    cloneNowHidden: function() {
      Xe = !0;
    },
    cloneNowShown: function() {
      Xe = !1;
    },
    dispatchSortableEvent: function(s) {
      ve({
        sortable: n,
        name: s,
        originalEvent: o
      });
    }
  }, a));
};
function ve(e) {
  bl(Fe({
    putSortable: de,
    cloneEl: ne,
    targetEl: w,
    rootEl: J,
    oldIndex: Tt,
    oldDraggableIndex: Zt,
    newIndex: Se,
    newDraggableIndex: Ze
  }, e));
}
var w, te, L, J, ut, wn, ne, Xe, Tt, Se, Zt, Ze, hn, de, St = !1, Hn = !1, Fn = [], it, ke, yr, br, Yo, qo, $t, Ct, Xt, Kt = !1, vn = !1, Tn, fe, Er = [], zr = !1, Vn = [], ir = typeof document < "u", mn = ui, Zo = ln || je ? "cssFloat" : "float", _l = ir && !dl && !ui && "draggable" in document.createElement("div"), vi = function() {
  if (!!ir) {
    if (je)
      return !1;
    var e = document.createElement("x");
    return e.style.cssText = "pointer-events:auto", e.style.pointerEvents === "auto";
  }
}(), mi = function(t, n) {
  var r = F(t), o = parseInt(r.width) - parseInt(r.paddingLeft) - parseInt(r.paddingRight) - parseInt(r.borderLeftWidth) - parseInt(r.borderRightWidth), a = Nt(t, 0, n), i = Nt(t, 1, n), s = a && F(a), l = i && F(i), u = s && parseInt(s.marginLeft) + parseInt(s.marginRight) + ie(a).width, c = l && parseInt(l.marginLeft) + parseInt(l.marginRight) + ie(i).width;
  if (r.display === "flex")
    return r.flexDirection === "column" || r.flexDirection === "column-reverse" ? "vertical" : "horizontal";
  if (r.display === "grid")
    return r.gridTemplateColumns.split(" ").length <= 1 ? "vertical" : "horizontal";
  if (a && s.float && s.float !== "none") {
    var d = s.float === "left" ? "left" : "right";
    return i && (l.clear === "both" || l.clear === d) ? "vertical" : "horizontal";
  }
  return a && (s.display === "block" || s.display === "flex" || s.display === "table" || s.display === "grid" || u >= o && r[Zo] === "none" || i && r[Zo] === "none" && u + c > o) ? "vertical" : "horizontal";
}, Cl = function(t, n, r) {
  var o = r ? t.left : t.top, a = r ? t.right : t.bottom, i = r ? t.width : t.height, s = r ? n.left : n.top, l = r ? n.right : n.bottom, u = r ? n.width : n.height;
  return o === s || a === l || o + i / 2 === s + u / 2;
}, Dl = function(t, n) {
  var r;
  return Fn.some(function(o) {
    var a = o[we].options.emptyInsertThreshold;
    if (!(!a || po(o))) {
      var i = ie(o), s = t >= i.left - a && t <= i.right + a, l = n >= i.top - a && n <= i.bottom + a;
      if (s && l)
        return r = o;
    }
  }), r;
}, gi = function(t) {
  function n(a, i) {
    return function(s, l, u, c) {
      var d = s.options.group.name && l.options.group.name && s.options.group.name === l.options.group.name;
      if (a == null && (i || d))
        return !0;
      if (a == null || a === !1)
        return !1;
      if (i && a === "clone")
        return a;
      if (typeof a == "function")
        return n(a(s, l, u, c), i)(s, l, u, c);
      var f = (i ? s : l).options.group.name;
      return a === !0 || typeof a == "string" && a === f || a.join && a.indexOf(f) > -1;
    };
  }
  var r = {}, o = t.group;
  (!o || Sn(o) != "object") && (o = {
    name: o
  }), r.name = o.name, r.checkPull = n(o.pull, !0), r.checkPut = n(o.put), r.revertClone = o.revertClone, t.group = r;
}, yi = function() {
  !vi && L && F(L, "display", "none");
}, bi = function() {
  !vi && L && F(L, "display", "");
};
ir && document.addEventListener("click", function(e) {
  if (Hn)
    return e.preventDefault(), e.stopPropagation && e.stopPropagation(), e.stopImmediatePropagation && e.stopImmediatePropagation(), Hn = !1, !1;
}, !0);
var st = function(t) {
  if (w) {
    t = t.touches ? t.touches[0] : t;
    var n = Dl(t.clientX, t.clientY);
    if (n) {
      var r = {};
      for (var o in t)
        t.hasOwnProperty(o) && (r[o] = t[o]);
      r.target = r.rootEl = n, r.preventDefault = void 0, r.stopPropagation = void 0, n[we]._onDragOver(r);
    }
  }
}, Sl = function(t) {
  w && w.parentNode[we]._isOutsideThisEl(t.target);
};
function V(e, t) {
  if (!(e && e.nodeType && e.nodeType === 1))
    throw "Sortable: `el` must be an HTMLElement, not ".concat({}.toString.call(e));
  this.el = e, this.options = t = $e({}, t), e[we] = this;
  var n = {
    group: null,
    sort: !0,
    disabled: !1,
    store: null,
    handle: null,
    draggable: /^[uo]l$/i.test(e.nodeName) ? ">li" : ">*",
    swapThreshold: 1,
    invertSwap: !1,
    invertedSwapThreshold: null,
    removeCloneOnHide: !0,
    direction: function() {
      return mi(e, this.options);
    },
    ghostClass: "sortable-ghost",
    chosenClass: "sortable-chosen",
    dragClass: "sortable-drag",
    ignore: "a, img",
    filter: null,
    preventOnFilter: !0,
    animation: 0,
    easing: null,
    setData: function(i, s) {
      i.setData("Text", s.textContent);
    },
    dropBubble: !1,
    dragoverBubble: !1,
    dataIdAttr: "data-id",
    delay: 0,
    delayOnTouchOnly: !1,
    touchStartThreshold: (Number.parseInt ? Number : window).parseInt(window.devicePixelRatio, 10) || 1,
    forceFallback: !1,
    fallbackClass: "sortable-fallback",
    fallbackOnBody: !1,
    fallbackTolerance: 0,
    fallbackOffset: {
      x: 0,
      y: 0
    },
    supportPointer: V.supportPointer !== !1 && "PointerEvent" in window && !Yt,
    emptyInsertThreshold: 5
  };
  un.initializePlugins(this, e, n);
  for (var r in n)
    !(r in t) && (t[r] = n[r]);
  gi(t);
  for (var o in this)
    o.charAt(0) === "_" && typeof this[o] == "function" && (this[o] = this[o].bind(this));
  this.nativeDraggable = t.forceFallback ? !1 : _l, this.nativeDraggable && (this.options.touchStartThreshold = 1), t.supportPointer ? G(e, "pointerdown", this._onTapStart) : (G(e, "mousedown", this._onTapStart), G(e, "touchstart", this._onTapStart)), this.nativeDraggable && (G(e, "dragover", this), G(e, "dragenter", this)), Fn.push(this.el), t.store && t.store.get && this.sort(t.store.get(this) || []), $e(this, ml());
}
V.prototype = {
  constructor: V,
  _isOutsideThisEl: function(t) {
    !this.el.contains(t) && t !== this.el && (Ct = null);
  },
  _getDirection: function(t, n) {
    return typeof this.options.direction == "function" ? this.options.direction.call(this, t, n, w) : this.options.direction;
  },
  _onTapStart: function(t) {
    if (!!t.cancelable) {
      var n = this, r = this.el, o = this.options, a = o.preventOnFilter, i = t.type, s = t.touches && t.touches[0] || t.pointerType && t.pointerType === "touch" && t, l = (s || t).target, u = t.target.shadowRoot && (t.path && t.path[0] || t.composedPath && t.composedPath()[0]) || l, c = o.filter;
      if (Pl(r), !w && !(/mousedown|pointerdown/.test(i) && t.button !== 0 || o.disabled) && !u.isContentEditable && !(!this.nativeDraggable && Yt && l && l.tagName.toUpperCase() === "SELECT") && (l = Pe(l, o.draggable, r, !1), !(l && l.animated) && wn !== l)) {
        if (Tt = Te(l), Zt = Te(l, o.draggable), typeof c == "function") {
          if (c.call(this, t, l, this)) {
            ve({
              sortable: n,
              rootEl: u,
              name: "filter",
              targetEl: l,
              toEl: r,
              fromEl: r
            }), be("filter", n, {
              evt: t
            }), a && t.cancelable && t.preventDefault();
            return;
          }
        } else if (c && (c = c.split(",").some(function(d) {
          if (d = Pe(u, d.trim(), r, !1), d)
            return ve({
              sortable: n,
              rootEl: d,
              name: "filter",
              targetEl: l,
              fromEl: r,
              toEl: r
            }), be("filter", n, {
              evt: t
            }), !0;
        }), c)) {
          a && t.cancelable && t.preventDefault();
          return;
        }
        o.handle && !Pe(u, o.handle, r, !1) || this._prepareDragStart(t, s, l);
      }
    }
  },
  _prepareDragStart: function(t, n, r) {
    var o = this, a = o.el, i = o.options, s = a.ownerDocument, l;
    if (r && !w && r.parentNode === a) {
      var u = ie(r);
      if (J = a, w = r, te = w.parentNode, ut = w.nextSibling, wn = r, hn = i.group, V.dragged = w, it = {
        target: w,
        clientX: (n || t).clientX,
        clientY: (n || t).clientY
      }, Yo = it.clientX - u.left, qo = it.clientY - u.top, this._lastX = (n || t).clientX, this._lastY = (n || t).clientY, w.style["will-change"] = "all", l = function() {
        if (be("delayEnded", o, {
          evt: t
        }), V.eventCanceled) {
          o._onDrop();
          return;
        }
        o._disableDelayedDragEvents(), !$o && o.nativeDraggable && (w.draggable = !0), o._triggerDragStart(t, n), ve({
          sortable: o,
          name: "choose",
          originalEvent: t
        }), De(w, i.chosenClass, !0);
      }, i.ignore.split(",").forEach(function(c) {
        di(w, c.trim(), _r);
      }), G(s, "dragover", st), G(s, "mousemove", st), G(s, "touchmove", st), G(s, "mouseup", o._onDrop), G(s, "touchend", o._onDrop), G(s, "touchcancel", o._onDrop), $o && this.nativeDraggable && (this.options.touchStartThreshold = 4, w.draggable = !0), be("delayStart", this, {
        evt: t
      }), i.delay && (!i.delayOnTouchOnly || n) && (!this.nativeDraggable || !(ln || je))) {
        if (V.eventCanceled) {
          this._onDrop();
          return;
        }
        G(s, "mouseup", o._disableDelayedDrag), G(s, "touchend", o._disableDelayedDrag), G(s, "touchcancel", o._disableDelayedDrag), G(s, "mousemove", o._delayedDragTouchMoveHandler), G(s, "touchmove", o._delayedDragTouchMoveHandler), i.supportPointer && G(s, "pointermove", o._delayedDragTouchMoveHandler), o._dragStartTimer = setTimeout(l, i.delay);
      } else
        l();
    }
  },
  _delayedDragTouchMoveHandler: function(t) {
    var n = t.touches ? t.touches[0] : t;
    Math.max(Math.abs(n.clientX - this._lastX), Math.abs(n.clientY - this._lastY)) >= Math.floor(this.options.touchStartThreshold / (this.nativeDraggable && window.devicePixelRatio || 1)) && this._disableDelayedDrag();
  },
  _disableDelayedDrag: function() {
    w && _r(w), clearTimeout(this._dragStartTimer), this._disableDelayedDragEvents();
  },
  _disableDelayedDragEvents: function() {
    var t = this.el.ownerDocument;
    $(t, "mouseup", this._disableDelayedDrag), $(t, "touchend", this._disableDelayedDrag), $(t, "touchcancel", this._disableDelayedDrag), $(t, "mousemove", this._delayedDragTouchMoveHandler), $(t, "touchmove", this._delayedDragTouchMoveHandler), $(t, "pointermove", this._delayedDragTouchMoveHandler);
  },
  _triggerDragStart: function(t, n) {
    n = n || t.pointerType == "touch" && t, !this.nativeDraggable || n ? this.options.supportPointer ? G(document, "pointermove", this._onTouchMove) : n ? G(document, "touchmove", this._onTouchMove) : G(document, "mousemove", this._onTouchMove) : (G(w, "dragend", this), G(J, "dragstart", this._onDragStart));
    try {
      document.selection ? Rn(function() {
        document.selection.empty();
      }) : window.getSelection().removeAllRanges();
    } catch {
    }
  },
  _dragStarted: function(t, n) {
    if (St = !1, J && w) {
      be("dragStarted", this, {
        evt: n
      }), this.nativeDraggable && G(document, "dragover", Sl);
      var r = this.options;
      !t && De(w, r.dragClass, !1), De(w, r.ghostClass, !0), V.active = this, t && this._appendGhost(), ve({
        sortable: this,
        name: "start",
        originalEvent: n
      });
    } else
      this._nulling();
  },
  _emulateDragOver: function() {
    if (ke) {
      this._lastX = ke.clientX, this._lastY = ke.clientY, yi();
      for (var t = document.elementFromPoint(ke.clientX, ke.clientY), n = t; t && t.shadowRoot && (t = t.shadowRoot.elementFromPoint(ke.clientX, ke.clientY), t !== n); )
        n = t;
      if (w.parentNode[we]._isOutsideThisEl(t), n)
        do {
          if (n[we]) {
            var r = void 0;
            if (r = n[we]._onDragOver({
              clientX: ke.clientX,
              clientY: ke.clientY,
              target: t,
              rootEl: n
            }), r && !this.options.dragoverBubble)
              break;
          }
          t = n;
        } while (n = n.parentNode);
      bi();
    }
  },
  _onTouchMove: function(t) {
    if (it) {
      var n = this.options, r = n.fallbackTolerance, o = n.fallbackOffset, a = t.touches ? t.touches[0] : t, i = L && kt(L, !0), s = L && i && i.a, l = L && i && i.d, u = mn && fe && jo(fe), c = (a.clientX - it.clientX + o.x) / (s || 1) + (u ? u[0] - Er[0] : 0) / (s || 1), d = (a.clientY - it.clientY + o.y) / (l || 1) + (u ? u[1] - Er[1] : 0) / (l || 1);
      if (!V.active && !St) {
        if (r && Math.max(Math.abs(a.clientX - this._lastX), Math.abs(a.clientY - this._lastY)) < r)
          return;
        this._onDragStart(t, !0);
      }
      if (L) {
        i ? (i.e += c - (yr || 0), i.f += d - (br || 0)) : i = {
          a: 1,
          b: 0,
          c: 0,
          d: 1,
          e: c,
          f: d
        };
        var f = "matrix(".concat(i.a, ",").concat(i.b, ",").concat(i.c, ",").concat(i.d, ",").concat(i.e, ",").concat(i.f, ")");
        F(L, "webkitTransform", f), F(L, "mozTransform", f), F(L, "msTransform", f), F(L, "transform", f), yr = c, br = d, ke = a;
      }
      t.cancelable && t.preventDefault();
    }
  },
  _appendGhost: function() {
    if (!L) {
      var t = this.options.fallbackOnBody ? document.body : J, n = ie(w, !0, mn, !0, t), r = this.options;
      if (mn) {
        for (fe = t; F(fe, "position") === "static" && F(fe, "transform") === "none" && fe !== document; )
          fe = fe.parentNode;
        fe !== document.body && fe !== document.documentElement ? (fe === document && (fe = Ne()), n.top += fe.scrollTop, n.left += fe.scrollLeft) : fe = Ne(), Er = jo(fe);
      }
      L = w.cloneNode(!0), De(L, r.ghostClass, !1), De(L, r.fallbackClass, !0), De(L, r.dragClass, !0), F(L, "transition", ""), F(L, "transform", ""), F(L, "box-sizing", "border-box"), F(L, "margin", 0), F(L, "top", n.top), F(L, "left", n.left), F(L, "width", n.width), F(L, "height", n.height), F(L, "opacity", "0.8"), F(L, "position", mn ? "absolute" : "fixed"), F(L, "zIndex", "100000"), F(L, "pointerEvents", "none"), V.ghost = L, t.appendChild(L), F(L, "transform-origin", Yo / parseInt(L.style.width) * 100 + "% " + qo / parseInt(L.style.height) * 100 + "%");
    }
  },
  _onDragStart: function(t, n) {
    var r = this, o = t.dataTransfer, a = r.options;
    if (be("dragStart", this, {
      evt: t
    }), V.eventCanceled) {
      this._onDrop();
      return;
    }
    be("setupClone", this), V.eventCanceled || (ne = hi(w), ne.draggable = !1, ne.style["will-change"] = "", this._hideClone(), De(ne, this.options.chosenClass, !1), V.clone = ne), r.cloneId = Rn(function() {
      be("clone", r), !V.eventCanceled && (r.options.removeCloneOnHide || J.insertBefore(ne, w), r._hideClone(), ve({
        sortable: r,
        name: "clone"
      }));
    }), !n && De(w, a.dragClass, !0), n ? (Hn = !0, r._loopId = setInterval(r._emulateDragOver, 50)) : ($(document, "mouseup", r._onDrop), $(document, "touchend", r._onDrop), $(document, "touchcancel", r._onDrop), o && (o.effectAllowed = "move", a.setData && a.setData.call(r, o, w)), G(document, "drop", r), F(w, "transform", "translateZ(0)")), St = !0, r._dragStartId = Rn(r._dragStarted.bind(r, n, t)), G(document, "selectstart", r), $t = !0, Yt && F(document.body, "user-select", "none");
  },
  _onDragOver: function(t) {
    var n = this.el, r = t.target, o, a, i, s = this.options, l = s.group, u = V.active, c = hn === l, d = s.sort, f = de || u, p, h = this, b = !1;
    if (zr)
      return;
    function E(Bt, $s) {
      be(Bt, h, Fe({
        evt: t,
        isOwner: c,
        axis: p ? "vertical" : "horizontal",
        revert: i,
        dragRect: o,
        targetRect: a,
        canSort: d,
        fromSortable: f,
        target: r,
        completed: D,
        onMove: function(Ao, zs) {
          return gn(J, n, w, o, Ao, ie(Ao), t, zs);
        },
        changed: k
      }, $s));
    }
    function y() {
      E("dragOverAnimationCapture"), h.captureAnimationState(), h !== f && f.captureAnimationState();
    }
    function D(Bt) {
      return E("dragOverCompleted", {
        insertion: Bt
      }), Bt && (c ? u._hideClone() : u._showClone(h), h !== f && (De(w, de ? de.options.ghostClass : u.options.ghostClass, !1), De(w, s.ghostClass, !0)), de !== h && h !== V.active ? de = h : h === V.active && de && (de = null), f === h && (h._ignoreWhileAnimating = r), h.animateAll(function() {
        E("dragOverAnimationComplete"), h._ignoreWhileAnimating = null;
      }), h !== f && (f.animateAll(), f._ignoreWhileAnimating = null)), (r === w && !w.animated || r === n && !r.animated) && (Ct = null), !s.dragoverBubble && !t.rootEl && r !== document && (w.parentNode[we]._isOutsideThisEl(t.target), !Bt && st(t)), !s.dragoverBubble && t.stopPropagation && t.stopPropagation(), b = !0;
    }
    function k() {
      Se = Te(w), Ze = Te(w, s.draggable), ve({
        sortable: h,
        name: "change",
        toEl: n,
        newIndex: Se,
        newDraggableIndex: Ze,
        originalEvent: t
      });
    }
    if (t.preventDefault !== void 0 && t.cancelable && t.preventDefault(), r = Pe(r, s.draggable, n, !0), E("dragOver"), V.eventCanceled)
      return b;
    if (w.contains(t.target) || r.animated && r.animatingX && r.animatingY || h._ignoreWhileAnimating === r)
      return D(!1);
    if (Hn = !1, u && !s.disabled && (c ? d || (i = te !== J) : de === this || (this.lastPutMode = hn.checkPull(this, u, w, t)) && l.checkPut(this, u, w, t))) {
      if (p = this._getDirection(t, r) === "vertical", o = ie(w), E("dragOverValid"), V.eventCanceled)
        return b;
      if (i)
        return te = J, y(), this._hideClone(), E("revert"), V.eventCanceled || (ut ? J.insertBefore(w, ut) : J.appendChild(w)), D(!0);
      var T = po(n, s.draggable);
      if (!T || kl(t, p, this) && !T.animated) {
        if (T === w)
          return D(!1);
        if (T && n === t.target && (r = T), r && (a = ie(r)), gn(J, n, w, o, r, a, t, !!r) !== !1)
          return y(), n.appendChild(w), te = n, k(), D(!0);
      } else if (T && Rl(t, p, this)) {
        var U = Nt(n, 0, s, !0);
        if (U === w)
          return D(!1);
        if (r = U, a = ie(r), gn(J, n, w, o, r, a, t, !1) !== !1)
          return y(), n.insertBefore(w, U), te = n, k(), D(!0);
      } else if (r.parentNode === n) {
        a = ie(r);
        var O = 0, R, A = w.parentNode !== n, M = !Cl(w.animated && w.toRect || o, r.animated && r.toRect || a, p), I = p ? "top" : "left", N = Go(r, "top", "top") || Go(w, "top", "top"), j = N ? N.scrollTop : void 0;
        Ct !== r && (R = a[I], Kt = !1, vn = !M && s.invertSwap || A), O = Il(t, r, a, p, M ? 1 : s.swapThreshold, s.invertedSwapThreshold == null ? s.swapThreshold : s.invertedSwapThreshold, vn, Ct === r);
        var oe;
        if (O !== 0) {
          var Ue = Te(w);
          do
            Ue -= O, oe = te.children[Ue];
          while (oe && (F(oe, "display") === "none" || oe === L));
        }
        if (O === 0 || oe === r)
          return D(!1);
        Ct = r, Xt = O;
        var at = r.nextElementSibling, ye = !1;
        ye = O === 1;
        var ce = gn(J, n, w, o, r, a, t, ye);
        if (ce !== !1)
          return (ce === 1 || ce === -1) && (ye = ce === 1), zr = !0, setTimeout(Tl, 30), y(), ye && !at ? n.appendChild(w) : r.parentNode.insertBefore(w, ye ? at : r), N && pi(N, 0, j - N.scrollTop), te = w.parentNode, R !== void 0 && !vn && (Tn = Math.abs(R - ie(r)[I])), k(), D(!0);
      }
      if (n.contains(w))
        return D(!1);
    }
    return !1;
  },
  _ignoreWhileAnimating: null,
  _offMoveEvents: function() {
    $(document, "mousemove", this._onTouchMove), $(document, "touchmove", this._onTouchMove), $(document, "pointermove", this._onTouchMove), $(document, "dragover", st), $(document, "mousemove", st), $(document, "touchmove", st);
  },
  _offUpEvents: function() {
    var t = this.el.ownerDocument;
    $(t, "mouseup", this._onDrop), $(t, "touchend", this._onDrop), $(t, "pointerup", this._onDrop), $(t, "touchcancel", this._onDrop), $(document, "selectstart", this);
  },
  _onDrop: function(t) {
    var n = this.el, r = this.options;
    if (Se = Te(w), Ze = Te(w, r.draggable), be("drop", this, {
      evt: t
    }), te = w && w.parentNode, Se = Te(w), Ze = Te(w, r.draggable), V.eventCanceled) {
      this._nulling();
      return;
    }
    St = !1, vn = !1, Kt = !1, clearInterval(this._loopId), clearTimeout(this._dragStartTimer), Gr(this.cloneId), Gr(this._dragStartId), this.nativeDraggable && ($(document, "drop", this), $(n, "dragstart", this._onDragStart)), this._offMoveEvents(), this._offUpEvents(), Yt && F(document.body, "user-select", ""), F(w, "transform", ""), t && ($t && (t.cancelable && t.preventDefault(), !r.dropBubble && t.stopPropagation()), L && L.parentNode && L.parentNode.removeChild(L), (J === te || de && de.lastPutMode !== "clone") && ne && ne.parentNode && ne.parentNode.removeChild(ne), w && (this.nativeDraggable && $(w, "dragend", this), _r(w), w.style["will-change"] = "", $t && !St && De(w, de ? de.options.ghostClass : this.options.ghostClass, !1), De(w, this.options.chosenClass, !1), ve({
      sortable: this,
      name: "unchoose",
      toEl: te,
      newIndex: null,
      newDraggableIndex: null,
      originalEvent: t
    }), J !== te ? (Se >= 0 && (ve({
      rootEl: te,
      name: "add",
      toEl: te,
      fromEl: J,
      originalEvent: t
    }), ve({
      sortable: this,
      name: "remove",
      toEl: te,
      originalEvent: t
    }), ve({
      rootEl: te,
      name: "sort",
      toEl: te,
      fromEl: J,
      originalEvent: t
    }), ve({
      sortable: this,
      name: "sort",
      toEl: te,
      originalEvent: t
    })), de && de.save()) : Se !== Tt && Se >= 0 && (ve({
      sortable: this,
      name: "update",
      toEl: te,
      originalEvent: t
    }), ve({
      sortable: this,
      name: "sort",
      toEl: te,
      originalEvent: t
    })), V.active && ((Se == null || Se === -1) && (Se = Tt, Ze = Zt), ve({
      sortable: this,
      name: "end",
      toEl: te,
      originalEvent: t
    }), this.save()))), this._nulling();
  },
  _nulling: function() {
    be("nulling", this), J = w = te = L = ut = ne = wn = Xe = it = ke = $t = Se = Ze = Tt = Zt = Ct = Xt = de = hn = V.dragged = V.ghost = V.clone = V.active = null, Vn.forEach(function(t) {
      t.checked = !0;
    }), Vn.length = yr = br = 0;
  },
  handleEvent: function(t) {
    switch (t.type) {
      case "drop":
      case "dragend":
        this._onDrop(t);
        break;
      case "dragenter":
      case "dragover":
        w && (this._onDragOver(t), wl(t));
        break;
      case "selectstart":
        t.preventDefault();
        break;
    }
  },
  toArray: function() {
    for (var t = [], n, r = this.el.children, o = 0, a = r.length, i = this.options; o < a; o++)
      n = r[o], Pe(n, i.draggable, this.el, !1) && t.push(n.getAttribute(i.dataIdAttr) || Ml(n));
    return t;
  },
  sort: function(t, n) {
    var r = {}, o = this.el;
    this.toArray().forEach(function(a, i) {
      var s = o.children[i];
      Pe(s, this.options.draggable, o, !1) && (r[a] = s);
    }, this), n && this.captureAnimationState(), t.forEach(function(a) {
      r[a] && (o.removeChild(r[a]), o.appendChild(r[a]));
    }), n && this.animateAll();
  },
  save: function() {
    var t = this.options.store;
    t && t.set && t.set(this);
  },
  closest: function(t, n) {
    return Pe(t, n || this.options.draggable, this.el, !1);
  },
  option: function(t, n) {
    var r = this.options;
    if (n === void 0)
      return r[t];
    var o = un.modifyOption(this, t, n);
    typeof o < "u" ? r[t] = o : r[t] = n, t === "group" && gi(r);
  },
  destroy: function() {
    be("destroy", this);
    var t = this.el;
    t[we] = null, $(t, "mousedown", this._onTapStart), $(t, "touchstart", this._onTapStart), $(t, "pointerdown", this._onTapStart), this.nativeDraggable && ($(t, "dragover", this), $(t, "dragenter", this)), Array.prototype.forEach.call(t.querySelectorAll("[draggable]"), function(n) {
      n.removeAttribute("draggable");
    }), this._onDrop(), this._disableDelayedDragEvents(), Fn.splice(Fn.indexOf(this.el), 1), this.el = t = null;
  },
  _hideClone: function() {
    if (!Xe) {
      if (be("hideClone", this), V.eventCanceled)
        return;
      F(ne, "display", "none"), this.options.removeCloneOnHide && ne.parentNode && ne.parentNode.removeChild(ne), Xe = !0;
    }
  },
  _showClone: function(t) {
    if (t.lastPutMode !== "clone") {
      this._hideClone();
      return;
    }
    if (Xe) {
      if (be("showClone", this), V.eventCanceled)
        return;
      w.parentNode == J && !this.options.group.revertClone ? J.insertBefore(ne, w) : ut ? J.insertBefore(ne, ut) : J.appendChild(ne), this.options.group.revertClone && this.animate(w, ne), F(ne, "display", ""), Xe = !1;
    }
  }
};
function wl(e) {
  e.dataTransfer && (e.dataTransfer.dropEffect = "move"), e.cancelable && e.preventDefault();
}
function gn(e, t, n, r, o, a, i, s) {
  var l, u = e[we], c = u.options.onMove, d;
  return window.CustomEvent && !je && !ln ? l = new CustomEvent("move", {
    bubbles: !0,
    cancelable: !0
  }) : (l = document.createEvent("Event"), l.initEvent("move", !0, !0)), l.to = t, l.from = e, l.dragged = n, l.draggedRect = r, l.related = o || t, l.relatedRect = a || ie(t), l.willInsertAfter = s, l.originalEvent = i, e.dispatchEvent(l), c && (d = c.call(u, l, i)), d;
}
function _r(e) {
  e.draggable = !1;
}
function Tl() {
  zr = !1;
}
function Rl(e, t, n) {
  var r = ie(Nt(n.el, 0, n.options, !0)), o = 10;
  return t ? e.clientX < r.left - o || e.clientY < r.top && e.clientX < r.right : e.clientY < r.top - o || e.clientY < r.bottom && e.clientX < r.left;
}
function kl(e, t, n) {
  var r = ie(po(n.el, n.options.draggable)), o = 10;
  return t ? e.clientX > r.right + o || e.clientX <= r.right && e.clientY > r.bottom && e.clientX >= r.left : e.clientX > r.right && e.clientY > r.top || e.clientX <= r.right && e.clientY > r.bottom + o;
}
function Il(e, t, n, r, o, a, i, s) {
  var l = r ? e.clientY : e.clientX, u = r ? n.height : n.width, c = r ? n.top : n.left, d = r ? n.bottom : n.right, f = !1;
  if (!i) {
    if (s && Tn < u * o) {
      if (!Kt && (Xt === 1 ? l > c + u * a / 2 : l < d - u * a / 2) && (Kt = !0), Kt)
        f = !0;
      else if (Xt === 1 ? l < c + Tn : l > d - Tn)
        return -Xt;
    } else if (l > c + u * (1 - o) / 2 && l < d - u * (1 - o) / 2)
      return xl(t);
  }
  return f = f || i, f && (l < c + u * a / 2 || l > d - u * a / 2) ? l > c + u / 2 ? 1 : -1 : 0;
}
function xl(e) {
  return Te(w) < Te(e) ? 1 : -1;
}
function Ml(e) {
  for (var t = e.tagName + e.className + e.src + e.href + e.textContent, n = t.length, r = 0; n--; )
    r += t.charCodeAt(n);
  return r.toString(36);
}
function Pl(e) {
  Vn.length = 0;
  for (var t = e.getElementsByTagName("input"), n = t.length; n--; ) {
    var r = t[n];
    r.checked && Vn.push(r);
  }
}
function Rn(e) {
  return setTimeout(e, 0);
}
function Gr(e) {
  return clearTimeout(e);
}
ir && G(document, "touchmove", function(e) {
  (V.active || St) && e.cancelable && e.preventDefault();
});
V.utils = {
  on: G,
  off: $,
  css: F,
  find: di,
  is: function(t, n) {
    return !!Pe(t, n, t, !1);
  },
  extend: hl,
  throttle: fi,
  closest: Pe,
  toggleClass: De,
  clone: hi,
  index: Te,
  nextTick: Rn,
  cancelNextTick: Gr,
  detectDirection: mi,
  getChild: Nt
};
V.get = function(e) {
  return e[we];
};
V.mount = function() {
  for (var e = arguments.length, t = new Array(e), n = 0; n < e; n++)
    t[n] = arguments[n];
  t[0].constructor === Array && (t = t[0]), t.forEach(function(r) {
    if (!r.prototype || !r.prototype.constructor)
      throw "Sortable: Mounted plugin must be a constructor function, not ".concat({}.toString.call(r));
    r.utils && (V.utils = Fe(Fe({}, V.utils), r.utils)), un.mount(r);
  });
};
V.create = function(e, t) {
  return new V(e, t);
};
V.version = cl;
var ae = [], zt, jr, Yr = !1, Cr, Dr, Un, Gt;
function Ol() {
  function e() {
    this.defaults = {
      scroll: !0,
      forceAutoScrollFallback: !1,
      scrollSensitivity: 30,
      scrollSpeed: 10,
      bubbleScroll: !0
    };
    for (var t in this)
      t.charAt(0) === "_" && typeof this[t] == "function" && (this[t] = this[t].bind(this));
  }
  return e.prototype = {
    dragStarted: function(n) {
      var r = n.originalEvent;
      this.sortable.nativeDraggable ? G(document, "dragover", this._handleAutoScroll) : this.options.supportPointer ? G(document, "pointermove", this._handleFallbackAutoScroll) : r.touches ? G(document, "touchmove", this._handleFallbackAutoScroll) : G(document, "mousemove", this._handleFallbackAutoScroll);
    },
    dragOverCompleted: function(n) {
      var r = n.originalEvent;
      !this.options.dragOverBubble && !r.rootEl && this._handleAutoScroll(r);
    },
    drop: function() {
      this.sortable.nativeDraggable ? $(document, "dragover", this._handleAutoScroll) : ($(document, "pointermove", this._handleFallbackAutoScroll), $(document, "touchmove", this._handleFallbackAutoScroll), $(document, "mousemove", this._handleFallbackAutoScroll)), Xo(), kn(), vl();
    },
    nulling: function() {
      Un = jr = zt = Yr = Gt = Cr = Dr = null, ae.length = 0;
    },
    _handleFallbackAutoScroll: function(n) {
      this._handleAutoScroll(n, !0);
    },
    _handleAutoScroll: function(n, r) {
      var o = this, a = (n.touches ? n.touches[0] : n).clientX, i = (n.touches ? n.touches[0] : n).clientY, s = document.elementFromPoint(a, i);
      if (Un = n, r || this.options.forceAutoScrollFallback || ln || je || Yt) {
        Sr(n, this.options, s, r);
        var l = Ke(s, !0);
        Yr && (!Gt || a !== Cr || i !== Dr) && (Gt && Xo(), Gt = setInterval(function() {
          var u = Ke(document.elementFromPoint(a, i), !0);
          u !== l && (l = u, kn()), Sr(n, o.options, u, r);
        }, 10), Cr = a, Dr = i);
      } else {
        if (!this.options.bubbleScroll || Ke(s, !0) === Ne()) {
          kn();
          return;
        }
        Sr(n, this.options, Ke(s, !1), !1);
      }
    }
  }, $e(e, {
    pluginName: "scroll",
    initializeByDefault: !0
  });
}
function kn() {
  ae.forEach(function(e) {
    clearInterval(e.pid);
  }), ae = [];
}
function Xo() {
  clearInterval(Gt);
}
var Sr = fi(function(e, t, n, r) {
  if (!!t.scroll) {
    var o = (e.touches ? e.touches[0] : e).clientX, a = (e.touches ? e.touches[0] : e).clientY, i = t.scrollSensitivity, s = t.scrollSpeed, l = Ne(), u = !1, c;
    jr !== n && (jr = n, kn(), zt = t.scroll, c = t.scrollFn, zt === !0 && (zt = Ke(n, !0)));
    var d = 0, f = zt;
    do {
      var p = f, h = ie(p), b = h.top, E = h.bottom, y = h.left, D = h.right, k = h.width, T = h.height, U = void 0, O = void 0, R = p.scrollWidth, A = p.scrollHeight, M = F(p), I = p.scrollLeft, N = p.scrollTop;
      p === l ? (U = k < R && (M.overflowX === "auto" || M.overflowX === "scroll" || M.overflowX === "visible"), O = T < A && (M.overflowY === "auto" || M.overflowY === "scroll" || M.overflowY === "visible")) : (U = k < R && (M.overflowX === "auto" || M.overflowX === "scroll"), O = T < A && (M.overflowY === "auto" || M.overflowY === "scroll"));
      var j = U && (Math.abs(D - o) <= i && I + k < R) - (Math.abs(y - o) <= i && !!I), oe = O && (Math.abs(E - a) <= i && N + T < A) - (Math.abs(b - a) <= i && !!N);
      if (!ae[d])
        for (var Ue = 0; Ue <= d; Ue++)
          ae[Ue] || (ae[Ue] = {});
      (ae[d].vx != j || ae[d].vy != oe || ae[d].el !== p) && (ae[d].el = p, ae[d].vx = j, ae[d].vy = oe, clearInterval(ae[d].pid), (j != 0 || oe != 0) && (u = !0, ae[d].pid = setInterval(function() {
        r && this.layer === 0 && V.active._onTouchMove(Un);
        var at = ae[this.layer].vy ? ae[this.layer].vy * s : 0, ye = ae[this.layer].vx ? ae[this.layer].vx * s : 0;
        typeof c == "function" && c.call(V.dragged.parentNode[we], ye, at, e, Un, ae[this.layer].el) !== "continue" || pi(ae[this.layer].el, ye, at);
      }.bind({
        layer: d
      }), 24))), d++;
    } while (t.bubbleScroll && f !== l && (f = Ke(f, !1)));
    Yr = u;
  }
}, 30), Ei = function(t) {
  var n = t.originalEvent, r = t.putSortable, o = t.dragEl, a = t.activeSortable, i = t.dispatchSortableEvent, s = t.hideGhostForTarget, l = t.unhideGhostForTarget;
  if (!!n) {
    var u = r || a;
    s();
    var c = n.changedTouches && n.changedTouches.length ? n.changedTouches[0] : n, d = document.elementFromPoint(c.clientX, c.clientY);
    l(), u && !u.el.contains(d) && (i("spill"), this.onSpill({
      dragEl: o,
      putSortable: r
    }));
  }
};
function ho() {
}
ho.prototype = {
  startIndex: null,
  dragStart: function(t) {
    var n = t.oldDraggableIndex;
    this.startIndex = n;
  },
  onSpill: function(t) {
    var n = t.dragEl, r = t.putSortable;
    this.sortable.captureAnimationState(), r && r.captureAnimationState();
    var o = Nt(this.sortable.el, this.startIndex, this.options);
    o ? this.sortable.el.insertBefore(n, o) : this.sortable.el.appendChild(n), this.sortable.animateAll(), r && r.animateAll();
  },
  drop: Ei
};
$e(ho, {
  pluginName: "revertOnSpill"
});
function vo() {
}
vo.prototype = {
  onSpill: function(t) {
    var n = t.dragEl, r = t.putSortable, o = r || this.sortable;
    o.captureAnimationState(), n.parentNode && n.parentNode.removeChild(n), o.animateAll();
  },
  drop: Ei
};
$e(vo, {
  pluginName: "removeOnSpill"
});
V.mount(new Ol());
V.mount(vo, ho);
function Al() {
  return typeof window < "u" ? window.console : global.console;
}
const Nl = Al();
function Hl(e) {
  const t = /* @__PURE__ */ Object.create(null);
  return function(r) {
    return t[r] || (t[r] = e(r));
  };
}
const Fl = /-(\w)/g, Ko = Hl((e) => e.replace(Fl, (t, n) => n ? n.toUpperCase() : ""));
function wr(e) {
  e.parentElement !== null && e.parentElement.removeChild(e);
}
function Jo(e, t, n) {
  const r = n === 0 ? e.children[0] : e.children[n - 1].nextSibling;
  e.insertBefore(t, r);
}
function Vl(e, t) {
  return Object.values(e).indexOf(t);
}
function Ul(e, t, n, r) {
  if (!e)
    return [];
  const o = Object.values(e), a = t.length - r;
  return [...t].map((s, l) => l >= a ? o.length : o.indexOf(s));
}
function _i(e, t) {
  this.$nextTick(() => this.$emit(e.toLowerCase(), t));
}
function Ll(e) {
  return (t) => {
    this.realList !== null && this["onDrag" + e](t), _i.call(this, e, t);
  };
}
function Bl(e) {
  return ["transition-group", "TransitionGroup"].includes(e);
}
function Wl(e) {
  if (!e || e.length !== 1)
    return !1;
  const [{ type: t }] = e;
  return t ? Bl(t.name) : !1;
}
function $l(e, t) {
  return t ? { ...t.props, ...t.attrs } : e;
}
const qr = ["Start", "Add", "Remove", "Update", "End"], Zr = ["Choose", "Unchoose", "Sort", "Filter", "Clone"], zl = ["Move", ...qr, ...Zr].map((e) => "on" + e);
let Tr = null;
const Gl = {
  options: Object,
  list: {
    type: Array,
    required: !1,
    default: null
  },
  noTransitionOnDrag: {
    type: Boolean,
    default: !1
  },
  clone: {
    type: Function,
    default: (e) => e
  },
  tag: {
    type: String,
    default: "div"
  },
  move: {
    type: Function,
    default: null
  },
  componentData: {
    type: Object,
    required: !1,
    default: null
  },
  component: {
    type: String,
    default: null
  },
  modelValue: {
    type: Array,
    required: !1,
    default: null
  }
}, Ci = K({
  name: "VueDraggableNext",
  inheritAttrs: !1,
  emits: [
    "update:modelValue",
    "move",
    "change",
    ...qr.map((e) => e.toLowerCase()),
    ...Zr.map((e) => e.toLowerCase())
  ],
  props: Gl,
  data() {
    return {
      transitionMode: !1,
      noneFunctionalComponentMode: !1,
      headerOffset: 0,
      footerOffset: 0,
      _sortable: {},
      visibleIndexes: [],
      context: {}
    };
  },
  render() {
    const e = this.$slots.default ? this.$slots.default() : null, t = $l(this.$attrs, this.componentData);
    return e ? (this.transitionMode = Wl(e), Fo(this.getTag(), t, e)) : Fo(this.getTag(), t, []);
  },
  created() {
    this.list !== null && this.modelValue !== null && Nl.error("list props are mutually exclusive! Please set one.");
  },
  mounted() {
    const e = {};
    qr.forEach((o) => {
      e["on" + o] = Ll.call(this, o);
    }), Zr.forEach((o) => {
      e["on" + o] = _i.bind(this, o);
    });
    const t = Object.keys(this.$attrs).reduce((o, a) => (o[Ko(a)] = this.$attrs[a], o), {}), n = Object.assign({}, t, e, {
      onMove: (o, a) => this.onDragMove(o, a)
    });
    !("draggable" in n) && (n.draggable = ">*");
    const r = this.$el.nodeType === 1 ? this.$el : this.$el.parentElement;
    this._sortable = new V(r, n), r.__draggable_component__ = this, this.computeIndexes();
  },
  beforeUnmount() {
    try {
      this._sortable !== void 0 && this._sortable.destroy();
    } catch {
    }
  },
  computed: {
    realList() {
      return this.list ? this.list : this.modelValue;
    }
  },
  watch: {
    $attrs: {
      handler(e) {
        this.updateOptions(e);
      },
      deep: !0
    },
    realList() {
      this.computeIndexes();
    }
  },
  methods: {
    getTag() {
      return this.component ? ge(this.component) : this.tag;
    },
    updateOptions(e) {
      for (var t in e) {
        const n = Ko(t);
        zl.indexOf(n) === -1 && this._sortable.option(n, e[t]);
      }
    },
    getChildrenNodes() {
      return this.$el.children;
    },
    computeIndexes() {
      this.$nextTick(() => {
        this.visibleIndexes = Ul(this.getChildrenNodes(), this.$el.children, this.transitionMode, this.footerOffset);
      });
    },
    getUnderlyingVm(e) {
      const t = Vl(this.getChildrenNodes() || [], e);
      if (t === -1)
        return null;
      const n = this.realList[t];
      return { index: t, element: n };
    },
    emitChanges(e) {
      this.$nextTick(() => {
        this.$emit("change", e);
      });
    },
    alterList(e) {
      if (this.list) {
        e(this.list);
        return;
      }
      const t = [...this.modelValue];
      e(t), this.$emit("update:modelValue", t);
    },
    spliceList() {
      const e = (t) => t.splice(...arguments);
      this.alterList(e);
    },
    updatePosition(e, t) {
      const n = (r) => r.splice(t, 0, r.splice(e, 1)[0]);
      this.alterList(n);
    },
    getVmIndex(e) {
      const t = this.visibleIndexes, n = t.length;
      return e > n - 1 ? n : t[e];
    },
    getComponent() {
      return this.$slots.default ? this.$slots.default()[0].componentInstance : null;
    },
    resetTransitionData(e) {
      if (!this.noTransitionOnDrag || !this.transitionMode)
        return;
      var t = this.getChildrenNodes();
      t[e].data = null;
      const n = this.getComponent();
      n.children = [], n.kept = void 0;
    },
    onDragStart(e) {
      this.context = this.getUnderlyingVm(e.item), this.context && (e.item._underlying_vm_ = this.clone(this.context.element), Tr = e.item);
    },
    onDragAdd(e) {
      const t = e.item._underlying_vm_;
      if (t === void 0)
        return;
      wr(e.item);
      const n = this.getVmIndex(e.newIndex);
      this.spliceList(n, 0, t), this.computeIndexes();
      const r = { element: t, newIndex: n };
      this.emitChanges({ added: r });
    },
    onDragRemove(e) {
      if (Jo(this.$el, e.item, e.oldIndex), e.pullMode === "clone") {
        wr(e.clone);
        return;
      }
      if (!this.context)
        return;
      const t = this.context.index;
      this.spliceList(t, 1);
      const n = { element: this.context.element, oldIndex: t };
      this.resetTransitionData(t), this.emitChanges({ removed: n });
    },
    onDragUpdate(e) {
      wr(e.item), Jo(e.from, e.item, e.oldIndex);
      const t = this.context.index, n = this.getVmIndex(e.newIndex);
      this.updatePosition(t, n);
      const r = { element: this.context.element, oldIndex: t, newIndex: n };
      this.emitChanges({ moved: r });
    },
    updateProperty(e, t) {
      e.hasOwnProperty(t) && (e[t] += this.headerOffset);
    },
    onDragMove(e, t) {
      const n = this.move;
      if (!n || !this.realList)
        return !0;
      const r = this.getRelatedContextFromMoveEvent(e), o = this.context, a = this.computeFutureIndex(r, e);
      Object.assign(o, { futureIndex: a });
      const i = Object.assign({}, e, {
        relatedContext: r,
        draggedContext: o
      });
      return n(i, t);
    },
    onDragEnd() {
      this.computeIndexes(), Tr = null;
    },
    getTrargetedComponent(e) {
      return e.__draggable_component__;
    },
    getRelatedContextFromMoveEvent({ to: e, related: t }) {
      const n = this.getTrargetedComponent(e);
      if (!n)
        return { component: n };
      const r = n.realList, o = { list: r, component: n };
      if (e !== t && r && n.getUnderlyingVm) {
        const a = n.getUnderlyingVm(t);
        if (a)
          return Object.assign(a, o);
      }
      return o;
    },
    computeFutureIndex(e, t) {
      const n = [...t.to.children].filter((i) => i.style.display !== "none");
      if (n.length === 0)
        return 0;
      const r = n.indexOf(t.related), o = e.component.getVmIndex(r);
      return n.indexOf(Tr) !== -1 || !t.willInsertAfter ? o : o + 1;
    }
  }
}), jl = { key: 0 }, Yl = { key: 0 }, ql = { class: "text-field-lable" }, Zl = { key: 1 }, Xl = { class: "text-field-lable" }, Kl = { key: 1 }, Jl = { key: 0 }, Ql = { key: 1 }, eu = /* @__PURE__ */ K({
  __name: "Text",
  props: {
    model: null,
    textType: null,
    dispLang: { type: Boolean }
  },
  setup(e) {
    return ar(), (t, n) => e.dispLang ? (S(), x("div", jl, [
      e.textType === _(Ot).ShortAnswer ? (S(), x("div", Yl, [
        C("span", ql, se(e.model.lang) + ": ", 1),
        Ee(C("input", {
          type: "text",
          "onUpdate:modelValue": n[0] || (n[0] = (r) => e.model.value = r),
          class: "text-field"
        }, null, 512), [
          [xe, e.model.value]
        ])
      ])) : e.textType === "Paragraph" ? (S(), x("div", Zl, [
        C("span", Xl, se(e.model.lang) + ": ", 1),
        Ee(C("textarea", {
          "onUpdate:modelValue": n[1] || (n[1] = (r) => e.model.value = r),
          class: "field-text-area"
        }, null, 512), [
          [xe, e.model.value]
        ])
      ])) : Z("", !0)
    ])) : (S(), x("div", Kl, [
      e.textType === _(Ot).ShortAnswer ? (S(), x("div", Jl, [
        Ee(C("input", {
          type: "text",
          "onUpdate:modelValue": n[2] || (n[2] = (r) => e.model.value = r),
          class: "text-field"
        }, null, 512), [
          [xe, e.model.value]
        ])
      ])) : e.textType === "Paragraph" ? (S(), x("div", Ql, [
        Ee(C("textarea", {
          "onUpdate:modelValue": n[3] || (n[3] = (r) => e.model.value = r),
          class: "text-area"
        }, null, 512), [
          [xe, e.model.value]
        ])
      ])) : Z("", !0)
    ]));
  }
}), In = /* @__PURE__ */ K({
  __name: "TextCollection",
  props: {
    model: null,
    textType: null
  },
  setup(e) {
    return (t, n) => (S(!0), x(q, null, Ce(e.model.values, (r) => {
      var o;
      return S(), pe(eu, {
        key: r.id,
        model: r,
        "text-type": e.textType,
        "disp-lang": ((o = e.model.values) == null ? void 0 : o.length) > 1
      }, null, 8, ["model", "text-type", "disp-lang"]);
    }), 128));
  }
}), tu = { key: 0 }, nu = {
  key: 0,
  class: "option-values"
}, ru = { key: 1 }, ou = /* @__PURE__ */ K({
  __name: "Option",
  props: {
    model: null,
    optionType: null
  },
  setup(e) {
    ar();
    const t = Jn(!1);
    return (n, r) => {
      const o = ge("font-awesome-icon");
      return t.value ? (S(), x("span", ru, [
        B(In, {
          model: e.model.optionText,
          "text-type": _(z).ShortAnswer
        }, null, 8, ["model", "text-type"]),
        B(o, {
          icon: "fa-solid fa-circle-check",
          onClick: r[1] || (r[1] = (a) => t.value = !1),
          class: "fa-icon delete-button"
        })
      ])) : (S(), x("span", tu, [
        (S(!0), x(q, null, Ce(e.model.optionText.values, (a) => {
          var i;
          return S(), x("span", null, [
            ((i = a.value) == null ? void 0 : i.length) > 0 ? (S(), x("span", nu, se(a.value), 1)) : Z("", !0)
          ]);
        }), 256)),
        B(o, {
          icon: "fa-solid fa-pen-to-square",
          onClick: r[0] || (r[0] = (a) => t.value = !0),
          class: "fa-icon"
        })
      ]));
    };
  }
}), au = /* @__PURE__ */ C("h6", null, "Title:", -1), iu = /* @__PURE__ */ C("h6", null, "Description:", -1), su = { key: 0 }, lu = /* @__PURE__ */ C("h6", null, "Options:", -1), uu = { class: "display-options" }, cu = /* @__PURE__ */ K({
  __name: "Field",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = fo(t.model), r = ar(), o = Jn(At(r.lang)), a = () => {
      var s;
      (s = t.model.options) == null || s.push(ai(r.lang, nl(o.value))), o.value.values.forEach((l) => {
        l.value = "";
      });
    }, i = (s) => {
      var u, c;
      const l = (u = t.model.options) == null ? void 0 : u.findIndex((d) => d.id == s);
      (c = t.model.options) == null || c.splice(l, 1);
    };
    return (s, l) => {
      const u = ge("font-awesome-icon");
      return S(), x(q, null, [
        C("h5", null, se(e.model.type), 1),
        C("div", null, [
          au,
          B(In, {
            model: e.model.title,
            "text-type": _(z).ShortAnswer
          }, null, 8, ["model", "text-type"])
        ]),
        C("div", null, [
          iu,
          B(In, {
            model: e.model.description,
            "text-type": _(z).Paragraph
          }, null, 8, ["model", "text-type"])
        ]),
        _(n) ? (S(), x("div", su, [
          lu,
          C("div", uu, [
            B(_(Ci), {
              class: "dragArea list-group w-full",
              list: e.model.options
            }, {
              default: Be(() => [
                (S(!0), x(q, null, Ce(e.model.options, (c) => (S(), x("div", {
                  key: c.id,
                  class: "option-entry"
                }, [
                  B(ou, {
                    model: c,
                    "option-type": e.model.type
                  }, null, 8, ["model", "option-type"]),
                  C("span", null, [
                    B(u, {
                      icon: "fa-solid fa-circle-xmark",
                      onClick: (d) => i(c.id),
                      class: "fa-icon delete"
                    }, null, 8, ["onClick"])
                  ])
                ]))), 128))
              ]),
              _: 1
            }, 8, ["list"])
          ]),
          C("div", null, [
            B(In, {
              model: o.value,
              "text-type": _(z).ShortAnswer
            }, null, 8, ["model", "text-type"]),
            B(u, {
              icon: "fa-solid fa-circle-plus",
              onClick: l[0] || (l[0] = (c) => a()),
              class: "fa-icon plus add-option"
            })
          ])
        ])) : Z("", !0)
      ], 64);
    };
  }
});
const du = /* @__PURE__ */ C("div", null, [
  /* @__PURE__ */ C("h4", null, "Form properties")
], -1), fu = { class: "form-field-border" }, pu = /* @__PURE__ */ C("span", { class: "text-field-lable" }, "Name:", -1), hu = { style: { display: "inline" } }, vu = /* @__PURE__ */ C("span", { class: "text-area-lable" }, "Description:", -1), mu = /* @__PURE__ */ C("h3", null, "Fields", -1), gu = /* @__PURE__ */ K({
  __name: "Form",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = (r) => {
      var a, i;
      const o = (a = t.model.fields) == null ? void 0 : a.findIndex((s) => s.id == r);
      (i = t.model.fields) == null || i.splice(o, 1);
    };
    return (r, o) => {
      var i;
      const a = ge("font-awesome-icon");
      return S(), x(q, null, [
        du,
        C("div", fu, [
          C("div", null, [
            pu,
            Ee(C("input", {
              type: "text",
              "onUpdate:modelValue": o[0] || (o[0] = (s) => e.model.name = s),
              class: "text-field"
            }, null, 512), [
              [xe, e.model.name]
            ])
          ]),
          C("div", hu, [
            vu,
            Ee(C("textarea", {
              "onUpdate:modelValue": o[1] || (o[1] = (s) => e.model.description = s),
              class: "text-area"
            }, null, 512), [
              [xe, e.model.description]
            ])
          ])
        ]),
        mu,
        B(_(Ci), {
          class: "dragArea list-group w-full",
          list: (i = e.model) == null ? void 0 : i.fields
        }, {
          default: Be(() => {
            var s;
            return [
              (S(!0), x(q, null, Ce((s = e.model) == null ? void 0 : s.fields, (l) => (S(), x("div", {
                key: l.id,
                class: "form-field-border form-field"
              }, [
                B(a, {
                  icon: "fa-solid fa-circle-xmark",
                  onClick: (u) => n(l.id),
                  class: "fa-icon field-delete"
                }, null, 8, ["onClick"]),
                B(cu, { model: l }, null, 8, ["model"])
              ]))), 128))
            ];
          }),
          _: 1
        }, 8, ["list"])
      ], 64);
    };
  }
}), Di = (e) => (er("data-v-ebc05723"), e = e(), tr(), e), yu = /* @__PURE__ */ Di(() => /* @__PURE__ */ C("h2", null, "Form Builder", -1)), bu = { class: "control" }, Eu = ["disabled"], _u = ["disabled"], Cu = { class: "toolbar" }, Du = ["disabled"], Su = ["disabled"], wu = ["disabled"], Tu = ["disabled"], Ru = ["disabled"], ku = ["disabled"], Iu = ["disabled"], xu = ["disabled"], Mu = ["disabled"], Pu = ["disabled"], Ou = ["disabled"], Au = ["disabled"], Nu = ["disabled"], Hu = /* @__PURE__ */ Di(() => /* @__PURE__ */ C("hr", null, null, -1)), Fu = /* @__PURE__ */ K({
  __name: "App",
  props: {
    piniaInstance: null,
    repositoryRoot: null,
    formId: null
  },
  setup(e) {
    const t = e, n = ar(t.piniaInstance);
    t.formId && n.loadForm(t.formId), Qn(() => n.transientMessage, async (s) => {
      s && setTimeout(() => {
        n.transientMessage = null;
      }, 2e3);
    });
    const r = () => {
      n.form = {
        id: ee.EMPTY,
        name: "",
        description: "",
        fields: []
      };
    }, o = () => n.saveForm(), a = he(() => !n.form), i = (s) => {
      var u;
      const l = {
        id: ee.create().toString(),
        title: At(n.lang),
        description: At(n.lang),
        type: s
      };
      fo(l) && (l.options = [ai(n.lang, null)]), (u = n.form) == null || u.fields.push(l);
    };
    return (s, l) => (S(), x(q, null, [
      B(lo, { name: "fade" }, {
        default: Be(() => [
          _(n).transientMessage ? (S(), x("p", {
            key: 0,
            class: uo("alert alert-" + _(n).transientMessageClass)
          }, se(_(n).transientMessage), 3)) : Z("", !0)
        ]),
        _: 1
      }),
      yu,
      _(n).form ? (S(), pe(gu, {
        key: 0,
        model: _(n).form
      }, null, 8, ["model"])) : Z("", !0),
      C("div", bu, [
        C("button", {
          type: "button",
          class: "btn btn-primary",
          disabled: !_(a),
          onClick: r
        }, "New Form", 8, Eu),
        C("button", {
          type: "button",
          class: "btn btn-success",
          disabled: _(a),
          onClick: o
        }, "Save", 8, _u)
      ]),
      C("div", Cu, [
        C("button", {
          disabled: _(a),
          onClick: l[0] || (l[0] = (u) => i(_(z).ShortAnswer))
        }, "+ Short Answer", 8, Du),
        C("button", {
          disabled: _(a),
          onClick: l[1] || (l[1] = (u) => i(_(z).Paragraph))
        }, "+ Paragraph", 8, Su),
        C("button", {
          disabled: _(a),
          onClick: l[2] || (l[2] = (u) => i(_(z).RichText))
        }, "+ Rich Text", 8, wu),
        C("button", {
          disabled: _(a),
          onClick: l[3] || (l[3] = (u) => i(_(z).Date))
        }, "+ Date", 8, Tu),
        C("button", {
          disabled: _(a),
          onClick: l[4] || (l[4] = (u) => i(_(z).DateTime))
        }, "+ Date/Time", 8, Ru),
        C("button", {
          disabled: _(a),
          onClick: l[5] || (l[5] = (u) => i(_(z).Decimal))
        }, "+ Decimal", 8, ku),
        C("button", {
          disabled: _(a),
          onClick: l[6] || (l[6] = (u) => i(_(z).Integer))
        }, "+ Integer", 8, Iu),
        C("button", {
          disabled: _(a),
          onClick: l[7] || (l[7] = (u) => i(_(z).Email))
        }, "+ Email", 8, xu),
        C("button", {
          disabled: _(a),
          onClick: l[8] || (l[8] = (u) => i(_(z).Checkboxes))
        }, "+ Checkboxes", 8, Mu),
        C("button", {
          disabled: _(a),
          onClick: l[9] || (l[9] = (u) => i(_(z).DataList))
        }, "+ Data List", 8, Pu),
        C("button", {
          disabled: _(a),
          onClick: l[10] || (l[10] = (u) => i(_(z).RadioButtons))
        }, "+ Radio Buttons", 8, Ou),
        C("button", {
          disabled: _(a),
          onClick: l[11] || (l[11] = (u) => i(_(z).DropDown))
        }, "+ Drop Down", 8, Au),
        C("button", {
          disabled: _(a),
          onClick: l[12] || (l[12] = (u) => i(_(z).InfoSection))
        }, "+ Info Section", 8, Nu)
      ]),
      Hu
    ], 64));
  }
});
const sr = (e, t) => {
  const n = e.__vccOpts || e;
  for (const [r, o] of t)
    n[r] = o;
  return n;
}, Wm = /* @__PURE__ */ sr(Fu, [["__scopeId", "data-v-ebc05723"]]), Ve = sn("FormSubmissionStore", {
  state: () => ({
    lang: "en",
    form: null,
    formData: {},
    transientMessage: null,
    transientMessageClass: null
  }),
  actions: {
    loadForm(e, t) {
      let n = `https://localhost:5020/api/forms/${e}`;
      console.log(n), fetch(n, {
        method: "GET"
      }).then((r) => r.json()).then((r) => {
        this.form = r, t || (this.formData = il(this.form, this.lang));
      }).catch((r) => {
        console.error("Load Form API Error:", r);
      });
    },
    loadSubmission(e) {
      let t = `https://localhost:5020/api/form-submissions/${e}`;
      console.log(t), fetch(t, {
        method: "GET"
      }).then((n) => n.json()).then((n) => {
        var r;
        this.formData = n, (r = this.formData) != null && r.formId && this.loadForm(this.formData.formId, !0);
      }).catch((n) => {
        console.error("Load Form API Error:", n);
      });
    },
    validateFormData() {
      return console.log("TODO: Validate form data."), !0;
    },
    submitForm() {
      var r, o;
      if (!this.validateFormData()) {
        console.log("Form validation failed.");
        return;
      }
      const e = ((o = (r = this.formData) == null ? void 0 : r.id) == null ? void 0 : o.toString()) === ee.EMPTY;
      let t = "https://localhost:5020/api/form-submissions", n = "";
      e ? n = "POST" : (t = `${t}/${this.formData.id}`, n = "PUT"), fetch(
        t,
        {
          body: JSON.stringify(this.formData),
          method: n,
          headers: {
            encType: "multipart/form-data",
            "Content-Type": "application/json"
          }
        }
      ).then(async (a) => {
        if (a.ok) {
          if (e) {
            const i = await a.json();
            this.formData.id = i;
          }
          this.transientMessage = "Success", this.transientMessageClass = "success", console.log("Form submission successfull.");
        } else
          switch (this.transientMessageClass = "danger", a.status) {
            case 400:
              this.transientMessage = "Bad request. Failed to submit the form";
              break;
            case 404:
              this.transientMessage = "Form submission not found";
              break;
            case 500:
              this.transientMessage = "An internal server error occurred. Failed to submit the form";
              break;
            default:
              this.transientMessage = "Unknown error occured. Failed to submit the form";
              break;
          }
      }).catch((a) => {
        e && this.formData && (this.formData.id = ee.EMPTY), this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("FormData Submit API Error:", a);
      });
    },
    saveForm() {
      var r, o;
      if (!this.form) {
        console.error("Cannot save null form.");
        return;
      }
      const e = ((o = (r = this.form) == null ? void 0 : r.id) == null ? void 0 : o.toString()) === ee.EMPTY;
      let t = "https://localhost:5020/api/forms", n = "";
      e ? (console.log("Saving new form."), this.form.id = ee.create().toString(), n = "POST") : (console.log("Updating existing form."), t = `${t}/${this.form.id}`, n = "PUT"), fetch(
        t,
        {
          body: JSON.stringify(this.form),
          method: n,
          headers: {
            encType: "multipart/form-data",
            "Content-Type": "application/json"
          }
        }
      ).then((a) => {
        if (a.ok)
          this.transientMessage = "The form saved successfully", this.transientMessageClass = "success";
        else
          switch (this.transientMessageClass = "danger", a.status) {
            case 400:
              this.transientMessage = "Bad request. Failed to save the form";
              break;
            case 404:
              this.transientMessage = "Form not found";
              break;
            case 500:
              this.transientMessage = "An internal server error occurred. Failed to save the form";
              break;
            default:
              this.transientMessage = "Unknown error occured. Failed to save the form";
              break;
          }
      }).catch((a) => {
        this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("Form Save API Error:", a);
      });
    },
    clearMessages() {
      this.transientMessage = null;
    }
  }
}), Vu = ["onUpdate:modelValue"], Uu = { class: "col-sm-2" }, Si = /* @__PURE__ */ K({
  __name: "CustomOptions",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = he(() => {
      var i;
      return (i = n.formData.fieldData) == null ? void 0 : i.find((s) => s.fieldId == t.model.id);
    }), o = (i) => {
      var s;
      console.log(i), (s = r.value.customOptionValues) == null || s.splice(i, 1);
    }, a = () => {
      r.value.customOptionValues || (r.value.customOptionValues = []), r.value.customOptionValues.push("");
    };
    return (i, s) => {
      const l = ge("font-awesome-icon");
      return S(), x(q, null, [
        C("div", null, [
          (S(!0), x(q, null, Ce(_(r).customOptionValues, (u, c) => (S(), x("span", {
            class: "custom-option",
            key: u.id
          }, [
            Ee(C("input", {
              type: "text",
              "onUpdate:modelValue": (d) => _(r).customOptionValues[c] = d
            }, null, 8, Vu), [
              [xe, _(r).customOptionValues[c]]
            ]),
            B(l, {
              icon: "fa-solid fa-circle-xmark",
              onClick: (d) => o(c),
              class: "fa-icon delete"
            }, null, 8, ["onClick"])
          ]))), 128))
        ]),
        C("div", Uu, [
          B(l, {
            icon: "fa-solid fa-circle-plus",
            onClick: s[0] || (s[0] = (u) => a()),
            class: "fa-icon plus add-option"
          })
        ])
      ], 64);
    };
  }
}), Lu = ["checked", "onChange"], Bu = { key: 0 }, Wu = /* @__PURE__ */ K({
  __name: "Checkboxes",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = he(() => {
      var i;
      return (i = n.formData.fieldData) == null ? void 0 : i.find((s) => s.fieldId == t.model.id);
    }), o = (i) => {
      var s;
      return (s = r.value.selectedOptionIds) == null ? void 0 : s.includes(i);
    }, a = (i, s) => {
      var l, u, c;
      return s ? (l = r.value.selectedOptionIds) == null ? void 0 : l.push(i) : (c = r.value.selectedOptionIds) == null ? void 0 : c.splice((u = r.value.selectedOptionIds) == null ? void 0 : u.indexOf(i), 1);
    };
    return (i, s) => (S(), x(q, null, [
      (S(!0), x(q, null, Ce(e.model.options, (l) => (S(), x("div", {
        key: l.id,
        class: "option-field"
      }, [
        C("input", {
          type: "checkbox",
          checked: o(l.id),
          onChange: (u) => a(l.id, u.target.checked)
        }, null, 40, Lu),
        He(" " + se(Rt(l, _(n).lang)) + " ", 1),
        l.isExtendedInput != _(or).None ? (S(), x("span", Bu)) : Z("", !0)
      ]))), 128)),
      B(Si, { model: e.model }, null, 8, ["model"])
    ], 64));
  }
}), $u = { class: "col-sm-8" }, zu = { id: "dataOptions" }, Gu = { class: "col-sm-2" }, ju = /* @__PURE__ */ K({
  __name: "DataList",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = he(() => {
      var s;
      return (s = n.formData.fieldData) == null ? void 0 : s.find((l) => l.fieldId == t.model.id);
    }), o = (s) => {
      var u, c;
      const l = (c = (u = t.model) == null ? void 0 : u.options) == null ? void 0 : c.filter((d) => d.id === s).at(0);
      return l ? Rt(l, n.lang) : "";
    }, a = (s) => {
      var u, c;
      const l = (c = (u = t.model) == null ? void 0 : u.options) == null ? void 0 : c.filter((d) => Rt(d, n.lang) === s).at(0);
      return l == null ? void 0 : l.id;
    }, i = he({
      get: () => {
        var s, l;
        return o((l = (s = r == null ? void 0 : r.value) == null ? void 0 : s.selectedOptionIds) == null ? void 0 : l.at(0));
      },
      set: (s) => {
        const l = a(s);
        l ? r.value.selectedOptionIds = [l] : r.value.selectedOptionIds = [];
      }
    });
    return (s, l) => {
      const u = ge("b-form-input");
      return S(), x(q, null, [
        C("div", $u, [
          B(u, {
            list: "dataOptions",
            id: "model.id",
            name: "model.id",
            modelValue: _(i),
            "onUpdate:modelValue": l[0] || (l[0] = (c) => nt(i) ? i.value = c : null)
          }, null, 8, ["modelValue"]),
          C("datalist", zu, [
            (S(!0), x(q, null, Ce(e.model.options, (c) => (S(), x("option", {
              key: c.id
            }, se(Rt(c, _(n).lang)), 1))), 128))
          ])
        ]),
        C("div", Gu, [
          B(Si, { model: e.model }, null, 8, ["model"])
        ])
      ], 64);
    };
  }
}), Yu = { class: "col-sm-3" }, qu = ["value"], Zu = /* @__PURE__ */ K({
  __name: "DropDown",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = he(() => {
      var a;
      return (a = n.formData.fieldData) == null ? void 0 : a.find((i) => i.fieldId == t.model.id);
    }), o = he({
      get: () => {
        var a;
        return ((a = r == null ? void 0 : r.value) == null ? void 0 : a.selectedOptionIds) && r.value.selectedOptionIds.length > 0 ? r.value.selectedOptionIds[0] : ee.EMPTY;
      },
      set: (a) => r.value.selectedOptionIds = [a]
    });
    return (a, i) => (S(), x("div", Yu, [
      Ee(C("select", {
        "onUpdate:modelValue": i[0] || (i[0] = (s) => nt(o) ? o.value = s : null),
        class: "form-select"
      }, [
        (S(!0), x(q, null, Ce(e.model.options, (s) => (S(), x("option", {
          key: s.id,
          value: s.id
        }, se(Rt(s, _(n).lang)), 9, qu))), 128))
      ], 512), [
        [ei, _(o)]
      ])
    ]));
  }
}), Xu = ["value"], Ku = /* @__PURE__ */ K({
  __name: "RadioButtons",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = he(() => {
      var a;
      return (a = n.formData.fieldData) == null ? void 0 : a.find((i) => i.fieldId == t.model.id);
    }), o = he({
      get: () => {
        var a;
        return ((a = r == null ? void 0 : r.value) == null ? void 0 : a.selectedOptionIds) && r.value.selectedOptionIds.length > 0 ? r.value.selectedOptionIds[0] : ee.EMPTY;
      },
      set: (a) => r.value.selectedOptionIds = [a]
    });
    return (a, i) => (S(), x(q, null, [
      (S(!0), x(q, null, Ce(e.model.options, (s) => (S(), x("div", {
        key: s.id,
        class: "option-field"
      }, [
        Ee(C("input", {
          type: "radio",
          name: "model.id",
          value: s.id,
          "onUpdate:modelValue": i[0] || (i[0] = (l) => nt(o) ? o.value = l : null)
        }, null, 8, Xu), [
          [Xs, _(o)]
        ]),
        He(" " + se(Rt(s, _(n).lang)), 1)
      ]))), 128)),
      He(" " + se(_(r)), 1)
    ], 64));
  }
}), Ju = { key: 0 }, Qu = { key: 1 }, ec = { key: 2 }, tc = { key: 3 }, nc = { key: 4 }, rc = { key: 5 }, oc = { key: 6 }, ac = { key: 7 }, wi = /* @__PURE__ */ K({
  __name: "Text",
  props: {
    model: null,
    textType: null,
    decimalPoints: null
  },
  setup(e) {
    const t = e, n = t.decimalPoints ? t.decimalPoints : 2;
    return (r, o) => {
      const a = ge("b-form-input"), i = ge("b-form-textarea");
      return S(), x(q, null, [
        e.textType === _(z).ShortAnswer ? (S(), x("div", Ju, [
          B(a, {
            modelValue: e.model.value,
            "onUpdate:modelValue": o[0] || (o[0] = (s) => e.model.value = s)
          }, null, 8, ["modelValue"])
        ])) : e.textType === _(z).Paragraph ? (S(), x("div", Qu, [
          B(i, {
            modelValue: e.model.value,
            "onUpdate:modelValue": o[1] || (o[1] = (s) => e.model.value = s),
            rows: "3",
            "max-rows": "6"
          }, null, 8, ["modelValue"])
        ])) : e.textType === _(z).RichText ? (S(), x("div", ec, [
          Ee(C("textarea", {
            "onUpdate:modelValue": o[2] || (o[2] = (s) => e.model.value = s),
            class: "field-text-area"
          }, null, 512), [
            [xe, e.model.value]
          ])
        ])) : Z("", !0),
        e.textType === _(z).Email ? (S(), x("div", tc, [
          B(a, {
            modelValue: e.model.value,
            "onUpdate:modelValue": o[3] || (o[3] = (s) => e.model.value = s),
            type: "email"
          }, null, 8, ["modelValue"])
        ])) : Z("", !0),
        e.textType === _(z).Integer ? (S(), x("div", nc, [
          B(a, {
            type: "number",
            step: "1",
            modelValue: e.model.value,
            "onUpdate:modelValue": o[4] || (o[4] = (s) => e.model.value = s)
          }, null, 8, ["modelValue"])
        ])) : Z("", !0),
        e.textType === _(z).Decimal ? (S(), x("div", rc, [
          B(a, {
            type: "number",
            step: Math.pow(10, -_(n)),
            modelValue: e.model.value,
            "onUpdate:modelValue": o[5] || (o[5] = (s) => e.model.value = s)
          }, null, 8, ["step", "modelValue"])
        ])) : Z("", !0),
        e.textType === _(z).Date ? (S(), x("div", oc, [
          B(a, {
            modelValue: e.model.value,
            "onUpdate:modelValue": o[6] || (o[6] = (s) => e.model.value = s),
            type: "date"
          }, null, 8, ["modelValue"])
        ])) : Z("", !0),
        e.textType === _(z).DateTime ? (S(), x("div", ac, [
          Ee(C("input", {
            type: "datetime-local",
            "onUpdate:modelValue": o[7] || (o[7] = (s) => e.model.value = s),
            class: "col-sm-8"
          }, null, 512), [
            [xe, e.model.value]
          ])
        ])) : Z("", !0)
      ], 64);
    };
  }
}), ic = /* @__PURE__ */ K({
  __name: "TextCollection",
  props: {
    model: null,
    textType: null
  },
  setup(e) {
    return (t, n) => (S(!0), x(q, null, Ce(e.model.values, (r) => (S(), pe(wi, {
      key: r.id,
      model: r,
      "text-type": e.textType
    }, null, 8, ["model", "text-type"]))), 128));
  }
}), sc = ["model"], lc = { class: "col col-sm-11" }, uc = { class: "col col-sm-1" }, cc = { key: 0 }, dc = /* @__PURE__ */ K({
  __name: "MultilingualTextInput",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = he(() => {
      var i;
      return (i = n.formData.fieldData) == null ? void 0 : i.find((s) => s.fieldId == t.model.id);
    }), o = () => {
      var i;
      return (i = r.value.multilingualTextValues) == null ? void 0 : i.push(At(n.lang));
    }, a = (i) => {
      var s;
      console.log(i), (s = r.value.multilingualTextValues) == null || s.splice(i, 1);
    };
    return (i, s) => {
      const l = ge("font-awesome-icon");
      return S(), x(q, null, [
        (S(!0), x(q, null, Ce(_(r).multilingualTextValues, (u) => (S(), x("div", {
          key: u.id,
          model: u,
          class: "row mb-3"
        }, [
          C("div", lc, [
            B(ic, {
              model: u,
              "text-type": e.model.type
            }, null, 8, ["model", "text-type"])
          ]),
          C("div", uc, [
            _(r).multilingualTextValues.length > 1 ? (S(), x("div", cc, [
              B(l, {
                icon: "fa-solid fa-circle-xmark",
                onClick: (c) => a(u.id),
                class: "fa-icon delete"
              }, null, 8, ["onClick"])
            ])) : Z("", !0)
          ])
        ], 8, sc))), 128)),
        C("div", null, [
          B(l, {
            icon: "fa-solid fa-circle-plus",
            onClick: s[0] || (s[0] = (u) => o()),
            class: "fa-icon plus add-option"
          })
        ])
      ], 64);
    };
  }
}), fc = ["model"], pc = { class: "col col-sm-11" }, hc = {
  key: 0,
  class: "col-sm-1"
}, vc = { class: "col-sm-1" }, mc = /* @__PURE__ */ K({
  __name: "MonolingualTextInput",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = he(() => {
      var i;
      return (i = n.formData.fieldData) == null ? void 0 : i.find((s) => s.fieldId == t.model.id);
    }), o = () => {
      var i;
      return (i = r.value.monolingualTextValues) == null ? void 0 : i.push(ri(null));
    }, a = (i) => {
      var s;
      (s = r.value.monolingualTextValues) == null || s.splice(i, 1);
    };
    return (i, s) => {
      const l = ge("font-awesome-icon");
      return S(), x(q, null, [
        (S(!0), x(q, null, Ce(_(r).monolingualTextValues, (u) => (S(), x("div", {
          key: u.id,
          model: u,
          class: "row mb-3"
        }, [
          C("div", pc, [
            B(wi, {
              model: u,
              "text-type": e.model.type
            }, null, 8, ["model", "text-type"])
          ]),
          _(r).monolingualTextValues.length > 1 ? (S(), x("div", hc, [
            B(l, {
              icon: "fa-solid fa-circle-xmark",
              onClick: (c) => a(u.id),
              class: "fa-icon delete"
            }, null, 8, ["onClick"])
          ])) : Z("", !0)
        ], 8, fc))), 128)),
        C("div", vc, [
          B(l, {
            icon: "fa-solid fa-circle-plus",
            onClick: s[0] || (s[0] = (u) => o()),
            class: "fa-icon plus add-option"
          })
        ]),
        He(" " + se(_(r)), 1)
      ], 64);
    };
  }
}), gc = /* @__PURE__ */ C("br", null, null, -1), yc = ["innerHTML"], bc = /* @__PURE__ */ K({
  __name: "InfoSection",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = li(t.model, n.lang);
    return (o, a) => (S(), x("div", null, [
      gc,
      C("div", {
        innerHTML: _(r),
        class: "alert alert-info"
      }, null, 8, yc)
    ]));
  }
}), Ec = {
  key: 0,
  class: "alert alert-info"
}, _c = { class: "text-field-lable" }, Cc = ["data-hover"], Dc = /* @__PURE__ */ He(" : "), Sc = /* @__PURE__ */ C("br", null, null, -1), wc = /* @__PURE__ */ K({
  __name: "Field",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = Ve(), r = ol(t.model, n.lang), o = li(t.model, n.lang), a = ii(t.model), i = si(t.model);
    return (s, l) => {
      const u = ge("font-awesome-icon"), c = ge("b-col"), d = ge("b-row"), f = ge("b-container");
      return S(), pe(f, null, {
        default: Be(() => [
          B(d, null, {
            default: Be(() => [
              e.model.type === _(z).InfoSection ? (S(), x("div", Ec, [
                C("h3", _c, se(_(r)), 1)
              ])) : (S(), pe(c, {
                key: 1,
                class: "col-sm-2"
              }, {
                default: Be(() => [
                  He(se(_(r)) + " ", 1),
                  _(o) ? (S(), x("span", {
                    key: 0,
                    class: "hovertext",
                    "data-hover": _(o)
                  }, [
                    B(u, {
                      icon: "fas fa-question-circle",
                      class: "fas fa-question-circle"
                    })
                  ], 8, Cc)) : Z("", !0),
                  Dc
                ]),
                _: 1
              })),
              B(c, { class: "col-sm-10" }, {
                default: Be(() => [
                  e.model.type === _(z).Checkboxes ? (S(), pe(Wu, {
                    key: 0,
                    model: e.model
                  }, null, 8, ["model"])) : Z("", !0),
                  e.model.type === _(z).DataList ? (S(), pe(ju, {
                    key: 1,
                    model: e.model
                  }, null, 8, ["model"])) : Z("", !0),
                  e.model.type === _(z).DropDown ? (S(), pe(Zu, {
                    key: 2,
                    model: e.model
                  }, null, 8, ["model"])) : Z("", !0),
                  e.model.type === _(z).RadioButtons ? (S(), pe(Ku, {
                    key: 3,
                    model: e.model
                  }, null, 8, ["model"])) : Z("", !0),
                  _(a) ? (S(), pe(dc, {
                    key: 4,
                    model: e.model
                  }, null, 8, ["model"])) : Z("", !0),
                  _(i) ? (S(), pe(mc, {
                    key: 5,
                    model: e.model
                  }, null, 8, ["model"])) : Z("", !0),
                  e.model.type === _(z).InfoSection ? (S(), pe(bc, {
                    key: 6,
                    model: e.model
                  }, null, 8, ["model"])) : Z("", !0)
                ]),
                _: 1
              })
            ]),
            _: 1
          }),
          Sc
        ]),
        _: 1
      });
    };
  }
}), Tc = /* @__PURE__ */ K({
  __name: "Form",
  props: {
    model: null
  },
  setup(e) {
    return (t, n) => {
      var r;
      return S(!0), x(q, null, Ce((r = e.model) == null ? void 0 : r.fields, (o) => (S(), pe(wc, {
        key: o.id,
        model: o
      }, null, 8, ["model"]))), 128);
    };
  }
}), Ti = (e) => (er("data-v-4acd9d0f"), e = e(), tr(), e), Rc = /* @__PURE__ */ Ti(() => /* @__PURE__ */ C("h2", null, "Form Submission", -1)), kc = /* @__PURE__ */ Ti(() => /* @__PURE__ */ C("hr", null, null, -1)), Ic = { class: "control" }, xc = ["disabled"], Mc = /* @__PURE__ */ K({
  __name: "App",
  props: {
    piniaInstance: null,
    repositoryRoot: null,
    formId: null,
    submissionId: null
  },
  setup(e) {
    const t = e, n = Ve(t.piniaInstance);
    t.formId ? n.loadForm(t.formId) : t.submissionId && n.loadSubmission(t.submissionId), Qn(() => n.transientMessage, async (a) => {
      a && setTimeout(() => {
        n.transientMessage = null;
      }, 2e3);
    });
    const r = () => n.submitForm(), o = he(() => !!n.form);
    return (a, i) => (S(), x(q, null, [
      B(lo, { name: "fade" }, {
        default: Be(() => [
          _(n).transientMessage ? (S(), x("p", {
            key: 0,
            class: uo("alert alert-" + _(n).transientMessageClass)
          }, se(_(n).transientMessage), 3)) : Z("", !0)
        ]),
        _: 1
      }),
      Rc,
      kc,
      _(n).form ? (S(), pe(Tc, {
        key: 0,
        model: _(n).form
      }, null, 8, ["model"])) : Z("", !0),
      C("div", Ic, [
        C("button", {
          type: "button",
          class: "btn btn-primary",
          disabled: !_(o),
          onClick: r
        }, "Submit", 8, xc)
      ])
    ], 64));
  }
});
const $m = /* @__PURE__ */ sr(Mc, [["__scopeId", "data-v-4acd9d0f"]]), Pc = sn("LoginStore", {
  state: () => ({
    authorizationApiRoot: null,
    loginResult: null
  }),
  actions: {
    authorize(e) {
      var n;
      if (!e) {
        console.error("JWT token is null.");
        return;
      }
      if (!this.authorizationApiRoot) {
        console.error("Authorization service root is not specified.");
        return;
      }
      const t = ((n = this.authorizationApiRoot) == null ? void 0 : n.replace(/\/+$/, "")) + "/api/GoogleIdentity";
      fetch(
        t,
        {
          body: JSON.stringify(e),
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          }
        }
      ).then((r) => r.json()).then((r) => {
        r.success ? this.loginResult = r : (this.loginResult = r, console.error("User authorization not successful."));
      }).catch((r) => {
        this.loginResult = {}, console.error("User authorization failed: ", r);
      });
    }
  }
}), Oc = /* @__PURE__ */ C("h2", null, "Login", -1), Ac = /* @__PURE__ */ C("br", null, null, -1), Nc = /* @__PURE__ */ C("br", null, null, -1), zm = /* @__PURE__ */ K({
  __name: "App",
  props: {
    piniaInstance: null,
    authorizationRoot: null
  },
  setup(e) {
    const t = e, n = Pc(t.piniaInstance);
    ti(() => {
      n.authorizationApiRoot = t.authorizationRoot;
    });
    const r = (o) => {
      n.authorize(o.credential);
    };
    return (o, a) => {
      const i = ge("GoogleLogin");
      return S(), x(q, null, [
        Oc,
        Ac,
        B(i, { callback: r }),
        Nc
      ], 64);
    };
  }
}), Hc = sn("WorkflowBuilderStore", {
  state: () => ({
    workflow: null,
    transientMessage: null,
    transientMessageClass: null
  }),
  actions: {
    loadWorkflow(e) {
      const t = `https://localhost:5020/api/workflow/${e}`;
      fetch(t, {
        method: "GET"
      }).then((n) => n.json()).then((n) => {
        this.workflow = n;
      }).catch((n) => {
        console.error("Load Workflow API Error:", n);
      });
    },
    saveWorkflow() {
      var r, o;
      if (!this.workflow) {
        console.error("Cannot save null workflow.");
        return;
      }
      const e = ((o = (r = this.workflow) == null ? void 0 : r.id) == null ? void 0 : o.toString()) === ee.EMPTY;
      let t = "https://localhost:5020/api/workflow", n = "";
      e ? (console.log("Saving new workflow."), n = "POST") : (console.log("Updating existing workflow."), t = `${t}/${this.workflow.id}`, n = "PUT"), fetch(
        t,
        {
          body: JSON.stringify(this.workflow),
          method: n,
          headers: {
            encType: "multipart/form-data",
            "Content-Type": "application/json"
          }
        }
      ).then((a) => {
        if (a.ok)
          this.transientMessage = "The form saved successfully", this.transientMessageClass = "success";
        else
          switch (this.transientMessageClass = "danger", a.status) {
            case 400:
              this.transientMessage = "Bad request. Failed to save the workflow";
              break;
            case 404:
              this.transientMessage = "Workflow not found";
              break;
            case 500:
              this.transientMessage = "An internal server error occurred. Failed to save the workflow";
              break;
            default:
              this.transientMessage = "Unknown error occured. Failed to save the workflow";
              break;
          }
      }).catch((a) => {
        this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("Workflow Save API Error:", a);
      });
    }
  }
}), Ri = (e) => (er("data-v-4ad7f70c"), e = e(), tr(), e), Fc = { class: "action" }, Vc = /* @__PURE__ */ He(" Name: "), Uc = /* @__PURE__ */ He(" Description: "), Lc = /* @__PURE__ */ He(" Form: "), Bc = /* @__PURE__ */ Ri(() => /* @__PURE__ */ C("option", null, "TO DO Form 1", -1)), Wc = /* @__PURE__ */ Ri(() => /* @__PURE__ */ C("option", null, "TO DO Form 2", -1)), $c = [
  Bc,
  Wc
], zc = /* @__PURE__ */ K({
  __name: "WorkflowAction",
  props: {
    model: null
  },
  setup(e) {
    return (t, n) => (S(), x("div", Fc, [
      He(" Workflow Action: " + se(e.model.id) + " ", 1),
      C("div", null, [
        C("h4", null, se(e.model.name), 1),
        Vc,
        Ee(C("input", {
          type: "text",
          "onUpdate:modelValue": n[0] || (n[0] = (r) => e.model.name = r)
        }, null, 512), [
          [xe, e.model.name]
        ])
      ]),
      C("div", null, [
        C("p", null, se(e.model.description), 1),
        Uc,
        Ee(C("textarea", {
          "onUpdate:modelValue": n[1] || (n[1] = (r) => e.model.description = r)
        }, null, 512), [
          [xe, e.model.description]
        ])
      ]),
      C("div", null, [
        C("p", null, se(e.model.description), 1),
        Lc,
        Ee(C("select", {
          "onUpdate:modelValue": n[2] || (n[2] = (r) => e.model.formId = r)
        }, $c, 512), [
          [ei, e.model.formId]
        ])
      ])
    ]));
  }
});
const Gc = /* @__PURE__ */ sr(zc, [["__scopeId", "data-v-4ad7f70c"]]), jc = /* @__PURE__ */ K({
  __name: "Workflow",
  props: {
    model: null
  },
  setup(e) {
    return (t, n) => (S(!0), x(q, null, Ce(e.model.actions, (r) => (S(), pe(Gc, {
      key: r.id,
      model: r
    }, null, 8, ["model"]))), 128));
  }
}), ki = (e) => (er("data-v-493b52a9"), e = e(), tr(), e), Yc = /* @__PURE__ */ ki(() => /* @__PURE__ */ C("h2", null, "Workflow Builder", -1)), qc = { class: "control" }, Zc = ["disabled"], Xc = ["disabled"], Kc = { class: "toolbar" }, Jc = ["disabled"], Qc = /* @__PURE__ */ ki(() => /* @__PURE__ */ C("hr", null, null, -1)), ed = /* @__PURE__ */ K({
  __name: "App",
  props: {
    piniaInstance: null,
    repositoryRoot: null,
    workflowId: null
  },
  setup(e) {
    const t = e, n = Hc(t.piniaInstance);
    t.workflowId && n.loadWorkflow(t.workflowId), Qn(() => n.transientMessage, async (i) => {
      i && setTimeout(() => {
        n.transientMessage = null;
      }, 2e3);
    });
    const r = () => {
      n.workflow = {
        id: ee.EMPTY,
        name: "",
        description: "",
        states: []
      };
    }, o = he(() => !n.workflow), a = () => {
      if (!n.workflow) {
        console.error("Cannot add action to null workflow");
        return;
      }
      const i = {
        id: ee.create().toString(),
        title: "",
        description: "",
        formId: ee.createEmpty()
      };
      n.workflow.actions ? n.workflow.actions.push(i) : n.workflow.actions = [i];
    };
    return (i, s) => (S(), x(q, null, [
      B(lo, { name: "fade" }, {
        default: Be(() => [
          _(n).transientMessage ? (S(), x("p", {
            key: 0,
            class: uo("alert alert-" + _(n).transientMessageClass)
          }, se(_(n).transientMessage), 3)) : Z("", !0)
        ]),
        _: 1
      }),
      Yc,
      C("div", qc, [
        C("button", {
          type: "button",
          class: "btn btn-primary",
          disabled: !_(o),
          onClick: r
        }, "New Workflow", 8, Zc),
        C("button", {
          type: "button",
          class: "btn btn-success",
          disabled: _(o),
          onClick: s[0] || (s[0] = (...l) => i.saveForm && i.saveForm(...l))
        }, "Save", 8, Xc)
      ]),
      C("div", Kc, [
        C("button", {
          disabled: _(o),
          onClick: s[1] || (s[1] = (l) => a())
        }, "+ Form Submission Action", 8, Jc)
      ]),
      Qc,
      _(n).workflow ? (S(), pe(jc, {
        key: 0,
        model: _(n).workflow
      }, null, 8, ["model"])) : Z("", !0)
    ], 64));
  }
});
const Gm = /* @__PURE__ */ sr(ed, [["__scopeId", "data-v-493b52a9"]]);
var Xr = function(e, t) {
  return Xr = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function(n, r) {
    n.__proto__ = r;
  } || function(n, r) {
    for (var o in r)
      Object.prototype.hasOwnProperty.call(r, o) && (n[o] = r[o]);
  }, Xr(e, t);
};
function H(e, t) {
  if (typeof t != "function" && t !== null)
    throw new TypeError("Class extends value " + String(t) + " is not a constructor or null");
  Xr(e, t);
  function n() {
    this.constructor = e;
  }
  e.prototype = t === null ? Object.create(t) : (n.prototype = t.prototype, new n());
}
var v = function() {
  return v = Object.assign || function(t) {
    for (var n, r = 1, o = arguments.length; r < o; r++) {
      n = arguments[r];
      for (var a in n)
        Object.prototype.hasOwnProperty.call(n, a) && (t[a] = n[a]);
    }
    return t;
  }, v.apply(this, arguments);
};
function re(e, t, n) {
  if (n || arguments.length === 2)
    for (var r = 0, o = t.length, a; r < o; r++)
      (a || !(r in t)) && (a || (a = Array.prototype.slice.call(t, 0, r)), a[r] = t[r]);
  return e.concat(a || Array.prototype.slice.call(t));
}
var lr, P, Ii, Jt, Qo, xi, Ln = {}, Mi = [], td = /acit|ex(?:s|g|n|p|$)|rph|grid|ows|mnc|ntw|ine[ch]|zoo|^ord|itera/i;
function Je(e, t) {
  for (var n in t)
    e[n] = t[n];
  return e;
}
function Pi(e) {
  var t = e.parentNode;
  t && t.removeChild(e);
}
function rt(e, t, n) {
  var r, o, a, i = {};
  for (a in t)
    a == "key" ? r = t[a] : a == "ref" ? o = t[a] : i[a] = t[a];
  if (arguments.length > 2 && (i.children = arguments.length > 3 ? lr.call(arguments, 2) : n), typeof e == "function" && e.defaultProps != null)
    for (a in e.defaultProps)
      i[a] === void 0 && (i[a] = e.defaultProps[a]);
  return xn(e, i, r, o, null);
}
function xn(e, t, n, r, o) {
  var a = { type: e, props: t, key: n, ref: r, __k: null, __: null, __b: 0, __e: null, __d: void 0, __c: null, __h: null, constructor: void 0, __v: o == null ? ++Ii : o };
  return o == null && P.vnode != null && P.vnode(a), a;
}
function nd() {
  return { current: null };
}
function ht(e) {
  return e.children;
}
function Me(e, t) {
  this.props = e, this.context = t;
}
function Ht(e, t) {
  if (t == null)
    return e.__ ? Ht(e.__, e.__.__k.indexOf(e) + 1) : null;
  for (var n; t < e.__k.length; t++)
    if ((n = e.__k[t]) != null && n.__e != null)
      return n.__e;
  return typeof e.type == "function" ? Ht(e) : null;
}
function Oi(e) {
  var t, n;
  if ((e = e.__) != null && e.__c != null) {
    for (e.__e = e.__c.base = null, t = 0; t < e.__k.length; t++)
      if ((n = e.__k[t]) != null && n.__e != null) {
        e.__e = e.__c.base = n.__e;
        break;
      }
    return Oi(e);
  }
}
function Kr(e) {
  (!e.__d && (e.__d = !0) && Jt.push(e) && !Bn.__r++ || Qo !== P.debounceRendering) && ((Qo = P.debounceRendering) || setTimeout)(Bn);
}
function Bn() {
  for (var e; Bn.__r = Jt.length; )
    e = Jt.sort(function(t, n) {
      return t.__v.__b - n.__v.__b;
    }), Jt = [], e.some(function(t) {
      var n, r, o, a, i, s;
      t.__d && (i = (a = (n = t).__v).__e, (s = n.__P) && (r = [], (o = Je({}, a)).__v = a.__v + 1, mo(s, a, o, n.__n, s.ownerSVGElement !== void 0, a.__h != null ? [i] : null, r, i == null ? Ht(a) : i, a.__h), Fi(r, a), a.__e != i && Oi(a)));
    });
}
function Ai(e, t, n, r, o, a, i, s, l, u) {
  var c, d, f, p, h, b, E, y = r && r.__k || Mi, D = y.length;
  for (n.__k = [], c = 0; c < t.length; c++)
    if ((p = n.__k[c] = (p = t[c]) == null || typeof p == "boolean" ? null : typeof p == "string" || typeof p == "number" || typeof p == "bigint" ? xn(null, p, null, null, p) : Array.isArray(p) ? xn(ht, { children: p }, null, null, null) : p.__b > 0 ? xn(p.type, p.props, p.key, null, p.__v) : p) != null) {
      if (p.__ = n, p.__b = n.__b + 1, (f = y[c]) === null || f && p.key == f.key && p.type === f.type)
        y[c] = void 0;
      else
        for (d = 0; d < D; d++) {
          if ((f = y[d]) && p.key == f.key && p.type === f.type) {
            y[d] = void 0;
            break;
          }
          f = null;
        }
      mo(e, p, f = f || Ln, o, a, i, s, l, u), h = p.__e, (d = p.ref) && f.ref != d && (E || (E = []), f.ref && E.push(f.ref, null, p), E.push(d, p.__c || h, p)), h != null ? (b == null && (b = h), typeof p.type == "function" && p.__k === f.__k ? p.__d = l = Ni(p, l, e) : l = Hi(e, p, f, y, h, l), typeof n.type == "function" && (n.__d = l)) : l && f.__e == l && l.parentNode != e && (l = Ht(f));
    }
  for (n.__e = b, c = D; c--; )
    y[c] != null && (typeof n.type == "function" && y[c].__e != null && y[c].__e == n.__d && (n.__d = Ht(r, c + 1)), Ui(y[c], y[c]));
  if (E)
    for (c = 0; c < E.length; c++)
      Vi(E[c], E[++c], E[++c]);
}
function Ni(e, t, n) {
  for (var r, o = e.__k, a = 0; o && a < o.length; a++)
    (r = o[a]) && (r.__ = e, t = typeof r.type == "function" ? Ni(r, t, n) : Hi(n, r, r, o, r.__e, t));
  return t;
}
function Wn(e, t) {
  return t = t || [], e == null || typeof e == "boolean" || (Array.isArray(e) ? e.some(function(n) {
    Wn(n, t);
  }) : t.push(e)), t;
}
function Hi(e, t, n, r, o, a) {
  var i, s, l;
  if (t.__d !== void 0)
    i = t.__d, t.__d = void 0;
  else if (n == null || o != a || o.parentNode == null)
    e:
      if (a == null || a.parentNode !== e)
        e.appendChild(o), i = null;
      else {
        for (s = a, l = 0; (s = s.nextSibling) && l < r.length; l += 2)
          if (s == o)
            break e;
        e.insertBefore(o, a), i = a;
      }
  return i !== void 0 ? i : o.nextSibling;
}
function rd(e, t, n, r, o) {
  var a;
  for (a in n)
    a === "children" || a === "key" || a in t || $n(e, a, null, n[a], r);
  for (a in t)
    o && typeof t[a] != "function" || a === "children" || a === "key" || a === "value" || a === "checked" || n[a] === t[a] || $n(e, a, t[a], n[a], r);
}
function ea(e, t, n) {
  t[0] === "-" ? e.setProperty(t, n) : e[t] = n == null ? "" : typeof n != "number" || td.test(t) ? n : n + "px";
}
function $n(e, t, n, r, o) {
  var a;
  e:
    if (t === "style")
      if (typeof n == "string")
        e.style.cssText = n;
      else {
        if (typeof r == "string" && (e.style.cssText = r = ""), r)
          for (t in r)
            n && t in n || ea(e.style, t, "");
        if (n)
          for (t in n)
            r && n[t] === r[t] || ea(e.style, t, n[t]);
      }
    else if (t[0] === "o" && t[1] === "n")
      a = t !== (t = t.replace(/Capture$/, "")), t = t.toLowerCase() in e ? t.toLowerCase().slice(2) : t.slice(2), e.l || (e.l = {}), e.l[t + a] = n, n ? r || e.addEventListener(t, a ? na : ta, a) : e.removeEventListener(t, a ? na : ta, a);
    else if (t !== "dangerouslySetInnerHTML") {
      if (o)
        t = t.replace(/xlink(H|:h)/, "h").replace(/sName$/, "s");
      else if (t !== "href" && t !== "list" && t !== "form" && t !== "tabIndex" && t !== "download" && t in e)
        try {
          e[t] = n == null ? "" : n;
          break e;
        } catch {
        }
      typeof n == "function" || (n != null && (n !== !1 || t[0] === "a" && t[1] === "r") ? e.setAttribute(t, n) : e.removeAttribute(t));
    }
}
function ta(e) {
  this.l[e.type + !1](P.event ? P.event(e) : e);
}
function na(e) {
  this.l[e.type + !0](P.event ? P.event(e) : e);
}
function mo(e, t, n, r, o, a, i, s, l) {
  var u, c, d, f, p, h, b, E, y, D, k, T, U, O = t.type;
  if (t.constructor !== void 0)
    return null;
  n.__h != null && (l = n.__h, s = t.__e = n.__e, t.__h = null, a = [s]), (u = P.__b) && u(t);
  try {
    e:
      if (typeof O == "function") {
        if (E = t.props, y = (u = O.contextType) && r[u.__c], D = u ? y ? y.props.value : u.__ : r, n.__c ? b = (c = t.__c = n.__c).__ = c.__E : ("prototype" in O && O.prototype.render ? t.__c = c = new O(E, D) : (t.__c = c = new Me(E, D), c.constructor = O, c.render = ad), y && y.sub(c), c.props = E, c.state || (c.state = {}), c.context = D, c.__n = r, d = c.__d = !0, c.__h = []), c.__s == null && (c.__s = c.state), O.getDerivedStateFromProps != null && (c.__s == c.state && (c.__s = Je({}, c.__s)), Je(c.__s, O.getDerivedStateFromProps(E, c.__s))), f = c.props, p = c.state, d)
          O.getDerivedStateFromProps == null && c.componentWillMount != null && c.componentWillMount(), c.componentDidMount != null && c.__h.push(c.componentDidMount);
        else {
          if (O.getDerivedStateFromProps == null && E !== f && c.componentWillReceiveProps != null && c.componentWillReceiveProps(E, D), !c.__e && c.shouldComponentUpdate != null && c.shouldComponentUpdate(E, c.__s, D) === !1 || t.__v === n.__v) {
            c.props = E, c.state = c.__s, t.__v !== n.__v && (c.__d = !1), c.__v = t, t.__e = n.__e, t.__k = n.__k, t.__k.forEach(function(R) {
              R && (R.__ = t);
            }), c.__h.length && i.push(c);
            break e;
          }
          c.componentWillUpdate != null && c.componentWillUpdate(E, c.__s, D), c.componentDidUpdate != null && c.__h.push(function() {
            c.componentDidUpdate(f, p, h);
          });
        }
        if (c.context = D, c.props = E, c.__v = t, c.__P = e, k = P.__r, T = 0, "prototype" in O && O.prototype.render)
          c.state = c.__s, c.__d = !1, k && k(t), u = c.render(c.props, c.state, c.context);
        else
          do
            c.__d = !1, k && k(t), u = c.render(c.props, c.state, c.context), c.state = c.__s;
          while (c.__d && ++T < 25);
        c.state = c.__s, c.getChildContext != null && (r = Je(Je({}, r), c.getChildContext())), d || c.getSnapshotBeforeUpdate == null || (h = c.getSnapshotBeforeUpdate(f, p)), U = u != null && u.type === ht && u.key == null ? u.props.children : u, Ai(e, Array.isArray(U) ? U : [U], t, n, r, o, a, i, s, l), c.base = t.__e, t.__h = null, c.__h.length && i.push(c), b && (c.__E = c.__ = null), c.__e = !1;
      } else
        a == null && t.__v === n.__v ? (t.__k = n.__k, t.__e = n.__e) : t.__e = od(n.__e, t, n, r, o, a, i, l);
    (u = P.diffed) && u(t);
  } catch (R) {
    t.__v = null, (l || a != null) && (t.__e = s, t.__h = !!l, a[a.indexOf(s)] = null), P.__e(R, t, n);
  }
}
function Fi(e, t) {
  P.__c && P.__c(t, e), e.some(function(n) {
    try {
      e = n.__h, n.__h = [], e.some(function(r) {
        r.call(n);
      });
    } catch (r) {
      P.__e(r, n.__v);
    }
  });
}
function od(e, t, n, r, o, a, i, s) {
  var l, u, c, d = n.props, f = t.props, p = t.type, h = 0;
  if (p === "svg" && (o = !0), a != null) {
    for (; h < a.length; h++)
      if ((l = a[h]) && "setAttribute" in l == !!p && (p ? l.localName === p : l.nodeType === 3)) {
        e = l, a[h] = null;
        break;
      }
  }
  if (e == null) {
    if (p === null)
      return document.createTextNode(f);
    e = o ? document.createElementNS("http://www.w3.org/2000/svg", p) : document.createElement(p, f.is && f), a = null, s = !1;
  }
  if (p === null)
    d === f || s && e.data === f || (e.data = f);
  else {
    if (a = a && lr.call(e.childNodes), u = (d = n.props || Ln).dangerouslySetInnerHTML, c = f.dangerouslySetInnerHTML, !s) {
      if (a != null)
        for (d = {}, h = 0; h < e.attributes.length; h++)
          d[e.attributes[h].name] = e.attributes[h].value;
      (c || u) && (c && (u && c.__html == u.__html || c.__html === e.innerHTML) || (e.innerHTML = c && c.__html || ""));
    }
    if (rd(e, f, d, o, s), c)
      t.__k = [];
    else if (h = t.props.children, Ai(e, Array.isArray(h) ? h : [h], t, n, r, o && p !== "foreignObject", a, i, a ? a[0] : n.__k && Ht(n, 0), s), a != null)
      for (h = a.length; h--; )
        a[h] != null && Pi(a[h]);
    s || ("value" in f && (h = f.value) !== void 0 && (h !== e.value || p === "progress" && !h || p === "option" && h !== d.value) && $n(e, "value", h, d.value, !1), "checked" in f && (h = f.checked) !== void 0 && h !== e.checked && $n(e, "checked", h, d.checked, !1));
  }
  return e;
}
function Vi(e, t, n) {
  try {
    typeof e == "function" ? e(t) : e.current = t;
  } catch (r) {
    P.__e(r, n);
  }
}
function Ui(e, t, n) {
  var r, o;
  if (P.unmount && P.unmount(e), (r = e.ref) && (r.current && r.current !== e.__e || Vi(r, null, t)), (r = e.__c) != null) {
    if (r.componentWillUnmount)
      try {
        r.componentWillUnmount();
      } catch (a) {
        P.__e(a, t);
      }
    r.base = r.__P = null;
  }
  if (r = e.__k)
    for (o = 0; o < r.length; o++)
      r[o] && Ui(r[o], t, typeof e.type != "function");
  n || e.__e == null || Pi(e.__e), e.__e = e.__d = void 0;
}
function ad(e, t, n) {
  return this.constructor(e, n);
}
function rn(e, t, n) {
  var r, o, a;
  P.__ && P.__(e, t), o = (r = typeof n == "function") ? null : n && n.__k || t.__k, a = [], mo(t, e = (!r && n || t).__k = rt(ht, null, [e]), o || Ln, Ln, t.ownerSVGElement !== void 0, !r && n ? [n] : o ? null : t.firstChild ? lr.call(t.childNodes) : null, a, !r && n ? n : o ? o.__e : t.firstChild, r), Fi(a, e);
}
function id(e, t) {
  var n = { __c: t = "__cC" + xi++, __: e, Consumer: function(r, o) {
    return r.children(o);
  }, Provider: function(r) {
    var o, a;
    return this.getChildContext || (o = [], (a = {})[t] = this, this.getChildContext = function() {
      return a;
    }, this.shouldComponentUpdate = function(i) {
      this.props.value !== i.value && o.some(Kr);
    }, this.sub = function(i) {
      o.push(i);
      var s = i.componentWillUnmount;
      i.componentWillUnmount = function() {
        o.splice(o.indexOf(i), 1), s && s.call(i);
      };
    }), r.children;
  } };
  return n.Provider.__ = n.Consumer.contextType = n;
}
lr = Mi.slice, P = { __e: function(e, t, n, r) {
  for (var o, a, i; t = t.__; )
    if ((o = t.__c) && !o.__)
      try {
        if ((a = o.constructor) && a.getDerivedStateFromError != null && (o.setState(a.getDerivedStateFromError(e)), i = o.__d), o.componentDidCatch != null && (o.componentDidCatch(e, r || {}), i = o.__d), i)
          return o.__E = o;
      } catch (s) {
        e = s;
      }
  throw e;
} }, Ii = 0, Me.prototype.setState = function(e, t) {
  var n;
  n = this.__s != null && this.__s !== this.state ? this.__s : this.__s = Je({}, this.state), typeof e == "function" && (e = e(Je({}, n), this.props)), e && Je(n, e), e != null && this.__v && (t && this.__h.push(t), Kr(this));
}, Me.prototype.forceUpdate = function(e) {
  this.__v && (this.__e = !0, e && this.__h.push(e), Kr(this));
}, Me.prototype.render = ht, Jt = [], Bn.__r = 0, xi = 0;
var Oe, Rr, ra, Li = [], kr = [], oa = P.__b, aa = P.__r, ia = P.diffed, sa = P.__c, la = P.unmount;
function sd() {
  for (var e; e = Li.shift(); )
    if (e.__P && e.__H)
      try {
        e.__H.__h.forEach(Mn), e.__H.__h.forEach(Jr), e.__H.__h = [];
      } catch (t) {
        e.__H.__h = [], P.__e(t, e.__v);
      }
}
P.__b = function(e) {
  Oe = null, oa && oa(e);
}, P.__r = function(e) {
  aa && aa(e);
  var t = (Oe = e.__c).__H;
  t && (Rr === Oe ? (t.__h = [], Oe.__h = [], t.__.forEach(function(n) {
    n.__N && (n.__ = n.__N), n.__V = kr, n.__N = n.i = void 0;
  })) : (t.__h.forEach(Mn), t.__h.forEach(Jr), t.__h = [])), Rr = Oe;
}, P.diffed = function(e) {
  ia && ia(e);
  var t = e.__c;
  t && t.__H && (t.__H.__h.length && (Li.push(t) !== 1 && ra === P.requestAnimationFrame || ((ra = P.requestAnimationFrame) || function(n) {
    var r, o = function() {
      clearTimeout(a), ua && cancelAnimationFrame(r), setTimeout(n);
    }, a = setTimeout(o, 100);
    ua && (r = requestAnimationFrame(o));
  })(sd)), t.__H.__.forEach(function(n) {
    n.i && (n.__H = n.i), n.__V !== kr && (n.__ = n.__V), n.i = void 0, n.__V = kr;
  })), Rr = Oe = null;
}, P.__c = function(e, t) {
  t.some(function(n) {
    try {
      n.__h.forEach(Mn), n.__h = n.__h.filter(function(r) {
        return !r.__ || Jr(r);
      });
    } catch (r) {
      t.some(function(o) {
        o.__h && (o.__h = []);
      }), t = [], P.__e(r, n.__v);
    }
  }), sa && sa(e, t);
}, P.unmount = function(e) {
  la && la(e);
  var t, n = e.__c;
  n && n.__H && (n.__H.__.forEach(function(r) {
    try {
      Mn(r);
    } catch (o) {
      t = o;
    }
  }), t && P.__e(t, n.__v));
};
var ua = typeof requestAnimationFrame == "function";
function Mn(e) {
  var t = Oe, n = e.__c;
  typeof n == "function" && (e.__c = void 0, n()), Oe = t;
}
function Jr(e) {
  var t = Oe;
  e.__c = e.__(), Oe = t;
}
function ld(e, t) {
  for (var n in t)
    e[n] = t[n];
  return e;
}
function ca(e, t) {
  for (var n in e)
    if (n !== "__source" && !(n in t))
      return !0;
  for (var r in t)
    if (r !== "__source" && e[r] !== t[r])
      return !0;
  return !1;
}
function da(e) {
  this.props = e;
}
(da.prototype = new Me()).isPureReactComponent = !0, da.prototype.shouldComponentUpdate = function(e, t) {
  return ca(this.props, e) || ca(this.state, t);
};
var fa = P.__b;
P.__b = function(e) {
  e.type && e.type.__f && e.ref && (e.props.ref = e.ref, e.ref = null), fa && fa(e);
};
var ud = P.__e;
P.__e = function(e, t, n, r) {
  if (e.then) {
    for (var o, a = t; a = a.__; )
      if ((o = a.__c) && o.__c)
        return t.__e == null && (t.__e = n.__e, t.__k = n.__k), o.__c(e, t);
  }
  ud(e, t, n, r);
};
var pa = P.unmount;
function Ir() {
  this.__u = 0, this.t = null, this.__b = null;
}
function Bi(e) {
  var t = e.__.__c;
  return t && t.__a && t.__a(e);
}
function yn() {
  this.u = null, this.o = null;
}
P.unmount = function(e) {
  var t = e.__c;
  t && t.__R && t.__R(), t && e.__h === !0 && (e.type = null), pa && pa(e);
}, (Ir.prototype = new Me()).__c = function(e, t) {
  var n = t.__c, r = this;
  r.t == null && (r.t = []), r.t.push(n);
  var o = Bi(r.__v), a = !1, i = function() {
    a || (a = !0, n.__R = null, o ? o(s) : s());
  };
  n.__R = i;
  var s = function() {
    if (!--r.__u) {
      if (r.state.__a) {
        var u = r.state.__a;
        r.__v.__k[0] = function d(f, p, h) {
          return f && (f.__v = null, f.__k = f.__k && f.__k.map(function(b) {
            return d(b, p, h);
          }), f.__c && f.__c.__P === p && (f.__e && h.insertBefore(f.__e, f.__d), f.__c.__e = !0, f.__c.__P = h)), f;
        }(u, u.__c.__P, u.__c.__O);
      }
      var c;
      for (r.setState({ __a: r.__b = null }); c = r.t.pop(); )
        c.forceUpdate();
    }
  }, l = t.__h === !0;
  r.__u++ || l || r.setState({ __a: r.__b = r.__v.__k[0] }), e.then(i, i);
}, Ir.prototype.componentWillUnmount = function() {
  this.t = [];
}, Ir.prototype.render = function(e, t) {
  if (this.__b) {
    if (this.__v.__k) {
      var n = document.createElement("div"), r = this.__v.__k[0].__c;
      this.__v.__k[0] = function a(i, s, l) {
        return i && (i.__c && i.__c.__H && (i.__c.__H.__.forEach(function(u) {
          typeof u.__c == "function" && u.__c();
        }), i.__c.__H = null), (i = ld({}, i)).__c != null && (i.__c.__P === l && (i.__c.__P = s), i.__c = null), i.__k = i.__k && i.__k.map(function(u) {
          return a(u, s, l);
        })), i;
      }(this.__b, n, r.__O = r.__P);
    }
    this.__b = null;
  }
  var o = t.__a && rt(ht, null, e.fallback);
  return o && (o.__h = null), [rt(ht, null, t.__a ? null : e.children), o];
};
var ha = function(e, t, n) {
  if (++n[1] === n[0] && e.o.delete(t), e.props.revealOrder && (e.props.revealOrder[0] !== "t" || !e.o.size))
    for (n = e.u; n; ) {
      for (; n.length > 3; )
        n.pop()();
      if (n[1] < n[0])
        break;
      e.u = n = n[2];
    }
};
function cd(e) {
  return this.getChildContext = function() {
    return e.context;
  }, e.children;
}
function dd(e) {
  var t = this, n = e.i;
  t.componentWillUnmount = function() {
    rn(null, t.l), t.l = null, t.i = null;
  }, t.i && t.i !== n && t.componentWillUnmount(), e.__v ? (t.l || (t.i = n, t.l = { nodeType: 1, parentNode: n, childNodes: [], appendChild: function(r) {
    this.childNodes.push(r), t.i.appendChild(r);
  }, insertBefore: function(r, o) {
    this.childNodes.push(r), t.i.appendChild(r);
  }, removeChild: function(r) {
    this.childNodes.splice(this.childNodes.indexOf(r) >>> 1, 1), t.i.removeChild(r);
  } }), rn(rt(cd, { context: t.context }, e.__v), t.l)) : t.l && t.componentWillUnmount();
}
function fd(e, t) {
  var n = rt(dd, { __v: e, i: t });
  return n.containerInfo = t, n;
}
(yn.prototype = new Me()).__a = function(e) {
  var t = this, n = Bi(t.__v), r = t.o.get(e);
  return r[0]++, function(o) {
    var a = function() {
      t.props.revealOrder ? (r.push(o), ha(t, e, r)) : o();
    };
    n ? n(a) : a();
  };
}, yn.prototype.render = function(e) {
  this.u = null, this.o = /* @__PURE__ */ new Map();
  var t = Wn(e.children);
  e.revealOrder && e.revealOrder[0] === "b" && t.reverse();
  for (var n = t.length; n--; )
    this.o.set(t[n], this.u = [1, 0, this.u]);
  return e.children;
}, yn.prototype.componentDidUpdate = yn.prototype.componentDidMount = function() {
  var e = this;
  this.o.forEach(function(t, n) {
    ha(e, n, t);
  });
};
var pd = typeof Symbol < "u" && Symbol.for && Symbol.for("react.element") || 60103, hd = /^(?:accent|alignment|arabic|baseline|cap|clip(?!PathU)|color|dominant|fill|flood|font|glyph(?!R)|horiz|image|letter|lighting|marker(?!H|W|U)|overline|paint|pointer|shape|stop|strikethrough|stroke|text(?!L)|transform|underline|unicode|units|v|vector|vert|word|writing|x(?!C))[A-Z]/, vd = typeof document < "u", md = function(e) {
  return (typeof Symbol < "u" && typeof Symbol() == "symbol" ? /fil|che|rad/i : /fil|che|ra/i).test(e);
};
Me.prototype.isReactComponent = {}, ["componentWillMount", "componentWillReceiveProps", "componentWillUpdate"].forEach(function(e) {
  Object.defineProperty(Me.prototype, e, { configurable: !0, get: function() {
    return this["UNSAFE_" + e];
  }, set: function(t) {
    Object.defineProperty(this, e, { configurable: !0, writable: !0, value: t });
  } });
});
var va = P.event;
function gd() {
}
function yd() {
  return this.cancelBubble;
}
function bd() {
  return this.defaultPrevented;
}
P.event = function(e) {
  return va && (e = va(e)), e.persist = gd, e.isPropagationStopped = yd, e.isDefaultPrevented = bd, e.nativeEvent = e;
};
var ma = { configurable: !0, get: function() {
  return this.class;
} }, ga = P.vnode;
P.vnode = function(e) {
  var t = e.type, n = e.props, r = n;
  if (typeof t == "string") {
    var o = t.indexOf("-") === -1;
    for (var a in r = {}, n) {
      var i = n[a];
      vd && a === "children" && t === "noscript" || a === "value" && "defaultValue" in n && i == null || (a === "defaultValue" && "value" in n && n.value == null ? a = "value" : a === "download" && i === !0 ? i = "" : /ondoubleclick/i.test(a) ? a = "ondblclick" : /^onchange(textarea|input)/i.test(a + t) && !md(n.type) ? a = "oninput" : /^onfocus$/i.test(a) ? a = "onfocusin" : /^onblur$/i.test(a) ? a = "onfocusout" : /^on(Ani|Tra|Tou|BeforeInp|Compo)/.test(a) ? a = a.toLowerCase() : o && hd.test(a) ? a = a.replace(/[A-Z0-9]/g, "-$&").toLowerCase() : i === null && (i = void 0), /^oninput$/i.test(a) && (a = a.toLowerCase(), r[a] && (a = "oninputCapture")), r[a] = i);
    }
    t == "select" && r.multiple && Array.isArray(r.value) && (r.value = Wn(n.children).forEach(function(s) {
      s.props.selected = r.value.indexOf(s.props.value) != -1;
    })), t == "select" && r.defaultValue != null && (r.value = Wn(n.children).forEach(function(s) {
      s.props.selected = r.multiple ? r.defaultValue.indexOf(s.props.value) != -1 : r.defaultValue == s.props.value;
    })), e.props = r, n.class != n.className && (ma.enumerable = "className" in n, n.className != null && (r.class = n.className), Object.defineProperty(r, "className", ma));
  }
  e.$$typeof = pd, ga && ga(e);
};
var ya = P.__r;
P.__r = function(e) {
  ya && ya(e), e.__c;
};
var ba = typeof globalThis < "u" ? globalThis : window;
ba.FullCalendarVDom ? console.warn("FullCalendar VDOM already loaded") : ba.FullCalendarVDom = {
  Component: Me,
  createElement: rt,
  render: rn,
  createRef: nd,
  Fragment: ht,
  createContext: Cd,
  createPortal: fd,
  flushSync: Ed,
  unmountComponentAtNode: Dd
};
function Ed(e) {
  e();
  var t = P.debounceRendering, n = [];
  function r(o) {
    n.push(o);
  }
  for (P.debounceRendering = r, rn(rt(_d, {}), document.createElement("div")); n.length; )
    n.shift()();
  P.debounceRendering = t;
}
var _d = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    return rt("div", {});
  }, t.prototype.componentDidMount = function() {
    this.setState({});
  }, t;
}(Me);
function Cd(e) {
  var t = id(e), n = t.Provider;
  return t.Provider = function() {
    var r = this, o = !this.getChildContext, a = n.apply(this, arguments);
    if (o) {
      var i = [];
      this.shouldComponentUpdate = function(s) {
        r.props.value !== s.value && i.forEach(function(l) {
          l.context = s.value, l.forceUpdate();
        });
      }, this.sub = function(s) {
        i.push(s);
        var l = s.componentWillUnmount;
        s.componentWillUnmount = function() {
          i.splice(i.indexOf(s), 1), l && l.call(s);
        };
      };
    }
    return a;
  }, t;
}
function Dd(e) {
  rn(null, e);
}
if (typeof FullCalendarVDom > "u")
  throw new Error("Please import the top-level fullcalendar lib before attempting to import a plugin.");
var go = FullCalendarVDom.Component, m = FullCalendarVDom.createElement, Sd = FullCalendarVDom.render, Re = FullCalendarVDom.createRef, me = FullCalendarVDom.Fragment, Wi = FullCalendarVDom.createContext, wd = FullCalendarVDom.createPortal, Ea = FullCalendarVDom.flushSync, Td = FullCalendarVDom.unmountComponentAtNode;
/*!
FullCalendar v5.11.2
Docs & License: https://fullcalendar.io/
(c) 2022 Adam Shaw
*/
var wt = function() {
  function e(t, n) {
    this.context = t, this.internalEventSource = n;
  }
  return e.prototype.remove = function() {
    this.context.dispatch({
      type: "REMOVE_EVENT_SOURCE",
      sourceId: this.internalEventSource.sourceId
    });
  }, e.prototype.refetch = function() {
    this.context.dispatch({
      type: "FETCH_EVENT_SOURCES",
      sourceIds: [this.internalEventSource.sourceId],
      isRefetch: !0
    });
  }, Object.defineProperty(e.prototype, "id", {
    get: function() {
      return this.internalEventSource.publicId;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "url", {
    get: function() {
      return this.internalEventSource.meta.url;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "format", {
    get: function() {
      return this.internalEventSource.meta.format;
    },
    enumerable: !1,
    configurable: !0
  }), e;
}();
function Rd(e) {
  e.parentNode && e.parentNode.removeChild(e);
}
function Le(e, t) {
  if (e.closest)
    return e.closest(t);
  if (!document.documentElement.contains(e))
    return null;
  do {
    if (kd(e, t))
      return e;
    e = e.parentElement || e.parentNode;
  } while (e !== null && e.nodeType === 1);
  return null;
}
function kd(e, t) {
  var n = e.matches || e.matchesSelector || e.msMatchesSelector;
  return n.call(e, t);
}
function Id(e, t) {
  for (var n = e instanceof HTMLElement ? [e] : e, r = [], o = 0; o < n.length; o += 1)
    for (var a = n[o].querySelectorAll(t), i = 0; i < a.length; i += 1)
      r.push(a[i]);
  return r;
}
var xd = /(top|left|right|bottom|width|height)$/i;
function Md(e, t) {
  for (var n in t)
    $i(e, n, t[n]);
}
function $i(e, t, n) {
  n == null ? e.style[t] = "" : typeof n == "number" && xd.test(t) ? e.style[t] = n + "px" : e.style[t] = n;
}
function Pd(e) {
  var t, n;
  return (n = (t = e.composedPath) === null || t === void 0 ? void 0 : t.call(e)[0]) !== null && n !== void 0 ? n : e.target;
}
var _a = 0;
function tt() {
  return _a += 1, "fc-dom-" + _a;
}
function Od(e, t) {
  return function(n) {
    var r = Le(n.target, e);
    r && t.call(r, n, r);
  };
}
function zi(e, t, n, r) {
  var o = Od(n, r);
  return e.addEventListener(t, o), function() {
    e.removeEventListener(t, o);
  };
}
function Ad(e, t, n, r) {
  var o;
  return zi(e, "mouseover", t, function(a, i) {
    if (i !== o) {
      o = i, n(a, i);
      var s = function(l) {
        o = null, r(l, i), i.removeEventListener("mouseleave", s);
      };
      i.addEventListener("mouseleave", s);
    }
  });
}
function Gi(e) {
  return v({ onClick: e }, ji(e));
}
function ji(e) {
  return {
    tabIndex: 0,
    onKeyDown: function(t) {
      (t.key === "Enter" || t.key === " ") && (e(t), t.preventDefault());
    }
  };
}
var Ca = 0;
function Lt() {
  return Ca += 1, String(Ca);
}
function Nd(e) {
  var t = [], n = [], r, o;
  for (typeof e == "string" ? n = e.split(/\s*,\s*/) : typeof e == "function" ? n = [e] : Array.isArray(e) && (n = e), r = 0; r < n.length; r += 1)
    o = n[r], typeof o == "string" ? t.push(o.charAt(0) === "-" ? { field: o.substring(1), order: -1 } : { field: o, order: 1 }) : typeof o == "function" && t.push({ func: o });
  return t;
}
function Hd(e, t, n) {
  var r, o;
  for (r = 0; r < n.length; r += 1)
    if (o = Fd(e, t, n[r]), o)
      return o;
  return 0;
}
function Fd(e, t, n) {
  return n.func ? n.func(e, t) : Vd(e[n.field], t[n.field]) * (n.order || 1);
}
function Vd(e, t) {
  return !e && !t ? 0 : t == null ? -1 : e == null ? 1 : typeof e == "string" || typeof t == "string" ? String(e).localeCompare(String(t)) : e - t;
}
function xr(e, t) {
  var n = String(e);
  return "000".substr(0, t - n.length) + n;
}
function Qt(e, t, n) {
  return typeof e == "function" ? e.apply(void 0, t) : typeof e == "string" ? t.reduce(function(r, o, a) {
    return r.replace("$" + a, o || "");
  }, e) : n;
}
function Mr(e) {
  return e % 1 === 0;
}
function Ud(e) {
  var t = e.querySelector(".fc-scrollgrid-shrink-frame"), n = e.querySelector(".fc-scrollgrid-shrink-cushion");
  if (!t)
    throw new Error("needs fc-scrollgrid-shrink-frame className");
  if (!n)
    throw new Error("needs fc-scrollgrid-shrink-cushion className");
  return e.getBoundingClientRect().width - t.getBoundingClientRect().width + n.getBoundingClientRect().width;
}
var Ld = ["sun", "mon", "tue", "wed", "thu", "fri", "sat"];
function Da(e, t) {
  var n = Qe(e);
  return n[2] += t * 7, _e(n);
}
function ue(e, t) {
  var n = Qe(e);
  return n[2] += t, _e(n);
}
function vt(e, t) {
  var n = Qe(e);
  return n[6] += t, _e(n);
}
function Bd(e, t) {
  return gt(e, t) / 7;
}
function gt(e, t) {
  return (t.valueOf() - e.valueOf()) / (1e3 * 60 * 60 * 24);
}
function Wd(e, t) {
  return (t.valueOf() - e.valueOf()) / (1e3 * 60 * 60);
}
function $d(e, t) {
  return (t.valueOf() - e.valueOf()) / (1e3 * 60);
}
function zd(e, t) {
  return (t.valueOf() - e.valueOf()) / 1e3;
}
function Gd(e, t) {
  var n = Q(e), r = Q(t);
  return {
    years: 0,
    months: 0,
    days: Math.round(gt(n, r)),
    milliseconds: t.valueOf() - r.valueOf() - (e.valueOf() - n.valueOf())
  };
}
function jd(e, t) {
  var n = zn(e, t);
  return n !== null && n % 7 === 0 ? n / 7 : null;
}
function zn(e, t) {
  return et(e) === et(t) ? Math.round(gt(e, t)) : null;
}
function Q(e) {
  return _e([
    e.getUTCFullYear(),
    e.getUTCMonth(),
    e.getUTCDate()
  ]);
}
function Yd(e) {
  return _e([
    e.getUTCFullYear(),
    e.getUTCMonth(),
    e.getUTCDate(),
    e.getUTCHours()
  ]);
}
function qd(e) {
  return _e([
    e.getUTCFullYear(),
    e.getUTCMonth(),
    e.getUTCDate(),
    e.getUTCHours(),
    e.getUTCMinutes()
  ]);
}
function Zd(e) {
  return _e([
    e.getUTCFullYear(),
    e.getUTCMonth(),
    e.getUTCDate(),
    e.getUTCHours(),
    e.getUTCMinutes(),
    e.getUTCSeconds()
  ]);
}
function Xd(e, t, n) {
  var r = e.getUTCFullYear(), o = Pr(e, r, t, n);
  if (o < 1)
    return Pr(e, r - 1, t, n);
  var a = Pr(e, r + 1, t, n);
  return a >= 1 ? Math.min(o, a) : o;
}
function Pr(e, t, n, r) {
  var o = _e([t, 0, 1 + Kd(t, n, r)]), a = Q(e), i = Math.round(gt(o, a));
  return Math.floor(i / 7) + 1;
}
function Kd(e, t, n) {
  var r = 7 + t - n, o = (7 + _e([e, 0, r]).getUTCDay() - t) % 7;
  return -o + r - 1;
}
function Sa(e) {
  return [
    e.getFullYear(),
    e.getMonth(),
    e.getDate(),
    e.getHours(),
    e.getMinutes(),
    e.getSeconds(),
    e.getMilliseconds()
  ];
}
function wa(e) {
  return new Date(
    e[0],
    e[1] || 0,
    e[2] == null ? 1 : e[2],
    e[3] || 0,
    e[4] || 0,
    e[5] || 0
  );
}
function Qe(e) {
  return [
    e.getUTCFullYear(),
    e.getUTCMonth(),
    e.getUTCDate(),
    e.getUTCHours(),
    e.getUTCMinutes(),
    e.getUTCSeconds(),
    e.getUTCMilliseconds()
  ];
}
function _e(e) {
  return e.length === 1 && (e = e.concat([0])), new Date(Date.UTC.apply(Date, e));
}
function Yi(e) {
  return !isNaN(e.valueOf());
}
function et(e) {
  return e.getUTCHours() * 1e3 * 60 * 60 + e.getUTCMinutes() * 1e3 * 60 + e.getUTCSeconds() * 1e3 + e.getUTCMilliseconds();
}
function yo(e, t, n, r) {
  return {
    instanceId: Lt(),
    defId: e,
    range: t,
    forcedStartTzo: n == null ? null : n,
    forcedEndTzo: r == null ? null : r
  };
}
var Gn = Object.prototype.hasOwnProperty;
function bo(e, t) {
  var n = {};
  if (t)
    for (var r in t) {
      for (var o = [], a = e.length - 1; a >= 0; a -= 1) {
        var i = e[a][r];
        if (typeof i == "object" && i)
          o.unshift(i);
        else if (i !== void 0) {
          n[r] = i;
          break;
        }
      }
      o.length && (n[r] = bo(o));
    }
  for (var a = e.length - 1; a >= 0; a -= 1) {
    var s = e[a];
    for (var l in s)
      l in n || (n[l] = s[l]);
  }
  return n;
}
function Ft(e, t) {
  var n = {};
  for (var r in e)
    t(e[r], r) && (n[r] = e[r]);
  return n;
}
function cn(e, t) {
  var n = {};
  for (var r in e)
    n[r] = t(e[r], r);
  return n;
}
function qi(e) {
  for (var t = {}, n = 0, r = e; n < r.length; n++) {
    var o = r[n];
    t[o] = !0;
  }
  return t;
}
function Eo(e) {
  var t = [];
  for (var n in e)
    t.push(e[n]);
  return t;
}
function ze(e, t) {
  if (e === t)
    return !0;
  for (var n in e)
    if (Gn.call(e, n) && !(n in t))
      return !1;
  for (var n in t)
    if (Gn.call(t, n) && e[n] !== t[n])
      return !1;
  return !0;
}
function Ta(e, t) {
  var n = [];
  for (var r in e)
    Gn.call(e, r) && (r in t || n.push(r));
  for (var r in t)
    Gn.call(t, r) && e[r] !== t[r] && n.push(r);
  return n;
}
function Or(e, t, n) {
  if (n === void 0 && (n = {}), e === t)
    return !0;
  for (var r in t)
    if (!(r in e && Jd(e[r], t[r], n[r])))
      return !1;
  for (var r in e)
    if (!(r in t))
      return !1;
  return !0;
}
function Jd(e, t, n) {
  return e === t || n === !0 ? !0 : n ? n(e, t) : !1;
}
function Qd(e, t, n, r) {
  t === void 0 && (t = 0), r === void 0 && (r = 1);
  var o = [];
  n == null && (n = Object.keys(e).length);
  for (var a = t; a < n; a += r) {
    var i = e[a];
    i !== void 0 && o.push(i);
  }
  return o;
}
function ef(e, t, n, r) {
  for (var o = 0; o < r.length; o += 1) {
    var a = r[o].parse(e, n);
    if (a) {
      var i = e.allDay;
      return i == null && (i = t, i == null && (i = a.allDayGuess, i == null && (i = !1))), {
        allDay: i,
        duration: a.duration,
        typeData: a.typeData,
        typeId: o
      };
    }
  }
  return null;
}
function ur(e, t, n) {
  var r = n.dateEnv, o = n.pluginHooks, a = n.options, i = e.defs, s = e.instances;
  s = Ft(s, function(E) {
    return !i[E.defId].recurringDef;
  });
  for (var l in i) {
    var u = i[l];
    if (u.recurringDef) {
      var c = u.recurringDef.duration;
      c || (c = u.allDay ? a.defaultAllDayEventDuration : a.defaultTimedEventDuration);
      for (var d = tf(u, c, t, r, o.recurringTypes), f = 0, p = d; f < p.length; f++) {
        var h = p[f], b = yo(l, {
          start: h,
          end: r.add(h, c)
        });
        s[b.instanceId] = b;
      }
    }
  }
  return { defs: i, instances: s };
}
function tf(e, t, n, r, o) {
  var a = o[e.recurringDef.typeId], i = a.expand(e.recurringDef.typeData, {
    start: r.subtract(n.start, t),
    end: n.end
  }, r);
  return e.allDay && (i = i.map(Q)), i;
}
var nf = /^(-?)(?:(\d+)\.)?(\d+):(\d\d)(?::(\d\d)(?:\.(\d\d\d))?)?/;
function Y(e, t) {
  var n;
  return typeof e == "string" ? rf(e) : typeof e == "object" && e ? Ra(e) : typeof e == "number" ? Ra((n = {}, n[t || "milliseconds"] = e, n)) : null;
}
function rf(e) {
  var t = nf.exec(e);
  if (t) {
    var n = t[1] ? -1 : 1;
    return {
      years: 0,
      months: 0,
      days: n * (t[2] ? parseInt(t[2], 10) : 0),
      milliseconds: n * ((t[3] ? parseInt(t[3], 10) : 0) * 60 * 60 * 1e3 + (t[4] ? parseInt(t[4], 10) : 0) * 60 * 1e3 + (t[5] ? parseInt(t[5], 10) : 0) * 1e3 + (t[6] ? parseInt(t[6], 10) : 0))
    };
  }
  return null;
}
function Ra(e) {
  var t = {
    years: e.years || e.year || 0,
    months: e.months || e.month || 0,
    days: e.days || e.day || 0,
    milliseconds: (e.hours || e.hour || 0) * 60 * 60 * 1e3 + (e.minutes || e.minute || 0) * 60 * 1e3 + (e.seconds || e.second || 0) * 1e3 + (e.milliseconds || e.millisecond || e.ms || 0)
  }, n = e.weeks || e.week;
  return n && (t.days += n * 7, t.specifiedWeeks = !0), t;
}
function of(e, t) {
  return e.years === t.years && e.months === t.months && e.days === t.days && e.milliseconds === t.milliseconds;
}
function af(e, t) {
  return {
    years: e.years - t.years,
    months: e.months - t.months,
    days: e.days - t.days,
    milliseconds: e.milliseconds - t.milliseconds
  };
}
function sf(e) {
  return It(e) / 365;
}
function lf(e) {
  return It(e) / 30;
}
function It(e) {
  return on(e) / 864e5;
}
function on(e) {
  return e.years * (365 * 864e5) + e.months * (30 * 864e5) + e.days * 864e5 + e.milliseconds;
}
function Qr(e) {
  var t = e.milliseconds;
  if (t) {
    if (t % 1e3 !== 0)
      return { unit: "millisecond", value: t };
    if (t % (1e3 * 60) !== 0)
      return { unit: "second", value: t / 1e3 };
    if (t % (1e3 * 60 * 60) !== 0)
      return { unit: "minute", value: t / (1e3 * 60) };
    if (t)
      return { unit: "hour", value: t / (1e3 * 60 * 60) };
  }
  return e.days ? e.specifiedWeeks && e.days % 7 === 0 ? { unit: "week", value: e.days / 7 } : { unit: "day", value: e.days } : e.months ? { unit: "month", value: e.months } : e.years ? { unit: "year", value: e.years } : { unit: "millisecond", value: 0 };
}
function uf(e, t, n) {
  n === void 0 && (n = !1);
  var r = e.toISOString();
  return r = r.replace(".000", ""), n && (r = r.replace("T00:00:00Z", "")), r.length > 10 && (t == null ? r = r.replace("Z", "") : t !== 0 && (r = r.replace("Z", _o(t, !0)))), r;
}
function cr(e) {
  return e.toISOString().replace(/T.*$/, "");
}
function _o(e, t) {
  t === void 0 && (t = !1);
  var n = e < 0 ? "-" : "+", r = Math.abs(e), o = Math.floor(r / 60), a = Math.round(r % 60);
  return t ? n + xr(o, 2) + ":" + xr(a, 2) : "GMT" + n + o + (a ? ":" + xr(a, 2) : "");
}
function Vt(e, t, n) {
  if (e === t)
    return !0;
  var r = e.length, o;
  if (r !== t.length)
    return !1;
  for (o = 0; o < r; o += 1)
    if (!(n ? n(e[o], t[o]) : e[o] === t[o]))
      return !1;
  return !0;
}
function W(e, t, n) {
  var r, o;
  return function() {
    for (var a = [], i = 0; i < arguments.length; i++)
      a[i] = arguments[i];
    if (!r)
      o = e.apply(this, a);
    else if (!Vt(r, a)) {
      n && n(o);
      var s = e.apply(this, a);
      (!t || !t(s, o)) && (o = s);
    }
    return r = a, o;
  };
}
function Pn(e, t, n) {
  var r = this, o, a;
  return function(i) {
    if (!o)
      a = e.call(r, i);
    else if (!ze(o, i)) {
      n && n(a);
      var s = e.call(r, i);
      (!t || !t(s, a)) && (a = s);
    }
    return o = i, a;
  };
}
var ka = {
  week: 3,
  separator: 0,
  omitZeroMinute: 0,
  meridiem: 0,
  omitCommas: 0
}, jn = {
  timeZoneName: 7,
  era: 6,
  year: 5,
  month: 4,
  day: 2,
  weekday: 2,
  hour: 1,
  minute: 1,
  second: 1
}, bn = /\s*([ap])\.?m\.?/i, cf = /,/g, df = /\s+/g, ff = /\u200e/g, pf = /UTC|GMT/, hf = function() {
  function e(t) {
    var n = {}, r = {}, o = 0;
    for (var a in t)
      a in ka ? (r[a] = t[a], o = Math.max(ka[a], o)) : (n[a] = t[a], a in jn && (o = Math.max(jn[a], o)));
    this.standardDateProps = n, this.extendedSettings = r, this.severity = o, this.buildFormattingFunc = W(Ia);
  }
  return e.prototype.format = function(t, n) {
    return this.buildFormattingFunc(this.standardDateProps, this.extendedSettings, n)(t);
  }, e.prototype.formatRange = function(t, n, r, o) {
    var a = this, i = a.standardDateProps, s = a.extendedSettings, l = Ef(t.marker, n.marker, r.calendarSystem);
    if (!l)
      return this.format(t, r);
    var u = l;
    u > 1 && (i.year === "numeric" || i.year === "2-digit") && (i.month === "numeric" || i.month === "2-digit") && (i.day === "numeric" || i.day === "2-digit") && (u = 1);
    var c = this.format(t, r), d = this.format(n, r);
    if (c === d)
      return c;
    var f = _f(i, u), p = Ia(f, s, r), h = p(t), b = p(n), E = Cf(c, h, d, b), y = s.separator || o || r.defaultSeparator || "";
    return E ? E.before + h + y + b + E.after : c + y + d;
  }, e.prototype.getLargestUnit = function() {
    switch (this.severity) {
      case 7:
      case 6:
      case 5:
        return "year";
      case 4:
        return "month";
      case 3:
        return "week";
      case 2:
        return "day";
      default:
        return "time";
    }
  }, e;
}();
function Ia(e, t, n) {
  var r = Object.keys(e).length;
  return r === 1 && e.timeZoneName === "short" ? function(o) {
    return _o(o.timeZoneOffset);
  } : r === 0 && t.week ? function(o) {
    return bf(n.computeWeekNumber(o.marker), n.weekText, n.weekTextLong, n.locale, t.week);
  } : vf(e, t, n);
}
function vf(e, t, n) {
  e = v({}, e), t = v({}, t), mf(e, t), e.timeZone = "UTC";
  var r = new Intl.DateTimeFormat(n.locale.codes, e), o;
  if (t.omitZeroMinute) {
    var a = v({}, e);
    delete a.minute, o = new Intl.DateTimeFormat(n.locale.codes, a);
  }
  return function(i) {
    var s = i.marker, l;
    o && !s.getUTCMinutes() ? l = o : l = r;
    var u = l.format(s);
    return gf(u, i, e, t, n);
  };
}
function mf(e, t) {
  e.timeZoneName && (e.hour || (e.hour = "2-digit"), e.minute || (e.minute = "2-digit")), e.timeZoneName === "long" && (e.timeZoneName = "short"), t.omitZeroMinute && (e.second || e.millisecond) && delete t.omitZeroMinute;
}
function gf(e, t, n, r, o) {
  return e = e.replace(ff, ""), n.timeZoneName === "short" && (e = yf(e, o.timeZone === "UTC" || t.timeZoneOffset == null ? "UTC" : _o(t.timeZoneOffset))), r.omitCommas && (e = e.replace(cf, "").trim()), r.omitZeroMinute && (e = e.replace(":00", "")), r.meridiem === !1 ? e = e.replace(bn, "").trim() : r.meridiem === "narrow" ? e = e.replace(bn, function(a, i) {
    return i.toLocaleLowerCase();
  }) : r.meridiem === "short" ? e = e.replace(bn, function(a, i) {
    return i.toLocaleLowerCase() + "m";
  }) : r.meridiem === "lowercase" && (e = e.replace(bn, function(a) {
    return a.toLocaleLowerCase();
  })), e = e.replace(df, " "), e = e.trim(), e;
}
function yf(e, t) {
  var n = !1;
  return e = e.replace(pf, function() {
    return n = !0, t;
  }), n || (e += " " + t), e;
}
function bf(e, t, n, r, o) {
  var a = [];
  return o === "long" ? a.push(n) : (o === "short" || o === "narrow") && a.push(t), (o === "long" || o === "short") && a.push(" "), a.push(r.simpleNumberFormat.format(e)), r.options.direction === "rtl" && a.reverse(), a.join("");
}
function Ef(e, t, n) {
  return n.getMarkerYear(e) !== n.getMarkerYear(t) ? 5 : n.getMarkerMonth(e) !== n.getMarkerMonth(t) ? 4 : n.getMarkerDay(e) !== n.getMarkerDay(t) ? 2 : et(e) !== et(t) ? 1 : 0;
}
function _f(e, t) {
  var n = {};
  for (var r in e)
    (!(r in jn) || jn[r] <= t) && (n[r] = e[r]);
  return n;
}
function Cf(e, t, n, r) {
  for (var o = 0; o < e.length; ) {
    var a = e.indexOf(t, o);
    if (a === -1)
      break;
    var i = e.substr(0, a);
    o = a + t.length;
    for (var s = e.substr(o), l = 0; l < n.length; ) {
      var u = n.indexOf(r, l);
      if (u === -1)
        break;
      var c = n.substr(0, u);
      l = u + r.length;
      var d = n.substr(l);
      if (i === c && s === d)
        return {
          before: i,
          after: s
        };
    }
  }
  return null;
}
function xa(e, t) {
  var n = t.markerToArray(e.marker);
  return {
    marker: e.marker,
    timeZoneOffset: e.timeZoneOffset,
    array: n,
    year: n[0],
    month: n[1],
    day: n[2],
    hour: n[3],
    minute: n[4],
    second: n[5],
    millisecond: n[6]
  };
}
function Yn(e, t, n, r) {
  var o = xa(e, n.calendarSystem), a = t ? xa(t, n.calendarSystem) : null;
  return {
    date: o,
    start: o,
    end: a,
    timeZone: n.timeZone,
    localeCodes: n.locale.codes,
    defaultSeparator: r || n.defaultSeparator
  };
}
var Df = function() {
  function e(t) {
    this.cmdStr = t;
  }
  return e.prototype.format = function(t, n, r) {
    return n.cmdFormatter(this.cmdStr, Yn(t, null, n, r));
  }, e.prototype.formatRange = function(t, n, r, o) {
    return r.cmdFormatter(this.cmdStr, Yn(t, n, r, o));
  }, e;
}(), Sf = function() {
  function e(t) {
    this.func = t;
  }
  return e.prototype.format = function(t, n, r) {
    return this.func(Yn(t, null, n, r));
  }, e.prototype.formatRange = function(t, n, r, o) {
    return this.func(Yn(t, n, r, o));
  }, e;
}();
function le(e) {
  return typeof e == "object" && e ? new hf(e) : typeof e == "string" ? new Df(e) : typeof e == "function" ? new Sf(e) : null;
}
var Ma = {
  navLinkDayClick: g,
  navLinkWeekClick: g,
  duration: Y,
  bootstrapFontAwesome: g,
  buttonIcons: g,
  customButtons: g,
  defaultAllDayEventDuration: Y,
  defaultTimedEventDuration: Y,
  nextDayThreshold: Y,
  scrollTime: Y,
  scrollTimeReset: Boolean,
  slotMinTime: Y,
  slotMaxTime: Y,
  dayPopoverFormat: le,
  slotDuration: Y,
  snapDuration: Y,
  headerToolbar: g,
  footerToolbar: g,
  defaultRangeSeparator: String,
  titleRangeSeparator: String,
  forceEventDuration: Boolean,
  dayHeaders: Boolean,
  dayHeaderFormat: le,
  dayHeaderClassNames: g,
  dayHeaderContent: g,
  dayHeaderDidMount: g,
  dayHeaderWillUnmount: g,
  dayCellClassNames: g,
  dayCellContent: g,
  dayCellDidMount: g,
  dayCellWillUnmount: g,
  initialView: String,
  aspectRatio: Number,
  weekends: Boolean,
  weekNumberCalculation: g,
  weekNumbers: Boolean,
  weekNumberClassNames: g,
  weekNumberContent: g,
  weekNumberDidMount: g,
  weekNumberWillUnmount: g,
  editable: Boolean,
  viewClassNames: g,
  viewDidMount: g,
  viewWillUnmount: g,
  nowIndicator: Boolean,
  nowIndicatorClassNames: g,
  nowIndicatorContent: g,
  nowIndicatorDidMount: g,
  nowIndicatorWillUnmount: g,
  showNonCurrentDates: Boolean,
  lazyFetching: Boolean,
  startParam: String,
  endParam: String,
  timeZoneParam: String,
  timeZone: String,
  locales: g,
  locale: g,
  themeSystem: String,
  dragRevertDuration: Number,
  dragScroll: Boolean,
  allDayMaintainDuration: Boolean,
  unselectAuto: Boolean,
  dropAccept: g,
  eventOrder: Nd,
  eventOrderStrict: Boolean,
  handleWindowResize: Boolean,
  windowResizeDelay: Number,
  longPressDelay: Number,
  eventDragMinDistance: Number,
  expandRows: Boolean,
  height: g,
  contentHeight: g,
  direction: String,
  weekNumberFormat: le,
  eventResizableFromStart: Boolean,
  displayEventTime: Boolean,
  displayEventEnd: Boolean,
  weekText: String,
  weekTextLong: String,
  progressiveEventRendering: Boolean,
  businessHours: g,
  initialDate: g,
  now: g,
  eventDataTransform: g,
  stickyHeaderDates: g,
  stickyFooterScrollbar: g,
  viewHeight: g,
  defaultAllDay: Boolean,
  eventSourceFailure: g,
  eventSourceSuccess: g,
  eventDisplay: String,
  eventStartEditable: Boolean,
  eventDurationEditable: Boolean,
  eventOverlap: g,
  eventConstraint: g,
  eventAllow: g,
  eventBackgroundColor: String,
  eventBorderColor: String,
  eventTextColor: String,
  eventColor: String,
  eventClassNames: g,
  eventContent: g,
  eventDidMount: g,
  eventWillUnmount: g,
  selectConstraint: g,
  selectOverlap: g,
  selectAllow: g,
  droppable: Boolean,
  unselectCancel: String,
  slotLabelFormat: g,
  slotLaneClassNames: g,
  slotLaneContent: g,
  slotLaneDidMount: g,
  slotLaneWillUnmount: g,
  slotLabelClassNames: g,
  slotLabelContent: g,
  slotLabelDidMount: g,
  slotLabelWillUnmount: g,
  dayMaxEvents: g,
  dayMaxEventRows: g,
  dayMinWidth: Number,
  slotLabelInterval: Y,
  allDayText: String,
  allDayClassNames: g,
  allDayContent: g,
  allDayDidMount: g,
  allDayWillUnmount: g,
  slotMinWidth: Number,
  navLinks: Boolean,
  eventTimeFormat: le,
  rerenderDelay: Number,
  moreLinkText: g,
  moreLinkHint: g,
  selectMinDistance: Number,
  selectable: Boolean,
  selectLongPressDelay: Number,
  eventLongPressDelay: Number,
  selectMirror: Boolean,
  eventMaxStack: Number,
  eventMinHeight: Number,
  eventMinWidth: Number,
  eventShortHeight: Number,
  slotEventOverlap: Boolean,
  plugins: g,
  firstDay: Number,
  dayCount: Number,
  dateAlignment: String,
  dateIncrement: Y,
  hiddenDays: g,
  monthMode: Boolean,
  fixedWeekCount: Boolean,
  validRange: g,
  visibleRange: g,
  titleFormat: g,
  eventInteractive: Boolean,
  noEventsText: String,
  viewHint: g,
  navLinkHint: g,
  closeHint: String,
  timeHint: String,
  eventHint: String,
  moreLinkClick: g,
  moreLinkClassNames: g,
  moreLinkContent: g,
  moreLinkDidMount: g,
  moreLinkWillUnmount: g
}, en = {
  eventDisplay: "auto",
  defaultRangeSeparator: " - ",
  titleRangeSeparator: " \u2013 ",
  defaultTimedEventDuration: "01:00:00",
  defaultAllDayEventDuration: { day: 1 },
  forceEventDuration: !1,
  nextDayThreshold: "00:00:00",
  dayHeaders: !0,
  initialView: "",
  aspectRatio: 1.35,
  headerToolbar: {
    start: "title",
    center: "",
    end: "today prev,next"
  },
  weekends: !0,
  weekNumbers: !1,
  weekNumberCalculation: "local",
  editable: !1,
  nowIndicator: !1,
  scrollTime: "06:00:00",
  scrollTimeReset: !0,
  slotMinTime: "00:00:00",
  slotMaxTime: "24:00:00",
  showNonCurrentDates: !0,
  lazyFetching: !0,
  startParam: "start",
  endParam: "end",
  timeZoneParam: "timeZone",
  timeZone: "local",
  locales: [],
  locale: "",
  themeSystem: "standard",
  dragRevertDuration: 500,
  dragScroll: !0,
  allDayMaintainDuration: !1,
  unselectAuto: !0,
  dropAccept: "*",
  eventOrder: "start,-duration,allDay,title",
  dayPopoverFormat: { month: "long", day: "numeric", year: "numeric" },
  handleWindowResize: !0,
  windowResizeDelay: 100,
  longPressDelay: 1e3,
  eventDragMinDistance: 5,
  expandRows: !1,
  navLinks: !1,
  selectable: !1,
  eventMinHeight: 15,
  eventMinWidth: 30,
  eventShortHeight: 30
}, Pa = {
  datesSet: g,
  eventsSet: g,
  eventAdd: g,
  eventChange: g,
  eventRemove: g,
  windowResize: g,
  eventClick: g,
  eventMouseEnter: g,
  eventMouseLeave: g,
  select: g,
  unselect: g,
  loading: g,
  _unmount: g,
  _beforeprint: g,
  _afterprint: g,
  _noEventDrop: g,
  _noEventResize: g,
  _resize: g,
  _scrollRequest: g
}, Oa = {
  buttonText: g,
  buttonHints: g,
  views: g,
  plugins: g,
  initialEvents: g,
  events: g,
  eventSources: g
}, ct = {
  headerToolbar: Dt,
  footerToolbar: Dt,
  buttonText: Dt,
  buttonHints: Dt,
  buttonIcons: Dt,
  dateIncrement: Dt
};
function Dt(e, t) {
  return typeof e == "object" && typeof t == "object" && e && t ? ze(e, t) : e === t;
}
var wf = {
  type: String,
  component: g,
  buttonText: String,
  buttonTextKey: String,
  dateProfileGeneratorClass: g,
  usesMinMaxTime: Boolean,
  classNames: g,
  content: g,
  didMount: g,
  willUnmount: g
};
function Ar(e) {
  return bo(e, ct);
}
function Co(e, t) {
  var n = {}, r = {};
  for (var o in t)
    o in e && (n[o] = t[o](e[o]));
  for (var o in e)
    o in t || (r[o] = e[o]);
  return { refined: n, extra: r };
}
function g(e) {
  return e;
}
function qn(e, t, n, r) {
  for (var o = mt(), a = wo(n), i = 0, s = e; i < s.length; i++) {
    var l = s[i], u = Xi(l, t, n, r, a);
    u && eo(u, o);
  }
  return o;
}
function eo(e, t) {
  return t === void 0 && (t = mt()), t.defs[e.def.defId] = e.def, e.instance && (t.instances[e.instance.instanceId] = e.instance), t;
}
function Tf(e, t) {
  var n = e.instances[t];
  if (n) {
    var r = e.defs[n.defId], o = So(e, function(a) {
      return Rf(r, a);
    });
    return o.defs[r.defId] = r, o.instances[n.instanceId] = n, o;
  }
  return mt();
}
function Rf(e, t) {
  return Boolean(e.groupId && e.groupId === t.groupId);
}
function mt() {
  return { defs: {}, instances: {} };
}
function Do(e, t) {
  return {
    defs: v(v({}, e.defs), t.defs),
    instances: v(v({}, e.instances), t.instances)
  };
}
function So(e, t) {
  var n = Ft(e.defs, t), r = Ft(e.instances, function(o) {
    return n[o.defId];
  });
  return { defs: n, instances: r };
}
function kf(e, t) {
  var n = e.defs, r = e.instances, o = {}, a = {};
  for (var i in n)
    t.defs[i] || (o[i] = n[i]);
  for (var s in r)
    !t.instances[s] && o[r[s].defId] && (a[s] = r[s]);
  return {
    defs: o,
    instances: a
  };
}
function If(e, t) {
  return Array.isArray(e) ? qn(e, null, t, !0) : typeof e == "object" && e ? qn([e], null, t, !0) : e != null ? String(e) : null;
}
function to(e) {
  return Array.isArray(e) ? e : typeof e == "string" ? e.split(/\s+/) : [];
}
var Zn = {
  display: String,
  editable: Boolean,
  startEditable: Boolean,
  durationEditable: Boolean,
  constraint: g,
  overlap: g,
  allow: g,
  className: to,
  classNames: to,
  color: String,
  backgroundColor: String,
  borderColor: String,
  textColor: String
}, xf = {
  display: null,
  startEditable: null,
  durationEditable: null,
  constraints: [],
  overlap: null,
  allows: [],
  backgroundColor: "",
  borderColor: "",
  textColor: "",
  classNames: []
};
function Xn(e, t) {
  var n = If(e.constraint, t);
  return {
    display: e.display || null,
    startEditable: e.startEditable != null ? e.startEditable : e.editable,
    durationEditable: e.durationEditable != null ? e.durationEditable : e.editable,
    constraints: n != null ? [n] : [],
    overlap: e.overlap != null ? e.overlap : null,
    allows: e.allow != null ? [e.allow] : [],
    backgroundColor: e.backgroundColor || e.color || "",
    borderColor: e.borderColor || e.color || "",
    textColor: e.textColor || "",
    classNames: (e.className || []).concat(e.classNames || [])
  };
}
function Mf(e) {
  return e.reduce(Pf, xf);
}
function Pf(e, t) {
  return {
    display: t.display != null ? t.display : e.display,
    startEditable: t.startEditable != null ? t.startEditable : e.startEditable,
    durationEditable: t.durationEditable != null ? t.durationEditable : e.durationEditable,
    constraints: e.constraints.concat(t.constraints),
    overlap: typeof t.overlap == "boolean" ? t.overlap : e.overlap,
    allows: e.allows.concat(t.allows),
    backgroundColor: t.backgroundColor || e.backgroundColor,
    borderColor: t.borderColor || e.borderColor,
    textColor: t.textColor || e.textColor,
    classNames: e.classNames.concat(t.classNames)
  };
}
var On = {
  id: String,
  groupId: String,
  title: String,
  url: String,
  interactive: Boolean
}, Zi = {
  start: g,
  end: g,
  date: g,
  allDay: Boolean
}, Of = v(v(v({}, On), Zi), { extendedProps: g });
function Xi(e, t, n, r, o) {
  o === void 0 && (o = wo(n));
  var a = Ki(e, n, o), i = a.refined, s = a.extra, l = Nf(t, n), u = ef(i, l, n.dateEnv, n.pluginHooks.recurringTypes);
  if (u) {
    var c = no(i, s, t ? t.sourceId : "", u.allDay, Boolean(u.duration), n);
    return c.recurringDef = {
      typeId: u.typeId,
      typeData: u.typeData,
      duration: u.duration
    }, { def: c, instance: null };
  }
  var d = Af(i, l, n, r);
  if (d) {
    var c = no(i, s, t ? t.sourceId : "", d.allDay, d.hasEnd, n), f = yo(c.defId, d.range, d.forcedStartTzo, d.forcedEndTzo);
    return { def: c, instance: f };
  }
  return null;
}
function Ki(e, t, n) {
  return n === void 0 && (n = wo(t)), Co(e, n);
}
function wo(e) {
  return v(v(v({}, Zn), Of), e.pluginHooks.eventRefiners);
}
function no(e, t, n, r, o, a) {
  for (var i = {
    title: e.title || "",
    groupId: e.groupId || "",
    publicId: e.id || "",
    url: e.url || "",
    recurringDef: null,
    defId: Lt(),
    sourceId: n,
    allDay: r,
    hasEnd: o,
    interactive: e.interactive,
    ui: Xn(e, a),
    extendedProps: v(v({}, e.extendedProps || {}), t)
  }, s = 0, l = a.pluginHooks.eventDefMemberAdders; s < l.length; s++) {
    var u = l[s];
    v(i, u(e));
  }
  return Object.freeze(i.ui.classNames), Object.freeze(i.extendedProps), i;
}
function Af(e, t, n, r) {
  var o = e.allDay, a, i = null, s = !1, l, u = null, c = e.start != null ? e.start : e.date;
  if (a = n.dateEnv.createMarkerMeta(c), a)
    i = a.marker;
  else if (!r)
    return null;
  return e.end != null && (l = n.dateEnv.createMarkerMeta(e.end)), o == null && (t != null ? o = t : o = (!a || a.isTimeUnspecified) && (!l || l.isTimeUnspecified)), o && i && (i = Q(i)), l && (u = l.marker, o && (u = Q(u)), i && u <= i && (u = null)), u ? s = !0 : r || (s = n.options.forceEventDuration || !1, u = n.dateEnv.add(i, o ? n.options.defaultAllDayEventDuration : n.options.defaultTimedEventDuration)), {
    allDay: o,
    hasEnd: s,
    range: { start: i, end: u },
    forcedStartTzo: a ? a.forcedTzo : null,
    forcedEndTzo: l ? l.forcedTzo : null
  };
}
function Nf(e, t) {
  var n = null;
  return e && (n = e.defaultAllDay), n == null && (n = t.options.defaultAllDay), n;
}
function Ji(e) {
  var t = Math.floor(gt(e.start, e.end)) || 1, n = Q(e.start), r = ue(n, t);
  return { start: n, end: r };
}
function To(e, t) {
  t === void 0 && (t = Y(0));
  var n = null, r = null;
  if (e.end) {
    r = Q(e.end);
    var o = e.end.valueOf() - r.valueOf();
    o && o >= on(t) && (r = ue(r, 1));
  }
  return e.start && (n = Q(e.start), r && r <= n && (r = ue(n, 1))), { start: n, end: r };
}
function Hf(e) {
  var t = To(e);
  return gt(t.start, t.end) > 1;
}
function En(e, t, n, r) {
  return r === "year" ? Y(n.diffWholeYears(e, t), "year") : r === "month" ? Y(n.diffWholeMonths(e, t), "month") : Gd(e, t);
}
function Ff(e, t) {
  var n = null, r = null;
  return e.start && (n = t.createMarker(e.start)), e.end && (r = t.createMarker(e.end)), !n && !r || n && r && r < n ? null : { start: n, end: r };
}
function Aa(e, t) {
  var n = [], r = t.start, o, a;
  for (e.sort(Vf), o = 0; o < e.length; o += 1)
    a = e[o], a.start > r && n.push({ start: r, end: a.start }), a.end > r && (r = a.end);
  return r < t.end && n.push({ start: r, end: t.end }), n;
}
function Vf(e, t) {
  return e.start.valueOf() - t.start.valueOf();
}
function Ut(e, t) {
  var n = e.start, r = e.end, o = null;
  return t.start !== null && (n === null ? n = t.start : n = new Date(Math.max(n.valueOf(), t.start.valueOf()))), t.end != null && (r === null ? r = t.end : r = new Date(Math.min(r.valueOf(), t.end.valueOf()))), (n === null || r === null || n < r) && (o = { start: n, end: r }), o;
}
function Uf(e, t) {
  return (e.end === null || t.start === null || e.end > t.start) && (e.start === null || t.end === null || e.start < t.end);
}
function ft(e, t) {
  return (e.start === null || t >= e.start) && (e.end === null || t < e.end);
}
function Lf(e, t) {
  return t.start != null && e < t.start ? t.start : t.end != null && e >= t.end ? new Date(t.end.valueOf() - 1) : e;
}
function ro(e, t, n, r) {
  var o = {}, a = {}, i = {}, s = [], l = [], u = Qi(e.defs, t);
  for (var c in e.defs) {
    var d = e.defs[c], f = u[d.defId];
    f.display === "inverse-background" && (d.groupId ? (o[d.groupId] = [], i[d.groupId] || (i[d.groupId] = d)) : a[c] = []);
  }
  for (var p in e.instances) {
    var h = e.instances[p], d = e.defs[h.defId], f = u[d.defId], b = h.range, E = !d.allDay && r ? To(b, r) : b, y = Ut(E, n);
    y && (f.display === "inverse-background" ? d.groupId ? o[d.groupId].push(y) : a[h.defId].push(y) : f.display !== "none" && (f.display === "background" ? s : l).push({
      def: d,
      ui: f,
      instance: h,
      range: y,
      isStart: E.start && E.start.valueOf() === y.start.valueOf(),
      isEnd: E.end && E.end.valueOf() === y.end.valueOf()
    }));
  }
  for (var D in o)
    for (var k = o[D], T = Aa(k, n), U = 0, O = T; U < O.length; U++) {
      var R = O[U], d = i[D], f = u[d.defId];
      s.push({
        def: d,
        ui: f,
        instance: null,
        range: R,
        isStart: !1,
        isEnd: !1
      });
    }
  for (var c in a)
    for (var k = a[c], T = Aa(k, n), A = 0, M = T; A < M.length; A++) {
      var R = M[A];
      s.push({
        def: e.defs[c],
        ui: u[c],
        instance: null,
        range: R,
        isStart: !1,
        isEnd: !1
      });
    }
  return { bg: s, fg: l };
}
function Na(e, t) {
  e.fcSeg = t;
}
function oo(e) {
  return e.fcSeg || e.parentNode.fcSeg || null;
}
function Qi(e, t) {
  return cn(e, function(n) {
    return es(n, t);
  });
}
function es(e, t) {
  var n = [];
  return t[""] && n.push(t[""]), t[e.defId] && n.push(t[e.defId]), n.push(e.ui), Mf(n);
}
function ts(e, t) {
  var n = e.map(Bf);
  return n.sort(function(r, o) {
    return Hd(r, o, t);
  }), n.map(function(r) {
    return r._seg;
  });
}
function Bf(e) {
  var t = e.eventRange, n = t.def, r = t.instance ? t.instance.range : t.range, o = r.start ? r.start.valueOf() : 0, a = r.end ? r.end.valueOf() : 0;
  return v(v(v({}, n.extendedProps), n), {
    id: n.publicId,
    start: o,
    end: a,
    duration: a - o,
    allDay: Number(n.allDay),
    _seg: e
  });
}
function Wf(e, t) {
  for (var n = t.pluginHooks, r = n.isDraggableTransformers, o = e.eventRange, a = o.def, i = o.ui, s = i.startEditable, l = 0, u = r; l < u.length; l++) {
    var c = u[l];
    s = c(s, a, i, t);
  }
  return s;
}
function $f(e, t) {
  return e.isStart && e.eventRange.ui.durationEditable && t.options.eventResizableFromStart;
}
function zf(e, t) {
  return e.isEnd && e.eventRange.ui.durationEditable;
}
function tn(e, t, n, r, o, a, i) {
  var s = n.dateEnv, l = n.options, u = l.displayEventTime, c = l.displayEventEnd, d = e.eventRange.def, f = e.eventRange.instance;
  u == null && (u = r !== !1), c == null && (c = o !== !1);
  var p = f.range.start, h = f.range.end, b = a || e.start || e.eventRange.range.start, E = i || e.end || e.eventRange.range.end, y = Q(p).valueOf() === Q(b).valueOf(), D = Q(vt(h, -1)).valueOf() === Q(vt(E, -1)).valueOf();
  return u && !d.allDay && (y || D) ? (b = y ? p : b, E = D ? h : E, c && d.hasEnd ? s.formatRange(b, E, t, {
    forcedStartTzo: a ? null : f.forcedStartTzo,
    forcedEndTzo: i ? null : f.forcedEndTzo
  }) : s.format(b, t, {
    forcedTzo: a ? null : f.forcedStartTzo
  })) : "";
}
function xt(e, t, n) {
  var r = e.eventRange.range;
  return {
    isPast: r.end < (n || t.start),
    isFuture: r.start >= (n || t.end),
    isToday: t && ft(t, r.start)
  };
}
function Gf(e) {
  var t = ["fc-event"];
  return e.isMirror && t.push("fc-event-mirror"), e.isDraggable && t.push("fc-event-draggable"), (e.isStartResizable || e.isEndResizable) && t.push("fc-event-resizable"), e.isDragging && t.push("fc-event-dragging"), e.isResizing && t.push("fc-event-resizing"), e.isSelected && t.push("fc-event-selected"), e.isStart && t.push("fc-event-start"), e.isEnd && t.push("fc-event-end"), e.isPast && t.push("fc-event-past"), e.isToday && t.push("fc-event-today"), e.isFuture && t.push("fc-event-future"), t;
}
function jf(e) {
  return e.instance ? e.instance.instanceId : e.def.defId + ":" + e.range.start.toISOString();
}
function Ro(e, t) {
  var n = e.eventRange, r = n.def, o = n.instance, a = r.url;
  if (a)
    return { href: a };
  var i = t.emitter, s = t.options, l = s.eventInteractive;
  return l == null && (l = r.interactive, l == null && (l = Boolean(i.hasHandlers("eventClick")))), l ? ji(function(u) {
    i.trigger("eventClick", {
      el: u.target,
      event: new Ae(t, r, o),
      jsEvent: u,
      view: t.viewApi
    });
  }) : {};
}
var Yf = {
  start: g,
  end: g,
  allDay: Boolean
};
function qf(e, t, n) {
  var r = Zf(e, t), o = r.range;
  if (!o.start)
    return null;
  if (!o.end) {
    if (n == null)
      return null;
    o.end = t.add(o.start, n);
  }
  return r;
}
function Zf(e, t) {
  var n = Co(e, Yf), r = n.refined, o = n.extra, a = r.start ? t.createMarkerMeta(r.start) : null, i = r.end ? t.createMarkerMeta(r.end) : null, s = r.allDay;
  return s == null && (s = a && a.isTimeUnspecified && (!i || i.isTimeUnspecified)), v({ range: {
    start: a ? a.marker : null,
    end: i ? i.marker : null
  }, allDay: s }, o);
}
function Xf(e, t) {
  return v(v({}, rs(e.range, t, e.allDay)), { allDay: e.allDay });
}
function ns(e, t, n) {
  return v(v({}, rs(e, t, n)), { timeZone: t.timeZone });
}
function rs(e, t, n) {
  return {
    start: t.toDate(e.start),
    end: t.toDate(e.end),
    startStr: t.formatIso(e.start, { omitTime: n }),
    endStr: t.formatIso(e.end, { omitTime: n })
  };
}
function Kf(e, t, n) {
  var r = Ki({ editable: !1 }, n), o = no(
    r.refined,
    r.extra,
    "",
    e.allDay,
    !0,
    n
  );
  return {
    def: o,
    ui: es(o, t),
    instance: yo(o.defId, e.range),
    range: e.range,
    isStart: !0,
    isEnd: !0
  };
}
function Jf(e, t, n) {
  n.emitter.trigger("select", v(v({}, ep(e, n)), { jsEvent: t ? t.origEvent : null, view: n.viewApi || n.calendarApi.view }));
}
function Qf(e, t) {
  t.emitter.trigger("unselect", {
    jsEvent: e ? e.origEvent : null,
    view: t.viewApi || t.calendarApi.view
  });
}
function ep(e, t) {
  for (var n = {}, r = 0, o = t.pluginHooks.dateSpanTransforms; r < o.length; r++) {
    var a = o[r];
    v(n, a(e, t));
  }
  return v(n, Xf(e, t.dateEnv)), n;
}
function Ha(e, t, n) {
  var r = n.dateEnv, o = n.options, a = t;
  return e ? (a = Q(a), a = r.add(a, o.defaultAllDayEventDuration)) : a = r.add(a, o.defaultTimedEventDuration), a;
}
function tp(e, t, n, r) {
  var o = Qi(e.defs, t), a = mt();
  for (var i in e.defs) {
    var s = e.defs[i];
    a.defs[i] = np(s, o[i], n, r);
  }
  for (var l in e.instances) {
    var u = e.instances[l], s = a.defs[u.defId];
    a.instances[l] = rp(u, s, o[u.defId], n, r);
  }
  return a;
}
function np(e, t, n, r) {
  var o = n.standardProps || {};
  o.hasEnd == null && t.durationEditable && (n.startDelta || n.endDelta) && (o.hasEnd = !0);
  var a = v(v(v({}, e), o), { ui: v(v({}, e.ui), o.ui) });
  n.extendedProps && (a.extendedProps = v(v({}, a.extendedProps), n.extendedProps));
  for (var i = 0, s = r.pluginHooks.eventDefMutationAppliers; i < s.length; i++) {
    var l = s[i];
    l(a, n, r);
  }
  return !a.hasEnd && r.options.forceEventDuration && (a.hasEnd = !0), a;
}
function rp(e, t, n, r, o) {
  var a = o.dateEnv, i = r.standardProps && r.standardProps.allDay === !0, s = r.standardProps && r.standardProps.hasEnd === !1, l = v({}, e);
  return i && (l.range = Ji(l.range)), r.datesDelta && n.startEditable && (l.range = {
    start: a.add(l.range.start, r.datesDelta),
    end: a.add(l.range.end, r.datesDelta)
  }), r.startDelta && n.durationEditable && (l.range = {
    start: a.add(l.range.start, r.startDelta),
    end: l.range.end
  }), r.endDelta && n.durationEditable && (l.range = {
    start: l.range.start,
    end: a.add(l.range.end, r.endDelta)
  }), s && (l.range = {
    start: l.range.start,
    end: Ha(t.allDay, l.range.start, o)
  }), t.allDay && (l.range = {
    start: Q(l.range.start),
    end: Q(l.range.end)
  }), l.range.end < l.range.start && (l.range.end = Ha(t.allDay, l.range.start, o)), l;
}
var op = function() {
  function e(t, n, r) {
    this.type = t, this.getCurrentData = n, this.dateEnv = r;
  }
  return Object.defineProperty(e.prototype, "calendar", {
    get: function() {
      return this.getCurrentData().calendarApi;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "title", {
    get: function() {
      return this.getCurrentData().viewTitle;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "activeStart", {
    get: function() {
      return this.dateEnv.toDate(this.getCurrentData().dateProfile.activeRange.start);
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "activeEnd", {
    get: function() {
      return this.dateEnv.toDate(this.getCurrentData().dateProfile.activeRange.end);
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "currentStart", {
    get: function() {
      return this.dateEnv.toDate(this.getCurrentData().dateProfile.currentRange.start);
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "currentEnd", {
    get: function() {
      return this.dateEnv.toDate(this.getCurrentData().dateProfile.currentRange.end);
    },
    enumerable: !1,
    configurable: !0
  }), e.prototype.getOption = function(t) {
    return this.getCurrentData().options[t];
  }, e;
}(), ap = {
  id: String,
  defaultAllDay: Boolean,
  url: String,
  format: String,
  events: g,
  eventDataTransform: g,
  success: g,
  failure: g
};
function os(e, t, n) {
  n === void 0 && (n = as(t));
  var r;
  if (typeof e == "string" ? r = { url: e } : typeof e == "function" || Array.isArray(e) ? r = { events: e } : typeof e == "object" && e && (r = e), r) {
    var o = Co(r, n), a = o.refined, i = o.extra, s = ip(a, t);
    if (s)
      return {
        _raw: e,
        isFetching: !1,
        latestFetchId: "",
        fetchRange: null,
        defaultAllDay: a.defaultAllDay,
        eventDataTransform: a.eventDataTransform,
        success: a.success,
        failure: a.failure,
        publicId: a.id || "",
        sourceId: Lt(),
        sourceDefId: s.sourceDefId,
        meta: s.meta,
        ui: Xn(a, t),
        extendedProps: i
      };
  }
  return null;
}
function as(e) {
  return v(v(v({}, Zn), ap), e.pluginHooks.eventSourceRefiners);
}
function ip(e, t) {
  for (var n = t.pluginHooks.eventSourceDefs, r = n.length - 1; r >= 0; r -= 1) {
    var o = n[r], a = o.parseMeta(e);
    if (a)
      return { sourceDefId: r, meta: a };
  }
  return null;
}
function sp(e, t) {
  switch (t.type) {
    case "CHANGE_DATE":
      return t.dateMarker;
    default:
      return e;
  }
}
function lp(e, t) {
  var n = e.initialDate;
  return n != null ? t.createMarker(n) : dn(e.now, t);
}
function dn(e, t) {
  return typeof e == "function" && (e = e()), e == null ? t.createNowMarker() : t.createMarker(e);
}
var up = function() {
  function e() {
  }
  return e.prototype.getCurrentData = function() {
    return this.currentDataManager.getCurrentData();
  }, e.prototype.dispatch = function(t) {
    return this.currentDataManager.dispatch(t);
  }, Object.defineProperty(e.prototype, "view", {
    get: function() {
      return this.getCurrentData().viewApi;
    },
    enumerable: !1,
    configurable: !0
  }), e.prototype.batchRendering = function(t) {
    t();
  }, e.prototype.updateSize = function() {
    this.trigger("_resize", !0);
  }, e.prototype.setOption = function(t, n) {
    this.dispatch({
      type: "SET_OPTION",
      optionName: t,
      rawOptionValue: n
    });
  }, e.prototype.getOption = function(t) {
    return this.currentDataManager.currentCalendarOptionsInput[t];
  }, e.prototype.getAvailableLocaleCodes = function() {
    return Object.keys(this.getCurrentData().availableRawLocales);
  }, e.prototype.on = function(t, n) {
    var r = this.currentDataManager;
    r.currentCalendarOptionsRefiners[t] ? r.emitter.on(t, n) : console.warn("Unknown listener name '" + t + "'");
  }, e.prototype.off = function(t, n) {
    this.currentDataManager.emitter.off(t, n);
  }, e.prototype.trigger = function(t) {
    for (var n, r = [], o = 1; o < arguments.length; o++)
      r[o - 1] = arguments[o];
    (n = this.currentDataManager.emitter).trigger.apply(n, re([t], r));
  }, e.prototype.changeView = function(t, n) {
    var r = this;
    this.batchRendering(function() {
      if (r.unselect(), n)
        if (n.start && n.end)
          r.dispatch({
            type: "CHANGE_VIEW_TYPE",
            viewType: t
          }), r.dispatch({
            type: "SET_OPTION",
            optionName: "visibleRange",
            rawOptionValue: n
          });
        else {
          var o = r.getCurrentData().dateEnv;
          r.dispatch({
            type: "CHANGE_VIEW_TYPE",
            viewType: t,
            dateMarker: o.createMarker(n)
          });
        }
      else
        r.dispatch({
          type: "CHANGE_VIEW_TYPE",
          viewType: t
        });
    });
  }, e.prototype.zoomTo = function(t, n) {
    var r = this.getCurrentData(), o;
    n = n || "day", o = r.viewSpecs[n] || this.getUnitViewSpec(n), this.unselect(), o ? this.dispatch({
      type: "CHANGE_VIEW_TYPE",
      viewType: o.type,
      dateMarker: t
    }) : this.dispatch({
      type: "CHANGE_DATE",
      dateMarker: t
    });
  }, e.prototype.getUnitViewSpec = function(t) {
    var n = this.getCurrentData(), r = n.viewSpecs, o = n.toolbarConfig, a = [].concat(o.header ? o.header.viewsWithButtons : [], o.footer ? o.footer.viewsWithButtons : []), i, s;
    for (var l in r)
      a.push(l);
    for (i = 0; i < a.length; i += 1)
      if (s = r[a[i]], s && s.singleUnit === t)
        return s;
    return null;
  }, e.prototype.prev = function() {
    this.unselect(), this.dispatch({ type: "PREV" });
  }, e.prototype.next = function() {
    this.unselect(), this.dispatch({ type: "NEXT" });
  }, e.prototype.prevYear = function() {
    var t = this.getCurrentData();
    this.unselect(), this.dispatch({
      type: "CHANGE_DATE",
      dateMarker: t.dateEnv.addYears(t.currentDate, -1)
    });
  }, e.prototype.nextYear = function() {
    var t = this.getCurrentData();
    this.unselect(), this.dispatch({
      type: "CHANGE_DATE",
      dateMarker: t.dateEnv.addYears(t.currentDate, 1)
    });
  }, e.prototype.today = function() {
    var t = this.getCurrentData();
    this.unselect(), this.dispatch({
      type: "CHANGE_DATE",
      dateMarker: dn(t.calendarOptions.now, t.dateEnv)
    });
  }, e.prototype.gotoDate = function(t) {
    var n = this.getCurrentData();
    this.unselect(), this.dispatch({
      type: "CHANGE_DATE",
      dateMarker: n.dateEnv.createMarker(t)
    });
  }, e.prototype.incrementDate = function(t) {
    var n = this.getCurrentData(), r = Y(t);
    r && (this.unselect(), this.dispatch({
      type: "CHANGE_DATE",
      dateMarker: n.dateEnv.add(n.currentDate, r)
    }));
  }, e.prototype.getDate = function() {
    var t = this.getCurrentData();
    return t.dateEnv.toDate(t.currentDate);
  }, e.prototype.formatDate = function(t, n) {
    var r = this.getCurrentData().dateEnv;
    return r.format(r.createMarker(t), le(n));
  }, e.prototype.formatRange = function(t, n, r) {
    var o = this.getCurrentData().dateEnv;
    return o.formatRange(o.createMarker(t), o.createMarker(n), le(r), r);
  }, e.prototype.formatIso = function(t, n) {
    var r = this.getCurrentData().dateEnv;
    return r.formatIso(r.createMarker(t), { omitTime: n });
  }, e.prototype.select = function(t, n) {
    var r;
    n == null ? t.start != null ? r = t : r = {
      start: t,
      end: null
    } : r = {
      start: t,
      end: n
    };
    var o = this.getCurrentData(), a = qf(r, o.dateEnv, Y({ days: 1 }));
    a && (this.dispatch({ type: "SELECT_DATES", selection: a }), Jf(a, null, o));
  }, e.prototype.unselect = function(t) {
    var n = this.getCurrentData();
    n.dateSelection && (this.dispatch({ type: "UNSELECT_DATES" }), Qf(t, n));
  }, e.prototype.addEvent = function(t, n) {
    if (t instanceof Ae) {
      var r = t._def, o = t._instance, a = this.getCurrentData();
      return a.eventStore.defs[r.defId] || (this.dispatch({
        type: "ADD_EVENTS",
        eventStore: eo({ def: r, instance: o })
      }), this.triggerEventAdd(t)), t;
    }
    var i = this.getCurrentData(), s;
    if (n instanceof wt)
      s = n.internalEventSource;
    else if (typeof n == "boolean")
      n && (s = Eo(i.eventSources)[0]);
    else if (n != null) {
      var l = this.getEventSourceById(n);
      if (!l)
        return console.warn('Could not find an event source with ID "' + n + '"'), null;
      s = l.internalEventSource;
    }
    var u = Xi(t, s, i, !1);
    if (u) {
      var c = new Ae(i, u.def, u.def.recurringDef ? null : u.instance);
      return this.dispatch({
        type: "ADD_EVENTS",
        eventStore: eo(u)
      }), this.triggerEventAdd(c), c;
    }
    return null;
  }, e.prototype.triggerEventAdd = function(t) {
    var n = this, r = this.getCurrentData().emitter;
    r.trigger("eventAdd", {
      event: t,
      relatedEvents: [],
      revert: function() {
        n.dispatch({
          type: "REMOVE_EVENTS",
          eventStore: is(t)
        });
      }
    });
  }, e.prototype.getEventById = function(t) {
    var n = this.getCurrentData(), r = n.eventStore, o = r.defs, a = r.instances;
    t = String(t);
    for (var i in o) {
      var s = o[i];
      if (s.publicId === t) {
        if (s.recurringDef)
          return new Ae(n, s, null);
        for (var l in a) {
          var u = a[l];
          if (u.defId === s.defId)
            return new Ae(n, s, u);
        }
      }
    }
    return null;
  }, e.prototype.getEvents = function() {
    var t = this.getCurrentData();
    return ko(t.eventStore, t);
  }, e.prototype.removeAllEvents = function() {
    this.dispatch({ type: "REMOVE_ALL_EVENTS" });
  }, e.prototype.getEventSources = function() {
    var t = this.getCurrentData(), n = t.eventSources, r = [];
    for (var o in n)
      r.push(new wt(t, n[o]));
    return r;
  }, e.prototype.getEventSourceById = function(t) {
    var n = this.getCurrentData(), r = n.eventSources;
    t = String(t);
    for (var o in r)
      if (r[o].publicId === t)
        return new wt(n, r[o]);
    return null;
  }, e.prototype.addEventSource = function(t) {
    var n = this.getCurrentData();
    if (t instanceof wt)
      return n.eventSources[t.internalEventSource.sourceId] || this.dispatch({
        type: "ADD_EVENT_SOURCES",
        sources: [t.internalEventSource]
      }), t;
    var r = os(t, n);
    return r ? (this.dispatch({ type: "ADD_EVENT_SOURCES", sources: [r] }), new wt(n, r)) : null;
  }, e.prototype.removeAllEventSources = function() {
    this.dispatch({ type: "REMOVE_ALL_EVENT_SOURCES" });
  }, e.prototype.refetchEvents = function() {
    this.dispatch({ type: "FETCH_EVENT_SOURCES", isRefetch: !0 });
  }, e.prototype.scrollToTime = function(t) {
    var n = Y(t);
    n && this.trigger("_scrollRequest", { time: n });
  }, e;
}(), Ae = function() {
  function e(t, n, r) {
    this._context = t, this._def = n, this._instance = r || null;
  }
  return e.prototype.setProp = function(t, n) {
    var r, o;
    if (t in Zi)
      console.warn("Could not set date-related prop 'name'. Use one of the date-related methods instead.");
    else if (t === "id")
      n = On[t](n), this.mutate({
        standardProps: { publicId: n }
      });
    else if (t in On)
      n = On[t](n), this.mutate({
        standardProps: (r = {}, r[t] = n, r)
      });
    else if (t in Zn) {
      var a = Zn[t](n);
      t === "color" ? a = { backgroundColor: n, borderColor: n } : t === "editable" ? a = { startEditable: n, durationEditable: n } : a = (o = {}, o[t] = n, o), this.mutate({
        standardProps: { ui: a }
      });
    } else
      console.warn("Could not set prop '" + t + "'. Use setExtendedProp instead.");
  }, e.prototype.setExtendedProp = function(t, n) {
    var r;
    this.mutate({
      extendedProps: (r = {}, r[t] = n, r)
    });
  }, e.prototype.setStart = function(t, n) {
    n === void 0 && (n = {});
    var r = this._context.dateEnv, o = r.createMarker(t);
    if (o && this._instance) {
      var a = this._instance.range, i = En(a.start, o, r, n.granularity);
      n.maintainDuration ? this.mutate({ datesDelta: i }) : this.mutate({ startDelta: i });
    }
  }, e.prototype.setEnd = function(t, n) {
    n === void 0 && (n = {});
    var r = this._context.dateEnv, o;
    if (!(t != null && (o = r.createMarker(t), !o)) && this._instance)
      if (o) {
        var a = En(this._instance.range.end, o, r, n.granularity);
        this.mutate({ endDelta: a });
      } else
        this.mutate({ standardProps: { hasEnd: !1 } });
  }, e.prototype.setDates = function(t, n, r) {
    r === void 0 && (r = {});
    var o = this._context.dateEnv, a = { allDay: r.allDay }, i = o.createMarker(t), s;
    if (!!i && !(n != null && (s = o.createMarker(n), !s)) && this._instance) {
      var l = this._instance.range;
      r.allDay === !0 && (l = Ji(l));
      var u = En(l.start, i, o, r.granularity);
      if (s) {
        var c = En(l.end, s, o, r.granularity);
        of(u, c) ? this.mutate({ datesDelta: u, standardProps: a }) : this.mutate({ startDelta: u, endDelta: c, standardProps: a });
      } else
        a.hasEnd = !1, this.mutate({ datesDelta: u, standardProps: a });
    }
  }, e.prototype.moveStart = function(t) {
    var n = Y(t);
    n && this.mutate({ startDelta: n });
  }, e.prototype.moveEnd = function(t) {
    var n = Y(t);
    n && this.mutate({ endDelta: n });
  }, e.prototype.moveDates = function(t) {
    var n = Y(t);
    n && this.mutate({ datesDelta: n });
  }, e.prototype.setAllDay = function(t, n) {
    n === void 0 && (n = {});
    var r = { allDay: t }, o = n.maintainDuration;
    o == null && (o = this._context.options.allDayMaintainDuration), this._def.allDay !== t && (r.hasEnd = o), this.mutate({ standardProps: r });
  }, e.prototype.formatRange = function(t) {
    var n = this._context.dateEnv, r = this._instance, o = le(t);
    return this._def.hasEnd ? n.formatRange(r.range.start, r.range.end, o, {
      forcedStartTzo: r.forcedStartTzo,
      forcedEndTzo: r.forcedEndTzo
    }) : n.format(r.range.start, o, {
      forcedTzo: r.forcedStartTzo
    });
  }, e.prototype.mutate = function(t) {
    var n = this._instance;
    if (n) {
      var r = this._def, o = this._context, a = o.getCurrentData().eventStore, i = Tf(a, n.instanceId), s = {
        "": {
          display: "",
          startEditable: !0,
          durationEditable: !0,
          constraints: [],
          overlap: null,
          allows: [],
          backgroundColor: "",
          borderColor: "",
          textColor: "",
          classNames: []
        }
      };
      i = tp(i, s, t, o);
      var l = new e(o, r, n);
      this._def = i.defs[r.defId], this._instance = i.instances[n.instanceId], o.dispatch({
        type: "MERGE_EVENTS",
        eventStore: i
      }), o.emitter.trigger("eventChange", {
        oldEvent: l,
        event: this,
        relatedEvents: ko(i, o, n),
        revert: function() {
          o.dispatch({
            type: "RESET_EVENTS",
            eventStore: a
          });
        }
      });
    }
  }, e.prototype.remove = function() {
    var t = this._context, n = is(this);
    t.dispatch({
      type: "REMOVE_EVENTS",
      eventStore: n
    }), t.emitter.trigger("eventRemove", {
      event: this,
      relatedEvents: [],
      revert: function() {
        t.dispatch({
          type: "MERGE_EVENTS",
          eventStore: n
        });
      }
    });
  }, Object.defineProperty(e.prototype, "source", {
    get: function() {
      var t = this._def.sourceId;
      return t ? new wt(this._context, this._context.getCurrentData().eventSources[t]) : null;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "start", {
    get: function() {
      return this._instance ? this._context.dateEnv.toDate(this._instance.range.start) : null;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "end", {
    get: function() {
      return this._instance && this._def.hasEnd ? this._context.dateEnv.toDate(this._instance.range.end) : null;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "startStr", {
    get: function() {
      var t = this._instance;
      return t ? this._context.dateEnv.formatIso(t.range.start, {
        omitTime: this._def.allDay,
        forcedTzo: t.forcedStartTzo
      }) : "";
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "endStr", {
    get: function() {
      var t = this._instance;
      return t && this._def.hasEnd ? this._context.dateEnv.formatIso(t.range.end, {
        omitTime: this._def.allDay,
        forcedTzo: t.forcedEndTzo
      }) : "";
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "id", {
    get: function() {
      return this._def.publicId;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "groupId", {
    get: function() {
      return this._def.groupId;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "allDay", {
    get: function() {
      return this._def.allDay;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "title", {
    get: function() {
      return this._def.title;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "url", {
    get: function() {
      return this._def.url;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "display", {
    get: function() {
      return this._def.ui.display || "auto";
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "startEditable", {
    get: function() {
      return this._def.ui.startEditable;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "durationEditable", {
    get: function() {
      return this._def.ui.durationEditable;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "constraint", {
    get: function() {
      return this._def.ui.constraints[0] || null;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "overlap", {
    get: function() {
      return this._def.ui.overlap;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "allow", {
    get: function() {
      return this._def.ui.allows[0] || null;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "backgroundColor", {
    get: function() {
      return this._def.ui.backgroundColor;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "borderColor", {
    get: function() {
      return this._def.ui.borderColor;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "textColor", {
    get: function() {
      return this._def.ui.textColor;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "classNames", {
    get: function() {
      return this._def.ui.classNames;
    },
    enumerable: !1,
    configurable: !0
  }), Object.defineProperty(e.prototype, "extendedProps", {
    get: function() {
      return this._def.extendedProps;
    },
    enumerable: !1,
    configurable: !0
  }), e.prototype.toPlainObject = function(t) {
    t === void 0 && (t = {});
    var n = this._def, r = n.ui, o = this, a = o.startStr, i = o.endStr, s = {};
    return n.title && (s.title = n.title), a && (s.start = a), i && (s.end = i), n.publicId && (s.id = n.publicId), n.groupId && (s.groupId = n.groupId), n.url && (s.url = n.url), r.display && r.display !== "auto" && (s.display = r.display), t.collapseColor && r.backgroundColor && r.backgroundColor === r.borderColor ? s.color = r.backgroundColor : (r.backgroundColor && (s.backgroundColor = r.backgroundColor), r.borderColor && (s.borderColor = r.borderColor)), r.textColor && (s.textColor = r.textColor), r.classNames.length && (s.classNames = r.classNames), Object.keys(n.extendedProps).length && (t.collapseExtendedProps ? v(s, n.extendedProps) : s.extendedProps = n.extendedProps), s;
  }, e.prototype.toJSON = function() {
    return this.toPlainObject();
  }, e;
}();
function is(e) {
  var t, n, r = e._def, o = e._instance;
  return {
    defs: (t = {}, t[r.defId] = r, t),
    instances: o ? (n = {}, n[o.instanceId] = o, n) : {}
  };
}
function ko(e, t, n) {
  var r = e.defs, o = e.instances, a = [], i = n ? n.instanceId : "";
  for (var s in o) {
    var l = o[s], u = r[l.defId];
    l.instanceId !== i && a.push(new Ae(t, u, l));
  }
  return a;
}
var ss = {};
function cp(e, t) {
  ss[e] = t;
}
function dp(e) {
  return new ss[e]();
}
var fp = function() {
  function e() {
  }
  return e.prototype.getMarkerYear = function(t) {
    return t.getUTCFullYear();
  }, e.prototype.getMarkerMonth = function(t) {
    return t.getUTCMonth();
  }, e.prototype.getMarkerDay = function(t) {
    return t.getUTCDate();
  }, e.prototype.arrayToMarker = function(t) {
    return _e(t);
  }, e.prototype.markerToArray = function(t) {
    return Qe(t);
  }, e;
}();
cp("gregory", fp);
var pp = /^\s*(\d{4})(-?(\d{2})(-?(\d{2})([T ](\d{2}):?(\d{2})(:?(\d{2})(\.(\d+))?)?(Z|(([-+])(\d{2})(:?(\d{2}))?))?)?)?)?$/;
function hp(e) {
  var t = pp.exec(e);
  if (t) {
    var n = new Date(Date.UTC(Number(t[1]), t[3] ? Number(t[3]) - 1 : 0, Number(t[5] || 1), Number(t[7] || 0), Number(t[8] || 0), Number(t[10] || 0), t[12] ? Number("0." + t[12]) * 1e3 : 0));
    if (Yi(n)) {
      var r = null;
      return t[13] && (r = (t[15] === "-" ? -1 : 1) * (Number(t[16] || 0) * 60 + Number(t[18] || 0))), {
        marker: n,
        isTimeUnspecified: !t[6],
        timeZoneOffset: r
      };
    }
  }
  return null;
}
var vp = function() {
  function e(t) {
    var n = this.timeZone = t.timeZone, r = n !== "local" && n !== "UTC";
    t.namedTimeZoneImpl && r && (this.namedTimeZoneImpl = new t.namedTimeZoneImpl(n)), this.canComputeOffset = Boolean(!r || this.namedTimeZoneImpl), this.calendarSystem = dp(t.calendarSystem), this.locale = t.locale, this.weekDow = t.locale.week.dow, this.weekDoy = t.locale.week.doy, t.weekNumberCalculation === "ISO" && (this.weekDow = 1, this.weekDoy = 4), typeof t.firstDay == "number" && (this.weekDow = t.firstDay), typeof t.weekNumberCalculation == "function" && (this.weekNumberFunc = t.weekNumberCalculation), this.weekText = t.weekText != null ? t.weekText : t.locale.options.weekText, this.weekTextLong = (t.weekTextLong != null ? t.weekTextLong : t.locale.options.weekTextLong) || this.weekText, this.cmdFormatter = t.cmdFormatter, this.defaultSeparator = t.defaultSeparator;
  }
  return e.prototype.createMarker = function(t) {
    var n = this.createMarkerMeta(t);
    return n === null ? null : n.marker;
  }, e.prototype.createNowMarker = function() {
    return this.canComputeOffset ? this.timestampToMarker(new Date().valueOf()) : _e(Sa(new Date()));
  }, e.prototype.createMarkerMeta = function(t) {
    if (typeof t == "string")
      return this.parse(t);
    var n = null;
    return typeof t == "number" ? n = this.timestampToMarker(t) : t instanceof Date ? (t = t.valueOf(), isNaN(t) || (n = this.timestampToMarker(t))) : Array.isArray(t) && (n = _e(t)), n === null || !Yi(n) ? null : { marker: n, isTimeUnspecified: !1, forcedTzo: null };
  }, e.prototype.parse = function(t) {
    var n = hp(t);
    if (n === null)
      return null;
    var r = n.marker, o = null;
    return n.timeZoneOffset !== null && (this.canComputeOffset ? r = this.timestampToMarker(r.valueOf() - n.timeZoneOffset * 60 * 1e3) : o = n.timeZoneOffset), { marker: r, isTimeUnspecified: n.isTimeUnspecified, forcedTzo: o };
  }, e.prototype.getYear = function(t) {
    return this.calendarSystem.getMarkerYear(t);
  }, e.prototype.getMonth = function(t) {
    return this.calendarSystem.getMarkerMonth(t);
  }, e.prototype.add = function(t, n) {
    var r = this.calendarSystem.markerToArray(t);
    return r[0] += n.years, r[1] += n.months, r[2] += n.days, r[6] += n.milliseconds, this.calendarSystem.arrayToMarker(r);
  }, e.prototype.subtract = function(t, n) {
    var r = this.calendarSystem.markerToArray(t);
    return r[0] -= n.years, r[1] -= n.months, r[2] -= n.days, r[6] -= n.milliseconds, this.calendarSystem.arrayToMarker(r);
  }, e.prototype.addYears = function(t, n) {
    var r = this.calendarSystem.markerToArray(t);
    return r[0] += n, this.calendarSystem.arrayToMarker(r);
  }, e.prototype.addMonths = function(t, n) {
    var r = this.calendarSystem.markerToArray(t);
    return r[1] += n, this.calendarSystem.arrayToMarker(r);
  }, e.prototype.diffWholeYears = function(t, n) {
    var r = this.calendarSystem;
    return et(t) === et(n) && r.getMarkerDay(t) === r.getMarkerDay(n) && r.getMarkerMonth(t) === r.getMarkerMonth(n) ? r.getMarkerYear(n) - r.getMarkerYear(t) : null;
  }, e.prototype.diffWholeMonths = function(t, n) {
    var r = this.calendarSystem;
    return et(t) === et(n) && r.getMarkerDay(t) === r.getMarkerDay(n) ? r.getMarkerMonth(n) - r.getMarkerMonth(t) + (r.getMarkerYear(n) - r.getMarkerYear(t)) * 12 : null;
  }, e.prototype.greatestWholeUnit = function(t, n) {
    var r = this.diffWholeYears(t, n);
    return r !== null ? { unit: "year", value: r } : (r = this.diffWholeMonths(t, n), r !== null ? { unit: "month", value: r } : (r = jd(t, n), r !== null ? { unit: "week", value: r } : (r = zn(t, n), r !== null ? { unit: "day", value: r } : (r = Wd(t, n), Mr(r) ? { unit: "hour", value: r } : (r = $d(t, n), Mr(r) ? { unit: "minute", value: r } : (r = zd(t, n), Mr(r) ? { unit: "second", value: r } : { unit: "millisecond", value: n.valueOf() - t.valueOf() }))))));
  }, e.prototype.countDurationsBetween = function(t, n, r) {
    var o;
    return r.years && (o = this.diffWholeYears(t, n), o !== null) ? o / sf(r) : r.months && (o = this.diffWholeMonths(t, n), o !== null) ? o / lf(r) : r.days && (o = zn(t, n), o !== null) ? o / It(r) : (n.valueOf() - t.valueOf()) / on(r);
  }, e.prototype.startOf = function(t, n) {
    return n === "year" ? this.startOfYear(t) : n === "month" ? this.startOfMonth(t) : n === "week" ? this.startOfWeek(t) : n === "day" ? Q(t) : n === "hour" ? Yd(t) : n === "minute" ? qd(t) : n === "second" ? Zd(t) : null;
  }, e.prototype.startOfYear = function(t) {
    return this.calendarSystem.arrayToMarker([
      this.calendarSystem.getMarkerYear(t)
    ]);
  }, e.prototype.startOfMonth = function(t) {
    return this.calendarSystem.arrayToMarker([
      this.calendarSystem.getMarkerYear(t),
      this.calendarSystem.getMarkerMonth(t)
    ]);
  }, e.prototype.startOfWeek = function(t) {
    return this.calendarSystem.arrayToMarker([
      this.calendarSystem.getMarkerYear(t),
      this.calendarSystem.getMarkerMonth(t),
      t.getUTCDate() - (t.getUTCDay() - this.weekDow + 7) % 7
    ]);
  }, e.prototype.computeWeekNumber = function(t) {
    return this.weekNumberFunc ? this.weekNumberFunc(this.toDate(t)) : Xd(t, this.weekDow, this.weekDoy);
  }, e.prototype.format = function(t, n, r) {
    return r === void 0 && (r = {}), n.format({
      marker: t,
      timeZoneOffset: r.forcedTzo != null ? r.forcedTzo : this.offsetForMarker(t)
    }, this);
  }, e.prototype.formatRange = function(t, n, r, o) {
    return o === void 0 && (o = {}), o.isEndExclusive && (n = vt(n, -1)), r.formatRange({
      marker: t,
      timeZoneOffset: o.forcedStartTzo != null ? o.forcedStartTzo : this.offsetForMarker(t)
    }, {
      marker: n,
      timeZoneOffset: o.forcedEndTzo != null ? o.forcedEndTzo : this.offsetForMarker(n)
    }, this, o.defaultSeparator);
  }, e.prototype.formatIso = function(t, n) {
    n === void 0 && (n = {});
    var r = null;
    return n.omitTimeZoneOffset || (n.forcedTzo != null ? r = n.forcedTzo : r = this.offsetForMarker(t)), uf(t, r, n.omitTime);
  }, e.prototype.timestampToMarker = function(t) {
    return this.timeZone === "local" ? _e(Sa(new Date(t))) : this.timeZone === "UTC" || !this.namedTimeZoneImpl ? new Date(t) : _e(this.namedTimeZoneImpl.timestampToArray(t));
  }, e.prototype.offsetForMarker = function(t) {
    return this.timeZone === "local" ? -wa(Qe(t)).getTimezoneOffset() : this.timeZone === "UTC" ? 0 : this.namedTimeZoneImpl ? this.namedTimeZoneImpl.offsetForArray(Qe(t)) : null;
  }, e.prototype.toDate = function(t, n) {
    return this.timeZone === "local" ? wa(Qe(t)) : this.timeZone === "UTC" ? new Date(t.valueOf()) : this.namedTimeZoneImpl ? new Date(t.valueOf() - this.namedTimeZoneImpl.offsetForArray(Qe(t)) * 1e3 * 60) : new Date(t.valueOf() - (n || 0));
  }, e;
}(), mp = [], ls = {
  code: "en",
  week: {
    dow: 0,
    doy: 4
  },
  direction: "ltr",
  buttonText: {
    prev: "prev",
    next: "next",
    prevYear: "prev year",
    nextYear: "next year",
    year: "year",
    today: "today",
    month: "month",
    week: "week",
    day: "day",
    list: "list"
  },
  weekText: "W",
  weekTextLong: "Week",
  closeHint: "Close",
  timeHint: "Time",
  eventHint: "Event",
  allDayText: "all-day",
  moreLinkText: "more",
  noEventsText: "No events to display"
}, us = v(v({}, ls), {
  buttonHints: {
    prev: "Previous $0",
    next: "Next $0",
    today: function(e, t) {
      return t === "day" ? "Today" : "This " + e;
    }
  },
  viewHint: "$0 view",
  navLinkHint: "Go to $0",
  moreLinkHint: function(e) {
    return "Show " + e + " more event" + (e === 1 ? "" : "s");
  }
});
function gp(e) {
  for (var t = e.length > 0 ? e[0].code : "en", n = mp.concat(e), r = {
    en: us
  }, o = 0, a = n; o < a.length; o++) {
    var i = a[o];
    r[i.code] = i;
  }
  return {
    map: r,
    defaultCode: t
  };
}
function cs(e, t) {
  return typeof e == "object" && !Array.isArray(e) ? ds(e.code, [e.code], e) : yp(e, t);
}
function yp(e, t) {
  var n = [].concat(e || []), r = bp(n, t) || us;
  return ds(e, n, r);
}
function bp(e, t) {
  for (var n = 0; n < e.length; n += 1)
    for (var r = e[n].toLocaleLowerCase().split("-"), o = r.length; o > 0; o -= 1) {
      var a = r.slice(0, o).join("-");
      if (t[a])
        return t[a];
    }
  return null;
}
function ds(e, t, n) {
  var r = bo([ls, n], ["buttonText"]);
  delete r.code;
  var o = r.week;
  return delete r.week, {
    codeArg: e,
    codes: t,
    week: o,
    simpleNumberFormat: new Intl.NumberFormat(e),
    options: r
  };
}
var Ep = {
  startTime: "09:00",
  endTime: "17:00",
  daysOfWeek: [1, 2, 3, 4, 5],
  display: "inverse-background",
  classNames: "fc-non-business",
  groupId: "_businessHours"
};
function _p(e, t) {
  return qn(Cp(e), null, t);
}
function Cp(e) {
  var t;
  return e === !0 ? t = [{}] : Array.isArray(e) ? t = e.filter(function(n) {
    return n.daysOfWeek;
  }) : typeof e == "object" && e ? t = [e] : t = [], t = t.map(function(n) {
    return v(v({}, Ep), n);
  }), t;
}
function Dp(e, t) {
  var n = {
    left: Math.max(e.left, t.left),
    right: Math.min(e.right, t.right),
    top: Math.max(e.top, t.top),
    bottom: Math.min(e.bottom, t.bottom)
  };
  return n.left < n.right && n.top < n.bottom ? n : !1;
}
var Nr;
function fs() {
  return Nr == null && (Nr = Sp()), Nr;
}
function Sp() {
  if (typeof document > "u")
    return !0;
  var e = document.createElement("div");
  e.style.position = "absolute", e.style.top = "0px", e.style.left = "0px", e.innerHTML = "<table><tr><td><div></div></td></tr></table>", e.querySelector("table").style.height = "100px", e.querySelector("div").style.height = "100%", document.body.appendChild(e);
  var t = e.querySelector("div"), n = t.offsetHeight > 0;
  return document.body.removeChild(e), n;
}
function Io(e, t, n, r) {
  return {
    dow: e.getUTCDay(),
    isDisabled: Boolean(r && !ft(r.activeRange, e)),
    isOther: Boolean(r && !ft(r.currentRange, e)),
    isToday: Boolean(t && ft(t, e)),
    isPast: Boolean(n ? e < n : t ? e < t.start : !1),
    isFuture: Boolean(n ? e > n : t ? e >= t.end : !1)
  };
}
function dr(e, t) {
  var n = [
    "fc-day",
    "fc-day-" + Ld[e.dow]
  ];
  return e.isDisabled ? n.push("fc-day-disabled") : (e.isToday && (n.push("fc-day-today"), n.push(t.getClass("today"))), e.isPast && n.push("fc-day-past"), e.isFuture && n.push("fc-day-future"), e.isOther && n.push("fc-day-other")), n;
}
var wp = le({ year: "numeric", month: "long", day: "numeric" }), Tp = le({ week: "long" });
function an(e, t, n, r) {
  n === void 0 && (n = "day"), r === void 0 && (r = !0);
  var o = e.dateEnv, a = e.options, i = e.calendarApi, s = o.format(t, n === "week" ? Tp : wp);
  if (a.navLinks) {
    var l = o.toDate(t), u = function(c) {
      var d = n === "day" ? a.navLinkDayClick : n === "week" ? a.navLinkWeekClick : null;
      typeof d == "function" ? d.call(i, o.toDate(t), c) : (typeof d == "string" && (n = d), i.zoomTo(t, n));
    };
    return v({ title: Qt(a.navLinkHint, [s, l], s), "data-navlink": "" }, r ? Gi(u) : { onClick: u });
  }
  return { "aria-label": s };
}
var Hr;
function Rp() {
  return Hr || (Hr = kp()), Hr;
}
function kp() {
  var e = document.createElement("div");
  e.style.overflow = "scroll", e.style.position = "absolute", e.style.top = "-9999px", e.style.left = "-9999px", document.body.appendChild(e);
  var t = Ip(e);
  return document.body.removeChild(e), t;
}
function Ip(e) {
  return {
    x: e.offsetHeight - e.clientHeight,
    y: e.offsetWidth - e.clientWidth
  };
}
function xp(e) {
  for (var t = Mp(e), n = e.getBoundingClientRect(), r = 0, o = t; r < o.length; r++) {
    var a = o[r], i = Dp(n, a.getBoundingClientRect());
    if (i)
      n = i;
    else
      return null;
  }
  return n;
}
function Mp(e) {
  for (var t = []; e instanceof HTMLElement; ) {
    var n = window.getComputedStyle(e);
    if (n.position === "fixed")
      break;
    /(auto|scroll)/.test(n.overflow + n.overflowY + n.overflowX) && t.push(e), e = e.parentNode;
  }
  return t;
}
function Pp(e, t, n) {
  var r = !1, o = function() {
    r || (r = !0, t.apply(this, arguments));
  }, a = function() {
    r || (r = !0, n && n.apply(this, arguments));
  }, i = e(o, a);
  i && typeof i.then == "function" && i.then(o, a);
}
var Op = function() {
  function e() {
    this.handlers = {}, this.thisContext = null;
  }
  return e.prototype.setThisContext = function(t) {
    this.thisContext = t;
  }, e.prototype.setOptions = function(t) {
    this.options = t;
  }, e.prototype.on = function(t, n) {
    Ap(this.handlers, t, n);
  }, e.prototype.off = function(t, n) {
    Np(this.handlers, t, n);
  }, e.prototype.trigger = function(t) {
    for (var n = [], r = 1; r < arguments.length; r++)
      n[r - 1] = arguments[r];
    for (var o = this.handlers[t] || [], a = this.options && this.options[t], i = [].concat(a || [], o), s = 0, l = i; s < l.length; s++) {
      var u = l[s];
      u.apply(this.thisContext, n);
    }
  }, e.prototype.hasHandlers = function(t) {
    return Boolean(this.handlers[t] && this.handlers[t].length || this.options && this.options[t]);
  }, e;
}();
function Ap(e, t, n) {
  (e[t] || (e[t] = [])).push(n);
}
function Np(e, t, n) {
  n ? e[t] && (e[t] = e[t].filter(function(r) {
    return r !== n;
  })) : delete e[t];
}
var ao = function() {
  function e(t, n, r, o) {
    this.els = n;
    var a = this.originClientRect = t.getBoundingClientRect();
    r && this.buildElHorizontals(a.left), o && this.buildElVerticals(a.top);
  }
  return e.prototype.buildElHorizontals = function(t) {
    for (var n = [], r = [], o = 0, a = this.els; o < a.length; o++) {
      var i = a[o], s = i.getBoundingClientRect();
      n.push(s.left - t), r.push(s.right - t);
    }
    this.lefts = n, this.rights = r;
  }, e.prototype.buildElVerticals = function(t) {
    for (var n = [], r = [], o = 0, a = this.els; o < a.length; o++) {
      var i = a[o], s = i.getBoundingClientRect();
      n.push(s.top - t), r.push(s.bottom - t);
    }
    this.tops = n, this.bottoms = r;
  }, e.prototype.leftToIndex = function(t) {
    var n = this, r = n.lefts, o = n.rights, a = r.length, i;
    for (i = 0; i < a; i += 1)
      if (t >= r[i] && t < o[i])
        return i;
  }, e.prototype.topToIndex = function(t) {
    var n = this, r = n.tops, o = n.bottoms, a = r.length, i;
    for (i = 0; i < a; i += 1)
      if (t >= r[i] && t < o[i])
        return i;
  }, e.prototype.getWidth = function(t) {
    return this.rights[t] - this.lefts[t];
  }, e.prototype.getHeight = function(t) {
    return this.bottoms[t] - this.tops[t];
  }, e;
}(), ps = function() {
  function e() {
  }
  return e.prototype.getMaxScrollTop = function() {
    return this.getScrollHeight() - this.getClientHeight();
  }, e.prototype.getMaxScrollLeft = function() {
    return this.getScrollWidth() - this.getClientWidth();
  }, e.prototype.canScrollVertically = function() {
    return this.getMaxScrollTop() > 0;
  }, e.prototype.canScrollHorizontally = function() {
    return this.getMaxScrollLeft() > 0;
  }, e.prototype.canScrollUp = function() {
    return this.getScrollTop() > 0;
  }, e.prototype.canScrollDown = function() {
    return this.getScrollTop() < this.getMaxScrollTop();
  }, e.prototype.canScrollLeft = function() {
    return this.getScrollLeft() > 0;
  }, e.prototype.canScrollRight = function() {
    return this.getScrollLeft() < this.getMaxScrollLeft();
  }, e;
}();
(function(e) {
  H(t, e);
  function t(n) {
    var r = e.call(this) || this;
    return r.el = n, r;
  }
  return t.prototype.getScrollTop = function() {
    return this.el.scrollTop;
  }, t.prototype.getScrollLeft = function() {
    return this.el.scrollLeft;
  }, t.prototype.setScrollTop = function(n) {
    this.el.scrollTop = n;
  }, t.prototype.setScrollLeft = function(n) {
    this.el.scrollLeft = n;
  }, t.prototype.getScrollWidth = function() {
    return this.el.scrollWidth;
  }, t.prototype.getScrollHeight = function() {
    return this.el.scrollHeight;
  }, t.prototype.getClientHeight = function() {
    return this.el.clientHeight;
  }, t.prototype.getClientWidth = function() {
    return this.el.clientWidth;
  }, t;
})(ps);
(function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.getScrollTop = function() {
    return window.pageYOffset;
  }, t.prototype.getScrollLeft = function() {
    return window.pageXOffset;
  }, t.prototype.setScrollTop = function(n) {
    window.scroll(window.pageXOffset, n);
  }, t.prototype.setScrollLeft = function(n) {
    window.scroll(n, window.pageYOffset);
  }, t.prototype.getScrollWidth = function() {
    return document.documentElement.scrollWidth;
  }, t.prototype.getScrollHeight = function() {
    return document.documentElement.scrollHeight;
  }, t.prototype.getClientHeight = function() {
    return document.documentElement.clientHeight;
  }, t.prototype.getClientWidth = function() {
    return document.documentElement.clientWidth;
  }, t;
})(ps);
var fn = function() {
  function e(t) {
    this.iconOverrideOption && this.setIconOverride(t[this.iconOverrideOption]);
  }
  return e.prototype.setIconOverride = function(t) {
    var n, r;
    if (typeof t == "object" && t) {
      n = v({}, this.iconClasses);
      for (r in t)
        n[r] = this.applyIconOverridePrefix(t[r]);
      this.iconClasses = n;
    } else
      t === !1 && (this.iconClasses = {});
  }, e.prototype.applyIconOverridePrefix = function(t) {
    var n = this.iconOverridePrefix;
    return n && t.indexOf(n) !== 0 && (t = n + t), t;
  }, e.prototype.getClass = function(t) {
    return this.classes[t] || "";
  }, e.prototype.getIconClass = function(t, n) {
    var r;
    return n && this.rtlIconClasses ? r = this.rtlIconClasses[t] || this.iconClasses[t] : r = this.iconClasses[t], r ? this.baseIconClass + " " + r : "";
  }, e.prototype.getCustomButtonIconClass = function(t) {
    var n;
    return this.iconOverrideCustomButtonOption && (n = t[this.iconOverrideCustomButtonOption], n) ? this.baseIconClass + " " + this.applyIconOverridePrefix(n) : "";
  }, e;
}();
fn.prototype.classes = {};
fn.prototype.iconClasses = {};
fn.prototype.baseIconClass = "";
fn.prototype.iconOverridePrefix = "";
var Hp = function() {
  function e(t, n, r, o) {
    var a = this;
    this.execFunc = t, this.emitter = n, this.scrollTime = r, this.scrollTimeReset = o, this.handleScrollRequest = function(i) {
      a.queuedRequest = v({}, a.queuedRequest || {}, i), a.drain();
    }, n.on("_scrollRequest", this.handleScrollRequest), this.fireInitialScroll();
  }
  return e.prototype.detach = function() {
    this.emitter.off("_scrollRequest", this.handleScrollRequest);
  }, e.prototype.update = function(t) {
    t && this.scrollTimeReset ? this.fireInitialScroll() : this.drain();
  }, e.prototype.fireInitialScroll = function() {
    this.handleScrollRequest({
      time: this.scrollTime
    });
  }, e.prototype.drain = function() {
    this.queuedRequest && this.execFunc(this.queuedRequest) && (this.queuedRequest = null);
  }, e;
}(), yt = Wi({});
function Fp(e, t, n, r, o, a, i, s, l, u, c, d, f) {
  return {
    dateEnv: o,
    options: n,
    pluginHooks: i,
    emitter: u,
    dispatch: s,
    getCurrentData: l,
    calendarApi: c,
    viewSpec: e,
    viewApi: t,
    dateProfileGenerator: r,
    theme: a,
    isRtl: n.direction === "rtl",
    addResizeHandler: function(p) {
      u.on("_resize", p);
    },
    removeResizeHandler: function(p) {
      u.off("_resize", p);
    },
    createScrollResponder: function(p) {
      return new Hp(p, u, Y(n.scrollTime), n.scrollTimeReset);
    },
    registerInteractiveComponent: d,
    unregisterInteractiveComponent: f
  };
}
var fr = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.shouldComponentUpdate = function(n, r) {
    return this.debug && console.log(Ta(n, this.props), Ta(r, this.state)), !Or(this.props, n, this.propEquality) || !Or(this.state, r, this.stateEquality);
  }, t.prototype.safeSetState = function(n) {
    Or(this.state, v(v({}, this.state), n), this.stateEquality) || this.setState(n);
  }, t.addPropsEquality = Vp, t.addStateEquality = Up, t.contextType = yt, t;
}(go);
fr.prototype.propEquality = {};
fr.prototype.stateEquality = {};
var X = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.contextType = yt, t;
}(fr);
function Vp(e) {
  var t = Object.create(this.prototype.propEquality);
  v(t, e), this.prototype.propEquality = t;
}
function Up(e) {
  var t = Object.create(this.prototype.stateEquality);
  v(t, e), this.prototype.stateEquality = t;
}
function Ge(e, t) {
  typeof e == "function" ? e(t) : e && (e.current = t);
}
var bt = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.uid = Lt(), n;
  }
  return t.prototype.prepareHits = function() {
  }, t.prototype.queryHit = function(n, r, o, a) {
    return null;
  }, t.prototype.isValidSegDownEl = function(n) {
    return !this.props.eventDrag && !this.props.eventResize && !Le(n, ".fc-event-mirror");
  }, t.prototype.isValidDateDownEl = function(n) {
    return !Le(n, ".fc-event:not(.fc-bg-event)") && !Le(n, ".fc-more-link") && !Le(n, "a[data-navlink]") && !Le(n, ".fc-popover");
  }, t;
}(X);
function Ye(e) {
  return {
    id: Lt(),
    deps: e.deps || [],
    reducers: e.reducers || [],
    isLoadingFuncs: e.isLoadingFuncs || [],
    contextInit: [].concat(e.contextInit || []),
    eventRefiners: e.eventRefiners || {},
    eventDefMemberAdders: e.eventDefMemberAdders || [],
    eventSourceRefiners: e.eventSourceRefiners || {},
    isDraggableTransformers: e.isDraggableTransformers || [],
    eventDragMutationMassagers: e.eventDragMutationMassagers || [],
    eventDefMutationAppliers: e.eventDefMutationAppliers || [],
    dateSelectionTransformers: e.dateSelectionTransformers || [],
    datePointTransforms: e.datePointTransforms || [],
    dateSpanTransforms: e.dateSpanTransforms || [],
    views: e.views || {},
    viewPropsTransformers: e.viewPropsTransformers || [],
    isPropsValid: e.isPropsValid || null,
    externalDefTransforms: e.externalDefTransforms || [],
    viewContainerAppends: e.viewContainerAppends || [],
    eventDropTransformers: e.eventDropTransformers || [],
    componentInteractions: e.componentInteractions || [],
    calendarInteractions: e.calendarInteractions || [],
    themeClasses: e.themeClasses || {},
    eventSourceDefs: e.eventSourceDefs || [],
    cmdFormatter: e.cmdFormatter,
    recurringTypes: e.recurringTypes || [],
    namedTimeZonedImpl: e.namedTimeZonedImpl,
    initialView: e.initialView || "",
    elementDraggingImpl: e.elementDraggingImpl,
    optionChangeHandlers: e.optionChangeHandlers || {},
    scrollGridImpl: e.scrollGridImpl || null,
    contentTypeHandlers: e.contentTypeHandlers || {},
    listenerRefiners: e.listenerRefiners || {},
    optionRefiners: e.optionRefiners || {},
    propSetHandlers: e.propSetHandlers || {}
  };
}
function Lp(e, t) {
  var n = {}, r = {
    reducers: [],
    isLoadingFuncs: [],
    contextInit: [],
    eventRefiners: {},
    eventDefMemberAdders: [],
    eventSourceRefiners: {},
    isDraggableTransformers: [],
    eventDragMutationMassagers: [],
    eventDefMutationAppliers: [],
    dateSelectionTransformers: [],
    datePointTransforms: [],
    dateSpanTransforms: [],
    views: {},
    viewPropsTransformers: [],
    isPropsValid: null,
    externalDefTransforms: [],
    viewContainerAppends: [],
    eventDropTransformers: [],
    componentInteractions: [],
    calendarInteractions: [],
    themeClasses: {},
    eventSourceDefs: [],
    cmdFormatter: null,
    recurringTypes: [],
    namedTimeZonedImpl: null,
    initialView: "",
    elementDraggingImpl: null,
    optionChangeHandlers: {},
    scrollGridImpl: null,
    contentTypeHandlers: {},
    listenerRefiners: {},
    optionRefiners: {},
    propSetHandlers: {}
  };
  function o(a) {
    for (var i = 0, s = a; i < s.length; i++) {
      var l = s[i];
      n[l.id] || (n[l.id] = !0, o(l.deps), r = Wp(r, l));
    }
  }
  return e && o(e), o(t), r;
}
function Bp() {
  var e = [], t = [], n;
  return function(r, o) {
    return (!n || !Vt(r, e) || !Vt(o, t)) && (n = Lp(r, o)), e = r, t = o, n;
  };
}
function Wp(e, t) {
  return {
    reducers: e.reducers.concat(t.reducers),
    isLoadingFuncs: e.isLoadingFuncs.concat(t.isLoadingFuncs),
    contextInit: e.contextInit.concat(t.contextInit),
    eventRefiners: v(v({}, e.eventRefiners), t.eventRefiners),
    eventDefMemberAdders: e.eventDefMemberAdders.concat(t.eventDefMemberAdders),
    eventSourceRefiners: v(v({}, e.eventSourceRefiners), t.eventSourceRefiners),
    isDraggableTransformers: e.isDraggableTransformers.concat(t.isDraggableTransformers),
    eventDragMutationMassagers: e.eventDragMutationMassagers.concat(t.eventDragMutationMassagers),
    eventDefMutationAppliers: e.eventDefMutationAppliers.concat(t.eventDefMutationAppliers),
    dateSelectionTransformers: e.dateSelectionTransformers.concat(t.dateSelectionTransformers),
    datePointTransforms: e.datePointTransforms.concat(t.datePointTransforms),
    dateSpanTransforms: e.dateSpanTransforms.concat(t.dateSpanTransforms),
    views: v(v({}, e.views), t.views),
    viewPropsTransformers: e.viewPropsTransformers.concat(t.viewPropsTransformers),
    isPropsValid: t.isPropsValid || e.isPropsValid,
    externalDefTransforms: e.externalDefTransforms.concat(t.externalDefTransforms),
    viewContainerAppends: e.viewContainerAppends.concat(t.viewContainerAppends),
    eventDropTransformers: e.eventDropTransformers.concat(t.eventDropTransformers),
    calendarInteractions: e.calendarInteractions.concat(t.calendarInteractions),
    componentInteractions: e.componentInteractions.concat(t.componentInteractions),
    themeClasses: v(v({}, e.themeClasses), t.themeClasses),
    eventSourceDefs: e.eventSourceDefs.concat(t.eventSourceDefs),
    cmdFormatter: t.cmdFormatter || e.cmdFormatter,
    recurringTypes: e.recurringTypes.concat(t.recurringTypes),
    namedTimeZonedImpl: t.namedTimeZonedImpl || e.namedTimeZonedImpl,
    initialView: e.initialView || t.initialView,
    elementDraggingImpl: e.elementDraggingImpl || t.elementDraggingImpl,
    optionChangeHandlers: v(v({}, e.optionChangeHandlers), t.optionChangeHandlers),
    scrollGridImpl: t.scrollGridImpl || e.scrollGridImpl,
    contentTypeHandlers: v(v({}, e.contentTypeHandlers), t.contentTypeHandlers),
    listenerRefiners: v(v({}, e.listenerRefiners), t.listenerRefiners),
    optionRefiners: v(v({}, e.optionRefiners), t.optionRefiners),
    propSetHandlers: v(v({}, e.propSetHandlers), t.propSetHandlers)
  };
}
var ot = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t;
}(fn);
ot.prototype.classes = {
  root: "fc-theme-standard",
  tableCellShaded: "fc-cell-shaded",
  buttonGroup: "fc-button-group",
  button: "fc-button fc-button-primary",
  buttonActive: "fc-button-active"
};
ot.prototype.baseIconClass = "fc-icon";
ot.prototype.iconClasses = {
  close: "fc-icon-x",
  prev: "fc-icon-chevron-left",
  next: "fc-icon-chevron-right",
  prevYear: "fc-icon-chevrons-left",
  nextYear: "fc-icon-chevrons-right"
};
ot.prototype.rtlIconClasses = {
  prev: "fc-icon-chevron-right",
  next: "fc-icon-chevron-left",
  prevYear: "fc-icon-chevrons-right",
  nextYear: "fc-icon-chevrons-left"
};
ot.prototype.iconOverrideOption = "buttonIcons";
ot.prototype.iconOverrideCustomButtonOption = "icon";
ot.prototype.iconOverridePrefix = "fc-icon-";
function $p(e, t) {
  var n = {}, r;
  for (r in e)
    io(r, n, e, t);
  for (r in t)
    io(r, n, e, t);
  return n;
}
function io(e, t, n, r) {
  if (t[e])
    return t[e];
  var o = zp(e, t, n, r);
  return o && (t[e] = o), o;
}
function zp(e, t, n, r) {
  var o = n[e], a = r[e], i = function(c) {
    return o && o[c] !== null ? o[c] : a && a[c] !== null ? a[c] : null;
  }, s = i("component"), l = i("superType"), u = null;
  if (l) {
    if (l === e)
      throw new Error("Can't have a custom view type that references itself");
    u = io(l, t, n, r);
  }
  return !s && u && (s = u.component), s ? {
    type: e,
    component: s,
    defaults: v(v({}, u ? u.defaults : {}), o ? o.rawOptions : {}),
    overrides: v(v({}, u ? u.overrides : {}), a ? a.rawOptions : {})
  } : null;
}
var qe = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.rootElRef = Re(), n.handleRootEl = function(r) {
      Ge(n.rootElRef, r), n.props.elRef && Ge(n.props.elRef, r);
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = this.props, o = r.hookProps;
    return m(xo, { hookProps: o, didMount: r.didMount, willUnmount: r.willUnmount, elRef: this.handleRootEl }, function(a) {
      return m(vs, { hookProps: o, content: r.content, defaultContent: r.defaultContent, backupElRef: n.rootElRef }, function(i, s) {
        return r.children(a, gs(r.classNames, o), i, s);
      });
    });
  }, t;
}(X), hs = Wi(0);
function vs(e) {
  return m(hs.Consumer, null, function(t) {
    return m(Gp, v({ renderId: t }, e));
  });
}
var Gp = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.innerElRef = Re(), n;
  }
  return t.prototype.render = function() {
    return this.props.children(this.innerElRef, this.renderInnerContent());
  }, t.prototype.componentDidMount = function() {
    this.updateCustomContent();
  }, t.prototype.componentDidUpdate = function() {
    this.updateCustomContent();
  }, t.prototype.componentWillUnmount = function() {
    this.customContentInfo && this.customContentInfo.destroy && this.customContentInfo.destroy();
  }, t.prototype.renderInnerContent = function() {
    var n = this.customContentInfo, r = this.getInnerContent(), o = this.getContentMeta(r);
    return !n || n.contentKey !== o.contentKey ? (n && (n.destroy && n.destroy(), n = this.customContentInfo = null), o.contentKey && (n = this.customContentInfo = v({ contentKey: o.contentKey, contentVal: r[o.contentKey] }, o.buildLifecycleFuncs()))) : n && (n.contentVal = r[o.contentKey]), n ? [] : r;
  }, t.prototype.getInnerContent = function() {
    var n = this.props, r = Fa(n.content, n.hookProps);
    return r === void 0 && (r = Fa(n.defaultContent, n.hookProps)), r == null ? null : r;
  }, t.prototype.getContentMeta = function(n) {
    var r = this.context.pluginHooks.contentTypeHandlers, o = "", a = null;
    if (n) {
      for (var i in r)
        if (n[i] !== void 0) {
          o = i, a = r[i];
          break;
        }
    }
    return { contentKey: o, buildLifecycleFuncs: a };
  }, t.prototype.updateCustomContent = function() {
    this.customContentInfo && this.customContentInfo.render(
      this.innerElRef.current || this.props.backupElRef.current,
      this.customContentInfo.contentVal
    );
  }, t;
}(X), xo = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.handleRootEl = function(r) {
      n.rootEl = r, n.props.elRef && Ge(n.props.elRef, r);
    }, n;
  }
  return t.prototype.render = function() {
    return this.props.children(this.handleRootEl);
  }, t.prototype.componentDidMount = function() {
    var n = this.props.didMount;
    n && n(v(v({}, this.props.hookProps), { el: this.rootEl }));
  }, t.prototype.componentWillUnmount = function() {
    var n = this.props.willUnmount;
    n && n(v(v({}, this.props.hookProps), { el: this.rootEl }));
  }, t;
}(X);
function ms() {
  var e, t, n = [];
  return function(r, o) {
    return (!t || !ze(t, o) || r !== e) && (e = r, t = o, n = gs(r, o)), n;
  };
}
function gs(e, t) {
  return typeof e == "function" && (e = e(t)), to(e);
}
function Fa(e, t) {
  return typeof e == "function" ? e(t, m) : e;
}
var Kn = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.normalizeClassNames = ms(), n;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context, a = o.options, i = { view: o.viewApi }, s = this.normalizeClassNames(a.viewClassNames, i);
    return m(xo, { hookProps: i, didMount: a.viewDidMount, willUnmount: a.viewWillUnmount, elRef: r.elRef }, function(l) {
      return r.children(l, ["fc-" + r.viewSpec.type + "-view", "fc-view"].concat(s));
    });
  }, t;
}(X);
function Va(e) {
  return cn(e, jp);
}
function jp(e) {
  var t = typeof e == "function" ? { component: e } : e, n = t.component;
  return t.content && (n = Yp(t)), {
    superType: t.type,
    component: n,
    rawOptions: t
  };
}
function Yp(e) {
  return function(t) {
    return m(yt.Consumer, null, function(n) {
      return m(Kn, { viewSpec: n.viewSpec }, function(r, o) {
        var a = v(v({}, t), { nextDayThreshold: n.options.nextDayThreshold });
        return m(qe, { hookProps: a, classNames: e.classNames, content: e.content, didMount: e.didMount, willUnmount: e.willUnmount, elRef: r }, function(i, s, l, u) {
          return m("div", { className: o.concat(s).join(" "), ref: i }, u);
        });
      });
    });
  };
}
function qp(e, t, n, r) {
  var o = Va(e), a = Va(t.views), i = $p(o, a);
  return cn(i, function(s) {
    return Zp(s, a, t, n, r);
  });
}
function Zp(e, t, n, r, o) {
  var a = e.overrides.duration || e.defaults.duration || r.duration || n.duration, i = null, s = "", l = "", u = {};
  if (a && (i = Xp(a), i)) {
    var c = Qr(i);
    s = c.unit, c.value === 1 && (l = s, u = t[s] ? t[s].rawOptions : {});
  }
  var d = function(p) {
    var h = p.buttonText || {}, b = e.defaults.buttonTextKey;
    return b != null && h[b] != null ? h[b] : h[e.type] != null ? h[e.type] : h[l] != null ? h[l] : null;
  }, f = function(p) {
    var h = p.buttonHints || {}, b = e.defaults.buttonTextKey;
    return b != null && h[b] != null ? h[b] : h[e.type] != null ? h[e.type] : h[l] != null ? h[l] : null;
  };
  return {
    type: e.type,
    component: e.component,
    duration: i,
    durationUnit: s,
    singleUnit: l,
    optionDefaults: e.defaults,
    optionOverrides: v(v({}, u), e.overrides),
    buttonTextOverride: d(r) || d(n) || e.overrides.buttonText,
    buttonTextDefault: d(o) || e.defaults.buttonText || d(en) || e.type,
    buttonTitleOverride: f(r) || f(n) || e.overrides.buttonHint,
    buttonTitleDefault: f(o) || e.defaults.buttonHint || f(en)
  };
}
var Ua = {};
function Xp(e) {
  var t = JSON.stringify(e), n = Ua[t];
  return n === void 0 && (n = Y(e), Ua[t] = n), n;
}
var ys = function() {
  function e(t) {
    this.props = t, this.nowDate = dn(t.nowInput, t.dateEnv), this.initHiddenDays();
  }
  return e.prototype.buildPrev = function(t, n, r) {
    var o = this.props.dateEnv, a = o.subtract(
      o.startOf(n, t.currentRangeUnit),
      t.dateIncrement
    );
    return this.build(a, -1, r);
  }, e.prototype.buildNext = function(t, n, r) {
    var o = this.props.dateEnv, a = o.add(
      o.startOf(n, t.currentRangeUnit),
      t.dateIncrement
    );
    return this.build(a, 1, r);
  }, e.prototype.build = function(t, n, r) {
    r === void 0 && (r = !0);
    var o = this.props, a, i, s, l, u, c;
    return a = this.buildValidRange(), a = this.trimHiddenDays(a), r && (t = Lf(t, a)), i = this.buildCurrentRangeInfo(t, n), s = /^(year|month|week|day)$/.test(i.unit), l = this.buildRenderRange(this.trimHiddenDays(i.range), i.unit, s), l = this.trimHiddenDays(l), u = l, o.showNonCurrentDates || (u = Ut(u, i.range)), u = this.adjustActiveRange(u), u = Ut(u, a), c = Uf(i.range, a), {
      validRange: a,
      currentRange: i.range,
      currentRangeUnit: i.unit,
      isRangeAllDay: s,
      activeRange: u,
      renderRange: l,
      slotMinTime: o.slotMinTime,
      slotMaxTime: o.slotMaxTime,
      isValid: c,
      dateIncrement: this.buildDateIncrement(i.duration)
    };
  }, e.prototype.buildValidRange = function() {
    var t = this.props.validRangeInput, n = typeof t == "function" ? t.call(this.props.calendarApi, this.nowDate) : t;
    return this.refineRange(n) || { start: null, end: null };
  }, e.prototype.buildCurrentRangeInfo = function(t, n) {
    var r = this.props, o = null, a = null, i = null, s;
    return r.duration ? (o = r.duration, a = r.durationUnit, i = this.buildRangeFromDuration(t, n, o, a)) : (s = this.props.dayCount) ? (a = "day", i = this.buildRangeFromDayCount(t, n, s)) : (i = this.buildCustomVisibleRange(t)) ? a = r.dateEnv.greatestWholeUnit(i.start, i.end).unit : (o = this.getFallbackDuration(), a = Qr(o).unit, i = this.buildRangeFromDuration(t, n, o, a)), { duration: o, unit: a, range: i };
  }, e.prototype.getFallbackDuration = function() {
    return Y({ day: 1 });
  }, e.prototype.adjustActiveRange = function(t) {
    var n = this.props, r = n.dateEnv, o = n.usesMinMaxTime, a = n.slotMinTime, i = n.slotMaxTime, s = t.start, l = t.end;
    return o && (It(a) < 0 && (s = Q(s), s = r.add(s, a)), It(i) > 1 && (l = Q(l), l = ue(l, -1), l = r.add(l, i))), { start: s, end: l };
  }, e.prototype.buildRangeFromDuration = function(t, n, r, o) {
    var a = this.props, i = a.dateEnv, s = a.dateAlignment, l, u, c;
    if (!s) {
      var d = this.props.dateIncrement;
      d && on(d) < on(r) ? s = Qr(d).unit : s = o;
    }
    It(r) <= 1 && this.isHiddenDay(l) && (l = this.skipHiddenDays(l, n), l = Q(l));
    function f() {
      l = i.startOf(t, s), u = i.add(l, r), c = { start: l, end: u };
    }
    return f(), this.trimHiddenDays(c) || (t = this.skipHiddenDays(t, n), f()), c;
  }, e.prototype.buildRangeFromDayCount = function(t, n, r) {
    var o = this.props, a = o.dateEnv, i = o.dateAlignment, s = 0, l = t, u;
    i && (l = a.startOf(l, i)), l = Q(l), l = this.skipHiddenDays(l, n), u = l;
    do
      u = ue(u, 1), this.isHiddenDay(u) || (s += 1);
    while (s < r);
    return { start: l, end: u };
  }, e.prototype.buildCustomVisibleRange = function(t) {
    var n = this.props, r = n.visibleRangeInput, o = typeof r == "function" ? r.call(n.calendarApi, n.dateEnv.toDate(t)) : r, a = this.refineRange(o);
    return a && (a.start == null || a.end == null) ? null : a;
  }, e.prototype.buildRenderRange = function(t, n, r) {
    return t;
  }, e.prototype.buildDateIncrement = function(t) {
    var n = this.props.dateIncrement, r;
    return n || ((r = this.props.dateAlignment) ? Y(1, r) : t || Y({ days: 1 }));
  }, e.prototype.refineRange = function(t) {
    if (t) {
      var n = Ff(t, this.props.dateEnv);
      return n && (n = To(n)), n;
    }
    return null;
  }, e.prototype.initHiddenDays = function() {
    var t = this.props.hiddenDays || [], n = [], r = 0, o;
    for (this.props.weekends === !1 && t.push(0, 6), o = 0; o < 7; o += 1)
      (n[o] = t.indexOf(o) !== -1) || (r += 1);
    if (!r)
      throw new Error("invalid hiddenDays");
    this.isHiddenDayHash = n;
  }, e.prototype.trimHiddenDays = function(t) {
    var n = t.start, r = t.end;
    return n && (n = this.skipHiddenDays(n)), r && (r = this.skipHiddenDays(r, -1, !0)), n == null || r == null || n < r ? { start: n, end: r } : null;
  }, e.prototype.isHiddenDay = function(t) {
    return t instanceof Date && (t = t.getUTCDay()), this.isHiddenDayHash[t];
  }, e.prototype.skipHiddenDays = function(t, n, r) {
    for (n === void 0 && (n = 1), r === void 0 && (r = !1); this.isHiddenDayHash[(t.getUTCDay() + (r ? n : 0) + 7) % 7]; )
      t = ue(t, n);
    return t;
  }, e;
}();
function Kp(e, t) {
  switch (t.type) {
    case "CHANGE_VIEW_TYPE":
      e = t.viewType;
  }
  return e;
}
function Jp(e, t) {
  var n;
  switch (t.type) {
    case "SET_OPTION":
      return v(v({}, e), (n = {}, n[t.optionName] = t.rawOptionValue, n));
    default:
      return e;
  }
}
function Qp(e, t, n, r) {
  var o;
  switch (t.type) {
    case "CHANGE_VIEW_TYPE":
      return r.build(t.dateMarker || n);
    case "CHANGE_DATE":
      return r.build(t.dateMarker);
    case "PREV":
      if (o = r.buildPrev(e, n), o.isValid)
        return o;
      break;
    case "NEXT":
      if (o = r.buildNext(e, n), o.isValid)
        return o;
      break;
  }
  return e;
}
function eh(e, t, n) {
  var r = t ? t.activeRange : null;
  return Es({}, sh(e, n), r, n);
}
function th(e, t, n, r) {
  var o = n ? n.activeRange : null;
  switch (t.type) {
    case "ADD_EVENT_SOURCES":
      return Es(e, t.sources, o, r);
    case "REMOVE_EVENT_SOURCE":
      return rh(e, t.sourceId);
    case "PREV":
    case "NEXT":
    case "CHANGE_DATE":
    case "CHANGE_VIEW_TYPE":
      return n ? _s(e, o, r) : e;
    case "FETCH_EVENT_SOURCES":
      return Mo(e, t.sourceIds ? qi(t.sourceIds) : Cs(e, r), o, t.isRefetch || !1, r);
    case "RECEIVE_EVENTS":
    case "RECEIVE_EVENT_ERROR":
      return ih(e, t.sourceId, t.fetchId, t.fetchRange);
    case "REMOVE_ALL_EVENT_SOURCES":
      return {};
    default:
      return e;
  }
}
function nh(e, t, n) {
  var r = t ? t.activeRange : null;
  return Mo(e, Cs(e, n), r, !0, n);
}
function bs(e) {
  for (var t in e)
    if (e[t].isFetching)
      return !0;
  return !1;
}
function Es(e, t, n, r) {
  for (var o = {}, a = 0, i = t; a < i.length; a++) {
    var s = i[a];
    o[s.sourceId] = s;
  }
  return n && (o = _s(o, n, r)), v(v({}, e), o);
}
function rh(e, t) {
  return Ft(e, function(n) {
    return n.sourceId !== t;
  });
}
function _s(e, t, n) {
  return Mo(e, Ft(e, function(r) {
    return oh(r, t, n);
  }), t, !1, n);
}
function oh(e, t, n) {
  return Ds(e, n) ? !n.options.lazyFetching || !e.fetchRange || e.isFetching || t.start < e.fetchRange.start || t.end > e.fetchRange.end : !e.latestFetchId;
}
function Mo(e, t, n, r, o) {
  var a = {};
  for (var i in e) {
    var s = e[i];
    t[i] ? a[i] = ah(s, n, r, o) : a[i] = s;
  }
  return a;
}
function ah(e, t, n, r) {
  var o = r.options, a = r.calendarApi, i = r.pluginHooks.eventSourceDefs[e.sourceDefId], s = Lt();
  return i.fetch({
    eventSource: e,
    range: t,
    isRefetch: n,
    context: r
  }, function(l) {
    var u = l.rawEvents;
    o.eventSourceSuccess && (u = o.eventSourceSuccess.call(a, u, l.xhr) || u), e.success && (u = e.success.call(a, u, l.xhr) || u), r.dispatch({
      type: "RECEIVE_EVENTS",
      sourceId: e.sourceId,
      fetchId: s,
      fetchRange: t,
      rawEvents: u
    });
  }, function(l) {
    console.warn(l.message, l), o.eventSourceFailure && o.eventSourceFailure.call(a, l), e.failure && e.failure(l), r.dispatch({
      type: "RECEIVE_EVENT_ERROR",
      sourceId: e.sourceId,
      fetchId: s,
      fetchRange: t,
      error: l
    });
  }), v(v({}, e), { isFetching: !0, latestFetchId: s });
}
function ih(e, t, n, r) {
  var o, a = e[t];
  return a && n === a.latestFetchId ? v(v({}, e), (o = {}, o[t] = v(v({}, a), { isFetching: !1, fetchRange: r }), o)) : e;
}
function Cs(e, t) {
  return Ft(e, function(n) {
    return Ds(n, t);
  });
}
function sh(e, t) {
  var n = as(t), r = [].concat(e.eventSources || []), o = [];
  e.initialEvents && r.unshift(e.initialEvents), e.events && r.unshift(e.events);
  for (var a = 0, i = r; a < i.length; a++) {
    var s = i[a], l = os(s, t, n);
    l && o.push(l);
  }
  return o;
}
function Ds(e, t) {
  var n = t.pluginHooks.eventSourceDefs;
  return !n[e.sourceDefId].ignoreRange;
}
function lh(e, t, n, r, o) {
  switch (t.type) {
    case "RECEIVE_EVENTS":
      return uh(e, n[t.sourceId], t.fetchId, t.fetchRange, t.rawEvents, o);
    case "ADD_EVENTS":
      return dh(
        e,
        t.eventStore,
        r ? r.activeRange : null,
        o
      );
    case "RESET_EVENTS":
      return t.eventStore;
    case "MERGE_EVENTS":
      return Do(e, t.eventStore);
    case "PREV":
    case "NEXT":
    case "CHANGE_DATE":
    case "CHANGE_VIEW_TYPE":
      return r ? ur(e, r.activeRange, o) : e;
    case "REMOVE_EVENTS":
      return kf(e, t.eventStore);
    case "REMOVE_EVENT_SOURCE":
      return Ss(e, t.sourceId);
    case "REMOVE_ALL_EVENT_SOURCES":
      return So(e, function(a) {
        return !a.sourceId;
      });
    case "REMOVE_ALL_EVENTS":
      return mt();
    default:
      return e;
  }
}
function uh(e, t, n, r, o, a) {
  if (t && n === t.latestFetchId) {
    var i = qn(ch(o, t, a), t, a);
    return r && (i = ur(i, r, a)), Do(Ss(e, t.sourceId), i);
  }
  return e;
}
function ch(e, t, n) {
  var r = n.options.eventDataTransform, o = t ? t.eventDataTransform : null;
  return o && (e = La(e, o)), r && (e = La(e, r)), e;
}
function La(e, t) {
  var n;
  if (!t)
    n = e;
  else {
    n = [];
    for (var r = 0, o = e; r < o.length; r++) {
      var a = o[r], i = t(a);
      i ? n.push(i) : i == null && n.push(a);
    }
  }
  return n;
}
function dh(e, t, n, r) {
  return n && (t = ur(t, n, r)), Do(e, t);
}
function fh(e, t, n) {
  var r = e.defs, o = cn(e.instances, function(a) {
    var i = r[a.defId];
    return i.allDay || i.recurringDef ? a : v(v({}, a), { range: {
      start: n.createMarker(t.toDate(a.range.start, a.forcedStartTzo)),
      end: n.createMarker(t.toDate(a.range.end, a.forcedEndTzo))
    }, forcedStartTzo: n.canComputeOffset ? null : a.forcedStartTzo, forcedEndTzo: n.canComputeOffset ? null : a.forcedEndTzo });
  });
  return { defs: r, instances: o };
}
function Ss(e, t) {
  return So(e, function(n) {
    return n.sourceId !== t;
  });
}
function ph(e, t) {
  switch (t.type) {
    case "UNSELECT_DATES":
      return null;
    case "SELECT_DATES":
      return t.selection;
    default:
      return e;
  }
}
function hh(e, t) {
  switch (t.type) {
    case "UNSELECT_EVENT":
      return "";
    case "SELECT_EVENT":
      return t.eventInstanceId;
    default:
      return e;
  }
}
function vh(e, t) {
  var n;
  switch (t.type) {
    case "UNSET_EVENT_DRAG":
      return null;
    case "SET_EVENT_DRAG":
      return n = t.state, {
        affectedEvents: n.affectedEvents,
        mutatedEvents: n.mutatedEvents,
        isEvent: n.isEvent
      };
    default:
      return e;
  }
}
function mh(e, t) {
  var n;
  switch (t.type) {
    case "UNSET_EVENT_RESIZE":
      return null;
    case "SET_EVENT_RESIZE":
      return n = t.state, {
        affectedEvents: n.affectedEvents,
        mutatedEvents: n.mutatedEvents,
        isEvent: n.isEvent
      };
    default:
      return e;
  }
}
function gh(e, t, n, r, o) {
  var a = e.headerToolbar ? Ba(e.headerToolbar, e, t, n, r, o) : null, i = e.footerToolbar ? Ba(e.footerToolbar, e, t, n, r, o) : null;
  return { header: a, footer: i };
}
function Ba(e, t, n, r, o, a) {
  var i = {}, s = [], l = !1;
  for (var u in e) {
    var c = e[u], d = yh(c, t, n, r, o, a);
    i[u] = d.widgets, s.push.apply(s, d.viewsWithButtons), l = l || d.hasTitle;
  }
  return { sectionWidgets: i, viewsWithButtons: s, hasTitle: l };
}
function yh(e, t, n, r, o, a) {
  var i = t.direction === "rtl", s = t.customButtons || {}, l = n.buttonText || {}, u = t.buttonText || {}, c = n.buttonHints || {}, d = t.buttonHints || {}, f = e ? e.split(" ") : [], p = [], h = !1, b = f.map(function(E) {
    return E.split(",").map(function(y) {
      if (y === "title")
        return h = !0, { buttonName: y };
      var D, k, T, U, O, R;
      if (D = s[y])
        T = function(I) {
          D.click && D.click.call(I.target, I, I.target);
        }, (U = r.getCustomButtonIconClass(D)) || (U = r.getIconClass(y, i)) || (O = D.text), R = D.hint || D.text;
      else if (k = o[y]) {
        p.push(y), T = function() {
          a.changeView(y);
        }, (O = k.buttonTextOverride) || (U = r.getIconClass(y, i)) || (O = k.buttonTextDefault);
        var A = k.buttonTextOverride || k.buttonTextDefault;
        R = Qt(
          k.buttonTitleOverride || k.buttonTitleDefault || t.viewHint,
          [A, y],
          A
        );
      } else if (a[y])
        if (T = function() {
          a[y]();
        }, (O = l[y]) || (U = r.getIconClass(y, i)) || (O = u[y]), y === "prevYear" || y === "nextYear") {
          var M = y === "prevYear" ? "prev" : "next";
          R = Qt(c[M] || d[M], [
            u.year || "year",
            "year"
          ], u[y]);
        } else
          R = function(I) {
            return Qt(c[y] || d[y], [
              u[I] || I,
              I
            ], u[y]);
          };
      return { buttonName: y, buttonClick: T, buttonIcon: U, buttonText: O, buttonHint: R };
    });
  });
  return { widgets: b, viewsWithButtons: p, hasTitle: h };
}
var bh = {
  ignoreRange: !0,
  parseMeta: function(e) {
    return Array.isArray(e.events) ? e.events : null;
  },
  fetch: function(e, t) {
    t({
      rawEvents: e.eventSource.meta
    });
  }
}, Eh = Ye({
  eventSourceDefs: [bh]
}), _h = {
  parseMeta: function(e) {
    return typeof e.events == "function" ? e.events : null;
  },
  fetch: function(e, t, n) {
    var r = e.context.dateEnv, o = e.eventSource.meta;
    Pp(o.bind(null, ns(e.range, r)), function(a) {
      t({ rawEvents: a });
    }, n);
  }
}, Ch = Ye({
  eventSourceDefs: [_h]
});
function ws(e, t, n, r, o) {
  e = e.toUpperCase();
  var a = null;
  e === "GET" ? t = Dh(t, n) : a = Ts(n);
  var i = new XMLHttpRequest();
  i.open(e, t, !0), e !== "GET" && i.setRequestHeader("Content-Type", "application/x-www-form-urlencoded"), i.onload = function() {
    if (i.status >= 200 && i.status < 400) {
      var s = !1, l = void 0;
      try {
        l = JSON.parse(i.responseText), s = !0;
      } catch {
      }
      s ? r(l, i) : o("Failure parsing JSON", i);
    } else
      o("Request failed", i);
  }, i.onerror = function() {
    o("Request failed", i);
  }, i.send(a);
}
function Dh(e, t) {
  return e + (e.indexOf("?") === -1 ? "?" : "&") + Ts(t);
}
function Ts(e) {
  var t = [];
  for (var n in e)
    t.push(encodeURIComponent(n) + "=" + encodeURIComponent(e[n]));
  return t.join("&");
}
var Sh = {
  method: String,
  extraParams: g,
  startParam: String,
  endParam: String,
  timeZoneParam: String
}, wh = {
  parseMeta: function(e) {
    return e.url && (e.format === "json" || !e.format) ? {
      url: e.url,
      format: "json",
      method: (e.method || "GET").toUpperCase(),
      extraParams: e.extraParams,
      startParam: e.startParam,
      endParam: e.endParam,
      timeZoneParam: e.timeZoneParam
    } : null;
  },
  fetch: function(e, t, n) {
    var r = e.eventSource.meta, o = Rh(r, e.range, e.context);
    ws(r.method, r.url, o, function(a, i) {
      t({ rawEvents: a, xhr: i });
    }, function(a, i) {
      n({ message: a, xhr: i });
    });
  }
}, Th = Ye({
  eventSourceRefiners: Sh,
  eventSourceDefs: [wh]
});
function Rh(e, t, n) {
  var r = n.dateEnv, o = n.options, a, i, s, l, u = {};
  return a = e.startParam, a == null && (a = o.startParam), i = e.endParam, i == null && (i = o.endParam), s = e.timeZoneParam, s == null && (s = o.timeZoneParam), typeof e.extraParams == "function" ? l = e.extraParams() : l = e.extraParams || {}, v(u, l), u[a] = r.formatIso(t.start), u[i] = r.formatIso(t.end), r.timeZone !== "local" && (u[s] = r.timeZone), u;
}
var kh = {
  daysOfWeek: g,
  startTime: Y,
  endTime: Y,
  duration: Y,
  startRecur: g,
  endRecur: g
}, Ih = {
  parse: function(e, t) {
    if (e.daysOfWeek || e.startTime || e.endTime || e.startRecur || e.endRecur) {
      var n = {
        daysOfWeek: e.daysOfWeek || null,
        startTime: e.startTime || null,
        endTime: e.endTime || null,
        startRecur: e.startRecur ? t.createMarker(e.startRecur) : null,
        endRecur: e.endRecur ? t.createMarker(e.endRecur) : null
      }, r = void 0;
      return e.duration && (r = e.duration), !r && e.startTime && e.endTime && (r = af(e.endTime, e.startTime)), {
        allDayGuess: Boolean(!e.startTime && !e.endTime),
        duration: r,
        typeData: n
      };
    }
    return null;
  },
  expand: function(e, t, n) {
    var r = Ut(t, { start: e.startRecur, end: e.endRecur });
    return r ? Mh(e.daysOfWeek, e.startTime, r, n) : [];
  }
}, xh = Ye({
  recurringTypes: [Ih],
  eventRefiners: kh
});
function Mh(e, t, n, r) {
  for (var o = e ? qi(e) : null, a = Q(n.start), i = n.end, s = []; a < i; ) {
    var l = void 0;
    (!o || o[a.getUTCDay()]) && (t ? l = r.add(a, t) : l = a, s.push(l)), a = ue(a, 1);
  }
  return s;
}
var Ph = Ye({
  optionChangeHandlers: {
    events: function(e, t) {
      Wa([e], t);
    },
    eventSources: Wa
  }
});
function Wa(e, t) {
  for (var n = Eo(t.getCurrentData().eventSources), r = [], o = 0, a = e; o < a.length; o++) {
    for (var i = a[o], s = !1, l = 0; l < n.length; l += 1)
      if (n[l]._raw === i) {
        n.splice(l, 1), s = !0;
        break;
      }
    s || r.push(i);
  }
  for (var u = 0, c = n; u < c.length; u++) {
    var d = c[u];
    t.dispatch({
      type: "REMOVE_EVENT_SOURCE",
      sourceId: d.sourceId
    });
  }
  for (var f = 0, p = r; f < p.length; f++) {
    var h = p[f];
    t.calendarApi.addEventSource(h);
  }
}
function Oh(e, t) {
  t.emitter.trigger("datesSet", v(v({}, ns(e.activeRange, t.dateEnv)), { view: t.viewApi }));
}
function Ah(e, t) {
  var n = t.emitter;
  n.hasHandlers("eventsSet") && n.trigger("eventsSet", ko(e, t));
}
var Nh = [
  Eh,
  Ch,
  Th,
  xh,
  Ph,
  Ye({
    isLoadingFuncs: [
      function(e) {
        return bs(e.eventSources);
      }
    ],
    contentTypeHandlers: {
      html: Hh,
      domNodes: Fh
    },
    propSetHandlers: {
      dateProfile: Oh,
      eventStore: Ah
    }
  })
];
function Hh() {
  var e = null, t = "";
  function n(o, a) {
    (o !== e || a !== t) && (o.innerHTML = a), e = o, t = a;
  }
  function r() {
    e.innerHTML = "", e = null, t = "";
  }
  return { render: n, destroy: r };
}
function Fh() {
  var e = null, t = [];
  function n(o, a) {
    var i = Array.prototype.slice.call(a);
    if (o !== e || !Vt(t, i)) {
      for (var s = 0, l = i; s < l.length; s++) {
        var u = l[s];
        o.appendChild(u);
      }
      r();
    }
    e = o, t = i;
  }
  function r() {
    t.forEach(Rd), t = [], e = null;
  }
  return { render: n, destroy: r };
}
var Po = function() {
  function e(t) {
    this.drainedOption = t, this.isRunning = !1, this.isDirty = !1, this.pauseDepths = {}, this.timeoutId = 0;
  }
  return e.prototype.request = function(t) {
    this.isDirty = !0, this.isPaused() || (this.clearTimeout(), t == null ? this.tryDrain() : this.timeoutId = setTimeout(
      this.tryDrain.bind(this),
      t
    ));
  }, e.prototype.pause = function(t) {
    t === void 0 && (t = "");
    var n = this.pauseDepths;
    n[t] = (n[t] || 0) + 1, this.clearTimeout();
  }, e.prototype.resume = function(t, n) {
    t === void 0 && (t = "");
    var r = this.pauseDepths;
    if (t in r) {
      if (n)
        delete r[t];
      else {
        r[t] -= 1;
        var o = r[t];
        o <= 0 && delete r[t];
      }
      this.tryDrain();
    }
  }, e.prototype.isPaused = function() {
    return Object.keys(this.pauseDepths).length;
  }, e.prototype.tryDrain = function() {
    if (!this.isRunning && !this.isPaused()) {
      for (this.isRunning = !0; this.isDirty; )
        this.isDirty = !1, this.drained();
      this.isRunning = !1;
    }
  }, e.prototype.clear = function() {
    this.clearTimeout(), this.isDirty = !1, this.pauseDepths = {};
  }, e.prototype.clearTimeout = function() {
    this.timeoutId && (clearTimeout(this.timeoutId), this.timeoutId = 0);
  }, e.prototype.drained = function() {
    this.drainedOption && this.drainedOption();
  }, e;
}(), Vh = function() {
  function e(t, n) {
    this.runTaskOption = t, this.drainedOption = n, this.queue = [], this.delayedRunner = new Po(this.drain.bind(this));
  }
  return e.prototype.request = function(t, n) {
    this.queue.push(t), this.delayedRunner.request(n);
  }, e.prototype.pause = function(t) {
    this.delayedRunner.pause(t);
  }, e.prototype.resume = function(t, n) {
    this.delayedRunner.resume(t, n);
  }, e.prototype.drain = function() {
    for (var t = this.queue; t.length; ) {
      for (var n = [], r = void 0; r = t.shift(); )
        this.runTask(r), n.push(r);
      this.drained(n);
    }
  }, e.prototype.runTask = function(t) {
    this.runTaskOption && this.runTaskOption(t);
  }, e.prototype.drained = function(t) {
    this.drainedOption && this.drainedOption(t);
  }, e;
}();
function Uh(e, t, n) {
  var r;
  return /^(year|month)$/.test(e.currentRangeUnit) ? r = e.currentRange : r = e.activeRange, n.formatRange(r.start, r.end, le(t.titleFormat || Lh(e)), {
    isEndExclusive: e.isRangeAllDay,
    defaultSeparator: t.titleRangeSeparator
  });
}
function Lh(e) {
  var t = e.currentRangeUnit;
  if (t === "year")
    return { year: "numeric" };
  if (t === "month")
    return { year: "numeric", month: "long" };
  var n = zn(e.currentRange.start, e.currentRange.end);
  return n !== null && n > 1 ? { year: "numeric", month: "short", day: "numeric" } : { year: "numeric", month: "long", day: "numeric" };
}
var Rs = function() {
  function e(t) {
    var n = this;
    this.computeOptionsData = W(this._computeOptionsData), this.computeCurrentViewData = W(this._computeCurrentViewData), this.organizeRawLocales = W(gp), this.buildLocale = W(cs), this.buildPluginHooks = Bp(), this.buildDateEnv = W(Bh), this.buildTheme = W(Wh), this.parseToolbars = W(gh), this.buildViewSpecs = W(qp), this.buildDateProfileGenerator = Pn($h), this.buildViewApi = W(zh), this.buildViewUiProps = Pn(Yh), this.buildEventUiBySource = W(Gh, ze), this.buildEventUiBases = W(jh), this.parseContextBusinessHours = Pn(qh), this.buildTitle = W(Uh), this.emitter = new Op(), this.actionRunner = new Vh(this._handleAction.bind(this), this.updateData.bind(this)), this.currentCalendarOptionsInput = {}, this.currentCalendarOptionsRefined = {}, this.currentViewOptionsInput = {}, this.currentViewOptionsRefined = {}, this.currentCalendarOptionsRefiners = {}, this.getCurrentData = function() {
      return n.data;
    }, this.dispatch = function(k) {
      n.actionRunner.request(k);
    }, this.props = t, this.actionRunner.pause();
    var r = {}, o = this.computeOptionsData(t.optionOverrides, r, t.calendarApi), a = o.calendarOptions.initialView || o.pluginHooks.initialView, i = this.computeCurrentViewData(a, o, t.optionOverrides, r);
    t.calendarApi.currentDataManager = this, this.emitter.setThisContext(t.calendarApi), this.emitter.setOptions(i.options);
    var s = lp(o.calendarOptions, o.dateEnv), l = i.dateProfileGenerator.build(s);
    ft(l.activeRange, s) || (s = l.currentRange.start);
    for (var u = {
      dateEnv: o.dateEnv,
      options: o.calendarOptions,
      pluginHooks: o.pluginHooks,
      calendarApi: t.calendarApi,
      dispatch: this.dispatch,
      emitter: this.emitter,
      getCurrentData: this.getCurrentData
    }, c = 0, d = o.pluginHooks.contextInit; c < d.length; c++) {
      var f = d[c];
      f(u);
    }
    for (var p = eh(o.calendarOptions, l, u), h = {
      dynamicOptionOverrides: r,
      currentViewType: a,
      currentDate: s,
      dateProfile: l,
      businessHours: this.parseContextBusinessHours(u),
      eventSources: p,
      eventUiBases: {},
      eventStore: mt(),
      renderableEventStore: mt(),
      dateSelection: null,
      eventSelection: "",
      eventDrag: null,
      eventResize: null,
      selectionConfig: this.buildViewUiProps(u).selectionConfig
    }, b = v(v({}, u), h), E = 0, y = o.pluginHooks.reducers; E < y.length; E++) {
      var D = y[E];
      v(h, D(null, null, b));
    }
    Fr(h, u) && this.emitter.trigger("loading", !0), this.state = h, this.updateData(), this.actionRunner.resume();
  }
  return e.prototype.resetOptions = function(t, n) {
    var r = this.props;
    r.optionOverrides = n ? v(v({}, r.optionOverrides), t) : t, this.actionRunner.request({
      type: "NOTHING"
    });
  }, e.prototype._handleAction = function(t) {
    var n = this, r = n.props, o = n.state, a = n.emitter, i = Jp(o.dynamicOptionOverrides, t), s = this.computeOptionsData(r.optionOverrides, i, r.calendarApi), l = Kp(o.currentViewType, t), u = this.computeCurrentViewData(l, s, r.optionOverrides, i);
    r.calendarApi.currentDataManager = this, a.setThisContext(r.calendarApi), a.setOptions(u.options);
    var c = {
      dateEnv: s.dateEnv,
      options: s.calendarOptions,
      pluginHooks: s.pluginHooks,
      calendarApi: r.calendarApi,
      dispatch: this.dispatch,
      emitter: a,
      getCurrentData: this.getCurrentData
    }, d = o.currentDate, f = o.dateProfile;
    this.data && this.data.dateProfileGenerator !== u.dateProfileGenerator && (f = u.dateProfileGenerator.build(d)), d = sp(d, t), f = Qp(f, t, d, u.dateProfileGenerator), (t.type === "PREV" || t.type === "NEXT" || !ft(f.currentRange, d)) && (d = f.currentRange.start);
    for (var p = th(o.eventSources, t, f, c), h = lh(o.eventStore, t, p, f, c), b = bs(p), E = b && !u.options.progressiveEventRendering && o.renderableEventStore || h, y = this.buildViewUiProps(c), D = y.eventUiSingleBase, k = y.selectionConfig, T = this.buildEventUiBySource(p), U = this.buildEventUiBases(E.defs, D, T), O = {
      dynamicOptionOverrides: i,
      currentViewType: l,
      currentDate: d,
      dateProfile: f,
      eventSources: p,
      eventStore: h,
      renderableEventStore: E,
      selectionConfig: k,
      eventUiBases: U,
      businessHours: this.parseContextBusinessHours(c),
      dateSelection: ph(o.dateSelection, t),
      eventSelection: hh(o.eventSelection, t),
      eventDrag: vh(o.eventDrag, t),
      eventResize: mh(o.eventResize, t)
    }, R = v(v({}, c), O), A = 0, M = s.pluginHooks.reducers; A < M.length; A++) {
      var I = M[A];
      v(O, I(o, t, R));
    }
    var N = Fr(o, c), j = Fr(O, c);
    !N && j ? a.trigger("loading", !0) : N && !j && a.trigger("loading", !1), this.state = O, r.onAction && r.onAction(t);
  }, e.prototype.updateData = function() {
    var t = this, n = t.props, r = t.state, o = this.data, a = this.computeOptionsData(n.optionOverrides, r.dynamicOptionOverrides, n.calendarApi), i = this.computeCurrentViewData(r.currentViewType, a, n.optionOverrides, r.dynamicOptionOverrides), s = this.data = v(v(v({ viewTitle: this.buildTitle(r.dateProfile, i.options, a.dateEnv), calendarApi: n.calendarApi, dispatch: this.dispatch, emitter: this.emitter, getCurrentData: this.getCurrentData }, a), i), r), l = a.pluginHooks.optionChangeHandlers, u = o && o.calendarOptions, c = a.calendarOptions;
    if (u && u !== c) {
      u.timeZone !== c.timeZone && (r.eventSources = s.eventSources = nh(s.eventSources, r.dateProfile, s), r.eventStore = s.eventStore = fh(s.eventStore, o.dateEnv, s.dateEnv));
      for (var d in l)
        u[d] !== c[d] && l[d](c[d], s);
    }
    n.onData && n.onData(s);
  }, e.prototype._computeOptionsData = function(t, n, r) {
    var o = this.processRawCalendarOptions(t, n), a = o.refinedOptions, i = o.pluginHooks, s = o.localeDefaults, l = o.availableLocaleData, u = o.extra;
    $a(u);
    var c = this.buildDateEnv(a.timeZone, a.locale, a.weekNumberCalculation, a.firstDay, a.weekText, i, l, a.defaultRangeSeparator), d = this.buildViewSpecs(i.views, t, n, s), f = this.buildTheme(a, i), p = this.parseToolbars(a, t, f, d, r);
    return {
      calendarOptions: a,
      pluginHooks: i,
      dateEnv: c,
      viewSpecs: d,
      theme: f,
      toolbarConfig: p,
      localeDefaults: s,
      availableRawLocales: l.map
    };
  }, e.prototype.processRawCalendarOptions = function(t, n) {
    var r = Ar([
      en,
      t,
      n
    ]), o = r.locales, a = r.locale, i = this.organizeRawLocales(o), s = i.map, l = this.buildLocale(a || i.defaultCode, s).options, u = this.buildPluginHooks(t.plugins || [], Nh), c = this.currentCalendarOptionsRefiners = v(v(v(v(v({}, Ma), Pa), Oa), u.listenerRefiners), u.optionRefiners), d = {}, f = Ar([
      en,
      l,
      t,
      n
    ]), p = {}, h = this.currentCalendarOptionsInput, b = this.currentCalendarOptionsRefined, E = !1;
    for (var y in f)
      y !== "plugins" && (f[y] === h[y] || ct[y] && y in h && ct[y](h[y], f[y]) ? p[y] = b[y] : c[y] ? (p[y] = c[y](f[y]), E = !0) : d[y] = h[y]);
    return E && (this.currentCalendarOptionsInput = f, this.currentCalendarOptionsRefined = p), {
      rawOptions: this.currentCalendarOptionsInput,
      refinedOptions: this.currentCalendarOptionsRefined,
      pluginHooks: u,
      availableLocaleData: i,
      localeDefaults: l,
      extra: d
    };
  }, e.prototype._computeCurrentViewData = function(t, n, r, o) {
    var a = n.viewSpecs[t];
    if (!a)
      throw new Error('viewType "' + t + `" is not available. Please make sure you've loaded all neccessary plugins`);
    var i = this.processRawViewOptions(a, n.pluginHooks, n.localeDefaults, r, o), s = i.refinedOptions, l = i.extra;
    $a(l);
    var u = this.buildDateProfileGenerator({
      dateProfileGeneratorClass: a.optionDefaults.dateProfileGeneratorClass,
      duration: a.duration,
      durationUnit: a.durationUnit,
      usesMinMaxTime: a.optionDefaults.usesMinMaxTime,
      dateEnv: n.dateEnv,
      calendarApi: this.props.calendarApi,
      slotMinTime: s.slotMinTime,
      slotMaxTime: s.slotMaxTime,
      showNonCurrentDates: s.showNonCurrentDates,
      dayCount: s.dayCount,
      dateAlignment: s.dateAlignment,
      dateIncrement: s.dateIncrement,
      hiddenDays: s.hiddenDays,
      weekends: s.weekends,
      nowInput: s.now,
      validRangeInput: s.validRange,
      visibleRangeInput: s.visibleRange,
      monthMode: s.monthMode,
      fixedWeekCount: s.fixedWeekCount
    }), c = this.buildViewApi(t, this.getCurrentData, n.dateEnv);
    return { viewSpec: a, options: s, dateProfileGenerator: u, viewApi: c };
  }, e.prototype.processRawViewOptions = function(t, n, r, o, a) {
    var i = Ar([
      en,
      t.optionDefaults,
      r,
      o,
      t.optionOverrides,
      a
    ]), s = v(v(v(v(v(v({}, Ma), Pa), Oa), wf), n.listenerRefiners), n.optionRefiners), l = {}, u = this.currentViewOptionsInput, c = this.currentViewOptionsRefined, d = !1, f = {};
    for (var p in i)
      i[p] === u[p] || ct[p] && ct[p](i[p], u[p]) ? l[p] = c[p] : (i[p] === this.currentCalendarOptionsInput[p] || ct[p] && ct[p](i[p], this.currentCalendarOptionsInput[p]) ? p in this.currentCalendarOptionsRefined && (l[p] = this.currentCalendarOptionsRefined[p]) : s[p] ? l[p] = s[p](i[p]) : f[p] = i[p], d = !0);
    return d && (this.currentViewOptionsInput = i, this.currentViewOptionsRefined = l), {
      rawOptions: this.currentViewOptionsInput,
      refinedOptions: this.currentViewOptionsRefined,
      extra: f
    };
  }, e;
}();
function Bh(e, t, n, r, o, a, i, s) {
  var l = cs(t || i.defaultCode, i.map);
  return new vp({
    calendarSystem: "gregory",
    timeZone: e,
    namedTimeZoneImpl: a.namedTimeZonedImpl,
    locale: l,
    weekNumberCalculation: n,
    firstDay: r,
    weekText: o,
    cmdFormatter: a.cmdFormatter,
    defaultSeparator: s
  });
}
function Wh(e, t) {
  var n = t.themeClasses[e.themeSystem] || ot;
  return new n(e);
}
function $h(e) {
  var t = e.dateProfileGeneratorClass || ys;
  return new t(e);
}
function zh(e, t, n) {
  return new op(e, t, n);
}
function Gh(e) {
  return cn(e, function(t) {
    return t.ui;
  });
}
function jh(e, t, n) {
  var r = { "": t };
  for (var o in e) {
    var a = e[o];
    a.sourceId && n[a.sourceId] && (r[o] = n[a.sourceId]);
  }
  return r;
}
function Yh(e) {
  var t = e.options;
  return {
    eventUiSingleBase: Xn({
      display: t.eventDisplay,
      editable: t.editable,
      startEditable: t.eventStartEditable,
      durationEditable: t.eventDurationEditable,
      constraint: t.eventConstraint,
      overlap: typeof t.eventOverlap == "boolean" ? t.eventOverlap : void 0,
      allow: t.eventAllow,
      backgroundColor: t.eventBackgroundColor,
      borderColor: t.eventBorderColor,
      textColor: t.eventTextColor,
      color: t.eventColor
    }, e),
    selectionConfig: Xn({
      constraint: t.selectConstraint,
      overlap: typeof t.selectOverlap == "boolean" ? t.selectOverlap : void 0,
      allow: t.selectAllow
    }, e)
  };
}
function Fr(e, t) {
  for (var n = 0, r = t.pluginHooks.isLoadingFuncs; n < r.length; n++) {
    var o = r[n];
    if (o(e))
      return !0;
  }
  return !1;
}
function qh(e) {
  return _p(e.options.businessHours, e);
}
function $a(e, t) {
  for (var n in e)
    console.warn("Unknown option '" + n + "'" + (t ? " for view '" + t + "'" : ""));
}
(function(e) {
  H(t, e);
  function t(n) {
    var r = e.call(this, n) || this;
    return r.handleData = function(o) {
      r.dataManager ? r.setState(o) : r.state = o;
    }, r.dataManager = new Rs({
      optionOverrides: n.optionOverrides,
      calendarApi: n.calendarApi,
      onData: r.handleData
    }), r;
  }
  return t.prototype.render = function() {
    return this.props.children(this.state);
  }, t.prototype.componentDidUpdate = function(n) {
    var r = this.props.optionOverrides;
    r !== n.optionOverrides && this.dataManager.resetOptions(r);
  }, t;
})(go);
var Zh = function() {
  function e() {
    this.strictOrder = !1, this.allowReslicing = !1, this.maxCoord = -1, this.maxStackCnt = -1, this.levelCoords = [], this.entriesByLevel = [], this.stackCnts = {};
  }
  return e.prototype.addSegs = function(t) {
    for (var n = [], r = 0, o = t; r < o.length; r++) {
      var a = o[r];
      this.insertEntry(a, n);
    }
    return n;
  }, e.prototype.insertEntry = function(t, n) {
    var r = this.findInsertion(t);
    return this.isInsertionValid(r, t) ? (this.insertEntryAt(t, r), 1) : this.handleInvalidInsertion(r, t, n);
  }, e.prototype.isInsertionValid = function(t, n) {
    return (this.maxCoord === -1 || t.levelCoord + n.thickness <= this.maxCoord) && (this.maxStackCnt === -1 || t.stackCnt < this.maxStackCnt);
  }, e.prototype.handleInvalidInsertion = function(t, n, r) {
    return this.allowReslicing && t.touchingEntry ? this.splitEntry(n, t.touchingEntry, r) : (r.push(n), 0);
  }, e.prototype.splitEntry = function(t, n, r) {
    var o = 0, a = [], i = t.span, s = n.span;
    return i.start < s.start && (o += this.insertEntry({
      index: t.index,
      thickness: t.thickness,
      span: { start: i.start, end: s.start }
    }, a)), i.end > s.end && (o += this.insertEntry({
      index: t.index,
      thickness: t.thickness,
      span: { start: s.end, end: i.end }
    }, a)), o ? (r.push.apply(r, re([{
      index: t.index,
      thickness: t.thickness,
      span: ks(s, i)
    }], a)), o) : (r.push(t), 0);
  }, e.prototype.insertEntryAt = function(t, n) {
    var r = this, o = r.entriesByLevel, a = r.levelCoords;
    n.lateral === -1 ? (Vr(a, n.level, n.levelCoord), Vr(o, n.level, [t])) : Vr(o[n.level], n.lateral, t), this.stackCnts[nn(t)] = n.stackCnt;
  }, e.prototype.findInsertion = function(t) {
    for (var n = this, r = n.levelCoords, o = n.entriesByLevel, a = n.strictOrder, i = n.stackCnts, s = r.length, l = 0, u = -1, c = -1, d = null, f = 0, p = 0; p < s; p += 1) {
      var h = r[p];
      if (!a && h >= l + t.thickness)
        break;
      for (var b = o[p], E = void 0, y = Ga(b, t.span.start, za), D = y[0] + y[1]; (E = b[D]) && E.span.start < t.span.end; ) {
        var k = h + E.thickness;
        k > l && (l = k, d = E, u = p, c = D), k === l && (f = Math.max(f, i[nn(E)] + 1)), D += 1;
      }
    }
    var T = 0;
    if (d)
      for (T = u + 1; T < s && r[T] < l; )
        T += 1;
    var U = -1;
    return T < s && r[T] === l && (U = Ga(o[T], t.span.end, za)[0]), {
      touchingLevel: u,
      touchingLateral: c,
      touchingEntry: d,
      stackCnt: f,
      levelCoord: l,
      level: T,
      lateral: U
    };
  }, e.prototype.toRects = function() {
    for (var t = this, n = t.entriesByLevel, r = t.levelCoords, o = n.length, a = [], i = 0; i < o; i += 1)
      for (var s = n[i], l = r[i], u = 0, c = s; u < c.length; u++) {
        var d = c[u];
        a.push(v(v({}, d), { levelCoord: l }));
      }
    return a;
  }, e;
}();
function za(e) {
  return e.span.end;
}
function nn(e) {
  return e.index + ":" + e.span.start;
}
function ks(e, t) {
  var n = Math.max(e.start, t.start), r = Math.min(e.end, t.end);
  return n < r ? { start: n, end: r } : null;
}
function Vr(e, t, n) {
  e.splice(t, 0, n);
}
function Ga(e, t, n) {
  var r = 0, o = e.length;
  if (!o || t < n(e[r]))
    return [0, 0];
  if (t > n(e[o - 1]))
    return [o, 0];
  for (; r < o; ) {
    var a = Math.floor(r + (o - r) / 2), i = n(e[a]);
    if (t < i)
      o = a;
    else if (t > i)
      r = a + 1;
    else
      return [a, 1];
  }
  return [r, 0];
}
var Is = function() {
  function e(t) {
    this.component = t.component, this.isHitComboAllowed = t.isHitComboAllowed || null;
  }
  return e.prototype.destroy = function() {
  }, e;
}();
function Xh(e, t) {
  return {
    component: e,
    el: t.el,
    useEventCenter: t.useEventCenter != null ? t.useEventCenter : !0,
    isHitComboAllowed: t.isHitComboAllowed || null
  };
}
var ja = {}, Kh = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this, r = this.props.widgetGroups.map(function(o) {
      return n.renderWidgetGroup(o);
    });
    return m.apply(void 0, re(["div", { className: "fc-toolbar-chunk" }], r));
  }, t.prototype.renderWidgetGroup = function(n) {
    for (var r = this.props, o = this.context.theme, a = [], i = !0, s = 0, l = n; s < l.length; s++) {
      var u = l[s], c = u.buttonName, d = u.buttonClick, f = u.buttonText, p = u.buttonIcon, h = u.buttonHint;
      if (c === "title")
        i = !1, a.push(m("h2", { className: "fc-toolbar-title", id: r.titleId }, r.title));
      else {
        var b = c === r.activeButton, E = !r.isTodayEnabled && c === "today" || !r.isPrevEnabled && c === "prev" || !r.isNextEnabled && c === "next", y = ["fc-" + c + "-button", o.getClass("button")];
        b && y.push(o.getClass("buttonActive")), a.push(m("button", { type: "button", title: typeof h == "function" ? h(r.navUnit) : h, disabled: E, "aria-pressed": b, className: y.join(" "), onClick: d }, f || (p ? m("span", { className: p }) : "")));
      }
    }
    if (a.length > 1) {
      var D = i && o.getClass("buttonGroup") || "";
      return m.apply(void 0, re(["div", { className: D }], a));
    }
    return a[0];
  }, t;
}(X), Ya = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this.props, r = n.model, o = n.extraClassName, a = !1, i, s, l = r.sectionWidgets, u = l.center;
    l.left ? (a = !0, i = l.left) : i = l.start, l.right ? (a = !0, s = l.right) : s = l.end;
    var c = [
      o || "",
      "fc-toolbar",
      a ? "fc-toolbar-ltr" : ""
    ];
    return m(
      "div",
      { className: c.join(" ") },
      this.renderSection("start", i || []),
      this.renderSection("center", u || []),
      this.renderSection("end", s || [])
    );
  }, t.prototype.renderSection = function(n, r) {
    var o = this.props;
    return m(Kh, { key: n, widgetGroups: r, title: o.title, navUnit: o.navUnit, activeButton: o.activeButton, isTodayEnabled: o.isTodayEnabled, isPrevEnabled: o.isPrevEnabled, isNextEnabled: o.isNextEnabled, titleId: o.titleId });
  }, t;
}(X), Jh = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.state = {
      availableWidth: null
    }, n.handleEl = function(r) {
      n.el = r, Ge(n.props.elRef, r), n.updateAvailableWidth();
    }, n.handleResize = function() {
      n.updateAvailableWidth();
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.state, a = r.aspectRatio, i = [
      "fc-view-harness",
      a || r.liquid || r.height ? "fc-view-harness-active" : "fc-view-harness-passive"
    ], s = "", l = "";
    return a ? o.availableWidth !== null ? s = o.availableWidth / a : l = 1 / a * 100 + "%" : s = r.height || "", m("div", { "aria-labelledby": r.labeledById, ref: this.handleEl, className: i.join(" "), style: { height: s, paddingBottom: l } }, r.children);
  }, t.prototype.componentDidMount = function() {
    this.context.addResizeHandler(this.handleResize);
  }, t.prototype.componentWillUnmount = function() {
    this.context.removeResizeHandler(this.handleResize);
  }, t.prototype.updateAvailableWidth = function() {
    this.el && this.props.aspectRatio && this.setState({ availableWidth: this.el.offsetWidth });
  }, t;
}(X), Qh = function(e) {
  H(t, e);
  function t(n) {
    var r = e.call(this, n) || this;
    return r.handleSegClick = function(o, a) {
      var i = r.component, s = i.context, l = oo(a);
      if (l && i.isValidSegDownEl(o.target)) {
        var u = Le(o.target, ".fc-event-forced-url"), c = u ? u.querySelector("a[href]").href : "";
        s.emitter.trigger("eventClick", {
          el: a,
          event: new Ae(i.context, l.eventRange.def, l.eventRange.instance),
          jsEvent: o,
          view: s.viewApi
        }), c && !o.defaultPrevented && (window.location.href = c);
      }
    }, r.destroy = zi(
      n.el,
      "click",
      ".fc-event",
      r.handleSegClick
    ), r;
  }
  return t;
}(Is), ev = function(e) {
  H(t, e);
  function t(n) {
    var r = e.call(this, n) || this;
    return r.handleEventElRemove = function(o) {
      o === r.currentSegEl && r.handleSegLeave(null, r.currentSegEl);
    }, r.handleSegEnter = function(o, a) {
      oo(a) && (r.currentSegEl = a, r.triggerEvent("eventMouseEnter", o, a));
    }, r.handleSegLeave = function(o, a) {
      r.currentSegEl && (r.currentSegEl = null, r.triggerEvent("eventMouseLeave", o, a));
    }, r.removeHoverListeners = Ad(
      n.el,
      ".fc-event",
      r.handleSegEnter,
      r.handleSegLeave
    ), r;
  }
  return t.prototype.destroy = function() {
    this.removeHoverListeners();
  }, t.prototype.triggerEvent = function(n, r, o) {
    var a = this.component, i = a.context, s = oo(o);
    (!r || a.isValidSegDownEl(r.target)) && i.emitter.trigger(n, {
      el: o,
      event: new Ae(i, s.eventRange.def, s.eventRange.instance),
      jsEvent: r,
      view: i.viewApi
    });
  }, t;
}(Is), tv = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.buildViewContext = W(Fp), n.buildViewPropTransformers = W(rv), n.buildToolbarProps = W(nv), n.headerRef = Re(), n.footerRef = Re(), n.interactionsStore = {}, n.state = {
      viewLabelId: tt()
    }, n.registerInteractiveComponent = function(r, o) {
      var a = Xh(r, o), i = [
        Qh,
        ev
      ], s = i.concat(n.props.pluginHooks.componentInteractions), l = s.map(function(u) {
        return new u(a);
      });
      n.interactionsStore[r.uid] = l, ja[r.uid] = a;
    }, n.unregisterInteractiveComponent = function(r) {
      var o = n.interactionsStore[r.uid];
      if (o) {
        for (var a = 0, i = o; a < i.length; a++) {
          var s = i[a];
          s.destroy();
        }
        delete n.interactionsStore[r.uid];
      }
      delete ja[r.uid];
    }, n.resizeRunner = new Po(function() {
      n.props.emitter.trigger("_resize", !0), n.props.emitter.trigger("windowResize", { view: n.props.viewApi });
    }), n.handleWindowResize = function(r) {
      var o = n.props.options;
      o.handleWindowResize && r.target === window && n.resizeRunner.request(o.windowResizeDelay);
    }, n;
  }
  return t.prototype.render = function() {
    var n = this.props, r = n.toolbarConfig, o = n.options, a = this.buildToolbarProps(
      n.viewSpec,
      n.dateProfile,
      n.dateProfileGenerator,
      n.currentDate,
      dn(n.options.now, n.dateEnv),
      n.viewTitle
    ), i = !1, s = "", l;
    n.isHeightAuto || n.forPrint ? s = "" : o.height != null ? i = !0 : o.contentHeight != null ? s = o.contentHeight : l = Math.max(o.aspectRatio, 0.5);
    var u = this.buildViewContext(n.viewSpec, n.viewApi, n.options, n.dateProfileGenerator, n.dateEnv, n.theme, n.pluginHooks, n.dispatch, n.getCurrentData, n.emitter, n.calendarApi, this.registerInteractiveComponent, this.unregisterInteractiveComponent), c = r.header && r.header.hasTitle ? this.state.viewLabelId : "";
    return m(
      yt.Provider,
      { value: u },
      r.header && m(Ya, v({ ref: this.headerRef, extraClassName: "fc-header-toolbar", model: r.header, titleId: c }, a)),
      m(
        Jh,
        { liquid: i, height: s, aspectRatio: l, labeledById: c },
        this.renderView(n),
        this.buildAppendContent()
      ),
      r.footer && m(Ya, v({ ref: this.footerRef, extraClassName: "fc-footer-toolbar", model: r.footer, titleId: "" }, a))
    );
  }, t.prototype.componentDidMount = function() {
    var n = this.props;
    this.calendarInteractions = n.pluginHooks.calendarInteractions.map(function(a) {
      return new a(n);
    }), window.addEventListener("resize", this.handleWindowResize);
    var r = n.pluginHooks.propSetHandlers;
    for (var o in r)
      r[o](n[o], n);
  }, t.prototype.componentDidUpdate = function(n) {
    var r = this.props, o = r.pluginHooks.propSetHandlers;
    for (var a in o)
      r[a] !== n[a] && o[a](r[a], r);
  }, t.prototype.componentWillUnmount = function() {
    window.removeEventListener("resize", this.handleWindowResize), this.resizeRunner.clear();
    for (var n = 0, r = this.calendarInteractions; n < r.length; n++) {
      var o = r[n];
      o.destroy();
    }
    this.props.emitter.trigger("_unmount");
  }, t.prototype.buildAppendContent = function() {
    var n = this.props, r = n.pluginHooks.viewContainerAppends.map(function(o) {
      return o(n);
    });
    return m.apply(void 0, re([me, {}], r));
  }, t.prototype.renderView = function(n) {
    for (var r = n.pluginHooks, o = n.viewSpec, a = {
      dateProfile: n.dateProfile,
      businessHours: n.businessHours,
      eventStore: n.renderableEventStore,
      eventUiBases: n.eventUiBases,
      dateSelection: n.dateSelection,
      eventSelection: n.eventSelection,
      eventDrag: n.eventDrag,
      eventResize: n.eventResize,
      isHeightAuto: n.isHeightAuto,
      forPrint: n.forPrint
    }, i = this.buildViewPropTransformers(r.viewPropsTransformers), s = 0, l = i; s < l.length; s++) {
      var u = l[s];
      v(a, u.transform(a, n));
    }
    var c = o.component;
    return m(c, v({}, a));
  }, t;
}(fr);
function nv(e, t, n, r, o, a) {
  var i = n.build(o, void 0, !1), s = n.buildPrev(t, r, !1), l = n.buildNext(t, r, !1);
  return {
    title: a,
    activeButton: e.type,
    navUnit: e.singleUnit,
    isTodayEnabled: i.isValid && !ft(t.currentRange, o),
    isPrevEnabled: s.isValid,
    isNextEnabled: l.isValid
  };
}
function rv(e) {
  return e.map(function(t) {
    return new t();
  });
}
var ov = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.state = {
      forPrint: !1
    }, n.handleBeforePrint = function() {
      n.setState({ forPrint: !0 });
    }, n.handleAfterPrint = function() {
      n.setState({ forPrint: !1 });
    }, n;
  }
  return t.prototype.render = function() {
    var n = this.props, r = n.options, o = this.state.forPrint, a = o || r.height === "auto" || r.contentHeight === "auto", i = !a && r.height != null ? r.height : "", s = [
      "fc",
      o ? "fc-media-print" : "fc-media-screen",
      "fc-direction-" + r.direction,
      n.theme.getClass("root")
    ];
    return fs() || s.push("fc-liquid-hack"), n.children(s, i, a, o);
  }, t.prototype.componentDidMount = function() {
    var n = this.props.emitter;
    n.on("_beforeprint", this.handleBeforePrint), n.on("_afterprint", this.handleAfterPrint);
  }, t.prototype.componentWillUnmount = function() {
    var n = this.props.emitter;
    n.off("_beforeprint", this.handleBeforePrint), n.off("_afterprint", this.handleAfterPrint);
  }, t;
}(X);
function av(e, t) {
  return !e || t > 10 ? le({ weekday: "short" }) : t > 1 ? le({ weekday: "short", month: "numeric", day: "numeric", omitCommas: !0 }) : le({ weekday: "long" });
}
var xs = "fc-col-header-cell";
function Ms(e) {
  return e.text;
}
var iv = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this.context, r = n.dateEnv, o = n.options, a = n.theme, i = n.viewApi, s = this.props, l = s.date, u = s.dateProfile, c = Io(l, s.todayRange, null, u), d = [xs].concat(dr(c, a)), f = r.format(l, s.dayHeaderFormat), p = !c.isDisabled && s.colCnt > 1 ? an(this.context, l) : {}, h = v(v(v({ date: r.toDate(l), view: i }, s.extraHookProps), { text: f }), c);
    return m(qe, { hookProps: h, classNames: o.dayHeaderClassNames, content: o.dayHeaderContent, defaultContent: Ms, didMount: o.dayHeaderDidMount, willUnmount: o.dayHeaderWillUnmount }, function(b, E, y, D) {
      return m(
        "th",
        v({ ref: b, role: "columnheader", className: d.concat(E).join(" "), "data-date": c.isDisabled ? void 0 : cr(l), colSpan: s.colSpan }, s.extraDataAttrs),
        m("div", { className: "fc-scrollgrid-sync-inner" }, !c.isDisabled && m("a", v({ ref: y, className: [
          "fc-col-header-cell-cushion",
          s.isSticky ? "fc-sticky" : ""
        ].join(" ") }, p), D))
      );
    });
  }, t;
}(X), sv = le({ weekday: "long" }), lv = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this.props, r = this.context, o = r.dateEnv, a = r.theme, i = r.viewApi, s = r.options, l = ue(new Date(2592e5), n.dow), u = {
      dow: n.dow,
      isDisabled: !1,
      isFuture: !1,
      isPast: !1,
      isToday: !1,
      isOther: !1
    }, c = [xs].concat(dr(u, a), n.extraClassNames || []), d = o.format(l, n.dayHeaderFormat), f = v(v(v(v({
      date: l
    }, u), { view: i }), n.extraHookProps), { text: d });
    return m(qe, { hookProps: f, classNames: s.dayHeaderClassNames, content: s.dayHeaderContent, defaultContent: Ms, didMount: s.dayHeaderDidMount, willUnmount: s.dayHeaderWillUnmount }, function(p, h, b, E) {
      return m(
        "th",
        v({ ref: p, role: "columnheader", className: c.concat(h).join(" "), colSpan: n.colSpan }, n.extraDataAttrs),
        m(
          "div",
          { className: "fc-scrollgrid-sync-inner" },
          m("a", { "aria-label": o.format(l, sv), className: [
            "fc-col-header-cell-cushion",
            n.isSticky ? "fc-sticky" : ""
          ].join(" "), ref: b }, E)
        )
      );
    });
  }, t;
}(X), Oo = function(e) {
  H(t, e);
  function t(n, r) {
    var o = e.call(this, n, r) || this;
    return o.initialNowDate = dn(r.options.now, r.dateEnv), o.initialNowQueriedMs = new Date().valueOf(), o.state = o.computeTiming().currentState, o;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.state;
    return r.children(o.nowDate, o.todayRange);
  }, t.prototype.componentDidMount = function() {
    this.setTimeout();
  }, t.prototype.componentDidUpdate = function(n) {
    n.unit !== this.props.unit && (this.clearTimeout(), this.setTimeout());
  }, t.prototype.componentWillUnmount = function() {
    this.clearTimeout();
  }, t.prototype.computeTiming = function() {
    var n = this, r = n.props, o = n.context, a = vt(this.initialNowDate, new Date().valueOf() - this.initialNowQueriedMs), i = o.dateEnv.startOf(a, r.unit), s = o.dateEnv.add(i, Y(1, r.unit)), l = s.valueOf() - a.valueOf();
    return l = Math.min(1e3 * 60 * 60 * 24, l), {
      currentState: { nowDate: i, todayRange: qa(i) },
      nextState: { nowDate: s, todayRange: qa(s) },
      waitMs: l
    };
  }, t.prototype.setTimeout = function() {
    var n = this, r = this.computeTiming(), o = r.nextState, a = r.waitMs;
    this.timeoutId = setTimeout(function() {
      n.setState(o, function() {
        n.setTimeout();
      });
    }, a);
  }, t.prototype.clearTimeout = function() {
    this.timeoutId && clearTimeout(this.timeoutId);
  }, t.contextType = yt, t;
}(go);
function qa(e) {
  var t = Q(e), n = ue(t, 1);
  return { start: t, end: n };
}
var uv = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.createDayHeaderFormatter = W(cv), n;
  }
  return t.prototype.render = function() {
    var n = this.context, r = this.props, o = r.dates, a = r.dateProfile, i = r.datesRepDistinctDays, s = r.renderIntro, l = this.createDayHeaderFormatter(n.options.dayHeaderFormat, i, o.length);
    return m(Oo, { unit: "day" }, function(u, c) {
      return m(
        "tr",
        { role: "row" },
        s && s("day"),
        o.map(function(d) {
          return i ? m(iv, { key: d.toISOString(), date: d, dateProfile: a, todayRange: c, colCnt: o.length, dayHeaderFormat: l }) : m(lv, { key: d.getUTCDay(), dow: d.getUTCDay(), dayHeaderFormat: l });
        })
      );
    });
  }, t;
}(X);
function cv(e, t, n) {
  return e || av(t, n);
}
var dv = function() {
  function e(t, n) {
    for (var r = t.start, o = t.end, a = [], i = [], s = -1; r < o; )
      n.isHiddenDay(r) ? a.push(s + 0.5) : (s += 1, a.push(s), i.push(r)), r = ue(r, 1);
    this.dates = i, this.indices = a, this.cnt = i.length;
  }
  return e.prototype.sliceRange = function(t) {
    var n = this.getDateDayIndex(t.start), r = this.getDateDayIndex(ue(t.end, -1)), o = Math.max(0, n), a = Math.min(this.cnt - 1, r);
    return o = Math.ceil(o), a = Math.floor(a), o <= a ? {
      firstIndex: o,
      lastIndex: a,
      isStart: n === o,
      isEnd: r === a
    } : null;
  }, e.prototype.getDateDayIndex = function(t) {
    var n = this.indices, r = Math.floor(gt(this.dates[0], t));
    return r < 0 ? n[0] - 1 : r >= n.length ? n[n.length - 1] + 1 : n[r];
  }, e;
}(), fv = function() {
  function e(t, n) {
    var r = t.dates, o, a, i;
    if (n) {
      for (a = r[0].getUTCDay(), o = 1; o < r.length && r[o].getUTCDay() !== a; o += 1)
        ;
      i = Math.ceil(r.length / o);
    } else
      i = 1, o = r.length;
    this.rowCnt = i, this.colCnt = o, this.daySeries = t, this.cells = this.buildCells(), this.headerDates = this.buildHeaderDates();
  }
  return e.prototype.buildCells = function() {
    for (var t = [], n = 0; n < this.rowCnt; n += 1) {
      for (var r = [], o = 0; o < this.colCnt; o += 1)
        r.push(this.buildCell(n, o));
      t.push(r);
    }
    return t;
  }, e.prototype.buildCell = function(t, n) {
    var r = this.daySeries.dates[t * this.colCnt + n];
    return {
      key: r.toISOString(),
      date: r
    };
  }, e.prototype.buildHeaderDates = function() {
    for (var t = [], n = 0; n < this.colCnt; n += 1)
      t.push(this.cells[0][n].date);
    return t;
  }, e.prototype.sliceRange = function(t) {
    var n = this.colCnt, r = this.daySeries.sliceRange(t), o = [];
    if (r)
      for (var a = r.firstIndex, i = r.lastIndex, s = a; s <= i; ) {
        var l = Math.floor(s / n), u = Math.min((l + 1) * n, i + 1);
        o.push({
          row: l,
          firstCol: s % n,
          lastCol: (u - 1) % n,
          isStart: r.isStart && s === a,
          isEnd: r.isEnd && u - 1 === i
        }), s = u;
      }
    return o;
  }, e;
}(), pv = function() {
  function e() {
    this.sliceBusinessHours = W(this._sliceBusinessHours), this.sliceDateSelection = W(this._sliceDateSpan), this.sliceEventStore = W(this._sliceEventStore), this.sliceEventDrag = W(this._sliceInteraction), this.sliceEventResize = W(this._sliceInteraction), this.forceDayIfListItem = !1;
  }
  return e.prototype.sliceProps = function(t, n, r, o) {
    for (var a = [], i = 4; i < arguments.length; i++)
      a[i - 4] = arguments[i];
    var s = t.eventUiBases, l = this.sliceEventStore.apply(this, re([t.eventStore, s, n, r], a));
    return {
      dateSelectionSegs: this.sliceDateSelection.apply(this, re([t.dateSelection, s, o], a)),
      businessHourSegs: this.sliceBusinessHours.apply(this, re([t.businessHours, n, r, o], a)),
      fgEventSegs: l.fg,
      bgEventSegs: l.bg,
      eventDrag: this.sliceEventDrag.apply(this, re([t.eventDrag, s, n, r], a)),
      eventResize: this.sliceEventResize.apply(this, re([t.eventResize, s, n, r], a)),
      eventSelection: t.eventSelection
    };
  }, e.prototype.sliceNowDate = function(t, n) {
    for (var r = [], o = 2; o < arguments.length; o++)
      r[o - 2] = arguments[o];
    return this._sliceDateSpan.apply(this, re([
      { range: { start: t, end: vt(t, 1) }, allDay: !1 },
      {},
      n
    ], r));
  }, e.prototype._sliceBusinessHours = function(t, n, r, o) {
    for (var a = [], i = 4; i < arguments.length; i++)
      a[i - 4] = arguments[i];
    return t ? this._sliceEventStore.apply(this, re([
      ur(t, Ur(n, Boolean(r)), o),
      {},
      n,
      r
    ], a)).bg : [];
  }, e.prototype._sliceEventStore = function(t, n, r, o) {
    for (var a = [], i = 4; i < arguments.length; i++)
      a[i - 4] = arguments[i];
    if (t) {
      var s = ro(t, n, Ur(r, Boolean(o)), o);
      return {
        bg: this.sliceEventRanges(s.bg, a),
        fg: this.sliceEventRanges(s.fg, a)
      };
    }
    return { bg: [], fg: [] };
  }, e.prototype._sliceInteraction = function(t, n, r, o) {
    for (var a = [], i = 4; i < arguments.length; i++)
      a[i - 4] = arguments[i];
    if (!t)
      return null;
    var s = ro(t.mutatedEvents, n, Ur(r, Boolean(o)), o);
    return {
      segs: this.sliceEventRanges(s.fg, a),
      affectedInstances: t.affectedEvents.instances,
      isEvent: t.isEvent
    };
  }, e.prototype._sliceDateSpan = function(t, n, r) {
    for (var o = [], a = 3; a < arguments.length; a++)
      o[a - 3] = arguments[a];
    if (!t)
      return [];
    for (var i = Kf(t, n, r), s = this.sliceRange.apply(this, re([t.range], o)), l = 0, u = s; l < u.length; l++) {
      var c = u[l];
      c.eventRange = i;
    }
    return s;
  }, e.prototype.sliceEventRanges = function(t, n) {
    for (var r = [], o = 0, a = t; o < a.length; o++) {
      var i = a[o];
      r.push.apply(r, this.sliceEventRange(i, n));
    }
    return r;
  }, e.prototype.sliceEventRange = function(t, n) {
    var r = t.range;
    this.forceDayIfListItem && t.ui.display === "list-item" && (r = {
      start: r.start,
      end: ue(r.start, 1)
    });
    for (var o = this.sliceRange.apply(this, re([r], n)), a = 0, i = o; a < i.length; a++) {
      var s = i[a];
      s.eventRange = t, s.isStart = t.isStart && s.isStart, s.isEnd = t.isEnd && s.isEnd;
    }
    return o;
  }, e;
}();
function Ur(e, t) {
  var n = e.activeRange;
  return t ? n : {
    start: vt(n.start, e.slotMinTime.milliseconds),
    end: vt(n.end, e.slotMaxTime.milliseconds - 864e5)
  };
}
var _n = /^(visible|hidden)$/, Ps = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.handleEl = function(r) {
      n.el = r, Ge(n.props.elRef, r);
    }, n;
  }
  return t.prototype.render = function() {
    var n = this.props, r = n.liquid, o = n.liquidIsAbsolute, a = r && o, i = ["fc-scroller"];
    return r && (o ? i.push("fc-scroller-liquid-absolute") : i.push("fc-scroller-liquid")), m("div", { ref: this.handleEl, className: i.join(" "), style: {
      overflowX: n.overflowX,
      overflowY: n.overflowY,
      left: a && -(n.overcomeLeft || 0) || "",
      right: a && -(n.overcomeRight || 0) || "",
      bottom: a && -(n.overcomeBottom || 0) || "",
      marginLeft: !a && -(n.overcomeLeft || 0) || "",
      marginRight: !a && -(n.overcomeRight || 0) || "",
      marginBottom: !a && -(n.overcomeBottom || 0) || "",
      maxHeight: n.maxHeight || ""
    } }, n.children);
  }, t.prototype.needsXScrolling = function() {
    if (_n.test(this.props.overflowX))
      return !1;
    for (var n = this.el, r = this.el.getBoundingClientRect().width - this.getYScrollbarWidth(), o = n.children, a = 0; a < o.length; a += 1) {
      var i = o[a];
      if (i.getBoundingClientRect().width > r)
        return !0;
    }
    return !1;
  }, t.prototype.needsYScrolling = function() {
    if (_n.test(this.props.overflowY))
      return !1;
    for (var n = this.el, r = this.el.getBoundingClientRect().height - this.getXScrollbarWidth(), o = n.children, a = 0; a < o.length; a += 1) {
      var i = o[a];
      if (i.getBoundingClientRect().height > r)
        return !0;
    }
    return !1;
  }, t.prototype.getXScrollbarWidth = function() {
    return _n.test(this.props.overflowX) ? 0 : this.el.offsetHeight - this.el.clientHeight;
  }, t.prototype.getYScrollbarWidth = function() {
    return _n.test(this.props.overflowY) ? 0 : this.el.offsetWidth - this.el.clientWidth;
  }, t;
}(X), dt = function() {
  function e(t) {
    var n = this;
    this.masterCallback = t, this.currentMap = {}, this.depths = {}, this.callbackMap = {}, this.handleValue = function(r, o) {
      var a = n, i = a.depths, s = a.currentMap, l = !1, u = !1;
      r !== null ? (l = o in s, s[o] = r, i[o] = (i[o] || 0) + 1, u = !0) : (i[o] -= 1, i[o] || (delete s[o], delete n.callbackMap[o], l = !0)), n.masterCallback && (l && n.masterCallback(null, String(o)), u && n.masterCallback(r, String(o)));
    };
  }
  return e.prototype.createRef = function(t) {
    var n = this, r = this.callbackMap[t];
    return r || (r = this.callbackMap[t] = function(o) {
      n.handleValue(o, String(t));
    }), r;
  }, e.prototype.collect = function(t, n, r) {
    return Qd(this.currentMap, t, n, r);
  }, e.prototype.getAll = function() {
    return Eo(this.currentMap);
  }, e;
}();
function hv(e) {
  for (var t = Id(e, ".fc-scrollgrid-shrink"), n = 0, r = 0, o = t; r < o.length; r++) {
    var a = o[r];
    n = Math.max(n, Ud(a));
  }
  return Math.ceil(n);
}
function Os(e, t) {
  return e.liquid && t.liquid;
}
function vv(e, t) {
  return t.maxHeight != null || Os(e, t);
}
function mv(e, t, n, r) {
  var o = n.expandRows, a = typeof t.content == "function" ? t.content(n) : m("table", {
    role: "presentation",
    className: [
      t.tableClassName,
      e.syncRowHeights ? "fc-scrollgrid-sync-table" : ""
    ].join(" "),
    style: {
      minWidth: n.tableMinWidth,
      width: n.clientWidth,
      height: o ? n.clientHeight : ""
    }
  }, n.tableColGroupNode, m(r ? "thead" : "tbody", {
    role: "presentation"
  }, typeof t.rowContent == "function" ? t.rowContent(n) : t.rowContent));
  return a;
}
function gv(e, t) {
  return Vt(e, t, ze);
}
function yv(e, t) {
  for (var n = [], r = 0, o = e; r < o.length; r++)
    for (var a = o[r], i = a.span || 1, s = 0; s < i; s += 1)
      n.push(m("col", { style: {
        width: a.width === "shrink" ? bv(t) : a.width || "",
        minWidth: a.minWidth || ""
      } }));
  return m.apply(void 0, re(["colgroup", {}], n));
}
function bv(e) {
  return e == null ? 4 : e;
}
function Ev(e) {
  for (var t = 0, n = e; t < n.length; t++) {
    var r = n[t];
    if (r.width === "shrink")
      return !0;
  }
  return !1;
}
function _v(e, t) {
  var n = [
    "fc-scrollgrid",
    t.theme.getClass("table")
  ];
  return e && n.push("fc-scrollgrid-liquid"), n;
}
function Cv(e, t) {
  var n = [
    "fc-scrollgrid-section",
    "fc-scrollgrid-section-" + e.type,
    e.className
  ];
  return t && e.liquid && e.maxHeight == null && n.push("fc-scrollgrid-section-liquid"), e.isSticky && n.push("fc-scrollgrid-section-sticky"), n;
}
function Dv(e) {
  return m("div", { className: "fc-scrollgrid-sticky-shim", style: {
    width: e.clientWidth,
    minWidth: e.tableMinWidth
  } });
}
function Za(e) {
  var t = e.stickyHeaderDates;
  return (t == null || t === "auto") && (t = e.height === "auto" || e.viewHeight === "auto"), t;
}
function Sv(e) {
  var t = e.stickyFooterScrollbar;
  return (t == null || t === "auto") && (t = e.height === "auto" || e.viewHeight === "auto"), t;
}
var As = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.processCols = W(function(r) {
      return r;
    }, gv), n.renderMicroColGroup = W(yv), n.scrollerRefs = new dt(), n.scrollerElRefs = new dt(n._handleScrollerEl.bind(n)), n.state = {
      shrinkWidth: null,
      forceYScrollbars: !1,
      scrollerClientWidths: {},
      scrollerClientHeights: {}
    }, n.handleSizing = function() {
      n.safeSetState(v({ shrinkWidth: n.computeShrinkWidth() }, n.computeScrollerDims()));
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.state, a = n.context, i = r.sections || [], s = this.processCols(r.cols), l = this.renderMicroColGroup(s, o.shrinkWidth), u = _v(r.liquid, a);
    r.collapsibleWidth && u.push("fc-scrollgrid-collapsible");
    for (var c = i.length, d = 0, f, p = [], h = [], b = []; d < c && (f = i[d]).type === "header"; )
      p.push(this.renderSection(f, l, !0)), d += 1;
    for (; d < c && (f = i[d]).type === "body"; )
      h.push(this.renderSection(f, l, !1)), d += 1;
    for (; d < c && (f = i[d]).type === "footer"; )
      b.push(this.renderSection(f, l, !0)), d += 1;
    var E = !fs(), y = { role: "rowgroup" };
    return m("table", {
      role: "grid",
      className: u.join(" "),
      style: { height: r.height }
    }, Boolean(!E && p.length) && m.apply(void 0, re(["thead", y], p)), Boolean(!E && h.length) && m.apply(void 0, re(["tbody", y], h)), Boolean(!E && b.length) && m.apply(void 0, re(["tfoot", y], b)), E && m.apply(void 0, re(re(re(["tbody", y], p), h), b)));
  }, t.prototype.renderSection = function(n, r, o) {
    return "outerContent" in n ? m(me, { key: n.key }, n.outerContent) : m("tr", { key: n.key, role: "presentation", className: Cv(n, this.props.liquid).join(" ") }, this.renderChunkTd(n, r, n.chunk, o));
  }, t.prototype.renderChunkTd = function(n, r, o, a) {
    if ("outerContent" in o)
      return o.outerContent;
    var i = this.props, s = this.state, l = s.forceYScrollbars, u = s.scrollerClientWidths, c = s.scrollerClientHeights, d = vv(i, n), f = Os(i, n), p = i.liquid ? l ? "scroll" : d ? "auto" : "hidden" : "visible", h = n.key, b = mv(n, o, {
      tableColGroupNode: r,
      tableMinWidth: "",
      clientWidth: !i.collapsibleWidth && u[h] !== void 0 ? u[h] : null,
      clientHeight: c[h] !== void 0 ? c[h] : null,
      expandRows: n.expandRows,
      syncRowHeights: !1,
      rowSyncHeights: [],
      reportRowHeightChange: function() {
      }
    }, a);
    return m(a ? "th" : "td", {
      ref: o.elRef,
      role: "presentation"
    }, m(
      "div",
      { className: "fc-scroller-harness" + (f ? " fc-scroller-harness-liquid" : "") },
      m(Ps, { ref: this.scrollerRefs.createRef(h), elRef: this.scrollerElRefs.createRef(h), overflowY: p, overflowX: i.liquid ? "hidden" : "visible", maxHeight: n.maxHeight, liquid: f, liquidIsAbsolute: !0 }, b)
    ));
  }, t.prototype._handleScrollerEl = function(n, r) {
    var o = wv(this.props.sections, r);
    o && Ge(o.chunk.scrollerElRef, n);
  }, t.prototype.componentDidMount = function() {
    this.handleSizing(), this.context.addResizeHandler(this.handleSizing);
  }, t.prototype.componentDidUpdate = function() {
    this.handleSizing();
  }, t.prototype.componentWillUnmount = function() {
    this.context.removeResizeHandler(this.handleSizing);
  }, t.prototype.computeShrinkWidth = function() {
    return Ev(this.props.cols) ? hv(this.scrollerElRefs.getAll()) : 0;
  }, t.prototype.computeScrollerDims = function() {
    var n = Rp(), r = this, o = r.scrollerRefs, a = r.scrollerElRefs, i = !1, s = {}, l = {};
    for (var u in o.currentMap) {
      var c = o.currentMap[u];
      if (c && c.needsYScrolling()) {
        i = !0;
        break;
      }
    }
    for (var d = 0, f = this.props.sections; d < f.length; d++) {
      var p = f[d], u = p.key, h = a.currentMap[u];
      if (h) {
        var b = h.parentNode;
        s[u] = Math.floor(b.getBoundingClientRect().width - (i ? n.y : 0)), l[u] = Math.floor(b.getBoundingClientRect().height);
      }
    }
    return { forceYScrollbars: i, scrollerClientWidths: s, scrollerClientHeights: l };
  }, t;
}(X);
As.addStateEquality({
  scrollerClientWidths: ze,
  scrollerClientHeights: ze
});
function wv(e, t) {
  for (var n = 0, r = e; n < r.length; n++) {
    var o = r[n];
    if (o.key === t)
      return o;
  }
  return null;
}
var pr = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.elRef = Re(), n;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context, a = o.options, i = r.seg, s = i.eventRange, l = s.ui, u = {
      event: new Ae(o, s.def, s.instance),
      view: o.viewApi,
      timeText: r.timeText,
      textColor: l.textColor,
      backgroundColor: l.backgroundColor,
      borderColor: l.borderColor,
      isDraggable: !r.disableDragging && Wf(i, o),
      isStartResizable: !r.disableResizing && $f(i, o),
      isEndResizable: !r.disableResizing && zf(i),
      isMirror: Boolean(r.isDragging || r.isResizing || r.isDateSelecting),
      isStart: Boolean(i.isStart),
      isEnd: Boolean(i.isEnd),
      isPast: Boolean(r.isPast),
      isFuture: Boolean(r.isFuture),
      isToday: Boolean(r.isToday),
      isSelected: Boolean(r.isSelected),
      isDragging: Boolean(r.isDragging),
      isResizing: Boolean(r.isResizing)
    }, c = Gf(u).concat(l.classNames);
    return m(qe, { hookProps: u, classNames: a.eventClassNames, content: a.eventContent, defaultContent: r.defaultContent, didMount: a.eventDidMount, willUnmount: a.eventWillUnmount, elRef: this.elRef }, function(d, f, p, h) {
      return r.children(d, c.concat(f), p, h, u);
    });
  }, t.prototype.componentDidMount = function() {
    Na(this.elRef.current, this.props.seg);
  }, t.prototype.componentDidUpdate = function(n) {
    var r = this.props.seg;
    r !== n.seg && Na(this.elRef.current, r);
  }, t;
}(X), Tv = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context, a = r.seg, i = o.options.eventTimeFormat || r.defaultTimeFormat, s = tn(a, i, o, r.defaultDisplayEventTime, r.defaultDisplayEventEnd);
    return m(pr, { seg: a, timeText: s, disableDragging: r.disableDragging, disableResizing: r.disableResizing, defaultContent: r.defaultContent || Rv, isDragging: r.isDragging, isResizing: r.isResizing, isDateSelecting: r.isDateSelecting, isSelected: r.isSelected, isPast: r.isPast, isFuture: r.isFuture, isToday: r.isToday }, function(l, u, c, d, f) {
      return m(
        "a",
        v({ className: r.extraClassNames.concat(u).join(" "), style: {
          borderColor: f.borderColor,
          backgroundColor: f.backgroundColor
        }, ref: l }, Ro(a, o)),
        m("div", { className: "fc-event-main", ref: c, style: { color: f.textColor } }, d),
        f.isStartResizable && m("div", { className: "fc-event-resizer fc-event-resizer-start" }),
        f.isEndResizable && m("div", { className: "fc-event-resizer fc-event-resizer-end" })
      );
    });
  }, t;
}(X);
function Rv(e) {
  return m(
    "div",
    { className: "fc-event-main-frame" },
    e.timeText && m("div", { className: "fc-event-time" }, e.timeText),
    m(
      "div",
      { className: "fc-event-title-container" },
      m("div", { className: "fc-event-title fc-sticky" }, e.event.title || m(me, null, "\xA0"))
    )
  );
}
var kv = le({ day: "numeric" }), Ns = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context, a = o.options, i = Hs({
      date: r.date,
      dateProfile: r.dateProfile,
      todayRange: r.todayRange,
      showDayNumber: r.showDayNumber,
      extraProps: r.extraHookProps,
      viewApi: o.viewApi,
      dateEnv: o.dateEnv
    });
    return m(vs, { hookProps: i, content: a.dayCellContent, defaultContent: r.defaultContent }, r.children);
  }, t;
}(X);
function Hs(e) {
  var t = e.date, n = e.dateEnv, r = Io(t, e.todayRange, null, e.dateProfile);
  return v(v(v({ date: n.toDate(t), view: e.viewApi }, r), { dayNumberText: e.showDayNumber ? n.format(t, kv) : "" }), e.extraProps);
}
var Fs = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.refineHookProps = Pn(Hs), n.normalizeClassNames = ms(), n;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context, a = o.options, i = this.refineHookProps({
      date: r.date,
      dateProfile: r.dateProfile,
      todayRange: r.todayRange,
      showDayNumber: r.showDayNumber,
      extraProps: r.extraHookProps,
      viewApi: o.viewApi,
      dateEnv: o.dateEnv
    }), s = dr(i, o.theme).concat(i.isDisabled ? [] : this.normalizeClassNames(a.dayCellClassNames, i)), l = i.isDisabled ? {} : {
      "data-date": cr(r.date)
    };
    return m(xo, { hookProps: i, didMount: a.dayCellDidMount, willUnmount: a.dayCellWillUnmount, elRef: r.elRef }, function(u) {
      return r.children(u, s, l, i.isDisabled);
    });
  }, t;
}(X);
function Iv(e) {
  return m("div", { className: "fc-" + e });
}
var xv = function(e) {
  return m(pr, { defaultContent: Mv, seg: e.seg, timeText: "", disableDragging: !0, disableResizing: !0, isDragging: !1, isResizing: !1, isDateSelecting: !1, isSelected: !1, isPast: e.isPast, isFuture: e.isFuture, isToday: e.isToday }, function(t, n, r, o, a) {
    return m("div", { ref: t, className: ["fc-bg-event"].concat(n).join(" "), style: {
      backgroundColor: a.backgroundColor
    } }, o);
  });
};
function Mv(e) {
  var t = e.event.title;
  return t && m("div", { className: "fc-event-title" }, e.event.title);
}
var Pv = function(e) {
  return m(yt.Consumer, null, function(t) {
    var n = t.dateEnv, r = t.options, o = e.date, a = r.weekNumberFormat || e.defaultFormat, i = n.computeWeekNumber(o), s = n.format(o, a), l = { num: i, text: s, date: o };
    return m(qe, { hookProps: l, classNames: r.weekNumberClassNames, content: r.weekNumberContent, defaultContent: Ov, didMount: r.weekNumberDidMount, willUnmount: r.weekNumberWillUnmount }, e.children);
  });
};
function Ov(e) {
  return e.text;
}
var Lr = 10, Av = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.state = {
      titleId: tt()
    }, n.handleRootEl = function(r) {
      n.rootEl = r, n.props.elRef && Ge(n.props.elRef, r);
    }, n.handleDocumentMouseDown = function(r) {
      var o = Pd(r);
      n.rootEl.contains(o) || n.handleCloseClick();
    }, n.handleDocumentKeyDown = function(r) {
      r.key === "Escape" && n.handleCloseClick();
    }, n.handleCloseClick = function() {
      var r = n.props.onClose;
      r && r();
    }, n;
  }
  return t.prototype.render = function() {
    var n = this.context, r = n.theme, o = n.options, a = this, i = a.props, s = a.state, l = [
      "fc-popover",
      r.getClass("popover")
    ].concat(i.extraClassNames || []);
    return wd(m(
      "div",
      v({ id: i.id, className: l.join(" "), "aria-labelledby": s.titleId }, i.extraAttrs, { ref: this.handleRootEl }),
      m(
        "div",
        { className: "fc-popover-header " + r.getClass("popoverHeader") },
        m("span", { className: "fc-popover-title", id: s.titleId }, i.title),
        m("span", { className: "fc-popover-close " + r.getIconClass("close"), title: o.closeHint, onClick: this.handleCloseClick })
      ),
      m("div", { className: "fc-popover-body " + r.getClass("popoverContent") }, i.children)
    ), i.parentEl);
  }, t.prototype.componentDidMount = function() {
    document.addEventListener("mousedown", this.handleDocumentMouseDown), document.addEventListener("keydown", this.handleDocumentKeyDown), this.updateSize();
  }, t.prototype.componentWillUnmount = function() {
    document.removeEventListener("mousedown", this.handleDocumentMouseDown), document.removeEventListener("keydown", this.handleDocumentKeyDown);
  }, t.prototype.updateSize = function() {
    var n = this.context.isRtl, r = this.props, o = r.alignmentEl, a = r.alignGridTop, i = this.rootEl, s = xp(o);
    if (s) {
      var l = i.getBoundingClientRect(), u = a ? Le(o, ".fc-scrollgrid").getBoundingClientRect().top : s.top, c = n ? s.right - l.width : s.left;
      u = Math.max(u, Lr), c = Math.min(c, document.documentElement.clientWidth - Lr - l.width), c = Math.max(c, Lr);
      var d = i.offsetParent.getBoundingClientRect();
      Md(i, {
        top: u - d.top,
        left: c - d.left
      });
    }
  }, t;
}(X), Nv = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.handleRootEl = function(r) {
      n.rootEl = r, r ? n.context.registerInteractiveComponent(n, {
        el: r,
        useEventCenter: !1
      }) : n.context.unregisterInteractiveComponent(n);
    }, n;
  }
  return t.prototype.render = function() {
    var n = this.context, r = n.options, o = n.dateEnv, a = this.props, i = a.startDate, s = a.todayRange, l = a.dateProfile, u = o.format(i, r.dayPopoverFormat);
    return m(Fs, { date: i, dateProfile: l, todayRange: s, elRef: this.handleRootEl }, function(c, d, f) {
      return m(
        Av,
        { elRef: c, id: a.id, title: u, extraClassNames: ["fc-more-popover"].concat(d), extraAttrs: f, parentEl: a.parentEl, alignmentEl: a.alignmentEl, alignGridTop: a.alignGridTop, onClose: a.onClose },
        m(Ns, { date: i, dateProfile: l, todayRange: s }, function(p, h) {
          return h && m("div", { className: "fc-more-popover-misc", ref: p }, h);
        }),
        a.children
      );
    });
  }, t.prototype.queryHit = function(n, r, o, a) {
    var i = this, s = i.rootEl, l = i.props;
    return n >= 0 && n < o && r >= 0 && r < a ? {
      dateProfile: l.dateProfile,
      dateSpan: v({ allDay: !0, range: {
        start: l.startDate,
        end: l.endDate
      } }, l.extraDateSpan),
      dayEl: s,
      rect: {
        left: 0,
        top: 0,
        right: o,
        bottom: a
      },
      layer: 1
    } : null;
  }, t;
}(bt), Hv = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.linkElRef = Re(), n.state = {
      isPopoverOpen: !1,
      popoverId: tt()
    }, n.handleClick = function(r) {
      var o = n, a = o.props, i = o.context, s = i.options.moreLinkClick, l = Xa(a).start;
      function u(c) {
        var d = c.eventRange, f = d.def, p = d.instance, h = d.range;
        return {
          event: new Ae(i, f, p),
          start: i.dateEnv.toDate(h.start),
          end: i.dateEnv.toDate(h.end),
          isStart: c.isStart,
          isEnd: c.isEnd
        };
      }
      typeof s == "function" && (s = s({
        date: l,
        allDay: Boolean(a.allDayDate),
        allSegs: a.allSegs.map(u),
        hiddenSegs: a.hiddenSegs.map(u),
        jsEvent: r,
        view: i.viewApi
      })), !s || s === "popover" ? n.setState({ isPopoverOpen: !0 }) : typeof s == "string" && i.calendarApi.zoomTo(l, s);
    }, n.handlePopoverClose = function() {
      n.setState({ isPopoverOpen: !1 });
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = this, o = r.props, a = r.state;
    return m(yt.Consumer, null, function(i) {
      var s = i.viewApi, l = i.options, u = i.calendarApi, c = l.moreLinkText, d = o.moreCnt, f = Xa(o), p = typeof c == "function" ? c.call(u, d) : "+" + d + " " + c, h = Qt(l.moreLinkHint, [d], p), b = {
        num: d,
        shortText: "+" + d,
        text: p,
        view: s
      };
      return m(
        me,
        null,
        Boolean(o.moreCnt) && m(qe, { elRef: n.linkElRef, hookProps: b, classNames: l.moreLinkClassNames, content: l.moreLinkContent, defaultContent: o.defaultContent || Fv, didMount: l.moreLinkDidMount, willUnmount: l.moreLinkWillUnmount }, function(E, y, D, k) {
          return o.children(E, ["fc-more-link"].concat(y), D, k, n.handleClick, h, a.isPopoverOpen, a.isPopoverOpen ? a.popoverId : "");
        }),
        a.isPopoverOpen && m(Nv, { id: a.popoverId, startDate: f.start, endDate: f.end, dateProfile: o.dateProfile, todayRange: o.todayRange, extraDateSpan: o.extraDateSpan, parentEl: n.parentEl, alignmentEl: o.alignmentElRef.current, alignGridTop: o.alignGridTop, onClose: n.handlePopoverClose }, o.popoverContent())
      );
    });
  }, t.prototype.componentDidMount = function() {
    this.updateParentEl();
  }, t.prototype.componentDidUpdate = function() {
    this.updateParentEl();
  }, t.prototype.updateParentEl = function() {
    this.linkElRef.current && (this.parentEl = Le(this.linkElRef.current, ".fc-view-harness"));
  }, t;
}(X);
function Fv(e) {
  return e.text;
}
function Xa(e) {
  if (e.allDayDate)
    return {
      start: e.allDayDate,
      end: ue(e.allDayDate, 1)
    };
  var t = e.hiddenSegs;
  return {
    start: Vv(t),
    end: Lv(t)
  };
}
function Vv(e) {
  return e.reduce(Uv).eventRange.range.start;
}
function Uv(e, t) {
  return e.eventRange.range.start < t.eventRange.range.start ? e : t;
}
function Lv(e) {
  return e.reduce(Bv).eventRange.range.end;
}
function Bv(e, t) {
  return e.eventRange.range.end > t.eventRange.range.end ? e : t;
}
/*!
FullCalendar v5.11.2
Docs & License: https://fullcalendar.io/
(c) 2022 Adam Shaw
*/
var Wv = function(e) {
  H(t, e);
  function t(n, r) {
    r === void 0 && (r = {});
    var o = e.call(this) || this;
    return o.isRendering = !1, o.isRendered = !1, o.currentClassNames = [], o.customContentRenderId = 0, o.handleAction = function(a) {
      switch (a.type) {
        case "SET_EVENT_DRAG":
        case "SET_EVENT_RESIZE":
          o.renderRunner.tryDrain();
      }
    }, o.handleData = function(a) {
      o.currentData = a, o.renderRunner.request(a.calendarOptions.rerenderDelay);
    }, o.handleRenderRequest = function() {
      if (o.isRendering) {
        o.isRendered = !0;
        var a = o.currentData;
        Ea(function() {
          Sd(m(ov, { options: a.calendarOptions, theme: a.theme, emitter: a.emitter }, function(i, s, l, u) {
            return o.setClassNames(i), o.setHeight(s), m(
              hs.Provider,
              { value: o.customContentRenderId },
              m(tv, v({ isHeightAuto: l, forPrint: u }, a))
            );
          }), o.el);
        });
      } else
        o.isRendered && (o.isRendered = !1, Td(o.el), o.setClassNames([]), o.setHeight(""));
    }, o.el = n, o.renderRunner = new Po(o.handleRenderRequest), new Rs({
      optionOverrides: r,
      calendarApi: o,
      onAction: o.handleAction,
      onData: o.handleData
    }), o;
  }
  return Object.defineProperty(t.prototype, "view", {
    get: function() {
      return this.currentData.viewApi;
    },
    enumerable: !1,
    configurable: !0
  }), t.prototype.render = function() {
    var n = this.isRendering;
    n ? this.customContentRenderId += 1 : this.isRendering = !0, this.renderRunner.request(), n && this.updateSize();
  }, t.prototype.destroy = function() {
    this.isRendering && (this.isRendering = !1, this.renderRunner.request());
  }, t.prototype.updateSize = function() {
    var n = this;
    Ea(function() {
      e.prototype.updateSize.call(n);
    });
  }, t.prototype.batchRendering = function(n) {
    this.renderRunner.pause("batchRendering"), n(), this.renderRunner.resume("batchRendering");
  }, t.prototype.pauseRendering = function() {
    this.renderRunner.pause("pauseRendering");
  }, t.prototype.resumeRendering = function() {
    this.renderRunner.resume("pauseRendering", !0);
  }, t.prototype.resetOptions = function(n, r) {
    this.currentDataManager.resetOptions(n, r);
  }, t.prototype.setClassNames = function(n) {
    if (!Vt(n, this.currentClassNames)) {
      for (var r = this.el.classList, o = 0, a = this.currentClassNames; o < a.length; o++) {
        var i = a[o];
        r.remove(i);
      }
      for (var s = 0, l = n; s < l.length; s++) {
        var i = l[s];
        r.add(i);
      }
      this.currentClassNames = n;
    }
  }, t.prototype.setHeight = function(n) {
    $i(this.el, "height", n);
  }, t;
}(up);
/*!
FullCalendar v5.11.2
Docs & License: https://fullcalendar.io/
(c) 2022 Adam Shaw
*/
var $v = {
  googleCalendarApiKey: String
}, zv = {
  googleCalendarApiKey: String,
  googleCalendarId: String,
  googleCalendarApiBase: String,
  extraParams: g
}, Gv = "https://www.googleapis.com/calendar/v3/calendars", jv = {
  parseMeta: function(e) {
    var t = e.googleCalendarId;
    return !t && e.url && (t = Yv(e.url)), t ? {
      googleCalendarId: t,
      googleCalendarApiKey: e.googleCalendarApiKey,
      googleCalendarApiBase: e.googleCalendarApiBase,
      extraParams: e.extraParams
    } : null;
  },
  fetch: function(e, t, n) {
    var r = e.context, o = r.dateEnv, a = r.options, i = e.eventSource.meta, s = i.googleCalendarApiKey || a.googleCalendarApiKey;
    if (!s)
      n({
        message: "Specify a googleCalendarApiKey. See http://fullcalendar.io/docs/google_calendar/"
      });
    else {
      var l = qv(i), u = i.extraParams, c = typeof u == "function" ? u() : u, d = Zv(e.range, s, c, o);
      ws("GET", l, d, function(f, p) {
        f.error ? n({
          message: "Google Calendar API: " + f.error.message,
          errors: f.error.errors,
          xhr: p
        }) : t({
          rawEvents: Xv(f.items, d.timeZone),
          xhr: p
        });
      }, function(f, p) {
        n({ message: f, xhr: p });
      });
    }
  }
};
function Yv(e) {
  var t;
  return /^[^/]+@([^/.]+\.)*(google|googlemail|gmail)\.com$/.test(e) ? e : (t = /^https:\/\/www.googleapis.com\/calendar\/v3\/calendars\/([^/]*)/.exec(e)) || (t = /^https?:\/\/www.google.com\/calendar\/feeds\/([^/]*)/.exec(e)) ? decodeURIComponent(t[1]) : null;
}
function qv(e) {
  var t = e.googleCalendarApiBase;
  return t || (t = Gv), t + "/" + encodeURIComponent(e.googleCalendarId) + "/events";
}
function Zv(e, t, n, r) {
  var o, a, i;
  return r.canComputeOffset ? (a = r.formatIso(e.start), i = r.formatIso(e.end)) : (a = ue(e.start, -1).toISOString(), i = ue(e.end, 1).toISOString()), o = v(v({}, n || {}), { key: t, timeMin: a, timeMax: i, singleEvents: !0, maxResults: 9999 }), r.timeZone !== "local" && (o.timeZone = r.timeZone), o;
}
function Xv(e, t) {
  return e.map(function(n) {
    return Kv(n, t);
  });
}
function Kv(e, t) {
  var n = e.htmlLink || null;
  return n && t && (n = Jv(n, "ctz=" + t)), {
    id: e.id,
    title: e.summary,
    start: e.start.dateTime || e.start.date,
    end: e.end.dateTime || e.end.date,
    url: n,
    location: e.location,
    description: e.description,
    attachments: e.attachments || [],
    extendedProps: (e.extendedProperties || {}).shared || {}
  };
}
function Jv(e, t) {
  return e.replace(/(\?.*?)?(#|$)/, function(n, r, o) {
    return (r ? r + "&" : "?") + t + o;
  });
}
var Qv = Ye({
  eventSourceDefs: [jv],
  optionRefiners: $v,
  eventSourceRefiners: zv
});
/*!
FullCalendar v5.11.2
Docs & License: https://fullcalendar.io/
(c) 2022 Adam Shaw
*/
var em = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.headerElRef = Re(), n;
  }
  return t.prototype.renderSimpleLayout = function(n, r) {
    var o = this, a = o.props, i = o.context, s = [], l = Za(i.options);
    return n && s.push({
      type: "header",
      key: "header",
      isSticky: l,
      chunk: {
        elRef: this.headerElRef,
        tableClassName: "fc-col-header",
        rowContent: n
      }
    }), s.push({
      type: "body",
      key: "body",
      liquid: !0,
      chunk: { content: r }
    }), m(Kn, { viewSpec: i.viewSpec }, function(u, c) {
      return m(
        "div",
        { ref: u, className: ["fc-daygrid"].concat(c).join(" ") },
        m(As, { liquid: !a.isHeightAuto && !a.forPrint, collapsibleWidth: a.forPrint, cols: [], sections: s })
      );
    });
  }, t.prototype.renderHScrollLayout = function(n, r, o, a) {
    var i = this.context.pluginHooks.scrollGridImpl;
    if (!i)
      throw new Error("No ScrollGrid implementation");
    var s = this, l = s.props, u = s.context, c = !l.forPrint && Za(u.options), d = !l.forPrint && Sv(u.options), f = [];
    return n && f.push({
      type: "header",
      key: "header",
      isSticky: c,
      chunks: [{
        key: "main",
        elRef: this.headerElRef,
        tableClassName: "fc-col-header",
        rowContent: n
      }]
    }), f.push({
      type: "body",
      key: "body",
      liquid: !0,
      chunks: [{
        key: "main",
        content: r
      }]
    }), d && f.push({
      type: "footer",
      key: "footer",
      isSticky: !0,
      chunks: [{
        key: "main",
        content: Dv
      }]
    }), m(Kn, { viewSpec: u.viewSpec }, function(p, h) {
      return m(
        "div",
        { ref: p, className: ["fc-daygrid"].concat(h).join(" ") },
        m(i, { liquid: !l.isHeightAuto && !l.forPrint, collapsibleWidth: l.forPrint, colGroups: [{ cols: [{ span: o, minWidth: a }] }], sections: f })
      );
    });
  }, t;
}(bt);
function Cn(e, t) {
  for (var n = [], r = 0; r < t; r += 1)
    n[r] = [];
  for (var o = 0, a = e; o < a.length; o++) {
    var i = a[o];
    n[i.row].push(i);
  }
  return n;
}
function Dn(e, t) {
  for (var n = [], r = 0; r < t; r += 1)
    n[r] = [];
  for (var o = 0, a = e; o < a.length; o++) {
    var i = a[o];
    n[i.firstCol].push(i);
  }
  return n;
}
function Ka(e, t) {
  var n = [];
  if (e) {
    for (var r = 0; r < t; r += 1)
      n[r] = {
        affectedInstances: e.affectedInstances,
        isEvent: e.isEvent,
        segs: []
      };
    for (var o = 0, a = e.segs; o < a.length; o++) {
      var i = a[o];
      n[i.row].segs.push(i);
    }
  } else
    for (var r = 0; r < t; r += 1)
      n[r] = null;
  return n;
}
var tm = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this.props, r = an(this.context, n.date);
    return m(Ns, { date: n.date, dateProfile: n.dateProfile, todayRange: n.todayRange, showDayNumber: n.showDayNumber, extraHookProps: n.extraHookProps, defaultContent: nm }, function(o, a) {
      return (a || n.forceDayTop) && m(
        "div",
        { className: "fc-daygrid-day-top", ref: o },
        m("a", v({ id: n.dayNumberId, className: "fc-daygrid-day-number" }, r), a || m(me, null, "\xA0"))
      );
    });
  }, t;
}(X);
function nm(e) {
  return e.dayNumberText;
}
var Vs = le({
  hour: "numeric",
  minute: "2-digit",
  omitZeroMinute: !0,
  meridiem: "narrow"
});
function Us(e) {
  var t = e.eventRange.ui.display;
  return t === "list-item" || t === "auto" && !e.eventRange.def.allDay && e.firstCol === e.lastCol && e.isStart && e.isEnd;
}
var Ls = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this.props;
    return m(Tv, v({}, n, { extraClassNames: ["fc-daygrid-event", "fc-daygrid-block-event", "fc-h-event"], defaultTimeFormat: Vs, defaultDisplayEventEnd: n.defaultDisplayEventEnd, disableResizing: !n.seg.eventRange.def.allDay }));
  }, t;
}(X), Bs = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context, a = o.options.eventTimeFormat || Vs, i = tn(r.seg, a, o, !0, r.defaultDisplayEventEnd);
    return m(pr, { seg: r.seg, timeText: i, defaultContent: rm, isDragging: r.isDragging, isResizing: !1, isDateSelecting: !1, isSelected: r.isSelected, isPast: r.isPast, isFuture: r.isFuture, isToday: r.isToday }, function(s, l, u, c) {
      return m("a", v({ className: ["fc-daygrid-event", "fc-daygrid-dot-event"].concat(l).join(" "), ref: s }, Ro(r.seg, o)), c);
    });
  }, t;
}(X);
function rm(e) {
  return m(
    me,
    null,
    m("div", { className: "fc-daygrid-event-dot", style: { borderColor: e.borderColor || e.backgroundColor } }),
    e.timeText && m("div", { className: "fc-event-time" }, e.timeText),
    m("div", { className: "fc-event-title" }, e.event.title || m(me, null, "\xA0"))
  );
}
var om = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.compileSegs = W(am), n;
  }
  return t.prototype.render = function() {
    var n = this.props, r = this.compileSegs(n.singlePlacements), o = r.allSegs, a = r.invisibleSegs;
    return m(Hv, { dateProfile: n.dateProfile, todayRange: n.todayRange, allDayDate: n.allDayDate, moreCnt: n.moreCnt, allSegs: o, hiddenSegs: a, alignmentElRef: n.alignmentElRef, alignGridTop: n.alignGridTop, extraDateSpan: n.extraDateSpan, popoverContent: function() {
      var i = (n.eventDrag ? n.eventDrag.affectedInstances : null) || (n.eventResize ? n.eventResize.affectedInstances : null) || {};
      return m(me, null, o.map(function(s) {
        var l = s.eventRange.instance.instanceId;
        return m("div", { className: "fc-daygrid-event-harness", key: l, style: {
          visibility: i[l] ? "hidden" : ""
        } }, Us(s) ? m(Bs, v({ seg: s, isDragging: !1, isSelected: l === n.eventSelection, defaultDisplayEventEnd: !1 }, xt(s, n.todayRange))) : m(Ls, v({ seg: s, isDragging: !1, isResizing: !1, isDateSelecting: !1, isSelected: l === n.eventSelection, defaultDisplayEventEnd: !1 }, xt(s, n.todayRange))));
      }));
    } }, function(i, s, l, u, c, d, f, p) {
      return m("a", v({ ref: i, className: ["fc-daygrid-more-link"].concat(s).join(" "), title: d, "aria-expanded": f, "aria-controls": p }, Gi(c)), u);
    });
  }, t;
}(X);
function am(e) {
  for (var t = [], n = [], r = 0, o = e; r < o.length; r++) {
    var a = o[r];
    t.push(a.seg), a.isVisible || n.push(a.seg);
  }
  return { allSegs: t, invisibleSegs: n };
}
var im = le({ week: "narrow" }), sm = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.rootElRef = Re(), n.state = {
      dayNumberId: tt()
    }, n.handleRootEl = function(r) {
      Ge(n.rootElRef, r), Ge(n.props.elRef, r);
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = n.context, o = n.props, a = n.state, i = n.rootElRef, s = o.date, l = o.dateProfile, u = an(r, s, "week");
    return m(Fs, { date: s, dateProfile: l, todayRange: o.todayRange, showDayNumber: o.showDayNumber, extraHookProps: o.extraHookProps, elRef: this.handleRootEl }, function(c, d, f, p) {
      return m(
        "td",
        v({ ref: c, role: "gridcell", className: ["fc-daygrid-day"].concat(d, o.extraClassNames || []).join(" ") }, f, o.extraDataAttrs, o.showDayNumber ? { "aria-labelledby": a.dayNumberId } : {}),
        m(
          "div",
          { className: "fc-daygrid-day-frame fc-scrollgrid-sync-inner", ref: o.innerElRef },
          o.showWeekNumber && m(Pv, { date: s, defaultFormat: im }, function(h, b, E, y) {
            return m("a", v({ ref: h, className: ["fc-daygrid-week-number"].concat(b).join(" ") }, u), y);
          }),
          !p && m(tm, { date: s, dateProfile: l, showDayNumber: o.showDayNumber, dayNumberId: a.dayNumberId, forceDayTop: o.forceDayTop, todayRange: o.todayRange, extraHookProps: o.extraHookProps }),
          m(
            "div",
            { className: "fc-daygrid-day-events", ref: o.fgContentElRef },
            o.fgContent,
            m(
              "div",
              { className: "fc-daygrid-day-bottom", style: { marginTop: o.moreMarginTop } },
              m(om, { allDayDate: s, singlePlacements: o.singlePlacements, moreCnt: o.moreCnt, alignmentElRef: i, alignGridTop: !o.showDayNumber, extraDateSpan: o.extraDateSpan, dateProfile: o.dateProfile, eventSelection: o.eventSelection, eventDrag: o.eventDrag, eventResize: o.eventResize, todayRange: o.todayRange })
            )
          ),
          m("div", { className: "fc-daygrid-day-bg" }, o.bgContent)
        )
      );
    });
  }, t;
}(bt);
function lm(e, t, n, r, o, a, i) {
  var s = new dm();
  s.allowReslicing = !0, s.strictOrder = r, t === !0 || n === !0 ? (s.maxCoord = a, s.hiddenConsumes = !0) : typeof t == "number" ? s.maxStackCnt = t : typeof n == "number" && (s.maxStackCnt = n, s.hiddenConsumes = !0);
  for (var l = [], u = [], c = 0; c < e.length; c += 1) {
    var d = e[c], f = d.eventRange.instance.instanceId, p = o[f];
    p != null ? l.push({
      index: c,
      thickness: p,
      span: {
        start: d.firstCol,
        end: d.lastCol + 1
      }
    }) : u.push(d);
  }
  for (var h = s.addSegs(l), b = s.toRects(), E = um(b, e, i), y = E.singleColPlacements, D = E.multiColPlacements, k = E.leftoverMargins, T = [], U = [], O = 0, R = u; O < R.length; O++) {
    var d = R[O];
    D[d.firstCol].push({
      seg: d,
      isVisible: !1,
      isAbsolute: !0,
      absoluteTop: 0,
      marginTop: 0
    });
    for (var A = d.firstCol; A <= d.lastCol; A += 1)
      y[A].push({
        seg: Mt(d, A, A + 1, i),
        isVisible: !1,
        isAbsolute: !1,
        absoluteTop: 0,
        marginTop: 0
      });
  }
  for (var A = 0; A < i.length; A += 1)
    T.push(0);
  for (var M = 0, I = h; M < I.length; M++) {
    var N = I[M], d = e[N.index], j = N.span;
    D[j.start].push({
      seg: Mt(d, j.start, j.end, i),
      isVisible: !1,
      isAbsolute: !0,
      absoluteTop: 0,
      marginTop: 0
    });
    for (var A = j.start; A < j.end; A += 1)
      T[A] += 1, y[A].push({
        seg: Mt(d, A, A + 1, i),
        isVisible: !1,
        isAbsolute: !1,
        absoluteTop: 0,
        marginTop: 0
      });
  }
  for (var A = 0; A < i.length; A += 1)
    U.push(k[A]);
  return { singleColPlacements: y, multiColPlacements: D, moreCnts: T, moreMarginTops: U };
}
function um(e, t, n) {
  for (var r = cm(e, n.length), o = [], a = [], i = [], s = 0; s < n.length; s += 1) {
    for (var l = r[s], u = [], c = 0, d = 0, f = 0, p = l; f < p.length; f++) {
      var h = p[f], b = t[h.index];
      u.push({
        seg: Mt(b, s, s + 1, n),
        isVisible: !0,
        isAbsolute: !1,
        absoluteTop: h.levelCoord,
        marginTop: h.levelCoord - c
      }), c = h.levelCoord + h.thickness;
    }
    var E = [];
    c = 0, d = 0;
    for (var y = 0, D = l; y < D.length; y++) {
      var h = D[y], b = t[h.index], k = h.span.end - h.span.start > 1, T = h.span.start === s;
      d += h.levelCoord - c, c = h.levelCoord + h.thickness, k ? (d += h.thickness, T && E.push({
        seg: Mt(b, h.span.start, h.span.end, n),
        isVisible: !0,
        isAbsolute: !0,
        absoluteTop: h.levelCoord,
        marginTop: 0
      })) : T && (E.push({
        seg: Mt(b, h.span.start, h.span.end, n),
        isVisible: !0,
        isAbsolute: !1,
        absoluteTop: h.levelCoord,
        marginTop: d
      }), d = 0);
    }
    o.push(u), a.push(E), i.push(d);
  }
  return { singleColPlacements: o, multiColPlacements: a, leftoverMargins: i };
}
function cm(e, t) {
  for (var n = [], r = 0; r < t; r += 1)
    n.push([]);
  for (var o = 0, a = e; o < a.length; o++)
    for (var i = a[o], r = i.span.start; r < i.span.end; r += 1)
      n[r].push(i);
  return n;
}
function Mt(e, t, n, r) {
  if (e.firstCol === t && e.lastCol === n - 1)
    return e;
  var o = e.eventRange, a = o.range, i = Ut(a, {
    start: r[t].date,
    end: ue(r[n - 1].date, 1)
  });
  return v(v({}, e), { firstCol: t, lastCol: n - 1, eventRange: {
    def: o.def,
    ui: v(v({}, o.ui), { durationEditable: !1 }),
    instance: o.instance,
    range: i
  }, isStart: e.isStart && i.start.valueOf() === a.start.valueOf(), isEnd: e.isEnd && i.end.valueOf() === a.end.valueOf() });
}
var dm = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.hiddenConsumes = !1, n.forceHidden = {}, n;
  }
  return t.prototype.addSegs = function(n) {
    for (var r = this, o = e.prototype.addSegs.call(this, n), a = this.entriesByLevel, i = function(l) {
      return !r.forceHidden[nn(l)];
    }, s = 0; s < a.length; s += 1)
      a[s] = a[s].filter(i);
    return o;
  }, t.prototype.handleInvalidInsertion = function(n, r, o) {
    var a = this, i = a.entriesByLevel, s = a.forceHidden, l = n.touchingEntry, u = n.touchingLevel, c = n.touchingLateral;
    if (this.hiddenConsumes && l) {
      var d = nn(l);
      if (!s[d])
        if (this.allowReslicing) {
          var f = v(v({}, l), { span: ks(l.span, r.span) }), p = nn(f);
          s[p] = !0, i[u][c] = f, this.splitEntry(l, r, o);
        } else
          s[d] = !0, o.push(l);
    }
    return e.prototype.handleInvalidInsertion.call(this, n, r, o);
  }, t;
}(Zh), Ws = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.cellElRefs = new dt(), n.frameElRefs = new dt(), n.fgElRefs = new dt(), n.segHarnessRefs = new dt(), n.rootElRef = Re(), n.state = {
      framePositions: null,
      maxContentHeight: null,
      eventInstanceHeights: {}
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = this, o = r.props, a = r.state, i = r.context, s = i.options, l = o.cells.length, u = Dn(o.businessHourSegs, l), c = Dn(o.bgEventSegs, l), d = Dn(this.getHighlightSegs(), l), f = Dn(this.getMirrorSegs(), l), p = lm(ts(o.fgEventSegs, s.eventOrder), o.dayMaxEvents, o.dayMaxEventRows, s.eventOrderStrict, a.eventInstanceHeights, a.maxContentHeight, o.cells), h = p.singleColPlacements, b = p.multiColPlacements, E = p.moreCnts, y = p.moreMarginTops, D = o.eventDrag && o.eventDrag.affectedInstances || o.eventResize && o.eventResize.affectedInstances || {};
    return m(
      "tr",
      { ref: this.rootElRef, role: "row" },
      o.renderIntro && o.renderIntro(),
      o.cells.map(function(k, T) {
        var U = n.renderFgSegs(T, o.forPrint ? h[T] : b[T], o.todayRange, D), O = n.renderFgSegs(T, fm(f[T], b), o.todayRange, {}, Boolean(o.eventDrag), Boolean(o.eventResize), !1);
        return m(sm, { key: k.key, elRef: n.cellElRefs.createRef(k.key), innerElRef: n.frameElRefs.createRef(k.key), dateProfile: o.dateProfile, date: k.date, showDayNumber: o.showDayNumbers, showWeekNumber: o.showWeekNumbers && T === 0, forceDayTop: o.showWeekNumbers, todayRange: o.todayRange, eventSelection: o.eventSelection, eventDrag: o.eventDrag, eventResize: o.eventResize, extraHookProps: k.extraHookProps, extraDataAttrs: k.extraDataAttrs, extraClassNames: k.extraClassNames, extraDateSpan: k.extraDateSpan, moreCnt: E[T], moreMarginTop: y[T], singlePlacements: h[T], fgContentElRef: n.fgElRefs.createRef(k.key), fgContent: m(
          me,
          null,
          m(me, null, U),
          m(me, null, O)
        ), bgContent: m(
          me,
          null,
          n.renderFillSegs(d[T], "highlight"),
          n.renderFillSegs(u[T], "non-business"),
          n.renderFillSegs(c[T], "bg-event")
        ) });
      })
    );
  }, t.prototype.componentDidMount = function() {
    this.updateSizing(!0);
  }, t.prototype.componentDidUpdate = function(n, r) {
    var o = this.props;
    this.updateSizing(!ze(n, o));
  }, t.prototype.getHighlightSegs = function() {
    var n = this.props;
    return n.eventDrag && n.eventDrag.segs.length ? n.eventDrag.segs : n.eventResize && n.eventResize.segs.length ? n.eventResize.segs : n.dateSelectionSegs;
  }, t.prototype.getMirrorSegs = function() {
    var n = this.props;
    return n.eventResize && n.eventResize.segs.length ? n.eventResize.segs : [];
  }, t.prototype.renderFgSegs = function(n, r, o, a, i, s, l) {
    var u = this.context, c = this.props.eventSelection, d = this.state.framePositions, f = this.props.cells.length === 1, p = i || s || l, h = [];
    if (d)
      for (var b = 0, E = r; b < E.length; b++) {
        var y = E[b], D = y.seg, k = D.eventRange.instance.instanceId, T = k + ":" + n, U = y.isVisible && !a[k], O = y.isAbsolute, R = "", A = "";
        O && (u.isRtl ? (A = 0, R = d.lefts[D.lastCol] - d.lefts[D.firstCol]) : (R = 0, A = d.rights[D.firstCol] - d.rights[D.lastCol])), h.push(m("div", { className: "fc-daygrid-event-harness" + (O ? " fc-daygrid-event-harness-abs" : ""), key: T, ref: p ? null : this.segHarnessRefs.createRef(T), style: {
          visibility: U ? "" : "hidden",
          marginTop: O ? "" : y.marginTop,
          top: O ? y.absoluteTop : "",
          left: R,
          right: A
        } }, Us(D) ? m(Bs, v({ seg: D, isDragging: i, isSelected: k === c, defaultDisplayEventEnd: f }, xt(D, o))) : m(Ls, v({ seg: D, isDragging: i, isResizing: s, isDateSelecting: l, isSelected: k === c, defaultDisplayEventEnd: f }, xt(D, o)))));
      }
    return h;
  }, t.prototype.renderFillSegs = function(n, r) {
    var o = this.context.isRtl, a = this.props.todayRange, i = this.state.framePositions, s = [];
    if (i)
      for (var l = 0, u = n; l < u.length; l++) {
        var c = u[l], d = o ? {
          right: 0,
          left: i.lefts[c.lastCol] - i.lefts[c.firstCol]
        } : {
          left: 0,
          right: i.rights[c.firstCol] - i.rights[c.lastCol]
        };
        s.push(m("div", { key: jf(c.eventRange), className: "fc-daygrid-bg-harness", style: d }, r === "bg-event" ? m(xv, v({ seg: c }, xt(c, a))) : Iv(r)));
      }
    return m.apply(void 0, re([me, {}], s));
  }, t.prototype.updateSizing = function(n) {
    var r = this, o = r.props, a = r.frameElRefs;
    if (!o.forPrint && o.clientWidth !== null) {
      if (n) {
        var i = o.cells.map(function(d) {
          return a.currentMap[d.key];
        });
        if (i.length) {
          var s = this.rootElRef.current;
          this.setState({
            framePositions: new ao(
              s,
              i,
              !0,
              !1
            )
          });
        }
      }
      var l = this.state.eventInstanceHeights, u = this.queryEventInstanceHeights(), c = o.dayMaxEvents === !0 || o.dayMaxEventRows === !0;
      this.safeSetState({
        eventInstanceHeights: v(v({}, l), u),
        maxContentHeight: c ? this.computeMaxContentHeight() : null
      });
    }
  }, t.prototype.queryEventInstanceHeights = function() {
    var n = this.segHarnessRefs.currentMap, r = {};
    for (var o in n) {
      var a = Math.round(n[o].getBoundingClientRect().height), i = o.split(":")[0];
      r[i] = Math.max(r[i] || 0, a);
    }
    return r;
  }, t.prototype.computeMaxContentHeight = function() {
    var n = this.props.cells[0].key, r = this.cellElRefs.currentMap[n], o = this.fgElRefs.currentMap[n];
    return r.getBoundingClientRect().bottom - o.getBoundingClientRect().top;
  }, t.prototype.getCellEls = function() {
    var n = this.cellElRefs.currentMap;
    return this.props.cells.map(function(r) {
      return n[r.key];
    });
  }, t;
}(bt);
Ws.addStateEquality({
  eventInstanceHeights: ze
});
function fm(e, t) {
  if (!e.length)
    return [];
  var n = pm(t);
  return e.map(function(r) {
    return {
      seg: r,
      isVisible: !0,
      isAbsolute: !0,
      absoluteTop: n[r.eventRange.instance.instanceId],
      marginTop: 0
    };
  });
}
function pm(e) {
  for (var t = {}, n = 0, r = e; n < r.length; n++)
    for (var o = r[n], a = 0, i = o; a < i.length; a++) {
      var s = i[a];
      t[s.seg.eventRange.instance.instanceId] = s.absoluteTop;
    }
  return t;
}
var hm = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.splitBusinessHourSegs = W(Cn), n.splitBgEventSegs = W(Cn), n.splitFgEventSegs = W(Cn), n.splitDateSelectionSegs = W(Cn), n.splitEventDrag = W(Ka), n.splitEventResize = W(Ka), n.rowRefs = new dt(), n.handleRootEl = function(r) {
      n.rootEl = r, r ? n.context.registerInteractiveComponent(n, {
        el: r,
        isHitComboAllowed: n.props.isHitComboAllowed
      }) : n.context.unregisterInteractiveComponent(n);
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = this.props, o = r.dateProfile, a = r.dayMaxEventRows, i = r.dayMaxEvents, s = r.expandRows, l = r.cells.length, u = this.splitBusinessHourSegs(r.businessHourSegs, l), c = this.splitBgEventSegs(r.bgEventSegs, l), d = this.splitFgEventSegs(r.fgEventSegs, l), f = this.splitDateSelectionSegs(r.dateSelectionSegs, l), p = this.splitEventDrag(r.eventDrag, l), h = this.splitEventResize(r.eventResize, l), b = i === !0 || a === !0;
    b && !s && (b = !1, a = null, i = null);
    var E = [
      "fc-daygrid-body",
      b ? "fc-daygrid-body-balanced" : "fc-daygrid-body-unbalanced",
      s ? "" : "fc-daygrid-body-natural"
    ];
    return m(
      "div",
      { className: E.join(" "), ref: this.handleRootEl, style: {
        width: r.clientWidth,
        minWidth: r.tableMinWidth
      } },
      m(Oo, { unit: "day" }, function(y, D) {
        return m(
          me,
          null,
          m(
            "table",
            { role: "presentation", className: "fc-scrollgrid-sync-table", style: {
              width: r.clientWidth,
              minWidth: r.tableMinWidth,
              height: s ? r.clientHeight : ""
            } },
            r.colGroupNode,
            m("tbody", { role: "presentation" }, r.cells.map(function(k, T) {
              return m(Ws, {
                ref: n.rowRefs.createRef(T),
                key: k.length ? k[0].date.toISOString() : T,
                showDayNumbers: l > 1,
                showWeekNumbers: r.showWeekNumbers,
                todayRange: D,
                dateProfile: o,
                cells: k,
                renderIntro: r.renderRowIntro,
                businessHourSegs: u[T],
                eventSelection: r.eventSelection,
                bgEventSegs: c[T].filter(vm),
                fgEventSegs: d[T],
                dateSelectionSegs: f[T],
                eventDrag: p[T],
                eventResize: h[T],
                dayMaxEvents: i,
                dayMaxEventRows: a,
                clientWidth: r.clientWidth,
                clientHeight: r.clientHeight,
                forPrint: r.forPrint
              });
            }))
          )
        );
      })
    );
  }, t.prototype.prepareHits = function() {
    this.rowPositions = new ao(
      this.rootEl,
      this.rowRefs.collect().map(function(n) {
        return n.getCellEls()[0];
      }),
      !1,
      !0
    ), this.colPositions = new ao(
      this.rootEl,
      this.rowRefs.currentMap[0].getCellEls(),
      !0,
      !1
    );
  }, t.prototype.queryHit = function(n, r) {
    var o = this, a = o.colPositions, i = o.rowPositions, s = a.leftToIndex(n), l = i.topToIndex(r);
    if (l != null && s != null) {
      var u = this.props.cells[l][s];
      return {
        dateProfile: this.props.dateProfile,
        dateSpan: v({ range: this.getCellRange(l, s), allDay: !0 }, u.extraDateSpan),
        dayEl: this.getCellEl(l, s),
        rect: {
          left: a.lefts[s],
          right: a.rights[s],
          top: i.tops[l],
          bottom: i.bottoms[l]
        },
        layer: 0
      };
    }
    return null;
  }, t.prototype.getCellEl = function(n, r) {
    return this.rowRefs.currentMap[n].getCellEls()[r];
  }, t.prototype.getCellRange = function(n, r) {
    var o = this.props.cells[n][r].date, a = ue(o, 1);
    return { start: o, end: a };
  }, t;
}(bt);
function vm(e) {
  return e.eventRange.def.allDay;
}
var mm = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.forceDayIfListItem = !0, n;
  }
  return t.prototype.sliceRange = function(n, r) {
    return r.sliceRange(n);
  }, t;
}(pv), gm = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.slicer = new mm(), n.tableRef = Re(), n;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context;
    return m(hm, v({ ref: this.tableRef }, this.slicer.sliceProps(r, r.dateProfile, r.nextDayThreshold, o, r.dayTableModel), { dateProfile: r.dateProfile, cells: r.dayTableModel.cells, colGroupNode: r.colGroupNode, tableMinWidth: r.tableMinWidth, renderRowIntro: r.renderRowIntro, dayMaxEvents: r.dayMaxEvents, dayMaxEventRows: r.dayMaxEventRows, showWeekNumbers: r.showWeekNumbers, expandRows: r.expandRows, headerAlignElRef: r.headerAlignElRef, clientWidth: r.clientWidth, clientHeight: r.clientHeight, forPrint: r.forPrint }));
  }, t;
}(bt), ym = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.buildDayTableModel = W(bm), n.headerRef = Re(), n.tableRef = Re(), n;
  }
  return t.prototype.render = function() {
    var n = this, r = this.context, o = r.options, a = r.dateProfileGenerator, i = this.props, s = this.buildDayTableModel(i.dateProfile, a), l = o.dayHeaders && m(uv, { ref: this.headerRef, dateProfile: i.dateProfile, dates: s.headerDates, datesRepDistinctDays: s.rowCnt === 1 }), u = function(c) {
      return m(gm, { ref: n.tableRef, dateProfile: i.dateProfile, dayTableModel: s, businessHours: i.businessHours, dateSelection: i.dateSelection, eventStore: i.eventStore, eventUiBases: i.eventUiBases, eventSelection: i.eventSelection, eventDrag: i.eventDrag, eventResize: i.eventResize, nextDayThreshold: o.nextDayThreshold, colGroupNode: c.tableColGroupNode, tableMinWidth: c.tableMinWidth, dayMaxEvents: o.dayMaxEvents, dayMaxEventRows: o.dayMaxEventRows, showWeekNumbers: o.weekNumbers, expandRows: !i.isHeightAuto, headerAlignElRef: n.headerElRef, clientWidth: c.clientWidth, clientHeight: c.clientHeight, forPrint: i.forPrint });
    };
    return o.dayMinWidth ? this.renderHScrollLayout(l, u, s.colCnt, o.dayMinWidth) : this.renderSimpleLayout(l, u);
  }, t;
}(em);
function bm(e, t) {
  var n = new dv(e.renderRange, t);
  return new fv(n, /year|month|week/.test(e.currentRangeUnit));
}
var Em = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.buildRenderRange = function(n, r, o) {
    var a = this.props.dateEnv, i = e.prototype.buildRenderRange.call(this, n, r, o), s = i.start, l = i.end, u;
    if (/^(year|month)$/.test(r) && (s = a.startOfWeek(s), u = a.startOfWeek(l), u.valueOf() !== l.valueOf() && (l = Da(u, 1))), this.props.monthMode && this.props.fixedWeekCount) {
      var c = Math.ceil(
        Bd(s, l)
      );
      l = Da(l, 6 - c);
    }
    return { start: s, end: l };
  }, t;
}(ys), _m = Ye({
  initialView: "dayGridMonth",
  views: {
    dayGrid: {
      component: ym,
      dateProfileGeneratorClass: Em
    },
    dayGridDay: {
      type: "dayGrid",
      duration: { days: 1 }
    },
    dayGridWeek: {
      type: "dayGrid",
      duration: { weeks: 1 }
    },
    dayGridMonth: {
      type: "dayGrid",
      duration: { months: 1 },
      monthMode: !0,
      fixedWeekCount: !0
    }
  }
});
/*!
FullCalendar v5.11.2
Docs & License: https://fullcalendar.io/
(c) 2022 Adam Shaw
*/
var Cm = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.state = {
      textId: tt()
    }, n;
  }
  return t.prototype.render = function() {
    var n = this.context, r = n.theme, o = n.dateEnv, a = n.options, i = n.viewApi, s = this.props, l = s.cellId, u = s.dayDate, c = s.todayRange, d = this.state.textId, f = Io(u, c), p = a.listDayFormat ? o.format(u, a.listDayFormat) : "", h = a.listDaySideFormat ? o.format(u, a.listDaySideFormat) : "", b = v({
      date: o.toDate(u),
      view: i,
      textId: d,
      text: p,
      sideText: h,
      navLinkAttrs: an(this.context, u),
      sideNavLinkAttrs: an(this.context, u, "day", !1)
    }, f), E = ["fc-list-day"].concat(dr(f, r));
    return m(qe, { hookProps: b, classNames: a.dayHeaderClassNames, content: a.dayHeaderContent, defaultContent: Dm, didMount: a.dayHeaderDidMount, willUnmount: a.dayHeaderWillUnmount }, function(y, D, k, T) {
      return m(
        "tr",
        { ref: y, className: E.concat(D).join(" "), "data-date": cr(u) },
        m(
          "th",
          { scope: "colgroup", colSpan: 3, id: l, "aria-labelledby": d },
          m("div", { className: "fc-list-day-cushion " + r.getClass("tableCellShaded"), ref: k }, T)
        )
      );
    });
  }, t;
}(X);
function Dm(e) {
  return m(
    me,
    null,
    e.text && m("a", v({ id: e.textId, className: "fc-list-day-text" }, e.navLinkAttrs), e.text),
    e.sideText && m("a", v({ "aria-hidden": !0, className: "fc-list-day-side-text" }, e.sideNavLinkAttrs), e.sideText)
  );
}
var Sm = le({
  hour: "numeric",
  minute: "2-digit",
  meridiem: "short"
}), wm = function(e) {
  H(t, e);
  function t() {
    return e !== null && e.apply(this, arguments) || this;
  }
  return t.prototype.render = function() {
    var n = this, r = n.props, o = n.context, a = r.seg, i = r.timeHeaderId, s = r.eventHeaderId, l = r.dateHeaderId, u = o.options.eventTimeFormat || Sm;
    return m(pr, {
      seg: a,
      timeText: "",
      disableDragging: !0,
      disableResizing: !0,
      defaultContent: function() {
        return Tm(a, o);
      },
      isPast: r.isPast,
      isFuture: r.isFuture,
      isToday: r.isToday,
      isSelected: r.isSelected,
      isDragging: r.isDragging,
      isResizing: r.isResizing,
      isDateSelecting: r.isDateSelecting
    }, function(c, d, f, p, h) {
      return m(
        "tr",
        { className: ["fc-list-event", h.event.url ? "fc-event-forced-url" : ""].concat(d).join(" "), ref: c },
        Rm(a, u, o, i, l),
        m(
          "td",
          { "aria-hidden": !0, className: "fc-list-event-graphic" },
          m("span", { className: "fc-list-event-dot", style: { borderColor: h.borderColor || h.backgroundColor } })
        ),
        m("td", { ref: f, headers: s + " " + l, className: "fc-list-event-title" }, p)
      );
    });
  }, t;
}(X);
function Tm(e, t) {
  var n = Ro(e, t);
  return m("a", v({}, n), e.eventRange.def.title);
}
function Rm(e, t, n, r, o) {
  var a = n.options;
  if (a.displayEventTime !== !1) {
    var i = e.eventRange.def, s = e.eventRange.instance, l = !1, u = void 0;
    if (i.allDay ? l = !0 : Hf(e.eventRange.range) ? e.isStart ? u = tn(e, t, n, null, null, s.range.start, e.end) : e.isEnd ? u = tn(e, t, n, null, null, e.start, s.range.end) : l = !0 : u = tn(e, t, n), l) {
      var c = {
        text: n.options.allDayText,
        view: n.viewApi
      };
      return m(qe, { hookProps: c, classNames: a.allDayClassNames, content: a.allDayContent, defaultContent: km, didMount: a.allDayDidMount, willUnmount: a.allDayWillUnmount }, function(d, f, p, h) {
        return m("td", { ref: d, headers: r + " " + o, className: ["fc-list-event-time"].concat(f).join(" ") }, h);
      });
    }
    return m("td", { className: "fc-list-event-time" }, u);
  }
  return null;
}
function km(e) {
  return e.text;
}
var Im = function(e) {
  H(t, e);
  function t() {
    var n = e !== null && e.apply(this, arguments) || this;
    return n.computeDateVars = W(Mm), n.eventStoreToSegs = W(n._eventStoreToSegs), n.state = {
      timeHeaderId: tt(),
      eventHeaderId: tt(),
      dateHeaderIdRoot: tt()
    }, n.setRootEl = function(r) {
      r ? n.context.registerInteractiveComponent(n, {
        el: r
      }) : n.context.unregisterInteractiveComponent(n);
    }, n;
  }
  return t.prototype.render = function() {
    var n = this, r = this, o = r.props, a = r.context, i = [
      "fc-list",
      a.theme.getClass("table"),
      a.options.stickyHeaderDates !== !1 ? "fc-list-sticky" : ""
    ], s = this.computeDateVars(o.dateProfile), l = s.dayDates, u = s.dayRanges, c = this.eventStoreToSegs(o.eventStore, o.eventUiBases, u);
    return m(Kn, { viewSpec: a.viewSpec, elRef: this.setRootEl }, function(d, f) {
      return m(
        "div",
        { ref: d, className: i.concat(f).join(" ") },
        m(Ps, { liquid: !o.isHeightAuto, overflowX: o.isHeightAuto ? "visible" : "hidden", overflowY: o.isHeightAuto ? "visible" : "auto" }, c.length > 0 ? n.renderSegList(c, l) : n.renderEmptyMessage())
      );
    });
  }, t.prototype.renderEmptyMessage = function() {
    var n = this.context, r = n.options, o = n.viewApi, a = {
      text: r.noEventsText,
      view: o
    };
    return m(qe, { hookProps: a, classNames: r.noEventsClassNames, content: r.noEventsContent, defaultContent: xm, didMount: r.noEventsDidMount, willUnmount: r.noEventsWillUnmount }, function(i, s, l, u) {
      return m(
        "div",
        { className: ["fc-list-empty"].concat(s).join(" "), ref: i },
        m("div", { className: "fc-list-empty-cushion", ref: l }, u)
      );
    });
  }, t.prototype.renderSegList = function(n, r) {
    var o = this.context, a = o.theme, i = o.options, s = this.state, l = s.timeHeaderId, u = s.eventHeaderId, c = s.dateHeaderIdRoot, d = Pm(n);
    return m(Oo, { unit: "day" }, function(f, p) {
      for (var h = [], b = 0; b < d.length; b += 1) {
        var E = d[b];
        if (E) {
          var y = cr(r[b]), D = c + "-" + y;
          h.push(m(Cm, { key: y, cellId: D, dayDate: r[b], todayRange: p })), E = ts(E, i.eventOrder);
          for (var k = 0, T = E; k < T.length; k++) {
            var U = T[k];
            h.push(m(wm, v({ key: y + ":" + U.eventRange.instance.instanceId, seg: U, isDragging: !1, isResizing: !1, isDateSelecting: !1, isSelected: !1, timeHeaderId: l, eventHeaderId: u, dateHeaderId: D }, xt(U, p, f))));
          }
        }
      }
      return m(
        "table",
        { className: "fc-list-table " + a.getClass("table") },
        m(
          "thead",
          null,
          m(
            "tr",
            null,
            m("th", { scope: "col", id: l }, i.timeHint),
            m("th", { scope: "col", "aria-hidden": !0 }),
            m("th", { scope: "col", id: u }, i.eventHint)
          )
        ),
        m("tbody", null, h)
      );
    });
  }, t.prototype._eventStoreToSegs = function(n, r, o) {
    return this.eventRangesToSegs(ro(n, r, this.props.dateProfile.activeRange, this.context.options.nextDayThreshold).fg, o);
  }, t.prototype.eventRangesToSegs = function(n, r) {
    for (var o = [], a = 0, i = n; a < i.length; a++) {
      var s = i[a];
      o.push.apply(o, this.eventRangeToSegs(s, r));
    }
    return o;
  }, t.prototype.eventRangeToSegs = function(n, r) {
    var o = this.context.dateEnv, a = this.context.options.nextDayThreshold, i = n.range, s = n.def.allDay, l, u, c, d = [];
    for (l = 0; l < r.length; l += 1)
      if (u = Ut(i, r[l]), u && (c = {
        component: this,
        eventRange: n,
        start: u.start,
        end: u.end,
        isStart: n.isStart && u.start.valueOf() === i.start.valueOf(),
        isEnd: n.isEnd && u.end.valueOf() === i.end.valueOf(),
        dayIndex: l
      }, d.push(c), !c.isEnd && !s && l + 1 < r.length && i.end < o.add(r[l + 1].start, a))) {
        c.end = i.end, c.isEnd = !0;
        break;
      }
    return d;
  }, t;
}(bt);
function xm(e) {
  return e.text;
}
function Mm(e) {
  for (var t = Q(e.renderRange.start), n = e.renderRange.end, r = [], o = []; t < n; )
    r.push(t), o.push({
      start: t,
      end: ue(t, 1)
    }), t = ue(t, 1);
  return { dayDates: r, dayRanges: o };
}
function Pm(e) {
  var t = [], n, r;
  for (n = 0; n < e.length; n += 1)
    r = e[n], (t[r.dayIndex] || (t[r.dayIndex] = [])).push(r);
  return t;
}
var Om = {
  listDayFormat: Ja,
  listDaySideFormat: Ja,
  noEventsClassNames: g,
  noEventsContent: g,
  noEventsDidMount: g,
  noEventsWillUnmount: g
};
function Ja(e) {
  return e === !1 ? null : le(e);
}
var Am = Ye({
  optionRefiners: Om,
  views: {
    list: {
      component: Im,
      buttonTextKey: "list",
      listDayFormat: { month: "long", day: "numeric", year: "numeric" }
    },
    listDay: {
      type: "list",
      duration: { days: 1 },
      listDayFormat: { weekday: "long" }
    },
    listWeek: {
      type: "list",
      duration: { weeks: 1 },
      listDayFormat: { weekday: "long" },
      listDaySideFormat: { month: "long", day: "numeric", year: "numeric" }
    },
    listMonth: {
      type: "list",
      duration: { month: 1 },
      listDaySideFormat: { weekday: "long" }
    },
    listYear: {
      type: "list",
      duration: { year: 1 },
      listDaySideFormat: { weekday: "long" }
    }
  }
});
const Pt = {
  googleApiKey: "AIzaSyBgCnBy-LbHAqbB8PbQhNID68vZ8f8_j_Q",
  googleCalendarIds: ["ualberta.ca_kdp9enkplai8s5ipu2efknjels@group.calendar.google.com", "c_1netqe2n16i5o9su69ft5c8f8c@group.calendar.google.com"],
  maxEvents: 10,
  initialView: "dayGridMonth"
}, Nm = sn("GoogleCalendarStore", {
  state: () => ({
    id: null,
    events: null,
    upcomingEvents: null,
    calendarIds: null
  }),
  actions: {
    loadEvents() {
      let e = "https://www.googleapis.com/calendar/v3/calendars/" + Pt.googleCalendarIds[0] + "/events?key=" + Pt.googleApiKey;
      fetch(e, {
        method: "GET"
      }).then((t) => t.json()).then((t) => {
        this.events = t.items, console.log("events" + this.events);
      }).catch((t) => {
        console.error("Load google api Error:", t);
      });
    },
    getUpcomingEvents() {
      var t;
      return (t = this.events) == null ? void 0 : t.filter((n) => this.checkCurrEvent(n));
    },
    checkCurrEvent(e) {
      var t, n;
      if (((t = e.start) == null ? void 0 : t.dateTime) && new Date((n = e.start) == null ? void 0 : n.dateTime) >= new Date())
        return e;
    }
  },
  getters: {
    getCalendarIds: (e) => (Pt.googleCalendarIds.map(function(t) {
      var r;
      let n = { googleCalendarId: t, className: "gcal-event" };
      (r = e.calendarIds) == null || r.push(n);
    }), e.calendarIds)
  }
}), Hm = /* @__PURE__ */ C("h3", { style: { color: "green" } }, "Google Calendar", -1), Fm = /* @__PURE__ */ C("h3", null, "Upcoming events", -1), Vm = /* @__PURE__ */ C("div", { id: "es-calendar" }, null, -1), jm = /* @__PURE__ */ K({
  __name: "App",
  props: {
    piniaInstance: null
  },
  setup(e) {
    const n = Nm(e.piniaInstance);
    return n.loadEvents(), he(() => n.getUpcomingEvents()), ti(() => {
      let r = document.getElementById("es-calendar"), o = [];
      Pt.googleCalendarIds.map(function(i) {
        let s = { googleCalendarId: i, className: "gcal-event" };
        o == null || o.push(s);
      }), new Wv(r, {
        plugins: [Qv, _m, Am],
        googleCalendarApiKey: Pt.googleApiKey,
        eventSources: o,
        initialView: Pt.initialView
      }).render();
    }), (r, o) => (S(), x(q, null, [
      Hm,
      Fm,
      Vm
    ], 64));
  }
});
export {
  Wm as FormBuilder,
  Bm as FormModels,
  $m as FormSubmission,
  jm as GoogleCalendar,
  zm as Login,
  Gm as WorkflowBuilder,
  ar as useFormBuilderStore,
  Ve as useFormSubmissionStore,
  Nm as useGoogleCalendarStore,
  Pc as useLoginStore,
  Hc as useWorkflowBuilderStore
};
