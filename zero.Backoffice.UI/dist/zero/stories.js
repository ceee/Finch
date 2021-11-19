import {n as normalizeComponent, b as MediaApi, S as Strings, U as UiEditor, R as AuthorsApi, V as TagsApi, W as StoriesApi, O as Overlay} from "./index.js";
import "./vendor.js";
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("router-link", {staticClass: "stories-item", attrs: {to: _vm.link}}, [_c("div", {staticClass: "stories-item-image"}, [_vm.image ? _c("img", {attrs: {src: _vm.image}}) : _c("i", {staticClass: "fth-file-text"})]), _c("div", [_c("strong", {staticClass: "stories-item-name"}, [_vm._v(" " + _vm._s(_vm.value.name) + " ")]), _c("span", {staticClass: "stories-item-line"}, [_c("span", {staticClass: "stories-item-date"}, [_vm._v(_vm._s(_vm.date))]), _vm.value.excerpt ? [_vm._v(" | " + _vm._s(_vm.value.excerpt))] : _vm._e()], 2), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.status, expression: "status"}], staticClass: "stories-item-status"})])]);
};
var staticRenderFns$3 = [];
var storiesItem_vue_vue_type_style_index_0_lang = "a.stories-item {\n  display: grid;\n  grid-template-columns: auto minmax(0, 1fr);\n  flex-direction: row;\n  align-items: center;\n  justify-content: flex-start;\n  text-align: left;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n}\n.stories-item-image {\n  background: var(--color-box-nested);\n  border-radius: var(--radius);\n  width: 180px;\n  height: 120px;\n  padding: 0;\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  margin-right: var(--padding);\n}\n.stories-item-image i {\n  font-size: 28px;\n}\n.stories-item-image img {\n  border-radius: var(--radius);\n  object-fit: cover;\n  width: 100%;\n  height: 100%;\n}\n.stories-item-name {\n  display: block;\n  font-size: var(--font-size-m);\n  margin-bottom: 5px;\n  margin-top: -2px;\n}\n.stories-item-date {\n  color: var(--color-text);\n}\n.stories-item-line {\n  color: var(--color-text-dim);\n  display: block;\n  max-width: 620px;\n  text-overflow: ellipsis;\n  overflow: hidden;\n  -webkit-box-orient: vertical;\n  -webkit-line-clamp: 2;\n  display: -webkit-box;\n}\n.stories-item-line strong {\n  color: var(--color-text);\n}\n.stories-item-status {\n  display: inline-block;\n  font-size: 10px;\n  font-weight: 600;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  margin-top: var(--padding-s);\n  letter-spacing: 0.5px;\n  margin-left: -2px;\n}";
const script$3 = {
  props: {
    value: {
      type: Object,
      default: () => {
      }
    }
  },
  computed: {
    link() {
      return {
        name: "stories-edit",
        params: {id: this.value.id}
      };
    },
    image() {
      return this.value.imageId != null ? MediaApi.getImageSource(this.value.imageId, false) : null;
    },
    status() {
      if (this.value.publishDate && new Date(this.value.publishDate) > new Date()) {
        return "@stories.state.scheduled";
      }
      return !this.value.isActive ? "@stories.state.draft" : "@stories.state.published";
    },
    date() {
      return Strings.date(this.value.date);
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
component$3.options.__file = "../zero.Stories/Plugin/pages/stories-item.vue";
var StoriesItem = component$3.exports;
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "stories-author", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@stories.author.name", expression: "'@stories.author.name'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "stories-author-overlay"}, [_c("ui-editor", {attrs: {config: "story.author", meta: _vm.meta, disabled: _vm.disabled, infos: "none"}, model: {value: _vm.item, callback: function($$v) {
      _vm.item = $$v;
    }, expression: "item"}})], 1), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, state: form.state, label: "@ui.save"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}}), !_vm.disabled && _vm.model.id ? _c("ui-button", {staticStyle: {float: "right"}, attrs: {type: "light", label: "@ui.delete"}, on: {click: _vm.onDelete}}) : _vm._e()], 1)];
  }}], null, false, 412452267)}) : _vm._e();
};
var staticRenderFns$2 = [];
var author_vue_vue_type_style_index_0_lang = ".stories-author {\n  text-align: left;\n}\n.stories-author-overlay {\n  margin-top: var(--padding);\n  padding: 0;\n}\n.stories-author-overlay .ui-property + .ui-property,\n.stories-author-overlay .ui-split + .ui-property {\n  margin-top: var(--padding-m);\n}\n.stories-author-overlay .ui-box {\n  box-shadow: none;\n  padding: 0;\n  background: none;\n  margin: 0;\n}\n.stories-author-overlay .editor {\n  padding: 0;\n}\n.stories-author-overlay .ui-property {\n  flex-direction: column;\n  border-top: none;\n}\n.stories-author-overlay .ui-property .ui-property-label {\n  width: 100%;\n  padding-right: 0;\n}\n.stories-author-overlay .ui-property .ui-property-content {\n  margin-top: 5px;\n}";
const script$2 = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false,
    meta: {},
    item: {key: null, value: null},
    disabled: false
  }),
  components: {UiEditor},
  methods: {
    onLoad(form) {
      form.load(!this.model.id ? AuthorsApi.getEmpty() : AuthorsApi.getById(this.model.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.item = response.entity;
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(AuthorsApi.save(this.item)).then((res) => {
        this.config.confirm(res);
      });
    },
    onDelete() {
      this.$refs.form.onDelete(AuthorsApi.delete.bind(this, this.item.id));
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
component$2.options.__file = "../zero.Stories/Plugin/pages/overlays/author.vue";
var AuthorOverlay = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "stories-tag", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@stories.tag.name", expression: "'@stories.tag.name'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "stories-tag-items"}, [_c("ui-editor", {attrs: {config: "story.tag", meta: _vm.meta, disabled: _vm.disabled, infos: "none"}, model: {value: _vm.item, callback: function($$v) {
      _vm.item = $$v;
    }, expression: "item"}})], 1), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, state: form.state, label: "@ui.save"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}}), !_vm.disabled && _vm.model.id ? _c("ui-button", {staticStyle: {float: "right"}, attrs: {type: "light", label: "@ui.delete"}, on: {click: _vm.onDelete}}) : _vm._e()], 1)];
  }}], null, false, 12300727)}) : _vm._e();
};
var staticRenderFns$1 = [];
var tag_vue_vue_type_style_index_0_lang = ".stories-tag {\n  text-align: left;\n}\n.stories-tag-items {\n  margin-top: var(--padding);\n}\n.stories-tag-items .ui-property + .ui-property,\n.stories-tag-items .ui-split + .ui-property {\n  margin-top: var(--padding-m);\n}\n.stories-tag-items .editor {\n  padding: 0;\n}\n.stories-tag-items .ui-box {\n  box-shadow: none;\n  padding: 0;\n  background: none;\n  margin: 0;\n}\n.stories-tag-items .ui-property {\n  flex-direction: column;\n  border-top: none;\n}\n.stories-tag-items .ui-property .ui-property-label {\n  width: 100%;\n  padding-right: 0;\n}\n.stories-tag-items .ui-property .ui-property-content {\n  margin-top: 5px;\n}";
const script$1 = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false,
    meta: {},
    item: {key: null, value: null},
    disabled: false
  }),
  components: {UiEditor},
  methods: {
    onLoad(form) {
      form.load(!this.model.id ? TagsApi.getEmpty() : TagsApi.getById(this.model.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.item = response.entity;
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(TagsApi.save(this.item)).then((res) => {
        this.config.confirm(res);
      });
    },
    onDelete() {
      this.$refs.form.onDelete(TagsApi.delete.bind(this, this.item.id));
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
component$1.options.__file = "../zero.Stories/Plugin/pages/overlays/tag.vue";
var TagOverlay = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "stories"}, [_c("div", [_c("ui-header-bar", {attrs: {title: "@stories.story.list", count: _vm.stories.count}}), _c("main", {staticClass: "ui-blank-box stories-main"}, [_c("div", {staticClass: "ui-box"}, [_c("ui-add-button", {attrs: {route: "stories-create", label: "Add a story"}}), _c("hr", {staticClass: "ui-line"}), _c("ui-datagrid", {staticClass: "stories-items", on: {count: function($event) {
    _vm.stories.count = $event;
  }}, model: {value: _vm.stories, callback: function($$v) {
    _vm.stories = $$v;
  }, expression: "stories"}})], 1), _c("aside", {staticClass: "stories-aside"}, [_c("div", {staticClass: "stories-aside-box"}, [_c("h3", {staticClass: "ui-headline"}, [_vm._v("Authors")]), _vm._l(_vm.authors, function(author) {
    return _c("ui-select-button", {key: author.id, staticClass: "stories-authors-item", attrs: {"icon-as-image": true, icon: author.avatarId, label: author.name}, on: {click: function($event) {
      return _vm.editAuthor(author);
    }}});
  })], 2), _c("br"), _c("br"), _c("div", {staticClass: "stories-aside-box stories-tags"}, [_c("h3", {staticClass: "ui-headline"}, [_vm._v("Tags")]), _vm._l(_vm.tags, function(tag) {
    return _c("ui-select-button", {key: tag.id, staticClass: "stories-authors-item", attrs: {icon: "fth-tag", label: tag.name}, on: {click: function($event) {
      return _vm.editTag(tag);
    }}});
  })], 2)])])], 1)]);
};
var staticRenderFns = [];
var stories_vue_vue_type_style_index_0_lang = ".stories {\n  max-width: 1550px;\n  /*margin: 0 auto;\n  position: relative;*/\n}\n.stories-aside {\n  width: 360px;\n  margin-top: 0;\n  border-left: 1px dashed var(--color-line-dashed);\n  padding-left: var(--padding);\n}\n.stories .ui-datagrid-items {\n  display: block;\n}\n\n/*.stories-items .ui-datagrid-item + .ui-datagrid-item \n{\n  margin-top: var(--padding); \n}*/\n.stories-items .ui-datagrid-item + .ui-datagrid-item {\n  border-top: 1px solid var(--color-line);\n  margin-top: 20px;\n  padding-top: 20px;\n}\n.stories-aside-box {\n  margin-top: 2px;\n  margin-left: 0;\n}\n.stories .ui-header-bar {\n  /* hi */\n}\n.stories-aside .ui-line {\n  margin: var(--padding) 0;\n}\n.stories-aside .ui-button.type-big {\n  justify-content: center;\n  height: 60px;\n  margin-bottom: var(--padding-m);\n}\n.stories-aside-box .ui-headline {\n  margin-bottom: var(--padding-s);\n}\n.stories-main {\n  display: grid;\n  grid-template-columns: minmax(0, 1fr) auto;\n  grid-gap: var(--padding);\n}\n.stories-main .ui-pagination-button {\n  box-shadow: none !important;\n  background: var(--color-box-nested) !important;\n}\n.stories-main .ui-box {\n  margin: 0;\n}\n.stories-authors-item {\n  display: flex !important;\n  margin-top: 16px;\n  /* hi */\n}\n.stories-authors-item .ui-select-button-icon.is-image, .stories-authors-item .ui-select-button-icon {\n  padding: 0;\n  border-radius: 50px;\n}\n.stories-authors-item .ui-select-button-icon.is-image img, .stories-authors-item .ui-select-button-icon img {\n  border-radius: 50px;\n}\n.stories-authors-item.is-add {\n  margin-top: 16px;\n}\n.stories-authors-item.is-add .ui-select-button-icon {\n  border-radius: 50px;\n  box-shadow: var(--shadow-short);\n  background: var(--color-box);\n}\n.stories-authors-item.is-add .ui-select-button-label {\n  font-weight: 600;\n}\n.stories-tags .ui-select-button-icon {\n  background: none !important;\n  box-shadow: none !important;\n  width: auto;\n  height: auto;\n  margin-left: 2px;\n}";
const script = {
  data: () => ({
    stories: {
      count: 0,
      component: StoriesItem,
      items: StoriesApi.getByQuery,
      block: true
    },
    authors: [],
    tags: []
  }),
  mounted() {
    AuthorsApi.getTop(3).then((items) => this.authors = items);
    TagsApi.getTop(5).then((items) => this.tags = items);
  },
  methods: {
    editAuthor(item) {
      Overlay.open({
        component: AuthorOverlay,
        width: 600,
        model: item ? {id: item.id} : {}
      }).then((res) => {
      }, () => {
      });
    },
    editTag(item) {
      Overlay.open({
        component: TagOverlay,
        width: 600,
        model: item ? {id: item.id} : {}
      }).then((res) => {
      }, () => {
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
component.options.__file = "../zero.Stories/Plugin/pages/stories.vue";
var stories = component.exports;
export default stories;
