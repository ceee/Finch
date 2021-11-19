import "./vendor.js";
import {n as normalizeComponent} from "./index.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-add-button"}, [!_vm.component ? _c("ui-button", {attrs: {type: _vm.type, label: _vm.label, disabled: _vm.disabled}, on: {click: _vm.onClick}}) : _c(_vm.component, {tag: "component", attrs: {type: _vm.type, label: _vm.label, disabled: _vm.disabled, route: _vm.route}, on: {click: _vm.onClick}})], 1);
};
var staticRenderFns = [];
var addButton2_vue_vue_type_style_index_0_lang = "\n.ui-add-button\n{\n  display: flex;\n}\n.ui-add-button-items\n{\n  display: grid;\n  grid-template-columns: 1fr 1px 1fr;\n}\n.ui-add-button-item\n{\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  padding: 20px 10px;\n  font-size: var(--font-size);\n  border-radius: var(--radius);\n}\n.ui-add-button-item i\n{\n  font-size: 20px;\n  line-height: 24px;\n  margin-bottom: 12px;\n}\n.ui-add-button-item .is-primary\n{\n  font-size: 24px;\n  color: var(--color-primary);\n}\n.ui-add-button-item:hover\n{\n  background: var(--color-button-light);\n}\n.ui-add-button-items-line\n{\n  display: block;\n  height: 100%;\n  background: var(--color-line);\n}\n";
const script = {
  props: {
    label: {
      type: String,
      default: "@ui.add"
    },
    type: {
      type: String,
      default: "primary"
    },
    route: {
      type: [String, Object],
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    component: null
  }),
  created() {
    this.component = zero.overrides["add-button"] || null;
  },
  methods: {
    onClick() {
      if (this.$refs.dropdown) {
        this.$refs.dropdown.hide();
      }
      if (!!this.route) {
        let routeObj = typeof this.route === "object" ? this.route : {name: this.route};
        this.$router.push(routeObj);
      }
      this.$emit("click", false);
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
component.options.__file = "app/components/buttons/add-button-2.vue";
var addButton2 = component.exports;
export default addButton2;
