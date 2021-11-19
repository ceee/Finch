import {_ as __$_require_ssets_zero_2_png__, n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-preview"}, [_c("div", {staticClass: "app-preview-controls"}, [_c("img", {directives: [{name: "localize", rawName: "v-localize:alt", value: "@zero.name", expression: "'@zero.name'", arg: "alt"}], staticClass: "app-preview-logo", attrs: {src: __$_require_ssets_zero_2_png__}})]), _vm._m(0)]);
};
var staticRenderFns = [function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-preview-frame"}, [_c("iframe", {attrs: {src: "http://localhost:2300"}})]);
}];
var preview_vue_vue_type_style_index_0_lang = ".app-preview {\n  width: 100%;\n  height: 100vh;\n  display: grid;\n  grid-template-columns: auto 1fr;\n  background: var(--color-bg);\n  color: var(--color-text);\n}\n.app.is-preview {\n  display: grid;\n  grid-template-columns: 1fr;\n  grid-template-rows: 1fr;\n}\n.app.is-preview .app-nav {\n  display: none;\n}\n.app-preview-controls {\n  background: var(--color-bg-bright);\n  width: 80px;\n  color: var(--color-text);\n  height: 100vh;\n  box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);\n  position: relative;\n  z-index: 2;\n}\n.app-preview-logo {\n  margin: 50px 0 50px -5px;\n  max-width: 500px;\n  height: 22px;\n  transform: rotate(-90deg);\n  transform-origin: 50% 50%;\n}\n.app-preview-frame {\n  width: 100%;\n  height: 100vh;\n  overflow: hidden;\n  background: white;\n  position: relative;\n  z-index: 1;\n}\n.app-preview-frame iframe {\n  border-radius: var(--radius);\n  margin: 0;\n  border: none;\n  width: 100%;\n  height: 100%;\n  overflow-y: auto;\n}";
const script = {
  data: () => ({
    path: null
  }),
  mounted() {
    window.addEventListener("message", this.receiveMessage, false);
    window.opener.postMessage({
      preview: true,
      loaded: true
    }, window.location.origin);
  },
  methods: {
    receiveMessage(event) {
      if (typeof event.data !== "object" || !event.data.preview) {
        return;
      }
      console.info(event.data);
    }
  }
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "app/pages/preview.vue";
var preview = component.exports;
export default preview;
