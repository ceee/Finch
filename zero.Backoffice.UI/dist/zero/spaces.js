import {n as normalizeComponent, U as UiEditor, m as SpacesApi, o as List} from "./index.js";
import "./vendor.js";
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "space-editor", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {title: _vm.space.name, "title-disabled": _vm.space.view === "editor", disabled: _vm.disabled, "is-create": !_vm.id, state: form.state, "can-delete": _vm.meta.canDelete, "active-toggle": true}, on: {delete: _vm.onDelete}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _vm.editor ? _c("ui-editor", {attrs: {config: _vm.editor, meta: _vm.meta, disabled: _vm.disabled, "active-toggle": false}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}) : _vm._e()];
  }}])});
};
var staticRenderFns$3 = [];
var editor_vue_vue_type_style_index_0_lang = ".space-editor .ui-header-bar + .editor > .ui-box {\n  margin-top: 0;\n}\n.space-editor .editor-outer.-infos-aside {\n  grid-template-columns: 1fr;\n}\n.space-editor .editor-outer.-infos-aside .editor-infos {\n  margin: -31px var(--padding) 0;\n}";
const script$3 = {
  props: ["config", "space"],
  components: {UiEditor},
  data: () => ({
    disabled: false,
    editor: null,
    meta: {},
    route: null,
    model: {name: null}
  }),
  computed: {
    id() {
      return this.$route.params.id;
    },
    alias() {
      return this.$route.params.alias;
    },
    isList() {
      return !!this.id;
    }
  },
  watch: {
    $route: "setup"
  },
  methods: {
    setup() {
      this.editor = this.zero.getEditor("spaces." + this.space.alias);
    },
    onLoad(form) {
      this.setup();
      form.load(SpacesApi.getContent(this.alias, this.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
        this.route = {name: "space-item", params: {alias: this.alias}};
      });
    },
    onSubmit(form) {
      form.handle(SpacesApi.save(this.model));
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(SpacesApi.delete.bind(this, this.alias, this.id));
    }
  }
};
const __cssModules$3 = {};
var component$3 = normalizeComponent(script$3, render$3, staticRenderFns$3, false, injectStyles$3, null, null, null);
function injectStyles$3(context) {
  for (let o in __cssModules$3) {
    this[o] = __cssModules$3[o];
  }
}
component$3.options.__file = "app/pages/spaces/views/editor.vue";
var SpaceEditor = component$3.exports;
const list = new List("spaces.default");
list.column("name").name();
list.column("createdDate").created();
list.column("isActive").active();
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("div", {staticClass: "space-list", attrs: {"data-space": _vm.space.alias}}, [_c("ui-header-bar", {attrs: {title: _vm.space.name, count: _vm.count, "title-empty": "List"}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {route: _vm.createRoute}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: _vm.listRenderer}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1) : _vm._e();
};
var staticRenderFns$2 = [];
const script$2 = {
  props: ["space", "config"],
  data: () => ({
    count: 0,
    loading: true,
    listRenderer: null,
    createRoute: {
      name: "space-create",
      params: {alias: null}
    }
  }),
  watch: {
    space: "load"
  },
  created() {
    this.load();
  },
  methods: {
    load() {
      this.loading = true;
      const alias = "spaces." + this.space.alias;
      const listRenderer = this.zero.getList(alias) || list;
      this.createRoute.params.alias = this.space.alias;
      listRenderer.link = (item) => {
        return {
          name: "space-item",
          params: {alias: this.space.alias, id: item.id}
        };
      };
      listRenderer.onFetch((q) => SpacesApi.getList(this.space.alias, q));
      this.listRenderer = listRenderer;
      this.loading = false;
    },
    add() {
      this.$router.push({
        name: "space-create",
        params: {alias: this.space.alias}
      });
    }
  }
};
const __cssModules$2 = {};
var component$2 = normalizeComponent(script$2, render$2, staticRenderFns$2, false, injectStyles$2, null, null, null);
function injectStyles$2(context) {
  for (let o in __cssModules$2) {
    this[o] = __cssModules$2[o];
  }
}
component$2.options.__file = "app/pages/spaces/views/list.vue";
var SpaceList = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "space-editor"}, [_c("ui-header-bar", {attrs: {title: _vm.space.name, "title-empty": "Space"}}), _c("div", {staticClass: "ui-blank-box"}, [_vm._v(" custom ")])], 1);
};
var staticRenderFns$1 = [];
const script$1 = {
  props: ["alias"],
  data: () => ({
    space: {},
    tableConfig: {}
  }),
  watch: {
    $route: "load"
  },
  created() {
    this.load();
  },
  methods: {
    load() {
      SpacesApi.getByAlias(this.alias).then((response) => {
        this.space = response;
      });
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
component$1.options.__file = "app/pages/spaces/views/custom.vue";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "spaces"}, [_c("div", {directives: [{name: "resizable", rawName: "v-resizable", value: _vm.resizable, expression: "resizable"}], staticClass: "app-tree spaces-tree"}, [_c("ui-header-bar", {attrs: {title: "@space.list"}}), _c("div", {staticClass: "spaces-tree-items"}, _vm._l(_vm.spaces, function(item) {
    return _c("router-link", {key: item.alias, staticClass: "spaces-tree-item", class: {"has-line": item.lineBelow}, attrs: {to: {name: "space", params: {alias: item.alias}}}}, [_c("ui-icon", {staticClass: "spaces-tree-item-icon", attrs: {symbol: item.icon}}), _c("span", {staticClass: "spaces-tree-item-text"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: item.name, expression: "item.name"}]}), item.description ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item.description, expression: "item.description"}], staticClass: "-minor"}) : _vm._e()])], 1);
  }), 1), _c("div", {staticClass: "spaces-tree-resizable ui-resizable"})], 1), !_vm.isOverview && _vm.loaded && _vm.component ? _c(_vm.component, {ref: "comp", tag: "component", staticClass: "spaces-main", attrs: {space: _vm.space, config: _vm.spaceConfig}}) : _vm._e()], 1);
};
var staticRenderFns = [];
var spaces_vue_vue_type_style_index_0_lang = ".spaces {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 2px;\n  justify-content: stretch;\n  height: 100vh;\n}\n.spaces-main {\n  min-height: 100vh;\n  overflow-y: auto;\n}\n.spaces-overview {\n  padding: 95px 0 0 60px;\n}\n.spaces-tree-items {\n  margin-top: -13px;\n}\n.spaces-tree-item {\n  display: grid;\n  grid-template-columns: 32px 1fr auto;\n  gap: 6px;\n  height: 100%;\n  align-items: center;\n  position: relative;\n  padding: 15px var(--padding);\n  font-size: var(--font-size);\n  color: var(--color-text);\n  transition: color 0.2s ease;\n  /*&.is-active:before\n  {\n    content: '';\n    position: absolute;\n    left: 0;\n    top: 0;\n    bottom: 0;\n    width: 3px;\n    display: inline-block;\n    background: var(--color-tree-selected-line);\n  }*/\n}\n.spaces-tree-item:hover > .spaces-tree-item-actions {\n  transition-delay: 0.2s;\n  opacity: 1;\n}\n.spaces-tree-item.is-active {\n  background: var(--color-tree-selected);\n  font-weight: bold;\n}\n.spaces-tree-item.is-active .spaces-tree-item-text span {\n  font-weight: 400;\n}\n.spaces-tree-item.is-active .spaces-tree-item-text span:first-child {\n  font-weight: 700;\n}\n.spaces-tree-item:hover .spaces-tree-item-icon {\n  color: var(--color-text);\n}\n.spaces-tree-item.is-active .spaces-tree-item-icon {\n  color: var(--color-primary);\n}\n.spaces-tree-item-text {\n  display: flex;\n  flex-direction: column;\n}\n.spaces-tree-item-text .-minor {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}\n.spaces-tree-item-icon {\n  font-size: 18px;\n  line-height: 1;\n  font-weight: 400;\n  position: relative;\n  top: -2px;\n  color: var(--color-text-dim);\n  transition: color 0.2s ease;\n}";
const script = {
  data: () => ({
    spaces: [],
    loaded: false,
    component: null,
    space: null,
    spaceConfig: {},
    resizable: {
      axis: "x",
      min: 260,
      max: 520,
      save: "spaces-tree",
      handle: ".ui-resizable"
    }
  }),
  computed: {
    isOverview() {
      return !this.$route.params.alias;
    }
  },
  watch: {
    $route: "loadSpace"
  },
  created() {
    SpacesApi.getAll().then((response) => {
      this.spaces = response;
      this.loadSpace();
    });
  },
  beforeRouteLeave(to, from, next) {
    if (this.$refs.comp && this.$refs.comp.beforeRouteLeave) {
      this.$refs.comp.beforeRouteLeave(to, from, next);
    } else {
      next();
    }
  },
  beforeRouteUpdate(to, from, next) {
    if (this.$refs.comp && this.$refs.comp.beforeRouteLeave) {
      this.$refs.comp.beforeRouteLeave(to, from, next);
    } else {
      next();
    }
  },
  methods: {
    loadSpace() {
      this.loaded = false;
      this.$nextTick(() => {
        if (this.isOverview) {
          const space = this.spaces[0];
          if (space) {
            this.$router.replace({
              name: "space",
              params: {alias: space.alias}
            });
          } else {
            this.space = null;
            this.component = null;
            this.loaded = true;
          }
          return;
        }
        this.space = this.spaces.find((space) => space.alias === this.$route.params.alias);
        if (this.space.view === "editor" || this.$route.params.id || this.$route.meta.create) {
          this.component = SpaceEditor;
        } else if (this.space.view === "list") {
          this.component = SpaceList;
        } else {
          throw "Not implemented. Custom space view";
        }
        this.loaded = true;
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
component.options.__file = "app/pages/spaces/spaces.vue";
var spaces = component.exports;
export default spaces;
