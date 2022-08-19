import { getCurrentInstance as kn, inject as lo, markRaw as Pe, ref as Dt, watch as xt, reactive as ao, effectScope as uo, isRef as Oe, isReactive as tn, toRef as Mt, toRaw as co, nextTick as cn, computed as Z, onUnmounted as fo, toRefs as fn, defineComponent as A, h as hn, resolveComponent as ye, openBlock as m, createElementBlock as g, unref as d, createElementVNode as h, toDisplayString as L, withDirectives as U, vModelText as z, createCommentVNode as N, Fragment as P, renderList as ie, createBlock as ne, createVNode as $, withCtx as ze, Transition as nn, normalizeClass as on, pushScopeId as St, popScopeId as Tt, createTextVNode as oe, vModelSelect as Cn, vModelRadio as ho, onMounted as mo } from "vue";
var Be = /* @__PURE__ */ ((e) => (e.ShortAnswer = "Short Answer", e.Paragraph = "Paragraph", e.RichText = "Rich Text", e))(Be || {}), It = /* @__PURE__ */ ((e) => (e.Date = "Date", e.DateTime = "Date Time", e.Decimal = "Decimal", e.Integer = "Integer", e.Email = "Email", e))(It || {}), Ot = /* @__PURE__ */ ((e) => (e.Checkboxes = "Checkboxes", e.DataList = "Data List", e.RadioButtons = "Radio Buttons", e.DropDown = "Drop Down", e))(Ot || {}), sn = /* @__PURE__ */ ((e) => (e.InfoSection = "Info Section", e))(sn || {});
const k = { ...Be, ...It, ...Ot, ...sn };
var kt = /* @__PURE__ */ ((e) => (e[e.None = 0] = "None", e[e.Optional = 1] = "Optional", e[e.Required = 2] = "Required", e))(kt || {});
const br = /* @__PURE__ */ Object.freeze(/* @__PURE__ */ Object.defineProperty({
  __proto__: null,
  TextType: Be,
  MonolingualFieldType: It,
  OptionFieldType: Ot,
  InfoSectionType: sn,
  FieldTypes: k,
  ExtensionType: kt
}, Symbol.toStringTag, { value: "Module" }));
var po = function() {
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
    for (var n = "", o = 0; o < t; o++)
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
}(), F = po, go = !1;
function rt(e, t, n) {
  return Array.isArray(e) ? (e.length = Math.max(e.length, t), e.splice(t, 1, n), n) : (e[t] = n, n);
}
function At(e, t) {
  if (Array.isArray(e)) {
    e.splice(t, 1);
    return;
  }
  delete e[t];
}
/*!
  * pinia v2.0.16
  * (c) 2022 Eduardo San Martin Morote
  * @license MIT
  */
