import {$ as NavigationEditor, E as Editor, n as normalizeComponent, U as UiEditor, e as ApplicationsApi} from "./index.js";
import "./vendor.js";
const editor = new Editor("laola.settings.layout", "@laola.settings.layout.fields.");
const header = editor.tab("header", "@laola.settings.layout.groups.header");
const footer = editor.tab("footer", "@laola.settings.layout.groups.footer");
const social = editor.tab("social", "@laola.settings.layout.groups.social");
header.field("subtitle").text(40);
header.field("navigation").custom(NavigationEditor);
footer.field("copyright").text(40);
footer.field("footerText").text(100);
footer.field("footerNavigation").custom(NavigationEditor);
social.field("social.facebook").text(120);
social.field("social.instagram").text(120);
social.field("social.youtube").text(120);
social.field("social.twitter").text(120);
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "la-layout", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {title: "@laola.settings.layout.name", state: form.state, "can-delete": false, "title-disabled": true}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _vm.editorConfig ? _c("ui-editor", {attrs: {config: _vm.editorConfig, infos: "none"}, model: {value: _vm.model.layout, callback: function($$v) {
      _vm.$set(_vm.model, "layout", $$v);
    }, expression: "model.layout"}}) : _vm._e()];
  }}])});
};
var staticRenderFns = [];
var layout_vue_vue_type_style_index_0_lang = ".la-layout-boxes {\n  padding: 45px var(--padding);\n  padding-top: 0;\n  padding-left: 48px;\n}\n.la-layout-box .ui-headline {\n  margin-bottom: var(--padding-s);\n  color: var(--color-text-dim);\n  font-weight: 400;\n}\n.la-layout-box .ui-box {\n  margin: 0;\n}\n.la-layout-box + .la-layout-box {\n  margin-top: 64px;\n}";
const script = {
  components: {UiEditor},
  data: () => ({
    model: {
      header: {
        subtitle: null,
        navigation: []
      },
      footer: {
        navigation: []
      }
    },
    editorConfig: editor
  }),
  methods: {
    onLoad(form) {
      form.load(ApplicationsApi.getById(zero.appId)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
      });
    },
    onSubmit(form) {
      form.handle(ApplicationsApi.save(this.model)).then((response) => {
        this.model = response.model;
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
component.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/layout/layout.vue";
var layout = component.exports;
export default layout;
