import {A as ApplicationsItems} from "./applications-items.js";
import {g as get, n as normalizeComponent} from "./index.js";
import "./vendor.js";
var SettingsApi = {
  getAreas: async () => await get("settings/getAreas")
};
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "settings"}, [_vm._e(), _vm._l(_vm.groups, function(group) {
    return _c("div", {staticClass: "settings-group"}, [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: group.name, expression: "group.name"}], staticClass: "ui-headline settings-group-headline"}), _c("div", {staticClass: "settings-group-items"}, _vm._l(group.items, function(item) {
      return _c("router-link", {key: item.name, staticClass: "settings-group-item", attrs: {to: item.url || "/"}}, [_c("span", {staticClass: "settings-group-item-icon"}, [_c("ui-icon", {attrs: {symbol: item.icon || "fth-settings", size: 18}})], 1), _c("p", {staticClass: "settings-group-item-text"}, [_c("strong", {directives: [{name: "localize", rawName: "v-localize", value: item.name, expression: "item.name"}]}), item.description ? [_c("br"), _c("ui-localize", {attrs: {value: item.description, tokens: _vm.tokens}})] : _vm._e()], 2)]);
    }), 1)]);
  }), _c("router-view", {attrs: {name: "footer"}})], 2);
};
var staticRenderFns = [];
var settings_vue_vue_type_style_index_0_lang = ".settings {\n  min-height: 100%;\n  position: relative;\n  padding: 40px 80px;\n  width: 100%;\n  max-width: 2000px;\n  display: flex;\n  flex-direction: column;\n  gap: 80px 40px;\n}\n.settings-group-items {\n  display: grid;\n  gap: 20px;\n  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));\n  align-items: stretch;\n  margin-top: 40px;\n}\na.settings-group-item {\n  color: var(--color-text);\n  font-size: var(--font-size);\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 20px;\n  align-items: center;\n}\n.settings-group-item-icon {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  width: 60px;\n  height: 60px;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n  color: var(--color-text);\n  transition: color 0.2s ease;\n}\na.settings-group-item:hover .settings-group-item-icon {\n  color: var(--color-text);\n}\n.settings-group-item-text {\n  line-height: 1.3;\n  color: var(--color-text-dim);\n  margin: 0;\n}\n.settings-group-item-text strong {\n  display: inline-block;\n  margin-bottom: 5px;\n  color: var(--color-text);\n}";
const script = {
  name: "app-settings",
  components: {ApplicationsItems},
  data: () => ({
    page: true,
    groups: [],
    apps: [],
    tokens: {
      zero_version: zero.version,
      plugin_count: zero.pluginCount
    }
  }),
  mounted() {
    SettingsApi.getAreas().then((response) => {
      this.groups = response.groups;
      this.apps = response.applications;
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
component.options.__file = "app/pages/settings/settings.vue";
var settings = component.exports;
export default settings;
