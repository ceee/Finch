import {n as normalizeComponent, a as PagesApi, U as UiEditor, h as hub, O as Overlay, S as Strings} from "./index.js";
import {M as MoveOverlay, C as CopyOverlay} from "./copy.js";
import {h as debounce, c as find} from "./vendor.js";
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-view-box page-editor-info has-sidebar"}, [_c("div", [_c("div", {staticClass: "ui-box"}, [_c("h3", {directives: [{name: "localize", rawName: "v-localize", value: "Links", expression: "'Links'"}], staticClass: "ui-headline"}), _c("div", [_vm.value.route ? _c("a", {staticClass: "ui-button type-light", attrs: {href: _vm.value.route.url, target: "_blank"}}, [_c("span", {staticClass: "ui-button-text"}, [_vm._v("Open page")])]) : _vm._e(), _c("br"), _c("br"), _c("br"), _c("br")]), _c("h3", {directives: [{name: "localize", rawName: "v-localize", value: "@revisions.label", expression: "'@revisions.label'"}], staticClass: "ui-headline"}), !_vm.isCreate ? _c("ui-revisions", {attrs: {get: _vm.getRevisions}}) : _vm._e()], 1)]), _c("div", {staticClass: "ui-view-box-aside"}, [_c("div", {staticClass: "ui-box editor-active-toggle", class: {"is-active": _vm.value.isActive}}, [_c("ui-property", {attrs: {label: "@page.schedule.label", "is-text": true, vertical: true}}, [_c("ui-daterangepicker", {class: {"is-primary": _vm.value.publishDate || _vm.value.unpublishDate}, attrs: {value: {from: _vm.value.publishDate, to: _vm.value.unpublishDate}, disabled: _vm.disabled}, on: {input: _vm.onRangeChange}})], 1), !_vm.isCreate ? _c("ui-property", {attrs: {label: "@ui.id", "is-text": true, vertical: true}}, [_vm._v(" " + _vm._s(_vm.value.id) + " ")]) : _vm._e(), !_vm.isCreate ? _c("ui-property", {attrs: {label: "@ui.createdDate", "is-text": true, vertical: true}}, [_c("ui-date", {model: {value: _vm.value.createdDate, callback: function($$v) {
    _vm.$set(_vm.value, "createdDate", $$v);
  }, expression: "value.createdDate"}})], 1) : _vm._e(), _vm.pageType ? _c("ui-property", {attrs: {label: "@page.type", "is-text": true, vertical: true}}, [_c("i", {class: _vm.pageType.icon}), _vm._v(" " + _vm._s(_vm.pageType.name) + " ")]) : _vm._e()], 1)])]);
};
var staticRenderFns$1 = [];
var pageInfo_vue_vue_type_style_index_0_lang = ".page-editor-info {\n  padding: 0 !important;\n}\n.page-editor-info .ui-view-box-aside {\n  padding: 0;\n}";
const script$1 = {
  props: {
    value: {
      type: [Object, Array]
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    pageType: null
  }),
  computed: {
    isCreate() {
      return !this.value.id;
    }
  },
  mounted() {
    PagesApi.getPageType(this.value.pageTypeAlias).then((pageType) => {
      this.pageType = pageType;
    });
  },
  methods: {
    getRevisions(page2) {
      return PagesApi.getRevisions(this.value.id, page2);
    },
    onRangeChange(value) {
      this.value.publishDate = value.from;
      this.value.unpublishDate = value.to;
    }
  }
};
const __cssModules$1 = {};
var component$1 = normalizeComponent(script$1, render$1, staticRenderFns$1, false, injectStyles$1, null, null, null);
function injectStyles$1(context) {
  for (let o in __cssModules$1) {
    this[o] = __cssModules$1[o];
  }
}
component$1.options.__file = "app/pages/pages/page-info.vue";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "page page-editor", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {title: "@page.name", disabled: _vm.disabled, "is-create": !_vm.id, state: form.state, "active-toggle": !_vm.isFolder, "can-delete": _vm.meta.canDelete}, on: {delete: _vm.onDelete}, scopedSlots: _vm._u([{key: "actions", fn: function() {
      return [!_vm.isFolder ? _c("ui-dropdown-button", {attrs: {label: "@page.preview.title", icon: "fth-eye", disabled: _vm.disabled}, on: {click: _vm.openPreview}}) : _vm._e(), !_vm.isFolder ? _c("ui-dropdown-separator") : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@ui.move.title", icon: "fth-corner-down-right"}, on: {click: function($event) {
        return _vm.move(_vm.model);
      }}}), _c("ui-dropdown-button", {attrs: {label: "@ui.copy.title", icon: "fth-copy"}, on: {click: function($event) {
        return _vm.copy(_vm.model);
      }}}), _c("ui-dropdown-separator")];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _vm.preview.open ? _c("div", {staticClass: "page-editor-preview-message"}, [_c("div", {staticClass: "-text"}, [_c("span", [_vm._v("To update the "), _c("b", [_vm._v("preview")]), _vm._v(" with your unsaved changes click the "), _c("b", [_vm._v("Refresh")]), _vm._v(" button.")])]), _c("div", {staticClass: "-buttons"}, [_c("ui-button", {attrs: {type: "small light onbg", label: "Exit"}, on: {click: function($event) {
      return _vm.exitPreview();
    }}}), _c("ui-button", {attrs: {type: "small light onbg", label: "Open"}, on: {click: _vm.focusPreview}}), _c("ui-button", {attrs: {type: "small", label: "Refresh", icon: "fth-rotate-cw"}, on: {click: _vm.refreshPreview}})], 1)]) : _vm._e(), !_vm.loading && _vm.editor ? _c("ui-editor", {attrs: {config: _vm.editor, meta: _vm.meta, "is-page": true, infos: "none", "on-configure": _vm.onEditorConfigure, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}) : _vm._e(), _vm.isFolder ? _c("div") : _vm._e()];
  }}])});
};
var staticRenderFns = [];
var page_vue_vue_type_style_index_0_lang = ".page-editor {\n  overflow-y: auto;\n  height: 100%;\n}\n.page-editor-info .editor-infos {\n  margin: 0;\n  padding: 0;\n}\n.page-editor-preview-message {\n  margin: -10px var(--padding) var(--padding);\n  background: var(--color-box-light);\n  color: var(--color-primary);\n  font-size: var(--font-size);\n  display: flex;\n  align-items: center;\n  justify-content: space-between;\n  padding: 16px 20px;\n  border-radius: var(--radius);\n  position: relative;\n  line-height: 20px;\n  text-align: left;\n}\n.page-editor-preview-message .-buttons {\n  display: flex;\n}";
const script = {
  name: "page",
  props: ["id", "type", "parent"],
  components: {UiEditor},
  data: () => ({
    loading: true,
    disabled: false,
    editor: null,
    actions: [],
    meta: {},
    pageType: {},
    route: "page",
    model: {
      name: null,
      options: {
        hideInNavigation: false
      },
      link: null
    },
    preview: {
      open: false,
      window: null
    },
    isFolder: false,
    debouncedUpdatePreview: null
  }),
  computed: {
    isCreate() {
      return this.$route.name === "page-create";
    }
  },
  watch: {
    model: {
      deep: true,
      handler(value) {
        this.debouncedUpdatePreview(value);
      }
    }
  },
  mounted() {
    this.debouncedUpdatePreview = debounce(this.updatePreview, 1e3);
    hub.$on("page.sort", (items) => {
      let item = find(items, (x) => x.id === this.id);
      if (item) {
        this.model.sort = item.sort;
      }
    });
    hub.$on("page.move", (item) => {
      if (item.id === this.id) {
        this.model.parentId = item.parentId;
      }
    });
    hub.$on("page.delete", (ids) => {
      if (ids.indexOf(this.id) > -1) {
        this.$router.replace({name: "pages"});
      }
    });
  },
  methods: {
    onLoad(form) {
      this.loading = true;
      form.load(!this.id ? PagesApi.getEmpty(this.type, this.parent) : PagesApi.getById(this.id)).then((response) => {
        this.isFolder = response.entity.pageTypeAlias === __zero.alias.pages.folder;
        this.disabled = !response.meta.canEdit;
        this.editor = response.entity ? this.zero.getEditor("pages." + response.entity.pageTypeAlias) : null;
        this.model = response.entity;
        this.meta = response.meta;
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(PagesApi.save(this.model)).then((response) => {
        if (response.success) {
          hub.$emit("page.update", response.model);
          this.model = response.model;
          localStorage.setItem("zero.last-page." + response.model.appId, response.model.id);
        }
      });
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(PagesApi.delete.bind(this, this.id));
    },
    onEditorConfigure(editor) {
      if (this.isFolder) {
        return;
      }
    },
    move(item) {
      return Overlay.open({
        component: MoveOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        hub.$emit("page.move", value);
        hub.$emit("page.update");
      });
    },
    copy(item) {
      return Overlay.open({
        component: CopyOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        hub.$emit("page.update");
      });
    },
    openPreview() {
      if (this.preview.open) {
        return this.focusPreview();
      }
      const id = Strings.guid();
      this.preview.window = window.open(window.location.origin + "/zero/preview?id=" + id, "blank");
      this.preview.window.focus();
      window.addEventListener("message", (event) => {
        this.preview.window.postMessage({
          id,
          preview: true,
          model: this.model
        }, window.location.origin);
      }, false);
      this.preview.window.onbeforeunload = () => this.exitPreview(true);
      this.preview.open = true;
    },
    updatePreview(value) {
      if (!this.preview.open) {
        return;
      }
      this.preview.window.postMessage({
        preview: true,
        update: true,
        model: value
      }, window.location.origin);
    },
    focusPreview() {
      this.preview.window.focus();
    },
    refreshPreview() {
      this.focusPreview();
    },
    exitPreview(external) {
      if (!external) {
        this.preview.window.close();
      }
      this.preview.window = null;
      this.preview.open = false;
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
component.options.__file = "app/pages/pages/page.vue";
var page = component.exports;
export default page;
