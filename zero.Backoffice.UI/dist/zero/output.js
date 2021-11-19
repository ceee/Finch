import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.output, expression: "output"}], staticClass: "ui-property-output"});
};
var staticRenderFns = [];
const script = {
  props: {
    value: [Object, String, Array, Number, Boolean],
    entity: Object,
    render: Function
  },
  computed: {
    output() {
      return typeof this.render === "function" ? this.render(this.value, this.entity) : this.value;
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
component.options.__file = "app/editor/fields/output.vue";
var output = component.exports;
export default output;
