import {n as normalizeComponent} from "./index.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "apps-items"}, [_vm._l(_vm.value, function(app) {
    return _c("router-link", {key: app.id, staticClass: "apps-item", attrs: {to: _vm.getAppLink(app)}}, [_c("strong", {staticClass: "apps-item-name"}, [_vm._v(_vm._s(app.name))]), _c("span", {staticClass: "apps-item-minor"}, [_vm._v(_vm._s(app.domains[0]))]), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.getStatus(app), expression: "getStatus(app)"}], staticClass: "apps-item-status", class: {"is-active": app.isActive}})]);
  }), _c("router-link", {staticClass: "apps-items-add", attrs: {to: _vm.getAddLink()}}, [_c("ui-icon", {attrs: {symbol: "fth-plus", size: 24}})], 1)], 2);
};
var staticRenderFns = [];
var applicationsItems_vue_vue_type_style_index_0_lang = ".apps-items {\n  display: grid;\n  gap: var(--padding);\n  grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));\n  align-items: stretch;\n  margin-top: 40px;\n}\na.apps-item {\n  display: flex;\n  flex-direction: column;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  padding: var(--padding-m);\n  text-align: center;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n  box-shadow: var(--shadow-short);\n}\n.apps-item-minor {\n  color: var(--color-text-dim);\n}\n.apps-item-image {\n  text-align: center;\n  display: inline-block;\n  margin: 0 auto var(--padding-m);\n  position: relative;\n  max-width: 120px;\n  max-height: 50px;\n}\n.apps-item-status {\n  align-self: center;\n  display: inline-block;\n  font-size: 9px;\n  font-weight: 700;\n  margin-top: 15px;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  letter-spacing: 0.5px;\n}\n.apps-items-add {\n  background: transparent;\n  border: 1px dashed var(--color-line-dashed-onbg);\n  color: var(--color-text);\n  border-radius: var(--radius);\n  text-align: center;\n  display: inline-flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  font-size: 22px;\n  width: 60px;\n}";
const baseRoute = __zero.alias.settings.applications;
const script = {
  props: {
    value: {
      type: Array,
      default: () => []
    }
  },
  methods: {
    getAppLink(item) {
      return {
        name: baseRoute + "-edit",
        params: {id: item.id},
        query: {scope: "shared"}
      };
    },
    getStatus(item) {
      return item.isActive ? "@ui.active" : "@ui.inactive";
    },
    getAddLink() {
      return {
        name: baseRoute + "-create",
        query: {scope: "shared"}
      };
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
component.options.__file = "app/pages/settings/applications-items.vue";
var ApplicationsItems = component.exports;
export {ApplicationsItems as A};