let Ge;
const vt = (e) => Ge = e, vo = process.env.NODE_ENV !== "production" ? Symbol("pinia") : Symbol();
function Me(e) {
  return e && typeof e == "object" && Object.prototype.toString.call(e) === "[object Object]" && typeof e.toJSON != "function";
}
var Qe;
(function(e) {
  e.direct = "direct", e.patchObject = "patch object", e.patchFunction = "patch function";
})(Qe || (Qe = {}));
const He = typeof window < "u";
function $n(e, t) {
  for (const n in t) {
    const o = t[n];
    if (!(n in e))
      continue;
    const i = e[n];
    Me(i) && Me(o) && !Oe(o) && !tn(o) ? e[n] = $n(i, o) : e[n] = o;
  }
  return e;
}
const Pn = () => {
};
function mn(e, t, n, o = Pn) {
  e.push(t);
  const i = () => {
    const s = e.indexOf(t);
    s > -1 && (e.splice(s, 1), o());
  };
  return !n && kn() && fo(i), i;
}
function Ae(e, ...t) {
  e.slice().forEach((n) => {
    n(...t);
  });
}
function Gt(e, t) {
  for (const n in t) {
    if (!t.hasOwnProperty(n))
      continue;
    const o = t[n], i = e[n];
    Me(i) && Me(o) && e.hasOwnProperty(n) && !Oe(o) && !tn(o) ? e[n] = Gt(i, o) : e[n] = o;
  }
  return e;
}
const bo = process.env.NODE_ENV !== "production" ? Symbol("pinia:skipHydration") : Symbol();
function yo(e) {
  return !Me(e) || !e.hasOwnProperty(bo);
}
const { assign: pe } = Object;
function pn(e) {
  return !!(Oe(e) && e.effect);
}
function gn(e, t, n, o) {
  const { state: i, actions: s, getters: r } = t, l = n.state.value[e];
  let a;
  function u() {
    !l && (process.env.NODE_ENV === "production" || !o) && (n.state.value[e] = i ? i() : {});
    const c = process.env.NODE_ENV !== "production" && o ? fn(Dt(i ? i() : {}).value) : fn(n.state.value[e]);
    return pe(c, s, Object.keys(r || {}).reduce((f, b) => (process.env.NODE_ENV !== "production" && b in c && console.warn(`[\u{1F34D}]: A getter cannot have the same name as another state property. Rename one of them. Found with "${b}" in store "${e}".`), f[b] = Pe(Z(() => {
      vt(n);
      const T = n._s.get(e);
      return r[b].call(T, T);
    })), f), {}));
  }
  return a = Ht(e, u, t, n, o, !0), a.$reset = function() {
    const f = i ? i() : {};
    this.$patch((b) => {
      pe(b, f);
    });
  }, a;
}
function Ht(e, t, n = {}, o, i, s) {
  let r;
  const l = pe({ actions: {} }, n);
  if (process.env.NODE_ENV !== "production" && !o._e.active)
    throw new Error("Pinia destroyed");
  const a = {
    deep: !0
  };
  process.env.NODE_ENV !== "production" && !go && (a.onTrigger = (y) => {
    u ? T = y : u == !1 && !E._hotUpdating && (Array.isArray(T) ? T.push(y) : console.error("\u{1F34D} debuggerEvents should be an array. This is most likely an internal Pinia bug."));
  });
  let u, c, f = Pe([]), b = Pe([]), T;
  const S = o.state.value[e];
  !s && !S && (process.env.NODE_ENV === "production" || !i) && (o.state.value[e] = {});
  const I = Dt({});
  let Q;
  function de(y) {
    let v;
    u = c = !1, process.env.NODE_ENV !== "production" && (T = []), typeof y == "function" ? (y(o.state.value[e]), v = {
      type: Qe.patchFunction,
      storeId: e,
      events: T
    }) : (Gt(o.state.value[e], y), v = {
      type: Qe.patchObject,
      payload: y,
      storeId: e,
      events: T
    });
    const D = Q = Symbol();
    cn().then(() => {
      Q === D && (u = !0);
    }), c = !0, Ae(f, v, o.state.value[e]);
  }
  const X = process.env.NODE_ENV !== "production" ? () => {
    throw new Error(`\u{1F34D}: Store "${e}" is built using the setup syntax and does not implement $reset().`);
  } : Pn;
  function se() {
    r.stop(), f = [], b = [], o._s.delete(e);
  }
  function re(y, v) {
    return function() {
      vt(o);
      const D = Array.from(arguments), B = [], j = [];
      function we(G) {
        B.push(G);
      }
      function ke(G) {
        j.push(G);
      }
      Ae(b, {
        args: D,
        name: y,
        store: E,
        after: we,
        onError: ke
      });
      let ee;
      try {
        ee = v.apply(this && this.$id === e ? this : E, D);
      } catch (G) {
        throw Ae(j, G), G;
      }
      return ee instanceof Promise ? ee.then((G) => (Ae(B, G), G)).catch((G) => (Ae(j, G), Promise.reject(G))) : (Ae(B, ee), ee);
    };
  }
  const ce = /* @__PURE__ */ Pe({
    actions: {},
    getters: {},
    state: [],
    hotState: I
  }), fe = {
    _p: o,
    $id: e,
    $onAction: mn.bind(null, b),
    $patch: de,
    $reset: X,
    $subscribe(y, v = {}) {
      const D = mn(f, y, v.detached, () => B()), B = r.run(() => xt(() => o.state.value[e], (j) => {
        (v.flush === "sync" ? c : u) && y({
          storeId: e,
          type: Qe.direct,
          events: T
        }, j);
      }, pe({}, a, v)));
      return D;
    },
    $dispose: se
  }, E = ao(pe(process.env.NODE_ENV !== "production" && He ? {
    _customProperties: Pe(/* @__PURE__ */ new Set()),
    _hmrPayload: ce
  } : {}, fe));
  o._s.set(e, E);
  const K = o._e.run(() => (r = uo(), r.run(() => t())));
  for (const y in K) {
    const v = K[y];
    if (Oe(v) && !pn(v) || tn(v))
      process.env.NODE_ENV !== "production" && i ? rt(I.value, y, Mt(K, y)) : s || (S && yo(v) && (Oe(v) ? v.value = S[y] : Gt(v, S[y])), o.state.value[e][y] = v), process.env.NODE_ENV !== "production" && ce.state.push(y);
    else if (typeof v == "function") {
      const D = process.env.NODE_ENV !== "production" && i ? v : re(y, v);
      K[y] = D, process.env.NODE_ENV !== "production" && (ce.actions[y] = v), l.actions[y] = v;
    } else
      process.env.NODE_ENV !== "production" && pn(v) && (ce.getters[y] = s ? n.getters[y] : v, He && (K._getters || (K._getters = Pe([]))).push(y));
  }
  if (pe(E, K), pe(co(E), K), Object.defineProperty(E, "$state", {
    get: () => process.env.NODE_ENV !== "production" && i ? I.value : o.state.value[e],
    set: (y) => {
      if (process.env.NODE_ENV !== "production" && i)
        throw new Error("cannot set hotState");
      de((v) => {
        pe(v, y);
      });
    }
  }), process.env.NODE_ENV !== "production") {
    E._hotUpdate = Pe((v) => {
      E._hotUpdating = !0, v._hmrPayload.state.forEach((D) => {
        if (D in E.$state) {
          const B = v.$state[D], j = E.$state[D];
          typeof B == "object" && Me(B) && Me(j) ? $n(B, j) : v.$state[D] = j;
        }
        rt(E, D, Mt(v.$state, D));
      }), Object.keys(E.$state).forEach((D) => {
        D in v.$state || At(E, D);
      }), u = !1, c = !1, o.state.value[e] = Mt(v._hmrPayload, "hotState"), c = !0, cn().then(() => {
        u = !0;
      });
      for (const D in v._hmrPayload.actions) {
        const B = v[D];
        rt(E, D, re(D, B));
      }
      for (const D in v._hmrPayload.getters) {
        const B = v._hmrPayload.getters[D], j = s ? Z(() => (vt(o), B.call(E, E))) : B;
        rt(E, D, j);
      }
      Object.keys(E._hmrPayload.getters).forEach((D) => {
        D in v._hmrPayload.getters || At(E, D);
      }), Object.keys(E._hmrPayload.actions).forEach((D) => {
        D in v._hmrPayload.actions || At(E, D);
      }), E._hmrPayload = v._hmrPayload, E._getters = v._getters, E._hotUpdating = !1;
    });
    const y = {
      writable: !0,
      configurable: !0,
      enumerable: !1
    };
    He && ["_p", "_hmrPayload", "_getters", "_customProperties"].forEach((v) => {
      Object.defineProperty(E, v, {
        value: E[v],
        ...y
      });
    });
  }
  return o._p.forEach((y) => {
    if (process.env.NODE_ENV !== "production" && He) {
      const v = r.run(() => y({
        store: E,
        app: o._a,
        pinia: o,
        options: l
      }));
      Object.keys(v || {}).forEach((D) => E._customProperties.add(D)), pe(E, v);
    } else
      pe(E, r.run(() => y({
        store: E,
        app: o._a,
        pinia: o,
        options: l
      })));
  }), process.env.NODE_ENV !== "production" && E.$state && typeof E.$state == "object" && typeof E.$state.constructor == "function" && !E.$state.constructor.toString().includes("[native code]") && console.warn(`[\u{1F34D}]: The "state" must be a plain object. It cannot be
	state: () => new MyClass()
Found in store "${E.$id}".`), S && s && n.hydrate && n.hydrate(E.$state, S), u = !0, c = !0, E;
}
function Ct(e, t, n) {
  let o, i;
  const s = typeof t == "function";
  typeof e == "string" ? (o = e, i = s ? n : t) : (i = e, o = e.id);
  function r(l, a) {
    const u = kn();
    if (l = (process.env.NODE_ENV === "test" && Ge && Ge._testing ? null : l) || u && lo(vo), l && vt(l), process.env.NODE_ENV !== "production" && !Ge)
      throw new Error(`[\u{1F34D}]: getActivePinia was called with no active Pinia. Did you forget to install pinia?
	const pinia = createPinia()
	app.use(pinia)
This will fail in production.`);
    l = Ge, l._s.has(o) || (s ? Ht(o, t, i, l) : gn(o, i, l), process.env.NODE_ENV !== "production" && (r._pinia = l));
    const c = l._s.get(o);
    if (process.env.NODE_ENV !== "production" && a) {
      const f = "__hot:" + o, b = s ? Ht(f, t, i, l, !0) : gn(f, pe({}, i), l, !0);
      a._hotUpdate(b), delete l.state.value[f], l._s.delete(f);
    }
    if (process.env.NODE_ENV !== "production" && He && u && u.proxy && !a) {
      const f = u.proxy, b = "_pStores" in f ? f._pStores : f._pStores = {};
      b[o] = c;
    }
    return c;
  }
  return r.$id = o, r;
}
const $t = Ct("FormBuilderStore", {
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
      var o, i;
      if (!this.form) {
        console.error("Cannot save null form.");
        return;
      }
      const e = ((i = (o = this.form) == null ? void 0 : o.id) == null ? void 0 : i.toString()) === F.EMPTY;
      let t = "https://localhost:5020/api/forms", n = "";
      e ? (console.log("Saving new form."), this.form.id = F.create().toString(), n = "POST") : (console.log("Updating existing form."), t = `${t}/${this.form.id}`, n = "PUT"), fetch(t, {
        body: JSON.stringify(this.form),
        method: n,
        headers: {
          encType: "multipart/form-data",
          "Content-Type": "application/json"
        }
      }).then((s) => {
        if (s.ok)
          this.transientMessage = "The form saved successfully", this.transientMessageClass = "success";
        else
          switch (e && this.form && (this.form.id = F.EMPTY), this.transientMessageClass = "danger", s.status) {
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
      }).catch((s) => {
        e && this.form && (this.form.id = F.EMPTY), this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("Form Save API Error:", s);
      });
    }
  }
}), Ye = (e) => {
  const t = {
    id: F.create().toString(),
    values: []
  };
  return (typeof e == "string" ? [e] : e).forEach((o) => {
    t.values.push({
      id: F.create().toString(),
      lang: o
    });
  }), t;
}, Nn = (e) => {
  const t = {
    id: F.create().toString()
  };
  return e && (t.lang = e), t;
}, vn = () => F.create().toString(), Mn = (e, t, n) => {
  var i, s;
  let o;
  return t ? o = (i = e == null ? void 0 : e.values) == null ? void 0 : i.filter((r) => r.lang === t).map((r) => r.value) : o = (s = e == null ? void 0 : e.values) == null ? void 0 : s.map((r) => r.value), n ? o.join(n) : o;
}, _o = (e) => {
  const t = JSON.parse(JSON.stringify(e));
  return t.id = vn(), t.values.forEach((n) => {
    n.id = vn();
  }), t;
};
function An(e, t) {
  return {
    id: F.create().toString(),
    isExtendedInput: kt.None,
    optionText: t || Ye(e)
  };
}
const je = (e, t) => wo(e.optionText, t);
function wo(e, t) {
  var n, o, i;
  return t ? (o = (n = e == null ? void 0 : e.values) == null ? void 0 : n.filter((s) => s.lang === t).map((s) => s.value)) == null ? void 0 : o.at(0) : (i = e == null ? void 0 : e.values) == null ? void 0 : i.map((s) => s.value);
}
const rn = (e) => Object.values(Ot).map((t) => t).includes(e.type), Fn = (e) => Object.values(Be).map((t) => t).includes(e.type), Vn = (e) => Object.values(It).map((t) => t).includes(e.type), Eo = (e, t) => Mn(e.title, t)[0], Rn = (e, t) => Mn(e.description, t)[0], Do = (e, t) => {
  var o;
  const n = {
    id: F.create().toString(),
    fieldId: e.id
  };
  if (rn(e))
    n.selectedOptionIds = [], e.allowCustomOptionValues && (n.customOptionValues = []), (o = e.options) != null && o.find((i) => i.isExtendedInput) && (n.extendedOptionValues = []);
  else if (Fn(e)) {
    const i = typeof t == "string" ? [t] : t;
    n.multilingualTextValues = [Ye(i)];
  } else
    Vn(e) && (n.monolingualTextValues = [Nn(null)]);
  return n;
}, xo = (e, t) => {
  var o;
  const n = {
    id: F.EMPTY,
    formId: e.id,
    fieldData: []
  };
  return (o = e.fields) == null || o.forEach((i) => {
    const s = Do(i, t);
    n.fieldData.push(s);
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
function bn(e, t) {
  var n = Object.keys(e);
  if (Object.getOwnPropertySymbols) {
    var o = Object.getOwnPropertySymbols(e);
    t && (o = o.filter(function(i) {
      return Object.getOwnPropertyDescriptor(e, i).enumerable;
    })), n.push.apply(n, o);
  }
  return n;
}
function be(e) {
  for (var t = 1; t < arguments.length; t++) {
    var n = arguments[t] != null ? arguments[t] : {};
    t % 2 ? bn(Object(n), !0).forEach(function(o) {
      So(e, o, n[o]);
    }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(e, Object.getOwnPropertyDescriptors(n)) : bn(Object(n)).forEach(function(o) {
      Object.defineProperty(e, o, Object.getOwnPropertyDescriptor(n, o));
    });
  }
  return e;
}
function ct(e) {
  return typeof Symbol == "function" && typeof Symbol.iterator == "symbol" ? ct = function(t) {
    return typeof t;
  } : ct = function(t) {
    return t && typeof Symbol == "function" && t.constructor === Symbol && t !== Symbol.prototype ? "symbol" : typeof t;
  }, ct(e);
}
function So(e, t, n) {
  return t in e ? Object.defineProperty(e, t, {
    value: n,
    enumerable: !0,
    configurable: !0,
    writable: !0
  }) : e[t] = n, e;
}
function De() {
  return De = Object.assign || function(e) {
    for (var t = 1; t < arguments.length; t++) {
      var n = arguments[t];
      for (var o in n)
        Object.prototype.hasOwnProperty.call(n, o) && (e[o] = n[o]);
    }
    return e;
  }, De.apply(this, arguments);
}
function To(e, t) {
  if (e == null)
    return {};
  var n = {}, o = Object.keys(e), i, s;
  for (s = 0; s < o.length; s++)
    i = o[s], !(t.indexOf(i) >= 0) && (n[i] = e[i]);
  return n;
}
function Io(e, t) {
  if (e == null)
    return {};
  var n = To(e, t), o, i;
  if (Object.getOwnPropertySymbols) {
    var s = Object.getOwnPropertySymbols(e);
    for (i = 0; i < s.length; i++)
      o = s[i], !(t.indexOf(o) >= 0) && (!Object.prototype.propertyIsEnumerable.call(e, o) || (n[o] = e[o]));
  }
  return n;
}
var Oo = "1.14.0";
function Ee(e) {
  if (typeof window < "u" && window.navigator)
    return !!/* @__PURE__ */ navigator.userAgent.match(e);
}
var xe = Ee(/(?:Trident.*rv[ :]?11\.|msie|iemobile|Windows Phone)/i), it = Ee(/Edge/i), yn = Ee(/firefox/i), Ke = Ee(/safari/i) && !Ee(/chrome/i) && !Ee(/android/i), Ln = Ee(/iP(ad|od|hone)/i), ko = Ee(/chrome/i) && Ee(/android/i), jn = {
  capture: !1,
  passive: !1
};
function C(e, t, n) {
  e.addEventListener(t, n, !xe && jn);
}
function O(e, t, n) {
  e.removeEventListener(t, n, !xe && jn);
}
function bt(e, t) {
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
function Co(e) {
  return e.host && e !== document && e.host.nodeType ? e.host : e.parentNode;
}
function ge(e, t, n, o) {
  if (e) {
    n = n || document;
    do {
      if (t != null && (t[0] === ">" ? e.parentNode === n && bt(e, t) : bt(e, t)) || o && e === n)
        return e;
      if (e === n)
        break;
    } while (e = Co(e));
  }
  return null;
}
var _n = /\s+/g;
function le(e, t, n) {
  if (e && t)
    if (e.classList)
      e.classList[n ? "add" : "remove"](t);
    else {
      var o = (" " + e.className + " ").replace(_n, " ").replace(" " + t + " ", " ");
      e.className = (o + (n ? " " + t : "")).replace(_n, " ");
    }
}
function _(e, t, n) {
  var o = e && e.style;
  if (o) {
    if (n === void 0)
      return document.defaultView && document.defaultView.getComputedStyle ? n = document.defaultView.getComputedStyle(e, "") : e.currentStyle && (n = e.currentStyle), t === void 0 ? n : n[t];
    !(t in o) && t.indexOf("webkit") === -1 && (t = "-webkit-" + t), o[t] = n + (typeof n == "string" ? "" : "px");
  }
}
function Ue(e, t) {
  var n = "";
  if (typeof e == "string")
    n = e;
  else
    do {
      var o = _(e, "transform");
      o && o !== "none" && (n = o + " " + n);
    } while (!t && (e = e.parentNode));
  var i = window.DOMMatrix || window.WebKitCSSMatrix || window.CSSMatrix || window.MSCSSMatrix;
  return i && new i(n);
}
function Un(e, t, n) {
  if (e) {
    var o = e.getElementsByTagName(t), i = 0, s = o.length;
    if (n)
      for (; i < s; i++)
        n(o[i], i);
    return o;
  }
  return [];
}
function ve() {
  var e = document.scrollingElement;
  return e || document.documentElement;
}
function W(e, t, n, o, i) {
  if (!(!e.getBoundingClientRect && e !== window)) {
    var s, r, l, a, u, c, f;
    if (e !== window && e.parentNode && e !== ve() ? (s = e.getBoundingClientRect(), r = s.top, l = s.left, a = s.bottom, u = s.right, c = s.height, f = s.width) : (r = 0, l = 0, a = window.innerHeight, u = window.innerWidth, c = window.innerHeight, f = window.innerWidth), (t || n) && e !== window && (i = i || e.parentNode, !xe))
      do
        if (i && i.getBoundingClientRect && (_(i, "transform") !== "none" || n && _(i, "position") !== "static")) {
          var b = i.getBoundingClientRect();
          r -= b.top + parseInt(_(i, "border-top-width")), l -= b.left + parseInt(_(i, "border-left-width")), a = r + s.height, u = l + s.width;
          break;
        }
      while (i = i.parentNode);
    if (o && e !== window) {
      var T = Ue(i || e), S = T && T.a, I = T && T.d;
      T && (r /= I, l /= S, f /= S, c /= I, a = r + c, u = l + f);
    }
    return {
      top: r,
      left: l,
      bottom: a,
      right: u,
      width: f,
      height: c
    };
  }
}
function wn(e, t, n) {
  for (var o = Ie(e, !0), i = W(e)[t]; o; ) {
    var s = W(o)[n], r = void 0;
    if (n === "top" || n === "left" ? r = i >= s : r = i <= s, !r)
      return o;
    if (o === ve())
      break;
    o = Ie(o, !1);
  }
  return !1;
}
function We(e, t, n, o) {
  for (var i = 0, s = 0, r = e.children; s < r.length; ) {
    if (r[s].style.display !== "none" && r[s] !== w.ghost && (o || r[s] !== w.dragged) && ge(r[s], n.draggable, e, !1)) {
      if (i === t)
        return r[s];
      i++;
    }
    s++;
  }
  return null;
}
function ln(e, t) {
  for (var n = e.lastElementChild; n && (n === w.ghost || _(n, "display") === "none" || t && !bt(n, t)); )
    n = n.previousElementSibling;
  return n || null;
}
function he(e, t) {
  var n = 0;
  if (!e || !e.parentNode)
    return -1;
  for (; e = e.previousElementSibling; )
    e.nodeName.toUpperCase() !== "TEMPLATE" && e !== w.clone && (!t || bt(e, t)) && n++;
  return n;
}
function En(e) {
  var t = 0, n = 0, o = ve();
  if (e)
    do {
      var i = Ue(e), s = i.a, r = i.d;
      t += e.scrollLeft * s, n += e.scrollTop * r;
    } while (e !== o && (e = e.parentNode));
  return [t, n];
}
function $o(e, t) {
  for (var n in e)
    if (!!e.hasOwnProperty(n)) {
      for (var o in t)
        if (t.hasOwnProperty(o) && t[o] === e[n][o])
          return Number(n);
    }
  return -1;
}
function Ie(e, t) {
  if (!e || !e.getBoundingClientRect)
    return ve();
  var n = e, o = !1;
  do
    if (n.clientWidth < n.scrollWidth || n.clientHeight < n.scrollHeight) {
      var i = _(n);
      if (n.clientWidth < n.scrollWidth && (i.overflowX == "auto" || i.overflowX == "scroll") || n.clientHeight < n.scrollHeight && (i.overflowY == "auto" || i.overflowY == "scroll")) {
        if (!n.getBoundingClientRect || n === document.body)
          return ve();
        if (o || t)
          return n;
        o = !0;
      }
    }
  while (n = n.parentNode);
  return ve();
}
function Po(e, t) {
  if (e && t)
    for (var n in t)
      t.hasOwnProperty(n) && (e[n] = t[n]);
  return e;
}
function Ft(e, t) {
  return Math.round(e.top) === Math.round(t.top) && Math.round(e.left) === Math.round(t.left) && Math.round(e.height) === Math.round(t.height) && Math.round(e.width) === Math.round(t.width);
}
var et;
function Bn(e, t) {
  return function() {
    if (!et) {
      var n = arguments, o = this;
      n.length === 1 ? e.call(o, n[0]) : e.apply(o, n), et = setTimeout(function() {
        et = void 0;
      }, t);
    }
  };
}
function No() {
  clearTimeout(et), et = void 0;
}
function Yn(e, t, n) {
  e.scrollLeft += t, e.scrollTop += n;
}
function Wn(e) {
  var t = window.Polymer, n = window.jQuery || window.Zepto;
  return t && t.dom ? t.dom(e).cloneNode(!0) : n ? n(e).clone(!0)[0] : e.cloneNode(!0);
}
var ue = "Sortable" + new Date().getTime();
function Mo() {
  var e = [], t;
  return {
    captureAnimationState: function() {
      if (e = [], !!this.options.animation) {
        var o = [].slice.call(this.el.children);
        o.forEach(function(i) {
          if (!(_(i, "display") === "none" || i === w.ghost)) {
            e.push({
              target: i,
              rect: W(i)
            });
            var s = be({}, e[e.length - 1].rect);
            if (i.thisAnimationDuration) {
              var r = Ue(i, !0);
              r && (s.top -= r.f, s.left -= r.e);
            }
            i.fromRect = s;
          }
        });
      }
    },
    addAnimationState: function(o) {
      e.push(o);
    },
    removeAnimationState: function(o) {
      e.splice($o(e, {
        target: o
      }), 1);
    },
    animateAll: function(o) {
      var i = this;
      if (!this.options.animation) {
        clearTimeout(t), typeof o == "function" && o();
        return;
      }
      var s = !1, r = 0;
      e.forEach(function(l) {
        var a = 0, u = l.target, c = u.fromRect, f = W(u), b = u.prevFromRect, T = u.prevToRect, S = l.rect, I = Ue(u, !0);
        I && (f.top -= I.f, f.left -= I.e), u.toRect = f, u.thisAnimationDuration && Ft(b, f) && !Ft(c, f) && (S.top - f.top) / (S.left - f.left) === (c.top - f.top) / (c.left - f.left) && (a = Fo(S, b, T, i.options)), Ft(f, c) || (u.prevFromRect = c, u.prevToRect = f, a || (a = i.options.animation), i.animate(u, S, f, a)), a && (s = !0, r = Math.max(r, a), clearTimeout(u.animationResetTimer), u.animationResetTimer = setTimeout(function() {
          u.animationTime = 0, u.prevFromRect = null, u.fromRect = null, u.prevToRect = null, u.thisAnimationDuration = null;
        }, a), u.thisAnimationDuration = a);
      }), clearTimeout(t), s ? t = setTimeout(function() {
        typeof o == "function" && o();
      }, r) : typeof o == "function" && o(), e = [];
    },
    animate: function(o, i, s, r) {
      if (r) {
        _(o, "transition", ""), _(o, "transform", "");
        var l = Ue(this.el), a = l && l.a, u = l && l.d, c = (i.left - s.left) / (a || 1), f = (i.top - s.top) / (u || 1);
        o.animatingX = !!c, o.animatingY = !!f, _(o, "transform", "translate3d(" + c + "px," + f + "px,0)"), this.forRepaintDummy = Ao(o), _(o, "transition", "transform " + r + "ms" + (this.options.easing ? " " + this.options.easing : "")), _(o, "transform", "translate3d(0,0,0)"), typeof o.animated == "number" && clearTimeout(o.animated), o.animated = setTimeout(function() {
          _(o, "transition", ""), _(o, "transform", ""), o.animated = !1, o.animatingX = !1, o.animatingY = !1;
        }, r);
      }
    }
  };
}
function Ao(e) {
  return e.offsetWidth;
}
function Fo(e, t, n, o) {
  return Math.sqrt(Math.pow(t.top - e.top, 2) + Math.pow(t.left - e.left, 2)) / Math.sqrt(Math.pow(t.top - n.top, 2) + Math.pow(t.left - n.left, 2)) * o.animation;
}
var Fe = [], Vt = {
  initializeByDefault: !0
}, st = {
  mount: function(t) {
    for (var n in Vt)
      Vt.hasOwnProperty(n) && !(n in t) && (t[n] = Vt[n]);
    Fe.forEach(function(o) {
      if (o.pluginName === t.pluginName)
        throw "Sortable: Cannot mount plugin ".concat(t.pluginName, " more than once");
    }), Fe.push(t);
  },
  pluginEvent: function(t, n, o) {
    var i = this;
    this.eventCanceled = !1, o.cancel = function() {
      i.eventCanceled = !0;
    };
    var s = t + "Global";
    Fe.forEach(function(r) {
      !n[r.pluginName] || (n[r.pluginName][s] && n[r.pluginName][s](be({
        sortable: n
      }, o)), n.options[r.pluginName] && n[r.pluginName][t] && n[r.pluginName][t](be({
        sortable: n
      }, o)));
    });
  },
  initializePlugins: function(t, n, o, i) {
    Fe.forEach(function(l) {
      var a = l.pluginName;
      if (!(!t.options[a] && !l.initializeByDefault)) {
        var u = new l(t, n, t.options);
        u.sortable = t, u.options = t.options, t[a] = u, De(o, u.defaults);
      }
    });
    for (var s in t.options)
      if (!!t.options.hasOwnProperty(s)) {
        var r = this.modifyOption(t, s, t.options[s]);
        typeof r < "u" && (t.options[s] = r);
      }
  },
  getEventProperties: function(t, n) {
    var o = {};
    return Fe.forEach(function(i) {
      typeof i.eventProperties == "function" && De(o, i.eventProperties.call(n[i.pluginName], t));
    }), o;
  },
  modifyOption: function(t, n, o) {
    var i;
    return Fe.forEach(function(s) {
      !t[s.pluginName] || s.optionListeners && typeof s.optionListeners[n] == "function" && (i = s.optionListeners[n].call(t[s.pluginName], o));
    }), i;
  }
};
function Vo(e) {
  var t = e.sortable, n = e.rootEl, o = e.name, i = e.targetEl, s = e.cloneEl, r = e.toEl, l = e.fromEl, a = e.oldIndex, u = e.newIndex, c = e.oldDraggableIndex, f = e.newDraggableIndex, b = e.originalEvent, T = e.putSortable, S = e.extraEventProperties;
  if (t = t || n && n[ue], !!t) {
    var I, Q = t.options, de = "on" + o.charAt(0).toUpperCase() + o.substr(1);
    window.CustomEvent && !xe && !it ? I = new CustomEvent(o, {
      bubbles: !0,
      cancelable: !0
    }) : (I = document.createEvent("Event"), I.initEvent(o, !0, !0)), I.to = r || n, I.from = l || n, I.item = i || n, I.clone = s, I.oldIndex = a, I.newIndex = u, I.oldDraggableIndex = c, I.newDraggableIndex = f, I.originalEvent = b, I.pullMode = T ? T.lastPutMode : void 0;
    var X = be(be({}, S), st.getEventProperties(o, t));
    for (var se in X)
      I[se] = X[se];
    n && n.dispatchEvent(I), Q[de] && Q[de].call(t, I);
  }
}
var Ro = ["evt"], te = function(t, n) {
  var o = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : {}, i = o.evt, s = Io(o, Ro);
  st.pluginEvent.bind(w)(t, n, be({
    dragEl: p,
    parentEl: V,
    ghostEl: x,
    rootEl: M,
    nextEl: Ne,
    lastDownEl: ft,
    cloneEl: R,
    cloneHidden: Te,
    dragStarted: qe,
    putSortable: H,
    activeSortable: w.active,
    originalEvent: i,
    oldIndex: Le,
    oldDraggableIndex: tt,
    newIndex: ae,
    newDraggableIndex: Se,
    hideGhostForTarget: Hn,
    unhideGhostForTarget: qn,
    cloneNowHidden: function() {
      Te = !0;
    },
    cloneNowShown: function() {
      Te = !1;
    },
    dispatchSortableEvent: function(l) {
      J({
        sortable: n,
        name: l,
        originalEvent: i
      });
    }
  }, s));
};
function J(e) {
  Vo(be({
    putSortable: H,
    cloneEl: R,
    targetEl: p,
    rootEl: M,
    oldIndex: Le,
    oldDraggableIndex: tt,
    newIndex: ae,
    newDraggableIndex: Se
  }, e));
}
var p, V, x, M, Ne, ft, R, Te, Le, ae, tt, Se, lt, H, Re = !1, yt = !1, _t = [], Ce, me, Rt, Lt, Dn, xn, qe, Ve, nt, ot = !1, at = !1, ht, q, jt = [], qt = !1, wt = [], Pt = typeof document < "u", ut = Ln, Sn = it || xe ? "cssFloat" : "float", Lo = Pt && !ko && !Ln && "draggable" in document.createElement("div"), zn = function() {
  if (!!Pt) {
    if (xe)
      return !1;
    var e = document.createElement("x");
    return e.style.cssText = "pointer-events:auto", e.style.pointerEvents === "auto";
  }
}(), Xn = function(t, n) {
  var o = _(t), i = parseInt(o.width) - parseInt(o.paddingLeft) - parseInt(o.paddingRight) - parseInt(o.borderLeftWidth) - parseInt(o.borderRightWidth), s = We(t, 0, n), r = We(t, 1, n), l = s && _(s), a = r && _(r), u = l && parseInt(l.marginLeft) + parseInt(l.marginRight) + W(s).width, c = a && parseInt(a.marginLeft) + parseInt(a.marginRight) + W(r).width;
  if (o.display === "flex")
    return o.flexDirection === "column" || o.flexDirection === "column-reverse" ? "vertical" : "horizontal";
  if (o.display === "grid")
    return o.gridTemplateColumns.split(" ").length <= 1 ? "vertical" : "horizontal";
  if (s && l.float && l.float !== "none") {
    var f = l.float === "left" ? "left" : "right";
    return r && (a.clear === "both" || a.clear === f) ? "vertical" : "horizontal";
  }
  return s && (l.display === "block" || l.display === "flex" || l.display === "table" || l.display === "grid" || u >= i && o[Sn] === "none" || r && o[Sn] === "none" && u + c > i) ? "vertical" : "horizontal";
}, jo = function(t, n, o) {
  var i = o ? t.left : t.top, s = o ? t.right : t.bottom, r = o ? t.width : t.height, l = o ? n.left : n.top, a = o ? n.right : n.bottom, u = o ? n.width : n.height;
  return i === l || s === a || i + r / 2 === l + u / 2;
}, Uo = function(t, n) {
  var o;
  return _t.some(function(i) {
    var s = i[ue].options.emptyInsertThreshold;
    if (!(!s || ln(i))) {
      var r = W(i), l = t >= r.left - s && t <= r.right + s, a = n >= r.top - s && n <= r.bottom + s;
      if (l && a)
        return o = i;
    }
  }), o;
}, Gn = function(t) {
  function n(s, r) {
    return function(l, a, u, c) {
      var f = l.options.group.name && a.options.group.name && l.options.group.name === a.options.group.name;
      if (s == null && (r || f))
        return !0;
      if (s == null || s === !1)
        return !1;
      if (r && s === "clone")
        return s;
      if (typeof s == "function")
        return n(s(l, a, u, c), r)(l, a, u, c);
      var b = (r ? l : a).options.group.name;
      return s === !0 || typeof s == "string" && s === b || s.join && s.indexOf(b) > -1;
    };
  }
  var o = {}, i = t.group;
  (!i || ct(i) != "object") && (i = {
    name: i
  }), o.name = i.name, o.checkPull = n(i.pull, !0), o.checkPut = n(i.put), o.revertClone = i.revertClone, t.group = o;
}, Hn = function() {
  !zn && x && _(x, "display", "none");
}, qn = function() {
  !zn && x && _(x, "display", "");
};
Pt && document.addEventListener("click", function(e) {
  if (yt)
    return e.preventDefault(), e.stopPropagation && e.stopPropagation(), e.stopImmediatePropagation && e.stopImmediatePropagation(), yt = !1, !1;
}, !0);
var $e = function(t) {
  if (p) {
    t = t.touches ? t.touches[0] : t;
    var n = Uo(t.clientX, t.clientY);
    if (n) {
      var o = {};
      for (var i in t)
        t.hasOwnProperty(i) && (o[i] = t[i]);
      o.target = o.rootEl = n, o.preventDefault = void 0, o.stopPropagation = void 0, n[ue]._onDragOver(o);
    }
  }
}, Bo = function(t) {
  p && p.parentNode[ue]._isOutsideThisEl(t.target);
};
function w(e, t) {
  if (!(e && e.nodeType && e.nodeType === 1))
    throw "Sortable: `el` must be an HTMLElement, not ".concat({}.toString.call(e));
  this.el = e, this.options = t = De({}, t), e[ue] = this;
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
      return Xn(e, this.options);
    },
    ghostClass: "sortable-ghost",
    chosenClass: "sortable-chosen",
    dragClass: "sortable-drag",
    ignore: "a, img",
    filter: null,
    preventOnFilter: !0,
    animation: 0,
    easing: null,
    setData: function(r, l) {
      r.setData("Text", l.textContent);
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
    supportPointer: w.supportPointer !== !1 && "PointerEvent" in window && !Ke,
    emptyInsertThreshold: 5
  };
  st.initializePlugins(this, e, n);
  for (var o in n)
    !(o in t) && (t[o] = n[o]);
  Gn(t);
  for (var i in this)
    i.charAt(0) === "_" && typeof this[i] == "function" && (this[i] = this[i].bind(this));
  this.nativeDraggable = t.forceFallback ? !1 : Lo, this.nativeDraggable && (this.options.touchStartThreshold = 1), t.supportPointer ? C(e, "pointerdown", this._onTapStart) : (C(e, "mousedown", this._onTapStart), C(e, "touchstart", this._onTapStart)), this.nativeDraggable && (C(e, "dragover", this), C(e, "dragenter", this)), _t.push(this.el), t.store && t.store.get && this.sort(t.store.get(this) || []), De(this, Mo());
}
w.prototype = {
  constructor: w,
  _isOutsideThisEl: function(t) {
    !this.el.contains(t) && t !== this.el && (Ve = null);
  },
  _getDirection: function(t, n) {
    return typeof this.options.direction == "function" ? this.options.direction.call(this, t, n, p) : this.options.direction;
  },
  _onTapStart: function(t) {
    if (!!t.cancelable) {
      var n = this, o = this.el, i = this.options, s = i.preventOnFilter, r = t.type, l = t.touches && t.touches[0] || t.pointerType && t.pointerType === "touch" && t, a = (l || t).target, u = t.target.shadowRoot && (t.path && t.path[0] || t.composedPath && t.composedPath()[0]) || a, c = i.filter;
      if (Jo(o), !p && !(/mousedown|pointerdown/.test(r) && t.button !== 0 || i.disabled) && !u.isContentEditable && !(!this.nativeDraggable && Ke && a && a.tagName.toUpperCase() === "SELECT") && (a = ge(a, i.draggable, o, !1), !(a && a.animated) && ft !== a)) {
        if (Le = he(a), tt = he(a, i.draggable), typeof c == "function") {
          if (c.call(this, t, a, this)) {
            J({
              sortable: n,
              rootEl: u,
              name: "filter",
              targetEl: a,
              toEl: o,
              fromEl: o
            }), te("filter", n, {
              evt: t
            }), s && t.cancelable && t.preventDefault();
            return;
          }
        } else if (c && (c = c.split(",").some(function(f) {
          if (f = ge(u, f.trim(), o, !1), f)
            return J({
              sortable: n,
              rootEl: f,
              name: "filter",
              targetEl: a,
              fromEl: o,
              toEl: o
            }), te("filter", n, {
              evt: t
            }), !0;
        }), c)) {
          s && t.cancelable && t.preventDefault();
          return;
        }
        i.handle && !ge(u, i.handle, o, !1) || this._prepareDragStart(t, l, a);
      }
    }
  },
  _prepareDragStart: function(t, n, o) {
    var i = this, s = i.el, r = i.options, l = s.ownerDocument, a;
    if (o && !p && o.parentNode === s) {
      var u = W(o);
      if (M = s, p = o, V = p.parentNode, Ne = p.nextSibling, ft = o, lt = r.group, w.dragged = p, Ce = {
        target: p,
        clientX: (n || t).clientX,
        clientY: (n || t).clientY
      }, Dn = Ce.clientX - u.left, xn = Ce.clientY - u.top, this._lastX = (n || t).clientX, this._lastY = (n || t).clientY, p.style["will-change"] = "all", a = function() {
        if (te("delayEnded", i, {
          evt: t
        }), w.eventCanceled) {
          i._onDrop();
          return;
        }
        i._disableDelayedDragEvents(), !yn && i.nativeDraggable && (p.draggable = !0), i._triggerDragStart(t, n), J({
          sortable: i,
          name: "choose",
          originalEvent: t
        }), le(p, r.chosenClass, !0);
      }, r.ignore.split(",").forEach(function(c) {
        Un(p, c.trim(), Ut);
      }), C(l, "dragover", $e), C(l, "mousemove", $e), C(l, "touchmove", $e), C(l, "mouseup", i._onDrop), C(l, "touchend", i._onDrop), C(l, "touchcancel", i._onDrop), yn && this.nativeDraggable && (this.options.touchStartThreshold = 4, p.draggable = !0), te("delayStart", this, {
        evt: t
      }), r.delay && (!r.delayOnTouchOnly || n) && (!this.nativeDraggable || !(it || xe))) {
        if (w.eventCanceled) {
          this._onDrop();
          return;
        }
        C(l, "mouseup", i._disableDelayedDrag), C(l, "touchend", i._disableDelayedDrag), C(l, "touchcancel", i._disableDelayedDrag), C(l, "mousemove", i._delayedDragTouchMoveHandler), C(l, "touchmove", i._delayedDragTouchMoveHandler), r.supportPointer && C(l, "pointermove", i._delayedDragTouchMoveHandler), i._dragStartTimer = setTimeout(a, r.delay);
      } else
        a();
    }
  },
  _delayedDragTouchMoveHandler: function(t) {
    var n = t.touches ? t.touches[0] : t;
    Math.max(Math.abs(n.clientX - this._lastX), Math.abs(n.clientY - this._lastY)) >= Math.floor(this.options.touchStartThreshold / (this.nativeDraggable && window.devicePixelRatio || 1)) && this._disableDelayedDrag();
  },
  _disableDelayedDrag: function() {
    p && Ut(p), clearTimeout(this._dragStartTimer), this._disableDelayedDragEvents();
  },
  _disableDelayedDragEvents: function() {
    var t = this.el.ownerDocument;
    O(t, "mouseup", this._disableDelayedDrag), O(t, "touchend", this._disableDelayedDrag), O(t, "touchcancel", this._disableDelayedDrag), O(t, "mousemove", this._delayedDragTouchMoveHandler), O(t, "touchmove", this._delayedDragTouchMoveHandler), O(t, "pointermove", this._delayedDragTouchMoveHandler);
  },
  _triggerDragStart: function(t, n) {
    n = n || t.pointerType == "touch" && t, !this.nativeDraggable || n ? this.options.supportPointer ? C(document, "pointermove", this._onTouchMove) : n ? C(document, "touchmove", this._onTouchMove) : C(document, "mousemove", this._onTouchMove) : (C(p, "dragend", this), C(M, "dragstart", this._onDragStart));
    try {
      document.selection ? mt(function() {
        document.selection.empty();
      }) : window.getSelection().removeAllRanges();
    } catch {
    }
  },
  _dragStarted: function(t, n) {
    if (Re = !1, M && p) {
      te("dragStarted", this, {
        evt: n
      }), this.nativeDraggable && C(document, "dragover", Bo);
      var o = this.options;
      !t && le(p, o.dragClass, !1), le(p, o.ghostClass, !0), w.active = this, t && this._appendGhost(), J({
        sortable: this,
        name: "start",
        originalEvent: n
      });
    } else
      this._nulling();
  },
  _emulateDragOver: function() {
    if (me) {
      this._lastX = me.clientX, this._lastY = me.clientY, Hn();
      for (var t = document.elementFromPoint(me.clientX, me.clientY), n = t; t && t.shadowRoot && (t = t.shadowRoot.elementFromPoint(me.clientX, me.clientY), t !== n); )
        n = t;
      if (p.parentNode[ue]._isOutsideThisEl(t), n)
        do {
          if (n[ue]) {
            var o = void 0;
            if (o = n[ue]._onDragOver({
              clientX: me.clientX,
              clientY: me.clientY,
              target: t,
              rootEl: n
            }), o && !this.options.dragoverBubble)
              break;
          }
          t = n;
        } while (n = n.parentNode);
      qn();
    }
  },
  _onTouchMove: function(t) {
    if (Ce) {
      var n = this.options, o = n.fallbackTolerance, i = n.fallbackOffset, s = t.touches ? t.touches[0] : t, r = x && Ue(x, !0), l = x && r && r.a, a = x && r && r.d, u = ut && q && En(q), c = (s.clientX - Ce.clientX + i.x) / (l || 1) + (u ? u[0] - jt[0] : 0) / (l || 1), f = (s.clientY - Ce.clientY + i.y) / (a || 1) + (u ? u[1] - jt[1] : 0) / (a || 1);
      if (!w.active && !Re) {
        if (o && Math.max(Math.abs(s.clientX - this._lastX), Math.abs(s.clientY - this._lastY)) < o)
          return;
        this._onDragStart(t, !0);
      }
      if (x) {
        r ? (r.e += c - (Rt || 0), r.f += f - (Lt || 0)) : r = {
          a: 1,
          b: 0,
          c: 0,
          d: 1,
          e: c,
          f
        };
        var b = "matrix(".concat(r.a, ",").concat(r.b, ",").concat(r.c, ",").concat(r.d, ",").concat(r.e, ",").concat(r.f, ")");
        _(x, "webkitTransform", b), _(x, "mozTransform", b), _(x, "msTransform", b), _(x, "transform", b), Rt = c, Lt = f, me = s;
      }
      t.cancelable && t.preventDefault();
    }
  },
  _appendGhost: function() {
    if (!x) {
      var t = this.options.fallbackOnBody ? document.body : M, n = W(p, !0, ut, !0, t), o = this.options;
      if (ut) {
        for (q = t; _(q, "position") === "static" && _(q, "transform") === "none" && q !== document; )
          q = q.parentNode;
        q !== document.body && q !== document.documentElement ? (q === document && (q = ve()), n.top += q.scrollTop, n.left += q.scrollLeft) : q = ve(), jt = En(q);
      }
      x = p.cloneNode(!0), le(x, o.ghostClass, !1), le(x, o.fallbackClass, !0), le(x, o.dragClass, !0), _(x, "transition", ""), _(x, "transform", ""), _(x, "box-sizing", "border-box"), _(x, "margin", 0), _(x, "top", n.top), _(x, "left", n.left), _(x, "width", n.width), _(x, "height", n.height), _(x, "opacity", "0.8"), _(x, "position", ut ? "absolute" : "fixed"), _(x, "zIndex", "100000"), _(x, "pointerEvents", "none"), w.ghost = x, t.appendChild(x), _(x, "transform-origin", Dn / parseInt(x.style.width) * 100 + "% " + xn / parseInt(x.style.height) * 100 + "%");
    }
  },
  _onDragStart: function(t, n) {
    var o = this, i = t.dataTransfer, s = o.options;
    if (te("dragStart", this, {
      evt: t
    }), w.eventCanceled) {
      this._onDrop();
      return;
    }
    te("setupClone", this), w.eventCanceled || (R = Wn(p), R.draggable = !1, R.style["will-change"] = "", this._hideClone(), le(R, this.options.chosenClass, !1), w.clone = R), o.cloneId = mt(function() {
      te("clone", o), !w.eventCanceled && (o.options.removeCloneOnHide || M.insertBefore(R, p), o._hideClone(), J({
        sortable: o,
        name: "clone"
      }));
    }), !n && le(p, s.dragClass, !0), n ? (yt = !0, o._loopId = setInterval(o._emulateDragOver, 50)) : (O(document, "mouseup", o._onDrop), O(document, "touchend", o._onDrop), O(document, "touchcancel", o._onDrop), i && (i.effectAllowed = "move", s.setData && s.setData.call(o, i, p)), C(document, "drop", o), _(p, "transform", "translateZ(0)")), Re = !0, o._dragStartId = mt(o._dragStarted.bind(o, n, t)), C(document, "selectstart", o), qe = !0, Ke && _(document.body, "user-select", "none");
  },
  _onDragOver: function(t) {
    var n = this.el, o = t.target, i, s, r, l = this.options, a = l.group, u = w.active, c = lt === a, f = l.sort, b = H || u, T, S = this, I = !1;
    if (qt)
      return;
    function Q(Xe, so) {
      te(Xe, S, be({
        evt: t,
        isOwner: c,
        axis: T ? "vertical" : "horizontal",
        revert: r,
        dragRect: i,
        targetRect: s,
        canSort: f,
        fromSortable: b,
        target: o,
        completed: X,
        onMove: function(dn, ro) {
          return dt(M, n, p, i, dn, W(dn), t, ro);
        },
        changed: se
      }, so));
    }
    function de() {
      Q("dragOverAnimationCapture"), S.captureAnimationState(), S !== b && b.captureAnimationState();
    }
    function X(Xe) {
      return Q("dragOverCompleted", {
        insertion: Xe
      }), Xe && (c ? u._hideClone() : u._showClone(S), S !== b && (le(p, H ? H.options.ghostClass : u.options.ghostClass, !1), le(p, l.ghostClass, !0)), H !== S && S !== w.active ? H = S : S === w.active && H && (H = null), b === S && (S._ignoreWhileAnimating = o), S.animateAll(function() {
        Q("dragOverAnimationComplete"), S._ignoreWhileAnimating = null;
      }), S !== b && (b.animateAll(), b._ignoreWhileAnimating = null)), (o === p && !p.animated || o === n && !o.animated) && (Ve = null), !l.dragoverBubble && !t.rootEl && o !== document && (p.parentNode[ue]._isOutsideThisEl(t.target), !Xe && $e(t)), !l.dragoverBubble && t.stopPropagation && t.stopPropagation(), I = !0;
    }
    function se() {
      ae = he(p), Se = he(p, l.draggable), J({
        sortable: S,
        name: "change",
        toEl: n,
        newIndex: ae,
        newDraggableIndex: Se,
        originalEvent: t
      });
    }
    if (t.preventDefault !== void 0 && t.cancelable && t.preventDefault(), o = ge(o, l.draggable, n, !0), Q("dragOver"), w.eventCanceled)
      return I;
    if (p.contains(t.target) || o.animated && o.animatingX && o.animatingY || S._ignoreWhileAnimating === o)
      return X(!1);
    if (yt = !1, u && !l.disabled && (c ? f || (r = V !== M) : H === this || (this.lastPutMode = lt.checkPull(this, u, p, t)) && a.checkPut(this, u, p, t))) {
      if (T = this._getDirection(t, o) === "vertical", i = W(p), Q("dragOverValid"), w.eventCanceled)
        return I;
      if (r)
        return V = M, de(), this._hideClone(), Q("revert"), w.eventCanceled || (Ne ? M.insertBefore(p, Ne) : M.appendChild(p)), X(!0);
      var re = ln(n, l.draggable);
      if (!re || Xo(t, T, this) && !re.animated) {
        if (re === p)
          return X(!1);
        if (re && n === t.target && (o = re), o && (s = W(o)), dt(M, n, p, i, o, s, t, !!o) !== !1)
          return de(), n.appendChild(p), V = n, se(), X(!0);
      } else if (re && zo(t, T, this)) {
        var ce = We(n, 0, l, !0);
        if (ce === p)
          return X(!1);
        if (o = ce, s = W(o), dt(M, n, p, i, o, s, t, !1) !== !1)
          return de(), n.insertBefore(p, ce), V = n, se(), X(!0);
      } else if (o.parentNode === n) {
        s = W(o);
        var fe = 0, E, K = p.parentNode !== n, y = !jo(p.animated && p.toRect || i, o.animated && o.toRect || s, T), v = T ? "top" : "left", D = wn(o, "top", "top") || wn(p, "top", "top"), B = D ? D.scrollTop : void 0;
        Ve !== o && (E = s[v], ot = !1, at = !y && l.invertSwap || K), fe = Go(t, o, s, T, y ? 1 : l.swapThreshold, l.invertedSwapThreshold == null ? l.swapThreshold : l.invertedSwapThreshold, at, Ve === o);
        var j;
        if (fe !== 0) {
          var we = he(p);
          do
            we -= fe, j = V.children[we];
          while (j && (_(j, "display") === "none" || j === x));
        }
        if (fe === 0 || j === o)
          return X(!1);
        Ve = o, nt = fe;
        var ke = o.nextElementSibling, ee = !1;
        ee = fe === 1;
        var G = dt(M, n, p, i, o, s, t, ee);
        if (G !== !1)
          return (G === 1 || G === -1) && (ee = G === 1), qt = !0, setTimeout(Wo, 30), de(), ee && !ke ? n.appendChild(p) : o.parentNode.insertBefore(p, ee ? ke : o), D && Yn(D, 0, B - D.scrollTop), V = p.parentNode, E !== void 0 && !at && (ht = Math.abs(E - W(o)[v])), se(), X(!0);
      }
      if (n.contains(p))
        return X(!1);
    }
    return !1;
  },
  _ignoreWhileAnimating: null,
  _offMoveEvents: function() {
    O(document, "mousemove", this._onTouchMove), O(document, "touchmove", this._onTouchMove), O(document, "pointermove", this._onTouchMove), O(document, "dragover", $e), O(document, "mousemove", $e), O(document, "touchmove", $e);
  },
  _offUpEvents: function() {
    var t = this.el.ownerDocument;
    O(t, "mouseup", this._onDrop), O(t, "touchend", this._onDrop), O(t, "pointerup", this._onDrop), O(t, "touchcancel", this._onDrop), O(document, "selectstart", this);
  },
  _onDrop: function(t) {
    var n = this.el, o = this.options;
    if (ae = he(p), Se = he(p, o.draggable), te("drop", this, {
      evt: t
    }), V = p && p.parentNode, ae = he(p), Se = he(p, o.draggable), w.eventCanceled) {
      this._nulling();
      return;
    }
    Re = !1, at = !1, ot = !1, clearInterval(this._loopId), clearTimeout(this._dragStartTimer), Jt(this.cloneId), Jt(this._dragStartId), this.nativeDraggable && (O(document, "drop", this), O(n, "dragstart", this._onDragStart)), this._offMoveEvents(), this._offUpEvents(), Ke && _(document.body, "user-select", ""), _(p, "transform", ""), t && (qe && (t.cancelable && t.preventDefault(), !o.dropBubble && t.stopPropagation()), x && x.parentNode && x.parentNode.removeChild(x), (M === V || H && H.lastPutMode !== "clone") && R && R.parentNode && R.parentNode.removeChild(R), p && (this.nativeDraggable && O(p, "dragend", this), Ut(p), p.style["will-change"] = "", qe && !Re && le(p, H ? H.options.ghostClass : this.options.ghostClass, !1), le(p, this.options.chosenClass, !1), J({
      sortable: this,
      name: "unchoose",
      toEl: V,
      newIndex: null,
      newDraggableIndex: null,
      originalEvent: t
    }), M !== V ? (ae >= 0 && (J({
      rootEl: V,
      name: "add",
      toEl: V,
      fromEl: M,
      originalEvent: t
    }), J({
      sortable: this,
      name: "remove",
      toEl: V,
      originalEvent: t
    }), J({
      rootEl: V,
      name: "sort",
      toEl: V,
      fromEl: M,
      originalEvent: t
    }), J({
      sortable: this,
      name: "sort",
      toEl: V,
      originalEvent: t
    })), H && H.save()) : ae !== Le && ae >= 0 && (J({
      sortable: this,
      name: "update",
      toEl: V,
      originalEvent: t
    }), J({
      sortable: this,
      name: "sort",
      toEl: V,
      originalEvent: t
    })), w.active && ((ae == null || ae === -1) && (ae = Le, Se = tt), J({
      sortable: this,
      name: "end",
      toEl: V,
      originalEvent: t
    }), this.save()))), this._nulling();
  },
  _nulling: function() {
    te("nulling", this), M = p = V = x = Ne = R = ft = Te = Ce = me = qe = ae = Se = Le = tt = Ve = nt = H = lt = w.dragged = w.ghost = w.clone = w.active = null, wt.forEach(function(t) {
      t.checked = !0;
    }), wt.length = Rt = Lt = 0;
  },
  handleEvent: function(t) {
    switch (t.type) {
      case "drop":
      case "dragend":
        this._onDrop(t);
        break;
      case "dragenter":
      case "dragover":
        p && (this._onDragOver(t), Yo(t));
        break;
      case "selectstart":
        t.preventDefault();
        break;
    }
  },
  toArray: function() {
    for (var t = [], n, o = this.el.children, i = 0, s = o.length, r = this.options; i < s; i++)
      n = o[i], ge(n, r.draggable, this.el, !1) && t.push(n.getAttribute(r.dataIdAttr) || qo(n));
    return t;
  },
  sort: function(t, n) {
    var o = {}, i = this.el;
    this.toArray().forEach(function(s, r) {
      var l = i.children[r];
      ge(l, this.options.draggable, i, !1) && (o[s] = l);
    }, this), n && this.captureAnimationState(), t.forEach(function(s) {
      o[s] && (i.removeChild(o[s]), i.appendChild(o[s]));
    }), n && this.animateAll();
  },
  save: function() {
    var t = this.options.store;
    t && t.set && t.set(this);
  },
  closest: function(t, n) {
    return ge(t, n || this.options.draggable, this.el, !1);
  },
  option: function(t, n) {
    var o = this.options;
    if (n === void 0)
      return o[t];
    var i = st.modifyOption(this, t, n);
    typeof i < "u" ? o[t] = i : o[t] = n, t === "group" && Gn(o);
  },
  destroy: function() {
    te("destroy", this);
    var t = this.el;
    t[ue] = null, O(t, "mousedown", this._onTapStart), O(t, "touchstart", this._onTapStart), O(t, "pointerdown", this._onTapStart), this.nativeDraggable && (O(t, "dragover", this), O(t, "dragenter", this)), Array.prototype.forEach.call(t.querySelectorAll("[draggable]"), function(n) {
      n.removeAttribute("draggable");
    }), this._onDrop(), this._disableDelayedDragEvents(), _t.splice(_t.indexOf(this.el), 1), this.el = t = null;
  },
  _hideClone: function() {
    if (!Te) {
      if (te("hideClone", this), w.eventCanceled)
        return;
      _(R, "display", "none"), this.options.removeCloneOnHide && R.parentNode && R.parentNode.removeChild(R), Te = !0;
    }
  },
  _showClone: function(t) {
    if (t.lastPutMode !== "clone") {
      this._hideClone();
      return;
    }
    if (Te) {
      if (te("showClone", this), w.eventCanceled)
        return;
      p.parentNode == M && !this.options.group.revertClone ? M.insertBefore(R, p) : Ne ? M.insertBefore(R, Ne) : M.appendChild(R), this.options.group.revertClone && this.animate(p, R), _(R, "display", ""), Te = !1;
    }
  }
};
function Yo(e) {
  e.dataTransfer && (e.dataTransfer.dropEffect = "move"), e.cancelable && e.preventDefault();
}
function dt(e, t, n, o, i, s, r, l) {
  var a, u = e[ue], c = u.options.onMove, f;
  return window.CustomEvent && !xe && !it ? a = new CustomEvent("move", {
    bubbles: !0,
    cancelable: !0
  }) : (a = document.createEvent("Event"), a.initEvent("move", !0, !0)), a.to = t, a.from = e, a.dragged = n, a.draggedRect = o, a.related = i || t, a.relatedRect = s || W(t), a.willInsertAfter = l, a.originalEvent = r, e.dispatchEvent(a), c && (f = c.call(u, a, r)), f;
}
function Ut(e) {
  e.draggable = !1;
}
function Wo() {
  qt = !1;
}
function zo(e, t, n) {
  var o = W(We(n.el, 0, n.options, !0)), i = 10;
  return t ? e.clientX < o.left - i || e.clientY < o.top && e.clientX < o.right : e.clientY < o.top - i || e.clientY < o.bottom && e.clientX < o.left;
}
function Xo(e, t, n) {
  var o = W(ln(n.el, n.options.draggable)), i = 10;
  return t ? e.clientX > o.right + i || e.clientX <= o.right && e.clientY > o.bottom && e.clientX >= o.left : e.clientX > o.right && e.clientY > o.top || e.clientX <= o.right && e.clientY > o.bottom + i;
}
function Go(e, t, n, o, i, s, r, l) {
  var a = o ? e.clientY : e.clientX, u = o ? n.height : n.width, c = o ? n.top : n.left, f = o ? n.bottom : n.right, b = !1;
  if (!r) {
    if (l && ht < u * i) {
      if (!ot && (nt === 1 ? a > c + u * s / 2 : a < f - u * s / 2) && (ot = !0), ot)
        b = !0;
      else if (nt === 1 ? a < c + ht : a > f - ht)
        return -nt;
    } else if (a > c + u * (1 - i) / 2 && a < f - u * (1 - i) / 2)
      return Ho(t);
  }
  return b = b || r, b && (a < c + u * s / 2 || a > f - u * s / 2) ? a > c + u / 2 ? 1 : -1 : 0;
}
function Ho(e) {
  return he(p) < he(e) ? 1 : -1;
}
function qo(e) {
  for (var t = e.tagName + e.className + e.src + e.href + e.textContent, n = t.length, o = 0; n--; )
    o += t.charCodeAt(n);
  return o.toString(36);
}
function Jo(e) {
  wt.length = 0;
  for (var t = e.getElementsByTagName("input"), n = t.length; n--; ) {
    var o = t[n];
    o.checked && wt.push(o);
  }
}
function mt(e) {
  return setTimeout(e, 0);
}
function Jt(e) {
  return clearTimeout(e);
}
Pt && C(document, "touchmove", function(e) {
  (w.active || Re) && e.cancelable && e.preventDefault();
});
w.utils = {
  on: C,
  off: O,
  css: _,
  find: Un,
  is: function(t, n) {
    return !!ge(t, n, t, !1);
  },
  extend: Po,
  throttle: Bn,
  closest: ge,
  toggleClass: le,
  clone: Wn,
  index: he,
  nextTick: mt,
  cancelNextTick: Jt,
  detectDirection: Xn,
  getChild: We
};
w.get = function(e) {
  return e[ue];
};
w.mount = function() {
  for (var e = arguments.length, t = new Array(e), n = 0; n < e; n++)
    t[n] = arguments[n];
  t[0].constructor === Array && (t = t[0]), t.forEach(function(o) {
    if (!o.prototype || !o.prototype.constructor)
      throw "Sortable: Mounted plugin must be a constructor function, not ".concat({}.toString.call(o));
    o.utils && (w.utils = be(be({}, w.utils), o.utils)), st.mount(o);
  });
};
w.create = function(e, t) {
  return new w(e, t);
};
w.version = Oo;
var Y = [], Je, Zt, Qt = !1, Bt, Yt, Et, Ze;
function Zo() {
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
      var o = n.originalEvent;
      this.sortable.nativeDraggable ? C(document, "dragover", this._handleAutoScroll) : this.options.supportPointer ? C(document, "pointermove", this._handleFallbackAutoScroll) : o.touches ? C(document, "touchmove", this._handleFallbackAutoScroll) : C(document, "mousemove", this._handleFallbackAutoScroll);
    },
    dragOverCompleted: function(n) {
      var o = n.originalEvent;
      !this.options.dragOverBubble && !o.rootEl && this._handleAutoScroll(o);
    },
    drop: function() {
      this.sortable.nativeDraggable ? O(document, "dragover", this._handleAutoScroll) : (O(document, "pointermove", this._handleFallbackAutoScroll), O(document, "touchmove", this._handleFallbackAutoScroll), O(document, "mousemove", this._handleFallbackAutoScroll)), Tn(), pt(), No();
    },
    nulling: function() {
      Et = Zt = Je = Qt = Ze = Bt = Yt = null, Y.length = 0;
    },
    _handleFallbackAutoScroll: function(n) {
      this._handleAutoScroll(n, !0);
    },
    _handleAutoScroll: function(n, o) {
      var i = this, s = (n.touches ? n.touches[0] : n).clientX, r = (n.touches ? n.touches[0] : n).clientY, l = document.elementFromPoint(s, r);
      if (Et = n, o || this.options.forceAutoScrollFallback || it || xe || Ke) {
        Wt(n, this.options, l, o);
        var a = Ie(l, !0);
        Qt && (!Ze || s !== Bt || r !== Yt) && (Ze && Tn(), Ze = setInterval(function() {
          var u = Ie(document.elementFromPoint(s, r), !0);
          u !== a && (a = u, pt()), Wt(n, i.options, u, o);
        }, 10), Bt = s, Yt = r);
      } else {
        if (!this.options.bubbleScroll || Ie(l, !0) === ve()) {
          pt();
          return;
        }
        Wt(n, this.options, Ie(l, !1), !1);
      }
    }
  }, De(e, {
    pluginName: "scroll",
    initializeByDefault: !0
  });
}
function pt() {
  Y.forEach(function(e) {
    clearInterval(e.pid);
  }), Y = [];
}
function Tn() {
  clearInterval(Ze);
}
var Wt = Bn(function(e, t, n, o) {
  if (!!t.scroll) {
    var i = (e.touches ? e.touches[0] : e).clientX, s = (e.touches ? e.touches[0] : e).clientY, r = t.scrollSensitivity, l = t.scrollSpeed, a = ve(), u = !1, c;
    Zt !== n && (Zt = n, pt(), Je = t.scroll, c = t.scrollFn, Je === !0 && (Je = Ie(n, !0)));
    var f = 0, b = Je;
    do {
      var T = b, S = W(T), I = S.top, Q = S.bottom, de = S.left, X = S.right, se = S.width, re = S.height, ce = void 0, fe = void 0, E = T.scrollWidth, K = T.scrollHeight, y = _(T), v = T.scrollLeft, D = T.scrollTop;
      T === a ? (ce = se < E && (y.overflowX === "auto" || y.overflowX === "scroll" || y.overflowX === "visible"), fe = re < K && (y.overflowY === "auto" || y.overflowY === "scroll" || y.overflowY === "visible")) : (ce = se < E && (y.overflowX === "auto" || y.overflowX === "scroll"), fe = re < K && (y.overflowY === "auto" || y.overflowY === "scroll"));
      var B = ce && (Math.abs(X - i) <= r && v + se < E) - (Math.abs(de - i) <= r && !!v), j = fe && (Math.abs(Q - s) <= r && D + re < K) - (Math.abs(I - s) <= r && !!D);
      if (!Y[f])
        for (var we = 0; we <= f; we++)
          Y[we] || (Y[we] = {});
      (Y[f].vx != B || Y[f].vy != j || Y[f].el !== T) && (Y[f].el = T, Y[f].vx = B, Y[f].vy = j, clearInterval(Y[f].pid), (B != 0 || j != 0) && (u = !0, Y[f].pid = setInterval(function() {
        o && this.layer === 0 && w.active._onTouchMove(Et);
        var ke = Y[this.layer].vy ? Y[this.layer].vy * l : 0, ee = Y[this.layer].vx ? Y[this.layer].vx * l : 0;
        typeof c == "function" && c.call(w.dragged.parentNode[ue], ee, ke, e, Et, Y[this.layer].el) !== "continue" || Yn(Y[this.layer].el, ee, ke);
      }.bind({
        layer: f
      }), 24))), f++;
    } while (t.bubbleScroll && b !== a && (b = Ie(b, !1)));
    Qt = u;
  }
}, 30), Jn = function(t) {
  var n = t.originalEvent, o = t.putSortable, i = t.dragEl, s = t.activeSortable, r = t.dispatchSortableEvent, l = t.hideGhostForTarget, a = t.unhideGhostForTarget;
  if (!!n) {
    var u = o || s;
    l();
    var c = n.changedTouches && n.changedTouches.length ? n.changedTouches[0] : n, f = document.elementFromPoint(c.clientX, c.clientY);
    a(), u && !u.el.contains(f) && (r("spill"), this.onSpill({
      dragEl: i,
      putSortable: o
    }));
  }
};
function an() {
}
an.prototype = {
  startIndex: null,
  dragStart: function(t) {
    var n = t.oldDraggableIndex;
    this.startIndex = n;
  },
  onSpill: function(t) {
    var n = t.dragEl, o = t.putSortable;
    this.sortable.captureAnimationState(), o && o.captureAnimationState();
    var i = We(this.sortable.el, this.startIndex, this.options);
    i ? this.sortable.el.insertBefore(n, i) : this.sortable.el.appendChild(n), this.sortable.animateAll(), o && o.animateAll();
  },
  drop: Jn
};
De(an, {
  pluginName: "revertOnSpill"
});
function un() {
}
un.prototype = {
  onSpill: function(t) {
    var n = t.dragEl, o = t.putSortable, i = o || this.sortable;
    i.captureAnimationState(), n.parentNode && n.parentNode.removeChild(n), i.animateAll();
  },
  drop: Jn
};
De(un, {
  pluginName: "removeOnSpill"
});
w.mount(new Zo());
w.mount(un, an);
function Qo() {
  return typeof window < "u" ? window.console : global.console;
}
const Ko = Qo();
function ei(e) {
  const t = /* @__PURE__ */ Object.create(null);
  return function(o) {
    return t[o] || (t[o] = e(o));
  };
}
const ti = /-(\w)/g, In = ei((e) => e.replace(ti, (t, n) => n ? n.toUpperCase() : ""));
function zt(e) {
  e.parentElement !== null && e.parentElement.removeChild(e);
}
function On(e, t, n) {
  const o = n === 0 ? e.children[0] : e.children[n - 1].nextSibling;
  e.insertBefore(t, o);
}
function ni(e, t) {
  return Object.values(e).indexOf(t);
}
function oi(e, t, n, o) {
  if (!e)
    return [];
  const i = Object.values(e), s = t.length - o;
  return [...t].map((l, a) => a >= s ? i.length : i.indexOf(l));
}
function Zn(e, t) {
  this.$nextTick(() => this.$emit(e.toLowerCase(), t));
}
function ii(e) {
  return (t) => {
    this.realList !== null && this["onDrag" + e](t), Zn.call(this, e, t);
  };
}
function si(e) {
  return ["transition-group", "TransitionGroup"].includes(e);
}
function ri(e) {
  if (!e || e.length !== 1)
    return !1;
  const [{ type: t }] = e;
  return t ? si(t.name) : !1;
}
function li(e, t) {
  return t ? { ...t.props, ...t.attrs } : e;
}
const Kt = ["Start", "Add", "Remove", "Update", "End"], en = ["Choose", "Unchoose", "Sort", "Filter", "Clone"], ai = ["Move", ...Kt, ...en].map((e) => "on" + e);
let Xt = null;
const ui = {
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
}, Qn = A({
  name: "VueDraggableNext",
  inheritAttrs: !1,
  emits: [
    "update:modelValue",
    "move",
    "change",
    ...Kt.map((e) => e.toLowerCase()),
    ...en.map((e) => e.toLowerCase())
  ],
  props: ui,
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
    const e = this.$slots.default ? this.$slots.default() : null, t = li(this.$attrs, this.componentData);
    return e ? (this.transitionMode = ri(e), hn(this.getTag(), t, e)) : hn(this.getTag(), t, []);
  },
  created() {
    this.list !== null && this.modelValue !== null && Ko.error("list props are mutually exclusive! Please set one.");
  },
  mounted() {
    const e = {};
    Kt.forEach((i) => {
      e["on" + i] = ii.call(this, i);
    }), en.forEach((i) => {
      e["on" + i] = Zn.bind(this, i);
    });
    const t = Object.keys(this.$attrs).reduce((i, s) => (i[In(s)] = this.$attrs[s], i), {}), n = Object.assign({}, t, e, {
      onMove: (i, s) => this.onDragMove(i, s)
    });
    !("draggable" in n) && (n.draggable = ">*");
    const o = this.$el.nodeType === 1 ? this.$el : this.$el.parentElement;
    this._sortable = new w(o, n), o.__draggable_component__ = this, this.computeIndexes();
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
      return this.component ? ye(this.component) : this.tag;
    },
    updateOptions(e) {
      for (var t in e) {
        const n = In(t);
        ai.indexOf(n) === -1 && this._sortable.option(n, e[t]);
      }
    },
    getChildrenNodes() {
      return this.$el.children;
    },
    computeIndexes() {
      this.$nextTick(() => {
        this.visibleIndexes = oi(this.getChildrenNodes(), this.$el.children, this.transitionMode, this.footerOffset);
      });
    },
    getUnderlyingVm(e) {
      const t = ni(this.getChildrenNodes() || [], e);
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
      const n = (o) => o.splice(t, 0, o.splice(e, 1)[0]);
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
      this.context = this.getUnderlyingVm(e.item), this.context && (e.item._underlying_vm_ = this.clone(this.context.element), Xt = e.item);
    },
    onDragAdd(e) {
      const t = e.item._underlying_vm_;
      if (t === void 0)
        return;
      zt(e.item);
      const n = this.getVmIndex(e.newIndex);
      this.spliceList(n, 0, t), this.computeIndexes();
      const o = { element: t, newIndex: n };
      this.emitChanges({ added: o });
    },
    onDragRemove(e) {
      if (On(this.$el, e.item, e.oldIndex), e.pullMode === "clone") {
        zt(e.clone);
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
      zt(e.item), On(e.from, e.item, e.oldIndex);
      const t = this.context.index, n = this.getVmIndex(e.newIndex);
      this.updatePosition(t, n);
      const o = { element: this.context.element, oldIndex: t, newIndex: n };
      this.emitChanges({ moved: o });
    },
    updateProperty(e, t) {
      e.hasOwnProperty(t) && (e[t] += this.headerOffset);
    },
    onDragMove(e, t) {
      const n = this.move;
      if (!n || !this.realList)
        return !0;
      const o = this.getRelatedContextFromMoveEvent(e), i = this.context, s = this.computeFutureIndex(o, e);
      Object.assign(i, { futureIndex: s });
      const r = Object.assign({}, e, {
        relatedContext: o,
        draggedContext: i
      });
      return n(r, t);
    },
    onDragEnd() {
      this.computeIndexes(), Xt = null;
    },
    getTrargetedComponent(e) {
      return e.__draggable_component__;
    },
    getRelatedContextFromMoveEvent({ to: e, related: t }) {
      const n = this.getTrargetedComponent(e);
      if (!n)
        return { component: n };
      const o = n.realList, i = { list: o, component: n };
      if (e !== t && o && n.getUnderlyingVm) {
        const s = n.getUnderlyingVm(t);
        if (s)
          return Object.assign(s, i);
      }
      return i;
    },
    computeFutureIndex(e, t) {
      const n = [...t.to.children].filter((r) => r.style.display !== "none");
      if (n.length === 0)
        return 0;
      const o = n.indexOf(t.related), i = e.component.getVmIndex(o);
      return n.indexOf(Xt) !== -1 || !t.willInsertAfter ? i : i + 1;
    }
  }
}), di = { key: 0 }, ci = { key: 0 }, fi = { class: "text-field-lable" }, hi = { key: 1 }, mi = { class: "text-field-lable" }, pi = { key: 1 }, gi = { key: 0 }, vi = { key: 1 }, bi = /* @__PURE__ */ A({
  __name: "Text",
  props: {
    model: null,
    textType: null,
    dispLang: { type: Boolean }
  },
  setup(e) {
    return $t(), (t, n) => e.dispLang ? (m(), g("div", di, [
      e.textType === d(Be).ShortAnswer ? (m(), g("div", ci, [
        h("span", fi, L(e.model.lang) + ": ", 1),
        U(h("input", {
          type: "text",
          "onUpdate:modelValue": n[0] || (n[0] = (o) => e.model.value = o),
          class: "text-field"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : e.textType === "Paragraph" ? (m(), g("div", hi, [
        h("span", mi, L(e.model.lang) + ": ", 1),
        U(h("textarea", {
          "onUpdate:modelValue": n[1] || (n[1] = (o) => e.model.value = o),
          class: "field-text-area"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : N("", !0)
    ])) : (m(), g("div", pi, [
      e.textType === d(Be).ShortAnswer ? (m(), g("div", gi, [
        U(h("input", {
          type: "text",
          "onUpdate:modelValue": n[2] || (n[2] = (o) => e.model.value = o),
          class: "text-field"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : e.textType === "Paragraph" ? (m(), g("div", vi, [
        U(h("textarea", {
          "onUpdate:modelValue": n[3] || (n[3] = (o) => e.model.value = o),
          class: "text-area"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : N("", !0)
    ]));
  }
}), gt = /* @__PURE__ */ A({
  __name: "TextCollection",
  props: {
    model: null,
    textType: null
  },
  setup(e) {
    return (t, n) => (m(!0), g(P, null, ie(e.model.values, (o) => {
      var i;
      return m(), ne(bi, {
        key: o.id,
        model: o,
        "text-type": e.textType,
        "disp-lang": ((i = e.model.values) == null ? void 0 : i.length) > 1
      }, null, 8, ["model", "text-type", "disp-lang"]);
    }), 128));
  }
}), yi = { key: 0 }, _i = {
  key: 0,
  class: "option-values"
}, wi = { key: 1 }, Ei = /* @__PURE__ */ A({
  __name: "Option",
  props: {
    model: null,
    optionType: null
  },
  setup(e) {
    $t();
    const t = Dt(!1);
    return (n, o) => {
      const i = ye("font-awesome-icon");
      return t.value ? (m(), g("span", wi, [
        $(gt, {
          model: e.model.optionText,
          "text-type": d(k).ShortAnswer
        }, null, 8, ["model", "text-type"]),
        $(i, {
          icon: "fa-solid fa-circle-check",
          onClick: o[1] || (o[1] = (s) => t.value = !1),
          class: "fa-icon delete-button"
        })
      ])) : (m(), g("span", yi, [
        (m(!0), g(P, null, ie(e.model.optionText.values, (s) => {
          var r;
          return m(), g("span", null, [
            ((r = s.value) == null ? void 0 : r.length) > 0 ? (m(), g("span", _i, L(s.value), 1)) : N("", !0)
          ]);
        }), 256)),
        $(i, {
          icon: "fa-solid fa-pen-to-square",
          onClick: o[0] || (o[0] = (s) => t.value = !0),
          class: "fa-icon"
        })
      ]));
    };
  }
}), Di = /* @__PURE__ */ h("h6", null, "Title:", -1), xi = /* @__PURE__ */ h("h6", null, "Description:", -1), Si = { key: 0 }, Ti = /* @__PURE__ */ h("h6", null, "Options:", -1), Ii = { class: "display-options" }, Oi = /* @__PURE__ */ A({
  __name: "Field",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = rn(t.model), o = $t(), i = Dt(Ye(o.lang)), s = () => {
      var l;
      (l = t.model.options) == null || l.push(An(o.lang, _o(i.value))), i.value.values.forEach((a) => {
        a.value = "";
      });
    }, r = (l) => {
      var u, c;
      const a = (u = t.model.options) == null ? void 0 : u.findIndex((f) => f.id == l);
      (c = t.model.options) == null || c.splice(a, 1);
    };
    return (l, a) => {
      const u = ye("font-awesome-icon");
      return m(), g(P, null, [
        h("h5", null, L(e.model.type), 1),
        h("div", null, [
          Di,
          $(gt, {
            model: e.model.title,
            "text-type": d(k).ShortAnswer
          }, null, 8, ["model", "text-type"])
        ]),
        h("div", null, [
          xi,
          $(gt, {
            model: e.model.description,
            "text-type": d(k).Paragraph
          }, null, 8, ["model", "text-type"])
        ]),
        d(n) ? (m(), g("div", Si, [
          Ti,
          h("div", Ii, [
            $(d(Qn), {
              class: "dragArea list-group w-full",
              list: e.model.options
            }, {
              default: ze(() => [
                (m(!0), g(P, null, ie(e.model.options, (c) => (m(), g("div", {
                  key: c.id,
                  class: "option-entry"
                }, [
                  $(Ei, {
                    model: c,
                    "option-type": e.model.type
                  }, null, 8, ["model", "option-type"]),
                  h("span", null, [
                    $(u, {
                      icon: "fa-solid fa-circle-xmark",
                      onClick: (f) => r(c.id),
                      class: "fa-icon delete"
                    }, null, 8, ["onClick"])
                  ])
                ]))), 128))
              ]),
              _: 1
            }, 8, ["list"])
          ]),
          h("div", null, [
            $(gt, {
              model: i.value,
              "text-type": d(k).ShortAnswer
            }, null, 8, ["model", "text-type"]),
            $(u, {
              icon: "fa-solid fa-circle-plus",
              onClick: a[0] || (a[0] = (c) => s()),
              class: "fa-icon plus add-option"
            })
          ])
        ])) : N("", !0)
      ], 64);
    };
  }
});
const ki = /* @__PURE__ */ h("div", null, [
  /* @__PURE__ */ h("h4", null, "Form properties")
], -1), Ci = { class: "form-field-border" }, $i = /* @__PURE__ */ h("span", { class: "text-field-lable" }, "Name:", -1), Pi = { style: { display: "inline" } }, Ni = /* @__PURE__ */ h("span", { class: "text-area-lable" }, "Description:", -1), Mi = /* @__PURE__ */ h("h3", null, "Fields", -1), Ai = /* @__PURE__ */ A({
  __name: "Form",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = (o) => {
      var s, r;
      const i = (s = t.model.fields) == null ? void 0 : s.findIndex((l) => l.id == o);
      (r = t.model.fields) == null || r.splice(i, 1);
    };
    return (o, i) => {
      var r;
      const s = ye("font-awesome-icon");
      return m(), g(P, null, [
        ki,
        h("div", Ci, [
          h("div", null, [
            $i,
            U(h("input", {
              type: "text",
              "onUpdate:modelValue": i[0] || (i[0] = (l) => e.model.name = l),
              class: "text-field"
            }, null, 512), [
              [z, e.model.name]
            ])
          ]),
          h("div", Pi, [
            Ni,
            U(h("textarea", {
              "onUpdate:modelValue": i[1] || (i[1] = (l) => e.model.description = l),
              class: "text-area"
            }, null, 512), [
              [z, e.model.description]
            ])
          ])
        ]),
        Mi,
        $(d(Qn), {
          class: "dragArea list-group w-full",
          list: (r = e.model) == null ? void 0 : r.fields
        }, {
          default: ze(() => {
            var l;
            return [
              (m(!0), g(P, null, ie((l = e.model) == null ? void 0 : l.fields, (a) => (m(), g("div", {
                key: a.id,
                class: "form-field-border form-field"
              }, [
                $(s, {
                  icon: "fa-solid fa-circle-xmark",
                  onClick: (u) => n(a.id),
                  class: "fa-icon field-delete"
                }, null, 8, ["onClick"]),
                $(Oi, { model: a }, null, 8, ["model"])
              ]))), 128))
            ];
          }),
          _: 1
        }, 8, ["list"])
      ], 64);
    };
  }
}), Kn = (e) => (St("data-v-ebc05723"), e = e(), Tt(), e), Fi = /* @__PURE__ */ Kn(() => /* @__PURE__ */ h("h2", null, "Form Builder", -1)), Vi = { class: "control" }, Ri = ["disabled"], Li = ["disabled"], ji = { class: "toolbar" }, Ui = ["disabled"], Bi = ["disabled"], Yi = ["disabled"], Wi = ["disabled"], zi = ["disabled"], Xi = ["disabled"], Gi = ["disabled"], Hi = ["disabled"], qi = ["disabled"], Ji = ["disabled"], Zi = ["disabled"], Qi = ["disabled"], Ki = ["disabled"], es = /* @__PURE__ */ Kn(() => /* @__PURE__ */ h("hr", null, null, -1)), ts = /* @__PURE__ */ A({
  __name: "App",
  props: {
    piniaInstance: null,
    repositoryRoot: null,
    formId: null
  },
  setup(e) {
    const t = e, n = $t(t.piniaInstance);
    t.formId && n.loadForm(t.formId), xt(() => n.transientMessage, async (l) => {
      l && setTimeout(() => {
        n.transientMessage = null;
      }, 2e3);
    });
    const o = () => {
      n.form = {
        id: F.EMPTY,
        name: "",
        description: "",
        fields: []
      };
    }, i = () => n.saveForm(), s = Z(() => !n.form), r = (l) => {
      var u;
      const a = {
        id: F.create().toString(),
        title: Ye(n.lang),
        description: Ye(n.lang),
        type: l
      };
      rn(a) && (a.options = [An(n.lang, null)]), (u = n.form) == null || u.fields.push(a);
    };
    return (l, a) => (m(), g(P, null, [
      $(nn, { name: "fade" }, {
        default: ze(() => [
          d(n).transientMessage ? (m(), g("p", {
            key: 0,
            class: on("alert alert-" + d(n).transientMessageClass)
          }, L(d(n).transientMessage), 3)) : N("", !0)
        ]),
        _: 1
      }),
      Fi,
      d(n).form ? (m(), ne(Ai, {
        key: 0,
        model: d(n).form
      }, null, 8, ["model"])) : N("", !0),
      h("div", Vi, [
        h("button", {
          type: "button",
          class: "btn btn-primary",
          disabled: !d(s),
          onClick: o
        }, "New Form", 8, Ri),
        h("button", {
          type: "button",
          class: "btn btn-success",
          disabled: d(s),
          onClick: i
        }, "Save", 8, Li)
      ]),
      h("div", ji, [
        h("button", {
          disabled: d(s),
          onClick: a[0] || (a[0] = (u) => r(d(k).ShortAnswer))
        }, "+ Short Answer", 8, Ui),
        h("button", {
          disabled: d(s),
          onClick: a[1] || (a[1] = (u) => r(d(k).Paragraph))
        }, "+ Paragraph", 8, Bi),
        h("button", {
          disabled: d(s),
          onClick: a[2] || (a[2] = (u) => r(d(k).RichText))
        }, "+ Rich Text", 8, Yi),
        h("button", {
          disabled: d(s),
          onClick: a[3] || (a[3] = (u) => r(d(k).Date))
        }, "+ Date", 8, Wi),
        h("button", {
          disabled: d(s),
          onClick: a[4] || (a[4] = (u) => r(d(k).DateTime))
        }, "+ Date/Time", 8, zi),
        h("button", {
          disabled: d(s),
          onClick: a[5] || (a[5] = (u) => r(d(k).Decimal))
        }, "+ Decimal", 8, Xi),
        h("button", {
          disabled: d(s),
          onClick: a[6] || (a[6] = (u) => r(d(k).Integer))
        }, "+ Integer", 8, Gi),
        h("button", {
          disabled: d(s),
          onClick: a[7] || (a[7] = (u) => r(d(k).Email))
        }, "+ Email", 8, Hi),
        h("button", {
          disabled: d(s),
          onClick: a[8] || (a[8] = (u) => r(d(k).Checkboxes))
        }, "+ Checkboxes", 8, qi),
        h("button", {
          disabled: d(s),
          onClick: a[9] || (a[9] = (u) => r(d(k).DataList))
        }, "+ Data List", 8, Ji),
        h("button", {
          disabled: d(s),
          onClick: a[10] || (a[10] = (u) => r(d(k).RadioButtons))
        }, "+ Radio Buttons", 8, Zi),
        h("button", {
          disabled: d(s),
          onClick: a[11] || (a[11] = (u) => r(d(k).DropDown))
        }, "+ Drop Down", 8, Qi),
        h("button", {
          disabled: d(s),
          onClick: a[12] || (a[12] = (u) => r(d(k).InfoSection))
        }, "+ Info Section", 8, Ki)
      ]),
      es
    ], 64));
  }
});
const Nt = (e, t) => {
  const n = e.__vccOpts || e;
  for (const [o, i] of t)
    n[o] = i;
  return n;
}, yr = /* @__PURE__ */ Nt(ts, [["__scopeId", "data-v-ebc05723"]]), _e = Ct("FormSubmissionStore", {
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
      }).then((o) => o.json()).then((o) => {
        this.form = o, t || (this.formData = xo(this.form, this.lang));
      }).catch((o) => {
        console.error("Load Form API Error:", o);
      });
    },
    loadSubmission(e) {
      let t = `https://localhost:5020/api/form-submissions/${e}`;
      console.log(t), fetch(t, {
        method: "GET"
      }).then((n) => n.json()).then((n) => {
        var o;
        this.formData = n, (o = this.formData) != null && o.formId && this.loadForm(this.formData.formId, !0);
      }).catch((n) => {
        console.error("Load Form API Error:", n);
      });
    },
    validateFormData() {
      return console.log("TODO: Validate form data."), !0;
    },
    submitForm() {
      var o, i;
      if (!this.validateFormData()) {
        console.log("Form validation failed.");
        return;
      }
      const e = ((i = (o = this.formData) == null ? void 0 : o.id) == null ? void 0 : i.toString()) === F.EMPTY;
      let t = "https://localhost:5020/api/form-submissions", n = "";
      e ? n = "POST" : (t = `${t}/${this.formData.id}`, n = "PUT"), fetch(t, {
        body: JSON.stringify(this.formData),
        method: n,
        headers: {
          encType: "multipart/form-data",
          "Content-Type": "application/json"
        }
      }).then(async (s) => {
        if (s.ok) {
          if (e) {
            const r = await s.json();
            this.formData.id = r;
          }
          this.transientMessage = "Success", this.transientMessageClass = "success", console.log("Form submission successfull.");
        } else
          switch (this.transientMessageClass = "danger", s.status) {
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
      }).catch((s) => {
        e && this.formData && (this.formData.id = F.EMPTY), this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("FormData Submit API Error:", s);
      });
    },
    saveForm() {
      var o, i;
      if (!this.form) {
        console.error("Cannot save null form.");
        return;
      }
      const e = ((i = (o = this.form) == null ? void 0 : o.id) == null ? void 0 : i.toString()) === F.EMPTY;
      let t = "https://localhost:5020/api/forms", n = "";
      e ? (console.log("Saving new form."), this.form.id = F.create().toString(), n = "POST") : (console.log("Updating existing form."), t = `${t}/${this.form.id}`, n = "PUT"), fetch(t, {
        body: JSON.stringify(this.form),
        method: n,
        headers: {
          encType: "multipart/form-data",
          "Content-Type": "application/json"
        }
      }).then((s) => {
        if (s.ok)
          this.transientMessage = "The form saved successfully", this.transientMessageClass = "success";
        else
          switch (this.transientMessageClass = "danger", s.status) {
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
      }).catch((s) => {
        this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("Form Save API Error:", s);
      });
    },
    clearMessages() {
      this.transientMessage = null;
    }
  }
}), ns = ["onUpdate:modelValue"], eo = /* @__PURE__ */ A({
  __name: "CustomOptions",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Z(() => {
      var r;
      return (r = n.formData.fieldData) == null ? void 0 : r.find((l) => l.fieldId == t.model.id);
    }), i = (r) => {
      var l;
      console.log(r), (l = o.value.customOptionValues) == null || l.splice(r, 1);
    }, s = () => {
      o.value.customOptionValues || (o.value.customOptionValues = []), o.value.customOptionValues.push("");
    };
    return (r, l) => {
      const a = ye("font-awesome-icon");
      return m(), g(P, null, [
        h("div", null, [
          (m(!0), g(P, null, ie(d(o).customOptionValues, (u, c) => (m(), g("span", {
            class: "custom-option",
            key: u.id
          }, [
            U(h("input", {
              type: "text",
              "onUpdate:modelValue": (f) => d(o).customOptionValues[c] = f
            }, null, 8, ns), [
              [z, d(o).customOptionValues[c]]
            ]),
            $(a, {
              icon: "fa-solid fa-circle-xmark",
              onClick: (f) => i(c),
              class: "fa-icon delete"
            }, null, 8, ["onClick"])
          ]))), 128))
        ]),
        $(a, {
          icon: "fa-solid fa-circle-plus",
          onClick: l[0] || (l[0] = (u) => s()),
          class: "fa-icon plus add-option"
        })
      ], 64);
    };
  }
}), os = ["checked", "onChange"], is = { key: 0 }, ss = /* @__PURE__ */ A({
  __name: "Checkboxes",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Z(() => {
      var r;
      return (r = n.formData.fieldData) == null ? void 0 : r.find((l) => l.fieldId == t.model.id);
    }), i = (r) => {
      var l;
      return (l = o.value.selectedOptionIds) == null ? void 0 : l.includes(r);
    }, s = (r, l) => {
      var a, u, c;
      return l ? (a = o.value.selectedOptionIds) == null ? void 0 : a.push(r) : (c = o.value.selectedOptionIds) == null ? void 0 : c.splice((u = o.value.selectedOptionIds) == null ? void 0 : u.indexOf(r), 1);
    };
    return (r, l) => (m(), g(P, null, [
      (m(!0), g(P, null, ie(e.model.options, (a) => (m(), g("div", {
        key: a.id,
        class: "option-field"
      }, [
        h("input", {
          type: "checkbox",
          checked: i(a.id),
          onChange: (u) => s(a.id, u.target.checked)
        }, null, 40, os),
        oe(" " + L(je(a, d(n).lang)) + " ", 1),
        a.isExtendedInput != d(kt).None ? (m(), g("span", is, " TODO: ")) : N("", !0),
        oe(" " + L(a.id), 1)
      ]))), 128)),
      oe(" " + L(d(o)) + " ", 1),
      $(eo, { model: e.model }, null, 8, ["model"])
    ], 64));
  }
}), rs = { id: "dataOptions" }, ls = /* @__PURE__ */ A({
  __name: "DataList",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Z(() => {
      var l;
      return (l = n.formData.fieldData) == null ? void 0 : l.find((a) => a.fieldId == t.model.id);
    }), i = (l) => {
      var u, c;
      const a = (c = (u = t.model) == null ? void 0 : u.options) == null ? void 0 : c.filter((f) => f.id === l).at(0);
      return a ? je(a, n.lang) : "";
    }, s = (l) => {
      var u, c;
      const a = (c = (u = t.model) == null ? void 0 : u.options) == null ? void 0 : c.filter((f) => je(f, n.lang) === l).at(0);
      return a == null ? void 0 : a.id;
    }, r = Z({
      get: () => {
        var l, a;
        return i((a = (l = o == null ? void 0 : o.value) == null ? void 0 : l.selectedOptionIds) == null ? void 0 : a.at(0));
      },
      set: (l) => {
        const a = s(l);
        a ? o.value.selectedOptionIds = [a] : o.value.selectedOptionIds = [];
      }
    });
    return (l, a) => (m(), g(P, null, [
      U(h("input", {
        list: "dataOptions",
        id: "model.id",
        name: "model.id",
        "onUpdate:modelValue": a[0] || (a[0] = (u) => Oe(r) ? r.value = u : null)
      }, null, 512), [
        [z, d(r)]
      ]),
      h("datalist", rs, [
        (m(!0), g(P, null, ie(e.model.options, (u) => (m(), g("option", {
          key: u.id
        }, L(je(u, d(n).lang)), 1))), 128))
      ]),
      oe(" " + L(d(o)) + " ", 1),
      $(eo, { model: e.model }, null, 8, ["model"])
    ], 64));
  }
}), as = ["value"], us = /* @__PURE__ */ A({
  __name: "DropDown",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Z(() => {
      var s;
      return (s = n.formData.fieldData) == null ? void 0 : s.find((r) => r.fieldId == t.model.id);
    }), i = Z({
      get: () => {
        var s;
        return ((s = o == null ? void 0 : o.value) == null ? void 0 : s.selectedOptionIds) && o.value.selectedOptionIds.length > 0 ? o.value.selectedOptionIds[0] : F.EMPTY;
      },
      set: (s) => o.value.selectedOptionIds = [s]
    });
    return (s, r) => (m(), g(P, null, [
      U(h("select", {
        "onUpdate:modelValue": r[0] || (r[0] = (l) => Oe(i) ? i.value = l : null)
      }, [
        (m(!0), g(P, null, ie(e.model.options, (l) => (m(), g("option", {
          key: l.id,
          value: l.id
        }, L(je(l, d(n).lang)), 9, as))), 128))
      ], 512), [
        [Cn, d(i)]
      ]),
      oe(" " + L(d(o)), 1)
    ], 64));
  }
}), ds = ["value"], cs = /* @__PURE__ */ A({
  __name: "RadioButtons",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Z(() => {
      var s;
      return (s = n.formData.fieldData) == null ? void 0 : s.find((r) => r.fieldId == t.model.id);
    }), i = Z({
      get: () => {
        var s;
        return ((s = o == null ? void 0 : o.value) == null ? void 0 : s.selectedOptionIds) && o.value.selectedOptionIds.length > 0 ? o.value.selectedOptionIds[0] : F.EMPTY;
      },
      set: (s) => o.value.selectedOptionIds = [s]
    });
    return (s, r) => (m(), g(P, null, [
      (m(!0), g(P, null, ie(e.model.options, (l) => (m(), g("div", {
        key: l.id,
        class: "option-field"
      }, [
        U(h("input", {
          type: "radio",
          name: "model.id",
          value: l.id,
          "onUpdate:modelValue": r[0] || (r[0] = (a) => Oe(i) ? i.value = a : null)
        }, null, 8, ds), [
          [ho, d(i)]
        ]),
        oe(" " + L(je(l, d(n).lang)), 1)
      ]))), 128)),
      oe(" " + L(d(o)), 1)
    ], 64));
  }
}), fs = { key: 0 }, hs = { key: 1 }, ms = { key: 2 }, ps = { key: 3 }, gs = { key: 4 }, vs = { key: 5 }, bs = ["step"], ys = { key: 6 }, _s = { key: 7 }, to = /* @__PURE__ */ A({
  __name: "Text",
  props: {
    model: null,
    textType: null,
    decimalPoints: null
  },
  setup(e) {
    const t = e, n = t.decimalPoints ? t.decimalPoints : 2;
    return (o, i) => (m(), g(P, null, [
      e.textType === d(k).ShortAnswer ? (m(), g("span", fs, [
        U(h("input", {
          type: "text",
          "onUpdate:modelValue": i[0] || (i[0] = (s) => e.model.value = s),
          class: "text-field"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : e.textType === d(k).Paragraph ? (m(), g("span", hs, [
        U(h("textarea", {
          "onUpdate:modelValue": i[1] || (i[1] = (s) => e.model.value = s),
          class: "field-text-area"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : e.textType === d(k).RichText ? (m(), g("span", ms, [
        U(h("textarea", {
          "onUpdate:modelValue": i[2] || (i[2] = (s) => e.model.value = s),
          class: "field-text-area"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : N("", !0),
      e.textType === d(k).Email ? (m(), g("span", ps, [
        U(h("input", {
          type: "email",
          "onUpdate:modelValue": i[3] || (i[3] = (s) => e.model.value = s),
          class: "text-field"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : N("", !0),
      e.textType === d(k).Integer ? (m(), g("span", gs, [
        U(h("input", {
          type: "number",
          step: "1",
          "onUpdate:modelValue": i[4] || (i[4] = (s) => e.model.value = s),
          class: "text-field"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : N("", !0),
      e.textType === d(k).Decimal ? (m(), g("span", vs, [
        U(h("input", {
          type: "number",
          step: Math.pow(10, -d(n)),
          "onUpdate:modelValue": i[5] || (i[5] = (s) => e.model.value = s),
          class: "text-field"
        }, null, 8, bs), [
          [z, e.model.value]
        ])
      ])) : N("", !0),
      e.textType === d(k).Date ? (m(), g("span", ys, [
        U(h("input", {
          type: "date",
          "onUpdate:modelValue": i[6] || (i[6] = (s) => e.model.value = s),
          class: "text-field"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : N("", !0),
      e.textType === d(k).DateTime ? (m(), g("span", _s, [
        U(h("input", {
          type: "datetime-local",
          "onUpdate:modelValue": i[7] || (i[7] = (s) => e.model.value = s),
          class: "text-field"
        }, null, 512), [
          [z, e.model.value]
        ])
      ])) : N("", !0)
    ], 64));
  }
}), ws = /* @__PURE__ */ A({
  __name: "TextCollection",
  props: {
    model: null,
    textType: null
  },
  setup(e) {
    return (t, n) => (m(), g("span", null, [
      (m(!0), g(P, null, ie(e.model.values, (o) => (m(), ne(to, {
        key: o.id,
        model: o,
        "text-type": e.textType
      }, null, 8, ["model", "text-type"]))), 128))
    ]));
  }
}), Es = ["model"], Ds = {
  key: 0,
  class: "multilingual-field-delete"
}, xs = { style: { "margin-top": "-45px", "margin-left": "530px" } }, Ss = /* @__PURE__ */ A({
  __name: "MultilingualTextInput",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Z(() => {
      var r;
      return (r = n.formData.fieldData) == null ? void 0 : r.find((l) => l.fieldId == t.model.id);
    }), i = () => {
      var r;
      return (r = o.value.multilingualTextValues) == null ? void 0 : r.push(Ye(n.lang));
    }, s = (r) => {
      var l;
      console.log(r), (l = o.value.multilingualTextValues) == null || l.splice(r, 1);
    };
    return (r, l) => {
      const a = ye("font-awesome-icon");
      return m(), g(P, null, [
        (m(!0), g(P, null, ie(d(o).multilingualTextValues, (u) => (m(), g("span", {
          key: u.id,
          model: u
        }, [
          $(ws, {
            model: u,
            "text-type": e.model.type
          }, null, 8, ["model", "text-type"]),
          d(o).multilingualTextValues.length > 1 ? (m(), g("span", Ds, [
            $(a, {
              icon: "fa-solid fa-circle-xmark",
              onClick: (c) => s(u.id),
              class: "fa-icon delete"
            }, null, 8, ["onClick"])
          ])) : N("", !0)
        ], 8, Es))), 128)),
        h("span", xs, [
          $(a, {
            icon: "fa-solid fa-circle-plus",
            onClick: l[0] || (l[0] = (u) => i()),
            class: "fa-icon plus add-option"
          })
        ])
      ], 64);
    };
  }
}), Ts = ["model"], Is = {
  key: 0,
  class: "multilingual-field-delete"
}, Os = /* @__PURE__ */ h("br", null, null, -1), ks = /* @__PURE__ */ A({
  __name: "MonolingualTextInput",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Z(() => {
      var r;
      return (r = n.formData.fieldData) == null ? void 0 : r.find((l) => l.fieldId == t.model.id);
    }), i = () => {
      var r;
      return (r = o.value.monolingualTextValues) == null ? void 0 : r.push(Nn(null));
    }, s = (r) => {
      var l;
      (l = o.value.monolingualTextValues) == null || l.splice(r, 1);
    };
    return (r, l) => {
      const a = ye("font-awesome-icon");
      return m(), g(P, null, [
        (m(!0), g(P, null, ie(d(o).monolingualTextValues, (u) => (m(), g("span", {
          key: u.id,
          model: u
        }, [
          $(to, {
            model: u,
            "text-type": e.model.type
          }, null, 8, ["model", "text-type"]),
          d(o).monolingualTextValues.length > 1 ? (m(), g("span", Is, [
            $(a, {
              icon: "fa-solid fa-circle-xmark",
              onClick: (c) => s(u.id),
              class: "fa-icon delete"
            }, null, 8, ["onClick"])
          ])) : N("", !0)
        ], 8, Ts))), 128)),
        $(a, {
          icon: "fa-solid fa-circle-plus",
          onClick: l[0] || (l[0] = (u) => i()),
          class: "fa-icon plus add-option",
          style: { "margin-top": "-45px", "margin-left": "530px" }
        }),
        Os
      ], 64);
    };
  }
}), Cs = /* @__PURE__ */ h("br", null, null, -1), $s = /* @__PURE__ */ oe("Default Alert"), Ps = ["innerHTML"], Ns = /* @__PURE__ */ A({
  __name: "InfoSection",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Rn(t.model, n.lang);
    return (i, s) => {
      const r = ye("b-alert");
      return m(), g("div", null, [
        Cs,
        $(r, { show: "" }, {
          default: ze(() => [
            $s
          ]),
          _: 1
        }),
        h("div", {
          innerHTML: d(o),
          class: "alert alert-info"
        }, null, 8, Ps)
      ]);
    };
  }
}), Ms = {
  key: 0,
  class: "alert alert-info"
}, As = { class: "text-field-lable" }, Fs = { key: 1 }, Vs = { class: "text-field-lable" }, Rs = ["data-hover"], Ls = /* @__PURE__ */ oe(" : "), js = /* @__PURE__ */ A({
  __name: "Field",
  props: {
    model: null
  },
  setup(e) {
    const t = e, n = _e(), o = Eo(t.model, n.lang), i = Rn(t.model, n.lang), s = Fn(t.model), r = Vn(t.model);
    return (l, a) => {
      const u = ye("font-awesome-icon");
      return m(), g("div", null, [
        e.model.type === d(k).InfoSection ? (m(), g("div", Ms, [
          h("h3", As, L(d(o)), 1)
        ])) : (m(), g("span", Fs, [
          h("span", Vs, [
            oe(L(d(o)) + " ", 1),
            d(i) ? (m(), g("span", {
              key: 0,
              class: "hovertext",
              "data-hover": d(i)
            }, [
              $(u, {
                icon: "fas fa-question-circle",
                class: "fas fa-question-circle"
              })
            ], 8, Rs)) : N("", !0)
          ]),
          Ls
        ])),
        e.model.type === d(k).Checkboxes ? (m(), ne(ss, {
          key: 2,
          model: e.model
        }, null, 8, ["model"])) : N("", !0),
        e.model.type === d(k).DataList ? (m(), ne(ls, {
          key: 3,
          model: e.model
        }, null, 8, ["model"])) : N("", !0),
        e.model.type === d(k).DropDown ? (m(), ne(us, {
          key: 4,
          model: e.model
        }, null, 8, ["model"])) : N("", !0),
        e.model.type === d(k).RadioButtons ? (m(), ne(cs, {
          key: 5,
          model: e.model
        }, null, 8, ["model"])) : N("", !0),
        d(s) ? (m(), ne(Ss, {
          key: 6,
          model: e.model
        }, null, 8, ["model"])) : N("", !0),
        d(r) ? (m(), ne(ks, {
          key: 7,
          model: e.model
        }, null, 8, ["model"])) : N("", !0),
        e.model.type === d(k).InfoSection ? (m(), ne(Ns, {
          key: 8,
          model: e.model
        }, null, 8, ["model"])) : N("", !0)
      ]);
    };
  }
}), Us = /* @__PURE__ */ A({
  __name: "Form",
  props: {
    model: null
  },
  setup(e) {
    return (t, n) => {
      var o;
      return m(!0), g(P, null, ie((o = e.model) == null ? void 0 : o.fields, (i) => (m(), ne(js, {
        key: i.id,
        model: i
      }, null, 8, ["model"]))), 128);
    };
  }
}), no = (e) => (St("data-v-4acd9d0f"), e = e(), Tt(), e), Bs = /* @__PURE__ */ no(() => /* @__PURE__ */ h("h2", null, "Form Submission", -1)), Ys = /* @__PURE__ */ no(() => /* @__PURE__ */ h("hr", null, null, -1)), Ws = { class: "control" }, zs = ["disabled"], Xs = /* @__PURE__ */ A({
  __name: "App",
  props: {
    piniaInstance: null,
    repositoryRoot: null,
    formId: null,
    submissionId: null
  },
  setup(e) {
    const t = e, n = _e(t.piniaInstance);
    t.formId ? n.loadForm(t.formId) : t.submissionId && n.loadSubmission(t.submissionId), xt(() => n.transientMessage, async (s) => {
      s && setTimeout(() => {
        n.transientMessage = null;
      }, 2e3);
    });
    const o = () => n.submitForm(), i = Z(() => !!n.form);
    return (s, r) => (m(), g(P, null, [
      $(nn, { name: "fade" }, {
        default: ze(() => [
          d(n).transientMessage ? (m(), g("p", {
            key: 0,
            class: on("alert alert-" + d(n).transientMessageClass)
          }, L(d(n).transientMessage), 3)) : N("", !0)
        ]),
        _: 1
      }),
      Bs,
      Ys,
      d(n).form ? (m(), ne(Us, {
        key: 0,
        model: d(n).form
      }, null, 8, ["model"])) : N("", !0),
      h("div", Ws, [
        h("button", {
          type: "button",
          class: "btn btn-primary",
          disabled: !d(i),
          onClick: o
        }, "Submit", 8, zs)
      ])
    ], 64));
  }
});
const _r = /* @__PURE__ */ Nt(Xs, [["__scopeId", "data-v-4acd9d0f"]]), Gs = Ct("LoginStore", {
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
      fetch(t, {
        body: JSON.stringify(e),
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        }
      }).then((o) => o.json()).then((o) => {
        o.success ? this.loginResult = o : (this.loginResult = o, console.error("User authorization not successful."));
      }).catch((o) => {
        this.loginResult = {}, console.error("User authorization failed: ", o);
      });
    }
  }
}), Hs = /* @__PURE__ */ h("h2", null, "Login", -1), qs = /* @__PURE__ */ h("br", null, null, -1), Js = /* @__PURE__ */ h("br", null, null, -1), wr = /* @__PURE__ */ A({
  __name: "App",
  props: {
    piniaInstance: null,
    authorizationRoot: null
  },
  setup(e) {
    const t = e, n = Gs(t.piniaInstance);
    mo(() => {
      n.authorizationApiRoot = t.authorizationRoot;
    });
    const o = (i) => {
      n.authorize(i.credential);
    };
    return (i, s) => {
      const r = ye("GoogleLogin");
      return m(), g(P, null, [
        Hs,
        qs,
        $(r, { callback: o }),
        Js
      ], 64);
    };
  }
}), Zs = Ct("WorkflowBuilderStore", {
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
      var o, i;
      if (!this.workflow) {
        console.error("Cannot save null workflow.");
        return;
      }
      const e = ((i = (o = this.workflow) == null ? void 0 : o.id) == null ? void 0 : i.toString()) === F.EMPTY;
      let t = "https://localhost:5020/api/workflow", n = "";
      e ? (console.log("Saving new workflow."), n = "POST") : (console.log("Updating existing workflow."), t = `${t}/${this.workflow.id}`, n = "PUT"), fetch(t, {
        body: JSON.stringify(this.workflow),
        method: n,
        headers: {
          encType: "multipart/form-data",
          "Content-Type": "application/json"
        }
      }).then((s) => {
        if (s.ok)
          this.transientMessage = "The form saved successfully", this.transientMessageClass = "success";
        else
          switch (this.transientMessageClass = "danger", s.status) {
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
      }).catch((s) => {
        this.transientMessage = "Unknown error occurred", this.transientMessageClass = "danger", console.error("Workflow Save API Error:", s);
      });
    }
  }
}), oo = (e) => (St("data-v-4ad7f70c"), e = e(), Tt(), e), Qs = { class: "action" }, Ks = /* @__PURE__ */ oe(" Name: "), er = /* @__PURE__ */ oe(" Description: "), tr = /* @__PURE__ */ oe(" Form: "), nr = /* @__PURE__ */ oo(() => /* @__PURE__ */ h("option", null, "TO DO Form 1", -1)), or = /* @__PURE__ */ oo(() => /* @__PURE__ */ h("option", null, "TO DO Form 2", -1)), ir = [
  nr,
  or
], sr = /* @__PURE__ */ A({
  __name: "WorkflowAction",
  props: {
    model: null
  },
  setup(e) {
    return (t, n) => (m(), g("div", Qs, [
      oe(" Workflow Action: " + L(e.model.id) + " ", 1),
      h("div", null, [
        h("h4", null, L(e.model.name), 1),
        Ks,
        U(h("input", {
          type: "text",
          "onUpdate:modelValue": n[0] || (n[0] = (o) => e.model.name = o)
        }, null, 512), [
          [z, e.model.name]
        ])
      ]),
      h("div", null, [
        h("p", null, L(e.model.description), 1),
        er,
        U(h("textarea", {
          "onUpdate:modelValue": n[1] || (n[1] = (o) => e.model.description = o)
        }, null, 512), [
          [z, e.model.description]
        ])
      ]),
      h("div", null, [
        h("p", null, L(e.model.description), 1),
        tr,
        U(h("select", {
          "onUpdate:modelValue": n[2] || (n[2] = (o) => e.model.formId = o)
        }, ir, 512), [
          [Cn, e.model.formId]
        ])
      ])
    ]));
  }
});
const rr = /* @__PURE__ */ Nt(sr, [["__scopeId", "data-v-4ad7f70c"]]), lr = /* @__PURE__ */ A({
  __name: "Workflow",
  props: {
    model: null
  },
  setup(e) {
    return (t, n) => (m(!0), g(P, null, ie(e.model.actions, (o) => (m(), ne(rr, {
      key: o.id,
      model: o
    }, null, 8, ["model"]))), 128));
  }
}), io = (e) => (St("data-v-493b52a9"), e = e(), Tt(), e), ar = /* @__PURE__ */ io(() => /* @__PURE__ */ h("h2", null, "Workflow Builder", -1)), ur = { class: "control" }, dr = ["disabled"], cr = ["disabled"], fr = { class: "toolbar" }, hr = ["disabled"], mr = /* @__PURE__ */ io(() => /* @__PURE__ */ h("hr", null, null, -1)), pr = /* @__PURE__ */ A({
  __name: "App",
  props: {
    piniaInstance: null,
    repositoryRoot: null,
    workflowId: null
  },
  setup(e) {
    const t = e, n = Zs(t.piniaInstance);
    t.workflowId && n.loadWorkflow(t.workflowId), xt(() => n.transientMessage, async (r) => {
      r && setTimeout(() => {
        n.transientMessage = null;
      }, 2e3);
    });
    const o = () => {
      n.workflow = {
        id: F.EMPTY,
        name: "",
        description: "",
        states: []
      };
    }, i = Z(() => !n.workflow), s = () => {
      if (!n.workflow) {
        console.error("Cannot add action to null workflow");
        return;
      }
      const r = {
        id: F.create().toString(),
        title: "",
        description: "",
        formId: F.createEmpty()
      };
      n.workflow.actions ? n.workflow.actions.push(r) : n.workflow.actions = [r];
    };
    return (r, l) => (m(), g(P, null, [
      $(nn, { name: "fade" }, {
        default: ze(() => [
          d(n).transientMessage ? (m(), g("p", {
            key: 0,
            class: on("alert alert-" + d(n).transientMessageClass)
          }, L(d(n).transientMessage), 3)) : N("", !0)
        ]),
        _: 1
      }),
      ar,
      h("div", ur, [
        h("button", {
          type: "button",
          class: "btn btn-primary",
          disabled: !d(i),
          onClick: o
        }, "New Workflow", 8, dr),
        h("button", {
          type: "button",
          class: "btn btn-success",
          disabled: d(i),
          onClick: l[0] || (l[0] = (...a) => r.saveForm && r.saveForm(...a))
        }, "Save", 8, cr)
      ]),
      h("div", fr, [
        h("button", {
          disabled: d(i),
          onClick: l[1] || (l[1] = (a) => s())
        }, "+ Form Submission Action", 8, hr)
      ]),
      mr,
      d(n).workflow ? (m(), ne(lr, {
        key: 0,
        model: d(n).workflow
      }, null, 8, ["model"])) : N("", !0)
    ], 64));
  }
});
const Er = /* @__PURE__ */ Nt(pr, [["__scopeId", "data-v-493b52a9"]]);
export {
  yr as FormBuilder,
  br as FormModels,
  _r as FormSubmission,
  wr as Login,
  Er as WorkflowBuilder,
  $t as useFormBuilderStore,
  _e as useFormSubmissionStore,
  Gs as useLoginStore,
  Zs as useWorkflowBuilderStore
};
