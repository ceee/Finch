import {n as normalizeComponent, L as LanguagesApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("div", {staticClass: "ui-native-select", attrs: {disabled: _vm.disabled}}, [_c("select", {attrs: {disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: function($event) {
    return _vm.$emit("input", $event.target.value);
  }}}, _vm._l(_vm.items, function(item) {
    return _c("option", {domProps: {value: item.code}}, [_vm._v(_vm._s(item.name))]);
  }), 0)]) : _vm._e();
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: String
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    loading: true,
    items: []
  }),
  mounted() {
    LanguagesApi.getSupportedCultures().then((res) => {
      this.items = res;
      this.loading = false;
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
component.options.__file = "app/editor/fields/culturepicker.vue";
var culturepicker = component.exports;
export default culturepicker;
