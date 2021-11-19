import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-native-select", attrs: {disabled: _vm.disabled}}, [_c("select", {attrs: {disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: _vm.onChange}}, [_vm.emptyOption ? _c("option") : _vm._e(), _vm._l(_vm.options, function(option) {
    return _c("option", {directives: [{name: "localize", rawName: "v-localize", value: option.value, expression: "option.value"}], domProps: {value: option.key}});
  })], 2)]);
};
var staticRenderFns = [];
const script = {
  props: {
    value: [String, Number, Object],
    items: [Array, Function],
    entity: {
      type: Object,
      required: true
    },
    disabled: Boolean,
    emptyOption: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    options: []
  }),
  created() {
    this.rebuild();
  },
  watch: {
    items: {
      deep: true,
      handler() {
        this.rebuild();
        this.onChange({target: {value: this.value}});
      }
    },
    entity: {
      deep: true,
      handler() {
        this.rebuild();
        this.onChange({target: {value: this.value}});
      }
    }
  },
  methods: {
    rebuild() {
      let items = [];
      if (!this.entity || !this.items) {
        this.options = items;
        return;
      }
      if (typeof this.items === "function") {
        this.options = this.items(this.entity);
      } else {
        this.options = [...this.items];
      }
    },
    onChange(ev) {
      this.$emit("input", ev.target.value ? ev.target.value : null);
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
component.options.__file = "app/editor/fields/select.vue";
var select = component.exports;
export default select;
