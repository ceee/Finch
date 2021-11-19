import {n as normalizeComponent, U as UiEditor, b as MediaApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "mediaitem", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {prefix: "@media.list", title: "@media.name", disabled: _vm.disabled, "is-create": !_vm.id, state: form.state, "can-delete": _vm.meta.canDelete}, on: {delete: _vm.onDelete}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "media", meta: _vm.meta, "active-toggle": false, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "before", fn: function() {
        return [_c("ui-property", {attrs: {label: "@media.fields.size"}}, [_c("span", {directives: [{name: "filesize", rawName: "v-filesize", value: _vm.model.size, expression: "model.size"}]})]), _vm.model.imageMeta.width ? _c("ui-property", {attrs: {label: "@media.fields.dimension", "is-text": true}}, [_vm._v(" " + _vm._s(_vm.model.imageMeta.width) + " \xD7 " + _vm._s(_vm.model.imageMeta.height) + " ")]) : _vm._e(), _vm.model.imageMeta.dpi != 0 ? _c("ui-property", {attrs: {label: "@media.fields.dpi", "is-text": true}}, [_vm._v(" " + _vm._s(_vm.model.imageMeta.dpi) + " ")]) : _vm._e(), _vm.model.imageMeta.colorSpace ? _c("ui-property", {attrs: {label: "@media.fields.colorSpace", "is-text": true}}, [_vm._v(" " + _vm._s(_vm.model.imageMeta.colorSpace) + " ")]) : _vm._e(), _vm.model.imageMeta.frames > 1 ? _c("ui-property", {attrs: {label: "@media.fields.frames", "is-text": true}}, [_vm._v(" " + _vm._s(_vm.model.imageMeta.frames) + " ")]) : _vm._e()];
      }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
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
  components: {UiEditor},
  data: () => ({
    meta: {},
    model: {},
    route: "media-edit",
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(!this.id ? MediaApi.getEmpty() : MediaApi.getById(this.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
      });
    },
    onSubmit(form) {
      form.handle(MediaApi.save(this.model));
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(MediaApi.delete.bind(this, this.id));
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
component.options.__file = "app/pages/media/detail.vue";
var detail = component.exports;
export default detail;
