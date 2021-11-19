import {n as normalizeComponent, U as UiEditor, R as AuthorsApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "stories-author", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {title: "@stories.author.name", disabled: _vm.disabled, "is-create": !_vm.id, state: form.state, "can-delete": _vm.meta.canDelete}, on: {delete: _vm.onDelete}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "story.author", meta: _vm.meta, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})];
  }}])});
};
var staticRenderFns = [];
var author_vue_vue_type_style_index_0_lang = "\n.story .editor\n{\n  max-width: 1060px;\n}\n.story .editor .ui-tab.ui-box:first-child\n{\n  padding: 80px 0;\n}\n.story .ui-tab.ui-box:first-child .editor-tab-headline\n{\n  display: none;\n}\n";
const script = {
  props: ["id"],
  components: {UiEditor},
  data: () => ({
    meta: {},
    model: {name: null, publishDate: null},
    route: "stories-authors-edit",
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(!this.$route.params.id ? AuthorsApi.getEmpty() : AuthorsApi.getById(this.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
      });
    },
    onSubmit(form) {
      form.handle(AuthorsApi.save(this.model));
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(AuthorsApi.delete.bind(this, this.id));
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
component.options.__file = "../zero.Stories/Plugin/pages/author.vue";
var author = component.exports;
export default author;
