import {n as normalizeComponent, a7 as TranslationsApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "translation", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@translation.name", expression: "'@translation.name'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "translation-items"}, [_c("div", {staticClass: "ui-split"}, [_c("ui-property", {attrs: {label: "@translation.fields.key", required: true, vertical: true, field: "key"}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.item.key, expression: "item.key"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "300", disabled: _vm.disabled}, domProps: {value: _vm.item.key}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.item, "key", $event.target.value);
    }}})]), _c("ui-property", {attrs: {label: "@translation.fields.display", vertical: true, field: "display"}}, [_c("ui-state-button", {attrs: {disabled: _vm.disabled, items: _vm.displayItems}, model: {value: _vm.item.display, callback: function($$v) {
      _vm.$set(_vm.item, "display", $$v);
    }, expression: "item.display"}})], 1)], 1), _c("br"), _c("ui-property", {attrs: {label: "@translation.fields.value", required: true, vertical: true, field: "value"}}, [_vm.item.display === "text" ? _c("textarea", {directives: [{name: "model", rawName: "v-model", value: _vm.item.value, expression: "item.value"}], staticClass: "ui-input", attrs: {disabled: _vm.disabled}, domProps: {value: _vm.item.value}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.item, "value", $event.target.value);
    }}}) : _vm._e(), _vm.item.display === "html" ? _c("ui-rte", {attrs: {disabled: _vm.disabled}, model: {value: _vm.item.value, callback: function($$v) {
      _vm.$set(_vm.item, "value", $$v);
    }, expression: "item.value"}}) : _vm._e()], 1)], 1), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, state: form.state, label: "@ui.save"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}}), !_vm.disabled && _vm.model.id ? _c("ui-button", {staticStyle: {float: "right"}, attrs: {type: "light", label: "@ui.delete"}, on: {click: _vm.onDelete}}) : _vm._e()], 1)];
  }}], null, false, 1323012916)}) : _vm._e();
};
var staticRenderFns = [];
var translation_vue_vue_type_style_index_0_lang = ".translation {\n  text-align: left;\n}\n.translation-items {\n  margin-top: var(--padding);\n}\n.translation-items .ui-property + .ui-property,\n.translation-items .ui-split + .ui-property {\n  margin-top: 0;\n}";
const script = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false,
    meta: {},
    item: {key: null, value: null},
    disabled: false,
    route: zero.alias.settings.translations + "-edit",
    displayItems: [
      {label: "@translation.display.text", value: "text"},
      {label: "@translation.display.html", value: "html"}
    ]
  }),
  methods: {
    onLoad(form) {
      form.load(!this.model.id ? TranslationsApi.getEmpty() : TranslationsApi.getById(this.model.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.item = response.entity;
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(TranslationsApi.save(this.item)).then((res) => {
        this.config.confirm(res);
      });
    },
    onDelete() {
      this.$refs.form.onDelete(TranslationsApi.delete.bind(this, this.item.id));
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
component.options.__file = "app/pages/settings/translation.vue";
var translation = component.exports;
export default translation;
