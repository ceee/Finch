import {n as normalizeComponent, Y as FormsApi} from "./index.js";
import "./vendor.js";
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("router-link", {staticClass: "zforms-item", attrs: {to: {name: "forms-edit", params: {id: _vm.value.id}}}}, [_c("strong", {staticClass: "zforms-item-name"}, [_vm._v(_vm._s(_vm.value.name))]), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: _vm.entries, tokens: {count: _vm.value.count}}, expression: "{ key: entries, tokens: { count: value.count } }"}], staticClass: "zforms-item-minor"}), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.status, expression: "status"}], staticClass: "zforms-item-status", class: {"is-active": _vm.value.isActive}})]);
};
var staticRenderFns$1 = [];
var formsItem_vue_vue_type_style_index_0_lang = ".zforms-items {\n  display: grid;\n  gap: var(--padding);\n  grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));\n  align-items: stretch;\n  margin-top: 40px;\n}\na.zforms-item {\n  display: flex;\n  flex-direction: column;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  padding: var(--padding-m);\n  text-align: center;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n  box-shadow: var(--shadow-short);\n}\n.zforms-item-minor {\n  color: var(--color-text-dim);\n}\n.zforms-item-image {\n  text-align: center;\n  display: inline-block;\n  margin: 0 auto var(--padding-m);\n  position: relative;\n  max-width: 120px;\n  max-height: 50px;\n}\n.zforms-item-status {\n  align-self: center;\n  display: inline-block;\n  font-size: 9px;\n  font-weight: 700;\n  margin-top: 15px;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  letter-spacing: 0.5px;\n}\n.zforms-items-add {\n  background: transparent;\n  border: 1px dashed var(--color-line-dashed);\n  color: var(--color-text);\n  border-radius: var(--radius);\n  text-align: center;\n  display: inline-flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  font-size: 22px;\n  width: 60px;\n}";
const script$1 = {
  props: {
    value: Object
  },
  computed: {
    status() {
      return this.value.isActive ? "@ui.active" : "@ui.inactive";
    },
    entries() {
      return this.value.count !== 1 ? "@forms.entries_x" : "@forms.entries_1";
    }
  }
};
const __cssModules$1 = {};
var component$1 = normalizeComponent(script$1, render$1, staticRenderFns$1, false, injectStyles$1, null, null, null);
function injectStyles$1(context) {
  for (let o in __cssModules$1) {
    this[o] = __cssModules$1[o];
  }
}
component$1.options.__file = "../zero.Forms/Plugin/pages/partials/forms-item.vue";
var FormsItem = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "zforms"}, [_c("ui-header-bar", {attrs: {title: "@forms.list", count: _vm.count, "back-button": true}}, [_c("ui-add-button", {attrs: {route: _vm.createRoute}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("div", {staticClass: "zforms-items"}, _vm._l(_vm.items, function(item) {
    return _c("forms-item", {key: item.id, attrs: {value: item}});
  }), 1)])], 1);
};
var staticRenderFns = [];
var dashboard_vue_vue_type_style_index_0_lang = ".zforms-items {\n  display: grid;\n  gap: var(--padding);\n  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));\n  align-items: stretch;\n  margin-top: 0;\n}\na.zforms-item {\n  display: flex;\n  flex-direction: column;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  padding: var(--padding-m);\n  text-align: center;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n  box-shadow: var(--shadow-short);\n}\n.zforms-item-minor {\n  color: var(--color-text-dim);\n}\n.zforms-item-image {\n  text-align: center;\n  display: inline-block;\n  margin: 0 auto var(--padding-m);\n  position: relative;\n  max-width: 120px;\n  max-height: 50px;\n}\n.zforms-item-status {\n  align-self: center;\n  display: inline-block;\n  font-size: 9px;\n  font-weight: 700;\n  margin-top: 15px;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  letter-spacing: 0.5px;\n}\n.zforms-items-add {\n  background: transparent;\n  border: 1px dashed var(--color-line-dashed);\n  color: var(--color-text);\n  border-radius: var(--radius);\n  text-align: center;\n  display: inline-flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  font-size: 22px;\n  width: 60px;\n}";
const script = {
  data: () => ({
    items: [],
    count: 0,
    createRoute: "forms-create"
  }),
  components: {FormsItem},
  mounted() {
    FormsApi.getListByQuery().then((response) => {
      this.items = response.items;
      this.count = response.totalItems;
    });
  }
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "../zero.Forms/Plugin/dashboard.vue";
var dashboard = component.exports;
export default dashboard;
