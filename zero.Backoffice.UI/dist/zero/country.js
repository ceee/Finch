import "./vendor.js";
import {n as normalizeComponent, f as countriesApi} from "./index.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "country", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {prefix: "@country.list", title: "@country.name", disabled: _vm.disabled, "is-create": !_vm.$route.params.id, state: form.state, "can-delete": _vm.meta.canDelete}, on: {delete: _vm.onDelete}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "country", meta: _vm.meta, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})];
  }}])});
};
var staticRenderFns = [];
var country_vue_vue_type_style_index_0_lang = ".country .country-flag-input {\n  max-width: 80px;\n}";
const script = {
  props: ["id"],
  data: () => ({
    meta: {},
    model: {name: null, features: [], domains: []},
    route: __zero.alias.settings.countries + "-edit",
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(!this.$route.params.id ? countriesApi.getEmpty() : countriesApi.getById(this.id, {scope: "hallo"})).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
      });
    },
    onSubmit(form) {
      form.handle(countriesApi.save(this.model));
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(countriesApi.delete.bind(this, this.id));
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
component.options.__file = "app/pages/settings/country.vue";
var country = component.exports;
export default country;
