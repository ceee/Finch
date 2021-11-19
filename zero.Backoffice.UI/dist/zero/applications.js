import {A as ApplicationsItems} from "./applications-items.js";
import {n as normalizeComponent, e as ApplicationsApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "apps"}, [_c("ui-header-bar", {attrs: {title: "@application.list", count: _vm.count, "back-button": true}}), _c("div", {staticClass: "ui-blank-box"}, [_c("applications-items", {model: {value: _vm.apps, callback: function($$v) {
    _vm.apps = $$v;
  }, expression: "apps"}})], 1)], 1);
};
var staticRenderFns = [];
var applications_vue_vue_type_style_index_0_lang = ".apps .apps-items {\n  margin-top: 0;\n}";
const script = {
  data: () => ({
    count: 0,
    apps: []
  }),
  components: {ApplicationsItems},
  mounted() {
    ApplicationsApi.getAll().then((response) => {
      this.apps = response;
      this.count = response.length;
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
component.options.__file = "app/pages/settings/applications.vue";
var applications = component.exports;
export default applications;
