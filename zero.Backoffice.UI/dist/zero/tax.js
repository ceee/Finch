import {n as normalizeComponent, T as TaxesApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "shop-tax", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {prefix: "@shop.tax.list", title: "@shop.tax.name", disabled: _vm.disabled, "is-create": !_vm.id, state: form.state, "can-delete": _vm.meta.canDelete}, on: {delete: _vm.onDelete}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "commerce.tax", meta: _vm.meta, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
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
    route: "commerce-taxes-edit",
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(!this.id ? TaxesApi.getEmpty() : TaxesApi.getById(this.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
      });
    },
    onSubmit(form) {
      form.handle(TaxesApi.save(this.model));
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(TaxesApi.delete.bind(this, this.id));
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
component.options.__file = "../zero.Commerce/Plugin/pages/settings/tax.vue";
var tax = component.exports;
export default tax;
