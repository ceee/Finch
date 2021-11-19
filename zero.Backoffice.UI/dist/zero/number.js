import {n as normalizeComponent, I as NumbersApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "shop-number", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {prefix: "@shop.number.list", title: "@shop.number.name", disabled: _vm.disabled, state: form.state, "title-disabled": true, "active-disabled": true, "can-delete": false}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "commerce.number", meta: _vm.meta, disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})];
  }}])});
};
var staticRenderFns = [];
const script = {
  props: ["id"],
  data: () => ({
    meta: {},
    model: {name: null},
    route: "commerce-numbers-edit",
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(!this.id ? NumbersApi.getEmpty() : NumbersApi.getByAlias(this.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
      });
    },
    onSubmit(form) {
      form.handle(NumbersApi.save(this.model));
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
component.options.__file = "../zero.Commerce/Plugin/pages/settings/number.vue";
var number = component.exports;
export default number;
