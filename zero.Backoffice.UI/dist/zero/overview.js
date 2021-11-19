import {n as normalizeComponent, M as MediaFolderApi, b as MediaApi, N as Notification, O as Overlay, F as FolderOverlay, c as UploadStatusOverlay, h as hub} from "./index.js";
import {S as SelectOverlay} from "./select-overlay.js";
import "./vendor.js";
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "pages-move", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@ui.move.title", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.move.action", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: "@ui.move.text", tokens: {name: _vm.model.name}}, expression: "{ key: '@ui.move.text', tokens: { name: model.name } }", arg: "html"}], staticClass: "pages-move-text"}), _c("div", {staticClass: "ui-box pages-move-items"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems}, on: {select: _vm.onSelect}})], 1)]);
};
var staticRenderFns$3 = [];
var move_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.pages-move .ui-box {\n  margin: 0;\n  padding: 16px 0;\n}\n.pages-move .ui-box .ui-tree-item.is-disabled {\n  opacity: 0.5;\n}\n.pages-move .ui-box .ui-tree-item.is-selected, .pages-move .ui-box .ui-tree-item:hover:not(.is-disabled) {\n  background: var(--color-tree-selected);\n}\n.pages-move .ui-box .ui-tree-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n}\n.pages-move .ui-box .ui-tree-item.is-selected .ui-tree-item-text {\n  font-weight: bold;\n}\n.pages-move content {\n  padding-top: 0;\n}\n.pages-move-text {\n  margin: 0 0 20px;\n}';
const script$3 = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    items: [],
    selected: null,
    state: "default",
    cache: {},
    prevItem: null,
    selected: null
  }),
  mounted() {
    this.selected = this.model;
  },
  methods: {
    onSelect(item) {
      item.isSelected = true;
      if (this.prevItem && this.prevItem.id != item.id) {
        this.prevItem.isSelected = false;
      }
      this.prevItem = item;
      this.selected = item;
    },
    getItems(parent) {
      const key = !parent ? "__root" : parent;
      if (this.cache[key]) {
        return Promise.resolve(this.cache[key]);
      }
      return MediaFolderApi.getAllAsTree(parent).then((response) => {
        if (!parent) {
          response.splice(0, 0, {
            id: null,
            parentId: null,
            sort: 0,
            name: "@page.root",
            icon: "fth-arrow-down-circle",
            isOpen: false,
            modifier: null,
            hasChildren: false,
            childCount: 0,
            isInactive: false,
            hasActions: false
          });
        }
        response.forEach((item) => {
          item.isSelected = this.model.parentId == item.id;
          if (item.isSelected) {
            this.prevItem = item;
          }
          item.disabled = item.id === "recyclebin" || item.id == this.model.id;
          item.hasActions = false;
        });
        this.cache[key] = response;
        return response;
      });
    },
    onSave() {
      if (this.model.parentId == this.selected.id) {
        this.config.close();
        return;
      }
      this.state = "loading";
      (this.config.isFolder ? MediaFolderApi : MediaApi).move(this.model.id, this.selected.id).then((res) => {
        if (res.success) {
          this.state = "success";
          this.config.confirm(res.model);
        } else {
          this.state = "error";
          Notification.error(res.errors[0].message);
        }
      });
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
component$3.options.__file = "app/pages/media/overlays/move.vue";
var MoveOverlay = component$3.exports;
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "ui-media-overlay", on: {submit: _vm.onSubmit}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@media.name", expression: "'@media.name'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "ui-media-overlay-preview"}, [_vm.model.source ? _c("img", {staticClass: "ui-media-overlay-preview-image", attrs: {src: _vm.model.source, alt: _vm.model.name}}) : _vm._e()]), _c("div", {staticClass: "ui-media-overlay-infos ui-split-three"}, [_c("ui-property", {attrs: {label: "@media.fields.size", vertical: true, "is-text": true}}, [_c("span", {directives: [{name: "filesize", rawName: "v-filesize", value: _vm.model.size, expression: "model.size"}]})]), _vm.model.lastModifiedDate ? _c("ui-property", {attrs: {label: "@media.fields.date", vertical: true, "is-text": true}}, [_c("ui-date", {model: {value: _vm.model.lastModifiedDate, callback: function($$v) {
      _vm.$set(_vm.model, "lastModifiedDate", $$v);
    }, expression: "model.lastModifiedDate"}})], 1) : _vm._e(), _vm.model.dimension ? _c("ui-property", {attrs: {label: "@media.fields.dimension", vertical: true, "is-text": true}}, [_vm._v(" " + _vm._s(_vm.model.dimension.width) + " \xD7 " + _vm._s(_vm.model.dimension.height) + " ")]) : _vm._e()], 1), _c("div", {staticClass: "ui-media-overlay-fields"}, [_c("ui-property", {attrs: {label: "@ui.name", required: true, vertical: true, "is-text": true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.model.name, expression: "model.name"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "160", readonly: true}, domProps: {value: _vm.model.name}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.model, "name", $event.target.value);
    }}})]), _c("ui-property", {attrs: {label: "@media.fields.alternativeText", vertical: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.model.alternativeText, expression: "model.alternativeText"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "160", readonly: _vm.disabled}, domProps: {value: _vm.model.alternativeText}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.model, "alternativeText", $event.target.value);
    }}})]), _c("ui-property", {attrs: {label: "@media.fields.caption", vertical: true}}, [_c("textarea", {directives: [{name: "model", rawName: "v-model", value: _vm.model.caption, expression: "model.caption"}], staticClass: "ui-input", attrs: {readonly: _vm.disabled}, domProps: {value: _vm.model.caption}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.model, "caption", $event.target.value);
    }}})])], 1), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "action", submit: true, state: form.state, label: "@ui.save"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}}), !_vm.disabled && !_vm.isCreate ? _c("ui-button", {staticStyle: {float: "right"}, attrs: {type: "light", label: "@ui.remove"}, on: {click: _vm.onDelete}}) : _vm._e()], 1)];
  }}], null, false, 1961379330)}) : _vm._e();
};
var staticRenderFns$2 = [];
var create_vue_vue_type_style_index_0_lang = ".ui-media-overlay {\n  text-align: left;\n}\n.ui-media-overlay-preview {\n  height: 200px;\n  background: var(--color-bg-mid);\n  border-radius: var(--radius) var(--radius) 0 0;\n  margin-top: var(--padding);\n  padding: 10px;\n  overflow: hidden;\n}\n.ui-media-overlay-preview-image {\n  border-radius: 3px;\n  max-width: 100%;\n  max-height: 100%;\n  margin: auto;\n  display: block;\n  color: transprent;\n  overflow: hidden;\n  font-size: 0;\n  position: relative;\n  z-index: 2;\n}\n.ui-media-overlay-infos {\n  background: var(--color-bg-mid);\n  border-radius: 0 0 var(--radius) var(--radius);\n  padding: 15px 20px;\n}\n.ui-media-overlay-infos .ui-property-content {\n  color: var(--color-text-dim);\n}\n.ui-media-overlay-fields {\n  margin-top: var(--padding);\n}\n.ui-media-overlay-fields .ui-property + .ui-property, .ui-media-overlay-fields .ui-split + .ui-property {\n  margin-top: 0;\n}\n\n/*.translation-items\n{\n  margin-top: var(--padding);\n\n  .ui-property + .ui-property,\n  .ui-split + .ui-property\n  {\n    margin-top: 0;\n  }\n}*/";
const script$2 = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false
  }),
  computed: {
    disabled() {
      return this.config.disabled;
    },
    isCreate() {
      return this.config.isCreate;
    }
  },
  methods: {
    onSubmit() {
      this.config.confirm(this.model);
    },
    onDelete() {
      this.config.confirm({
        deletionRequested: true
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
component$2.options.__file = "app/pages/media/overlays/create.vue";
var CreateItemOverlay = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("router-link", {staticClass: "media-item", attrs: {to: _vm.link}}, [_c("div", {staticClass: "media-item-preview", class: {"media-pattern": _vm.value.image, "is-covered": _vm.covered}}, [_c("span", {staticClass: "media-item-check"}, [_c("ui-icon", {attrs: {symbol: "fth-check", size: 14}})], 1), _vm.value.image ? _c("img", {staticClass: "media-item-image", attrs: {src: _vm.value.image}}) : _vm._e(), !_vm.value.image ? _c("span", {staticClass: "media-item-icon"}, [_c("ui-icon", {attrs: {symbol: _vm.value.isFolder ? "fth-folder" : "fth-file", size: 36, "stroke-width": 1.5}})], 1) : _vm._e()]), _c("p", {staticClass: "media-item-text"}, [_c("span", {attrs: {title: _vm.value.name}}, [_vm._v(_vm._s(_vm.value.name) + " "), _vm.value.isShared ? _c("ui-icon", {staticClass: "media-item-shared", attrs: {symbol: "fth-cloud", size: 15}}) : _vm._e()], 1), !_vm.value.isFolder ? _c("span", {staticClass: "-minor"}, [_c("br"), _c("span", {directives: [{name: "filesize", rawName: "v-filesize", value: _vm.value.size, expression: "value.size"}]})]) : _vm._e(), _vm.value.isFolder ? _c("span", {staticClass: "-minor"}, [_c("br"), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: _vm.value.children === 1 ? "@media.child_count_1" : "@media.child_count_x", tokens: {count: _vm.value.children}}, expression: "{ key: value.children === 1 ? '@media.child_count_1' : '@media.child_count_x', tokens: { count: value.children }}"}]})]) : _vm._e()])]);
};
var staticRenderFns$1 = [];
var item_vue_vue_type_style_index_0_lang = ".media-item {\n  width: 100%;\n  min-height: 200px;\n  display: grid;\n  grid-template-rows: auto 1fr;\n  gap: 10px;\n  align-items: center;\n  line-height: 1.4;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  border-radius: var(--radius);\n}\n.media-items.is-selecting .media-item {\n  opacity: 0.5;\n}\n.media-items.is-selecting .media-item.is-selected {\n  opacity: 1;\n}\n.media-item-preview {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  flex-direction: column;\n  height: 200px;\n  width: 100%;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  overflow: visible !important;\n  position: relative;\n  text-align: center;\n  box-shadow: var(--shadow-short);\n}\n.media-item-image {\n  width: 100%;\n  height: 100%;\n  object-fit: contain;\n  position: relative;\n  border-radius: var(--radius);\n  z-index: 1;\n}\n.media-item-preview.is-covered .media-item-image {\n  object-fit: cover;\n}\n.media-item-check {\n  display: none;\n  justify-content: center;\n  align-items: center;\n  width: 30px;\n  height: 30px;\n  border-radius: 20px;\n  border: 5px solid var(--color-bg);\n  position: absolute;\n  z-index: 2;\n  left: -13px;\n  top: -13px;\n  background: var(--color-primary);\n  color: var(--color-primary-text);\n  box-shadow: 1px 1px 0 1px var(--color-shadow);\n  font-size: 11px;\n}\n.is-selected .media-item-check {\n  display: inline-flex;\n}\n.media-item-text {\n  white-space: nowrap;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  margin: 0;\n  padding-right: 16px;\n  font-weight: bold;\n}\n.media-item-text .-minor {\n  font-weight: 400;\n  color: var(--color-text-dim);\n}\n.media-item-shared {\n  margin-left: 0.4em;\n  color: var(--color-synchronized);\n  position: relative;\n  top: 2px;\n}";
const script$1 = {
  name: "mediaItem",
  props: {
    value: {
      type: Object,
      default: () => {
      }
    },
    selected: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    link() {
      return {
        name: this.value.isFolder ? "media" : "media-edit",
        params: {
          id: this.value.id
        },
        query: this.$route.query
      };
    },
    covered() {
      return this.value.aspectRatio < 1.2 && this.value.aspectRatio > 0.8;
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
component$1.options.__file = "app/pages/media/item.vue";
var MediaItem = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "media-content"}, [_c("ui-header-bar", {attrs: {"back-button": !!_vm.id, count: _vm.count}, scopedSlots: _vm._u([{key: "title", fn: function() {
    return [_c("h2", {staticClass: "ui-header-bar-title"}, _vm._l(_vm.hierarchy, function(item, index) {
      return _c("span", {key: item.id, staticClass: "media-items-hierarchy-item"}, [_c("router-link", {directives: [{name: "localize", rawName: "v-localize", value: item.name, expression: "item.name"}], attrs: {to: {name: "media", params: {id: item.id}}}}), index < _vm.hierarchy.length - 1 ? _c("ui-icon", {attrs: {symbol: "fth-chevron-right"}}) : _vm._e()], 1);
    }), 0)];
  }, proxy: true}])}, [[!_vm.selecting ? [_c("ui-search", {staticClass: "onbg", model: {value: _vm.gridConfig.search, callback: function($$v) {
    _vm.$set(_vm.gridConfig, "search", $$v);
  }, expression: "gridConfig.search"}}), !!_vm.id ? _c("ui-dropdown", {attrs: {align: "right", disabled: !!_vm.gridConfig.search}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@media.actions.folderdropdown", caret: "down", disabled: !!_vm.gridConfig.search}})];
  }, proxy: true}], null, false, 1337220338)}, [_c("ui-dropdown-button", {attrs: {label: "@ui.edit.title", icon: "fth-edit-2"}, on: {click: function($event) {
    return _vm.edit(_vm.current, true);
  }}}), _c("ui-dropdown-button", {attrs: {label: "@ui.move.title", icon: "fth-corner-down-right"}, on: {click: function($event) {
    return _vm.move(_vm.current, true);
  }}}), _c("ui-dropdown-separator"), _c("ui-dropdown-button", {attrs: {label: "@ui.delete", icon: "fth-trash"}, on: {click: function($event) {
    return _vm.remove(_vm.current, true);
  }}})], 1) : _vm._e(), _c("ui-button", {attrs: {type: "primary", label: "@ui.add"}, on: {click: _vm.add}})] : _vm._e(), _vm.selecting ? [_c("ui-button", {attrs: {type: "blank", label: "@media.selection.clear"}, on: {click: _vm.clearSelection}}), _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "primary", label: _vm.selectedText, caret: "down"}})];
  }, proxy: true}], null, false, 1719067584)}, [_c("ui-dropdown-button", {attrs: {label: "@ui.move.title", icon: "fth-corner-down-right"}, on: {click: function($event) {
    return _vm.move(_vm.current, true);
  }}}), _c("ui-dropdown-button", {attrs: {label: "@ui.delete", icon: "fth-trash"}, on: {click: function($event) {
    return _vm.remove(_vm.current, true);
  }}})], 1)] : _vm._e()]], 2), _c("div", {staticClass: "ui-view-box"}, [_c("div", {staticClass: "media-items", class: {"is-selecting": _vm.selecting}}, [_c("ui-datagrid", {ref: "grid", on: {select: _vm.onSelected, count: function($event) {
    _vm.count = $event;
  }}, scopedSlots: _vm._u([{key: "actions", fn: function(props) {
    return [props.item && props.item.isFolder ? _c("ui-dropdown-button", {attrs: {label: "@ui.open.title", icon: "fth-arrow-right"}, on: {click: function($event) {
      return _vm.goToFolder(props.item.id);
    }}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@ui.edit.title", icon: "fth-edit-2"}, on: {click: function($event) {
      return _vm.edit(props.item, props.item.isFolder);
    }}}), _c("ui-dropdown-button", {attrs: {label: "@ui.move.title", icon: "fth-corner-down-right"}, on: {click: function($event) {
      return _vm.move(props.item, props.item.isFolder);
    }}}), _c("ui-dropdown-button", {attrs: {label: "Select", icon: "fth-check-circle"}, on: {click: function($event) {
      return _vm.$refs.grid.select(props.item);
    }}}), _c("ui-dropdown-separator"), _c("ui-dropdown-button", {attrs: {label: "@ui.delete", icon: "fth-trash"}, on: {click: function($event) {
      return _vm.remove(props.item, props.item.isFolder);
    }}})];
  }}]), model: {value: _vm.gridConfig, callback: function($$v) {
    _vm.gridConfig = $$v;
  }, expression: "gridConfig"}})], 1)])], 1);
};
var staticRenderFns = [];
var overview_vue_vue_type_style_index_0_lang = ".media {\n  width: 100%;\n  height: 100vh;\n  overflow-y: auto;\n}\n.media-content {\n  height: 100vh;\n  overflow-y: auto;\n}\n.media-items .ui-datagrid-items {\n  gap: var(--padding);\n}\ninput[type=file].media-item-upload {\n  position: absolute;\n  height: 100%;\n  top: 0;\n  left: 0;\n  width: 100%;\n  z-index: 1;\n  bottom: 0;\n  opacity: 0.001;\n  cursor: pointer;\n}\n.media-items-hierarchy-item {\n  font-family: var(--font);\n  margin: 0;\n  font-size: var(--font-size-l);\n  font-weight: 400;\n  color: var(--color-text-dim);\n}\n.media-items-hierarchy-item a {\n  color: var(--color-text-dim);\n}\n.media-items-hierarchy-item:last-child a {\n  font-weight: 700;\n  color: var(--color-text);\n}\n.media-items-hierarchy-item a:hover {\n  color: var(--color-text);\n}";
const script = {
  props: ["id", "scope"],
  data: () => ({
    count: 0,
    current: null,
    hierarchy: [],
    gridConfig: {
      search: null,
      width: 180,
      component: MediaItem,
      selectable: true
    },
    selectedCount: 0
  }),
  computed: {
    selectedText() {
      return this.selectedCount + " selected";
    },
    selecting() {
      return this.selectedCount > 0;
    }
  },
  watch: {
    search(value) {
      this.$refs.grid.debouncedUpdate();
    },
    $route: function(val) {
      this.initialize();
    }
  },
  created() {
    this.gridConfig.items = this.getItems;
    this.initialize();
  },
  methods: {
    initialize() {
    },
    getItems(query) {
      if (!query) {
        query = {};
      }
      query.search = this.gridConfig.search;
      query.folderId = this.$route.params.id;
      query.searchIsGlobal = true;
      this.getFolderHierarchy(query.folderId, !!query.search);
      return MediaApi.getListByQuery(query).then((response) => {
        return Promise.resolve(response);
      });
    },
    getFolderHierarchy(id, isSearch) {
      if (!id) {
        this.current = {
          id: null,
          name: "@media.list"
        };
        this.hierarchy = [this.current];
        return;
      }
      if (isSearch) {
        this.current = {
          id: null,
          name: "Search results"
        };
        this.hierarchy = [this.current];
        return;
      }
      MediaFolderApi.getHierarchy(id).then((res) => {
        res.splice(0, 0, {
          id: null,
          name: "@media.list"
        });
        this.hierarchy = res;
        this.current = res[res.length - 1];
      });
    },
    goToFolder(id) {
      this.$router.push({
        name: "media",
        params: !id ? {} : {id}
      });
    },
    add() {
      Overlay.open({
        component: SelectOverlay,
        width: 480,
        theme: "dark",
        items: [
          {
            name: "@media.actions.addfile",
            description: "@media.actions.addfile_text",
            icon: "fth-upload-cloud"
          },
          {
            name: "@media.actions.addfolder",
            description: "@media.actions.addfolder_text",
            icon: "fth-folder-plus"
          }
        ]
      }).then((item) => {
      }, () => {
      });
    },
    addFolder(parentId) {
      Overlay.open({
        component: FolderOverlay,
        model: {parentId},
        theme: "dark"
      }).then((item) => this.goToFolder(item.model.id));
    },
    addFile(folderId) {
      let options = {
        title: "@iconpicker.title",
        closeLabel: "@ui.close",
        component: CreateItemOverlay,
        isCreate: true,
        model: {
          folderId
        },
        theme: "dark",
        width: 520
      };
      return Overlay.open(options).then((value) => {
        console.info(value);
      }, () => {
      });
    },
    onUpload(event) {
      let options = {
        title: "Upload status",
        closeLabel: "@ui.close",
        component: UploadStatusOverlay,
        isCreate: true,
        model: event.target.files,
        folderId: this.id,
        theme: "dark",
        width: 520
      };
      return Overlay.open(options).then((value) => {
        console.info(value);
      }, () => {
      });
    },
    onSelected(items) {
      this.selectedCount = items.length;
    },
    edit(item, isFolder) {
      if (!isFolder) {
        return this.$router.push({
          name: "media-edit",
          params: {id: item.id}
        });
      }
      Overlay.open({
        component: FolderOverlay,
        model: item,
        theme: "dark"
      }).then((res) => {
        if (res.deleted === true) {
          return this.remove(item, true);
        } else {
          hub.$emit("media.update");
          this.$refs.grid.update();
        }
      });
    },
    move(item, isFolder) {
      return Overlay.open({
        component: MoveOverlay,
        display: "editor",
        model: item,
        isFolder
      }).then((value) => {
        hub.$emit("page.move", value);
        hub.$emit("page.update");
        this.$refs.grid.update();
      });
    },
    remove(item, isFolder) {
      Overlay.confirmDelete(item.name, isFolder ? "@media.deleteoverlay.folder_text" : "@deleteoverlay.text").then((opts) => {
        opts.state("loading");
        (isFolder ? MediaFolderApi : MediaApi).delete(item.id).then((response) => {
          if (response.success) {
            opts.state("success");
            opts.hide();
            hub.$emit("media.delete", response.model);
            hub.$emit("media.update");
            Notification.success("@deleteoverlay.success", isFolder ? "@media.deleteoverlay.folder_success_text" : "@deleteoverlay.success_text");
            this.$refs.grid.update();
            if (isFolder && item.id === this.id) {
              this.$router.go(-1);
            } else {
              this.$refs.grid.update();
            }
          } else {
            opts.errors(response.errors);
          }
        });
      });
    },
    clearSelection() {
      if (this.$refs.grid) {
        this.$refs.grid.clearSelection();
      }
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
component.options.__file = "app/pages/media/overview.vue";
var overview = component.exports;
export default overview;
