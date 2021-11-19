import {n as normalizeComponent, O as Overlay, N as Notification} from "./index.js";
import {I as IntegrationsApi} from "./integrations.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-editor-overlay integration", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: _vm.config.model.name, prefix: "@integration.list", "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), !_vm.config.isCreate ? _c("ui-button", {attrs: {type: "light onbg", label: "@ui.remove"}, on: {click: _vm.onDelete}}) : _vm._e(), _vm.config.isCreate && !_vm.disabled ? _c("ui-button", {attrs: {type: "light onbg", submit: true, label: "@ui.save", state: form.state, disabled: _vm.loading}}) : _vm._e(), _vm.config.isCreate && !_vm.disabled ? _c("ui-button", {attrs: {type: "primary", label: "Save and activate", state: form.state, disabled: _vm.loading}, on: {click: _vm.saveAndActivate}}) : _vm._e(), !_vm.config.isCreate && !_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, label: "@ui.save", state: form.state, disabled: _vm.loading}}) : _vm._e()];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-editor-overlay-editor"}, [_c("ui-editor", {attrs: {config: _vm.editor, meta: _vm.meta, "is-page": false, infos: "none", disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns = [];
const script = {
  props: {
    config: Object
  },
  data: () => ({
    disabled: false,
    loading: true,
    state: "default",
    editor: null,
    meta: {},
    model: {}
  }),
  mounted() {
  },
  methods: {
    onLoad(form) {
      form.load(this.config.isCreate ? IntegrationsApi.getEmpty(this.config.alias) : IntegrationsApi.getByAlias(this.config.alias)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
        this.editor = this.model.typeAlias ? "integration." + this.model.typeAlias : null;
        this.loading = false;
      });
    },
    saveAndActivate(e) {
      this.model.isActive = true;
      this.onSubmit(this.$refs.form);
    },
    onSubmit(form) {
      form.handle(IntegrationsApi.save(this.model)).then((res) => {
        this.config.confirm(res);
      });
    },
    onDelete() {
      Overlay.confirmDelete().then((opts) => {
        opts.state("loading");
        IntegrationsApi.delete(this.config.alias).then((response) => {
          if (response.success) {
            opts.state("success");
            opts.hide();
            Notification.success("@deleteoverlay.success", "@deleteoverlay.success_text");
            this.config.confirm(response);
          } else {
            opts.errors(response.errors);
          }
        });
      });
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
component.options.__file = "app/pages/settings/integration.vue";
var integration = component.exports;
export default integration;
