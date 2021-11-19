var __defProp = Object.defineProperty;
var __hasOwnProp = Object.prototype.hasOwnProperty;
var __getOwnPropSymbols = Object.getOwnPropertySymbols;
var __propIsEnum = Object.prototype.propertyIsEnumerable;
var __defNormalProp = (obj, key, value) => key in obj ? __defProp(obj, key, {enumerable: true, configurable: true, writable: true, value}) : obj[key] = value;
var __assign = (a, b) => {
  for (var prop in b || (b = {}))
    if (__hasOwnProp.call(b, prop))
      __defNormalProp(a, prop, b[prop]);
  if (__getOwnPropSymbols)
    for (var prop of __getOwnPropSymbols(b)) {
      if (__propIsEnum.call(b, prop))
        __defNormalProp(a, prop, b[prop]);
    }
  return a;
};
import {d as dayjs, a as axios, f as filter$1, e as extend, b as each, V as Vue, c as find, i as isArray, g as isEmpty, h as debounce, m as max, D as Doc, j as clone, k as VueRouter, l as groupBy, n as map, P as Plugin$1, o as PluginKey, E as Extension, H as History, p as HardBreak, L as Link$1, B as Bold, C as Code, I as Italic, S as Strike, U as Underline, q as ListItem, r as BulletList, O as OrderedList, s as HorizontalRule, t as Heading, u as EditorContent, v as EditorMenuBubble, w as Placeholder, x as Editor$1, y as flatpickr, z as O__zero_zero_Web_UI_node_modules_qs_lib, A as Sortable} from "./vendor.js";
const p = function polyfill(modulePath = ".", importFunctionName = "__import__") {
  try {
    self[importFunctionName] = new Function("u", `return import(u)`);
  } catch (error) {
    const baseURL = new URL(modulePath, location);
    const cleanup = (script2) => {
      URL.revokeObjectURL(script2.src);
      script2.remove();
    };
    self[importFunctionName] = (url) => new Promise((resolve, reject) => {
      const absURL = new URL(url, baseURL);
      if (self[importFunctionName].moduleMap[absURL]) {
        return resolve(self[importFunctionName].moduleMap[absURL]);
      }
      const moduleBlob = new Blob([
        `import * as m from '${absURL}';`,
        `${importFunctionName}.moduleMap['${absURL}']=m;`
      ], {type: "text/javascript"});
      const script2 = Object.assign(document.createElement("script"), {
        type: "module",
        src: URL.createObjectURL(moduleBlob),
        onerror() {
          reject(new Error(`Failed to import: ${url}`));
          cleanup(script2);
        },
        onload() {
          resolve(self[importFunctionName].moduleMap[absURL]);
          cleanup(script2);
        }
      });
      document.head.appendChild(script2);
    });
    self[importFunctionName].moduleMap = {};
  }
};
p("/zero/assets/");
let scriptRel;
const seen = {};
const __vitePreload = function preload(baseModule, deps) {
  if (!deps) {
    return baseModule();
  }
  if (scriptRel === void 0) {
    const relList = document.createElement("link").relList;
    scriptRel = relList && relList.supports && relList.supports("modulepreload") ? "modulepreload" : "preload";
  }
  return Promise.all(deps.map((dep) => {
    if (dep in seen)
      return;
    seen[dep] = true;
    const isCss = dep.endsWith(".css");
    const cssSelector = isCss ? '[rel="stylesheet"]' : "";
    if (document.querySelector(`link[href="${dep}"]${cssSelector}`)) {
      return;
    }
    const link = document.createElement("link");
    link.rel = isCss ? "stylesheet" : scriptRel;
    if (!isCss) {
      link.as = "script";
      link.crossOrigin = "";
    }
    link.href = dep;
    document.head.appendChild(link);
    if (isCss) {
      return new Promise((res, rej) => {
        link.addEventListener("load", res);
        link.addEventListener("error", rej);
      });
    }
  })).then(() => baseModule());
};
class Plugin {
  constructor(name) {
    this.routes = [];
    this.editors = [];
    this.lists = [];
    this._name = name;
  }
  get name() {
    return this._name;
  }
  get install() {
    return this._onInstall;
  }
  set install(callback) {
    this._onInstall = callback;
  }
  addEditor(config2) {
    this.editors.push(config2);
  }
  addEditors(editors2) {
    let items = !Array.isArray(editors2) ? Object.values(editors2) : editors2;
    items.forEach((item2) => this.addEditor(item2));
  }
  addList(list2) {
    this.lists.push(list2);
  }
  addLists(lists2) {
    let items = !Array.isArray(lists2) ? Object.values(lists2) : lists2;
    items.forEach((item2) => this.addList(item2));
  }
  addRoute(route) {
    this.routes.push(route);
  }
  addRoutes(routes2) {
    routes2.forEach((route) => this.addRoute(route));
  }
}
const BYTE_UNIT = "B";
const UNITS = ["kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];
const DATE_FORMAT$2 = "DD.MM.YY";
const TIME_FORMAT = "HH:mm";
const DATETIME_FORMAT$2 = DATE_FORMAT$2 + " " + TIME_FORMAT;
var Strings = {
  guid(length) {
    var guid = ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) => (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16));
    if (length > 0) {
      return guid.replace(/-/g, "").substring(0, length);
    }
    return guid;
  },
  filesize(bytes) {
    if (typeof bytes !== "number") {
      return "0 " + BYTE_UNIT;
    }
    var thresh = 1024;
    if (Math.abs(bytes) < thresh) {
      return bytes + " " + BYTE_UNIT;
    }
    var u = -1;
    do {
      bytes /= thresh;
      ++u;
    } while (Math.abs(bytes) >= thresh && u < UNITS.length - 1);
    return bytes.toFixed(1) + " " + UNITS[u];
  },
  date(value, format) {
    if (!value) {
      return null;
    }
    format = format || DATE_FORMAT$2;
    if (format === "long") {
      format = DATETIME_FORMAT$2;
    } else if (format === "short" || format === "default") {
      format = DATE_FORMAT$2;
    } else if (format === "time") {
      format = TIME_FORMAT;
    }
    return dayjs(value).format(format);
  },
  selectorToArray(selector) {
    if (!selector) {
      return selector;
    }
    selector = selector.replace(/\[(\w+)\]/g, ".$1");
    selector = selector.replace(/^\./, "");
    return selector.split(".");
  },
  currency(value, decimals, hideSymbol, noEncode) {
    if (isNaN(value)) {
      value = 0;
    }
    var fixedDecimals = typeof decimals !== "undefined";
    decimals = !fixedDecimals ? 2 : decimals;
    var hasDecimals = ~~value !== value;
    var val = hasDecimals || fixedDecimals ? (value / 1).toFixed(decimals) : ~~value;
    if (val === "-0." + "0".repeat(decimals)) {
      val = "0." + "0".repeat(decimals);
    }
    return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, noEncode ? " " : "&nbsp;") + (hideSymbol === true ? "" : noEncode ? " \uFFFD" : "&nbsp;&euro;");
  },
  htmlToText(html) {
    let tag = document.createElement("div");
    tag.innerHTML = html;
    return tag.innerText;
  }
};
class EditorField {
  constructor(path, options2) {
    this.path = null;
    this.options = {
      label: null,
      hideLabel: false,
      description: null,
      helpText: null,
      condition: null,
      disabled: false,
      tab: null,
      allTabs: false,
      vertical: true,
      coreDatabase: false,
      fieldset: null,
      fieldsetColumns: null,
      class: "",
      onChange: null
    };
    this._preview = {
      icon: "fth-filter",
      preview: (x) => x,
      hasValue: (x) => !!x
    };
    this._component = null;
    this._componentOptions = {};
    this._required = false;
    this._isReadOnly = false;
    this.path = path;
    this.options = __assign(__assign({}, this.options), options2);
  }
  get component() {
    return this._component;
  }
  get componentOptions() {
    return this._componentOptions;
  }
  get isRequired() {
    return this._required;
  }
  get previewOptions() {
    return this._preview;
  }
  setBase(field) {
    this.path = field.path;
    this.options = __assign({}, field.options);
    this._preview = __assign({}, field.previewOptions);
    this._component = field.component;
    this._componentOptions = field.componentOptions;
    this._required = field.isRequired;
    return this;
  }
  _setComponent(component2, options2) {
    this._component = component2;
    this._componentOptions = options2 || {};
    return this;
  }
  cols(columnCount) {
    this.options.fieldsetColumns = columnCount < 1 ? 1 : columnCount > 12 ? 12 : columnCount;
    return this;
  }
  vertical(isVertical) {
    this.options.vertical = isVertical;
    return this;
  }
  disabled() {
    this.options.disabled = true;
    return this;
  }
  required(condition) {
    if (typeof condition === "function") {
      this._required = condition;
    } else if (typeof condition === "boolean") {
      this._required = condition;
    } else {
      this._required = true;
    }
    return this;
  }
  when(condition) {
    this.options.condition = condition;
    return this;
  }
  onChange(expression) {
    this.options.onChange = expression;
    return this;
  }
  custom(component2, options2) {
    return this._setComponent(component2, __assign({}, options2));
  }
  text(maxLength, placeholder2) {
    return this._setComponent(() => __vitePreload(() => __import__("./text.js"), true ? ["/zero/text.js","/zero/vendor.js"] : void 0), {maxLength, placeholder: placeholder2});
  }
  password(maxLength, placeholder2) {
    return this._setComponent(() => __vitePreload(() => __import__("./password.js"), true ? ["/zero/password.js","/zero/vendor.js"] : void 0), {maxLength, placeholder: placeholder2});
  }
  currency(placeholder2) {
    return this._setComponent(() => __vitePreload(() => __import__("./currency2.js"), true ? ["/zero/currency2.js","/zero/vendor.js"] : void 0), {placeholder: placeholder2});
  }
  number(maxLength, placeholder2) {
    return this._setComponent(() => __vitePreload(() => __import__("./number2.js"), true ? ["/zero/number2.js","/zero/vendor.js"] : void 0), {maxLength, placeholder: placeholder2});
  }
  rte(options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./rte.js"), true ? ["/zero/rte.js","/zero/vendor.js"] : void 0), __assign({}, options2));
  }
  select(items, options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./select.js"), true ? ["/zero/select.js","/zero/vendor.js"] : void 0), __assign({items}, options2));
  }
  textarea(maxLength) {
    return this._setComponent(() => __vitePreload(() => __import__("./textarea.js"), true ? ["/zero/textarea.js","/zero/vendor.js"] : void 0), {maxLength});
  }
  toggle(negative) {
    this.options.vertical = false;
    return this._setComponent(() => __vitePreload(() => __import__("./toggle.js"), true ? ["/zero/toggle.js","/zero/vendor.js"] : void 0), {negative});
  }
  output(render2) {
    this._isReadOnly = true;
    return this._setComponent(() => __vitePreload(() => __import__("./output.js"), true ? ["/zero/output.js","/zero/vendor.js"] : void 0), {render: render2});
  }
  alias(namePath) {
    return this._setComponent(() => __vitePreload(() => __import__("./alias.js"), true ? ["/zero/alias.js","/zero/vendor.js"] : void 0), {namePath});
  }
  checkList(items, options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./checklist.js"), true ? ["/zero/checklist.js","/zero/vendor.js"] : void 0), __assign({items}, options2));
  }
  colorPicker() {
    return this._setComponent(() => __vitePreload(() => __import__("./colorpicker.js"), true ? ["/zero/colorpicker.js","/zero/vendor.js"] : void 0));
  }
  countryPicker(limit) {
    return this._setComponent(() => __vitePreload(() => __import__("./countrypicker.js"), true ? ["/zero/countrypicker.js","/zero/vendor.js"] : void 0), {limit});
  }
  spacePicker(limit) {
    return this._setComponent(() => __vitePreload(() => __import__("./spacepicker.js"), true ? ["/zero/spacepicker.js","/zero/vendor.js"] : void 0), {limit});
  }
  culturePicker() {
    return this._setComponent(() => __vitePreload(() => __import__("./culturepicker.js"), true ? ["/zero/culturepicker.js","/zero/vendor.js"] : void 0));
  }
  mailTemplatePicker(limit) {
    return this._setComponent(() => __vitePreload(() => __import__("./mailtemplatepicker.js"), true ? ["/zero/mailtemplatepicker.js","/zero/vendor.js"] : void 0), {limit});
  }
  datePicker(options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./datepicker.js"), true ? ["/zero/datepicker.js","/zero/vendor.js"] : void 0), __assign({}, options2));
  }
  dateRangePicker(options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./daterangepicker.js"), true ? ["/zero/daterangepicker.js","/zero/vendor.js"] : void 0), __assign({}, options2));
  }
  iconPicker(iconSetAlias) {
    return this._setComponent(() => __vitePreload(() => __import__("./iconPicker.js"), true ? ["/zero/iconPicker.js","/zero/vendor.js"] : void 0), {set: iconSetAlias});
  }
  pagePicker(options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./pagepicker.js"), true ? ["/zero/pagepicker.js","/zero/vendor.js"] : void 0), __assign({}, options2));
  }
  linkPicker(options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./linkpicker.js"), true ? ["/zero/linkpicker.js","/zero/vendor.js"] : void 0), __assign({}, options2));
  }
  inputList(limit, maxItemLength, addLabel) {
    return this._setComponent(() => __vitePreload(() => __import__("./inputlist.js"), true ? ["/zero/inputlist.js","/zero/vendor.js"] : void 0), {limit, maxItemLength, addLabel});
  }
  tags(limit, maxItemLength) {
    return this._setComponent(() => __vitePreload(() => __import__("./tags2.js"), true ? ["/zero/tags2.js","/zero/vendor.js"] : void 0), {limit, maxItemLength});
  }
  languagePicker() {
    return this._setComponent(() => __vitePreload(() => __import__("./language2.js"), true ? ["/zero/language2.js","/zero/vendor.js"] : void 0));
  }
  modules(tags) {
    return this._setComponent(() => __vitePreload(() => __import__("./modules.js"), true ? ["/zero/modules.js","/zero/vendor.js"] : void 0), {tags});
  }
  nested(editor2, options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./nested.js"), true ? ["/zero/nested.js","/zero/vendor.js"] : void 0), __assign({editor: editor2}, options2));
  }
  state(items) {
    return this._setComponent(() => __vitePreload(() => __import__("./state.js"), true ? ["/zero/state.js","/zero/vendor.js"] : void 0), {items});
  }
  media(options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./media2.js"), true ? ["/zero/media2.js","/zero/vendor.js"] : void 0), __assign({}, options2));
  }
  image(options2) {
    return this._setComponent(() => __vitePreload(() => __import__("./media2.js"), true ? ["/zero/media2.js","/zero/vendor.js"] : void 0), __assign(__assign({}, options2), {fileExtensions: [".jpg", ".jpeg", ".png", ".webp", ".svg"]}));
  }
  video(limit) {
    return this._setComponent(() => __vitePreload(() => __import__("./video.js"), true ? ["/zero/video.js","/zero/video.css","/zero/vendor.js"] : void 0), {limit});
  }
  preview(options2) {
    this._preview = __assign(__assign({}, this.preview), options2);
    return this;
  }
}
class Editor {
  constructor(alias2, prefix2) {
    this._preview = {
      icon: null,
      template: null,
      hideLabel: false
    };
    this.templateLabel = (field) => this._prefix + field;
    this.templateDescription = (field) => this._prefix + field + "_text";
    this.tabs = [];
    this.fields = [];
    this.options = {
      disabled: false,
      display: "tabs",
      coreDatabase: true
    };
    this._alias = alias2;
    this._prefix = prefix2 || "";
  }
  get alias() {
    return this._alias;
  }
  get previewOptions() {
    return this._preview;
  }
  tab(alias2, name, count, disabled, classes, component2) {
    if (typeof disabled !== "undefined" && disabled != null && typeof disabled !== "boolean" && typeof disabled !== "function") {
      console.warn(`[zero] editor.tab: the disabled property has to be of type [boolean, function, undefined]`);
      return;
    }
    if (typeof count !== "undefined" && count != null && typeof count !== "number" && typeof count !== "function") {
      console.warn(`[zero] editor.tab: the count property has to be of type [number, function, undefined]`);
      return;
    }
    let tab = this.tabs.find((x) => x.alias === alias2);
    if (!tab) {
      tab = this._createTab(alias2, name, disabled, count, classes, component2);
      this.tabs.push(tab);
    }
    return tab;
  }
  field(path, options2) {
    options2 = options2 || {};
    if (this.tabs.length < 1) {
      this.tab("content", "@ui.tab_content", (x) => 0, (x) => false, null, null);
    }
    if (typeof options2.coreDatabase === "undefined") {
      options2.coreDatabase = this.options.coreDatabase;
    }
    if (!options2.tab) {
      options2.tab = "content";
    }
    let field = new EditorField(path, options2);
    this.fields.push(field);
    return field;
  }
  fieldset(configure) {
    let set = this._createFieldset();
    configure(set);
  }
  infoTab() {
    return this.tab("infos", "@ui.tab_infos", (x) => 0, false, "is-blank", () => __vitePreload(() => Promise.resolve().then(function() {
      return editorInfos;
    }), true ? void 0 : void 0));
  }
  disabled(condition) {
    this.options.disabled = condition;
    return this;
  }
  getFields(tab) {
    const alias2 = typeof tab === "undefined" ? null : typeof tab === "string" ? tab : tab.alias;
    return this.fields.filter((x) => !alias2 || x.options.allTabs ? true : x.options.tab === alias2);
  }
  getFieldsets(tab) {
    let fields = this.getFields(tab);
    let currentFieldset = "__undefined";
    let fieldsets = [];
    fields.forEach((field) => {
      if (field.options.fieldset != currentFieldset || !field.options.fieldset) {
        currentFieldset = field.options.fieldset;
        fieldsets.push({
          fields: [],
          cols: []
        });
      }
      fieldsets[fieldsets.length - 1].fields.push(field);
      fieldsets[fieldsets.length - 1].cols.push(field.options.fieldsetColumns);
    });
    fieldsets.forEach((fieldset) => {
      fieldset.count = fieldset.fields.length;
      let reserved = fieldset.cols.reduce((acc, x) => acc + (x || 0), 0);
      let rest = reserved < 1 ? 12 : (12 - reserved) % 12;
      let columnsToFill = fieldset.cols.filter((x) => !x).length;
      let perColumn = Math.floor(rest / columnsToFill);
      fieldset.fields.forEach((field) => {
        if (!field.options.fieldsetColumns) {
          field.options.fieldsetColumns = perColumn;
        }
      });
    });
    return fieldsets;
  }
  removeField(path) {
    const field = this.fields.find((x) => x.path === path);
    if (field != null) {
      const index = this.fields.indexOf(field);
      this.fields.splice(index, 1);
    }
  }
  removeTab(tab) {
    const alias2 = typeof tab === "object" ? tab.alias : tab;
    const foundTab = this.tabs.find((x) => x.alias === alias2);
    if (foundTab != null) {
      const index = this.tabs.indexOf(foundTab);
      this.tabs.splice(index, 1);
      this.fields.filter((x) => x.tab === alias2).forEach((field) => this.removeField(field.path));
    }
  }
  setBase(editor2) {
    this.fields = editor2.fields.map((x) => new EditorField(x.path).setBase(x));
    this.tabs = editor2.tabs.map((x) => this._createTab(x.alias, x.name, x.disabled, x.count, x.component));
    this._preview = __assign({}, editor2.previewOptions);
    return this;
  }
  preview(template2, options2) {
    this._preview = __assign(__assign(__assign({}, this._preview), {template: template2}), options2);
    return this;
  }
  _createTab(alias2, name, disabled, count, classes, component2) {
    return {
      alias: alias2,
      name,
      disabled,
      count,
      class: classes,
      component: component2,
      field: (path, options2) => {
        options2 = options2 || {};
        options2.tab = alias2;
        return this.field(path, options2);
      },
      fieldset: (configure) => {
        let set = this._createFieldset(alias2);
        configure(set);
      },
      removeField: (path) => this.removeField(path)
    };
  }
  _createFieldset(tab) {
    let id = Strings.guid();
    return {
      id,
      field: (path, options2) => {
        options2 = options2 || {};
        options2.fieldset = id;
        options2.tab = tab;
        return this.field(path, options2);
      },
      removeField: (path) => this.removeField(path)
    };
  }
}
const getConfig$1 = (config2) => {
  config2 = config2 || {};
  if (config2.scope) {
    config2.params = config2.params || {};
    config2.params.scope = config2.scope;
  }
  return config2;
};
async function get$1(url, config2 = null) {
  return await send$1(__assign({method: "get", url}, config2));
}
async function post$1(url, data = null, config2 = null) {
  return await send$1(__assign({method: "post", url, data}, config2));
}
async function del$1(url, config2 = null) {
  return await send$1(__assign({method: "delete", url}, config2));
}
async function send$1(config2) {
  config2 = getConfig$1(config2);
  try {
    const result = await axios(config2);
    return result.data;
  } catch (err) {
  }
}
function collection$1(base2) {
  return {
    getById: async (id, config2) => await get$1(base2 + "getById", __assign(__assign({}, config2), {params: {id}})),
    getByIds: async (ids, config2) => await get$1(base2 + "getByIds", __assign(__assign({}, config2), {params: {ids}})),
    getEmpty: async (config2) => await get$1(base2 + "getEmpty", __assign({}, config2)),
    getByQuery: async (query, config2) => await get$1(base2 + "getByQuery", __assign(__assign({}, config2), {params: {query}})),
    getAll: async (config2) => await get$1(base2 + "getAll", __assign({}, config2)),
    getPreviews: async (ids, config2) => await get$1(base2 + "getPreviews", __assign(__assign({}, config2), {params: {ids}})),
    getForPicker: async (config2) => await get$1(base2 + "getForPicker", __assign({}, config2)),
    save: async (model, config2) => await post$1(base2 + "save", model, __assign({}, config2)),
    delete: async (id, config2) => await del$1(base2 + "delete", __assign(__assign({}, config2), {params: {id}}))
  };
}
function download(response) {
  let filename = response.headers["zero-filename"] || "download.bin";
  var blob = response.data;
  if (typeof window.navigator.msSaveBlob !== "undefined") {
    window.navigator.msSaveBlob(blob, filename);
  } else {
    var blobURL = window.URL && window.URL.createObjectURL ? window.URL.createObjectURL(blob) : window.webkitURL.createObjectURL(blob);
    var tempLink = document.createElement("a");
    tempLink.style.display = "none";
    tempLink.href = blobURL;
    tempLink.setAttribute("download", filename);
    if (typeof tempLink.download === "undefined") {
      tempLink.setAttribute("target", "_blank");
    }
    document.body.appendChild(tempLink);
    tempLink.click();
    setTimeout(function() {
      document.body.removeChild(tempLink);
      window.URL.revokeObjectURL(blobURL);
    }, 200);
  }
}
var ApplicationsApi = __assign(__assign({}, collection$1("applications/")), {
  getAllFeatures: async () => await get$1("applications/getAllFeatures")
});
var render$2p = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "application-features"}, _vm._l(_vm.features, function(feature) {
    return _c("ui-property", {key: feature.alias, attrs: {label: feature.name, description: feature.description, vertical: false}}, [_c("ui-toggle", {attrs: {value: _vm.value.indexOf(feature.alias) > -1, disabled: _vm.disabled}, on: {input: function($event) {
      return _vm.onFeatureToggle($event, feature);
    }}})], 1);
  }), 1);
};
var staticRenderFns$2p = [];
function normalizeComponent(scriptExports, render2, staticRenderFns2, functionalTemplate, injectStyles2, scopeId, moduleIdentifier, shadowMode) {
  var options2 = typeof scriptExports === "function" ? scriptExports.options : scriptExports;
  if (render2) {
    options2.render = render2;
    options2.staticRenderFns = staticRenderFns2;
    options2._compiled = true;
  }
  if (functionalTemplate) {
    options2.functional = true;
  }
  if (scopeId) {
    options2._scopeId = "data-v-" + scopeId;
  }
  var hook;
  if (moduleIdentifier) {
    hook = function(context) {
      context = context || this.$vnode && this.$vnode.ssrContext || this.parent && this.parent.$vnode && this.parent.$vnode.ssrContext;
      if (!context && typeof __VUE_SSR_CONTEXT__ !== "undefined") {
        context = __VUE_SSR_CONTEXT__;
      }
      if (injectStyles2) {
        injectStyles2.call(this, context);
      }
      if (context && context._registeredComponents) {
        context._registeredComponents.add(moduleIdentifier);
      }
    };
    options2._ssrRegister = hook;
  } else if (injectStyles2) {
    hook = shadowMode ? function() {
      injectStyles2.call(this, (options2.functional ? this.parent : this).$root.$options.shadowRoot);
    } : injectStyles2;
  }
  if (hook) {
    if (options2.functional) {
      options2._injectStyles = hook;
      var originalRender = options2.render;
      options2.render = function renderWithStyleInjection(h, context) {
        hook.call(context);
        return originalRender(h, context);
      };
    } else {
      var existing = options2.beforeCreate;
      options2.beforeCreate = existing ? [].concat(existing, hook) : [hook];
    }
  }
  return {
    exports: scriptExports,
    options: options2
  };
}
const script$2p = {
  props: {
    value: {
      type: Array,
      default: () => []
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    features: []
  }),
  created() {
    ApplicationsApi.getAllFeatures().then((items) => this.features = items);
  },
  methods: {
    onFeatureToggle(isOn, feature) {
      const alias2 = feature.alias;
      const index = this.value.indexOf(alias2);
      if (!isOn && index > -1) {
        this.value.splice(index, 1);
      } else if (isOn && index === -1) {
        this.value.push(alias2);
      }
      this.$emit("input", this.value);
    }
  }
};
const __cssModules$2p = {};
var component$2p = normalizeComponent(script$2p, render$2p, staticRenderFns$2p, false, injectStyles$2p, null, null, null);
function injectStyles$2p(context) {
  for (let o in __cssModules$2p) {
    this[o] = __cssModules$2p[o];
  }
}
component$2p.options.__file = "app/pages/settings/application-features.vue";
var ApplicationFeatures = component$2p.exports;
const editor$10 = new Editor("application", "@application.fields.");
editor$10.options.coreDatabase = true;
const general$c = editor$10.tab("general", "@ui.tab_general");
const domains = editor$10.tab("domains", "@application.tab_domains", (x) => x.domains.length);
const features$1 = editor$10.tab("features", "@application.tab_features", (x) => x.features.length);
general$c.field("name", {label: "@ui.name"}).text(50).required();
general$c.field("fullName").text(120);
general$c.field("email").text(120).required();
general$c.field("imageId").image();
general$c.field("iconId").image();
domains.field("domains", {helpText: "@application.fields.domains_help"}).inputList(10, null, "@application.fields.domains_add").required();
features$1.field("features", {hideLabel: true}).custom(ApplicationFeatures);
const editor$$ = new Editor("country", "@country.fields.");
editor$$.field("name", {label: "@ui.name"}).text(120).required();
editor$$.field("code").text(2).required();
editor$$.field("isPreferred").toggle();
const editor$_ = new Editor("language", "@language.fields.");
editor$_.field("name", {label: "@ui.name"}).text(60).required();
editor$_.field("code").text(10).required();
editor$_.field("inheritedLanguageId").languagePicker();
editor$_.field("isDefault").toggle();
editor$_.field("isOptional").toggle();
const base$h = "media/";
const upload = async (file, folderId, onProgress, isTemporary) => {
  var data = new FormData();
  data.append("file", file);
  data.append("folderId", folderId);
  return await post$1(base$h + (isTemporary ? "uploadTemporary" : "upload"), data, {
    onUploadProgress: (progressEvent) => {
      if (typeof onProgress === "function") {
        var percentCompleted = Math.round(progressEvent.loaded * 100 / progressEvent.total);
        onProgress(percentCompleted);
      }
    }
  });
};
const getImageSource = (id, thumb, shared) => {
  if (!id || id.indexOf("http") === 0) {
    return id;
  }
  if (id.indexOf("url://") === 0) {
    return id.substring(6) + "?preset=productListing";
  }
  if (Array.isArray(id)) {
    id = id[0];
  }
  return zero.apiPath + base$h + "streamThumbnail/?id=" + id + (typeof thumb === "boolean" ? "&thumb=" + (thumb ? "true" : "false") : "") + (shared === true ? "&scope=shared" : "");
};
var MediaApi = __assign(__assign({}, collection$1(base$h)), {
  getListByQuery: async (query) => await get$1(base$h + "getListByQuery", {params: query}),
  move: async (id, destinationId) => await post$1(base$h + "move", {id, destinationId}),
  upload,
  getImageSource
});
var render$2o = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "media-upload"}, [_vm.entity.source ? _c("div", {staticClass: "media-upload-preview", attrs: {"data-type": _vm.entity.type}}, [_vm.entity.type === "image" ? _c("span", {ref: "image", staticClass: "media-upload-preview-image media-pattern", on: {click: _vm.setFocalPoint, dblclick: function($event) {
    _vm.entity.focalPoint = null;
  }}}, [_c("span", {staticClass: "media-upload-preview-focal-point", style: _vm.getFocalPointStyle(_vm.entity.focalPoint)}), _c("img", {attrs: {src: _vm.entity.previewSource, alt: _vm.entity.name}})]) : _vm._e(), _vm.entity.type === "file" ? _c("a", {staticClass: "media-upload-preview-file", attrs: {href: _vm.entity.source, target: "_blank"}}, [_c("i", {class: _vm.icons[_vm.entity.type], attrs: {"data-extension": _vm.entity.source.split(".").pop()}}), _c("div", [_c("span", [_vm._v(_vm._s(_vm.entity.source.split("/").pop()))]), _c("br"), _c("span", {staticClass: "is-minor"}, [_vm._v(_vm._s(_vm.entity.source.split(".").pop()))])])]) : _vm._e()]) : _vm._e(), _vm.entity.source ? _c("div", [_vm.entity.type === "image" ? _c("a", {staticClass: "ui-link media-upload-preview-remove", attrs: {href: _vm.entity.source, target: "_blank"}}, [_vm._v("Open")]) : _vm._e(), !_vm.disabled ? _c("button", {staticClass: "ui-link media-upload-preview-remove", attrs: {type: "button"}, on: {click: _vm.removeFile}}, [_vm._v("Remove file")]) : _vm._e()]) : _vm._e(), !_vm.entity.source && !_vm.disabled ? _c("div", {staticClass: "ui-select-button type-light"}, [_vm._m(0), _vm._m(1), _c("input", {staticClass: "media-upload-input", attrs: {type: "file"}, on: {change: _vm.onUpload}})]) : _vm._e()]);
};
var staticRenderFns$2o = [function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("span", {staticClass: "ui-select-button-icon"}, [_c("i", {staticClass: "fth-plus"})]);
}, function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("content", {staticClass: "ui-select-button-content"}, [_c("strong", {staticClass: "ui-select-button-label"}, [_vm._v("Upload file")])]);
}];
var upload_vue_vue_type_style_index_0_lang = ".media-upload-preview {\n  display: block;\n}\n.media-upload-preview-image {\n  padding: 0;\n  border-radius: var(--radius);\n  display: inline-block;\n  position: relative;\n  cursor: pointer;\n  overflow: visible;\n  user-select: none;\n  /*&:hover img\n  {\n    opacity: 0.7;\n  }*/\n}\n.media-upload-preview-image img {\n  display: block;\n  max-width: 100%;\n  max-height: 400px;\n  border-radius: var(--radius);\n  position: relative;\n  z-index: 1;\n}\n.media-upload-preview-file {\n  display: flex;\n  background: var(--color-box-nested);\n  border-radius: var(--radius);\n  align-items: center;\n  justify-content: flex-start;\n  color: var(--color-text);\n  padding: var(--padding-s);\n  padding-right: var(--padding);\n}\n.media-upload-preview-file i {\n  font-size: 22px;\n  position: relative;\n  margin-right: var(--padding-s);\n}\n.media-upload-preview-file .is-minor {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-xs);\n  text-transform: uppercase;\n}\n.media-upload-preview-focal-point {\n  display: none;\n  position: absolute;\n  left: 50%;\n  top: 50%;\n  margin: -8px 0 0 -8px;\n  width: 16px;\n  height: 16px;\n  border-radius: 20px;\n  background: white;\n  border: 3px solid #222;\n  box-shadow: 1px 1px 2px rgba(0, 0, 0, 0.4);\n  z-index: 2;\n}\n.media-upload-preview-remove {\n  margin-right: 8px;\n  margin-top: 10px;\n}\ninput[type=file].media-upload-input {\n  position: absolute;\n  height: 100%;\n  top: 0;\n  left: 0;\n  width: 100%;\n  z-index: 1;\n  bottom: 0;\n  opacity: 0.001;\n  cursor: pointer;\n}";
const script$2o = {
  props: {
    config: Object,
    entity: Object,
    value: String,
    disabled: Boolean
  },
  data: () => ({
    icons: {
      image: "fth-image",
      video: "fth-video",
      file: "fth-file"
    }
  }),
  methods: {
    removeFile() {
      this.$emit("input", (x) => x.source = null);
    },
    onUpload(event) {
      let file = event.target.files[0];
      if (!file) {
        return;
      }
      MediaApi.upload(file, this.entity.folderId, null, true).then((res) => {
        this.$emit("input", (x) => {
          x.source = res.source;
          x.previewSource = res.previewSource;
          x.thumbnailSource = res.thumbnailSource;
          x.imageMeta = res.imageMeta;
          x.type = res.type;
          x.size = res.size;
          x.focalPoint = res.focalPoint;
          x.name = res.name;
        });
      });
    },
    getFocalPointStyle(point) {
      return {
        display: "inline-block",
        left: (point ? point.left : 0.5) * 100 + "%",
        top: (point ? point.top : 0.5) * 100 + "%"
      };
    },
    setFocalPoint(ev) {
      if (this.disabled) {
        return;
      }
      let image2 = this.$refs.image.getBoundingClientRect();
      let point = {x: ev.pageX - image2.x, y: ev.pageY - image2.y};
      const left = +(point.x / image2.width).toFixed(2);
      const top = +(point.y / image2.height).toFixed(2);
      this.entity.focalPoint = {
        left: left < 0 ? 0 : left > 1 ? 1 : left,
        top: top < 0 ? 0 : top > 1 ? 1 : top
      };
    }
  }
};
const __cssModules$2o = {};
var component$2o = normalizeComponent(script$2o, render$2o, staticRenderFns$2o, false, injectStyles$2o, null, null, null);
function injectStyles$2o(context) {
  for (let o in __cssModules$2o) {
    this[o] = __cssModules$2o[o];
  }
}
component$2o.options.__file = "app/pages/media/upload.vue";
var MediaUpload = component$2o.exports;
const editor$Z = new Editor("media", "@media.fields.");
editor$Z.field("source").custom(MediaUpload).required();
editor$Z.field("name", {label: "@ui.name"}).text().required();
editor$Z.field("alternativeText").text();
editor$Z.field("caption").textarea();
var UserRolesApi = __assign(__assign({}, collection$1("userRoles/")), {
  getAllPermissions: async () => await get$1("userRoles/getAllPermissions")
});
var render$2n = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", [_vm.permissions.length ? _c("ui-inline-tabs", {staticClass: "ui-permissions", attrs: {"force-count": true}}, _vm._l(_vm.permissions, function(permissionCollection, index) {
    return _c("ui-tab", {key: index, attrs: {label: permissionCollection.label, title: permissionCollection.description, count: _vm.getCount(permissionCollection)}}, [_c("ui-error", {attrs: {field: "Claims"}}), _vm._l(permissionCollection.items, function(permission, index2) {
      return _c("ui-property", {key: index2, staticClass: "role-permission-toggle", attrs: {label: permission.label, description: permission.description, vertical: false}}, [permission.valueType === "crud" ? _c("div", {staticClass: "ui-permissions-crud"}, [_c("ui-toggle", {attrs: {value: permission.value != "none", disabled: _vm.disabled}, on: {input: function($event) {
        return _vm.onPermissionToggle($event, permission);
      }}}), permission.value != "none" ? _c("ui-check-list", {attrs: {value: permission.value.split(","), items: _vm.stateItems, inline: true, disabled: _vm.disabled}, on: {input: function($event) {
        return _vm.onPermissionCRUDChecked($event, permission);
      }}}) : _vm._e()], 1) : _vm._e(), permission.valueType === "boolean" ? _c("ui-toggle", {attrs: {disabled: _vm.disabled}, on: {input: _vm.onChange}, model: {value: permission.value, callback: function($$v) {
        _vm.$set(permission, "value", $$v);
      }, expression: "permission.value"}}) : _vm._e()], 1);
    })], 2);
  }), 1) : _vm._e()], 1);
};
var staticRenderFns$2n = [];
var permissions_vue_vue_type_style_index_0_lang = ".ui-permissions > .ui-property + .ui-property {\n  border-top: 1px solid var(--color-line);\n  padding-top: 40px;\n  margin-top: 40px;\n}\n.ui-permissions > .ui-property > .ui-property-label {\n  width: 300px;\n}\n.ui-permissions .ui-tab {\n  border-top: 1px solid var(--color-line);\n  padding-top: var(--padding);\n}\n.role-permission-toggle {\n  border-top-width: 0 !important;\n}\n.ui-permissions-crud {\n  display: flex;\n}\n.ui-permissions-crud .ui-toggle {\n  margin-right: var(--padding);\n}\n.ui-permissions-crud .ui-check-list {\n  margin-top: 1px;\n}";
const script$2n = {
  name: "uiPermissions",
  props: {
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  watch: {
    value(value) {
      this.rebuild();
    }
  },
  data: () => ({
    claims: [],
    stateItems: [
      {value: "@permission.states.read", key: "read"},
      {value: "@permission.states.update", key: "update"},
      {value: "@permission.states.create", key: "create"}
    ],
    permissions: []
  }),
  created() {
    UserRolesApi.getAllPermissions().then((response) => {
      this.permissions = response;
      this.rebuild();
    });
  },
  methods: {
    rebuild() {
      if (!this.permissions.length) {
        return;
      }
      let claims = {};
      this.value.forEach((claim) => {
        const parts2 = claim.value.split(":");
        claims[parts2[0]] = parts2[1];
      });
      this.permissions.forEach((collection2) => {
        collection2.items.forEach((permission) => {
          permission.value = this.parsePermissionValue(claims[permission.key], permission.valueType);
        });
      });
    },
    getCount(permissionGroup) {
      return filter$1(permissionGroup.items, (claim) => {
        return claim.value !== "none" && claim.value !== "false" && !!claim.value;
      }).length;
    },
    onChange() {
      let claims = [];
      this.permissions.forEach((collection2) => {
        collection2.items.forEach((permission) => {
          if (permission.valueType === "boolean" || permission.valueType === "crud" || permission.valueType === "string") {
            claims.push({
              type: "zero.claim.permission",
              value: permission.key + ":" + permission.value
            });
          }
        });
      });
      this.$emit("input", claims);
    },
    onPermissionToggle(isOn, permission) {
      if (!isOn) {
        permission._oldValue = permission.value;
        permission.value = "none";
      } else {
        permission.value = permission._oldValue || "read,create,update,delete";
      }
      this.onChange();
    },
    onPermissionCRUDChecked(val, permission) {
      if (val.indexOf("read") < 0) {
        val.push("read");
      }
      permission.value = val.join(",").trim(",");
      this.onChange();
    },
    parsePermissionValue(value, type) {
      if (type === "boolean") {
        return value === "true";
      } else if (type === "crud" && !value) {
        return "none";
      }
      return value;
    }
  }
};
const __cssModules$2n = {};
var component$2n = normalizeComponent(script$2n, render$2n, staticRenderFns$2n, false, injectStyles$2n, null, null, null);
function injectStyles$2n(context) {
  for (let o in __cssModules$2n) {
    this[o] = __cssModules$2n[o];
  }
}
component$2n.options.__file = "app/components/permissions.vue";
var uiPermissions = component$2n.exports;
const editor$Y = new Editor("user", "@user.fields.");
editor$Y.options.coreDatabase = true;
const permissionsCount$1 = (x) => (x.claims || []).filter((claim) => {
  const value = claim.value.split(":")[1];
  return value !== "none" && value !== "false" && !!value;
}).length;
const general$b = editor$Y.tab("general", "@ui.tab_general");
const permissions$1 = editor$Y.tab("permissions", "@user.tab_permissions", permissionsCount$1);
general$b.field("name", {label: "@ui.name"}).text(80).required();
general$b.field("email").text(120).required();
general$b.field("languageId").culturePicker().required();
general$b.field("avatarId").image();
permissions$1.field("claims", {hideLabel: true}).custom(uiPermissions);
const editor$X = new Editor("userRole", "@role.fields.");
editor$X.options.coreDatabase = true;
const permissionsCount = (x) => (x.claims || []).filter((claim) => {
  const value = claim.value.split(":")[1];
  return value !== "none" && value !== "false" && !!value;
}).length;
const general$a = editor$X.tab("general", "@ui.tab_general");
const permissions = editor$X.tab("permissions", "@role.tab_permissions", permissionsCount);
general$a.field("name", {label: "@ui.name"}).text(60).required();
general$a.field("description").text(120);
general$a.field("icon").iconPicker();
general$a.field("avatarId").image();
permissions.field("claims", {hideLabel: true}).custom(uiPermissions);
const editor$W = new Editor("mailTemplate", "@mailTemplate.fields.");
const general$9 = editor$W.tab("general", "@ui.tab_general");
const sender = editor$W.tab("sender", "@mailTemplate.tabs.sender");
const recipient = editor$W.tab("recipient", "@mailTemplate.tabs.recipient");
general$9.field("name", {label: "@ui.name"}).text(60).required();
general$9.field("key").text(60).required();
general$9.field("subject").text(80).required();
general$9.field("body").rte();
general$9.field("preheader").text(160);
sender.field("senderEmail").text();
sender.field("senderName").text();
recipient.field("recipientEmail").text();
recipient.field("cc").text();
recipient.field("bcc").text();
const editor$V = new Editor("pages." + __zero.alias.pages.folder, "@page.folder.fields.");
editor$V.field("isPartOfRoute").toggle();
editor$V.field("urlAlias").text(40).when((x) => x.isPartOfRoute);
var editors$4 = {
  application: editor$10,
  country: editor$$,
  language: editor$_,
  media: editor$Z,
  user: editor$Y,
  userRole: editor$X,
  mailTemplate: editor$W,
  pageFolder: editor$V
};
var Localization = {
  localize(key, options2) {
    let params = extend({
      force: false,
      tokens: {},
      hideEmpty: false
    }, options2 || {});
    if (!key) {
      return "";
    }
    const hasAtSign = key.indexOf("@") === 0;
    if (!params.force && !hasAtSign) {
      return this.replaceTokens(key, params.tokens);
    }
    key = hasAtSign ? key.slice(1) : key;
    const value = __zero.translations[key.toLowerCase()];
    if (!params.hideEmpty && (!value || typeof value !== "string")) {
      return "[" + key + "]";
    }
    return this.replaceTokens(value, params.tokens);
  },
  replaceTokens(value, tokens) {
    if (!value || value.indexOf("{") < 0) {
      return value;
    }
    each(tokens, (replacement, key) => {
      value = value.replace("{" + key + "}", replacement);
    });
    return value;
  }
};
class ListColumn {
  constructor(path, options2) {
    this.path = null;
    this.options = {
      label: null,
      hideLabel: false,
      width: null,
      canSort: true,
      class: ""
    };
    this._type = null;
    this._func = () => {
    };
    this._funcOptions = {};
    this._asHtml = false;
    this.path = path;
    this.options = __assign(__assign({}, this.options), options2);
  }
  get isHtml() {
    return this._asHtml;
  }
  get type() {
    return this._type;
  }
  get func() {
    return this._func;
  }
  get funcOptions() {
    return this._funcOptions;
  }
  setBase(column) {
    this.path = column.path;
    this.options = __assign({}, column.options);
    this._asHtml = column.isHtml;
    this._type = column.type;
    this._func = column.func;
    this._funcOptions = column.funcOptions;
    return this;
  }
  render(value, model) {
    return this._func(value, this._funcOptions, model);
  }
  custom(renderFunc, asHtml, type) {
    this._type = type || "custom";
    this._asHtml = asHtml || false;
    this._func = (value, opts, model) => {
      return renderFunc(value, model, opts);
    };
    return this;
  }
  text(options2) {
    this._type = "text";
    this._funcOptions = __assign({localize: false, tokens: {}, wrap: false}, options2);
    this._func = (value, opts) => {
      let result = value;
      if (opts.localize) {
        result = Localization.localize(value, opts.tokens || {});
      }
      return result;
    };
    return this;
  }
  html(options2) {
    this._type = "html";
    this._asHtml = true;
    return this.text(options2);
  }
  date(options2) {
    this._type = "date";
    this._funcOptions = __assign({format: "short"}, options2);
    this._func = (value, opts) => Strings.date(value, opts.format);
    return this;
  }
  currency() {
    this._type = "currency";
    this._asHtml = true;
    this._func = (value, opts) => {
      let price = isNaN(value) ? 0 : value;
      let hasDecimals = ~~price !== price;
      price = hasDecimals ? (price / 1).toFixed(2) : ~~price;
      return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "&nbsp;") + "&nbsp;&euro;";
    };
    return this;
  }
  boolean(isTrueFunc) {
    this._type = "boolean";
    this._asHtml = true;
    this._funcOptions = {isTrueFunc};
    this._func = (value, opts) => {
      let isTrue = typeof opts.isTrueFunc === "function" ? !!opts.isTrueFunc(value) : !!value;
      return `<svg class="ui-icon ui-table-field-bool" width="16" height="16" stroke-width="${isTrue ? "2.5" : "2"}" data-symbol="${isTrue ? "fth-check" : "fth-x"}">
        <use xlink:href="#${isTrue ? "fth-check" : "fth-x"}" />
      </svg>`;
    };
    return this;
  }
  image() {
    this._type = "image";
    this._asHtml = true;
    this._func = (value, opts) => value ? `<img src="${MediaApi.getImageSource(value)}" class="ui-table-field-image">` : "";
    return this;
  }
  icon(icon, size) {
    size = size || 17;
    this._type = "icon";
    this._asHtml = true;
    this._func = (value, opts) => {
      let ico = (icon || value).trim();
      let html = `<svg class="ui-icon ui-table-field-image" width="${size}" height="${size}" stroke-width="2" :data-symbol="${ico}">`;
      if (ico.indexOf("flag") !== 0) {
        html += `<use xlink:href="#${ico}" />`;
      }
      return html + `</svg>`;
    };
    return this;
  }
  name() {
    this.options.label = "@ui.name";
    this.options.class = "is-name";
    this._type = "text";
    this._asHtml = true;
    this._func = (value, opts, model) => {
      let html = "<b>" + value + "</b>";
      if (model.isActive === false) {
        html = value;
      }
      if (model.blueprint && model.blueprint.id) {
        html += ` <svg class="ui-icon" width="15" height="15" stroke-width="2" :data-symbol="fth-cloud" title="Synchronized"><use xlink:href="#fth-cloud" /></svg>`;
      }
      return html;
    };
    return this;
  }
  active() {
    this.options.label = "@ui.active";
    this.options.width = 150;
    return this.boolean();
  }
  created() {
    this.options.label = "@ui.createdDate";
    this.options.width = 150;
    return this.date();
  }
}
class ListAction {
  constructor(key, label, icon, action, autoclose = true) {
    this.autoclose = true;
    this.action = () => {
      console.warn(`[zero] A list action needs a "action" callback`);
    };
    this.key = key;
    this.label = label;
    this.icon = icon;
    this.action = action;
    this.autoclose = autoclose;
  }
  call(options2) {
    this.action(options2);
  }
}
class List {
  constructor(alias2) {
    this.query = {
      orderBy: "createdDate",
      isDescending: true,
      page: 1,
      pageSize: 25,
      search: null,
      filter: null
    };
    this.templateLabel = (field) => field;
    this.link = null;
    this.onClick = null;
    this.columns = [];
    this.actions = [];
    this.paramsToQuery = (params) => {
      let values2 = {};
      if (params.page !== this.query.page) {
        values2.page = params.page;
      }
      if (params.isDescending !== this.query.isDescending || params.orderBy !== this.query.orderBy) {
        values2.by = params.orderBy || this.query.orderBy;
        values2.desc = params.isDescending || this.query.isDescending;
      }
      if (!!params.search) {
        values2.search = params.search;
      }
      if (params.filter && params.filter.id) {
        values2.filter = params.filter.id;
      }
      return values2;
    };
    this.queryToParams = (query) => {
      if (!query) {
        return {};
      }
      let values2 = JSON.parse(JSON.stringify(this.query));
      if (query.page) {
        values2.page = +query.page || this.query.page;
      }
      if (query.by) {
        values2.orderBy = query.by;
      }
      if (query.desc) {
        values2.isDescending = query.desc === "true" || query.desc === true;
      }
      if (query.search) {
        values2.search = query.search;
      }
      if (query.filter) {
        values2.filter = values2.filter || {};
        values2.filter.id = query.filter;
      }
      return values2;
    };
    this._alias = alias2;
  }
  setBase(list2) {
    this.columns = list2.columns.map((x) => new ListColumn(x.path).setBase(x));
    this.actions = list2.actions.map((x) => new ListAction(x.key, x.label, x.icon, x.action, x.autoclose));
    this.query = __assign({}, list2.query);
    this.templateLabel = list2.templateLabel;
    this.link = list2.link;
    this.onClick = list2.onClick;
    this.actions = [...list2.actions];
    return this;
  }
  get alias() {
    return this._alias;
  }
  get filterOptions() {
    return this._filterOptions;
  }
  column(path, options2) {
    let column = this.columns.find((x) => x.path === path);
    if (!column) {
      column = new ListColumn(path, options2);
      if (options2 && options2.index > -1) {
        this.columns.splice(options2.index, 0, column);
      } else {
        this.columns.push(column);
      }
    } else {
      column.options = __assign(__assign({}, column.options), options2 || {});
    }
    return column;
  }
  removeColumn(path) {
    let column = this.columns.find((x) => x.path === path);
    if (column != null) {
      this.columns.splice(this.columns.indexOf(column), 1);
    }
  }
  action(key, label, icon, callback, autoclose) {
    const action = new ListAction(key, label, icon, callback);
    action.autoclose = typeof autoclose === "undefined" ? true : autoclose;
    this.actions.push(action);
    return action;
  }
  export(callback) {
    return this.action("export", "@ui.export.action", "fth-share", (opts) => {
      opts.loading(true);
      callback().then((_) => {
        opts.loading(false);
        opts.hide();
      });
    }, false);
  }
  onFetch(callback) {
    this._fetch = callback;
  }
  fetch(filter2) {
    return this._fetch(filter2);
  }
  useFilter(editor2, template2) {
    this._filterOptions = {editor: editor2, template: template2};
  }
}
const arrayMoveMutate = (array, from, to) => {
  const startIndex = to < 0 ? array.length + to : to;
  const item2 = array.splice(from, 1)[0];
  array.splice(startIndex, 0, item2);
};
function move(array, from, to) {
  array = array.slice();
  arrayMoveMutate(array, from, to);
  return array;
}
function replace(array, origin, target) {
  const index = array.indexOf(origin);
  if (index < 0) {
    return index;
  }
  array.splice(index, 1);
  array.splice(index, 0, target);
  return index;
}
function remove(array, value) {
  const index = array.indexOf(value);
  if (index < 0) {
    return;
  }
  array.splice(index, 1);
  return index;
}
var Arrays = {
  move,
  replace,
  remove
};
var countriesApi = __assign({}, collection$1("countries/"));
const list$u = new List("countries");
const prefix$6 = "@country.fields.";
list$u.templateLabel = (x) => prefix$6 + x;
list$u.link = zero.alias.settings.countries + "-edit";
list$u.onFetch((filter2) => countriesApi.getByQuery(filter2));
list$u.column("flag", {width: 62, canSort: false, hideLabel: true}).custom((value, model) => `<i class="ui-icon" data-symbol="flag-${model.code.toLowerCase()}"></i>`, true, "flag");
list$u.column("name").name();
list$u.column("code").custom((val) => val.toUpperCase());
list$u.column("isPreferred").boolean();
list$u.column("isActive").active();
var LanguagesApi = __assign(__assign({}, collection$1("languages/")), {
  getAllCultures: async () => await get$1("languages/getAllCultures"),
  getSupportedCultures: async () => await get$1("languages/getSupportedCultures")
});
const list$t = new List("languages");
const prefix$5 = "@language.fields.";
list$t.templateLabel = (x) => prefix$5 + x;
list$t.link = zero.alias.settings.languages + "-edit";
list$t.onFetch((filter2) => LanguagesApi.getByQuery(filter2));
list$t.column("name").name();
list$t.column("code").text();
list$t.column("isDefault", {width: 200}).boolean();
var TranslationsApi = __assign({}, collection$1("translations/"));
const list$s = new List("translations");
const prefix$4 = "@translation.fields.";
list$s.templateLabel = (x) => prefix$4 + x;
list$s.link = zero.alias.settings.translations + "-edit";
list$s.onFetch((filter2) => TranslationsApi.getByQuery(filter2));
list$s.column("key").name();
list$s.column("value").text();
const base$g = "users/";
var UsersApi = __assign(__assign({}, collection$1(base$g)), {
  updatePassword: async (model) => await post$1(base$g + "updatePassword", model),
  disable: async (model) => await post$1(base$g + "disable", model),
  enable: async (model) => await post$1(base$g + "enable", model)
});
const list$r = new List("users");
const prefix$3 = "@user.fields.";
list$r.templateLabel = (x) => prefix$3 + x;
list$r.link = zero.alias.settings.users + "-edit";
list$r.onFetch((filter2) => UsersApi.getAll(filter2));
list$r.column("avatarId", {width: 70, canSort: false, hideLabel: true}).image();
list$r.column("name").name();
list$r.column("email").text();
list$r.column("isActive").active();
var MailTemplatesApi = __assign({}, collection$1("mailTemplates/"));
const list$q = new List("mailTemplates");
list$q.templateLabel = (x) => "@mailTemplate.fields." + x;
list$q.link = zero.alias.settings.mails + "-edit";
list$q.onFetch((filter2) => MailTemplatesApi.getByQuery(filter2));
list$q.column("name").name();
list$q.column("subject").text();
list$q.column("key").text();
var lists$4 = {
  countries: list$u,
  languages: list$t,
  translations: list$s,
  users: list$r,
  mails: list$q
};
var render$2m = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "dashboard"});
};
var staticRenderFns$2m = [];
var dashboard_vue_vue_type_style_index_0_lang = ".dashboard {\n  position: relative;\n  padding-top: 0;\n}\n.dashboard-elements {\n  display: grid;\n  padding: 20px;\n  padding-left: 0;\n  gap: 16px;\n  grid-template-columns: repeat(auto-fill, minmax(360px, 1fr));\n  grid-auto-rows: minmax(240px, auto);\n  grid-auto-flow: dense;\n}";
const script$2m = {};
const __cssModules$2m = {};
var component$2m = normalizeComponent(script$2m, render$2m, staticRenderFns$2m, false, injectStyles$2m, null, null, null);
function injectStyles$2m(context) {
  for (let o in __cssModules$2m) {
    this[o] = __cssModules$2m[o];
  }
}
component$2m.options.__file = "app/pages/dashboard/dashboard.vue";
var Dashboard = component$2m.exports;
const alias$7 = __zero.alias.sections.dashboard;
const section$7 = __zero.sections.find((x) => x.alias === alias$7);
var dashboardRoutes = section$7 ? [
  {
    name: section$7.alias,
    path: section$7.url,
    component: Dashboard,
    meta: {
      name: section$7.name,
      alias: section$7.alias,
      section: section$7
    }
  }
] : [];
const alias$6 = __zero.alias.sections.pages;
const section$6 = __zero.sections.find((x) => x.alias === alias$6);
var pageRoutes = section$6 ? [
  {
    name: section$6.alias,
    path: section$6.url,
    component: () => __vitePreload(() => __import__("./pages2.js"), true ? ["/zero/pages2.js","/zero/pages2.css","/zero/copy.js","/zero/copy.css","/zero/vendor.js"] : void 0),
    meta: {
      name: section$6.name,
      alias: section$6.alias,
      section: section$6
    },
    children: [
      {
        name: "page",
        path: "edit/:id",
        section: alias$6,
        props: true,
        component: () => __vitePreload(() => __import__("./page.js"), true ? ["/zero/page.js","/zero/page.css","/zero/copy.js","/zero/copy.css","/zero/vendor.js"] : void 0)
      },
      {
        name: "page-create",
        path: "create/:type/:parent?",
        section: alias$6,
        props: true,
        component: () => __vitePreload(() => __import__("./page.js"), true ? ["/zero/page.js","/zero/page.css","/zero/copy.js","/zero/copy.css","/zero/vendor.js"] : void 0)
      },
      {
        name: "recyclebin",
        path: "recyclebin",
        section: alias$6,
        component: () => __vitePreload(() => __import__("./recyclebin.js"), true ? ["/zero/recyclebin.js","/zero/recyclebin.css","/zero/vendor.js"] : void 0),
        meta: {
          name: "@recyclebin.name"
        }
      }
    ]
  }
] : [];
const alias$5 = __zero.alias.sections.media;
const section$5 = __zero.sections.find((x) => x.alias === alias$5);
var mediaRoutes = section$5 ? [
  {
    name: section$5.alias,
    path: section$5.url + "/:id?",
    component: () => __vitePreload(() => __import__("./overview.js"), true ? ["/zero/overview.js","/zero/overview.css","/zero/select-overlay.js","/zero/select-overlay.css","/zero/vendor.js"] : void 0),
    props: true,
    meta: {
      name: section$5.name,
      alias: section$5.alias,
      section: section$5
    }
  },
  {
    name: section$5.alias + "-edit",
    path: section$5.url + "/edit/:id",
    component: () => __vitePreload(() => __import__("./detail.js"), true ? ["/zero/detail.js","/zero/vendor.js"] : void 0),
    props: true,
    meta: {
      name: section$5.name,
      alias: section$5.alias,
      section: section$5
    }
  }
] : [];
const alias$4 = __zero.alias.sections.settings;
const section$4 = __zero.sections.find((x) => x.alias === alias$4);
let settings$3 = [];
let routes$5 = [];
__zero.settingsAreas.forEach((group) => group.items.forEach((area) => {
  if (!area.isPlugin) {
    settings$3.push(area);
  }
}));
const addArea$1 = (areaAlias, component2, detailComponent, hasCreate, postCreate) => {
  let area = typeof areaAlias === "object" ? areaAlias : settings$3.find((x) => x.alias === areaAlias);
  if (!area) {
    return;
  }
  routes$5.push({
    name: area.alias,
    path: area.url,
    component: component2,
    meta: {
      name: area.name
    }
  });
  if (detailComponent && hasCreate) {
    routes$5.push({
      name: area.alias + "-create",
      path: area.url + "/create/:scope?",
      props: true,
      component: detailComponent,
      meta: {
        create: true,
        name: area.name
      }
    });
  }
  if (detailComponent) {
    routes$5.push({
      name: area.alias + "-edit",
      path: area.url + "/edit/:id",
      props: true,
      component: detailComponent,
      meta: {
        name: area.name
      }
    });
  }
  if (typeof postCreate === "function") {
    postCreate(area);
  }
};
if (section$4) {
  routes$5.push({
    name: section$4.alias,
    path: section$4.url,
    component: () => __vitePreload(() => __import__("./settings.js"), true ? ["/zero/settings.js","/zero/settings.css","/zero/applications-items.js","/zero/applications-items.css","/zero/vendor.js"] : void 0),
    meta: {
      name: section$4.name,
      alias: section$4.alias,
      section: section$4
    }
  });
  addArea$1(__zero.alias.settings.applications, () => __vitePreload(() => __import__("./applications.js"), true ? ["/zero/applications.js","/zero/applications.css","/zero/applications-items.js","/zero/applications-items.css","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./application.js"), true ? ["/zero/application.js","/zero/vendor.js"] : void 0), true);
  addArea$1(__zero.alias.settings.countries, () => __vitePreload(() => __import__("./countries.js"), true ? ["/zero/countries.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./country.js"), true ? ["/zero/country.js","/zero/country.css","/zero/vendor.js"] : void 0), true);
  addArea$1(__zero.alias.settings.languages, () => __vitePreload(() => __import__("./languages.js"), true ? ["/zero/languages.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./language.js"), true ? ["/zero/language.js","/zero/vendor.js"] : void 0), true);
  addArea$1(__zero.alias.settings.translations, () => __vitePreload(() => __import__("./translations.js"), true ? ["/zero/translations.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./translations.js"), true ? ["/zero/translations.js","/zero/vendor.js"] : void 0), true);
  addArea$1(__zero.alias.settings.mails, () => __vitePreload(() => __import__("./mails.js"), true ? ["/zero/mails.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./mail.js"), true ? ["/zero/mail.js","/zero/vendor.js"] : void 0), true);
  addArea$1(__zero.alias.settings.integrations, () => __vitePreload(() => __import__("./integrations.js").then(function(n) {
    return n.i;
  }), true ? ["/zero/integrations.js","/zero/integrations.css"] : void 0));
  addArea$1(__zero.alias.settings.users, () => __vitePreload(() => __import__("./users.js"), true ? ["/zero/users.js","/zero/users.css","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./user.js"), true ? ["/zero/user.js","/zero/user.css","/zero/vendor.js"] : void 0), true, (area) => {
    routes$5.push({
      name: "roles-create",
      path: "/" + section$4.alias + "/roles/create/:scope?",
      props: true,
      component: () => __vitePreload(() => __import__("./role.js"), true ? ["/zero/role.js","/zero/role.css","/zero/vendor.js"] : void 0),
      meta: {
        create: true,
        name: area.name
      }
    });
    routes$5.push({
      name: "roles-edit",
      path: "/" + section$4.alias + "/roles/edit/:id",
      props: true,
      component: () => __vitePreload(() => __import__("./role.js"), true ? ["/zero/role.js","/zero/role.css","/zero/vendor.js"] : void 0),
      meta: {
        name: area.name
      }
    });
  });
}
const alias$3 = __zero.alias.sections.spaces;
const section$3 = __zero.sections.find((x) => x.alias === alias$3);
var spaceRoutes = section$3 ? [
  {
    name: section$3.alias,
    path: section$3.url,
    component: () => __vitePreload(() => __import__("./spaces.js"), true ? ["/zero/spaces.js","/zero/spaces.css","/zero/vendor.js"] : void 0),
    meta: {
      name: section$3.name,
      alias: section$3.alias,
      section: section$3
    },
    children: [
      {
        name: "space-item",
        path: ":alias/edit/:id",
        props: true,
        component: () => __vitePreload(() => __import__("./spaces.js"), true ? ["/zero/spaces.js","/zero/spaces.css","/zero/vendor.js"] : void 0)
      },
      {
        name: "space",
        path: ":alias",
        props: true,
        component: () => __vitePreload(() => __import__("./spaces.js"), true ? ["/zero/spaces.js","/zero/spaces.css","/zero/vendor.js"] : void 0)
      },
      {
        name: "space-create",
        path: ":alias/create/:scope?",
        props: true,
        component: () => __vitePreload(() => __import__("./spaces.js"), true ? ["/zero/spaces.js","/zero/spaces.css","/zero/vendor.js"] : void 0),
        meta: {
          create: true
        }
      }
    ]
  }
] : [];
var routes$4 = [
  {
    name: "preview",
    path: "/preview",
    component: () => __vitePreload(() => __import__("./preview.js"), true ? ["/zero/preview.js","/zero/preview.css","/zero/vendor.js"] : void 0),
    meta: {
      name: "@preview.name"
    }
  },
  ...dashboardRoutes,
  ...pageRoutes,
  ...mediaRoutes,
  ...routes$5,
  ...spaceRoutes,
  {
    name: "404",
    path: "*",
    component: () => __vitePreload(() => __import__("./notfound.js"), true ? ["/zero/notfound.js","/zero/notfound.css","/zero/vendor.js"] : void 0)
  }
];
const plugin$4 = new Plugin("zero");
plugin$4.addEditors(editors$4);
plugin$4.addLists(lists$4);
plugin$4.addRoutes(routes$4);
plugin$4.install = (vue, zero2) => {
  zero2.config.linkPicker.areas.push({
    alias: "zero.pages",
    name: "@zero.config.linkareas.pages",
    component: () => __vitePreload(() => __import__("./pages.js"), true ? ["/zero/pages.js","/zero/pages.css","/zero/vendor.js"] : void 0)
  });
  zero2.config.linkPicker.areas.push({
    alias: "zero.media",
    name: "@zero.config.linkareas.media",
    component: () => __vitePreload(() => __import__("./media.js"), true ? ["/zero/media.js","/zero/vendor.js"] : void 0)
  });
};
const base$f = "commerceCategories/";
var CategoriesApi = {
  getById(id) {
    return axios.get(base$f + "getById", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getEmpty() {
    return axios.get(base$f + "getEmpty").then((res) => Promise.resolve(res.data));
  },
  getChildren(channel2, parent, active) {
    return axios.get(base$f + "getChildren", {params: {channel: channel2, parent, active}}).then((res) => Promise.resolve(res.data));
  },
  getPreviews(ids) {
    return axios.get(base$f + "getPreviews", {params: {ids}}).then((res) => Promise.resolve(res.data));
  },
  getForCatalogue(id, channel2) {
    return axios.get(base$f + "getForCatalogue", {params: {id, channel: channel2}}).then((res) => Promise.resolve(res.data));
  },
  save(model) {
    return axios.post(base$f + "save", model).then((res) => Promise.resolve(res.data));
  },
  saveSorting(ids) {
    return axios.post(base$f + "saveSorting", ids).then((res) => Promise.resolve(res.data));
  },
  move(id, destinationId) {
    return axios.post(base$f + "move", {id, destinationId}).then((res) => Promise.resolve(res.data));
  },
  copy(id, destinationId) {
    return axios.post(base$f + "copy", {id, destinationId}).then((res) => Promise.resolve(res.data));
  },
  delete(id) {
    return axios.delete(base$f + "delete", {params: {id}}).then((res) => Promise.resolve(res.data));
  }
};
var render$2l = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "shop-categorypicker-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.config.closeLabel, parent: _vm.config.rootId}, on: {click: _vm.config.hide}})];
  }, proxy: true}])}, [_vm.opened ? _c("div", {staticClass: "ui-box shop-categorypicker-overlay-items"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems, parent: _vm.config.rootId}, on: {select: _vm.onSelect}})], 1) : _vm._e()]);
};
var staticRenderFns$2l = [];
var overlay_vue_vue_type_style_index_0_lang$9 = '@charset "UTF-8";\n.shop-categorypicker-overlay content {\n  padding-top: 0;\n}\n.ui-box.shop-categorypicker-overlay-items {\n  margin: 0;\n  padding: 20px 0;\n}\n.ui-box.shop-categorypicker-overlay-items .ui-tree-item.is-selected, .ui-box.shop-categorypicker-overlay-items .ui-tree-item:hover:not(.is-disabled) {\n  background: var(--color-bg-xxlight);\n}\n.ui-box.shop-categorypicker-overlay-items + .ui-box {\n  margin-top: var(--padding);\n}\n.ui-box.shop-categorypicker-overlay-items .ui-tree-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n}\n.ui-box.shop-categorypicker-overlay-items .ui-tree-item.is-selected .ui-tree-item-text {\n  font-weight: bold;\n}';
const script$2l = {
  props: {
    model: String,
    config: Object
  },
  data: () => ({
    opened: false
  }),
  computed: {
    disabledIds() {
      return this.config.disabledIds || [];
    }
  },
  mounted() {
    setTimeout(() => this.opened = true, 300);
  },
  methods: {
    onSelect(item2) {
      this.config.confirm(item2);
    },
    getItems(parent) {
      var channelId = this.config.channelId;
      return CategoriesApi.getChildren(channelId, parent, this.model).then((res) => {
        res.items.forEach((item2) => {
          if (item2.id === this.model) {
            item2.isSelected = true;
          }
          if (this.disabledIds.indexOf(item2.id) > -1 && item2.id !== this.model) {
            item2.disabled = true;
          }
          item2.hasActions = false;
        });
        return res.items;
      });
    }
  }
};
const __cssModules$2l = {};
var component$2l = normalizeComponent(script$2l, render$2l, staticRenderFns$2l, false, injectStyles$2l, null, null, null);
function injectStyles$2l(context) {
  for (let o in __cssModules$2l) {
    this[o] = __cssModules$2l[o];
  }
}
component$2l.options.__file = "../zero.Commerce/Plugin/pickers/category/overlay.vue";
var CategoryOverlay = component$2l.exports;
var render$2k = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-confirm"}, [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: _vm.config.title, expression: "config.title"}], staticClass: "ui-headline"}), _c("p", {directives: [{name: "localize", rawName: "v-localize:html", value: _vm.config.text, expression: "config.text", arg: "html"}]}), _c("ui-error", {ref: "error", staticStyle: {"margin-top": "25px"}}), _c("div", {staticClass: "app-confirm-buttons"}, [_c("ui-button", {attrs: {type: _vm.config.confirmType, state: _vm.state, label: _vm.config.confirmLabel}, on: {click: _vm.confirm}}), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.state == "loading"}, on: {click: _vm.config.close}})], 1)], 1);
};
var staticRenderFns$2k = [];
var confirm_vue_vue_type_style_index_0_lang = ".app-confirm-buttons {\n  margin-top: 40px;\n}\n.app-confirm p {\n  line-height: 1.4;\n}";
const script$2k = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    state: "default"
  }),
  mounted() {
  },
  methods: {
    confirm() {
      var instance = this;
      this.config.confirm({
        hide() {
          instance.config.close();
        },
        state(state) {
          instance.state = state;
        },
        errors(errors) {
          instance.state = "error";
          if (!errors) {
            instance.$refs.error.clearErrors();
          } else {
            instance.$refs.error.setErrors(errors);
          }
          setTimeout(() => instance.state = "default", 1500);
        },
        success: true
      });
    }
  }
};
const __cssModules$2k = {};
var component$2k = normalizeComponent(script$2k, render$2k, staticRenderFns$2k, false, injectStyles$2k, null, null, null);
function injectStyles$2k(context) {
  for (let o in __cssModules$2k) {
    this[o] = __cssModules$2k[o];
  }
}
component$2k.options.__file = "app/components/overlays/confirm.vue";
var AppConfirm = component$2k.exports;
var Overlay = new Vue({
  data: () => ({
    dropdownInstance: null,
    instances: []
  }),
  methods: {
    setDropdown(instance) {
      if (this.dropdownInstance != null) {
        this.dropdownInstance.hide();
      }
      this.dropdownInstance = instance;
    },
    confirmDelete(title, text2) {
      let options2 = extend({
        title: typeof title === "string" ? title : "@deleteoverlay.title",
        text: text2 || "@deleteoverlay.text",
        confirmLabel: "@deleteoverlay.confirm",
        confirmType: "danger",
        closeLabel: "@deleteoverlay.close",
        component: AppConfirm,
        autoclose: false,
        softdismiss: false
      }, typeof title === "object" ? title : {});
      return this.open(options2);
    },
    confirm(title, text2) {
      let options2 = extend({
        title,
        text: text2,
        component: AppConfirm,
        autoclose: true,
        softdismiss: false
      }, typeof title === "object" ? title : {});
      return this.open(options2);
    },
    open(options2) {
      const defaultWidth = options2.display === "editor" ? 560 : 460;
      options2 = extend({
        id: Strings.guid(),
        display: "dialog",
        width: defaultWidth,
        hide: this.close,
        autoclose: true,
        softdismiss: options2.display !== "editor",
        closeLabel: "@ui.close",
        confirmLabel: "@ui.confirm",
        confirmType: "default",
        alias: options2.alias
      }, options2);
      if (typeof options2.theme === "undefined") {
        options2.theme = "default";
      }
      this.instances.push(options2);
      return new Promise((resolve, reject) => {
        options2.close = () => {
          this.close(options2);
          reject(options2);
        };
        options2.hide = options2.close;
        options2.confirm = (data) => {
          if (options2.autoclose) {
            this.close(options2);
          }
          resolve(data, options2);
        };
      });
    },
    close(instance) {
      if (this.instances.length < 1) {
        return;
      }
      if (!instance) {
        this.instances.pop();
        return;
      }
      if (typeof instance === "string") {
        instance = find(this.instances, (item2) => item2.id === instance);
      }
      if (instance) {
        const index = this.instances.indexOf(instance);
        this.instances.splice(index, 1);
      }
    },
    closeAll() {
      this.instances.forEach((instance) => {
      });
    }
  }
});
var render$2j = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-categorypicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _vm.previews.length > 0 ? _c("div", {staticClass: "shop-categorypicker-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {staticClass: "shop-categorypicker-preview"}, [_c("ui-select-button", {attrs: {icon: preview.icon, label: preview.name, description: preview.text, disabled: _vm.disabled, tokens: {id: preview.id}}, on: {click: function($event) {
      return _vm.pick(preview.id);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: function($event) {
      return _vm.remove(preview.id);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.canAdd ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.limit > 1 ? "@ui.add" : "@ui.select", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}}) : _vm._e()], 1);
};
var staticRenderFns$2j = [];
var picker_vue_vue_type_style_index_0_lang$3 = ".shop-categorypicker-preview {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.shop-categorypicker-preview .ui-icon-button {\n  height: 24px;\n  width: 24px;\n}\n.shop-categorypicker-preview .ui-icon-button i {\n  font-size: 13px;\n}\n.shop-categorypicker-previews + .ui-select-button,\n.shop-categorypicker-preview + .shop-categorypicker-preview {\n  margin-top: 10px;\n}";
const script$2j = {
  name: "uiCategorypicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    root: {
      type: String,
      default: null
    },
    channel: {
      type: String,
      default: null
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: []
  }),
  watch: {
    value() {
      this.updatePreviews();
    }
  },
  computed: {
    multiple() {
      return this.limit > 1;
    },
    canAdd() {
      let count = isArray(this.value) ? this.value.length : !this.value ? 0 : 1;
      return !this.disabled && count < this.limit;
    }
  },
  mounted() {
    this.updatePreviews();
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    updatePreviews() {
      if (!this.value || isEmpty(this.value)) {
        this.previews = [];
        return;
      }
      let ids = isArray(this.value) ? this.value : [this.value];
      CategoriesApi.getPreviews(ids).then((res) => {
        this.previews = res;
      });
    },
    remove(id) {
      if (isArray(this.value)) {
        let index = this.value.indexOf(id);
        this.value.splice(index, 1);
        this.onChange(this.value);
      } else {
        this.onChange(this.limit > 1 ? [] : null);
      }
    },
    pick(id) {
      if (this.disabled) {
        return;
      }
      let disabledIds = [];
      if (!!this.value && !isArray(this.value)) {
        disabledIds = [this.value];
      } else if (isArray(this.value)) {
        disabledIds = this.value;
      }
      let options2 = extend({
        title: "@shop.category.picker.headline",
        closeLabel: "@ui.close",
        component: CategoryOverlay,
        display: "editor",
        model: this.multiple ? id : this.value,
        rootId: this.root,
        channelId: this.channel,
        disabledIds
      }, typeof this.options === "object" ? this.options : {});
      return Overlay.open(options2).then((value) => {
        if (this.multiple) {
          if (!this.value || !isArray(this.value)) {
            this.onChange([value.id]);
          } else if (this.value.indexOf(value.id) < 0) {
            if (id) {
              this.remove(id);
            }
            this.value.push(value.id);
            this.onChange(this.value);
          }
        } else {
          this.onChange(value ? value.id : null);
        }
      });
    }
  }
};
const __cssModules$2j = {};
var component$2j = normalizeComponent(script$2j, render$2j, staticRenderFns$2j, false, injectStyles$2j, null, null, null);
function injectStyles$2j(context) {
  for (let o in __cssModules$2j) {
    this[o] = __cssModules$2j[o];
  }
}
component$2j.options.__file = "../zero.Commerce/Plugin/pickers/category/picker.vue";
var CategoryPicker$1 = component$2j.exports;
var render$2i = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("category-picker", {attrs: {value: _vm.value, channel: _vm.channelId, limit: _vm.limit}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}});
};
var staticRenderFns$2i = [];
const script$2i = {
  props: {
    value: {
      type: [String, Array]
    },
    entity: {
      type: Object,
      required: true
    },
    limit: {
      type: Number,
      default: 1
    },
    channel: [Function, String],
    config: Object,
    meta: Object
  },
  components: {CategoryPicker: CategoryPicker$1},
  computed: {
    channelId() {
      return typeof this.channel === "function" ? this.channel(this.entity, this.meta) : this.channel || null;
    }
  }
};
const __cssModules$2i = {};
var component$2i = normalizeComponent(script$2i, render$2i, staticRenderFns$2i, false, injectStyles$2i, null, null, null);
function injectStyles$2i(context) {
  for (let o in __cssModules$2i) {
    this[o] = __cssModules$2i[o];
  }
}
component$2i.options.__file = "../zero.Commerce/Plugin/editor/categorypicker.vue";
var CategoryPicker = component$2i.exports;
const base$e = "commerceProducts/";
var ProductsApi = {
  getByCatalogue(id, query) {
    return axios.get(base$e + "getByCatalogue", {params: {query, catalogue: id}}).then((res) => Promise.resolve(res.data));
  },
  getById(id) {
    return axios.get(base$e + "getById", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getEmpty(id) {
    return axios.get(base$e + "getEmpty").then((res) => Promise.resolve(res.data));
  },
  getPreview(id, variantId) {
    return axios.get(base$e + "getPreview", {params: {id, variantId}}).then((res) => Promise.resolve(res.data));
  },
  getPreviews(ids, variants2) {
    return axios.get(base$e + "getPreviews", {params: {ids, variants: variants2}}).then((res) => Promise.resolve(res.data));
  },
  getForPicker(search) {
    return axios.get(base$e + "getForPicker", {params: {search}}).then((res) => Promise.resolve(res.data));
  },
  getByQuery(channel2, category2, query) {
    return axios.get(base$e + "getByQuery", {params: {query, channel: channel2, category: category2}}).then((res) => Promise.resolve(res.data));
  },
  getOptions(id) {
    return axios.get(base$e + "getProductOptions", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  save(model) {
    return axios.post(base$e + "save", model).then((res) => Promise.resolve(res.data));
  },
  delete(id) {
    return axios.delete(base$e + "delete", {params: {id}}).then((res) => Promise.resolve(res.data));
  }
};
const list$p = new List("commerce.category-products");
list$p.templateLabel = (x) => "@shop.product.fields." + x;
list$p.link = "commerce-products-edit";
list$p.query.pageSize = 10;
list$p.onFetch((filter2) => ProductsApi.getByQuery(filter2));
list$p.column("name").name();
list$p.column("isActive").active();
var render$2h = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-category-products"}, [_c("ui-table", {ref: "table", attrs: {config: _vm.list, inline: true}, on: {loaded: _vm.onTableChange}, scopedSlots: _vm._u([{key: "top", fn: function() {
    return [_c("div", {staticClass: "ui-table-row"}, [_c("div", {staticClass: "ui-table-cell shop-category-products-top"}, [_c("ui-button", {attrs: {type: "light", icon: "fth-plus", label: "Add value"}, on: {click: function($event) {
      return _vm.onAdd();
    }}}), _vm.$refs.table ? _c("ui-search", {model: {value: _vm.$refs.table.query.search, callback: function($$v) {
      _vm.$set(_vm.$refs.table.query, "search", $$v);
    }, expression: "$refs.table.query.search"}}) : _vm._e(), _c("ui-button", {attrs: {type: "blank", icon: "fth-more-horizontal"}})], 1)])];
  }, proxy: true}])})], 1);
};
var staticRenderFns$2h = [];
var products_vue_vue_type_style_index_0_lang$1 = ".shop-category-products-top.ui-table-cell {\n  display: grid;\n  grid-template-columns: auto 1fr auto;\n  grid-gap: 20px;\n  padding-top: 16px;\n  padding-bottom: 15px;\n}\n.shop-category-products-top.ui-table-cell .ui-button + .ui-button {\n  margin-left: 0;\n}";
const script$2h = {
  name: "shopCategoryProducts",
  props: {
    value: {
      type: Array,
      default: () => []
    },
    entity: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    list: list$p
  }),
  watch: {
    $route(to, from) {
      this.rebuild();
    }
  },
  created() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.list.onFetch((filter2) => ProductsApi.getByQuery(this.entity.channelId, this.entity.id, filter2));
      if (this.$refs.table) {
        this.$refs.table.initialize();
      }
    },
    onTableChange(result) {
      let parent = this.$parent;
      while (parent != null && parent.$options.name !== "uiTab") {
        parent = parent.$parent;
      }
      if (parent != null) {
        parent.setCount(result.totalItems);
      }
    },
    onAdd() {
    }
  }
};
const __cssModules$2h = {};
var component$2h = normalizeComponent(script$2h, render$2h, staticRenderFns$2h, false, injectStyles$2h, null, null, null);
function injectStyles$2h(context) {
  for (let o in __cssModules$2h) {
    this[o] = __cssModules$2h[o];
  }
}
component$2h.options.__file = "../zero.Commerce/Plugin/pages/categories/partials/products.vue";
var ProductsList = component$2h.exports;
const SORTING_STATES = [
  {value: "@shop.category.sorting_states.default", key: "default"},
  {value: "@shop.category.sorting_states.relevance", key: "relevance"},
  {value: "@shop.category.sorting_states.manual", key: "manual"},
  {value: "@shop.category.sorting_states.dateDesc", key: "dateDesc"},
  {value: "@shop.category.sorting_states.dateAsc", key: "dateAsc"},
  {value: "@shop.category.sorting_states.priceDesc", key: "priceDesc"},
  {value: "@shop.category.sorting_states.priceAsc", key: "priceAsc"},
  {value: "@shop.category.sorting_states.soldUnitsDesc", key: "soldUnitsDesc"},
  {value: "@shop.category.sorting_states.soldUnitsAsc", key: "soldUnitsAsc"}
];
const editor$U = new Editor("commerce.category", "@shop.category.fields.");
const general$8 = editor$U.tab("general", "@shop.category.tabs.content");
const products$2 = editor$U.tab("products", "@shop.category.tabs.products");
general$8.field("name", {label: "@ui.name"}).text(80).required();
general$8.field("parentId").custom(CategoryPicker, {channel: (x) => x.channelId});
general$8.field("isFolder").toggle();
general$8.field("productSorting").select(SORTING_STATES);
general$8.field("description").textarea();
general$8.field("promotionalImageId").image();
products$2.field("assignment.productIds", {hideLabel: true}).when((x) => x.id).custom(ProductsList);
var render$2g = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-currency-input"}, [_c("input", {staticClass: "ui-input", attrs: {type: "text", disabled: _vm.disabled}, domProps: {value: _vm.price}, on: {input: function($event) {
    return _vm.onValueChange($event.target.value);
  }}}), _c("ui-button", {staticClass: "-currency", attrs: {type: "blank", label: "\u20AC"}})], 1);
};
var staticRenderFns$2g = [];
var currency_vue_vue_type_style_index_0_lang = ".shop-currency-input {\n  position: relative;\n}\n.shop-currency-input .ui-input {\n  padding-right: 40px;\n}\n.shop-currency-input .-currency {\n  position: absolute;\n  right: 0;\n  color: var(--color-text-dim);\n  font-weight: 400;\n}";
const script$2g = {
  props: {
    value: {
      type: Number
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    price: 0
  }),
  watch: {
    value() {
      this.rebuild();
    }
  },
  mounted() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.price = this.value;
    },
    onValueChange(value) {
      var parsedValue = parseFloat(value);
      if (isNaN(parsedValue)) {
        parsedValue = 0;
      }
      this.$emit("input", parsedValue);
    }
  }
};
const __cssModules$2g = {};
var component$2g = normalizeComponent(script$2g, render$2g, staticRenderFns$2g, false, injectStyles$2g, null, null, null);
function injectStyles$2g(context) {
  for (let o in __cssModules$2g) {
    this[o] = __cssModules$2g[o];
  }
}
component$2g.options.__file = "../zero.Commerce/Plugin/editor/currency.vue";
var UiCurrency = component$2g.exports;
var currency$1 = /* @__PURE__ */ Object.freeze({
  __proto__: null,
  [Symbol.toStringTag]: "Module",
  default: UiCurrency
});
const editor$T = new Editor("commerce.shipping-price", "@shop.shipping.fields.price.");
editor$T.fieldset((set) => {
  set.field("from").custom(UiCurrency);
  set.field("to").custom(UiCurrency);
});
editor$T.field("price").custom(UiCurrency).required();
const editor$S = new Editor("commerce.shipping", "@shop.shipping.fields.");
editor$S.field("name", {label: "@ui.name"}).text(60).required();
editor$S.field("description").text(120);
editor$S.field("deliveryTime").text(60);
editor$S.field("isPickup").toggle();
editor$S.field("imageId").image();
editor$S.field("countryIds").countryPicker(100);
editor$S.field("prices").nested(editor$T, {
  limit: 5,
  title: "@shop.shipping.fields.price.title",
  itemLabel: (x) => x.price <= 0 ? "@ui.price.free" : Strings.currency(x.price),
  itemDescription: (x) => Localization.localize("@ui.price.xtoy", {hideEmpty: true, tokens: {x: Strings.currency(!x.from ? 0 : x.from), y: !x.to ? "\u221E" : Strings.currency(x.to)}}),
  itemIcon: "fth-dollar-sign",
  template: {from: null, to: null, price: 0}
}).required();
var ShippingOptionsApi = __assign({}, collection$1("commerceShippingOptions/"));
var editor$R = '@charset "UTF-8";\n.editor {\n  width: 100%;\n  max-width: 890px;\n  margin: 0 auto;\n  padding: 0 var(--padding) var(--padding);\n}\n.editor:not(.display-tabs) .ui-tabs-list, .editor.hide-tabs .ui-tabs-list {\n  display: none;\n}\n.editor:not(.display-tabs) .ui-tab, .editor:not(.display-tabs) .editor-infos, .editor.hide-tabs .ui-tab, .editor.hide-tabs .editor-infos {\n  margin-top: 0;\n}\n.editor:not(.display-tabs) .editor-aside, .editor.hide-tabs .editor-aside {\n  margin-top: 0;\n}\n.editor.has-sidebar {\n  max-width: 1580px;\n  display: grid;\n  grid-template-columns: minmax(auto, 1fr) 260px;\n  grid-gap: 0 var(--padding);\n}\n\n.editor.has-below .ui-tab.ui-box {\n  border-bottom-left-radius: 0;\n  border-bottom-right-radius: 0;\n}\n\n.editor.display-boxes .ui-tab {\n  display: inherit !important;\n}\n\n.editor.display-boxes .ui-tab + .ui-tab {\n  margin-top: var(--padding-m);\n}\n\n.editor-tab-headline {\n  font-size: var(--font-size-l) !important;\n  font-weight: 900 !important;\n  margin-bottom: var(--padding) !important;\n}\n\n.editor-tabs .ui-tabs-list {\n  padding: 0;\n  margin-bottom: 0;\n}\n.editor-tabs .ui-tab {\n  margin: 0;\n}\n\n.editor-aside {\n  margin-top: 58px;\n}\n\n.editor-infos {\n  display: block;\n  border-top: 1px solid var(--color-line-onbg);\n}\n\n.theme-dark .editor-infos {\n  border-top: none;\n}\n\n.editor-infos .ui-box {\n  margin: 0;\n  border-top-left-radius: 0;\n  border-top-right-radius: 0;\n  display: flex;\n  gap: var(--padding-l);\n}\n\n.editor-infos .ui-property {\n  flex-direction: column;\n  border-top: none;\n  margin: 0;\n  padding: 0;\n  grid-gap: 6px !important;\n}\n\n.editor-infos .ui-property-label {\n  font-size: var(--font-size-s);\n  font-weight: 400;\n  color: var(--color-text-dim);\n  width: 100%;\n  padding-right: 0;\n}\n\n.editor-infos .ui-property-content {\n  font-size: var(--font-size-s);\n  font-weight: 400;\n  color: var(--color-text-dim);\n}\n\n.editor-infos-aside .ui-property + .ui-property {\n  margin-top: 0;\n  padding-top: 0;\n  border-top: none;\n}\n\n.editor-aside-links {\n  display: flex;\n  width: 100%;\n  margin-bottom: -5px;\n}\n\n.editor-aside-links .ui-link {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-s);\n  text-decoration: none !important;\n  position: relative;\n}\n\n.editor-aside-links .ui-link + .ui-link {\n  margin-left: 20px;\n}\n\n.editor-aside-links .ui-link + .ui-link:before {\n  content: "\xB7";\n  position: absolute;\n  left: -12px;\n  top: -1px;\n}';
var Objects = {
  setValue(model, props, value) {
    const clonedProps = [...props];
    const prop = clonedProps.shift();
    if (!model[prop]) {
      model[prop] = {};
    }
    if (!clonedProps.length) {
      if (value && typeof value === "object" && !Array.isArray(value)) {
        model[prop] = __assign(__assign({}, model[prop]), value);
      } else {
        model[prop] = value;
      }
      return;
    }
    this.setValue(model[prop], clonedProps, value);
  }
};
var render$2f = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.isHidden ? _c("ui-property", {class: {"is-disabled": _vm.isDisabled, "has-block": !!_vm.blockComponent}, attrs: {field: _vm.config.path, label: _vm.label, "hide-label": _vm.config.options.hideLabel, description: _vm.description, required: _vm.isRequired, disabled: _vm.isDisabled, vertical: _vm.config.options.vertical}, scopedSlots: _vm._u([_vm.blockComponent && _vm.loaded ? {key: "after", fn: function() {
    return [_c(_vm.blockComponent, _vm._b({tag: "component"}, "component", {config: _vm.config, editor: _vm.editor, value: _vm.value, model: _vm.model}, false))];
  }, proxy: true} : null], null, true)}, [_c(_vm.config.component, _vm._b({tag: "component", attrs: {value: _vm.model, entity: _vm.value, meta: _vm.meta, disabled: _vm.isDisabled}, on: {input: _vm.onChange}}, "component", _vm.config.componentOptions, false)), _vm.config.options.helpText ? _c("p", {directives: [{name: "localize", rawName: "v-localize", value: _vm.config.options.helpText, expression: "config.options.helpText"}], staticClass: "ui-property-help"}) : _vm._e()], 1) : _vm._e();
};
var staticRenderFns$2f = [];
const script$2f = {
  name: "uiEditorComponent",
  inject: ["meta"],
  props: {
    config: {
      type: Object,
      required: true
    },
    editor: {
      type: Object,
      required: true
    },
    value: {
      type: Object,
      required: true
    }
  },
  watch: {
    value: {
      deep: true,
      handler: function() {
        this.rebuildModel();
      }
    }
  },
  data: () => ({
    model: null,
    loaded: false,
    blockComponent: null,
    manualDisabled: false,
    selector: null
  }),
  mounted() {
    this.rebuildModel();
    this.loaded = true;
  },
  computed: {
    isHidden() {
      return this.loaded && typeof this.config.options.condition === "function" && !this.config.options.condition(this.value, this);
    },
    isRequired() {
      return typeof this.config.isRequired === "function" ? this.config.isRequired(this.value) : this.config.isRequired;
    },
    isDisabled() {
      return this.manualDisabled || typeof this.config.options.disabled === "boolean" && this.config.options.disabled || typeof this.config.options.disabled === "function" && this.config.options.disabled(this.value, this.model);
    },
    label() {
      return this.config.options.label || this.editor.templateLabel(this.config.path);
    },
    description() {
      return Localization.localize(this.config.options.description || this.editor.templateDescription(this.config.path), {hideEmpty: true});
    }
  },
  methods: {
    rebuildModel() {
      this.selector = Strings.selectorToArray(this.config.path);
      let currentValue = this.value;
      let found = false;
      if (!this.selector || !this.selector.length || !currentValue) {
        found = true;
        this.model = null;
      } else {
        for (var key of this.selector) {
          if (key in currentValue) {
            found = true;
            currentValue = currentValue[key];
          } else {
            break;
          }
        }
        this.model = found ? currentValue : null;
      }
    },
    onChange(value) {
      let oldValue = JSON.parse(JSON.stringify(this.model));
      if (typeof value === "function") {
        value(this.value);
      } else {
        Objects.setValue(this.value, this.selector, value);
      }
      this.$emit("input", this.value);
      if (typeof this.config.options.onChange === "function") {
        this.config.options.onChange(value, {
          oldValue,
          model: this.value,
          component: this
        });
      }
    },
    setDisabled(disabled) {
      this.manualDisabled = disabled;
    },
    setBlock(component2) {
      this.blockComponent = component2;
    }
  }
};
const __cssModules$2f = {};
var component$2f = normalizeComponent(script$2f, render$2f, staticRenderFns$2f, false, injectStyles$2f, null, null, null);
function injectStyles$2f(context) {
  for (let o in __cssModules$2f) {
    this[o] = __cssModules$2f[o];
  }
}
component$2f.options.__file = "app/editor/editor-component.vue";
var EditorComponent = component$2f.exports;
var render$2e = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", [_vm.value.id && _vm.value.lastModifiedDate ? _c("ui-property", {attrs: {field: "lastModifiedDate", label: "@ui.modifiedDate", "is-text": true, vertical: true}}, [_c("ui-date", {model: {value: _vm.value.lastModifiedDate, callback: function($$v) {
    _vm.$set(_vm.value, "lastModifiedDate", $$v);
  }, expression: "value.lastModifiedDate"}})], 1) : _vm._e(), _vm.value.id ? _c("ui-property", {attrs: {label: "@ui.createdDate", field: "createdDate", "is-text": true, vertical: true}}, [_c("ui-date", {model: {value: _vm.value.createdDate, callback: function($$v) {
    _vm.$set(_vm.value, "createdDate", $$v);
  }, expression: "value.createdDate"}})], 1) : _vm._e()], 1);
};
var staticRenderFns$2e = [];
var editorAside_vue_vue_type_style_index_0_lang = ".editor-aside {\n  padding-top: var(--padding);\n  padding-left: var(--padding-s);\n}\n.editor-aside .ui-property + .ui-property {\n  margin-top: var(--padding-m);\n}";
const script$2e = {
  name: "uiEditorAside",
  inject: ["meta"],
  props: {
    editor: {
      type: Object,
      required: true
    },
    value: {
      type: Object,
      required: true
    },
    infos: {
      type: String,
      default: "aside"
    },
    activeToggle: {
      type: Boolean,
      default: true
    },
    nested: {
      type: Boolean,
      default: false
    },
    isPage: {
      type: Boolean,
      default: false
    },
    disabled: {
      type: [Boolean, Function],
      default: false
    }
  },
  data: () => ({
    component: null
  }),
  created() {
    this.component = zero.overrides["editor-aside"] || null;
  }
};
const __cssModules$2e = {};
var component$2e = normalizeComponent(script$2e, render$2e, staticRenderFns$2e, false, injectStyles$2e, null, null, null);
function injectStyles$2e(context) {
  for (let o in __cssModules$2e) {
    this[o] = __cssModules$2e[o];
  }
}
component$2e.options.__file = "app/editor/editor-aside.vue";
var EditorAside = component$2e.exports;
var render$2d = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.loaded ? _c("div", {staticClass: "editor", class: ["display-" + _vm.display, {"has-sidebar": _vm.asideDefined, "hide-tabs": _vm.tabs.length < 2, "has-below": _vm.belowDefined}]}, [_c("ui-tabs", {staticClass: "editor-tabs"}, _vm._l(_vm.tabs, function(tab, index) {
    return !tab.disabled(_vm.value) ? _c("ui-tab", {key: index, staticClass: "ui-box", class: tab.class, attrs: {label: tab.name, count: tab.count(_vm.value)}}, [_vm.display == "boxes" && tab.name ? _c("h3", {directives: [{name: "localize", rawName: "v-localize", value: tab.name, expression: "tab.name"}], staticClass: "ui-headline editor-tab-headline"}) : _vm._e(), _vm._l(tab.fieldsets, function(fieldset) {
      return _c("div", {staticClass: "ui-property ui-property-parent"}, _vm._l(fieldset.fields, function(field, fieldIndex) {
        return _c("editor-component", {key: fieldIndex, class: field.options.class, style: {"grid-column": field.options.fieldset ? "span " + field.options.fieldsetColumns : null}, attrs: {config: field, editor: _vm.editorConfig, value: _vm.value, "data-cols": !!field.options.fieldset}, on: {input: _vm.onChange}});
      }), 1);
    }), tab.component ? _c(tab.component, {tag: "component", model: {value: _vm.value, callback: function($$v) {
      _vm.value = $$v;
    }, expression: "value"}}) : _vm._e()], 2) : _vm._e();
  }), 1), _vm.asideDefined ? _c("aside", {staticClass: "editor-aside"}, [_vm._t("aside")], 2) : _vm._e(), _vm.belowDefined ? _c("aside", {staticClass: "editor-below"}, [_vm._t("below")], 2) : _vm._e()], 1) : _vm._e();
};
var staticRenderFns$2d = [];
const script$2d = {
  name: "uiEditor",
  provide: function() {
    return {
      meta: this.meta,
      editor: this.config
    };
  },
  props: {
    config: {
      type: [String, Object],
      required: true
    },
    meta: {
      type: Object,
      default: () => {
      }
    },
    value: {
      type: Object
    },
    onConfigure: {
      type: Function,
      default: () => {
      }
    }
  },
  components: {EditorComponent, EditorAside},
  data: () => ({
    editorConfig: {},
    loaded: false,
    tabs: [],
    currentFieldset: null
  }),
  computed: {
    asideDefined() {
      return this.$scopedSlots.hasOwnProperty("aside");
    },
    belowDefined() {
      return this.$scopedSlots.hasOwnProperty("below");
    }
  },
  created() {
    this.editorConfig = typeof this.config === "string" ? this.zero.getEditor(this.config) : this.config;
    this.tabs = this.editorConfig.tabs.map((tab) => {
      let fieldsets = this.editorConfig.getFieldsets(tab);
      return __assign(__assign({}, tab), {
        count: (value) => typeof tab.count === "number" ? tab.count : typeof tab.count === "function" ? tab.count(value) : 0,
        disabled: (value) => typeof tab.disabled === "boolean" ? tab.disabled : typeof tab.disabled === "function" ? tab.disabled(value) : false,
        fieldsets
      });
    });
    this.display = this.editorConfig.options.display || "tabs";
    this.onConfigure(this);
    this.loaded = true;
  },
  methods: {
    onChange() {
      this.$emit("input", this.value);
    }
  }
};
const __cssModules$2d = {};
var component$2d = normalizeComponent(script$2d, render$2d, staticRenderFns$2d, false, injectStyles$2d, null, null, null);
function injectStyles$2d(context) {
  for (let o in __cssModules$2d) {
    this[o] = __cssModules$2d[o];
  }
}
component$2d.options.__file = "app/editor/editor.vue";
var UiEditor = component$2d.exports;
var render$2c = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-module-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: "@shop.channel.fields.mailing.templateOverrides_headline", "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", submit: true, label: "Confirm", state: form.state, disabled: _vm.loading || _vm.disabled}})];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-module-overlay-editor"}, [_c("ui-editor", {attrs: {config: _vm.editor, meta: _vm.meta, "is-page": false, infos: "none"}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$2c = [];
var mailtemplate_vue_vue_type_style_index_0_lang = ".ui-module-overlay > content {\n  position: relative;\n  padding-top: 0 !important;\n}\n.ui-module-overlay .ui-box {\n  margin: 0;\n}\n.ui-module-overlay .ui-tabs-list {\n  padding: 0;\n  padding-bottom: 32px;\n}\n.ui-module-overlay .ui-property.ui-modules {\n  margin: 0;\n  padding: 0;\n}\n.ui-module-overlay .editor-outer.-infos-aside:not(.is-page) {\n  display: block;\n}\n.ui-module-overlay .ui-loading {\n  position: absolute;\n  left: 50%;\n  top: 50%;\n  margin: -14px 0 0 -14px;\n}";
const script$2c = {
  props: {
    config: Object
  },
  components: {UiEditor},
  data: () => ({
    isAdd: true,
    disabled: false,
    id: null,
    loading: true,
    state: "default",
    meta: {},
    model: {},
    template: {},
    editor: null
  }),
  created() {
    this.editor = this.config.editor;
  },
  methods: {
    onLoad(form) {
      this.loading = true;
      this.model = JSON.parse(JSON.stringify(this.config.model));
      form.load(MailTemplatesApi.getById(this.config.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.template = response.entity;
        this.editor.fields.forEach((config2) => {
          let value = response.entity[config2.path];
          let channelDefault = this.config.defaults[config2.path];
          value = channelDefault || value;
          config2.options.helpText = !value ? null : Localization.localize("@shop.channel.fields.mailing.default_template", {
            tokens: {value}
          });
        });
        this.loading = false;
      });
    },
    onSubmit(form) {
      this.config.confirm(this.model);
    }
  }
};
const __cssModules$2c = {};
var component$2c = normalizeComponent(script$2c, render$2c, staticRenderFns$2c, false, injectStyles$2c, null, null, null);
function injectStyles$2c(context) {
  for (let o in __cssModules$2c) {
    this[o] = __cssModules$2c[o];
  }
}
component$2c.options.__file = "../zero.Commerce/Plugin/pages/channels/overlays/mailtemplate.vue";
var OverrideMailTemplateOverlay = component$2c.exports;
const editor$Q = new Editor("commerce.mail-override", "@mailTemplate.fields.");
editor$Q.field("isDeactivated", {label: "@shop.channel.fields.mailing.template_isDeactivated"}).toggle(true);
editor$Q.field("subject").when((x) => !x.isDeactivated).text(80).required();
editor$Q.field("body").when((x) => !x.isDeactivated).rte();
editor$Q.field("senderEmail").when((x) => !x.isDeactivated).text();
editor$Q.field("senderName").when((x) => !x.isDeactivated).text();
editor$Q.field("recipientEmail").when((x) => !x.isDeactivated).text();
editor$Q.field("cc").when((x) => !x.isDeactivated).text();
editor$Q.field("bcc").when((x) => !x.isDeactivated).text();
var render$2b = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-channel-mailtemplates"}, [_vm.previews.length ? _c("div", {staticClass: "ui-pick-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {staticClass: "ui-pick-preview", class: {"has-error": preview.hasError}}, [_c("ui-select-button", {attrs: {icon: preview.icon, label: preview.name, description: preview.text, disabled: _vm.disabled, tokens: preview}, on: {click: function($event) {
      preview.hasError ? null : _vm.edit(preview.override);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: function($event) {
      return _vm.removeItem(preview.override);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _c("ui-pick", {ref: "picker", attrs: {config: _vm.pickerConfig, disabled: _vm.disabled}, on: {select: _vm.add}})], 1);
};
var staticRenderFns$2b = [];
var mailtemplates_vue_vue_type_style_index_0_lang = ".shop-channel-mailtemplates .ui-pick-previews + .ui-pick {\n  margin-top: 10px;\n}\n.shop-channel-mailtemplates .ui-pick-preview.has-error .ui-select-button {\n  cursor: default;\n}";
const script$2b = {
  props: {
    value: {
      type: Array,
      default: () => []
    },
    entity: {
      type: Object,
      default: () => {
      }
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    threeCols() {
      return this.entity.type !== "select";
    }
  },
  data: () => ({
    items: [],
    previews: [],
    pickerConfig: {},
    selectedIds: []
  }),
  watch: {
    value(val) {
      this.rebuild();
    }
  },
  created() {
    this.pickerConfig = {
      scope: "mailtemplate",
      items: MailTemplatesApi.getForPicker,
      limit: 1,
      multiple: false,
      preview: {
        enabled: false
      }
    };
    this.rebuild();
  },
  methods: {
    rebuild() {
      let maps = {};
      this.selectedIds = [];
      this.items = [];
      if (this.value) {
        this.value.forEach((item2) => {
          this.selectedIds.push(item2.mailTemplateId);
          maps[item2.mailTemplateId] = item2;
          this.items.push(item2);
        });
      }
      this.pickerConfig.excludedIds = this.selectedIds;
      MailTemplatesApi.getPreviews(this.selectedIds).then((previews) => {
        previews.forEach((preview) => {
          preview.override = maps[preview.id];
          if (preview.override.isDeactivated) {
            preview.icon = "fth-slash";
            preview.text = "@shop.channel.fields.mailing.template_deactivated";
          }
        });
        this.previews = previews;
      });
      if (this.$refs.picker) {
        this.$refs.picker.refresh();
      }
    },
    add(id) {
      this.edit({
        mailTemplateId: id,
        isDeactivated: false,
        senderEmail: null,
        senderName: null,
        recipientEmail: null,
        cc: null,
        bcc: null,
        subject: null,
        body: null
      }, true);
    },
    edit(item2, isAdd) {
      let defaults = JSON.parse(JSON.stringify(this.entity.mailing));
      defaults.senderName = defaults.senderName || this.entity.name;
      return Overlay.open({
        component: OverrideMailTemplateOverlay,
        display: "editor",
        editor: editor$Q,
        id: item2.mailTemplateId,
        defaults,
        model: item2,
        width: 1100
      }).then((value) => {
        if (isAdd) {
          this.items.push(value);
        } else {
          const index = this.items.indexOf(item2);
          this.items[index] = value;
        }
        this.onChange();
      });
    },
    removeItem(item2) {
      const index = this.items.indexOf(item2);
      this.items.splice(index, 1);
      this.onChange();
    },
    onChange() {
      this.$emit("input", this.items);
    }
  }
};
const __cssModules$2b = {};
var component$2b = normalizeComponent(script$2b, render$2b, staticRenderFns$2b, false, injectStyles$2b, null, null, null);
function injectStyles$2b(context) {
  for (let o in __cssModules$2b) {
    this[o] = __cssModules$2b[o];
  }
}
component$2b.options.__file = "../zero.Commerce/Plugin/pages/channels/partials/mailtemplates.vue";
var MailTemplates = component$2b.exports;
const base$d = "commerceChannels/";
var ChannelsApi = {
  getById(id) {
    return axios.get(base$d + "getById", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getByIdOrDefault(id) {
    return axios.get(base$d + "getByIdOrDefault", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getEmpty(type) {
    return axios.get(base$d + "getEmpty", {params: {type}}).then((res) => Promise.resolve(res.data));
  },
  getByQuery(query) {
    return axios.get(base$d + "getListByQuery", {params: {query}}).then((res) => Promise.resolve(res.data));
  },
  getAll() {
    return axios.get(base$d + "getAll").then((res) => Promise.resolve(res.data));
  },
  getAllFeatures() {
    return axios.get(base$d + "getAllFeatures").then((res) => Promise.resolve(res.data));
  },
  getPreviews(ids) {
    return axios.get(base$d + "getPreviews", {params: {ids}}).then((res) => Promise.resolve(res.data));
  },
  getForPicker(search) {
    return axios.get(base$d + "getForPicker", {params: {search}}).then((res) => Promise.resolve(res.data));
  },
  save(model) {
    return axios.post(base$d + "save", model).then((res) => Promise.resolve(res.data));
  },
  delete(id) {
    return axios.delete(base$d + "delete", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getChannelTypes: async () => await get$1(base$d + "getChannelTypes")
};
var render$2a = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-channel-features"}, _vm._l(_vm.features, function(feature) {
    return _c("ui-property", {key: feature.alias, attrs: {label: feature.name, description: feature.description}}, [_c("ui-toggle", {attrs: {value: _vm.value.indexOf(feature.alias) > -1, disabled: _vm.disabled}, on: {input: function($event) {
      return _vm.onFeatureToggle($event, feature);
    }}})], 1);
  }), 1);
};
var staticRenderFns$2a = [];
const script$2a = {
  props: {
    value: {
      type: Array,
      default: () => []
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    features: []
  }),
  created() {
    ChannelsApi.getAllFeatures().then((items) => this.features = items);
  },
  methods: {
    onFeatureToggle(isOn, feature) {
      const alias2 = feature.alias;
      const index = this.value.indexOf(alias2);
      if (!isOn && index > -1) {
        this.value.splice(index, 1);
      } else if (isOn && index === -1) {
        this.value.push(alias2);
      }
      this.$emit("input", this.value);
    }
  }
};
const __cssModules$2a = {};
var component$2a = normalizeComponent(script$2a, render$2a, staticRenderFns$2a, false, injectStyles$2a, null, null, null);
function injectStyles$2a(context) {
  for (let o in __cssModules$2a) {
    this[o] = __cssModules$2a[o];
  }
}
component$2a.options.__file = "../zero.Commerce/Plugin/pages/channels/partials/features.vue";
var ChannelFeatures = component$2a.exports;
const editor$P = new Editor("commerce.channel", "@shop.channel.fields.");
const general$7 = editor$P.tab("general", "@ui.tab_general");
const mails = editor$P.tab("mails", "@shop.channel.tabs.mails");
const settings$2 = editor$P.tab("settings", "@shop.channel.tabs.settings");
const features = editor$P.tab("features", "@shop.channel.tabs.features", null, (x) => !x.features.length);
general$7.field("name", {label: "@ui.name"}).text(60).required();
general$7.field("imageId").image();
mails.field("mailing.senderEmail", {label: "@mailTemplate.fields.senderEmail", description: "@mailTemplate.fields.senderEmail_text"}).text(80);
mails.field("mailing.senderName", {label: "@mailTemplate.fields.senderName", description: "@mailTemplate.fields.senderName_text"}).text(80);
mails.field("mailing.recipientEmail", {label: "@mailTemplate.fields.recipientEmail", description: "@mailTemplate.fields.recipientEmail_text"}).text(80);
mails.field("mailing.cc", {label: "@mailTemplate.fields.cc", description: "@mailTemplate.fields.cc_text"}).text(120);
mails.field("mailing.bcc", {label: "@mailTemplate.fields.bcc", description: "@mailTemplate.fields.bcc_text"}).text(120);
mails.field("mailing.templateOverrides").custom(MailTemplates);
settings$2.field("shipping.hiddenIds").checkList(ShippingOptionsApi.getForPicker, {
  labelKey: "name",
  idKey: "id",
  reverse: true
});
settings$2.field("shipping.items").nested(editor$S, {
  title: "@shop.shipping.name",
  limit: 5,
  itemIcon: "fth-truck",
  itemLabel: (x) => x.name,
  itemDescription: (x) => x.description,
  template: {
    name: null,
    description: null,
    deliveryTime: null,
    countryIds: [],
    imageId: null,
    prices: []
  }
});
features.field("features", {hideLabel: true}).custom(ChannelFeatures);
const editor$O = new Editor("commerce.currency", "@shop.currency.fields.");
editor$O.field("name", {label: "@ui.name"}).text(30).required();
editor$O.field("isDefault").toggle();
editor$O.fieldset((set) => {
  set.field("code").text(10).required();
  set.field("symbol").text(2).required();
  set.field("factor").number().required();
});
const editor$N = new Editor("commerce.customer-address", "@shop.customer.fields.address.");
editor$N.fieldset((set) => {
  set.field("name").text(120).required();
  set.field("title").cols(3).text(15);
});
editor$N.fieldset((set) => {
  set.field("gender").select([
    {value: "@gender.undisclosed", key: "undisclosed"},
    {value: "@gender.female", key: "female"},
    {value: "@gender.male", key: "male"},
    {value: "@gender.nonbinary", key: "nonBinary"}
  ]).required();
  set.field("phoneNumber").text(30);
});
editor$N.fieldset((set) => {
  set.field("company").text(80);
  set.field("vatNo").text(30);
});
editor$N.fieldset((set) => {
  set.field("address").cols(5).text(120).required();
  set.field("addressNo").cols(3).text(10).required();
  set.field("addressLine1").cols(2).text(60);
  set.field("addressLine2").cols(2).text(60);
});
editor$N.fieldset((set) => {
  set.field("zip").cols(4).text(10).required();
  set.field("city").text(60).required();
});
const base$c = "commerceOrders/";
var OrdersApi = {
  getById(id) {
    return axios.get(base$c + "getById", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getEmpty(id) {
    return axios.get(base$c + "getEmpty").then((res) => Promise.resolve(res.data));
  },
  getByQuery(query, customer2) {
    return axios.get(base$c + "getListByQuery", {params: {query, customer: customer2}}).then((res) => Promise.resolve(res.data));
  },
  getPreviews(ids) {
    return axios.get(base$c + "getPreviews", {params: {ids}}).then((res) => Promise.resolve(res.data));
  },
  getForPicker() {
    return axios.get(base$c + "getForPicker").then((res) => Promise.resolve(res.data));
  },
  getRevisions(id, page) {
    return axios.get(base$c + "getRevisions", {params: {id, page}}).then((res) => Promise.resolve(res.data));
  },
  getDocumentFile(id, documentId) {
    return axios.get(base$c + "getDocument", {params: {id, documentId}}).then((res) => Promise.resolve(res.data));
  },
  getDocumentUrl: (id, documentId) => {
    return zero.apiPath + base$c + "getDocument?id=" + id + "&documentId=" + documentId;
  },
  save(model) {
    return axios.post(base$c + "save", model).then((res) => Promise.resolve(res.data));
  },
  delete(id) {
    return axios.delete(base$c + "delete", {params: {id}}).then((res) => Promise.resolve(res.data));
  }
};
const list$o = new List("commerce.customer-orders");
list$o.templateLabel = (x) => "@shop.order.fields." + x;
list$o.link = "commerce-orders-edit";
list$o.query.pageSize = 10;
list$o.onFetch((filter2) => OrdersApi.getByQuery(filter2));
list$o.column("order", {class: "is-bold"}).custom((value, model) => model.number);
list$o.column("state").custom((value, model) => `<span class="shop-orders-col-state2" data-state="${value}">${model.detailedState}</span>`, true);
list$o.column("price").custom((value, model) => Strings.currency(value), true);
list$o.column("createdDate").created();
var render$29 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-orders"}, [_c("ui-header-bar", {attrs: {title: "@shop.order.list", prefix: "@shop.headline_prefix", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {route: "commerce-orders-create", decision: false}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: "commerce.orders"}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1);
};
var staticRenderFns$29 = [];
var orders_vue_vue_type_style_index_0_lang = '.shop-orders .ui-table-row:not(.ui-table-head) {\n  min-height: 70px;\n}\n.shop-orders-col-channel-image {\n  max-height: 20px;\n  max-width: 20px;\n  margin-right: 12px;\n}\n.shop-orders .ui-icon.-flag {\n  position: relative;\n  top: 4px;\n  margin-right: 3px;\n  margin-left: -2px;\n  display: inline-block;\n  transform: scale(0.8);\n  transform-origin: 50% 0%;\n}\n.shop-orders-col-state {\n  display: inline-flex;\n  align-items: center;\n  font-size: var(--font-size-s);\n  color: var(--color-text);\n  height: 20px;\n  font-weight: 600;\n  /*&[data-state="completed"],\n  &[data-state="cancelled"]\n  {\n    color: var(--color-text-dim);\n  }*/\n}\n.shop-orders-col-state .-icon {\n  background: var(--color-box-nested);\n  width: 26px;\n  height: 26px;\n  border-radius: 16px;\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  font-size: var(--font-size-s);\n  margin-right: 10px;\n}\n.shop-orders-col-state[data-state=processing] .-icon {\n  background: var(--color-accent-info-bg);\n  color: var(--color-accent-info);\n}\n.shop-orders-col-state[data-state=open] .-icon {\n  background: var(--color-table-highlight);\n  color: var(--color-text);\n}\n.shop-orders-col-state[data-state=completed] .-icon {\n  background: var(--color-accent-success-bg);\n  color: var(--color-accent-success);\n}\n.shop-orders-col-state[data-state=completed], .shop-orders-col-state[data-state=cancelled] {\n  color: var(--color-text-dim);\n  font-weight: 400;\n}\n.shop-orders-col-state[data-state=cancelled] .-icon {\n  background: var(--color-accent-error-bg);\n  color: var(--color-accent-error);\n}\n.shop-orders-col-state2 {\n  display: inline-flex;\n  align-self: center;\n  align-items: center;\n  font-size: var(--font-size-xs);\n  color: var(--color-text);\n  font-weight: 600;\n  background: var(--color-table-highlight);\n  padding: 4px 12px;\n  border-radius: 20px;\n  /*&[data-payment-state="none"], &[data-payment-state="pending"], &[data-payment-state="cancelled"], &[data-payment-state="refunded"]\n  {\n    font-weight: 400;\n    color: var(--color-text-dim);\n  }*/\n}\n.shop-orders-col-state2[data-state=completed], .shop-orders-col-state2[data-state=cancelled] {\n  font-weight: 400;\n  color: var(--color-text-dim);\n}\n.shop-orders-col-state2[data-payment-state] {\n  font-weight: 400;\n  color: var(--color-text-dim);\n}\n.shop-orders-col-state2[data-payment-state=error] {\n  color: var(--color-accent-error);\n  background: var(--color-accent-error-bg);\n}\n.ui-table-row:hover .shop-orders-col-state2 {\n  background: var(--color-bg-shade-4);\n}\n.shop-orders-statistics {\n  display: flex;\n  grid-gap: var(--padding-s);\n  padding: 0 var(--padding);\n}\n.shop-orders-statistics .ui-box {\n  margin: 0;\n}\n.shop-orders-statistic {\n  font-size: var(--font-size-s);\n  color: var(--color-text-dim);\n  line-height: 1.5;\n  padding: var(--padding-s) var(--padding-m);\n  min-width: 200px;\n}\n.shop-orders-statistic strong {\n  display: block;\n  color: var(--color-text);\n  font-size: var(--font-size-l);\n}';
const script$29 = {
  data: () => ({
    count: 0
  })
};
const __cssModules$29 = {};
var component$29 = normalizeComponent(script$29, render$29, staticRenderFns$29, false, injectStyles$29, null, null, null);
function injectStyles$29(context) {
  for (let o in __cssModules$29) {
    this[o] = __cssModules$29[o];
  }
}
component$29.options.__file = "../zero.Commerce/Plugin/pages/orders/orders.vue";
var orders$1 = component$29.exports;
var orders$2 = /* @__PURE__ */ Object.freeze({
  __proto__: null,
  [Symbol.toStringTag]: "Module",
  default: orders$1
});
var render$28 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-customer-orders"}, [_c("ui-table", {ref: "table", attrs: {config: _vm.list, inline: true}, on: {loaded: _vm.onTableChange}})], 1);
};
var staticRenderFns$28 = [];
const script$28 = {
  name: "shopCustomerOrders",
  props: {
    value: {
      type: String,
      default: null
    },
    entity: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    list: list$o
  }),
  watch: {
    $route(to, from) {
      this.rebuild();
    }
  },
  created() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.list.onFetch((filter2) => OrdersApi.getByQuery(filter2, this.entity.id));
      if (this.$refs.table) {
        this.$refs.table.initialize();
      }
    },
    onTableChange(result) {
      let parent = this.$parent;
      while (parent != null && parent.$options.name !== "uiTab") {
        parent = parent.$parent;
      }
      if (parent != null) {
        parent.setCount(result.totalItems);
      }
    }
  }
};
const __cssModules$28 = {};
var component$28 = normalizeComponent(script$28, render$28, staticRenderFns$28, false, injectStyles$28, null, null, null);
function injectStyles$28(context) {
  for (let o in __cssModules$28) {
    this[o] = __cssModules$28[o];
  }
}
component$28.options.__file = "../zero.Commerce/Plugin/pages/customers/partials/orders.vue";
var OrdersList = component$28.exports;
const editor$M = new Editor("commerce.customer", "@shop.customer.fields.");
const general$6 = editor$M.tab("general", "@ui.tab_general");
const orders = editor$M.tab("orders", "@shop.customer.tabs.orders");
general$6.fieldset((set) => {
  set.field("email").text(120).required();
  set.field("no").cols(2).output();
});
general$6.field("addresses").nested(editor$N, {
  limit: 10,
  title: "@shop.customer.fields.address.edit_title",
  itemIcon: "fth-map-pin",
  itemLabel: (x) => x.company ? x.company : x.name,
  itemDescription: (x) => x.zip + " " + x.city + ", " + x.address + (x.addressNo ? " " + x.addressNo : ""),
  template: {
    company: null,
    name: null,
    gender: "undisclosed",
    title: null,
    vatNo: null,
    phoneNumber: null,
    address: null,
    addressLine1: null,
    addressLine2: null,
    zip: null,
    city: null,
    countryId: null
  }
}).required();
const createAddressOptions = (entity) => entity.addresses.map((x) => {
  return {
    key: x.id,
    value: x.company ? x.company : x.name + " (" + x.zip + " " + x.city + ", " + x.address + (x.addressNo ? " " + x.addressNo : "") + ")"
  };
});
general$6.field("defaultShippingAddressId").select(createAddressOptions, {emptyOption: true});
general$6.field("defaultInvoiceAddressId").select(createAddressOptions, {emptyOption: true});
orders.field("id", {hideLabel: true}).when((x) => x.id).custom(OrdersList);
const editor$L = new Editor("commerce.manufacturer", "@shop.manufacturer.fields.");
editor$L.field("name", {label: "@ui.name"}).text(60).required();
editor$L.field("imageId").image();
editor$L.field("description").rte();
editor$L.field("website").text();
editor$L.field("promotionalImageId").image();
editor$L.field("synonyms", {label: "@shop.synonyms.label", description: "@shop.synonyms.description"}).tags();
const base$b = "commerceNumbers/";
var NumbersApi = {
  getByAlias: async (alias2, config2) => await get$1(base$b + "getByAlias", __assign(__assign({}, config2), {params: {alias: alias2}})),
  getByQuery: async (query, config2) => await get$1(base$b + "getByQuery", __assign(__assign({}, config2), {params: {query}})),
  getCounters: async (alias2, config2) => await get$1(base$b + "getCounters", __assign(__assign({}, config2), {params: {alias: alias2}})),
  save: async (model, config2) => await post$1(base$b + "save", model, __assign({}, config2))
};
const list$n = new List("commerce.number-counters");
list$n.templateLabel = (x) => "@shop.number.fields.counterlist." + x;
list$n.query.pageSize = 10;
list$n.column("key", {canSort: false}).text();
list$n.column("count", {canSort: false}).text();
list$n.column("value", {canSort: false}).text({localize: true});
var render$27 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-number-counters"}, [_c("ui-table", {ref: "table", attrs: {config: _vm.list, inline: true}})], 1);
};
var staticRenderFns$27 = [];
const script$27 = {
  props: {
    value: {
      type: String,
      required: true
    },
    entity: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    list: list$n
  }),
  watch: {
    $route(to, from) {
      this.rebuild();
    }
  },
  created() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.list.onFetch((filter2) => NumbersApi.getCounters(this.value));
      if (this.$refs.table) {
        this.$refs.table.initialize();
      }
    }
  }
};
const __cssModules$27 = {};
var component$27 = normalizeComponent(script$27, render$27, staticRenderFns$27, false, injectStyles$27, null, null, null);
function injectStyles$27(context) {
  for (let o in __cssModules$27) {
    this[o] = __cssModules$27[o];
  }
}
component$27.options.__file = "../zero.Commerce/Plugin/pages/settings/partials/number-counters.vue";
var NumberCounters = component$27.exports;
const editor$K = new Editor("commerce.number", "@shop.number.fields.");
const general$5 = editor$K.tab("general", null);
const counters = editor$K.tab("counters", null);
editor$K.options.display = "boxes";
general$5.field("template").text(60).required();
general$5.fieldset((set) => {
  set.field("startNumber").number().required();
  set.field("minLength").number(2).required();
});
general$5.field("resetNumberPerYear").toggle();
counters.field("typeAlias", {label: "@shop.number.fields.counters", description: "@shop.number.fields.counters_text"}).custom(NumberCounters);
const STATE_TYPES$1 = [
  {value: "@shop.orderstate.states.open", key: "open"},
  {value: "@shop.orderstate.states.processing", key: "processing"},
  {value: "@shop.orderstate.states.completed", key: "completed"},
  {value: "@shop.orderstate.states.cancelled", key: "cancelled"}
];
const editor$J = new Editor("commerce.orderstate", "@shop.orderstate.fields.");
editor$J.field("name", {label: "@ui.name"}).text(60).required();
editor$J.field("underlyingState").when((x) => !x.isInternal).select(STATE_TYPES$1).required();
editor$J.field("underlyingState", {helpText: "@shop.orderstate.fields.isInternal_info"}).when((x) => x.isInternal).output((value) => {
  const state = STATE_TYPES$1.find((t) => t.key == value);
  return state ? Localization.localize(state.value) : null;
}).required();
editor$J.field("description").text(200);
editor$J.field("text").rte();
const editor$I = new Editor("commerce.order-address", "@shop.customer.fields.address.");
editor$I.field("countryId").countryPicker().required();
editor$I.fieldset((set) => {
  set.field("name").text(120).required();
  set.field("company").text(80);
});
editor$I.fieldset((set) => {
  set.field("address").cols(5).text(120).required();
  set.field("addressNo").cols(3).text(10).required();
  set.field("addressLine1").cols(2).text(60);
  set.field("addressLine2").cols(2).text(60);
});
editor$I.fieldset((set) => {
  set.field("zip").cols(4).text(10).required();
  set.field("city").text(60).required();
});
var render$26 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-shippingoptionpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange, select: _vm.onSelect}})], 1);
};
var staticRenderFns$26 = [];
const script$26 = {
  name: "shopShippingoptionpicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "shippingOption",
      items: ShippingOptionsApi.getForPicker,
      previews: ShippingOptionsApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true,
        iconAsImage: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    },
    onSelect(value, model) {
      this.$emit("select", value, model);
    }
  }
};
const __cssModules$26 = {};
var component$26 = normalizeComponent(script$26, render$26, staticRenderFns$26, false, injectStyles$26, null, null, null);
function injectStyles$26(context) {
  for (let o in __cssModules$26) {
    this[o] = __cssModules$26[o];
  }
}
component$26.options.__file = "../zero.Commerce/Plugin/pickers/shippingOption/picker.vue";
var ShippingOptionPicker = component$26.exports;
var render$25 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-shippinglines", class: {"is-disabled": _vm.disabled}}, [_vm._l(_vm.value, function(item2) {
    return _c("div", {staticClass: "shop-order-shippinglines-item"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: item2.key, expression: "item.key"}], staticClass: "ui-input -key", attrs: {type: "text", maxlength: _vm.maxLength, readonly: _vm.disabled, placeholder: "Key", size: "5"}, domProps: {value: item2.key}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "key", $event.target.value);
    }}}), _c("input", {directives: [{name: "model", rawName: "v-model", value: item2.value, expression: "item.value"}], staticClass: "ui-input -value", attrs: {type: "text", maxlength: _vm.maxLength, readonly: _vm.disabled, placeholder: "Value", size: "5"}, domProps: {value: item2.value}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "value", $event.target.value);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {type: "light", icon: "fth-x"}, on: {click: function($event) {
      return _vm.removeItem(item2);
    }}}) : _vm._e()], 1);
  }), !_vm.disabled && !_vm.plusButton ? _c("ui-button", {attrs: {type: "light", label: _vm.addLabel}, on: {click: _vm.addItem}}) : _vm._e(), !_vm.disabled && _vm.plusButton ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.items.length > 0 ? null : _vm.addLabel}, on: {click: _vm.addItem}}) : _vm._e()], 2);
};
var staticRenderFns$25 = [];
var shippingLines_vue_vue_type_style_index_0_lang = ".shop-order-shippinglines-item {\n  display: grid;\n  grid-template-columns: 1fr 1fr auto;\n  background: var(--color-input);\n  border-radius: var(--radius);\n}\n.shop-order-shippinglines-item + .shop-order-shippinglines-item, .shop-order-shippinglines-item + .ui-button, .shop-order-shippinglines-item + .ui-select-button {\n  margin-top: 6px;\n}\n.shop-order-shippinglines.is-disabled .shop-order-shippinglines-item {\n  background: transparent;\n}\n.shop-order-shippinglines-item .ui-icon-button {\n  height: 48px;\n  width: 48px;\n  border-left: none;\n  background: transparent !important;\n  box-shadow: none;\n}";
const script$25 = {
  name: "shopOrderShippingLines",
  props: {
    addLabel: {
      type: String,
      default: "@ui.add"
    },
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    maxItems: {
      type: Number,
      default: 100
    },
    maxLength: {
      type: Number,
      default: 200
    },
    plusButton: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    items: []
  }),
  methods: {
    addItem() {
      this.value.push({
        key: null,
        value: null
      });
      this.$nextTick(() => {
        this.$el.querySelector(".shop-order-shippinglines-item:last-of-type input").focus();
      });
    },
    removeItem(item2) {
      const index = this.value.indexOf(item2);
      this.value.splice(index, 1);
    }
  }
};
const __cssModules$25 = {};
var component$25 = normalizeComponent(script$25, render$25, staticRenderFns$25, false, injectStyles$25, null, null, null);
function injectStyles$25(context) {
  for (let o in __cssModules$25) {
    this[o] = __cssModules$25[o];
  }
}
component$25.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/shipping-lines.vue";
var ShippingLines = component$25.exports;
const editor$H = new Editor("commerce.order-shipping", "@shop.order.shipping.");
editor$H.field("shippingOptionId").custom(ShippingOptionPicker);
editor$H.fieldset((set) => {
  set.field("name").cols(8).text(120).required();
  set.field("price").custom(UiCurrency).required();
});
editor$H.field("lines").custom(ShippingLines);
var render$24 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-items shop-order-delivery-products ui-table is-inline"}, [_c("header", {staticClass: "ui-table-row ui-table-head"}, [_c("div", {staticClass: "ui-table-cell is-image"}), _c("div", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.dispatch.products.name", expression: "'@shop.order.dispatch.products.name'"}], staticClass: "ui-table-cell"}), _c("div", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.dispatch.products.input", expression: "'@shop.order.dispatch.products.input'"}], staticClass: "ui-table-cell is-input"}), _c("div", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.dispatch.products.sent", expression: "'@shop.order.dispatch.products.sent'"}], staticClass: "ui-table-cell is-total"})]), _vm._l(_vm.items, function(item2) {
    return _c("div", {key: item2.product.id, staticClass: "shop-order-item ui-table-row"}, [_c("div", {staticClass: "ui-table-cell is-image"}, [item2.image ? _c("img", {staticClass: "ui-table-field-image", attrs: {src: item2.image}}) : _vm._e()]), _c("div", {staticClass: "ui-table-cell is-name"}, [_c("strong", [_vm._v(_vm._s(item2.product.name))]), _c("br"), _c("span", {staticClass: "-minor"}, [_vm._v(_vm._s(item2.product.description))])]), _c("div", {staticClass: "ui-table-cell is-input"}, [_c("input", {directives: [{name: "model", rawName: "v-model:number", value: item2.input, expression: "item.input", arg: "number"}], attrs: {type: "text", onClick: "this.select();", disabled: item2.sent >= item2.quantity}, domProps: {value: item2.input}, on: {input: [function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "input", $event.target.value);
    }, _vm.onChange]}})]), _c("div", {staticClass: "ui-table-cell is-total", class: {"is-warn": item2.sent + ~~item2.input > item2.quantity}}, [_c("span", {class: {"has-value": ~~item2.input > 0}}, [_vm._v(_vm._s(item2.sent + ~~item2.input))]), _vm._v(" / " + _vm._s(item2.quantity) + " ")])]);
  })], 2);
};
var staticRenderFns$24 = [];
var deliveryProducts_vue_vue_type_style_index_0_lang = ".shop-order-delivery-products .ui-table-head {\n  position: static;\n}\n.shop-order-delivery-products .ui-table-row.shop-order-item {\n  border-top: none;\n}\n.shop-order-delivery-products .ui-table-cell.is-image {\n  flex: 0 1 74px;\n  padding: 18px 20px 17px 20px;\n  justify-content: center;\n}\n.shop-order-delivery-products .ui-table-cell.is-image img {\n  max-height: 48px;\n  max-width: 40px;\n}\n.shop-order-delivery-products .ui-table-cell.is-input {\n  flex: 0 1 135px;\n  justify-content: center;\n}\n.shop-order-delivery-products .ui-table-cell.is-input input {\n  text-align: center;\n}\n.shop-order-delivery-products .ui-table-cell.is-total {\n  flex: 0 1 100px;\n  justify-content: center;\n  padding-right: 20px;\n}\n.shop-order-delivery-products .ui-table-cell.is-total.is-warn {\n  color: var(--color-accent-error);\n  font-weight: bold;\n}\n.shop-order-delivery-products .ui-table-cell.is-total .has-value {\n  font-weight: bold;\n}";
const script$24 = {
  props: {
    value: {
      type: Array,
      default: () => []
    },
    meta: Object,
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    items: []
  }),
  mounted() {
    let value = this.value || [];
    let deliveredProducts = {};
    this.meta.deliveries.forEach((delivery) => {
      delivery.items.forEach((item2) => {
        deliveredProducts[item2.id] = !deliveredProducts[item2.id] ? item2.quantity : deliveredProducts[item2.id] + item2.quantity;
      });
    });
    this.items = this.meta.products.map((x) => {
      let meta = this.meta.productMeta[x.productId + ":" + x.variantId];
      let delivered = deliveredProducts[x.id];
      let deliveredOwnItem = value.find((v) => v.id === x.id);
      let deliveredOwn = deliveredOwnItem != null ? deliveredOwnItem.quantity : 0;
      return {
        product: x,
        meta,
        image: meta && meta.imageId ? MediaApi.getImageSource(meta.imageId) : null,
        quantity: x.quantity,
        sent: delivered - deliveredOwn || 0,
        input: deliveredOwn || 0
      };
    });
  },
  methods: {
    onChange() {
      let items = [];
      this.items.forEach((x) => {
        let count = ~~x.input;
        if (count > 0) {
          items.push({id: x.product.id, quantity: count});
        }
      });
      this.$emit("input", items);
    }
  }
};
const __cssModules$24 = {};
var component$24 = normalizeComponent(script$24, render$24, staticRenderFns$24, false, injectStyles$24, null, null, null);
function injectStyles$24(context) {
  for (let o in __cssModules$24) {
    this[o] = __cssModules$24[o];
  }
}
component$24.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/delivery-products.vue";
var DeliveryProducts = component$24.exports;
const editor$G = new Editor("commerce.order-shipping-delivery", "@shop.order.dispatch.");
const general$4 = editor$G.tab("general", "@shop.order.dispatch.tabs.general");
const tracking = editor$G.tab("tracking", "@shop.order.dispatch.tabs.tracking", (x) => !!x.trackingNumber || !!x.trackingUrl ? 1 : 0, (x) => x.isPickup);
const parts = editor$G.tab("parts", "@shop.order.dispatch.tabs.parts", (x) => x.items.reduce((sum, item2) => sum += item2.quantity, 0));
general$4.field("shipmentDate").datePicker().required();
general$4.field("isPickup").toggle();
general$4.field("carrierName").when((x) => !x.isPickup).text(60);
tracking.field("trackingNumber").text(120);
tracking.field("trackingUrl").text(500);
parts.field("items").custom(DeliveryProducts);
const base$a = "commerceCustomers/";
var CustomersApi = {
  getById(id) {
    return axios.get(base$a + "getById", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getEmpty() {
    return axios.get(base$a + "getEmpty").then((res) => Promise.resolve(res.data));
  },
  getByQuery(query) {
    return axios.get(base$a + "getByQuery", {params: {query}}).then((res) => Promise.resolve(res.data));
  },
  getPreviews(ids) {
    return axios.get(base$a + "getPreviews", {params: {ids}}).then((res) => Promise.resolve(res.data));
  },
  getForPicker(search) {
    return axios.get(base$a + "getForPicker", {params: {search}}).then((res) => Promise.resolve(res.data));
  },
  save(model) {
    return axios.post(base$a + "save", model).then((res) => Promise.resolve(res.data));
  },
  delete(id) {
    return axios.delete(base$a + "delete", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  export() {
    return axios.get(base$a + "export", {params: {options: {includeShared: false}}, responseType: "blob"}).then((res) => {
      download(res);
    });
  }
};
var render$23 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-customerpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$23 = [];
const script$23 = {
  name: "uiCustomerpicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "customer",
      items: CustomersApi.getForPicker,
      previews: CustomersApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      search: {
        local: false
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$23 = {};
var component$23 = normalizeComponent(script$23, render$23, staticRenderFns$23, false, injectStyles$23, null, null, null);
function injectStyles$23(context) {
  for (let o in __cssModules$23) {
    this[o] = __cssModules$23[o];
  }
}
component$23.options.__file = "../zero.Commerce/Plugin/pickers/customer/picker.vue";
var CustomerPicker = component$23.exports;
const editor$F = new Editor("commerce.order-contact", "@shop.order.contact.");
editor$F.field("id").custom(CustomerPicker).onChange((id, opts) => {
  Overlay.confirm({
    title: "@shop.order.contact.override.title",
    text: "@shop.order.contact.override.text",
    autoclose: false
  }).then((overlayOpts) => {
    overlayOpts.state("loading");
    CustomersApi.getById(id).then((res) => {
      let customer2 = res.entity;
      let address = res.entity.addresses[0];
      opts.model.name = customer2.name;
      opts.model.email = customer2.email;
      opts.model.phoneNumber = customer2.phoneNumber;
      opts.model.number = customer2.no;
      if (address) {
        opts.model.gender = address.gender;
        opts.model.title = address.title;
        opts.model.vatNo = address.vatNo;
      }
      overlayOpts.state("success");
      overlayOpts.hide();
    });
  }).catch(() => {
    opts.model.id = opts.oldValue;
  });
});
editor$F.fieldset((set) => {
  set.field("name").cols(7).text(120).required();
  set.field("gender").cols(3).select([
    {value: "@gender.undisclosed", key: "undisclosed"},
    {value: "@gender.female", key: "female"},
    {value: "@gender.male", key: "male"},
    {value: "@gender.nonbinary", key: "nonBinary"}
  ]).required();
  set.field("title").text(15);
});
editor$F.fieldset((set) => {
  set.field("email").text(80).required();
  set.field("phoneNumber").text(40);
});
editor$F.fieldset((set) => {
  set.field("vatNo").text(30);
  set.field("number").text(30);
});
var render$22 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-channelpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$22 = [];
var picker_vue_vue_type_style_index_0_lang$2 = "\n.shop-channelpicker .ui-select-button-icon.is-image\n{\n  padding: 10px;\n}\n";
const script$22 = {
  name: "uiChannelpicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "channel",
      items: ChannelsApi.getForPicker,
      previews: ChannelsApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        iconAsImage: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$22 = {};
var component$22 = normalizeComponent(script$22, render$22, staticRenderFns$22, false, injectStyles$22, null, null, null);
function injectStyles$22(context) {
  for (let o in __cssModules$22) {
    this[o] = __cssModules$22[o];
  }
}
component$22.options.__file = "../zero.Commerce/Plugin/pickers/channel/picker.vue";
var ChannelPicker = component$22.exports;
var CurrenciesApi = __assign({}, collection$1("commerceCurrencies/"));
var render$21 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-currencypicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$21 = [];
const script$21 = {
  name: "uiCurrenciespicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "currency",
      items: CurrenciesApi.getForPicker,
      previews: CurrenciesApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$21 = {};
var component$21 = normalizeComponent(script$21, render$21, staticRenderFns$21, false, injectStyles$21, null, null, null);
function injectStyles$21(context) {
  for (let o in __cssModules$21) {
    this[o] = __cssModules$21[o];
  }
}
component$21.options.__file = "../zero.Commerce/Plugin/pickers/currency/picker.vue";
var CurrencyPicker = component$21.exports;
var render$20 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-languagepicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$20 = [];
const script$20 = {
  name: "uiLanguagepicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "language",
      items: LanguagesApi.getForPicker,
      previews: LanguagesApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$20 = {};
var component$20 = normalizeComponent(script$20, render$20, staticRenderFns$20, false, injectStyles$20, null, null, null);
function injectStyles$20(context) {
  for (let o in __cssModules$20) {
    this[o] = __cssModules$20[o];
  }
}
component$20.options.__file = "app/components/pickers/languagePicker/languagepicker.vue";
var LanguagePicker = component$20.exports;
const editor$E = new Editor("commerce.order-meta", "@shop.order.meta.");
editor$E.fieldset((set) => {
  set.field("number").cols(7).text(60).required().onChange((number, opts) => {
    opts.model.name = number;
  });
  set.field("createdDate").datePicker({time: true}).required();
});
editor$E.field("channelId").custom(ChannelPicker).required();
editor$E.field("currency.currencyId").custom(CurrencyPicker).required();
editor$E.field("languageId").custom(LanguagePicker).required();
const base$9 = "commerceCatalogues/";
var CataloguesApi = __assign(__assign({}, collection$1(base$9)), {
  saveSorting: async (ids) => await post$1(base$9 + "saveSorting", ids),
  move: async (id, destinationId) => await post$1(base$9 + "move", {id, destinationId}),
  getChildren: async (parent, active) => await get$1(base$9 + "getChildren", {params: {parent, active}})
});
var render$1$ = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "shop-cataloguepicker-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.config.closeLabel, parent: _vm.config.rootId}, on: {click: _vm.config.hide}})];
  }, proxy: true}])}, [_vm.opened ? _c("div", {staticClass: "ui-box shop-cataloguepicker-overlay-items"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems, parent: _vm.config.rootId}, on: {select: _vm.onSelect}})], 1) : _vm._e()]);
};
var staticRenderFns$1$ = [];
var overlay_vue_vue_type_style_index_0_lang$8 = '@charset "UTF-8";\n.shop-cataloguepicker-overlay content {\n  padding-top: 0;\n}\n.ui-box.shop-cataloguepicker-overlay-items {\n  margin: 0;\n  padding: 20px 0;\n}\n.ui-box.shop-cataloguepicker-overlay-items .ui-tree-item.is-selected, .ui-box.shop-cataloguepicker-overlay-items .ui-tree-item:hover:not(.is-disabled) {\n  background: var(--color-bg-xxlight);\n}\n.ui-box.shop-cataloguepicker-overlay-items + .ui-box {\n  margin-top: var(--padding);\n}\n.ui-box.shop-cataloguepicker-overlay-items .ui-tree-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n}\n.ui-box.shop-cataloguepicker-overlay-items .ui-tree-item.is-selected .ui-tree-item-text {\n  font-weight: bold;\n}';
const script$1$ = {
  props: {
    model: String,
    config: Object
  },
  data: () => ({
    opened: false
  }),
  computed: {
    disabledIds() {
      return this.config.disabledIds || [];
    }
  },
  mounted() {
    setTimeout(() => this.opened = true, 300);
  },
  methods: {
    onSelect(item2) {
      console.info("selected", item2);
      this.config.confirm(item2);
    },
    getItems(parent) {
      return CataloguesApi.getChildren(parent, this.model).then((res) => {
        res.forEach((item2) => {
          if (item2.id === this.model) {
            item2.isSelected = true;
          }
          if (this.disabledIds.indexOf(item2.id) > -1) {
            item2.disabled = true;
          }
          item2.hasActions = false;
        });
        return res;
      });
    }
  }
};
const __cssModules$1$ = {};
var component$1$ = normalizeComponent(script$1$, render$1$, staticRenderFns$1$, false, injectStyles$1$, null, null, null);
function injectStyles$1$(context) {
  for (let o in __cssModules$1$) {
    this[o] = __cssModules$1$[o];
  }
}
component$1$.options.__file = "../zero.Commerce/Plugin/pickers/catalogue/overlay.vue";
var CatalogueOverlay = component$1$.exports;
var render$1_ = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-cataloguepicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _vm.previews.length > 0 ? _c("div", {staticClass: "shop-cataloguepicker-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {staticClass: "shop-cataloguepicker-preview"}, [_c("ui-select-button", {attrs: {icon: preview.icon, label: preview.name, description: preview.text, disabled: _vm.disabled, tokens: {id: preview.id}}, on: {click: function($event) {
      return _vm.pick(preview.id);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: function($event) {
      return _vm.remove(preview.id);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.canAdd ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.limit > 1 ? "@ui.add" : "@ui.select", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}}) : _vm._e()], 1);
};
var staticRenderFns$1_ = [];
var picker_vue_vue_type_style_index_0_lang$1 = ".shop-cataloguepicker-preview {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.shop-cataloguepicker-preview .ui-icon-button {\n  height: 24px;\n  width: 24px;\n}\n.shop-cataloguepicker-preview .ui-icon-button i {\n  font-size: 13px;\n}\n.shop-cataloguepicker-previews + .ui-select-button,\n.shop-cataloguepicker-preview + .shop-cataloguepicker-preview {\n  margin-top: 10px;\n}";
const script$1_ = {
  name: "uiCataloguepicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    root: {
      type: String,
      default: null
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: []
  }),
  watch: {
    value() {
      this.updatePreviews();
    }
  },
  computed: {
    multiple() {
      return this.limit > 1;
    },
    canAdd() {
      let count = isArray(this.value) ? this.value.length : !this.value ? 0 : 1;
      return !this.disabled && count < this.limit;
    }
  },
  mounted() {
    this.updatePreviews();
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    updatePreviews() {
      if (!this.value || isEmpty(this.value)) {
        this.previews = [];
        return;
      }
      let shopText = Localization.localize("@shop.catalogue.picker.hierarchy_prefix");
      let ids = isArray(this.value) ? this.value : [this.value];
      CataloguesApi.getPreviews(ids).then((res) => {
        this.previews = res.map((item2) => {
          item2.text = item2.text ? shopText + " - " + item2.text : null;
          return item2;
        });
      });
    },
    remove(id) {
      if (isArray(this.value)) {
        let index = this.value.indexOf(id);
        this.value.splice(index, 1);
        this.onChange(this.value);
      } else {
        this.onChange(this.limit > 1 ? [] : null);
      }
    },
    pick(id) {
      if (this.disabled) {
        return;
      }
      let disabledIds = [];
      if (!!this.value && !isArray(this.value)) {
        disabledIds = [this.value];
      } else if (isArray(this.value)) {
        disabledIds = this.value;
      }
      let options2 = extend({
        title: "@shop.catalogue.picker.headline",
        closeLabel: "@ui.close",
        component: CatalogueOverlay,
        display: "editor",
        model: this.multiple ? id : this.value,
        rootId: this.root,
        disabledIds
      }, typeof this.options === "object" ? this.options : {});
      return Overlay.open(options2).then((value) => {
        if (this.multiple) {
          if (!this.value || !isArray(this.value)) {
            this.onChange([value.id]);
          } else if (this.value.indexOf(value.id) < 0) {
            if (id) {
              this.remove(id);
            }
            this.value.push(value.id);
            this.onChange(this.value);
          }
        } else {
          this.onChange(value ? value.id : null);
        }
      });
    }
  }
};
const __cssModules$1_ = {};
var component$1_ = normalizeComponent(script$1_, render$1_, staticRenderFns$1_, false, injectStyles$1_, null, null, null);
function injectStyles$1_(context) {
  for (let o in __cssModules$1_) {
    this[o] = __cssModules$1_[o];
  }
}
component$1_.options.__file = "../zero.Commerce/Plugin/pickers/catalogue/picker.vue";
var CataloguePicker$1 = component$1_.exports;
var render$1Z = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("catalogue-picker", {attrs: {value: _vm.value}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}});
};
var staticRenderFns$1Z = [];
const script$1Z = {
  props: {
    value: {
      type: [String, Array]
    },
    entity: {
      type: Object,
      required: true
    },
    config: Object
  },
  components: {CataloguePicker: CataloguePicker$1}
};
const __cssModules$1Z = {};
var component$1Z = normalizeComponent(script$1Z, render$1Z, staticRenderFns$1Z, false, injectStyles$1Z, null, null, null);
function injectStyles$1Z(context) {
  for (let o in __cssModules$1Z) {
    this[o] = __cssModules$1Z[o];
  }
}
component$1Z.options.__file = "../zero.Commerce/Plugin/editor/cataloguepicker.vue";
var CataloguePicker = component$1Z.exports;
const base$8 = "commerceManufacturers/";
var ManufacturersApi = {
  getById(id) {
    return axios.get(base$8 + "getById", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getEmpty(id) {
    return axios.get(base$8 + "getEmpty").then((res) => Promise.resolve(res.data));
  },
  getByQuery(query) {
    return axios.get(base$8 + "getByQuery", {params: {query}}).then((res) => Promise.resolve(res.data));
  },
  getPreviews(ids) {
    return axios.get(base$8 + "getPreviews", {params: {ids}}).then((res) => Promise.resolve(res.data));
  },
  getForPicker() {
    return axios.get(base$8 + "getForPicker").then((res) => Promise.resolve(res.data));
  },
  save(model) {
    return axios.post(base$8 + "save", model).then((res) => Promise.resolve(res.data));
  },
  delete(id) {
    return axios.delete(base$8 + "delete", {params: {id}}).then((res) => Promise.resolve(res.data));
  }
};
var render$1Y = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-manufacturerpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$1Y = [];
const script$1Y = {
  name: "shopManufacturerpicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "manufacturer",
      items: ManufacturersApi.getForPicker,
      previews: ManufacturersApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true,
        iconAsImage: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1Y = {};
var component$1Y = normalizeComponent(script$1Y, render$1Y, staticRenderFns$1Y, false, injectStyles$1Y, null, null, null);
function injectStyles$1Y(context) {
  for (let o in __cssModules$1Y) {
    this[o] = __cssModules$1Y[o];
  }
}
component$1Y.options.__file = "../zero.Commerce/Plugin/pickers/manufacturer/picker.vue";
var ManufacturerPicker = component$1Y.exports;
var PropertiesApi = __assign(__assign({}, collection$1("commerceProperties/")), {
  getForPickerWithConfig: async (config2) => await get$1("commerceProperties/getForPickerWithConfig", __assign({}, config2))
});
var render$1X = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "shop-productpicker-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.config.closeLabel}, on: {click: _vm.config.hide}}), _vm.preview ? _c("ui-button", {attrs: {type: "primary", label: "@ui.confirm"}, on: {click: _vm.onConfirm}}) : _vm._e()];
  }, proxy: true}])}, [_vm.opened ? _c("div", [!_vm.global ? _c("div", {staticClass: "ui-box"}, [_c("ui-property", {attrs: {vertical: true}}, [_c("channel-picker", {attrs: {options: {preview: {delete: false, iconAsImage: true}}, disabled: _vm.channelPickerDisabled}, model: {value: _vm.selected.channelId, callback: function($$v) {
    _vm.$set(_vm.selected, "channelId", $$v);
  }, expression: "selected.channelId"}})], 1), _vm.preview ? _c("div", [_c("hr"), _c("ui-property", {attrs: {vertical: true}}, [_c("ui-select-button", {attrs: {icon: "fth-shopping-bag", label: _vm.preview.name, disabled: true}})], 1)], 1) : _vm._e()], 1) : _vm._e(), !_vm.preview ? _c("div", {staticClass: "ui-box"}, [_c("ui-property", {attrs: {vertical: true}}, [_c("ui-search", {model: {value: _vm.search, callback: function($$v) {
    _vm.search = $$v;
  }, expression: "search"}})], 1), _c("ui-property", {attrs: {vertical: true}}, [_c("div", {staticClass: "ui-list shop-productpicker-overlay-items"}, _vm._l(_vm.list.items, function(item2) {
    return _c("button", {key: item2.id, staticClass: "ui-list-item", class: {"is-selected": item2.id === _vm.selected.id}, attrs: {type: "button"}, on: {click: function($event) {
      return _vm.onSelect(item2);
    }}}, [_c("p", {staticClass: "ui-list-item-content"}, [_c("span", {staticClass: "ui-list-item-text"}, [_vm._v(_vm._s(item2.name))]), _c("span", {staticClass: "ui-list-item-description"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: item2.isActive ? "@ui.active" : "@ui.inactive", expression: "item.isActive ? '@ui.active' : '@ui.inactive'"}]}), _vm._v(", "), _c("span", {directives: [{name: "date", rawName: "v-date", value: item2.createdDate, expression: "item.createdDate"}]})])]), item2.image && item2.id !== _vm.selected.id ? _c("img", {staticClass: "ui-list-item-image", attrs: {src: item2.image}}) : _vm._e(), item2.id === _vm.selected.id ? _c("ui-icon", {staticClass: "ui-list-item-selected-icon", attrs: {symbol: "fth-check-circle", size: 16}}) : _vm._e()], 1);
  }), 0), _c("ui-pagination", {attrs: {pages: _vm.list.totalPages, page: _vm.page, inline: true}, on: {change: _vm.setPage}})], 1)], 1) : _vm._e(), _vm.preview ? _c("div", {staticClass: "ui-box"}, _vm._l(_vm.options.groups, function(group, index) {
    return _c("ui-property", {key: group.id, attrs: {label: group.name}}, [_c("div", {staticClass: "ui-native-select"}, [_c("select", {directives: [{name: "model", rawName: "v-model", value: group.selected, expression: "group.selected"}], on: {change: function($event) {
      var $$selectedVal = Array.prototype.filter.call($event.target.options, function(o) {
        return o.selected;
      }).map(function(o) {
        var val = "_value" in o ? o._value : o.value;
        return val;
      });
      _vm.$set(group, "selected", $event.target.multiple ? $$selectedVal : $$selectedVal[0]);
    }}}, _vm._l(group.options, function(option) {
      return _c("option", {key: option.id, domProps: {value: option.id}}, [_vm._v(_vm._s(option.name + (option.text ? " (" + option.text + ")" : "")))]);
    }), 0)])]);
  }), 1) : _vm._e()]) : _vm._e()]);
};
var staticRenderFns$1X = [];
var overlay_vue_vue_type_style_index_0_lang$7 = '.shop-productpicker-overlay content {\n  padding-top: 0;\n}\n.shop-productpicker-overlay-items {\n  margin: -30px -32px 0;\n}\n.shop-productpicker-overlay-items .ui-list-item {\n  padding-top: 20px;\n  padding-bottom: 20px;\n}\n.shop-productpicker-overlay-items .ui-list-item-image {\n  mix-blend-mode: multiply;\n}\n\n/*.ui-box.shop-productpicker-overlay-items\n{\n  margin: 0;\n  padding: 20px 0;\n\n  .ui-tree-item.is-selected, .ui-tree-item:hover:not(.is-disabled)\n  {\n    background: var(--color-bg-xxlight);\n  }\n\n  & +.ui-box\n  {\n    margin-top: var(--padding);\n  }\n\n  .ui-tree-item.is-selected\n  {\n    &:after\n    {\n      font-family: "Feather";\n      content: "\\e83e";\n      font-size: 16px;\n      color: var(--color-primary);\n    }\n\n    .ui-tree-item-text\n    {\n      font-weight: bold;\n    }\n  }\n}*/';
const script$1X = {
  components: {ChannelPicker},
  props: {
    model: String,
    config: Object
  },
  data: () => ({
    opened: false,
    search: null,
    preview: null,
    selected: {
      id: null,
      variantId: null
    },
    page: 1,
    list: {
      totalPages: 1,
      items: []
    },
    variants: [],
    channelPickerDisabled: false,
    global: false
  }),
  computed: {
    disabledIds() {
      return this.config.disabledIds || [];
    }
  },
  watch: {
    "selected.channelId": function() {
      this.onChannelChange();
    },
    search() {
      this.page = 1;
      this.debouncedLoad();
    }
  },
  created() {
    this.debouncedLoad = debounce(this.load, 300);
  },
  mounted() {
    this.channelPickerDisabled = this.config.channelPickerDisabled;
    this.global = this.config.global;
    if (!this.global) {
      ChannelsApi.getByIdOrDefault(this.config.channelId).then((res) => {
        this.selected.channelId = res.entity.id;
        this.opened = true;
        this.search = null;
      });
    } else {
      this.opened = true;
      this.search = null;
      this.load();
    }
  },
  methods: {
    onChannelChange() {
      this.load();
    },
    load() {
      let channelId = this.global ? null : this.selected.channelId;
      ProductsApi.getByQuery(channelId, null, {
        search: this.search,
        page: this.page,
        pageSize: 10
      }).then((res) => {
        res.items.forEach((item2) => {
          item2.image = item2.image ? MediaApi.getImageSource(item2.image) : null;
        });
        this.list = res;
      });
    },
    async loadVariants() {
      return await ProductsApi.getOptions(this.selected.id);
    },
    setPage(index) {
      this.page = index;
      this.load();
    },
    async onSelect(item2) {
      this.selected.id = item2.id;
      this.options = await this.loadVariants();
      this.options.groups.forEach((group) => {
        group.selected = group.options.length === 1 ? group.options[0].id : null;
      });
      this.preview = item2;
    },
    onConfirm() {
      let groups = [];
      this.options.groups.forEach((group) => {
        if (!group.selected)
          ;
        groups.push(group.selected);
      });
      this.selected.variantId = this.options.map[groups.join("-")];
      this.config.confirm(this.selected);
    }
  }
};
const __cssModules$1X = {};
var component$1X = normalizeComponent(script$1X, render$1X, staticRenderFns$1X, false, injectStyles$1X, null, null, null);
function injectStyles$1X(context) {
  for (let o in __cssModules$1X) {
    this[o] = __cssModules$1X[o];
  }
}
component$1X.options.__file = "../zero.Commerce/Plugin/pickers/product/overlay.vue";
var ProductPickerOverlay = component$1X.exports;
var render$1W = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-productpicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _vm.previews.length > 0 ? _c("div", {staticClass: "shop-productpicker-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {staticClass: "shop-productpicker-preview"}, [_c("ui-select-button", {attrs: {icon: preview.icon, label: preview.name, description: preview.text, disabled: _vm.disabled, tokens: {id: preview.id}}, on: {click: function($event) {
      return _vm.pick(preview.id);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: function($event) {
      return _vm.remove(preview.id);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.canAdd ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.limit > 1 ? "@ui.add" : "@ui.select", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}}) : _vm._e()], 1);
};
var staticRenderFns$1W = [];
var picker_vue_vue_type_style_index_0_lang = ".shop-productpicker-preview {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.shop-productpicker-preview .ui-icon-button {\n  height: 24px;\n  width: 24px;\n}\n.shop-productpicker-preview .ui-icon-button i {\n  font-size: 13px;\n}\n.shop-productpicker-previews + .ui-select-button,\n.shop-productpicker-preview + .shop-productpicker-preview {\n  margin-top: 10px;\n}";
const script$1W = {
  name: "uiProductpicker",
  props: {
    value: {
      type: [Object, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    root: {
      type: String,
      default: null
    },
    global: {
      type: Boolean,
      default: false
    },
    channel: {
      type: String,
      default: null
    },
    channelPickerDisabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: []
  }),
  watch: {
    value: {
      deep: true,
      handler() {
        this.updatePreviews();
      }
    }
  },
  computed: {
    multiple() {
      return this.limit > 1;
    },
    canAdd() {
      let count = isArray(this.value) ? this.value.length : !this.value ? 0 : 1;
      return !this.disabled && count < this.limit;
    }
  },
  mounted() {
    this.updatePreviews();
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    updatePreviews() {
      if (!this.value || isEmpty(this.value)) {
        this.previews = [];
        return;
      }
      let ids = [];
      let variants2 = [];
      let selected = isArray(this.value) ? this.value : [this.value];
      selected.forEach((item2) => {
        if (item2.id) {
          ids.push(item2.id);
          variants2.push(item2.variantId);
        }
      });
      if (ids.length < 1) {
        this.previews = [];
        return;
      }
      ProductsApi.getPreviews(ids, variants2).then((res) => {
        this.previews = res;
      });
    },
    remove(id) {
      if (isArray(this.value)) {
        let index = this.value.indexOf(id);
        this.value.splice(index, 1);
        this.onChange(this.value);
      } else {
        this.onChange(this.limit > 1 ? [] : null);
      }
    },
    pick(id) {
      if (this.disabled) {
        return;
      }
      let options2 = extend({
        title: "@shop.product.picker.headline",
        closeLabel: "@ui.close",
        component: ProductPickerOverlay,
        display: "editor",
        global: this.global,
        channelId: this.channel,
        channelPickerDisabled: this.channelPickerDisabled
      }, typeof this.options === "object" ? this.options : {});
      return Overlay.open(options2).then((value) => {
        if (this.multiple) {
          if (!this.value || !isArray(this.value)) {
            this.onChange([value]);
          } else if (!this.value.find((x) => x.id === value.id)) {
            if (id) {
              this.remove(id);
            }
            this.value.push(value);
            this.onChange(this.value);
          }
        } else {
          this.onChange(value);
        }
      });
    }
  }
};
const __cssModules$1W = {};
var component$1W = normalizeComponent(script$1W, render$1W, staticRenderFns$1W, false, injectStyles$1W, null, null, null);
function injectStyles$1W(context) {
  for (let o in __cssModules$1W) {
    this[o] = __cssModules$1W[o];
  }
}
component$1W.options.__file = "../zero.Commerce/Plugin/pickers/product/picker.vue";
var ProductPicker = component$1W.exports;
var TaxesApi = __assign({}, collection$1("commerceTaxes/"));
var render$1V = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-taxpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$1V = [];
const script$1V = {
  name: "shopTaxespicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "tax",
      items: TaxesApi.getForPicker,
      previews: TaxesApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1V = {};
var component$1V = normalizeComponent(script$1V, render$1V, staticRenderFns$1V, false, injectStyles$1V, null, null, null);
function injectStyles$1V(context) {
  for (let o in __cssModules$1V) {
    this[o] = __cssModules$1V[o];
  }
}
component$1V.options.__file = "../zero.Commerce/Plugin/pickers/tax/picker.vue";
var TaxPicker = component$1V.exports;
var PropertyValuesApi = __assign(__assign({}, collection$1("commercePropertyValues/")), {
  getForIdByQuery: async (propertyId, query, config2) => await get$1("commercePropertyValues/getForIdByQuery", __assign(__assign({}, config2), {params: {propertyId, query}})),
  getForList: async (propertyId, query, config2) => await get$1("commercePropertyValues/getForList", __assign(__assign({}, config2), {params: {propertyId, query}})),
  getNameByIds: async (ids, config2) => await get$1("commercePropertyValues/getNameByIds", __assign(__assign({}, config2), {params: {ids}}))
});
var render$1U = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-propertypicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {ref: "picker", attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onInput, change: _vm.onChange}})], 1);
};
var staticRenderFns$1U = [];
const script$1U = {
  name: "uiPropertypicker",
  props: {
    property: {
      type: Object,
      default: null
    },
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    },
    hideTitle: {
      type: Boolean,
      default: false
    },
    autoopen: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.init();
  },
  methods: {
    init() {
      let property = this.property;
      let hideTitle = this.hideTitle;
      this.pickerConfig = extend({
        paging: true,
        items: PropertyValuesApi.getForIdByQuery.bind(this, property.id),
        previews: PropertyValuesApi.getPreviews,
        scope: "propertyvalues",
        limit: property.limit,
        multiple: true,
        autocomplete: property.autocomplete,
        autoOpen: this.autoopen,
        closeOnClick: property.limit < 2,
        preview: {
          combined: true,
          combinedTitle: hideTitle ? null : property.name
        },
        search: {
          local: false
        }
      }, this.options);
    },
    open() {
      this.$refs.picker.pick();
    },
    onChange(value) {
      this.$emit("change", value);
    },
    onInput(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1U = {};
var component$1U = normalizeComponent(script$1U, render$1U, staticRenderFns$1U, false, injectStyles$1U, null, null, null);
function injectStyles$1U(context) {
  for (let o in __cssModules$1U) {
    this[o] = __cssModules$1U[o];
  }
}
component$1U.options.__file = "../zero.Commerce/Plugin/pickers/property/picker.vue";
var Picker = component$1U.exports;
var render$1T = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-propertiespicker", class: {"is-disabled": _vm.disabled}}, [_vm.items.length > 0 ? _c("div", {staticClass: "shop-propertiespicker-items"}, _vm._l(_vm.items, function(item2) {
    return _c("div", {key: item2.id, staticClass: "shop-propertiespicker-item"}, [_c("picker", {ref: "picker-" + item2.id, refInFor: true, attrs: {property: item2.property, autoopen: item2.autoopen, disabled: _vm.disabled || !item2.hasValues}, on: {change: item2.onChange}, model: {value: item2.values, callback: function($$v) {
      _vm.$set(item2, "values", $$v);
    }, expression: "item.values"}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x"}, on: {click: function($event) {
      return _vm.removeProperty(item2.id);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.properties.length ? _c("ui-pick", {attrs: {config: _vm.pickerConfig, disabled: _vm.disabled}, on: {select: _vm.onPropertySelected, deselect: _vm.onPropertyDeselected}, model: {value: _vm.selectedIds, callback: function($$v) {
    _vm.selectedIds = $$v;
  }, expression: "selectedIds"}}) : _vm._e()], 1);
};
var staticRenderFns$1T = [];
var multipicker_vue_vue_type_style_index_0_lang = ".shop-propertiespicker-item + .shop-propertiespicker-item,\n.shop-propertiespicker-items + .ui-pick {\n  margin-top: 10px;\n}\n.shop-propertiespicker-item {\n  display: flex;\n  width: 100%;\n  align-items: center;\n}\n.shop-propertiespicker-item > .shop-propertypicker {\n  flex-grow: 1;\n}\n.shop-propertiespicker-items .ui-icon-button {\n  height: 24px;\n  width: 24px;\n}\n.shop-propertiespicker-items .ui-icon-button .ui-button-icon {\n  font-size: 13px;\n}";
const script$1T = {
  name: "uiPropertiespicker",
  components: {Picker},
  props: {
    value: {
      type: Array,
      default: () => []
    },
    limit: {
      type: Number,
      default: 10
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  watch: {
    value() {
      this.reload();
    }
  },
  data: () => ({
    properties: [],
    items: [],
    selectedIds: [],
    pickerConfig: {},
    loaded: false
  }),
  created() {
    PropertiesApi.getForPickerWithConfig().then((properties) => {
      this.properties = properties;
      this.pickerConfig = extend({
        scope: "properties",
        items: properties,
        limit: this.limit,
        addButton: {
          label: this.limit > 1 ? "@ui.add" : "@ui.select"
        },
        multiple: true,
        search: {
          enabled: false
        },
        preview: {
          enabled: false
        }
      }, this.options);
      this.loaded = true;
      this.reload();
    });
  },
  methods: {
    reload() {
      if (this.loaded && this.value && this.value.length) {
        this.selectedIds = [];
        this.items = [];
        this.value.forEach((group) => {
          this.selectedIds.push(group.id);
          let property = find(this.properties, (p2) => p2.id === group.id);
          this.onPropertySelected(group.id, property, group.valueIds, true);
        });
      }
    },
    onPropertySelected(id, item2, values2, autoAdd) {
      const hasValues = item2.type !== "boolean";
      this.items.push({
        id,
        values: values2 || [],
        property: item2,
        onChange: (values3) => {
          this.onValuesChange(item2, values3);
        },
        autoopen: !autoAdd && hasValues,
        hasValues
      });
      if (autoAdd !== true && item2.type === "boolean") {
        this.onValuesChange();
      }
    },
    onPropertyDeselected(id, index) {
      this.items.splice(index, 1);
      this.onValuesChange();
    },
    removeProperty(id) {
      let index = this.selectedIds.indexOf(id);
      this.selectedIds.splice(index, 1);
      this.items.splice(index, 1);
      this.onValuesChange();
    },
    onValuesChange(item2, values2) {
      let result = [];
      this.items.forEach((item3) => {
        if (!item3.hasValues || item3.values && item3.values.length) {
          result.push({
            id: item3.id,
            values: !isArray(item3.values) ? [item3.values] : item3.values
          });
        }
      });
      this.$emit("input", result);
    }
  }
};
const __cssModules$1T = {};
var component$1T = normalizeComponent(script$1T, render$1T, staticRenderFns$1T, false, injectStyles$1T, null, null, null);
function injectStyles$1T(context) {
  for (let o in __cssModules$1T) {
    this[o] = __cssModules$1T[o];
  }
}
component$1T.options.__file = "../zero.Commerce/Plugin/pickers/property/multipicker.vue";
var PropertyMultipicker = component$1T.exports;
function createProductVariantsList() {
  const list2 = new List("commerce.products-variants");
  list2.templateLabel = (x) => "@shop.product.fields.variant." + x;
  list2.query.pageSize = 20;
  list2.column("imageIds", {hideLabel: true, width: 40}).image();
  list2.column("name", {width: 600}).text();
  list2.column("sku").text();
  list2.column("price").text();
  return list2;
}
var render$1S = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-variants"}, [_vm.list ? _c("ui-table", {ref: "table", attrs: {config: _vm.list, inline: true}, scopedSlots: _vm._u([{key: "top", fn: function() {
    return [_c("div", {staticClass: "ui-table-row"}, [_c("div", {staticClass: "ui-table-cell shop-variants-top"}, [_c("div", [_c("ui-button", {attrs: {type: "light", icon: "fth-plus", label: "Add variant"}}), _c("ui-button", {attrs: {type: "light", label: "Filter"}})], 1), _vm.$refs.table ? _c("ui-search", {model: {value: _vm.search, callback: function($$v) {
      _vm.search = $$v;
    }, expression: "search"}}) : _vm._e()], 1)])];
  }, proxy: true}], null, false, 1992987196)}) : _vm._e()], 1);
};
var staticRenderFns$1S = [];
var variants_vue_vue_type_style_index_0_lang = ".shop-variants {\n  position: relative;\n}\n.shop-variants-intro {\n  display: grid;\n  grid-template-columns: 1fr 1px 1fr;\n  gap: 32px;\n}\n.shop-variants-intro > div {\n  text-align: center;\n}\n.shop-variants-intro > .-line {\n  height: 100%;\n  background: var(--color-line);\n}\n.shop-variants-intro-item {\n  color: var(--color-text);\n  font-size: var(--font-size);\n  display: inline-grid;\n  grid-template-columns: auto 1fr;\n  gap: 25px;\n  align-items: center;\n}\n.shop-variants-intro-item-icon {\n  width: 70px;\n  height: 70px;\n  line-height: 68px !important;\n  font-size: 20px;\n  text-align: center;\n  background: var(--color-button-light);\n  border-radius: var(--radius);\n}\n.shop-variants-intro-item-text {\n  line-height: 1.3;\n  color: var(--color-text-dim);\n}\n.shop-variants-intro-item-text strong {\n  display: inline-block;\n  margin-bottom: 5px;\n  color: var(--color-text);\n}\n.shop-variants-top.ui-table-cell {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  grid-gap: 20px;\n  padding-top: 16px;\n  padding-bottom: 15px;\n}\n.shop-variants .ui-table-field-image {\n  max-height: 30px;\n  max-width: 40px;\n}";
const script$1S = {
  name: "shopVariants",
  props: {
    value: {
      type: Array,
      default: () => {
        return [];
      }
    },
    entity: {
      type: Object,
      required: true
    }
  },
  watch: {
    value: {
      deep: true,
      handler() {
        this.rebuild();
      }
    },
    search() {
      this.debouncedSearch();
    },
    $route(to, from) {
      this.rebuild();
    }
  },
  components: {},
  data: () => ({
    search: null,
    debouncedSearch: null,
    items: [],
    list: null,
    valueMap: []
  }),
  created() {
    this.debouncedSearch = debounce(() => this.$refs.table.query.search = this.search, 500, false);
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.items = JSON.parse(JSON.stringify(this.value || []));
      var valueIds = [];
      this.entity.variantGroups.forEach((group) => {
        group.valueIds.forEach((id) => {
          valueIds.push(id);
        });
      });
      let search = (text2, searchString) => {
        if (!text2) {
          return false;
        }
        const regexStr = "(?=.*" + searchString.split(/\,|\s/).join(")(?=.*") + ")";
        const searchRegEx = new RegExp(regexStr, "gi");
        return text2.match(searchRegEx) !== null;
      };
      PropertyValuesApi.getNameByIds(valueIds).then((res) => {
        this.valueMap = res;
        this.items.forEach((item2) => {
          item2.image = MediaApi.getImageSource(item2.imageIds[0]);
          item2.name = this.getName(item2);
        });
        this.list = createProductVariantsList();
        this.list.onFetch((filter2) => {
          let items = this.items;
          if (filter2.search) {
            items = this.items.filter((item2) => search(item2.name, filter2.search) || search(item2.sku, filter2.search) || search(item2.gtin, filter2.search));
          }
          const start = (filter2.page - 1) * filter2.pageSize;
          const end = start + filter2.pageSize;
          return Promise.resolve({
            items: items.slice(start, end),
            page: filter2.page,
            pageSize: filter2.pageSize,
            totalPages: Math.ceil(items.length / filter2.pageSize)
          });
        });
        if (this.$refs.table) {
          this.$refs.table.setup();
        }
      });
    },
    getName(variant) {
      let nameParts = [];
      var groups = this.entity.variantGroups.filter((x) => variant.groupIds.indexOf(x.id) > -1).sort((a, b) => a.level > b.level ? 1 : -1);
      groups.forEach((group) => {
        if (group.altValue) {
          nameParts.push(group.altValue);
        } else {
          let parts2 = group.valueIds.map((id) => this.valueMap[id]);
          nameParts.push(parts2.join("/"));
        }
      });
      return nameParts.join(" \u2013 ");
    }
  }
};
const __cssModules$1S = {};
var component$1S = normalizeComponent(script$1S, render$1S, staticRenderFns$1S, false, injectStyles$1S, null, null, null);
function injectStyles$1S(context) {
  for (let o in __cssModules$1S) {
    this[o] = __cssModules$1S[o];
  }
}
component$1S.options.__file = "../zero.Commerce/Plugin/pages/catalogue-old/products/variants-new/variants.vue";
var Variants = component$1S.exports;
var render$1R = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-product-giftvalues", class: {"is-disabled": _vm.disabled}}, [_c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated, handle: ".is-handle", enabled: !_vm.disabled}, expression: "{ onUpdate: onSortingUpdated, handle: '.is-handle', enabled: !disabled }"}], staticClass: "shop-product-giftvalues-sortable"}, _vm._l(_vm.items, function(item2) {
    return _c("div", {staticClass: "shop-product-giftvalues-item"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: item2.sku, expression: "item.sku"}], staticClass: "ui-input", attrs: {type: "text", readonly: _vm.disabled, size: "5", placeholder: "SKU"}, domProps: {value: item2.sku}, on: {input: [function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "sku", $event.target.value);
    }, _vm.onChange]}}), _c("input", {staticClass: "ui-input", attrs: {type: "text", readonly: _vm.disabled, size: "5", placeholder: "Price"}, domProps: {value: item2.price}, on: {input: function($event) {
      return _vm.onPriceChange(item2, $event.target.value);
    }}}), _c("ui-icon-button", {staticClass: "is-handle", attrs: {type: "light", icon: "fth-waffle", disabled: _vm.disabled}}), _c("ui-icon-button", {attrs: {type: "light", icon: "fth-x", disabled: _vm.disabled}, on: {click: function($event) {
      return _vm.removeItem(item2);
    }}})], 1);
  }), 0), !_vm.disabled ? _c("ui-button", {attrs: {type: "light", label: "@ui.add"}, on: {click: _vm.addItem}}) : _vm._e()], 1);
};
var staticRenderFns$1R = [];
var productGiftvalues_vue_vue_type_style_index_0_lang = ".shop-product-giftvalues-item {\n  display: grid;\n  grid-template-columns: 1fr 1fr auto auto;\n  background: var(--color-input);\n  border-radius: var(--radius);\n  margin-bottom: 6px;\n}\n.shop-product-giftvalues.is-disabled .shop-product-giftvalues-item {\n  background: transparent;\n}\n.shop-product-giftvalues-item .ui-input {\n  border-radius: var(--radius) 0 0 var(--radius);\n}\n.shop-product-giftvalues-item .ui-input:last-of-type {\n  border-left-color: transparent;\n  border-radius: 0 var(--radius) var(--radius) 0;\n}\n.shop-product-giftvalues-item .ui-icon-button {\n  border-radius: 0 var(--radius) var(--radius) 0;\n  height: 48px;\n  width: 48px;\n  border-left: none;\n  background: transparent !important;\n  box-shadow: none;\n}\n.shop-product-giftvalues-item .ui-icon-button + .ui-icon-button {\n  margin-left: 0;\n}";
const script$1R = {
  name: "shopProductGiftValues",
  props: {
    value: {
      type: Array,
      default: () => []
    },
    entity: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    loaded: false,
    items: [],
    properties: []
  }),
  watch: {
    value(value) {
      this.build();
    }
  },
  mounted() {
    this.build();
  },
  methods: {
    build() {
      this.items = this.value ? this.value.slice() : [];
    },
    onChange() {
      this.$emit("input", this.items);
    },
    onPriceChange(item2, value) {
      var parsedValue = parseFloat(value);
      if (isNaN(parsedValue)) {
        return;
      }
      item2.price = parsedValue;
      this.onChange();
    },
    addItem() {
      this.items.push({
        id: null,
        isActive: true,
        name: "",
        sort: 0,
        sku: null,
        manufacturerSku: null,
        gtin: null,
        stock: 0,
        price: null,
        imageIds: [],
        groupIds: []
      });
      this.onChange();
    },
    removeItem(item2) {
      const index = this.items.indexOf(item2);
      this.items.splice(index, 1);
      this.onChange();
    },
    onSortingUpdated(ev) {
      let sort = 0;
      this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
      this.items.forEach((x) => {
        x.sort = sort++;
      });
      this.onChange();
    }
  }
};
const __cssModules$1R = {};
var component$1R = normalizeComponent(script$1R, render$1R, staticRenderFns$1R, false, injectStyles$1R, null, null, null);
function injectStyles$1R(context) {
  for (let o in __cssModules$1R) {
    this[o] = __cssModules$1R[o];
  }
}
component$1R.options.__file = "../zero.Commerce/Plugin/pages/products/partials/product-giftvalues.vue";
var GiftValues = component$1R.exports;
var render$1Q = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-productfanout", class: {"is-disabled": _vm.disabled}}, [_vm.loaded ? _c("ui-check-list", {attrs: {value: _vm.value, items: _vm.items, limit: 10, "label-key": "name", "id-key": "id", disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}}) : _vm._e()], 1);
};
var staticRenderFns$1Q = [];
const script$1Q = {
  name: "shopProductFanout",
  props: {
    value: {
      type: Array,
      default: () => []
    },
    entity: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    loaded: false,
    items: [],
    properties: []
  }),
  watch: {
    "entity.variantGroups": {
      deep: true,
      handler() {
        this.rebuild();
      }
    },
    "entity.variants": {
      deep: true,
      handler() {
        this.rebuild();
      }
    }
  },
  created() {
    this.loadProperties();
  },
  methods: {
    loadProperties() {
      PropertiesApi.getForPickerWithConfig().then((res) => {
        this.properties = res;
        this.rebuild();
      });
    },
    rebuild() {
      let ids = [];
      this.entity.variantGroups.forEach((group) => ids.push(group.propertyId));
      this.items = this.properties.filter((x) => ids.indexOf(x.id) > -1);
      this.loaded = true;
    },
    onChange(ev) {
    }
  }
};
const __cssModules$1Q = {};
var component$1Q = normalizeComponent(script$1Q, render$1Q, staticRenderFns$1Q, false, injectStyles$1Q, null, null, null);
function injectStyles$1Q(context) {
  for (let o in __cssModules$1Q) {
    this[o] = __cssModules$1Q[o];
  }
}
component$1Q.options.__file = "../zero.Commerce/Plugin/pages/catalogue-old/products/partials/product-fanout.vue";
var ProductFanout = component$1Q.exports;
const TYPES$4 = [
  {value: "@shop.product.type_states.physical", key: "physical"},
  {value: "@shop.product.type_states.digital", key: "digital"},
  {value: "@shop.product.type_states.giftcard", key: "giftCard"}
];
const editor$D = new Editor("commerce.product", "@shop.product.fields.");
const general$3 = editor$D.tab("general", "@ui.tab_general");
const relations = editor$D.tab("relations", "@shop.product.tabs.relations");
const availability = editor$D.tab("availability", "@shop.product.tabs.availability");
const values$1 = editor$D.tab("values", "@shop.product.tabs.values", (x) => x.variants.length, (x) => x.type !== "giftCard");
const variants = editor$D.tab("variants", "@shop.product.tabs.variants", (x) => x.variants.length, (x) => x.type === "giftCard");
general$3.field("name", {label: "@ui.name"}).text(120).required();
general$3.field("type").select(TYPES$4).required();
general$3.field("description").rte();
general$3.field("infoDescription").rte({maxLength: 160});
general$3.field("presentationImageId").image();
relations.field("catalogueId").custom(CataloguePicker).required();
relations.field("manufacturerIds").custom(ManufacturerPicker, {limit: 3});
relations.field("taxId").custom(TaxPicker);
relations.field("properties").custom(PropertyMultipicker);
relations.field("crossSaleProducts").custom(ProductPicker, {limit: 20, global: true});
relations.field("similarProducts").custom(ProductPicker, {limit: 20, global: true});
availability.field("availability.releaseDate").dateRangePicker();
availability.field("availability.canBePurchased").toggle();
availability.field("availability.isListed").toggle();
availability.fieldset((set) => {
  set.field("availability.minimumPurchaseQuantity").number();
  set.field("availability.maximumPurchaseQuantity").number();
});
values$1.field("variants", {label: "@shop.product.fields.giftValues", description: "@shop.product.fields.giftValues_text"}).custom(GiftValues).required();
variants.field("variants", {hideLabel: true}).custom(Variants).required();
variants.field("fanoutPropertyIds").when((x) => x.variantGroups.length > 0).custom(ProductFanout);
const editor$C = new Editor("commerce.promotion", "@shop.promotion.fields.");
editor$C.field("name", {label: "@ui.name"}).text(60).required();
editor$C.field("imageId").image();
editor$C.field("description").rte();
editor$C.field("website").text();
editor$C.field("promotionalImageId").image();
editor$C.field("synonyms", {label: "@shop.synonyms.label", description: "@shop.synonyms.description"}).tags();
function createPropertyValuesList(type, includeGroups) {
  const list2 = new List("commerce.property-values");
  list2.templateLabel = (x) => "@shop.propertyvalue.fields." + x;
  list2.query.pageSize = 10;
  list2.onFetch((filter2) => PropertyValuesApi.getByQuery(filter2));
  if (type === "color") {
    list2.column("name", {class: "is-bold"}).custom((val, entity) => {
      return '<span class="-color" style="background-color:' + entity.hex + ';"></span> ' + val;
    }, true);
  } else {
    list2.column("name", {class: "is-bold"}).name();
  }
  if (includeGroups) {
    list2.column("groupId").text();
  }
  list2.column("count").text();
  return list2;
}
var render$1P = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "shop-property-value", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-editor", {attrs: {config: _vm.editor, meta: _vm.meta, disabled: _vm.disabled}, model: {value: _vm.item, callback: function($$v) {
      _vm.item = $$v;
    }, expression: "item"}}), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, state: form.state, label: "@ui.save"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}}), !_vm.disabled && _vm.model.id ? _c("ui-button", {staticStyle: {float: "right", "margin-right": "-16px"}, attrs: {type: "blank", label: "@ui.deleteQ"}, on: {click: _vm.onDelete}}) : _vm._e()], 1)];
  }}], null, false, 3212680143)}) : _vm._e();
};
var staticRenderFns$1P = [];
var propertyValue_vue_vue_type_style_index_0_lang = ".shop-property-value {\n  text-align: left;\n}\n.shop-property-value .editor {\n  margin: 0;\n  padding: 0;\n  padding-bottom: 30px;\n  border-bottom: 1px solid var(--color-line);\n}\n.shop-property-value .editor .ui-property + .ui-property, .shop-property-value .editor .ui-split + .ui-property {\n  margin-top: 25px;\n}\n.shop-property-value .editor .ui-split .ui-property + .ui-property {\n  margin-top: 0;\n}\n.shop-property-value .ui-box {\n  box-shadow: none;\n  padding: 0;\n}\n.shop-property-value .ui-colorpicker-input {\n  max-width: none !important;\n}";
const script$1P = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false,
    meta: {},
    item: {key: null, value: null},
    disabled: false,
    displayItems: [
      {label: "@translation.display.text", value: "text"},
      {label: "@translation.display.html", value: "html"}
    ],
    editor: null
  }),
  methods: {
    onLoad(form) {
      this.editor = this.zero.commerce.properties.valueEditorCreate(this.config.entity);
      form.load(!this.model.id ? PropertyValuesApi.getEmpty() : PropertyValuesApi.getById(this.model.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.item = response.entity;
        this.item.propertyId = this.config.propertyId;
        this.item._groups = this.config.groups;
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(PropertyValuesApi.save(this.item)).then((res) => {
        this.config.confirm(res);
      });
    },
    onDelete() {
      this.$refs.form.onDelete(PropertyValuesApi.delete.bind(this, this.item.id));
    }
  }
};
const __cssModules$1P = {};
var component$1P = normalizeComponent(script$1P, render$1P, staticRenderFns$1P, false, injectStyles$1P, null, null, null);
function injectStyles$1P(context) {
  for (let o in __cssModules$1P) {
    this[o] = __cssModules$1P[o];
  }
}
component$1P.options.__file = "../zero.Commerce/Plugin/pages/properties/property-value.vue";
var ItemOverlay = component$1P.exports;
var render$1O = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-property-values"}, [_vm.list ? _c("ui-table", {ref: "table", attrs: {config: _vm.list, inline: true}, on: {loaded: _vm.onTableChange}, scopedSlots: _vm._u([{key: "top", fn: function() {
    return [_c("div", {staticClass: "ui-table-row"}, [_c("div", {staticClass: "ui-table-cell shop-property-values-top"}, [_c("ui-button", {attrs: {type: "light", icon: "fth-plus", label: "Add value"}, on: {click: function($event) {
      return _vm.onValueClick();
    }}}), _vm.$refs.table ? _c("ui-search", {model: {value: _vm.$refs.table.query.search, callback: function($$v) {
      _vm.$set(_vm.$refs.table.query, "search", $$v);
    }, expression: "$refs.table.query.search"}}) : _vm._e()], 1)])];
  }, proxy: true}], null, false, 2144360228)}) : _vm._e()], 1);
};
var staticRenderFns$1O = [];
var propertyValues_vue_vue_type_style_index_0_lang = ".shop-property-values .-color {\n  display: inline-block;\n  width: 16px;\n  height: 16px;\n  border-radius: 2px;\n  box-shadow: var(--shadow-short);\n  margin-right: 16px;\n  position: relative;\n  top: -1px;\n}\n.shop-property-values-top.ui-table-cell {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  grid-gap: 20px;\n  padding-top: 16px;\n  padding-bottom: 15px;\n}";
const script$1O = {
  name: "ShopPropertyValues",
  props: {
    value: {
      type: String
    },
    entity: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    list: null
  }),
  watch: {
    $route(to, from) {
      this.rebuild();
    },
    "entity.type": function() {
      this.rebuild();
    }
  },
  created() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.list = createPropertyValuesList(this.entity ? this.entity.type : "text", false);
      this.list.onClick = this.onValueClick;
      this.list.onFetch((filter2) => PropertyValuesApi.getForList(this.entity.id, filter2));
      if (this.$refs.table) {
        this.$refs.table.setup();
      }
    },
    onTableChange(result) {
      let parent = this.$parent;
      while (parent != null && parent.$options.name !== "uiTab") {
        parent = parent.$parent;
      }
      if (parent != null) {
        parent.setCount(result.totalItems);
      }
    },
    onValueClick(item2) {
      Overlay.open({
        component: ItemOverlay,
        width: 520,
        model: item2 || {id: null},
        entity: this.entity,
        type: this.entity.type,
        propertyId: this.entity.id,
        groups: this.entity.groups
      }).then((res) => {
        console.info(res);
      }, () => {
      });
    }
  }
};
const __cssModules$1O = {};
var component$1O = normalizeComponent(script$1O, render$1O, staticRenderFns$1O, false, injectStyles$1O, null, null, null);
function injectStyles$1O(context) {
  for (let o in __cssModules$1O) {
    this[o] = __cssModules$1O[o];
  }
}
component$1O.options.__file = "../zero.Commerce/Plugin/pages/properties/property-values.vue";
var PropertyValues = component$1O.exports;
var render$1N = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-property-groups"}, [_c("div", {staticClass: "ui-input-list", class: {"is-disabled": _vm.disabled}}, [_c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated, handle: ".is-handle", enabled: !_vm.disabled}, expression: "{ onUpdate: onSortingUpdated, handle: '.is-handle', enabled: !disabled }"}], staticClass: "ui-input-list-sortable"}, _vm._l(_vm.value, function(item2) {
    return _c("div", {key: item2.id, staticClass: "ui-input-list-item"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: item2.name, expression: "item.name"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "40", readonly: _vm.disabled, size: "5"}, domProps: {value: item2.name}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "name", $event.target.value);
    }}}), !_vm.disabled ? _c("ui-icon-button", {staticClass: "is-handle", attrs: {type: "light", icon: "fth-grip-vertical"}}) : _vm._e(), !_vm.disabled ? _c("ui-icon-button", {attrs: {type: "light", icon: "fth-x"}, on: {click: function($event) {
      return _vm.removeItem(item2);
    }}}) : _vm._e()], 1);
  }), 0), !_vm.disabled ? _c("ui-button", {attrs: {type: "light", label: "Add group", icon: "fth-plus"}, on: {click: _vm.addItem}}) : _vm._e()], 1)]);
};
var staticRenderFns$1N = [];
const script$1N = {
  name: "ShopPropertyGroups",
  props: {
    value: {
      type: Array,
      default: () => []
    },
    entity: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    items: []
  }),
  watch: {
    value(value) {
      this.build();
    }
  },
  mounted() {
    this.build();
  },
  methods: {
    build() {
      this.items = this.value ? this.value.slice() : [];
    },
    onChange() {
      this.$emit("input", this.items);
    },
    addItem() {
      this.items.push({
        name: null,
        alias: null,
        id: Strings.guid(8),
        sort: (this.value.length < 1 ? -1 : max(this.value, (x) => x.sort).sort) + 1
      });
      this.onChange();
      this.$nextTick(() => {
        this.$el.querySelector(".ui-input-list-item:last-of-type input").focus();
      });
    },
    removeItem(item2) {
      const index = this.items.indexOf(item2);
      this.items.splice(index, 1);
      this.onChange();
    },
    onSortingUpdated(ev) {
      let sort = 0;
      this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
      this.items.forEach((x) => {
        x.sort = sort++;
      });
      this.onChange();
    }
  }
};
const __cssModules$1N = {};
var component$1N = normalizeComponent(script$1N, render$1N, staticRenderFns$1N, false, injectStyles$1N, null, null, null);
function injectStyles$1N(context) {
  for (let o in __cssModules$1N) {
    this[o] = __cssModules$1N[o];
  }
}
component$1N.options.__file = "../zero.Commerce/Plugin/pages/properties/property-groups.vue";
var PropertyGroups = component$1N.exports;
const TYPES$3 = [
  {value: "@shop.property.type_states.select", key: "select"},
  {value: "@shop.property.type_states.color", key: "color"},
  {value: "@shop.property.type_states.icon", key: "icon"},
  {value: "@shop.property.type_states.boolean", key: "boolean"}
];
const editor$B = new Editor("commerce.property", "@shop.property.fields.");
const general$2 = editor$B.tab("general", "@ui.tab_general");
const values = editor$B.tab("values", "@shop.property.tabs.values", 0, (x) => x.type === "boolean");
general$2.field("name", {label: "@ui.name"}).text(80).required();
general$2.field("description").text(200);
general$2.field("type").select(TYPES$3);
general$2.field("limit").when((x) => x.type !== "boolean").number(2).required();
general$2.field("allowText").when((x) => x.type !== "boolean").toggle();
values.field("id", {label: "@shop.property.fields.values", description: "@shop.property.fields.values_text"}).when((x) => x.type !== "boolean").custom(PropertyValues).required();
values.field("groups", {label: "@shop.property.fields.groups", description: "@shop.property.fields.groups_text"}).when((x) => x.type !== "boolean").custom(PropertyGroups);
const editor$A = new Editor("commerce.tax", "@shop.tax.fields.");
editor$A.field("name", {label: "@ui.name", helpText: "@shop.tax.fields.name_help"}).text(30).required();
editor$A.field("rate").number().required();
const FILTER_TYPES$1 = [
  {value: "@shop.filter.types.manufacturer", key: "manufacturer"},
  {value: "@shop.filter.types.price", key: "price"},
  {value: "@shop.filter.types.rating", key: "rating"},
  {value: "@shop.filter.types.property", key: "property"},
  {value: "@shop.filter.types.custom", key: "custom"}
];
const SORTING_TYPES$1 = [
  {value: "@shop.filter.sorting_states.manual", key: "manual"},
  {value: "@shop.filter.sorting_states.numeric", key: "numeric"},
  {value: "@shop.filter.sorting_states.alphanumeric", key: "alphanumeric"},
  {value: "@shop.filter.sorting_states.relevance", key: "relevance"}
];
const editor$z = new Editor("commerce.filter", "@shop.filter.fields.");
editor$z.field("name", {label: "@ui.name"}).text(30).required();
editor$z.field("isActive").toggle();
editor$z.field("filterType").select(FILTER_TYPES$1).disabled();
editor$z.field("propertyId").when((x) => x.filterType === "property").text().disabled();
editor$z.field("sortingMethod").select(SORTING_TYPES$1).required();
editor$z.field("displayProductCount").when((x) => x.filterType !== "price").toggle();
var editors$3 = {
  category: editor$U,
  channel: editor$P,
  currency: editor$O,
  customer: editor$M,
  manufacturer: editor$L,
  number: editor$K,
  orderstate: editor$J,
  orderaddress: editor$I,
  orderShipping: editor$H,
  orderShippingDelivery: editor$G,
  orderContact: editor$F,
  orderMeta: editor$E,
  product: editor$D,
  promotion: editor$C,
  property: editor$B,
  shippingOption: editor$S,
  shippingPrice: editor$T,
  tax: editor$A,
  filter: editor$z
};
var render$1M = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-pricerange"}, [_c("div", {staticClass: "ui-split"}, [_c("div", {staticClass: "ui-pricerange-group"}, [_c("ui-property", {attrs: {label: "@ui.price.range_from", vertical: true}}, [_c("currency-input", {attrs: {value: _vm.from, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.onValueChange($event, false);
  }}})], 1)], 1), _c("div", {staticClass: "ui-pricerange-group"}, [_c("ui-property", {attrs: {label: "@ui.price.range_to", vertical: true}}, [_c("currency-input", {attrs: {value: _vm.to, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.onValueChange($event, true);
  }}})], 1)], 1)])]);
};
var staticRenderFns$1M = [];
var pricerange_vue_vue_type_style_index_0_lang = "";
const script$1M = {
  props: {
    value: {
      type: Object,
      default: {
        from: null,
        to: null
      }
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  components: {CurrencyInput: UiCurrency},
  data: () => ({
    from: null,
    to: null
  }),
  watch: {
    value: {
      deep: true,
      handler() {
        this.rebuild();
      }
    }
  },
  mounted() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      if (!this.value) {
        this.from = null;
        this.to = null;
      } else {
        this.from = this.value.from;
        this.to = this.value.to;
      }
    },
    onValueChange(value, isTo) {
      var parsedValue = parseFloat(value);
      if (isNaN(parsedValue)) {
        return;
      }
      if (!this.value) {
        this.value = {};
      }
      if (isTo) {
        this.value.to = parsedValue;
      } else {
        this.value.from = parsedValue;
      }
      this.$emit("input", this.value);
    }
  }
};
const __cssModules$1M = {};
var component$1M = normalizeComponent(script$1M, render$1M, staticRenderFns$1M, false, injectStyles$1M, null, null, null);
function injectStyles$1M(context) {
  for (let o in __cssModules$1M) {
    this[o] = __cssModules$1M[o];
  }
}
component$1M.options.__file = "../zero.Commerce/Plugin/editor/pricerange.vue";
var PriceRange = component$1M.exports;
const editor$y = new Editor("commerce.customers-filter", "@shop.customer.filter.");
editor$y.field("date").dateRangePicker({inline: true}).preview({
  icon: "fth-calendar",
  hasValue: (x) => x.from || x.to,
  preview: (x) => Localization.localize(!x.from && !x.to ? null : x.from && !x.to ? "@ui.date.x" : !x.from && x.to ? "@ui.date.y" : "@ui.date.xtoy", {tokens: {x: Strings.date(x.from), y: Strings.date(x.to)}})
});
editor$y.field("countryId").countryPicker().preview({
  icon: "fth-globe",
  hasValue: (x) => !!x,
  preview: (x) => x != null ? "Selected" : null
});
editor$y.field("turnover").custom(PriceRange).preview({
  icon: "fth-dollar-sign",
  hasValue: (x) => x.from || x.to,
  preview: (x) => Localization.localize(!x.from && !x.to ? null : x.from && !x.to ? "@ui.price.x" : !x.from && x.to ? "@ui.price.y" : "@ui.price.xtoy", {tokens: {x: Strings.currency(x.from), y: Strings.currency(x.to)}})
});
editor$y.field("search").text(null, "Enter a search term...").preview({
  icon: "fth-search",
  hasValue: (x) => !!x,
  preview: (x) => x
});
const list$m = new List("commerce.customers");
list$m.templateLabel = (x) => "@shop.customer.fields." + x;
list$m.link = "commerce-customers-edit";
list$m.onFetch((filter2) => CustomersApi.getByQuery(filter2));
list$m.column("name").custom((value, model) => "<b>" + value + '</b> <span class="-minor" style="margin-left:8px;">#' + model.no + "</span>", true);
list$m.column("email").text();
list$m.column("address", {canSort: false, label: "@shop.customer.fields.address.address"}).custom((value, model) => `<i class="ui-icon" data-symbol="flag-${model.country.toLowerCase()}"></i> ` + value, true);
list$m.column("turnover", {width: 150, canSort: false}).currency();
list$m.column("isActive").active();
list$m.column("createdDate").created();
list$m.useFilter(editor$y, {
  date: {from: null, to: null},
  countryId: null,
  turnover: {from: null, to: null},
  search: null
});
list$m.export(CustomersApi.export);
const list$l = new List("commerce.manufacturers");
list$l.templateLabel = (x) => "@shop.manufacturer.fields." + x;
list$l.link = "commerce-manufacturers-edit";
list$l.onFetch((filter2) => ManufacturersApi.getByQuery(filter2));
list$l.column("name").name();
list$l.column("website").text();
list$l.column("isActive").active();
list$l.column("createdDate").created();
const list$k = new List("commerce.numbers");
list$k.templateLabel = (x) => "@shop.number.fields." + x;
list$k.link = (item2) => {
  return {
    name: "commerce-numbers-edit",
    params: {
      id: item2.typeAlias
    }
  };
};
list$k.onFetch((filter2) => NumbersApi.getByQuery(filter2));
list$k.column("name", {label: "@ui.name", class: "is-bold", canSort: false}).text({localize: true});
list$k.column("template", {canSort: false}).text();
list$k.column("startNumber", {canSort: false}).text();
list$k.column("minLength", {canSort: false}).text();
var OrderStatesApi = __assign({}, collection$1("commerceOrderStates/"));
const editor$x = new Editor("commerce.orders-filter", "@shop.order.filter.");
editor$x.field("stateIds").checkList(OrderStatesApi.getForPicker, {
  idKey: "id",
  labelKey: "name"
}).preview({
  icon: "fth-check-circle",
  hasValue: (x) => x.length > 0,
  preview: (x) => x.length > 0 ? x.length + " selected" : ""
});
editor$x.field("channelIds").checkList(ChannelsApi.getForPicker, {
  idKey: "id",
  labelKey: "name"
}).preview({
  icon: "fth-flag",
  hasValue: (x) => x.length > 0,
  preview: (x) => x.length > 0 ? x.length + " selected" : ""
});
editor$x.field("date").dateRangePicker({inline: true}).preview({
  icon: "fth-calendar",
  hasValue: (x) => x.from || x.to,
  preview: (x) => Localization.localize(!x.from && !x.to ? null : x.from && !x.to ? "@ui.date.x" : !x.from && x.to ? "@ui.date.y" : "@ui.date.xtoy", {tokens: {x: Strings.date(x.from), y: Strings.date(x.to)}})
});
editor$x.field("countryId").countryPicker().preview({
  icon: "fth-globe",
  hasValue: (x) => !!x,
  preview: (x) => x != null ? "Selected" : null
});
editor$x.field("price").custom(PriceRange).preview({
  icon: "fth-dollar-sign",
  hasValue: (x) => x.from || x.to,
  preview: (x) => Localization.localize(!x.from && !x.to ? null : x.from && !x.to ? "@ui.price.x" : !x.from && x.to ? "@ui.price.y" : "@ui.price.xtoy", {tokens: {x: Strings.currency(x.from), y: Strings.currency(x.to)}})
});
editor$x.field("search").text(null, "Enter a search term...").preview({
  icon: "fth-search",
  hasValue: (x) => !!x,
  preview: (x) => x
});
const list$j = new List("commerce.orders");
list$j.templateLabel = (x) => "@shop.order.fields." + x;
list$j.link = "commerce-orders-edit";
list$j.onFetch((filter2) => OrdersApi.getByQuery(filter2));
list$j.column("number", {class: "is-name is-bold", label: "@shop.order.fields.order", width: 180}).custom((value) => {
  return '<span class="-minor">#</span>' + value;
}, true);
list$j.column("customer", {class: "is-vertical"}).custom((value, model) => {
  return "<b>" + value + "</b>" + (model.address ? `<span class="-minor">${model.countryIso.toUpperCase() + ", " + model.address}</span>` : "");
}, true);
list$j.column("state", {width: 240}).custom((value, model) => `<span class="shop-orders-col-state2" data-state="${value}">${model.detailedState}</span>`, true);
list$j.column("paymentState", {width: 140}).boolean((x) => x === "authorized" || x === "captured");
list$j.column("shipped", {width: 140}).custom((x, model) => {
  if (model.countShipped >= model.countItems) {
    return `<svg class="ui-icon ui-table-field-bool" width="16" height="16" stroke-width="2.5" data-symbol="fth-check"><use xlink:href="#fth-check" /></svg>`;
  } else if (model.countShipped <= 0) {
    return `<svg class="ui-icon ui-table-field-bool" width="16" height="16" stroke-width="2" data-symbol="fth-x"><use xlink:href="#fth-x" /></svg>`;
  }
  return model.countShipped + " / " + model.countItems;
}, true);
list$j.column("price", {width: 180}).custom((value, model) => Strings.currency(value), true);
list$j.column("createdDate").created();
list$j.useFilter(editor$x, {
  stateIds: [],
  paymentStateIds: [],
  channelIds: [],
  date: {from: null, to: null},
  price: {from: null, to: null},
  search: null
});
const STATE_TYPES = [
  {label: "@shop.orderstate.states.open", value: "open"},
  {label: "@shop.orderstate.states.processing", value: "processing"},
  {label: "@shop.orderstate.states.completed", value: "completed"},
  {label: "@shop.orderstate.states.cancelled", value: "cancelled"}
];
const list$i = new List("commerce.orderstates");
list$i.templateLabel = (x) => "@shop.orderstate.fields." + x;
list$i.link = "commerce-orderstates-edit";
list$i.onFetch((filter2) => OrderStatesApi.getByQuery(filter2));
list$i.column("name").name();
list$i.column("description").text();
list$i.column("underlyingState", {width: 260}).custom((value) => Localization.localize(STATE_TYPES.find((t) => t.value == value).label));
const list$h = new List("commerce.products");
list$h.templateLabel = (x) => "@shop.product.fields." + x;
list$h.link = "commerce-products-edit";
list$h.column("image", {width: 64, canSort: false, hideLabel: true}).image();
list$h.column("name", {class: "is-vertical"}).custom((value, model) => {
  return `${model.isActive ? "<b>" + value + "</b>" : value}<span class="-minor">${model.catalogue}</span>`;
}, true);
list$h.column("price", {width: 160}).custom((value, model) => Strings.currency(model.priceFrom), true);
list$h.column("isActive").active();
list$h.column("createdDate").created();
const base$7 = "commercePromotions/";
var PromotionsApi = {
  getById(id) {
    return axios.get(base$7 + "getById", {params: {id}}).then((res) => Promise.resolve(res.data));
  },
  getEmpty(id) {
    return axios.get(base$7 + "getEmpty").then((res) => Promise.resolve(res.data));
  },
  getByQuery(query) {
    return axios.get(base$7 + "getByQuery", {params: {query}}).then((res) => Promise.resolve(res.data));
  },
  getPreviews(ids) {
    return axios.get(base$7 + "getPreviews", {params: {ids}}).then((res) => Promise.resolve(res.data));
  },
  getForPicker() {
    return axios.get(base$7 + "getForPicker").then((res) => Promise.resolve(res.data));
  },
  save(model) {
    return axios.post(base$7 + "save", model).then((res) => Promise.resolve(res.data));
  },
  delete(id) {
    return axios.delete(base$7 + "delete", {params: {id}}).then((res) => Promise.resolve(res.data));
  }
};
const list$g = new List("commerce.promotions");
list$g.templateLabel = (x) => "@shop.promotion.fields." + x;
list$g.link = "commerce-promotions-edit";
list$g.onFetch((filter2) => PromotionsApi.getByQuery(filter2));
list$g.column("name").name();
list$g.column("isActive").active();
const TYPES$2 = [
  {label: "@shop.property.type_states.select", value: "select"},
  {label: "@shop.property.type_states.color", value: "color"},
  {label: "@shop.property.type_states.icon", value: "icon"},
  {label: "@shop.property.type_states.boolean", value: "boolean"}
];
const list$f = new List("commerce.properties");
list$f.templateLabel = (x) => "@shop.property.fields." + x;
list$f.link = "commerce-properties-edit";
list$f.onFetch((filter2) => PropertiesApi.getByQuery(filter2));
list$f.column("name").name();
list$f.column("description").text();
list$f.column("type", {label: "@shop.property.fields.type_short"}).custom((value, model) => Localization.localize(TYPES$2.filter((x) => x.value === value)[0].label));
list$f.column("isActive").active();
var propertyValues = createPropertyValuesList("text", false);
const list$e = new List("commerce.shipping");
list$e.templateLabel = (x) => "@shop.shipping.fields." + x;
list$e.link = "commerce-shipping-edit";
list$e.onFetch((filter2) => ShippingOptionsApi.getByQuery(filter2));
list$e.column("name").name();
list$e.column("deliveryTime").text();
list$e.column("isActive").active();
const list$d = new List("commerce.taxes");
list$d.templateLabel = (x) => "@shop.tax.fields." + x;
list$d.link = "commerce-taxes-edit";
list$d.onFetch((filter2) => TaxesApi.getByQuery(filter2));
list$d.column("name").name();
list$d.column("rate").custom((value) => value + " %");
list$d.column("isActive").active();
const base$6 = "commerceProductFilters/";
var FiltersApi = __assign(__assign({}, collection$1(base$6)), {
  getAllWithCounts: async () => await get$1(base$6 + "getAllWithCounts"),
  saveSorting: async (ids) => await post$1(base$6 + "saveSorting", ids),
  setActive: async (id, isActive) => await post$1(base$6 + "setActive", {id, isActive})
});
const FILTER_TYPES = [
  {value: "@shop.filter.types.manufacturer", key: "manufacturer"},
  {value: "@shop.filter.types.price", key: "price"},
  {value: "@shop.filter.types.rating", key: "rating"},
  {value: "@shop.filter.types.property", key: "property"},
  {value: "@shop.filter.types.custom", key: "custom"}
];
const SORTING_TYPES = [
  {value: "@shop.filter.sorting_states.manual", key: "manual"},
  {value: "@shop.filter.sorting_states.numeric", key: "numeric"},
  {value: "@shop.filter.sorting_states.alphanumeric", key: "alphanumeric"},
  {value: "@shop.filter.sorting_states.relevance", key: "relevance"}
];
const list$c = new List("commerce.filters");
list$c.templateLabel = (x) => "@shop.filter.fields." + x;
list$c.link = "commerce-filters-edit";
list$c.onFetch((filter2) => FiltersApi.getByQuery(filter2));
list$c.column("name").name();
list$c.column("filterType").custom((value, model) => Localization.localize(FILTER_TYPES.find((t) => t.key == value).value));
list$c.column("sortingMethod").custom((value, model) => Localization.localize(SORTING_TYPES.find((t) => t.key == value).value));
list$c.column("isActive").active();
var lists$3 = {
  customers: list$m,
  manufacturers: list$l,
  numbers: list$k,
  orders: list$j,
  orderstates: list$i,
  products: list$h,
  promotions: list$g,
  properties: list$f,
  propertyValues,
  shipping: list$e,
  taxes: list$d,
  filters: list$c
};
const alias$2 = "commerce";
const section$2 = __zero.sections.find((section2) => section2.alias === alias$2);
const settings$1 = __zero.settingsAreas.find((area) => area.name === "@shop.settings.name");
__zero.sections.find((section2) => section2.alias === __zero.alias.sections.settings);
let routes$3 = [];
const addArea = (areaAlias, component2, detailComponent, hasCreate, isSettings, postCreate) => {
  let area = isSettings ? settings$1.items.find((x) => x.alias === section$2.alias + "-" + areaAlias) : null;
  if (isSettings && !area) {
    return;
  }
  let alias2 = isSettings ? area.alias : section$2.alias + "-" + areaAlias;
  let url = isSettings ? area.url : section$2.url + "/" + areaAlias;
  let name = isSettings ? area.name : "@shop.section." + areaAlias;
  routes$3.push({
    name: alias2,
    path: url,
    component: component2,
    meta: {
      name
    }
  });
  if (detailComponent && hasCreate) {
    routes$3.push({
      name: alias2 + "-create",
      path: url + "/create/:scope?",
      props: true,
      component: detailComponent,
      meta: {
        create: true,
        name
      }
    });
  }
  if (detailComponent) {
    routes$3.push({
      name: alias2 + "-edit",
      path: url + "/edit/:id",
      props: true,
      component: detailComponent,
      meta: {
        name
      }
    });
  }
  if (typeof postCreate === "function") {
    postCreate(area);
  }
};
if (section$2) {
  routes$3.push({
    path: "/" + alias$2,
    component: () => __vitePreload(() => __import__("./dashboard.js"), true ? ["/zero/dashboard.js","/zero/vendor.js"] : void 0),
    name: alias$2
  });
  routes$3.push({
    name: section$2.alias + "-products",
    path: "/" + section$2.alias + "/products/:id?",
    component: () => __vitePreload(() => __import__("./overview2.js"), true ? ["/zero/overview2.js","/zero/overview2.css","/zero/vendor.js"] : void 0),
    props: true,
    meta: {
      name: "@shop.catalogue.pathnames.catalogue"
    }
  });
  routes$3.push({
    name: section$2.alias + "-products-edit",
    path: "/" + section$2.alias + "/products/edit/:id",
    component: () => __vitePreload(() => __import__("./product.js"), true ? ["/zero/product.js","/zero/vendor.js"] : void 0),
    props: true,
    meta: {
      name: "[todo]"
    }
  });
  routes$3.push({
    name: section$2.alias + "-products-create",
    path: "/" + section$2.alias + "/products/create/:type/:catalogueId?",
    props: true,
    component: () => __vitePreload(() => __import__("./product.js"), true ? ["/zero/product.js","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: "[todo]"
    }
  });
  routes$3.push({
    path: "/" + section$2.alias + "/categories/:channelId?",
    component: () => __vitePreload(() => __import__("./categories.js"), true ? ["/zero/categories.js","/zero/categories.css","/zero/vendor.js"] : void 0),
    name: section$2.alias + "-categories",
    props: true,
    meta: {
      name: "@shop.catalogue.pathnames.catalogue"
    },
    children: [
      {
        name: section$2.alias + "-categories-edit",
        path: "edit/:id",
        section: alias$2,
        props: true,
        component: () => __vitePreload(() => __import__("./category.js"), true ? ["/zero/category.js","/zero/vendor.js"] : void 0)
      },
      {
        name: section$2.alias + "-categories-create",
        path: "create/:channelId/:parent?",
        section: alias$2,
        props: true,
        component: () => __vitePreload(() => __import__("./category.js"), true ? ["/zero/category.js","/zero/vendor.js"] : void 0)
      }
    ]
  });
  addArea("channels", () => __vitePreload(() => __import__("./channels.js"), true ? ["/zero/channels.js","/zero/channels.css","/zero/select-overlay.js","/zero/select-overlay.css","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./channel.js"), true ? ["/zero/channel.js","/zero/vendor.js"] : void 0), true, true);
  addArea("customers", () => __vitePreload(() => __import__("./customers.js"), true ? ["/zero/customers.js","/zero/customers.css","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./customer.js"), true ? ["/zero/customer.js","/zero/vendor.js"] : void 0), true);
  addArea("manufacturers", () => __vitePreload(() => __import__("./manufacturers.js"), true ? ["/zero/manufacturers.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./manufacturer.js"), true ? ["/zero/manufacturer.js","/zero/vendor.js"] : void 0), true, true);
  addArea("orders", () => __vitePreload(() => Promise.resolve().then(function() {
    return orders$2;
  }), true ? void 0 : void 0), () => __vitePreload(() => __import__("./order.js").then(function(n) {
    return n.o;
  }), true ? ["/zero/order.js","/zero/order.css","/zero/vendor.js"] : void 0), true);
  addArea("currencies", () => __vitePreload(() => __import__("./currencies.js"), true ? ["/zero/currencies.js","/zero/currencies.css","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./currency.js"), true ? ["/zero/currency.js","/zero/vendor.js"] : void 0), true, true);
  addArea("numbers", () => __vitePreload(() => __import__("./numbers.js"), true ? ["/zero/numbers.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./number.js"), true ? ["/zero/number.js","/zero/vendor.js"] : void 0), true, true);
  addArea("orderstates", () => __vitePreload(() => __import__("./orderstates.js"), true ? ["/zero/orderstates.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./orderstate.js"), true ? ["/zero/orderstate.js","/zero/vendor.js"] : void 0), true, true);
  addArea("shipping", () => __vitePreload(() => __import__("./shipping.js"), true ? ["/zero/shipping.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./shipping-option.js"), true ? ["/zero/shipping-option.js","/zero/vendor.js"] : void 0), true, true);
  addArea("taxes", () => __vitePreload(() => __import__("./taxes.js"), true ? ["/zero/taxes.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./tax.js"), true ? ["/zero/tax.js","/zero/vendor.js"] : void 0), true, true);
  addArea("properties", () => __vitePreload(() => __import__("./properties.js"), true ? ["/zero/properties.js","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./property.js"), true ? ["/zero/property.js","/zero/vendor.js"] : void 0), true, true);
  addArea("filters", () => __vitePreload(() => __import__("./filters.js"), true ? ["/zero/filters.js","/zero/filters.css","/zero/vendor.js"] : void 0), () => __vitePreload(() => __import__("./filter.js"), true ? ["/zero/filter.js","/zero/vendor.js"] : void 0), false, true);
}
var render$1L = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-property-value-options ui-split"}, [_vm.entity._groups.length ? _c("ui-property", {attrs: {label: "@shop.propertyvalue.fields.groupId", vertical: true, field: "groupId"}}, [_c("div", {staticClass: "ui-native-select"}, [_c("select", {directives: [{name: "model", rawName: "v-model", value: _vm.entity.groupId, expression: "entity.groupId"}], on: {change: function($event) {
    var $$selectedVal = Array.prototype.filter.call($event.target.options, function(o) {
      return o.selected;
    }).map(function(o) {
      var val = "_value" in o ? o._value : o.value;
      return val;
    });
    _vm.$set(_vm.entity, "groupId", $event.target.multiple ? $$selectedVal : $$selectedVal[0]);
  }}}, [_c("option"), _vm._l(_vm.entity._groups, function(item2) {
    return _c("option", {key: item2.id, domProps: {value: item2.id}}, [_vm._v(_vm._s(item2.name))]);
  })], 2)])]) : _vm._e(), _c("ui-property", {attrs: {label: "@shop.propertyvalue.fields.sort", vertical: true, field: "sort"}}, [_c("input", {directives: [{name: "model", rawName: "v-model.number", value: _vm.entity.sort, expression: "entity.sort", modifiers: {number: true}}], staticClass: "ui-input", attrs: {type: "text", inputmode: "numeric", maxlength: "10", disabled: _vm.disabled}, domProps: {value: _vm.entity.sort}, on: {input: function($event) {
    if ($event.target.composing) {
      return;
    }
    _vm.$set(_vm.entity, "sort", _vm._n($event.target.value));
  }, blur: function($event) {
    return _vm.$forceUpdate();
  }}})])], 1);
};
var staticRenderFns$1L = [];
const script$1L = {
  name: "ShopPropertyValueOptions",
  props: {
    entity: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  }
};
const __cssModules$1L = {};
var component$1L = normalizeComponent(script$1L, render$1L, staticRenderFns$1L, false, injectStyles$1L, null, null, null);
function injectStyles$1L(context) {
  for (let o in __cssModules$1L) {
    this[o] = __cssModules$1L[o];
  }
}
component$1L.options.__file = "../zero.Commerce/Plugin/pages/properties/partials/property-value-options.vue";
var PropertyValueOptions = component$1L.exports;
function propertyValueEditorCreator(property) {
  const editor2 = new Editor("commerce.property-value", "@shop.propertyValue.fields.");
  editor2.field("name", {label: "@ui.name", class: "is-vertical"}).text(50).required();
  if (property.type === "color") {
    editor2.field("hex", {class: "is-vertical"}).colorPicker().required();
  }
  if (property.type === "icon") {
    editor2.field("icon", {class: "is-vertical"}).iconPicker().required();
  }
  editor2.field("sort", {hideLabel: true, class: "is-vertical"}).custom(PropertyValueOptions);
  return editor2;
}
const plugin$3 = new Plugin("zero.commerce");
const config$1 = {
  alias: {
    localChannel: "local"
  },
  properties: {
    valueEditorCreate: propertyValueEditorCreator
  }
};
plugin$3.addEditors(editors$3);
plugin$3.addLists(lists$3);
plugin$3.addRoutes(routes$3);
plugin$3.install = (vue, zero2) => {
  vue.component("ui-currency", () => __vitePreload(() => Promise.resolve().then(function() {
    return currency$1;
  }), true ? void 0 : void 0));
  zero2.config.linkPicker.areas.push({
    alias: "zero.shop.products",
    name: "@shop.linkpicker.products.name",
    component: () => __vitePreload(() => __import__("./products.js"), true ? ["/zero/products.js","/zero/vendor.js"] : void 0)
  });
  Object.defineProperty(zero2, "commerce", {
    get: () => config$1
  });
};
const base$5 = "modules/";
var ModulesApi = {
  getModuleTypes: async (tags, pageId) => await get$1(base$5 + "getModuleTypes", {params: {tags, pageId}}),
  getModuleType: async (alias2) => await get$1(base$5 + "getModuleType", {params: {alias: alias2}}),
  getEmpty: async (alias2) => await get$1(base$5 + "getEmpty", {params: {alias: alias2}})
};
var render$1K = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-blocks-start"}, [_c("ui-dropdown", {scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {label: "Add content", type: "primary", icon: "fth-plus"}})];
  }, proxy: true}])}, [_c("div", {staticClass: "ui-blocks-start-dropdown-items"}, _vm._l(_vm.types, function(type) {
    return _c("ui-dropdown-button", {key: type.alias, attrs: {label: type.name, description: type.description, icon: type.icon}, on: {click: function($event) {
      return _vm.select(type);
    }}});
  }), 1)])], 1);
};
var staticRenderFns$1K = [];
var add_vue_vue_type_style_index_0_lang = ".ui-blocks-start {\n  margin: 0;\n  margin-top: 50px;\n  margin-left: 110px;\n  display: flex;\n  /*.ui-blocks-inner-sortable + &\n  {\n    margin-top: var(--padding);\n  }*/\n}\n.ui-blocks-start .ui-select-button-icon {\n  width: 32px;\n  height: 32px;\n  border-radius: 32px;\n}\n.ui-blocks-start-dropdown-items {\n  max-height: 300px;\n  overflow-y: auto;\n}";
const script$1K = {
  name: "uiBlocksAdd",
  props: {
    value: Array,
    config: Object,
    types: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    canAdd: true
  }),
  methods: {
    select(module) {
      this.$emit("selected", module);
    }
  }
};
const __cssModules$1K = {};
var component$1K = normalizeComponent(script$1K, render$1K, staticRenderFns$1K, false, injectStyles$1K, null, null, null);
function injectStyles$1K(context) {
  for (let o in __cssModules$1K) {
    this[o] = __cssModules$1K[o];
  }
}
component$1K.options.__file = "../zero.Stories/Plugin/components/blocks/add.vue";
var AddBlock = component$1K.exports;
var render$1J = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-blocks-select"}, [_c("h2", {staticClass: "ui-headline"}, [_vm._v("Add block")]), !_vm.loading ? _c("div", [_c("div", {staticClass: "ui-blocks-select-items"}, _vm._l(_vm.types, function(item2) {
    return _c("button", {staticClass: "ui-blocks-select-item", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.onSelect(item2);
    }}}, [_c("div", {staticClass: "ui-blocks-select-item-top"}, [_c("ui-icon", {staticClass: "ui-blocks-select-item-icon", attrs: {symbol: item2.icon, size: 22}})], 1), _c("span", {staticClass: "ui-blocks-select-item-text"}, [_c("ui-localize", {attrs: {value: item2.name}}), item2.description ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item2.description, expression: "item.description"}]}) : _vm._e()], 1)]);
  }), 0), !_vm.types.length ? _c("ui-message", {attrs: {type: "error", text: "@page.create.nonavailable"}}) : _vm._e(), _c("div", {staticClass: "app-confirm-buttons"}, [_c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel}, on: {click: _vm.config.close}})], 1)], 1) : _vm._e()]);
};
var staticRenderFns$1J = [];
var select_vue_vue_type_style_index_0_lang = ".ui-blocks-select {\n  text-align: left;\n}\n.ui-blocks-select .ui-message {\n  margin: 0;\n}\n.ui-blocks-select-items {\n  margin: 0 -16px;\n  margin-top: var(--padding-s);\n  max-height: 600px;\n  overflow-y: auto;\n  display: grid;\n  grid-template-columns: repeat(auto-fit, 101px);\n}\n.ui-blocks-select-item {\n  display: flex;\n  flex-direction: column;\n  width: 100%;\n  justify-content: center;\n  text-align: center;\n  align-items: center;\n  position: relative;\n  color: var(--color-text);\n  padding: 16px 4px;\n  border-radius: var(--radius);\n  margin-top: 10px;\n}\n.ui-blocks-select-item:hover, .ui-blocks-select-item:focus {\n  background: var(--color-tree-selected);\n}\n.ui-blocks-select-item-text {\n  display: flex;\n  flex-direction: column;\n}\n.ui-blocks-select-item-text span {\n  color: var(--color-text);\n  margin-top: 3px;\n}\n.ui-blocks-select-item-top {\n  height: 36px;\n}\n.ui-blocks-select-item-icon {\n  color: var(--color-text-dim);\n}";
const script$1J = {
  props: {
    config: Object
  },
  data: () => ({
    loading: false,
    item: {},
    disabled: false,
    types: []
  }),
  created() {
    this.types = this.config.types;
  },
  methods: {
    onSelect(item2) {
      this.config.confirm(item2);
    }
  }
};
const __cssModules$1J = {};
var component$1J = normalizeComponent(script$1J, render$1J, staticRenderFns$1J, false, injectStyles$1J, null, null, null);
function injectStyles$1J(context) {
  for (let o in __cssModules$1J) {
    this[o] = __cssModules$1J[o];
  }
}
component$1J.options.__file = "../zero.Stories/Plugin/components/blocks/select.vue";
var SelectOverlay = component$1J.exports;
var render$1I = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-blocks", class: {"is-disabled": _vm.disabled}}, [_vm.items.length ? _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated, handle: ".ui-blocks-grab-handle"}, expression: "{ onUpdate: onSortingUpdated, handle: '.ui-blocks-grab-handle' }"}], staticClass: "ui-blocks-inner-sortable"}, _vm._l(_vm.items, function(item2, itemIndex) {
    return _c("div", {directives: [{name: "show", rawName: "v-show", value: item2.config.visible(item2), expression: "item.config.visible(item)"}], key: item2.id, staticClass: "ui-block", attrs: {"data-alias": item2.moduleTypeAlias.replace("stories.", "")}}, [_c("ui-icon-button", {staticClass: "ui-blocks-grab-handle", attrs: {icon: "fth-waffle", stroke: 0, size: 17}}), _c("ui-dropdown", {scopedSlots: _vm._u([{key: "button", fn: function() {
      return [_c("ui-icon-button", {attrs: {icon: "fth-chevron-down"}})];
    }, proxy: true}], null, true)}, [_c("div", {staticClass: "ui-blocks-start-dropdown-items"}, [item2.config.actions ? [_vm._l(item2.config.actions, function(action, idx) {
      return _c("ui-dropdown-button", {key: idx, attrs: {icon: action.icon, label: action.label}, on: {click: function($event) {
        return action.action(item2, _vm.$refs.blocks[itemIndex]);
      }}});
    }), _c("ui-dropdown-separator")] : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "Insert above", icon: "fth-plus"}, on: {click: function($event) {
      return _vm.addAbove(item2);
    }}}), _c("ui-dropdown-button", {attrs: {label: "Insert below", icon: "fth-plus"}, on: {click: function($event) {
      return _vm.addBelow(item2);
    }}}), _c("ui-dropdown-button", {attrs: {label: "Remove", icon: "fth-trash"}, on: {click: function($event) {
      return _vm.onRemove(item2);
    }}})], 2)]), _c(item2.config.component, {ref: "blocks", refInFor: true, tag: "component", staticClass: "ui-block-content", attrs: {value: item2, methods: {removeBlock: _vm.onRemove}}})], 1);
  }), 0) : _vm._e(), _c("div", {staticClass: "ui-blocks-start"}, [_c("ui-button", {attrs: {label: "Add content", type: "primary", icon: "fth-plus"}, on: {click: _vm.select}})], 1)]);
};
var staticRenderFns$1I = [];
var blocks_vue_vue_type_style_index_0_lang = ".ui-blocks {\n  margin: 0 auto;\n  max-width: 960px;\n}\n.ui-block {\n  display: grid;\n  grid-template-columns: auto auto minmax(auto, 1fr) 80px;\n  grid-gap: var(--padding-s);\n  align-items: flex-start;\n  justify-content: flex-start;\n}\n.ui-block .ui-icon-button {\n  transition: opacity 0.2s ease;\n  opacity: 0;\n}\n.ui-blocks-grab-handle {\n  background: transparent !important;\n  cursor: grab !important;\n  position: relative;\n  top: 0;\n  right: -10px;\n}\n.ui-block + .ui-block {\n  margin-top: var(--padding-m);\n}\n.ui-block:last-child {\n  margin-bottom: var(--padding-m);\n}\n.ui-block:hover .ui-icon-button, .ui-block:focus-within .ui-icon-button {\n  opacity: 1;\n}\n\n/*.ui-block:hover .ui-block-content\n{\n  box-shadow: var(--shadow);\n}*/\n.ui-block .ui-dropdown-container {\n  top: 0;\n}\n.ui-block-content {\n  margin-top: 0;\n  min-height: 32px !important;\n}\n.ui-block-content .ui-rte {\n  background: transparent;\n  border: none;\n}\n.ui-block-content .ui-rte:focus-within {\n  background-color: transparent;\n  border: none;\n  box-shadow: none;\n  outline: none;\n}\n.ui-block-content .ui-rte-controls {\n  display: none;\n}\n.ui-block-content .ui-rte-input {\n  width: 100%;\n}\n.ui-block .ui-rte-input {\n  font-size: var(--font-size-m);\n  line-height: 1.7;\n}";
const script$1I = {
  name: "uiBlocks",
  components: {AddBlock},
  props: {
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: Object
  },
  data: () => ({
    items: [],
    moduleTypes: [],
    interval: null
  }),
  created() {
    ModulesApi.getModuleTypes(["story"]).then((res) => {
      this.moduleTypes = res;
      this.setup(this.value);
    });
    this.interval = setInterval(() => {
      this.onChange(this.items);
    }, 1e3);
  },
  destroyed() {
    clearInterval(this.interval);
  },
  methods: {
    setup(value) {
      this.items = JSON.parse(JSON.stringify(value || []));
      this.items.forEach((item2) => {
        item2.config = this.zero.stories.modules.find((x) => x.alias == item2.moduleTypeAlias);
        item2.config.visible = typeof item2.config.visible === "function" ? item2.config.visible : () => true;
      });
    },
    select(index) {
      Overlay.open({
        component: SelectOverlay,
        types: this.moduleTypes,
        width: 540,
        theme: "dark"
      }).then((module) => this.onAdd(module, index), () => {
      });
    },
    addAbove(module) {
      let index = this.items.indexOf(module);
      this.select(index);
    },
    addBelow(module) {
      let index = this.items.indexOf(module);
      this.select(index + 1);
    },
    async onAdd(module, index) {
      const content2 = await this.getEmpty(module);
      let config2 = this.zero.stories.modules.find((x) => x.alias == module.alias);
      config2.visible = typeof config2.visible === "function" ? config2.visible : () => true;
      content2.entity.config = config2;
      if (typeof index !== "undefined" && index > -1) {
        this.items.splice(index, 0, content2.entity);
      } else {
        this.items.push(content2.entity);
      }
      let idx = this.items.indexOf(content2.entity);
      this.$nextTick(() => {
        let block = this.$refs.blocks[idx];
        if (typeof block.onBlockAdded === "function") {
          block.onBlockAdded(content2.entity);
        }
      });
    },
    onRemove(item2) {
      const index = this.items.indexOf(item2);
      this.items.splice(index, 1);
    },
    getEmpty(module) {
      return ModulesApi.getEmpty(module.alias);
    },
    onSortingUpdated(ev) {
    },
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1I = {};
var component$1I = normalizeComponent(script$1I, render$1I, staticRenderFns$1I, false, injectStyles$1I, null, null, null);
function injectStyles$1I(context) {
  for (let o in __cssModules$1I) {
    this[o] = __cssModules$1I[o];
  }
}
component$1I.options.__file = "../zero.Stories/Plugin/components/blocks/blocks.vue";
var BlockEditor = component$1I.exports;
var render$1H = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-blocks-title", class: {"is-disabled": _vm.disabled}}, [_c("ui-rte", {staticClass: "ui-blocks-title-input", attrs: {value: _vm.value, disabled: _vm.disabled, setup: _vm.onSetup, placeholder: "Write a title..."}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$1H = [];
var blocksTitle_vue_vue_type_style_index_0_lang = ".ui-blocks-title {\n  margin: 0 auto;\n  padding: 0 96px;\n  max-width: 960px;\n}\n.ui-blocks-title-input.ui-rte {\n  background: none;\n  border: none;\n}\n.ui-blocks-title-input.ui-rte:focus-within {\n  border: none;\n  outline: none;\n  box-shadow: none;\n}\n.ui-blocks-title-input .ui-rte-input {\n  font-weight: 600;\n  padding-top: 0;\n  padding-bottom: 0;\n  min-height: 0;\n}\n.ui-blocks-title-input .ui-rte-input p {\n  font-size: 32px;\n  font-weight: 900;\n  line-height: 1.3;\n}";
class HeadlineDoc$1 extends Doc {
  get schema() {
    return {content: "block"};
  }
}
const script$1H = {
  name: "uiBlocksTitle",
  props: {
    value: {
      type: String,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: Object
  },
  methods: {
    onSetup(config2) {
      config2.extensions = [new HeadlineDoc$1()];
      config2.commands = [];
    },
    onChange(value) {
      value = value.replace("<p>", "").replace("</p>", "");
      this.$emit("input", value);
    }
  }
};
const __cssModules$1H = {};
var component$1H = normalizeComponent(script$1H, render$1H, staticRenderFns$1H, false, injectStyles$1H, null, null, null);
function injectStyles$1H(context) {
  for (let o in __cssModules$1H) {
    this[o] = __cssModules$1H[o];
  }
}
component$1H.options.__file = "../zero.Stories/Plugin/components/blocks/blocks-title.vue";
var BlocksTitleEditor = component$1H.exports;
var AuthorsApi = __assign(__assign({}, collection$1("storyAuthors/")), {
  getTop: async (limit) => await get$1("storyAuthors/getTop", {params: {limit}})
});
var render$1G = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "stories-authorpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$1G = [];
var authorpicker_vue_vue_type_style_index_0_lang = ".stories-authorpicker .ui-select-button-icon.is-image, .stories-authorpicker .ui-select-button-icon {\n  padding: 0;\n  border-radius: 50px;\n}\n.stories-authorpicker .ui-select-button-icon.is-image img, .stories-authorpicker .ui-select-button-icon img {\n  border-radius: 50px;\n}";
const script$1G = {
  name: "storiesAuthorpicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "storyAuthor",
      items: AuthorsApi.getForPicker,
      previews: AuthorsApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true,
        iconAsImage: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1G = {};
var component$1G = normalizeComponent(script$1G, render$1G, staticRenderFns$1G, false, injectStyles$1G, null, null, null);
function injectStyles$1G(context) {
  for (let o in __cssModules$1G) {
    this[o] = __cssModules$1G[o];
  }
}
component$1G.options.__file = "../zero.Stories/Plugin/components/authorpicker.vue";
var Authorpicker = component$1G.exports;
var TagsApi = __assign(__assign({}, collection$1("storyTags/")), {
  getTop: async (limit) => await get$1("storyTags/getTop", {params: {limit}})
});
var render$1F = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "stories-tagpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$1F = [];
const script$1F = {
  name: "storiesTagpicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 3
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "storyTag",
      items: TagsApi.getForPicker,
      previews: TagsApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1F = {};
var component$1F = normalizeComponent(script$1F, render$1F, staticRenderFns$1F, false, injectStyles$1F, null, null, null);
function injectStyles$1F(context) {
  for (let o in __cssModules$1F) {
    this[o] = __cssModules$1F[o];
  }
}
component$1F.options.__file = "../zero.Stories/Plugin/components/tagpicker.vue";
var Tagpicker = component$1F.exports;
const editor$w = new Editor("story", "@stories.story.fields.");
const general$1 = editor$w.tab("general", "Story");
const options$1 = editor$w.tab("options", "Options");
const seo = editor$w.tab("seo", "Metadata");
general$1.field("name", {hideLabel: true}).custom(BlocksTitleEditor).required();
general$1.field("content", {hideLabel: true}).custom(BlockEditor).required();
options$1.field("date").datePicker({time: true}).required();
options$1.field("publishDate").datePicker({time: true});
options$1.field("authorId").custom(Authorpicker);
options$1.field("imageId").image();
options$1.field("excerpt").textarea(300);
options$1.field("tagIds").custom(Tagpicker);
seo.field("seoTitle").text(70);
seo.field("seoDescription").textarea(156);
seo.field("seoImageId").image();
const editor$v = new Editor("story.author", "@stories.author.fields.");
editor$v.field("name", {label: "@ui.name"}).text(60).required();
editor$v.field("avatarId").image();
editor$v.field("bio").rte();
const editor$u = new Editor("story.tag", "@stories.tag.fields.");
editor$u.field("name", {label: "@ui.name"}).text(60).required();
editor$u.field("description").text(120);
const editor$t = new Editor("modules.stories.paragraph", "@stories.block.paragraph.fields.");
editor$t.field("text").rte().required();
var editors$2 = {
  story: editor$w,
  author: editor$v,
  tag: editor$u,
  moduleParagraph: editor$t
};
var StoriesApi = __assign({}, collection$1("stories/"));
const list$b = new List("stories");
list$b.templateLabel = (x) => "@stories.story.fields." + x;
list$b.link = "stories-edit";
list$b.onFetch((filter2) => StoriesApi.getByQuery(filter2));
list$b.column("name").name();
list$b.column("date").date();
list$b.column("isActive").active();
const list$a = new List("stories.authors");
const prefix$2 = "@stories.author.fields.";
list$a.templateLabel = (x) => prefix$2 + x;
list$a.link = "stories-authors-edit";
list$a.onFetch((filter2) => AuthorsApi.getByQuery(filter2));
list$a.column("avatarId", {width: 70, canSort: false, hideLabel: true}).image();
list$a.column("name").name();
list$a.column("createdDate").created();
const list$9 = new List("stories.tags");
const prefix$1 = "@stories.tag.fields.";
list$9.templateLabel = (x) => prefix$1 + x;
list$9.link = "stories-tags-edit";
list$9.onFetch((filter2) => TagsApi.getByQuery(filter2));
list$9.column("icon", {hideLabel: true, width: 60}).icon("fth-tag");
list$9.column("name").name();
list$9.column("description").text();
list$9.column("createdDate").created();
var lists$2 = {
  stories: list$b,
  authors: list$a,
  tags: list$9
};
const alias$1 = "stories";
const section$1 = __zero.sections.find((section2) => section2.alias === alias$1);
let routes$2 = [];
if (section$1) {
  routes$2.push({
    name: alias$1,
    path: "/" + alias$1,
    component: () => __vitePreload(() => __import__("./dashboard2.js"), true ? ["/zero/dashboard2.js","/zero/vendor.js"] : void 0)
  });
  routes$2.push({
    name: alias$1 + "-content",
    path: "/" + alias$1 + "/content",
    component: () => __vitePreload(() => __import__("./stories.js"), true ? ["/zero/stories.js","/zero/stories.css","/zero/vendor.js"] : void 0)
  });
  routes$2.push({
    name: alias$1 + "-create",
    path: "/" + alias$1 + "/content/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./story.js"), true ? ["/zero/story.js","/zero/story.css","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: section$1.name
    }
  });
  routes$2.push({
    name: alias$1 + "-edit",
    path: "/" + alias$1 + "/content/edit/:id",
    props: true,
    component: () => __vitePreload(() => __import__("./story.js"), true ? ["/zero/story.js","/zero/story.css","/zero/vendor.js"] : void 0),
    meta: {
      name: section$1.name
    }
  });
  routes$2.push({
    name: alias$1 + "-authors",
    path: "/" + alias$1 + "/authors",
    component: () => __vitePreload(() => __import__("./authors.js"), true ? ["/zero/authors.js","/zero/authors.css","/zero/vendor.js"] : void 0)
  });
  routes$2.push({
    name: alias$1 + "-authors-create",
    path: "/" + alias$1 + "/authors/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./author.js"), true ? ["/zero/author.js","/zero/story.css","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: section$1.name
    }
  });
  routes$2.push({
    name: alias$1 + "-authors-edit",
    path: "/" + alias$1 + "/authors/edit/:id",
    props: true,
    component: () => __vitePreload(() => __import__("./author.js"), true ? ["/zero/author.js","/zero/story.css","/zero/vendor.js"] : void 0),
    meta: {
      name: section$1.name
    }
  });
  routes$2.push({
    name: alias$1 + "-tags",
    path: "/" + alias$1 + "/tags",
    component: () => __vitePreload(() => __import__("./tags.js"), true ? ["/zero/tags.js","/zero/vendor.js"] : void 0)
  });
  routes$2.push({
    name: alias$1 + "-tags-create",
    path: "/" + alias$1 + "/tags/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./tag.js"), true ? ["/zero/tag.js","/zero/story.css","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: section$1.name
    }
  });
  routes$2.push({
    name: alias$1 + "-tags-edit",
    path: "/" + alias$1 + "/tags/edit/:id",
    props: true,
    component: () => __vitePreload(() => __import__("./tag.js"), true ? ["/zero/tag.js","/zero/story.css","/zero/vendor.js"] : void 0),
    meta: {
      name: section$1.name
    }
  });
}
var render$1E = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-headline"}, [_c("ui-rte", {attrs: {"data-size": _vm.value.size, disabled: _vm.disabled, setup: _vm.onSetup, placeholder: "Write a headline..."}, model: {value: _vm.value.text, callback: function($$v) {
    _vm.$set(_vm.value, "text", $$v);
  }, expression: "value.text"}})], 1);
};
var staticRenderFns$1E = [];
var headline_vue_vue_type_style_index_0_lang = ".ui-block-headline {\n  margin-bottom: -20px;\n}\n.ui-block-headline .ui-rte-input {\n  font-weight: 600;\n  padding-top: 0;\n  padding-bottom: 0;\n  min-height: 0;\n}\n.ui-block-headline .ui-rte[data-size=h1] .ui-rte-input p {\n  font-size: 22px;\n}\n.ui-block-headline .ui-rte[data-size=h2] .ui-rte-input p {\n  font-size: 20px;\n}\n.ui-block-headline .ui-rte[data-size=h3] .ui-rte-input p {\n  font-size: 18px;\n}";
class HeadlineDoc extends Doc {
  get schema() {
    return {content: "block"};
  }
}
const script$1E = {
  name: "uiBlockHeadline",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    onSetup(config2) {
      config2.extensions = [new HeadlineDoc()];
      config2.commands = [];
    }
  }
};
const __cssModules$1E = {};
var component$1E = normalizeComponent(script$1E, render$1E, staticRenderFns$1E, false, injectStyles$1E, null, null, null);
function injectStyles$1E(context) {
  for (let o in __cssModules$1E) {
    this[o] = __cssModules$1E[o];
  }
}
component$1E.options.__file = "../zero.Stories/Plugin/components/blocks/modules/headline.vue";
var Headline = component$1E.exports;
var render$1D = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-paragraph"}, [_c("ui-rte", {attrs: {disabled: _vm.disabled, setup: _vm.onSetup, placeholder: "Enter your content here..."}, model: {value: _vm.value.text, callback: function($$v) {
    _vm.$set(_vm.value, "text", $$v);
  }, expression: "value.text"}})], 1);
};
var staticRenderFns$1D = [];
var paragraph_vue_vue_type_style_index_0_lang = "\n.ui-block-paragraph .ui-rte-input\n{\n  padding-top: 0;\n  padding-bottom: 0;\n  max-height: none;\n}\n";
const script$1D = {
  name: "uiBlockParagraph",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    onSetup(config2) {
      config2.removeExtension("heading");
      config2.removeExtension("horizontal_rule");
      config2.removeCommand("heading");
      config2.removeCommand("line");
    }
  }
};
const __cssModules$1D = {};
var component$1D = normalizeComponent(script$1D, render$1D, staticRenderFns$1D, false, injectStyles$1D, null, null, null);
function injectStyles$1D(context) {
  for (let o in __cssModules$1D) {
    this[o] = __cssModules$1D[o];
  }
}
component$1D.options.__file = "../zero.Stories/Plugin/components/blocks/modules/paragraph.vue";
var Paragraph = component$1D.exports;
var render$1C = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  _vm._self._c || _h;
  return _vm._m(0);
};
var staticRenderFns$1C = [function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-line"}, [_c("hr")]);
}];
var line_vue_vue_type_style_index_0_lang = ".ui-block[data-alias=line] {\n  margin-bottom: -24px !important;\n}\n.ui-block[data-alias=line] .ui-blocks-grab-handle,\n.ui-block[data-alias=line] .ui-dropdown-container {\n  top: -15px;\n}\n.ui-block-line hr {\n  margin: 0px 16px;\n  border-bottom: 1px dashed var(--color-line-dashed);\n}";
const script$1C = {
  name: "uiBlockLine",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  }
};
const __cssModules$1C = {};
var component$1C = normalizeComponent(script$1C, render$1C, staticRenderFns$1C, false, injectStyles$1C, null, null, null);
function injectStyles$1C(context) {
  for (let o in __cssModules$1C) {
    this[o] = __cssModules$1C[o];
  }
}
component$1C.options.__file = "../zero.Stories/Plugin/components/blocks/modules/line.vue";
var Line = component$1C.exports;
var render$1B = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-hint", attrs: {"data-severity": _vm.value.severity}}, [_c("button", {staticClass: "ui-block-hint-icon", attrs: {type: "button"}, on: {click: _vm.navigateSeverity}}, [_c("ui-icon", {attrs: {symbol: _vm.icon, size: 22}})], 1), _c("ui-rte", {attrs: {disabled: _vm.disabled, setup: _vm.onSetup, placeholder: "Enter your content here..."}, model: {value: _vm.value.text, callback: function($$v) {
    _vm.$set(_vm.value, "text", $$v);
  }, expression: "value.text"}})], 1);
};
var staticRenderFns$1B = [];
var hint_vue_vue_type_style_index_0_lang = ".ui-block-content.ui-block-hint {\n  background: var(--color-box-nested);\n  margin: 0 16px;\n  border-radius: var(--radius);\n  border-left: 3px solid transparent;\n  padding: 6px 0 8px;\n  display: grid;\n  grid-template-columns: auto minmax(0, 1fr);\n  min-height: 55px !important;\n}\n.ui-block-hint .ui-rte-input {\n  min-height: 38px;\n}\n.ui-block-hint-icon {\n  margin-top: 6px;\n  margin-left: 20px;\n}\n.ui-block-hint[data-severity=info] {\n  border-left-color: var(--color-accent-info);\n}\n.ui-block-hint[data-severity=warning] {\n  border-left-color: var(--color-accent-warn);\n}\n.ui-block-hint[data-severity=error] {\n  border-left-color: var(--color-accent-error);\n}\n.ui-block-hint[data-severity=success] {\n  border-left-color: var(--color-accent-success);\n}\n.ui-block-hint .ui-icon[data-symbol=fth-info],\n.ui-block-hint[data-severity=info] .ui-block-hint-icon {\n  color: var(--color-accent-info);\n}\n.ui-block-hint .ui-icon[data-symbol=fth-alert-circle],\n.ui-block-hint[data-severity=warning] .ui-block-hint-icon {\n  color: var(--color-accent-warn);\n}\n.ui-block-hint .ui-icon[data-symbol=fth-alert-triangle],\n.ui-block-hint[data-severity=error] .ui-block-hint-icon {\n  color: var(--color-accent-error);\n}\n.ui-block-hint .ui-icon[data-symbol=fth-check-circle],\n.ui-block-hint[data-severity=success] .ui-block-hint-icon {\n  color: var(--color-accent-success);\n}";
const icons = {
  info: "fth-info",
  warning: "fth-alert-circle",
  error: "fth-alert-triangle",
  success: "fth-check-circle"
};
const severities = ["info", "warning", "error", "success"];
const script$1B = {
  name: "uiBlockHint",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    icon() {
      return icons[this.value.severity];
    }
  },
  methods: {
    onSetup(config2) {
      config2.removeExtension("heading");
      config2.removeExtension("horizontal_rule");
      config2.removeCommand("heading");
      config2.removeCommand("line");
    },
    navigateSeverity() {
      const idx = severities.indexOf(this.value.severity);
      const newIdx = idx + 1 > severities.length - 1 ? 0 : idx + 1;
      this.value.severity = severities[newIdx];
    }
  }
};
const __cssModules$1B = {};
var component$1B = normalizeComponent(script$1B, render$1B, staticRenderFns$1B, false, injectStyles$1B, null, null, null);
function injectStyles$1B(context) {
  for (let o in __cssModules$1B) {
    this[o] = __cssModules$1B[o];
  }
}
component$1B.options.__file = "../zero.Stories/Plugin/components/blocks/modules/hint.vue";
var Hint = component$1B.exports;
var render$1A = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-quote"}, [_c("ui-mediapicker", {staticClass: "ui-block-quote-upload", attrs: {config: _vm.mediaConfig, disabled: _vm.disabled}, model: {value: _vm.value.imageId, callback: function($$v) {
    _vm.$set(_vm.value, "imageId", $$v);
  }, expression: "value.imageId"}}), _c("ui-rte", {attrs: {disabled: _vm.disabled, setup: _vm.onSetup, placeholder: "Enter your content here..."}, model: {value: _vm.value.text, callback: function($$v) {
    _vm.$set(_vm.value, "text", $$v);
  }, expression: "value.text"}})], 1);
};
var staticRenderFns$1A = [];
var quote_vue_vue_type_style_index_0_lang = ".ui-block-content.ui-block-quote {\n  margin: 0 0 0 16px;\n  display: grid;\n  grid-template-columns: auto minmax(0, 1fr);\n  align-items: center;\n}\n.ui-block-quote .ui-rte-input {\n  color: var(--color-text-dim);\n  font-style: italic;\n  padding-top: 0;\n  padding-bottom: 0;\n  min-height: 0;\n}\n.ui-block-quote .ui-select-button-content,\n.ui-block-quote .ui-mediapicker-preview-text {\n  display: none;\n}\n.ui-block-quote .ui-select-button-icon,\n.ui-block-quote .ui-mediapicker-preview-image {\n  border-radius: 30px;\n  width: 48px;\n  height: 48px;\n}";
const script$1A = {
  name: "uiBlockQuote",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    mediaConfig: {
      limit: 1
    }
  }),
  methods: {
    onSetup(config2) {
      config2.removeExtension("heading");
      config2.removeExtension("horizontal_rule");
      config2.removeCommand("heading");
      config2.removeCommand("line");
      config2.removeCommand("italic");
    }
  }
};
const __cssModules$1A = {};
var component$1A = normalizeComponent(script$1A, render$1A, staticRenderFns$1A, false, injectStyles$1A, null, null, null);
function injectStyles$1A(context) {
  for (let o in __cssModules$1A) {
    this[o] = __cssModules$1A[o];
  }
}
component$1A.options.__file = "../zero.Stories/Plugin/components/blocks/modules/quote.vue";
var Quote = component$1A.exports;
var PageTreeApi = {
  getChildren: async (parent, active, search) => await get$1("pageTree/getChildren", {params: {parent, active, search}})
};
const YOUTUBE_REGEX = /youtu(?:\.be|be\.com)\/(?:.*v(?:\/|=)|(?:.*\/)?)([a-zA-Z0-9-_]+)/gim;
const VIMEO_REGEX = /vimeo\.com\/(\d+)($|\/)/gim;
let getVideoId = (url) => {
  if (!url) {
    return null;
  }
  if (url.indexOf("vimeo.com") > -1) {
    let matches = VIMEO_REGEX.exec(url);
    return matches && matches[1];
  } else if (url.indexOf("youtu") > -1) {
    let matches = YOUTUBE_REGEX.exec(url);
    return matches && matches[1];
  }
  return null;
};
let getVimeoMetadata = async (id) => {
  let result = {
    id,
    url: `https://vimeo.com/${id}`,
    success: false
  };
  let response = await fetch(`https://vimeo.com/api/v2/video/${id}.json`);
  if (response.ok) {
    let data = await response.json();
    result.data = data[0];
    result.image = result.data.thumbnail_large;
    result.title = result.data.title;
    result.description = result.data.description;
    result.success = true;
  }
  return result;
};
let getYoutubeMetadata = async (id) => {
  const apiKey = __zero.services.youTubeApiKey;
  let result = {
    id,
    image: `https://i3.ytimg.com/vi/${id}/maxresdefault.jpg`,
    url: `https://www.youtube.com/watch?v=${id}`,
    success: true
  };
  if (apiKey) {
    result.success = false;
    let response = await fetch(`https://www.googleapis.com/youtube/v3/videos?part=snippet&id=${id}&key=${apiKey}`);
    if (response.ok) {
      let data = await response.json();
      if (data && data.items && data.items.length) {
        result.data = data.items[0].snippet;
        let thumbs = Object.values(result.data.thumbnails);
        let thumb = thumbs[thumbs.length - 1];
        result.image = thumb.url;
        result.title = result.data.title;
        result.description = result.data.description;
        result.success = true;
      }
    }
    return result;
  } else {
    return {
      id,
      image: `https://i3.ytimg.com/vi/${id}/maxresdefault.jpg`,
      url: `https://www.youtube.com/watch?v=${id}`,
      success: true
    };
  }
};
var render$1z = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "ui-videpicker-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@videopicker.headline", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.config.closeLabel, parent: _vm.config.rootId}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.save", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_vm.opened ? _c("div", [_c("div", {staticClass: "ui-box ui-videpicker-overlay-options"}, [_c("ui-property", {attrs: {label: "Provider", vertical: true}}, [_c("ui-select", {attrs: {items: _vm.providers}, model: {value: _vm.item.provider, callback: function($$v) {
    _vm.$set(_vm.item, "provider", $$v);
  }, expression: "item.provider"}})], 1), _c("ui-property", {attrs: {label: "Video URL", vertical: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.item.videoUrl, expression: "item.videoUrl"}], staticClass: "ui-input", attrs: {type: "text"}, domProps: {value: _vm.item.videoUrl}, on: {input: function($event) {
    if ($event.target.composing) {
      return;
    }
    _vm.$set(_vm.item, "videoUrl", $event.target.value);
  }}}), _vm.item.videoId ? _c("p", {staticClass: "ui-property-help"}, [_vm._v("Found video ID is "), _c("b", [_vm._v(_vm._s(_vm.item.videoId))])]) : _vm._e()])], 1), _vm.preview ? _c("div", {staticClass: "ui-box"}, [_c("div", [_c("img", {attrs: {src: _vm.preview.image}}), _c("p", [_c("strong", [_vm._v(_vm._s(_vm.preview.title))]), _c("br"), _vm._v(" " + _vm._s(_vm.preview.description) + " ")])])]) : _vm._e()]) : _vm._e()]);
};
var staticRenderFns$1z = [];
var overlay_vue_vue_type_style_index_0_lang$6 = ".ui-videpicker-overlay content {\n  padding-top: 0;\n}\n.ui-videpicker-overlay-options .ui-property {\n  display: flex;\n  justify-content: space-between;\n}\n.ui-videpicker-overlay-options .ui-property + .ui-property {\n  margin-top: var(--padding-m);\n}\n.ui-videpicker-overlay-options .ui-property-content {\n  display: inline;\n  flex: 0 0 auto;\n}\n.ui-videpicker-overlay-options .ui-property-label {\n  padding-top: 1px;\n}";
const PROVIDERS = [
  {value: "@videopicker.providers.html", key: "html"},
  {value: "@videopicker.providers.youtube", key: "youtube"},
  {value: "@videopicker.providers.vimeo", key: "vimeo"}
];
const script$1z = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    opened: false,
    state: "default",
    providers: PROVIDERS,
    item: null,
    videoIdParsing: false,
    template: {
      provider: "youtube",
      videoId: null,
      videoUrl: null,
      videoPreviewImageUrl: null,
      title: null,
      previewImageId: null
    },
    preview: null,
    debouncedReloadPreview: null
  }),
  watch: {
    "item.videoUrl": function(val) {
      this.parseUrl(val);
    },
    "item.provider": function(val) {
      if (this.opened) {
        this.item.videoId = null;
        this.item.videoUrl = null;
        this.item.videoPreviewImageUrl = null;
        this.preview = null;
      }
    }
  },
  mounted() {
    this.debouncedReloadPreview = debounce(this.reloadPreview, 300);
    this.item = JSON.parse(JSON.stringify(this.model || this.template));
    setTimeout(() => {
      this.opened = true;
      if (this.model && this.model.provider !== "html" && this.model.videoId) {
        this.reloadPreview(this.model.videoId);
      }
    }, 300);
  },
  methods: {
    parseUrl(url) {
      this.item.videoId = getVideoId(url);
      this.debouncedReloadPreview(this.item.videoId);
    },
    reloadPreview(id) {
      if (!id) {
        this.preview = null;
        return;
      }
      if (this.item.provider === "vimeo") {
        getVimeoMetadata(id).then((res) => {
          this.preview = res.success ? res : null;
          this.handlePreview(this.preview);
        });
      } else {
        getYoutubeMetadata(id).then((res) => {
          this.preview = res.success ? res : null;
          this.handlePreview(this.preview);
        });
      }
    },
    handlePreview(preview) {
      if (!preview) {
        return;
      }
      this.item.title = this.preview.title;
      this.item.videoPreviewImageUrl = this.preview.image;
    },
    onSave() {
      this.config.confirm(this.item);
    }
  }
};
const __cssModules$1z = {};
var component$1z = normalizeComponent(script$1z, render$1z, staticRenderFns$1z, false, injectStyles$1z, null, null, null);
function injectStyles$1z(context) {
  for (let o in __cssModules$1z) {
    this[o] = __cssModules$1z[o];
  }
}
component$1z.options.__file = "app/components/pickers/videoPicker/overlay.vue";
var LinkpickerOverlay$1 = component$1z.exports;
var render$1y = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("button", {staticClass: "ui-block-video", attrs: {type: "button"}, on: {click: _vm.open}}, [!_vm.value.video ? _c("div", {staticClass: "ui-block-video-add"}, [_c("ui-icon", {attrs: {symbol: "fth-video", size: 32}}), _vm._m(0)], 1) : _vm._e(), _vm.video ? _c("figure", {staticClass: "ui-block-video-preview", attrs: {"data-provider": _vm.video && _vm.video.provider}}, [_c("img", {attrs: {src: _vm.image}}), _c("div", {staticClass: "ui-block-video-preview-play"}, [_c("ui-icon", {attrs: {symbol: "fth-play", size: 24}})], 1), _c("figcaption", {staticClass: "ui-block-video-preview-caption"}, [_c("p", {staticClass: "ui-block-video-preview-text"}, [_c("strong", [_vm._v(_vm._s(_vm.video.title))])]), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@videopicker.providers." + _vm.video.provider, expression: "'@videopicker.providers.' + video.provider"}], staticClass: "ui-block-video-preview-provider"})])]) : _vm._e()]);
};
var staticRenderFns$1y = [function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("p", [_c("b", [_vm._v("Add a video")])]);
}];
var video_vue_vue_type_style_index_0_lang = ".ui-block-content.ui-block-video {\n  margin: 0 16px;\n}\n.ui-block-video-add {\n  width: 100%;\n  height: 300px;\n  /*background: var(--color-box-nested);*/\n  border: 1px dashed var(--color-line-dashed);\n  border-radius: var(--radius);\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n}\n.ui-block-video-preview {\n  padding: 0;\n  margin: 0;\n  display: block;\n  position: relative;\n}\n.ui-block-video-preview-caption {\n  padding: var(--padding-s) var(--padding-m);\n  position: absolute;\n  bottom: 0;\n  left: 0;\n  background: rgba(0, 0, 0, 0.3);\n  width: 100%;\n  border-radius: var(--radius);\n  display: grid;\n  grid-template-columns: minmax(0, 1fr) auto;\n  grid-gap: 20px;\n  align-items: center;\n}\n.ui-block-video-preview img {\n  border-radius: var(--radius);\n  width: 100%;\n}\n.ui-block-video-preview-text {\n  margin: 0;\n  color: white;\n  font-size: var(--font-size-s);\n}\n.ui-block-video-preview-provider {\n  display: flex;\n  background: rgba(0, 0, 0, 0.3);\n  color: white;\n  height: 24px;\n  align-items: center;\n  padding: 0 10px;\n  border-radius: 12px;\n  font-size: var(--font-size-xs);\n}\n.ui-block-video-preview-play {\n  position: absolute;\n  left: 50%;\n  top: 50%;\n  background: white;\n  width: 60px;\n  height: 60px;\n  border-radius: 30px;\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  margin: -30px 0 0 -30px;\n}\n.ui-block-video-preview-play .ui-icon {\n  position: relative;\n  left: 2px;\n}\n[data-provider=vimeo] .ui-block-video-preview-provider {\n  background: #1AB7EA;\n}\n[data-provider=youtube] .ui-block-video-preview-provider {\n  background: #E62117;\n}";
const script$1y = {
  name: "uiBlockVideo",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    methods: {
      type: Object,
      required: true
    }
  },
  data: () => ({}),
  computed: {
    video() {
      return this.value && this.value.video;
    },
    image() {
      return this.value && this.value.video ? this.video.previewImageId ? MediaApi.getImageSource(this.video.previewImageId) : this.video.videoPreviewImageUrl : null;
    }
  },
  methods: {
    open(initial) {
      Overlay.open({
        title: "Select a video",
        closeLabel: "@ui.close",
        component: LinkpickerOverlay$1,
        display: "editor",
        model: this.value.video
      }).then((value) => {
        this.value.video = value;
        this.$emit("input", this.value);
      }).catch((err) => {
        this.methods.removeBlock(this.value);
      });
    },
    onBlockAdded() {
      this.open(true);
    }
  }
};
const __cssModules$1y = {};
var component$1y = normalizeComponent(script$1y, render$1y, staticRenderFns$1y, false, injectStyles$1y, null, null, null);
function injectStyles$1y(context) {
  for (let o in __cssModules$1y) {
    this[o] = __cssModules$1y[o];
  }
}
component$1y.options.__file = "../zero.Stories/Plugin/components/blocks/modules/video.vue";
var Video = component$1y.exports;
var render$1x = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-file"}, [_c("ui-mediapicker", {ref: "picker", staticClass: "ui-block-file-upload", attrs: {config: _vm.mediaConfig, disabled: _vm.disabled}, on: {previews: _vm.previewUpdated}, model: {value: _vm.value.mediaId, callback: function($$v) {
    _vm.$set(_vm.value, "mediaId", $$v);
  }, expression: "value.mediaId"}}), _vm.preview ? _c("div", {staticClass: "ui-block-file-preview"}, [_c("a", {staticClass: "ui-block-file-preview-download", attrs: {href: _vm.preview.source, download: ""}}, [_c("ui-icon", {attrs: {symbol: "fth-download", size: 22}})], 1), _c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.value.caption, expression: "value.caption"}], staticClass: "ui-block-file-preview-input", attrs: {type: "text", placeholder: "Enter a title..."}, domProps: {value: _vm.value.caption}, on: {input: function($event) {
    if ($event.target.composing) {
      return;
    }
    _vm.$set(_vm.value, "caption", $event.target.value);
  }}}), _c("p", {staticClass: "ui-block-file-preview-info"}, [_vm._v(_vm._s(_vm.preview.filename) + " - " + _vm._s(_vm.preview.size))])]) : _vm._e()], 1);
};
var staticRenderFns$1x = [];
var file_vue_vue_type_style_index_0_lang = ".ui-block-file {\n  margin: 0 16px;\n}\n.ui-block-file .ui-mediapicker-previews {\n  display: none;\n}\n.ui-block-file-upload {\n  visibility: hidden;\n}\n.ui-block-file-preview {\n  border-radius: var(--radius);\n  border: 1px solid var(--color-line);\n  padding: 6px var(--padding-m);\n  display: grid;\n  grid-template-columns: auto minmax(250px, 1fr) minmax(0, auto);\n  grid-gap: 10px;\n  min-height: 52px !important;\n  align-items: center;\n}\na.ui-block-file-preview-download {\n  position: relative;\n  top: 2px;\n  color: var(--color-primary);\n}\n.ui-block-file-preview-info {\n  font-size: var(--font-size-s);\n  color: var(--color-text-dim);\n  text-align: right;\n}\ninput[type=text].ui-block-file-preview-input {\n  font-weight: 600;\n  font-size: var(--font-size-m);\n  background: none;\n}\ninput[type=text].ui-block-file-preview-input:focus {\n  border-color: transparent !important;\n  background: none !important;\n  box-shadow: none !important;\n}\n.ui-block[data-alias=file] + .ui-block[data-alias=file] {\n  margin-top: var(--padding-s);\n}";
const script$1x = {
  name: "uiBlockFile",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    methods: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    mediaConfig: {
      limit: 1
    },
    preview: null
  }),
  methods: {
    previewUpdated(model) {
      if (!model) {
        this.preview = null;
        return;
      }
      let pathParts = model.source.split("/");
      this.preview = {
        filename: pathParts[pathParts.length - 1],
        size: Strings.filesize(model.size),
        source: model.source
      };
    },
    onBlockAdded() {
      this.$refs.picker.pick().catch(() => {
        this.methods.removeBlock(this.value);
      });
    }
  }
};
const __cssModules$1x = {};
var component$1x = normalizeComponent(script$1x, render$1x, staticRenderFns$1x, false, injectStyles$1x, null, null, null);
function injectStyles$1x(context) {
  for (let o in __cssModules$1x) {
    this[o] = __cssModules$1x[o];
  }
}
component$1x.options.__file = "../zero.Stories/Plugin/components/blocks/modules/file.vue";
var File = component$1x.exports;
var render$1w = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-media"}, [_vm.previews.length > 0 ? _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated}, expression: "{ onUpdate: onSortingUpdated }"}], staticClass: "ui-block-media-previews", attrs: {"data-count": _vm.previews.length}}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {key: preview.id, staticClass: "ui-block-media-preview"}, [!preview.error ? _c("div", {staticClass: "ui-block-media-preview-image", class: {"media-pattern": preview.thumbnailSource}}, [preview.thumbnailSource ? _c("img", {attrs: {src: preview.source, alt: preview.name}}) : _vm._e(), !_vm.disabled ? _c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.remove", expression: "'@ui.remove'", arg: "title"}], staticClass: "ui-mediapicker-preview-image-delete", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.$refs.mediapicker.remove(preview);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-x", size: 12}})], 1) : _vm._e(), !_vm.disabled ? _c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.edit", expression: "'@ui.edit'", arg: "title"}], staticClass: "ui-mediapicker-preview-image-edit", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.$refs.mediapicker.edit(preview);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-edit-2", size: 12}})], 1) : _vm._e()]) : _vm._e()]);
  }), 0) : _vm._e(), _vm.value.mediaIds.length < 1 ? _c("button", {staticClass: "ui-block-media-add", attrs: {type: "button"}, on: {click: _vm.add}}, [_c("ui-icon", {attrs: {symbol: "fth-image", size: 32}}), _vm._m(0)], 1) : _vm._e(), _c("ui-mediapicker", {ref: "mediapicker", staticClass: "ui-block-media-upload", attrs: {config: _vm.mediaConfig, disabled: _vm.disabled}, on: {previews: _vm.previewsUpdated}, model: {value: _vm.value.mediaIds, callback: function($$v) {
    _vm.$set(_vm.value, "mediaIds", $$v);
  }, expression: "value.mediaIds"}})], 1);
};
var staticRenderFns$1w = [function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("p", [_c("b", [_vm._v("Add images")])]);
}];
var media_vue_vue_type_style_index_0_lang = '.ui-block-content.ui-block-media {\n  margin: 0 16px;\n  /*display: grid;\n  grid-template-columns: auto minmax(0, 1fr);\n  align-items: center;*/\n}\n.ui-block-media-add {\n  width: 100%;\n  height: 300px;\n  /*background: var(--color-box-nested);*/\n  border: 1px dashed var(--color-line-dashed);\n  border-radius: var(--radius);\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n}\n.ui-block-media-previews + .ui-block-media-add {\n  margin-top: var(--padding-s);\n}\n.ui-block-media .ui-mediapicker-previews,\n.ui-block-media .ui-mediapicker-select {\n  display: none;\n}\n.ui-block-media-previews {\n  display: grid;\n  grid-gap: var(--padding-s);\n}\n.ui-block-media-previews:not([data-count="1"]) {\n  grid-template-columns: repeat(3, minmax(0, 1fr));\n}\n.ui-block-media-previews[data-count="2"] {\n  grid-template-columns: repeat(2, minmax(0, 1fr));\n}\n.ui-block-media-previews:not([data-count="1"]) .ui-block-media-preview img,\n.ui-block-media-previews:not([data-count="1"]) .ui-block-media-preview-image {\n  width: 100%;\n  height: 100%;\n  object-fit: cover;\n}\n.ui-block-media-preview {\n  text-align: center;\n}\n.ui-block-media-preview img {\n  max-width: 100%;\n  border-radius: var(--radius);\n  z-index: 1;\n  position: relative;\n}\n.ui-block-media-preview-image:hover .ui-mediapicker-preview-image-delete,\n.ui-block-media-preview-image:hover .ui-mediapicker-preview-image-edit {\n  opacity: 1;\n  transition-delay: 0.1s;\n}\n.ui-block[data-alias=media] + .ui-block[data-alias=media] {\n  margin-top: var(--padding-s);\n}';
const script$1w = {
  name: "uiBlockMedia",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    mediaConfig: {
      limit: 16
    },
    previews: []
  }),
  methods: {
    previewsUpdated(model) {
      this.previews = model;
    },
    add() {
      this.$refs.mediapicker.pick();
    },
    onSortingUpdated(ev) {
      this.value.mediaIds = Arrays.move(this.value.mediaIds, ev.oldIndex, ev.newIndex);
    }
  }
};
const __cssModules$1w = {};
var component$1w = normalizeComponent(script$1w, render$1w, staticRenderFns$1w, false, injectStyles$1w, null, null, null);
function injectStyles$1w(context) {
  for (let o in __cssModules$1w) {
    this[o] = __cssModules$1w[o];
  }
}
component$1w.options.__file = "../zero.Stories/Plugin/components/blocks/modules/media.vue";
var Media = component$1w.exports;
var render$1v = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-link"}, [_c("ui-linkpicker", {ref: "picker", staticClass: "ui-block-link-upload", attrs: {config: _vm.pickerConfig, disabled: _vm.disabled}, on: {previews: _vm.previewUpdated}, model: {value: _vm.value.link, callback: function($$v) {
    _vm.$set(_vm.value, "link", $$v);
  }, expression: "value.link"}}), _vm.preview ? _c("div", {staticClass: "ui-block-link-preview"}, [_c("a", {staticClass: "ui-block-link-preview-download", attrs: {href: _vm.preview.url, target: "_blank"}}, [_c("ui-icon", {attrs: {symbol: "fth-arrow-right", size: 22}})], 1), _c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.value.caption, expression: "value.caption"}], staticClass: "ui-block-link-preview-input", attrs: {type: "text", placeholder: _vm.preview.name}, domProps: {value: _vm.value.caption}, on: {input: function($event) {
    if ($event.target.composing) {
      return;
    }
    _vm.$set(_vm.value, "caption", $event.target.value);
  }}}), _c("p", {staticClass: "ui-block-link-preview-info"}, [_vm._v(_vm._s(_vm.preview.url))])]) : _vm._e()], 1);
};
var staticRenderFns$1v = [];
var link_vue_vue_type_style_index_0_lang = ".ui-block-link {\n  margin: 0 16px;\n}\n.ui-block-link .ui-linkpicker-previews {\n  display: none;\n}\n.ui-block-link-preview {\n  border-radius: var(--radius);\n  border: 1px solid var(--color-line);\n  padding: 6px var(--padding-m);\n  display: grid;\n  grid-template-columns: auto minmax(250px, 1fr) minmax(0, auto);\n  grid-gap: 10px;\n  min-height: 52px !important;\n  align-items: center;\n}\na.ui-block-link-preview-download {\n  position: relative;\n  top: 2px;\n  color: var(--color-primary);\n}\n.ui-block-link-preview-info {\n  font-size: var(--font-size-s);\n  color: var(--color-text-dim);\n  text-align: right;\n}\ninput[type=text].ui-block-link-preview-input {\n  font-weight: 600;\n  font-size: var(--font-size-m);\n  background: none;\n}\ninput[type=text].ui-block-link-preview-input:focus {\n  border-color: transparent !important;\n  background: none !important;\n  box-shadow: none !important;\n}\n.ui-block[data-alias=file] + .ui-block[data-alias=file] {\n  margin-top: var(--padding-s);\n}";
const script$1v = {
  name: "uiBlockLink",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    methods: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    pickerConfig: {
      limit: 1
    },
    preview: null
  }),
  methods: {
    previewUpdated(model) {
      if (!model) {
        this.preview = null;
        return;
      }
      if (!this.value.caption && model.name) {
        this.value.caption = model.name;
      }
      this.preview = {
        icon: model.icon,
        url: model.text,
        name: model.name
      };
    },
    onBlockAdded() {
      this.$refs.picker.pick().then((res) => {
        if (!res) {
          this.methods.removeBlock(this.value);
        }
      }).catch((err) => {
        this.methods.removeBlock(this.value);
      });
    }
  }
};
const __cssModules$1v = {};
var component$1v = normalizeComponent(script$1v, render$1v, staticRenderFns$1v, false, injectStyles$1v, null, null, null);
function injectStyles$1v(context) {
  for (let o in __cssModules$1v) {
    this[o] = __cssModules$1v[o];
  }
}
component$1v.options.__file = "../zero.Stories/Plugin/components/blocks/modules/link.vue";
var Link = component$1v.exports;
var render$1u = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-button"}, [_c("ui-linkpicker", {ref: "picker", staticClass: "ui-block-button-upload", attrs: {config: _vm.pickerConfig, disabled: _vm.disabled}, on: {previews: _vm.previewUpdated}, model: {value: _vm.value.link, callback: function($$v) {
    _vm.$set(_vm.value, "link", $$v);
  }, expression: "value.link"}}), _vm.preview ? _c("div", {staticClass: "ui-button ui-block-button-preview"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.value.caption, expression: "value.caption"}], staticClass: "ui-block-button-preview-input", attrs: {type: "text", placeholder: _vm.preview.name}, domProps: {value: _vm.value.caption}, on: {input: function($event) {
    if ($event.target.composing) {
      return;
    }
    _vm.$set(_vm.value, "caption", $event.target.value);
  }}})]) : _vm._e()], 1);
};
var staticRenderFns$1u = [];
var button_vue_vue_type_style_index_0_lang = ".ui-block-button {\n  margin: 0 16px;\n}\n.ui-block-button .ui-linkpicker-previews {\n  display: none;\n}\ninput[type=text].ui-block-button-preview-input {\n  font-weight: 600;\n  font-size: var(--font-size-m);\n  background: none;\n  color: var(--color-primary-text);\n  min-width: 1px !important;\n  padding: 0;\n  border: none;\n  text-indent: 0;\n}\ninput[type=text].ui-block-button-preview-input:focus {\n  border: none !important;\n  background: none !important;\n  box-shadow: none !important;\n}";
const script$1u = {
  name: "uiBlockButton",
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    methods: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    pickerConfig: {
      limit: 1
    },
    preview: null
  }),
  methods: {
    previewUpdated(model) {
      if (!model) {
        this.preview = null;
        return;
      }
      if (!this.value.caption && model.name) {
        this.value.caption = model.name;
      }
      this.preview = {
        icon: model.icon,
        url: model.text,
        name: model.name
      };
    },
    onBlockAdded() {
      this.$refs.picker.pick().then((res) => {
        if (!res) {
          this.methods.removeBlock(this.value);
        }
      }).catch((err) => {
        this.methods.removeBlock(this.value);
      });
    }
  }
};
const __cssModules$1u = {};
var component$1u = normalizeComponent(script$1u, render$1u, staticRenderFns$1u, false, injectStyles$1u, null, null, null);
function injectStyles$1u(context) {
  for (let o in __cssModules$1u) {
    this[o] = __cssModules$1u[o];
  }
}
component$1u.options.__file = "../zero.Stories/Plugin/components/blocks/modules/button.vue";
var Button = component$1u.exports;
var blocks = [
  {
    alias: "stories.headline",
    component: Headline,
    actions: [
      {
        label: "Large",
        action: (item2) => item2.size = "h1"
      },
      {
        label: "Medium",
        action: (item2) => item2.size = "h2"
      },
      {
        label: "Small",
        action: (item2) => item2.size = "h3"
      }
    ]
  },
  {
    alias: "stories.paragraph",
    component: Paragraph
  },
  {
    alias: "stories.line",
    component: Line
  },
  {
    alias: "stories.hint",
    component: Hint,
    actions: [
      {
        label: "Info",
        icon: "fth-info",
        action: (item2) => item2.severity = "info"
      },
      {
        label: "Warning",
        icon: "fth-alert-circle",
        action: (item2) => item2.severity = "warning"
      },
      {
        label: "Error",
        icon: "fth-alert-triangle",
        action: (item2) => item2.severity = "error"
      },
      {
        label: "Success",
        icon: "fth-check-circle",
        action: (item2) => item2.severity = "success"
      }
    ]
  },
  {
    alias: "stories.quote",
    component: Quote
  },
  {
    alias: "stories.video",
    component: Video,
    visible: (item2) => item2.video != null
  },
  {
    alias: "stories.file",
    component: File,
    visible: (item2) => item2.mediaId != null
  },
  {
    alias: "stories.media",
    component: Media,
    actions: [
      {
        label: "Add images",
        icon: "fth-plus",
        action: (item2, cmp) => cmp.add()
      }
    ]
  },
  {
    alias: "stories.link",
    component: Link,
    visible: (item2) => item2.link != null
  },
  {
    alias: "stories.button",
    component: Button,
    visible: (item2) => item2.link != null
  }
];
const plugin$2 = new Plugin("zero.stories");
const config = {
  modules: blocks
};
plugin$2.addEditors(editors$2);
plugin$2.addLists(lists$2);
plugin$2.addRoutes(routes$2);
plugin$2.install = (vue, zero2) => {
  zero2.config.linkPicker.areas.push({
    alias: "zero.stories",
    name: "@stories.name",
    component: () => __vitePreload(() => __import__("./linkpicker-area.js"), true ? ["/zero/linkpicker-area.js","/zero/linkpicker-area.css","/zero/vendor.js"] : void 0)
  });
  Object.defineProperty(zero2, "stories", {
    get: () => config
  });
};
const editor$s = new Editor("form", "@forms.form.fields.");
editor$s.tab("fields", "@forms.form.tabs.editor");
editor$s.tab("entries", "@forms.form.tabs.entries");
const options = editor$s.tab("options", "@ui.tab_options");
options.field("persistEntries").toggle();
options.field("isModerated").toggle();
var editors$1 = {
  form: editor$s
};
var FormsApi = __assign(__assign({}, collection$1("forms/")), {
  getListByQuery: async (query, config2) => await get$1("forms/getListByQuery", __assign(__assign({}, config2), {params: {query}}))
});
const list$8 = new List("forms");
list$8.templateLabel = (x) => "@forms.form.fields." + x;
list$8.link = "forms-edit";
list$8.onFetch((filter2) => FormsApi.getByQuery(filter2));
list$8.column("name").name();
list$8.column("date").date();
list$8.column("isActive").active();
var lists$1 = {
  forms: list$8
};
const alias = "forms";
const section = __zero.sections.find((s) => s.alias === alias);
let routes$1 = [];
if (section) {
  routes$1.push({
    name: alias,
    path: "/" + alias,
    component: () => __vitePreload(() => __import__("./dashboard3.js"), true ? ["/zero/dashboard3.js","/zero/dashboard.css","/zero/vendor.js"] : void 0)
  });
  routes$1.push({
    name: alias + "-create",
    path: "/" + alias + "/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./form.js"), true ? ["/zero/form.js","/zero/form.css","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: section.name
    }
  });
  routes$1.push({
    name: alias + "-edit",
    path: "/" + alias + "/edit/:id",
    props: true,
    component: () => __vitePreload(() => __import__("./form.js"), true ? ["/zero/form.js","/zero/form.css","/zero/vendor.js"] : void 0),
    meta: {
      name: section.name
    }
  });
}
const plugin$1 = new Plugin("zero.forms");
plugin$1.addEditors(editors$1);
plugin$1.addLists(lists$1);
plugin$1.addRoutes(routes$1);
plugin$1.install = (vue, zero2) => {
};
const editor$r = new Editor("spaces.team", "@laola.spaces.team.fields.");
editor$r.field("name", {label: "@ui.name"}).text(120).required();
editor$r.field("position").text(30).required();
editor$r.field("imageId").image();
editor$r.field("email").text(60);
var render$1t = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-labelcalulator-prices"}, [_vm._l(_vm.items, function(item2) {
    return _c("div", {staticClass: "laola-labelcalulator-price"}, [_c("ui-select", {attrs: {entity: _vm.entity, items: _vm.options, disabled: _vm.disabled}, model: {value: item2.printType, callback: function($$v) {
      _vm.$set(item2, "printType", $$v);
    }, expression: "item.printType"}}), _c("div", {staticClass: "laola-labelcalulator-price-extra"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: item2.price, expression: "item.price"}], staticClass: "ui-input", attrs: {type: "text", disabled: _vm.disabled}, domProps: {value: item2.price}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "price", $event.target.value);
    }}})]), !_vm.disabled ? _c("ui-icon-button", {attrs: {type: "light", icon: "fth-x"}, on: {click: function($event) {
      return _vm.removeItem(item2);
    }}}) : _vm._e()], 1);
  }), !_vm.disabled ? _c("ui-button", {attrs: {type: "light", label: "@ui.add"}, on: {click: _vm.addItem}}) : _vm._e()], 2);
};
var staticRenderFns$1t = [];
var labelcalculatorPrices_vue_vue_type_style_index_0_lang = ".laola-labelcalulator-price {\n  display: grid;\n  grid-template-columns: 70fr 30fr auto;\n  background: var(--color-input);\n  border-radius: var(--radius);\n}\n.laola-labelcalulator-price + .laola-labelcalulator-price, .laola-labelcalulator-price + .ui-button {\n  margin-top: 6px;\n}\n.laola-labelcalulator-price .ui-input {\n  border-right: none;\n}\n.laola-labelcalulator-price .ui-icon-button {\n  border-radius: 0 var(--radius) var(--radius) 0;\n  height: 48px;\n  width: 48px;\n  border-left: none;\n  background: transparent !important;\n}\n.laola-labelcalulator-price-extra {\n  display: flex;\n  align-items: stretch;\n}\n.laola-labelcalulator-price .ui-select-button-label {\n  display: none;\n}\n.laola-labelcalulator-price .ui-select-button-description {\n  color: var(--color-text);\n  font-size: var(--font-size);\n  margin-top: 0;\n}\n.laola-labelcalulator-price .ui-select-button-icon {\n  width: 24px;\n  height: 24px;\n  line-height: 24px !important;\n  font-size: 14px;\n  margin-left: 8px;\n}";
const TEMPLATE = {
  printType: "digital",
  price: 0
};
const script$1t = {
  props: {
    value: {
      type: Array,
      default: () => []
    },
    entity: {
      type: Object,
      default: () => {
      }
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    items: [],
    options: [
      {value: "@laola.modules.labelcalculator.print_types.digital", key: "digital"},
      {value: "@laola.modules.labelcalculator.print_types.flex", key: "flex"},
      {value: "@laola.modules.labelcalculator.print_types.sublimation", key: "sublimation"},
      {value: "@laola.modules.labelcalculator.print_types.silkscreen", key: "silkscreen"},
      {value: "@laola.modules.labelcalculator.print_types.stick", key: "stick"}
    ]
  }),
  watch: {
    value(val) {
      this.rebuild();
    }
  },
  created() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.items = [];
      if (this.value) {
        this.value.forEach((item2) => {
          this.items.push(item2);
        });
      }
    },
    onChange() {
      this.$emit("input", this.items);
    },
    addItem() {
      this.items.push(clone(TEMPLATE));
      this.onChange();
    },
    removeItem(item2) {
      const index = this.items.indexOf(item2);
      this.items.splice(index, 1);
      this.onChange();
    }
  }
};
const __cssModules$1t = {};
var component$1t = normalizeComponent(script$1t, render$1t, staticRenderFns$1t, false, injectStyles$1t, null, null, null);
function injectStyles$1t(context) {
  for (let o in __cssModules$1t) {
    this[o] = __cssModules$1t[o];
  }
}
component$1t.options.__file = "../../Laola/Laola.Backoffice/Plugin/components/labelcalculator-prices.vue";
var LabelCalculatorPrices = component$1t.exports;
const itemEditor = new Editor("module.page.labelcalculator.item", "@laola.modules.labelcalculator.fields.item.");
itemEditor.field("name").text(80).required();
itemEditor.field("description").text(200);
itemEditor.field("prices").custom(LabelCalculatorPrices).required();
const editor$q = new Editor("module.page.labelcalculator", "@laola.modules.labelcalculator.fields.");
editor$q.field("items").nested(itemEditor, {
  limit: 30,
  title: "@laola.modules.labelcalculator.itemHeadline",
  itemLabel: (x) => x.name,
  itemDescription: (x) => x.description,
  itemIcon: "fth-edit-2",
  template: {
    name: null,
    description: null,
    prices: []
  }
}).required();
var render$1s = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "storypicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$1s = [];
const script$1s = {
  name: "storypicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "story",
      items: StoriesApi.getForPicker,
      previews: StoriesApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true,
        iconAsImage: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1s = {};
var component$1s = normalizeComponent(script$1s, render$1s, staticRenderFns$1s, false, injectStyles$1s, null, null, null);
function injectStyles$1s(context) {
  for (let o in __cssModules$1s) {
    this[o] = __cssModules$1s[o];
  }
}
component$1s.options.__file = "../zero.Stories/Plugin/components/storypicker.vue";
var StoryPicker = component$1s.exports;
const COLORS = [
  {value: "@laola.modules.colors.light", key: "light"},
  {value: "@laola.modules.colors.white", key: "white"},
  {value: "@laola.modules.colors.primary", key: "primary"}
];
const addTextblocks = (editor2, limit) => {
  const textblockItem = new Editor("module.page.textblock.partial", "@laola.modules.textblockpartial.");
  textblockItem.field("title").text(60).required();
  textblockItem.field("text").rte().required();
  textblockItem.field("icon").iconPicker();
  textblockItem.field("badge").text(8);
  textblockItem.field("link").linkPicker();
  editor2.field("items").nested(textblockItem, {
    limit,
    title: "@laola.modules.textblockpartial.editor_headline",
    itemLabel: (x) => x.title,
    itemDescription: (x) => x.text,
    itemIcon: "fth-align-left",
    template: {
      title: null,
      text: null,
      badge: null,
      icon: null,
      link: null
    }
  }).required();
};
const addFigureblocks = (editor2, limit) => {
  const item2 = new Editor("module.page.figureblock.partial", "@laola.modules.figureblockpartial.");
  item2.field("title").text(60).required();
  item2.field("text").rte().required();
  item2.field("imageId").image().required();
  item2.field("link").linkPicker();
  item2.field("isTextWhite").toggle();
  editor2.field("items").nested(item2, {
    limit,
    title: "@laola.modules.figureblockpartial.editor_headline",
    itemLabel: (x) => x.title,
    itemDescription: (x) => x.text,
    itemIcon: "fth-align-left",
    template: {
      title: null,
      text: null,
      imageId: null,
      isTextWhite: false,
      link: null
    }
  }).required();
};
const text$1 = new Editor("module.page.text", "@laola.modules.text.fields.");
text$1.field("text").rte().required();
text$1.field("isBlank").toggle();
text$1.preview('<ui-module-preview-text :text="model.text" />');
const headline = new Editor("module.page.headline", "@laola.modules.headline.fields.");
headline.field("text").text(80).required();
headline.field("subline").rte();
headline.preview('<ui-module-preview-text :text="model.text" :subline="model.subline" />');
const image = new Editor("module.page.image", "@laola.modules.image.fields.");
image.field("imageId").image().required();
image.field("isFull").toggle();
image.field("text").text(40);
image.field("subline").text(80);
image.field("link").text(80);
image.preview('<ui-module-preview-figure :image="model.imageId" :text="model.text" :subline="model.subline" />');
const figure = new Editor("module.page.figure", "@laola.modules.figure.fields.");
const FIGURE_LAYOUTS = [
  {value: "@laola.modules.figure.layouts.imageBigLeft", key: "imageBigLeft"},
  {value: "@laola.modules.figure.layouts.imageSmallLeft", key: "imageSmallLeft"},
  {value: "@laola.modules.figure.layouts.imageBigRight", key: "imageBigRight"},
  {value: "@laola.modules.figure.layouts.imageSmallRight", key: "imageSmallRight"}
];
figure.field("imageId").image().required();
figure.field("layout").select(FIGURE_LAYOUTS);
figure.field("headline").text(40).required();
figure.field("text").rte();
figure.field("link").linkPicker();
figure.field("isPrimary").toggle();
figure.preview('<ui-module-preview-figure :image="model.imageId" :text="model.headline" :subline="model.text" />');
const accordionlist = new Editor("module.page.accordionlist", "@laola.modules.accordionlist.fields.");
const accordionItem = new Editor("module.page.accordionlist.item", "@laola.modules.accordionlist.item.");
accordionItem.field("title").text(60).required();
accordionItem.field("text").rte().required();
accordionlist.field("items").nested(accordionItem, {
  limit: 15,
  title: "@laola.modules.accordionlist.itemHeadline",
  itemLabel: (x) => x.title,
  itemDescription: (x) => x.text,
  itemIcon: "fth-align-left",
  template: {
    title: null,
    text: null
  }
}).required();
accordionlist.preview('<ui-module-preview-tags :items="model.items" property="title" />');
const button = new Editor("module.page.button", "@laola.modules.button.fields.");
const BUTTON_COLORS = [
  {value: "@laola.modules.button.colors.primary", key: "primary"},
  {value: "@laola.modules.button.colors.light", key: "light"}
];
const BUTTON_POSITIONS = [
  {value: "@laola.modules.button.positions.center", key: "center"},
  {value: "@laola.modules.button.positions.left", key: "left"},
  {value: "@laola.modules.button.positions.right", key: "right"}
];
button.field("color").select(BUTTON_COLORS);
button.field("position").select(BUTTON_POSITIONS);
button.field("text").text(40).required();
button.field("link").linkPicker();
button.preview('<ui-module-preview-button :text="model.text" />');
const buttonbox = new Editor("module.page.buttonbox", "@laola.modules.buttonbox.fields.");
buttonbox.field("headline").text(80);
buttonbox.field("text").rte();
buttonbox.field("color").select(BUTTON_COLORS);
buttonbox.field("position").select(BUTTON_POSITIONS);
buttonbox.field("buttonText").text(40).required();
buttonbox.field("link").linkPicker();
buttonbox.field("isPrimary").toggle();
buttonbox.preview('<ui-module-preview-button :text="model.text" />');
const newspreview = new Editor("module.page.newspreview", "@laola.modules.newspreview.fields.");
newspreview.field("ids").custom(StoryPicker, {limit: 3});
newspreview.preview('<ui-module-preview-text :text="model.text" :subline="model.subline" />');
const newsletter = new Editor("module.page.newsletter", "@laola.modules.newsletter.fields.");
newspreview.preview('<ui-module-preview-text :text="model.text" :subline="model.subline" />');
const line = new Editor("module.page.line", "@laola.modules.line.fields.");
line.preview("<hr />");
const textGrid = new Editor("module.page.textgrid", "@laola.modules.textgrid.fields.");
addTextblocks(textGrid, 12);
textGrid.preview('<ui-module-preview-tags :items="model.items" property="title" />');
const textCarousel = new Editor("module.page.textcarousel", "@laola.modules.textcarousel.fields.");
addTextblocks(textCarousel, 12);
textCarousel.preview('<ui-module-preview-tags :items="model.items" property="title" />');
const imageWithOrderedList = new Editor("module.page.imagewithorderedlist", "@laola.modules.imagewithorderedlist.fields.");
const imageWithOrderedListItem = new Editor("module.page.imagewithorderedlist.item", "@laola.modules.imagewithorderedlist.item.");
imageWithOrderedListItem.field("title").text(60).required();
imageWithOrderedListItem.field("text").rte().required();
imageWithOrderedListItem.field("link").linkPicker();
imageWithOrderedList.field("imageId").image().required();
imageWithOrderedList.field("imageRightAligned").toggle();
imageWithOrderedList.field("items").nested(imageWithOrderedListItem, {
  limit: 4,
  title: "@laola.modules.imagewithorderedlist.itemHeadline",
  itemLabel: (x) => x.title,
  itemDescription: (x) => x.text,
  itemIcon: "fth-align-left",
  template: {
    title: null,
    text: null,
    link: null
  }
}).required();
imageWithOrderedList.preview('<ui-module-preview-figure :image="model.imageId" />');
const team = new Editor("module.page.team", "@laola.modules.team.fields.");
team.preview("<div>team // TODO</div>");
const featureBlocks = new Editor("module.page.featureblocks", "@laola.modules.featureblocks.fields.");
const FEATURES_LAYOUTS = [
  {value: "@laola.modules.featureblocks.layouts.highThree", key: "highThree"},
  {value: "@laola.modules.featureblocks.layouts.lowThree", key: "lowThree"},
  {value: "@laola.modules.featureblocks.layouts.lowTwo", key: "lowTwo"}
];
featureBlocks.field("layout").select(FEATURES_LAYOUTS);
featureBlocks.field("isInline").toggle();
featureBlocks.field("background").when((x) => !x.isInline).select(COLORS);
addFigureblocks(featureBlocks, 6);
featureBlocks.preview('<ui-module-preview-tags :items="model.items" property="title" />');
const products$1 = new Editor("module.page.products", "@laola.modules.products.fields.");
products$1.field("headline").text(80);
products$1.field("items").custom(ProductPicker, {limit: 20});
products$1.preview('<ui-module-preview-text :text="model.headline" />');
const gallery = new Editor("module.page.gallery", "@laola.modules.gallery.fields.");
gallery.field("headline").text(60);
gallery.field("text").text(180);
gallery.field("link").linkPicker();
const galleryItem = new Editor("module.page.gallery.item", "@laola.modules.gallery.item.");
galleryItem.field("imageId").image().required;
galleryItem.field("text").text(180);
galleryItem.field("link").linkPicker();
gallery.field("items").nested(galleryItem, {
  limit: 20,
  title: "@laola.modules.gallery.itemHeadline",
  itemLabel: (x) => x.title,
  itemIcon: "fth-image",
  template: {
    title: null,
    imageId: null,
    link: null
  }
}).required();
gallery.preview('<ui-module-preview-tags :items="model.items" property="title" />');
const video = new Editor("module.page.video", "@laola.modules.video.fields.");
video.field("video").video();
video.preview('<ui-module-preview-figure :image="model.imageId" :text="model.headline" :subline="model.text" />');
var pageModules = [
  text$1,
  headline,
  image,
  editor$q,
  figure,
  accordionlist,
  button,
  buttonbox,
  newspreview,
  newsletter,
  line,
  textGrid,
  textCarousel,
  imageWithOrderedList,
  team,
  featureBlocks,
  products$1,
  gallery,
  video
];
const text = new Editor("module.news.text", "@laola.newsmodules.text.fields.");
text.field("text").rte().required();
var newsModules = [text];
const getOpts = (key, vertical) => {
  return {
    label: "@laola.pages.partials." + key,
    description: "@laola.pages.partials." + key + "_text",
    vertical: typeof vertical === "undefined" ? true : vertical
  };
};
const addTitle = (tab, prefix2) => {
  tab.field(prefix2 + "hideInTitle", getOpts("hideInTitle")).toggle();
  tab.field(prefix2 + "titleOverride", getOpts("titleOverride")).text(80);
  tab.field(prefix2 + "titleOverrideCurrentPage", getOpts("titleOverrideCurrentPage")).text(80);
  tab.field(prefix2 + "titleOverrideAll", getOpts("titleOverrideAll")).toggle();
};
const addSeo = (tab, prefix2) => {
  tab.field(prefix2 + "seoDescription", getOpts("seoDescription")).text(160);
  tab.field(prefix2 + "seoImageId", getOpts("seoImageId")).image();
  tab.field(prefix2 + "noFollow", getOpts("noFollow")).toggle();
  tab.field(prefix2 + "noIndex", getOpts("noIndex")).toggle();
};
const addOptions = (tab, prefix2) => {
  tab.field(prefix2 + "hideInNavigation", getOpts("hideInNavigation")).toggle();
  tab.field("urlAlias", getOpts("urlAlias")).text(80);
};
const addTabs = (editor2) => {
  const options2 = editor2.tab("options", "@laola.tabs.options");
  const title = editor2.tab("title", "@laola.tabs.title");
  const meta = editor2.tab("meta", "@laola.tabs.meta");
  addSeo(meta, "meta.");
  addTitle(title, "meta.");
  addOptions(options2, "options.");
  return {options: options2, title, meta};
};
var partials = {addTitle, addSeo, addOptions, addTabs};
const editor$p = new Editor("pages.content", "@laola.pages.content.fields.");
const content$b = editor$p.tab("content", "@laola.tabs.content");
partials.addTabs(editor$p);
content$b.field("modules", {hideLabel: true}).modules(["page"]).required();
const editor$o = new Editor("pages.root", "@laola.pages.content.fields.");
editor$o.setBase(editor$p);
var render$1r = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-editor-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), !_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, label: "Confirm", state: form.state, disabled: _vm.loading}}) : _vm._e()];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-editor-overlay-editor"}, [_c("ui-editor", {attrs: {config: _vm.editor, meta: _vm.meta, "is-page": false, infos: "none", disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$1r = [];
const script$1r = {
  props: {
    config: Object
  },
  components: {UiEditor},
  data: () => ({
    isAdd: true,
    disabled: false,
    id: null,
    loading: true,
    state: "default",
    parent: null,
    editor: null,
    meta: {},
    model: {}
  }),
  methods: {
    onLoad(form) {
      this.isAdd = this.config.create === true;
      this.meta = {
        parentModel: this.config.parentModel
      };
      this.model = JSON.parse(JSON.stringify(this.config.model));
      this.editor = this.config.editor;
      this.loading = false;
    },
    onSubmit(form) {
      this.config.confirm(this.model);
    }
  }
};
const __cssModules$1r = {};
var component$1r = normalizeComponent(script$1r, render$1r, staticRenderFns$1r, false, injectStyles$1r, null, null, null);
function injectStyles$1r(context) {
  for (let o in __cssModules$1r) {
    this[o] = __cssModules$1r[o];
  }
}
component$1r.options.__file = "app/editor/editor-overlay.vue";
var UiEditorOverlay = component$1r.exports;
const base$4 = "pages/";
var PagesApi = __assign(__assign({}, collection$1(base$4)), {
  getAllowedPageTypes: async (parent) => await get$1(base$4 + "getAllowedPageTypes", {params: {parent}}),
  getPageType: async (alias2) => await get$1(base$4 + "getPageType", {params: {alias: alias2}}),
  getEmpty: async (type, parent) => await get$1(base$4 + "getEmpty", {params: {type, parent}}),
  getRevisions: async (id, page) => await get$1(base$4 + "getRevisions", {params: {id, page}}),
  saveSorting: async (ids) => await post$1(base$4 + "saveSorting", ids),
  move: async (id, destinationId) => await post$1(base$4 + "move", {id, destinationId}),
  copy: async (id, destinationId, includeDescendants) => await post$1(base$4 + "copy", {id, destinationId, includeDescendants}),
  restore: async (id, includeDescendants) => await post$1(base$4 + "restore", {id, includeDescendants}),
  delete: async (id, moveToRecycleBin) => await del$1(base$4 + "delete", {params: {id, moveToRecycleBin}})
});
const template = {
  title: "@laola.settings.layout.navigationitem.title",
  limit: 10,
  itemIcon: "fth-folder",
  itemLabel: (x) => x.name,
  itemDescription: (x) => x.url,
  template: {
    name: null,
    pageId: null,
    url: null,
    isExternal: false,
    childType: "children",
    children: []
  }
};
const TYPES$1 = [
  {value: "@laola.settings.layout.navigationitem.types.children", key: "children"},
  {value: "@laola.settings.layout.navigationitem.types.custom", key: "custom"},
  {value: "@laola.settings.layout.navigationitem.types.products", key: "products"}
];
const editor$n = new Editor("laola.settings.layout.navigationitem", "@laola.settings.layout.navigationitem.fields.");
editor$n.field("pageId").pagePicker();
editor$n.field("name").text(40);
editor$n.field("url").when((x) => !x.pageId).text(240);
editor$n.field("isExternal").toggle();
editor$n.field("childType").select(TYPES$1);
var editorConfig = __assign({
  editor: editor$n
}, template);
var render$1q = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "editor-nested", attrs: {depth: _vm.depth}}, [_vm.items.length ? _c("div", {staticClass: "ui-pick-previews"}, _vm._l(_vm.previews, function(item2, index) {
    return _c("div", {key: index, staticClass: "ui-pick-preview"}, [_c("ui-select-button", {attrs: {icon: item2.icon, label: item2.name, description: item2.text, tokens: {id: item2.original.pageId}, disabled: _vm.disabled}, on: {click: function($event) {
      return _vm.editItem(item2.original);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close", disabled: _vm.disabled}, on: {click: function($event) {
      return _vm.removeItem(index);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.limit > _vm.items.length ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.addLabel || "@ui.add", disabled: _vm.disabled}, on: {click: _vm.addItem}}) : _vm._e()], 1);
};
var staticRenderFns$1q = [];
const script$1q = {
  components: {UiEditor},
  props: {
    value: [Array, Object],
    meta: Object
  },
  watch: {
    value: {
      deep: true,
      handler(val) {
        this.setup();
      }
    }
  },
  data: () => ({
    depth: 0,
    disabled: false,
    limit: editorConfig.limit,
    addLabel: editorConfig.addLabel,
    itemLabel: editorConfig.itemLabel,
    itemDescription: editorConfig.itemDescription,
    itemIcon: editorConfig.itemIcon,
    template: editorConfig.template,
    items: [],
    multiple: false,
    pageCache: {},
    previews: []
  }),
  mounted() {
    this.setup();
  },
  methods: {
    setup() {
      this.items = JSON.parse(JSON.stringify(this.value)) || [];
      this.multiple = this.limit > 1;
      if (!this.multiple) {
        this.items = this.items ? [this.items] : [];
      }
      let pageIds = this.items.filter((x) => !!x.pageId && !this.pageCache[x.pageId]).map((x) => x.pageId);
      if (pageIds.length > 0) {
        PagesApi.getPreviews(pageIds).then((res) => {
          res.forEach((preview) => {
            this.pageCache[preview.id] = preview;
          });
          this.createPreviews();
        });
      } else {
        this.createPreviews();
      }
    },
    createPreviews() {
      this.previews = [];
      this.items.forEach((item2) => {
        let preview = null;
        if (item2.pageId) {
          preview = this.pageCache[item2.pageId];
        } else {
          preview = {
            icon: "fth-link",
            name: item2.name,
            text: item2.url
          };
        }
        if (!preview.hasError && item2.name) {
          preview.name = item2.name;
        }
        preview.original = item2;
        this.previews.push(preview);
      });
    },
    getNewItem() {
      return JSON.parse(JSON.stringify(this.template || {}));
    },
    addItem() {
      if (this.limit <= this.items.length) {
        return;
      }
      this.editItem(this.getNewItem(), true);
      this.onChange();
    },
    editItem(item2, isAdd) {
      return Overlay.open({
        component: UiEditorOverlay,
        display: "editor",
        editor: editorConfig.editor,
        title: editorConfig.title || "@ui.edit.title",
        model: item2,
        width: 800,
        create: isAdd
      }).then((value) => {
        if (isAdd) {
          this.items.push(value);
        } else {
          const index = this.items.indexOf(item2);
          this.removeItem(index);
          this.items.splice(index, 0, value);
        }
        this.onChange();
      });
    },
    removeItem(index) {
      this.items.splice(index, 1);
      this.onChange();
    },
    onChange() {
      this.$emit("input", this.multiple ? this.items : this.items.length > 0 ? this.items[0] : null);
    }
  }
};
const __cssModules$1q = {};
var component$1q = normalizeComponent(script$1q, render$1q, staticRenderFns$1q, false, injectStyles$1q, null, null, null);
function injectStyles$1q(context) {
  for (let o in __cssModules$1q) {
    this[o] = __cssModules$1q[o];
  }
}
component$1q.options.__file = "../../Laola/Laola.Backoffice/Plugin/editor/navigation.vue";
var NavigationEditor = component$1q.exports;
const base$3 = "childNavigation/";
var ChildNavigationApi = {
  get: async (type) => await get$1(base$3 + type)
};
var render$1p = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-childnav"}, [_c("div", {staticClass: "laola-childnav-item is-head"}, [_c("label", {staticClass: "ui-property-label"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@laola.childnav.id", expression: "'@laola.childnav.id'"}]}), _c("small", {directives: [{name: "localize", rawName: "v-localize", value: "@laola.childnav.id_text", expression: "'@laola.childnav.id_text'"}]})]), _c("span"), _c("label", {staticClass: "ui-property-label"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@laola.childnav.name", expression: "'@laola.childnav.name'"}]}), _c("small", {directives: [{name: "localize", rawName: "v-localize", value: "@laola.childnav.name_text", expression: "'@laola.childnav.name_text'"}]})]), _c("label", {staticClass: "ui-property-label"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@laola.childnav.alias", expression: "'@laola.childnav.alias'"}]}), _c("small", {directives: [{name: "localize", rawName: "v-localize", value: "@laola.childnav.alias_text", expression: "'@laola.childnav.alias_text'"}]})])]), _vm._l(_vm.items, function(item2, index) {
    return !_vm.hidden(item2) ? _c("div", {key: item2.id, staticClass: "laola-childnav-item"}, [_c("span", {staticClass: "laola-childnav-item-index"}, [_vm._v(_vm._s(item2.id))]), _c("span", [_c("ui-icon-button", {staticClass: "laola-childnav-item-reset", attrs: {type: "small blank", icon: "fth-rotate-ccw", title: "Reset"}})], 1), _c("input", {directives: [{name: "model", rawName: "v-model", value: item2.name, expression: "item.name"}], staticClass: "ui-input", attrs: {type: "text"}, domProps: {value: item2.name}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "name", $event.target.value);
    }}}), _c("input", {directives: [{name: "model", rawName: "v-model", value: item2.alias, expression: "item.alias"}], staticClass: "ui-input", attrs: {type: "text"}, domProps: {value: item2.alias}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "alias", $event.target.value);
    }}})]) : _vm._e();
  })], 2);
};
var staticRenderFns$1p = [];
var children_vue_vue_type_style_index_0_lang = ".laola-childnav-item {\n  display: grid;\n  grid-template-columns: 130px 32px 1fr 1fr;\n  grid-gap: 6px;\n  align-items: center;\n  /*border-radius: var(--radius);\n  padding: var(--padding-m);\n  background: var(--color-box-nested);*/\n}\n.laola-childnav-item.is-head {\n  padding-bottom: var(--padding-s);\n  /*border-bottom: 1px solid var(--color-line);\n  padding-bottom: var(--padding-s);\n  margin-bottom: var(--padding-m);*/\n}\n.laola-childnav-item.is-head label {\n  padding-left: 12px;\n  padding-bottom: 10px;\n  align-self: flex-start;\n  width: 100%;\n}\n.laola-childnav-item.is-head label small {\n  padding-top: 2px;\n}\n.laola-childnav-item.is-head:first-child label {\n  padding-left: 0;\n}\n.laola-childnav-item-index {\n  text-align: left;\n  font-size: 13px;\n  font-weight: 500;\n  color: var(--color-text-dim);\n  padding-right: 15px;\n}\n.laola-childnav-item + .laola-childnav-item {\n  margin-top: 8px;\n}\n.laola-childnav-item input {\n  /*background: var(--color-box);\n  border: 1px dashed var(--color-line-dashed);*/\n}";
const script$1p = {
  components: {UiEditor},
  props: {
    value: [Array, Object],
    meta: Object,
    type: String,
    hidden: {
      type: Function,
      default: () => false
    }
  },
  watch: {
    value: {
      deep: true,
      handler(val) {
      }
    }
  },
  data: () => ({
    items: []
  }),
  mounted() {
    ChildNavigationApi.get(this.type).then((res) => {
      this.items = res;
    });
  },
  methods: {}
};
const __cssModules$1p = {};
var component$1p = normalizeComponent(script$1p, render$1p, staticRenderFns$1p, false, injectStyles$1p, null, null, null);
function injectStyles$1p(context) {
  for (let o in __cssModules$1p) {
    this[o] = __cssModules$1p[o];
  }
}
component$1p.options.__file = "../../Laola/Laola.Backoffice/Plugin/editor/children.vue";
var ChildrenComponent = component$1p.exports;
const editor$m = new Editor("pages.products", "@laola.pages.products.fields.");
const products = editor$m.tab("products", "@laola.pages.products.tab_products");
const content$a = editor$m.tab("content", "@laola.childnav.tab");
partials.addTabs(editor$m);
content$a.field("children", {hideLabel: true}).custom(ChildrenComponent, {
  type: "products"
});
products.field("channelId").custom(ChannelPicker, {limit: 1}).required();
products.field("helpPageId").pagePicker();
products.field("overviewOptions.showShoesFeature").toggle();
products.field("overviewOptions.showVoucherFeature").toggle();
products.fieldset((set) => {
  set.field("overviewOptions.mainCategoryId").custom(CategoryPicker, {channel: (x) => x.channelId});
  set.field("overviewOptions.mainCategoryText").text(100);
});
const editor$l = new Editor("pages.news", "@laola.pages.content.fields.");
editor$l.setBase(editor$p);
const editor$k = new Editor("pages.account", "@laola.pages.account.fields.");
const content$9 = editor$k.tab("content", "@laola.childnav.tab");
partials.addTabs(editor$k);
content$9.field("children", {hideLabel: true}).custom(ChildrenComponent, {
  type: "account",
  hidden: (item2) => ["wishlist", "payment"].indexOf(item2.id) > -1
});
const editor$j = new Editor("pages.faq", "@laola.pages.faq.fields.");
const content$8 = editor$j.tab("content", "@laola.tabs.content");
partials.addTabs(editor$j);
content$8.field("headline").text(60).required();
const editor$i = new Editor("laola.faq.question", "@laola.pages.faq.question.fields.");
editor$i.field("question").text(180).required();
editor$i.field("answer").rte().required();
const editor$h = new Editor("pages.faq.category", "@laola.pages.faq.category.fields.");
const content$7 = editor$h.tab("content", "@laola.pages.faq.category.tab");
partials.addTabs(editor$h);
content$7.field("icon").iconPicker();
content$7.field("questions").nested(editor$i, {
  limit: 32,
  title: "@laola.pages.faq.question.title",
  itemLabel: (x) => x.question,
  itemDescription: (x) => x.answer,
  itemIcon: "fth-message-square",
  template: {question: null, answer: null}
}).required();
const editor$g = new Editor("laola.contact.info", "@laola.pages.contact.fields.");
editor$g.field("icon").iconPicker().required();
editor$g.field("title").text(40).required();
editor$g.field("content").rte().required();
const editor$f = new Editor("pages.contact", "@laola.pages.contact.fields.");
const content$6 = editor$f.tab("content", "@laola.tabs.content");
partials.addTabs(editor$f);
content$6.field("infos").nested(editor$g, {
  limit: 4,
  title: "@laola.pages.faq.question.title",
  itemLabel: (x) => x.title,
  itemDescription: (x) => x.content,
  itemIcon: "fth-info",
  template: {title: null, content: null, icon: null}
}).required();
const editor$e = new Editor("pages.redirect", "@laola.pages.redirect.fields.");
const content$5 = editor$e.tab("content", "@laola.tabs.content");
partials.addTabs(editor$e);
content$5.field("link").linkPicker();
const PAGE_TYPE_VALUES = [
  {value: "@laola.pages.clubs.types.intro", key: "intro"},
  {value: "@laola.pages.clubs.types.listing", key: "listing"}
];
const editor$d = new Editor("pages.clubs", "@laola.pages.clubs.fields.");
const content$4 = editor$d.tab("content", "@laola.tabs.content");
partials.addTabs(editor$d);
content$4.field("type").select(PAGE_TYPE_VALUES).required();
content$4.field("introHeadline").text().required();
content$4.field("introSubline").when((x) => x.type === "listing").text().required();
content$4.field("introText").when((x) => x.type === "intro").rte();
content$4.field("introImageId").when((x) => x.type === "intro").image().required();
content$4.field("displayedChannelIds").when((x) => x.type === "intro").custom(ChannelPicker, {limit: 7});
content$4.field("modules").when((x) => x.type === "intro").modules(["page"]);
const editor$c = new Editor("integration.analytics.google", "@laola.integrations.googleanalytics.fields.");
editor$c.field("trackingId").text(30).required();
editor$c.field("siteVerificationId").text(150).required();
const editor$b = new Editor("integration.newsletter.sendinblue", "@laola.integrations.sendinblue.fields.");
editor$b.options.boxes = true;
const content$3 = editor$b.tab("content", "@laola.integrations.sendinblue.tabs.content");
content$3.field("authKey").text(150).required();
content$3.field("username", {helpText: "@laola.integrations.sendinblue.fields.username_help"}).text(150).required();
content$3.field("password").password(150).required();
const addressBook = editor$b.tab("addressBook", "@laola.integrations.sendinblue.tabs.addressBook");
addressBook.field("defaultListId").text(30).required();
addressBook.field("defaultGroupId").text(30);
const editor$a = new Editor("integration.analytics.fathom", "@laola.integrations.fathom.fields.");
editor$a.field("siteId").text(30).required();
editor$a.field("customDomain").text(150);
const editor$9 = new Editor("integration.payment.adyen.configuration", "@laola.integrations.adyen.fields.");
const general = editor$9.tab("general", null);
const notifications = editor$9.tab("notifications", "@laola.integrations.adyen.tabs.notification");
editor$9.options.display = "boxes";
general.field("testMode").toggle();
general.field("apiUrl").text(200).required();
general.field("merchantId").text(200).required();
general.field("apiKey").password(200).required();
general.field("publicKey").text(1e3).required();
general.field("token").text(200).required();
general.field("originKey").text(200).required();
notifications.field("notificationsUsername").text(100).required();
notifications.field("notificationsPassword").password(100).required();
const editor$8 = new Editor("integration.payment.adyen", "@laola.integrations.adyen.fields.");
editor$8.field("currentConfigurationId").select((entity) => {
  return entity.configurations.map((x) => {
    return {
      key: x.id,
      value: (x.testMode ? "TEST" : "LIVE") + " | " + x.merchantId
    };
  });
}, {emptyOption: true}).required();
editor$8.field("configurations").nested(editor$9, {
  limit: 4,
  title: "@laola.pages.faq.question.title",
  itemLabel: (x) => (x.testMode ? "TEST" : "LIVE") + " | " + x.merchantId,
  itemDescription: (x) => x.apiUrl,
  itemIcon: "fth-settings",
  template: {
    testMode: true,
    apiUrl: null,
    merchantId: null,
    apiKey: null,
    publicKey: null,
    token: null,
    originKey: null,
    notificationsUsername: null,
    notificationsPassword: null
  }
}).required();
var render$1o = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.entity ? _c("div", [_c("product-picker", {attrs: {value: _vm.model, channel: _vm.entity.channelId}, on: {change: _vm.onProductChange}})], 1) : _vm._e();
};
var staticRenderFns$1o = [];
const script$1o = {
  name: "laolaDiscountProduct",
  components: {ProductPicker},
  props: {
    value: {
      type: String,
      default: null
    },
    entity: {
      type: Object,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    model: {
      channelId: null,
      productId: null,
      variantId: null
    }
  }),
  watch: {
    value(val) {
      this.model.productId = val;
    },
    "entity.channelId": function(val) {
      this.model.channelId = val;
    }
  },
  mounted() {
    this.model.productId = this.value;
    this.model.channelId = this.entity.channelId;
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    onProductChange(value) {
      console.info(value);
    }
  }
};
const __cssModules$1o = {};
var component$1o = normalizeComponent(script$1o, render$1o, staticRenderFns$1o, false, injectStyles$1o, null, null, null);
function injectStyles$1o(context) {
  for (let o in __cssModules$1o) {
    this[o] = __cssModules$1o[o];
  }
}
component$1o.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/commerce/discount-product.vue";
var DiscountProduct = component$1o.exports;
const editor$7 = new Editor("commerce.discount", "@laola.discounts.fields.");
editor$7.tab("content", "@laola.tabs.content");
editor$7.field("activeDate").dateRangePicker({
  inline: true,
  time: true
});
editor$7.field("channelId").custom(ChannelPicker, {limit: 1}).required();
editor$7.field("productId").custom(DiscountProduct).required();
const editor$6 = new Editor("commerce.campaign", "@laola.campaign.fields.");
const content$2 = editor$6.tab("content", "@laola.tabs.content");
content$2.field("text").textarea().required();
content$2.field("activeDate").dateRangePicker({
  inline: true,
  time: true
});
content$2.field("link").linkPicker();
content$2.field("displayEverywhere").toggle();
content$2.field("channelIds").when((x) => !x.displayEverywhere).custom(ChannelPicker, {limit: 1e3}).required();
content$2.field("hiddenChannelIds").when((x) => x.displayEverywhere).custom(ChannelPicker, {limit: 1e3});
var render$1n = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-tableeditor", class: {"is-disabled": _vm.disabled}}, [_c("div", {staticClass: "-table"}, _vm._l(_vm.rows, function(row, index) {
    return _c("div", {key: index, staticClass: "-row"}, _vm._l(row, function(cell, cellIndex) {
      return _c("div", {key: cellIndex, staticClass: "-cell", class: {"-head": index == 0}}, [_c("p", {staticClass: "-content", attrs: {"data-placeholder": index == 0 ? "Title" : "Content", contenteditable: "true"}, domProps: {innerHTML: _vm._s(cell)}, on: {input: function($event) {
        return _vm.onCellChange($event, index, cellIndex);
      }}})]);
    }), 0);
  }), 0), _c("div", [_c("br"), _c("br"), _c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.idx, expression: "idx"}], staticStyle: {display: "inline", width: "80px", "margin-right": "10px"}, attrs: {type: "number", placeholder: "Index"}, domProps: {value: _vm.idx}, on: {input: function($event) {
    if ($event.target.composing) {
      return;
    }
    _vm.idx = $event.target.value;
  }}}), _c("button", {staticClass: "ui-button", attrs: {type: "button"}, on: {click: function($event) {
    return _vm.addRow(_vm.idx);
  }}}, [_vm._v("addRow")]), _c("button", {staticClass: "ui-button", attrs: {type: "button"}, on: {click: function($event) {
    return _vm.removeRow(_vm.idx);
  }}}, [_vm._v("removeRow")]), _c("button", {staticClass: "ui-button", attrs: {type: "button"}, on: {click: function($event) {
    return _vm.addColumn(_vm.idx);
  }}}, [_vm._v("addColumn")]), _c("button", {staticClass: "ui-button", attrs: {type: "button"}, on: {click: function($event) {
    return _vm.removeColumn(_vm.idx);
  }}}, [_vm._v("removeColumn")])])]);
};
var staticRenderFns$1n = [];
var table_vue_vue_type_style_index_0_lang = ".ui-tableeditor .-table {\n  width: 100%;\n  border-collapse: collapse;\n  break-inside: auto;\n  border-radius: var(--radius);\n  position: relative;\n  display: table;\n}\n.ui-tableeditor .-row {\n  display: table-row;\n}\n.ui-tableeditor .-cell {\n  display: table-cell;\n  text-align: left;\n  width: auto;\n  position: relative;\n  align-self: start;\n  border-top: medium none;\n  align-items: start;\n  border-right: medium none;\n  border-bottom: medium none;\n  -moz-box-align: start;\n}\n.ui-tableeditor .-cell.-head .-content {\n  font-weight: 700;\n}\n.ui-tableeditor .-content {\n  margin: 0;\n  min-height: 1.2em;\n  padding: 12px 8px;\n}\n\n/*.ui-tableeditor .-content[data-placeholder]:before\n{\n  content: attr(data-placeholder);\n  color: var(--color-text-dim-one);\n  position: absolute;\n  z-index: 0;\n}*/\n.ui-tableeditor .-content:focus-within {\n  background: var(--color-bg-dim);\n  outline: none;\n}\n.ui-tableeditor .-row {\n  border-bottom: 1px solid var(--color-line-onbg);\n  padding: 10px;\n  background: white none repeat scroll 0% 0%;\n  break-after: auto;\n  break-inside: avoid;\n}";
const script$1n = {
  name: "uiTableEditor",
  props: {
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    idx: 0,
    rows: []
  }),
  watch: {
    value(value) {
      this.rebuild();
    }
  },
  mounted() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.rows = JSON.parse(JSON.stringify(this.value));
    },
    update() {
      this.$emit("input", this.rows);
    },
    onCellChange(ev, rowIndex, cellIndex) {
      this.rows[rowIndex][cellIndex] = ev.target.innerText;
      this.update();
    },
    addRow(index) {
      this.rows.splice(index, 0, this.rows[0].map((_) => "<br>"));
      this.update();
    },
    removeRow(index) {
      this.rows.splice(index, 1);
      this.update();
    },
    addColumn(index) {
      this.rows.forEach((row) => {
        row.splice(index, 0, "<br>");
      });
      this.update();
    },
    removeColumn(index) {
      this.rows.forEach((row) => {
        row.splice(index, 1);
      });
      this.update();
    }
  }
};
const __cssModules$1n = {};
var component$1n = normalizeComponent(script$1n, render$1n, staticRenderFns$1n, false, injectStyles$1n, null, null, null);
function injectStyles$1n(context) {
  for (let o in __cssModules$1n) {
    this[o] = __cssModules$1n[o];
  }
}
component$1n.options.__file = "app/components/forms/table.vue";
var TableEditor = component$1n.exports;
const editor$5 = new Editor("commerce.sizechart", "@laola.sizechart.fields.");
const content$1 = editor$5.tab("content", "@laola.tabs.content");
content$1.field("title").text(60).required();
content$1.field("text").rte();
content$1.field("content").custom(TableEditor);
var render$1m = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("router-link", {staticClass: "ui-link laola-voucherowner", attrs: {to: {name: "commerce-orders-edit", params: {id: _vm.entity.owner.orderId}}}}, [_vm._v(" " + _vm._s(_vm.entity.owner.name) + " ")]);
};
var staticRenderFns$1m = [];
const script$1m = {
  name: "laolaVoucherOwner",
  props: {
    entity: {
      type: Object,
      default: () => {
      }
    }
  }
};
const __cssModules$1m = {};
var component$1m = normalizeComponent(script$1m, render$1m, staticRenderFns$1m, false, injectStyles$1m, null, null, null);
function injectStyles$1m(context) {
  for (let o in __cssModules$1m) {
    this[o] = __cssModules$1m[o];
  }
}
component$1m.options.__file = "../../Laola/Laola.Backoffice/Plugin/components/voucher-owner.vue";
var VoucherOwner = component$1m.exports;
var render$1l = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-voucherhistory ui-revisions"}, _vm._l(_vm.entity.history.slice().reverse(), function(item2) {
    return _c("div", {key: item2.createdDate, staticClass: "ui-revision"}, [_c("span", {directives: [{name: "currency", rawName: "v-currency", value: item2.difference, expression: "item.difference"}], staticClass: "ui-revision-action"}), _c("ui-date", {staticClass: "ui-revision-date", attrs: {format: "long", split: true}, model: {value: item2.createdDate, callback: function($$v) {
      _vm.$set(item2, "createdDate", $$v);
    }, expression: "item.createdDate"}}), _c("div"), item2.by.orderId ? _c("router-link", {staticClass: "ui-link is-minor", attrs: {to: {name: "commerce-orders-edit", params: {id: item2.by.orderId}}}}, [_vm._v("Order")]) : _vm._e()], 1);
  }), 0);
};
var staticRenderFns$1l = [];
var voucherHistory_vue_vue_type_style_index_0_lang = ".laola-voucherhistory .ui-revision-action {\n  font-size: var(--font-size-xs);\n  height: 28px;\n  line-height: 28px;\n  color: var(--color-text);\n}\n.laola-voucherhistory {\n  margin-top: 8px;\n}";
const script$1l = {
  name: "laolaVoucherHistory",
  props: {
    entity: {
      type: Object,
      default: () => {
      }
    }
  }
};
const __cssModules$1l = {};
var component$1l = normalizeComponent(script$1l, render$1l, staticRenderFns$1l, false, injectStyles$1l, null, null, null);
function injectStyles$1l(context) {
  for (let o in __cssModules$1l) {
    this[o] = __cssModules$1l[o];
  }
}
component$1l.options.__file = "../../Laola/Laola.Backoffice/Plugin/components/voucher-history.vue";
var VoucherHistory = component$1l.exports;
const editor$4 = new Editor("commerce.voucher", "@laola.voucher.fields.");
editor$4.field("number", {vertical: false}).output().when((x) => x.id);
editor$4.field("status", {vertical: false}).output((x) => "@laola.voucher.states." + x).when((x) => x.id);
editor$4.field("owner", {vertical: false}).custom(VoucherOwner).when((x) => x.owner && x.owner.orderId);
editor$4.fieldset((set) => {
  set.field("amount").custom(UiCurrency).required();
  set.field("balance").custom(UiCurrency).required().when((x) => x.id);
});
editor$4.field("history").custom(VoucherHistory).when((x) => x.history.length);
const editor$3 = new Editor("commerce.experience", "@laola.experience.fields.");
const content = editor$3.tab("content", "@laola.tabs.content");
content.field("name").text(60).required();
content.field("imageId").image().required();
content.field("link").linkPicker().required();
var editors = [
  editor$r,
  ...pageModules,
  ...newsModules,
  editor$p,
  editor$o,
  editor$m,
  editor$l,
  editor$k,
  editor$j,
  editor$h,
  editor$f,
  editor$e,
  editor$d,
  editor$c,
  editor$b,
  editor$a,
  editor$8,
  editor$7,
  editor$6,
  editor$5,
  editor$4,
  editor$3
];
const list$7 = new List("spaces.team");
list$7.templateLabel = (x) => "@laola.spaces.team.fields." + x;
list$7.query.pageSize = 10;
list$7.column("imageId", {width: 70, canSort: false, hideLabel: true}).image();
list$7.column("name").name();
list$7.column("position").text();
list$7.column("createdDate").created();
list$7.column("isActive").active();
var DiscountsApi = __assign(__assign({}, collection$1("commerceDiscounts/")), {
  getListByQuery: async (query, config2) => await get$1("commerceDiscounts/getListByQuery", __assign(__assign({}, config2), {params: {query}}))
});
const list$6 = new List("commerce.discounts");
const now$1 = new Date();
list$6.templateLabel = (x) => "@laola.discounts.fields." + x;
list$6.link = "commerce-discounts-edit";
list$6.query.pageSize = 25;
list$6.onFetch((filter2) => DiscountsApi.getListByQuery(filter2));
list$6.column("name").name();
list$6.column("count").text();
list$6.column("isActive", {width: 200, label: "@ui.active"}).custom((val) => {
  const isActive = val && (val.from ? new Date(val.from) <= now$1 : true) && (val.to ? new Date(val.to) >= now$1 : true);
  return '<span class="ui-table-field-bool' + (isActive ? " is-checked" : "") + '"></span>';
}, true);
list$6.column("activeDate", {width: 200}).custom((val) => {
  if (!val || !val.from && !val.to) {
    return "-";
  }
  return Localization.localize(val.from && !val.to ? "@ui.date.x" : !val.from && val.to ? "@ui.date.y" : "@ui.date.xtoy", {
    tokens: {
      x: Strings.date(val.from, "short"),
      y: Strings.date(val.to, "short")
    }
  });
});
var CampaignsApi = __assign(__assign({}, collection$1("commerceCampaigns/")), {
  getListByQuery: async (query, config2) => await get$1("commerceCampaigns/getListByQuery", __assign(__assign({}, config2), {params: {query}}))
});
const list$5 = new List("commerce.campaigns");
const now = new Date();
list$5.templateLabel = (x) => "@laola.campaign.fields." + x;
list$5.link = "commerce-campaigns-edit";
list$5.query.pageSize = 25;
list$5.onFetch((filter2) => CampaignsApi.getListByQuery(filter2));
list$5.column("name").name();
list$5.column("isActive", {width: 200, label: "@ui.active"}).custom((val) => {
  const isActive = val && (val.from ? new Date(val.from) <= now : true) && (val.to ? new Date(val.to) >= now : true);
  return '<span class="ui-table-field-bool' + (isActive ? " is-checked" : "") + '"></span>';
}, true);
list$5.column("activeDate", {width: 200}).custom((val) => {
  if (!val || !val.from && !val.to) {
    return "-";
  }
  return Localization.localize(val.from && !val.to ? "@ui.date.x" : !val.from && val.to ? "@ui.date.y" : "@ui.date.xtoy", {
    tokens: {
      x: Strings.date(val.from, "short"),
      y: Strings.date(val.to, "short")
    }
  });
});
var SizeChartsApi = __assign(__assign({}, collection$1("commerceSizeCharts/")), {
  getListByQuery: async (query, config2) => await get$1("commerceSizeCharts/getListByQuery", __assign(__assign({}, config2), {params: {query}}))
});
const list$4 = new List("commerce.sizecharts");
list$4.templateLabel = (x) => "@laola.sizechart.fields." + x;
list$4.link = "commerce-sizecharts-edit";
list$4.onFetch((filter2) => SizeChartsApi.getListByQuery(filter2));
list$4.column("name").name();
list$4.column("isActive").active();
var VouchersApi = __assign(__assign({}, collection$1("commerceVouchers/")), {
  getListByQuery: async (query, config2) => await get$1("commerceVouchers/getListByQuery", __assign(__assign({}, config2), {params: {query}}))
});
const STATES$1 = [
  {value: "@laola.voucher.states.pending", key: "pending"},
  {value: "@laola.voucher.states.active", key: "active"},
  {value: "@laola.voucher.states.disabled", key: "disabled"},
  {value: "@laola.voucher.states.expired", key: "expired"}
];
const list$3 = new List("commerce.vouchers");
list$3.templateLabel = (x) => "@laola.voucher.fields." + x;
list$3.link = "commerce-vouchers-edit";
list$3.onFetch((filter2) => VouchersApi.getListByQuery(filter2));
list$3.column("number", {label: "@laola.voucher.fields.number", bold: true}).custom((value) => `<b>${value}</b>`, true);
list$3.column("status").custom((value, model) => `<span class="shop-orders-col-state2" data-state="${value}">${Localization.localize(STATES$1.find((t) => t.key == value).value)}</span>`, true);
list$3.column("customer").custom((value, model) => model.customerName);
list$3.column("amount", {width: 150}).currency();
list$3.column("balance", {width: 150}).currency();
list$3.column("createdDate").created();
var ExperiencesApi = __assign(__assign({}, collection$1("commerceExperiences/")), {
  getListByQuery: async (query, config2) => await get$1("commerceExperiences/getListByQuery", __assign(__assign({}, config2), {params: {query}}))
});
const list$2 = new List("commerce.experiences");
list$2.templateLabel = (x) => "@laola.experience.fields." + x;
list$2.link = "commerce-experiences-edit";
list$2.onFetch((filter2) => ExperiencesApi.getListByQuery(filter2));
list$2.column("name").name();
list$2.column("createdDate").created();
const getConfig = (config2) => {
  config2 = config2 || {};
  if (config2.scope) {
    config2.params = config2.params || {};
    config2.params.scope = config2.scope;
  }
  return config2;
};
async function get(url, config2 = null) {
  return await send(__assign({method: "get", url}, config2));
}
async function post(url, data = null, config2 = null) {
  return await send(__assign({method: "post", url, data}, config2));
}
async function del(url, config2 = null) {
  return await send(__assign({method: "delete", url}, config2));
}
async function send(config2) {
  config2 = getConfig(config2);
  try {
    const result = await axios(config2);
    return result.data;
  } catch (err) {
  }
}
function collection(base2) {
  return {
    getById: async (id, changeVector, config2) => await get(base2 + "getById", __assign(__assign({}, config2), {params: {id, changeVector}})),
    getByIds: async (ids, config2) => await get(base2 + "getByIds", __assign(__assign({}, config2), {params: {ids}})),
    getEmpty: async (config2) => await get(base2 + "getEmpty", __assign({}, config2)),
    getByQuery: async (query, config2) => await get(base2 + "getByQuery", __assign(__assign({}, config2), {params: {query}})),
    getAll: async (config2) => await get(base2 + "getAll", __assign({}, config2)),
    getPreviews: async (ids, config2) => await get(base2 + "getPreviews", __assign(__assign({}, config2), {params: {ids}})),
    getForPicker: async (config2) => await get(base2 + "getForPicker", __assign({}, config2)),
    getRevisions: async (id, query, config2) => await get(base2 + "getRevisions", __assign(__assign({}, config2), {params: {id, query}})),
    save: async (model, config2) => await post(base2 + "save", model, __assign({}, config2)),
    delete: async (id, config2) => await del(base2 + "delete", __assign(__assign({}, config2), {params: {id}}))
  };
}
const base$2 = "links/";
var LinksApi = {
  getPreviews: async (links) => await post$1(base$2 + "getPreviews", links)
};
var MediaFolderApi = __assign(__assign({}, collection$1("mediaFolder/")), {
  getHierarchy: async (id) => await get$1("mediaFolder/getHierarchy", {params: {id}}),
  getAllAsTree: async (parent, active) => await get$1("mediaFolder/getAllAsTree", {params: {parent, active}}),
  move: async (id, destinationId) => await post$1("mediaFolder/move", {id, destinationId})
});
const base$1 = "spaces/";
var SpacesApi = {
  getByAlias: async (alias2) => await get$1(base$1 + "getByAlias", {params: {alias: alias2}}),
  getAll: async () => await get$1(base$1 + "getAll"),
  getList: async (alias2, query) => {
    query.alias = alias2;
    return await get$1(base$1 + "getList", {params: query});
  },
  getContent: async (alias2, contentId) => await get$1(base$1 + "getContent", {params: {alias: alias2, contentId}}),
  save: async (model) => await post$1(base$1 + "save", model),
  delete: async (alias2, id) => await del$1(base$1 + "delete", {params: {alias: alias2, id}})
};
var RequestsApi = __assign(__assign({}, collection("commerceRequests/")), {
  getByIdAsOrder: async (id) => await get("commerceRequests/getByIdAsOrder", {params: {id}}),
  getListByQuery: async (query, showDone, customer2, config2) => await get("commerceRequests/getListByQuery", __assign(__assign({}, config2), {params: {query, showDone, customer: customer2}})),
  getDocument: async (path, securityKey, config2) => await get("commerceRequests/getDocument", __assign(__assign({}, config2), {params: {path, securityKey}})),
  getDocumentUrl: (path, securityKey) => {
    return zero.apiPath + "commerceRequests/getDocument?path=" + path + "&securityKey=" + securityKey;
  },
  getMessageTemplate: async (channelId) => await get("commerceRequests/getMessageTemplate", {params: {channelId}}),
  sendMessage: async (data) => await post("commerceRequests/sendMessage", data)
});
const list$1 = new List("commerce.requests");
list$1.setBase(list$j);
list$1.link = "commerce-requests-edit";
list$1.onFetch((filter2) => RequestsApi.getListByQuery(filter2));
list$1.removeColumn("paymentState");
list$1.removeColumn("state");
list$1.removeColumn("shipped");
list$1.column("isCompleted", {label: "@laola.request.fields.isCompleted", index: 2, width: 180}).boolean();
list$1.column("offer", {label: "@laola.request.fields.offer", index: 3, width: 240}).custom((value) => {
  if (!value) {
    return '<svg class="ui-icon ui-table-field-bool" width="16" height="16" stroke-width="2" data-symbol="fth-x"><use xlink:href="#fth-x"></use></svg>';
  }
  var date2 = Strings.date(value.date);
  var image2 = value.imageId ? MediaApi.getImageSource(value.imageId) : null;
  return `<span class="laola-requests-offer">
    ${image2 ? `<img src="${image2}" class="-image">` : '<span class="-image -empty"><svg class="ui-icon" width="16" height="16" stroke-width="2" data-symbol="fth-user"><use xlink:href="#fth-user"></use></svg></span>'}
    <span>
      ${date2}<br>
      <span class="-minor">#${value.number}</span>
    </span>
   </span>`;
}, true);
list$1.columns.find((x) => x.path === "customer").options.width = null;
list$1.columns.find((x) => x.path === "number").options.label = "@laola.request.name";
list$1.columns.find((x) => x.path === "price").options.width = 180;
var lists = {
  spacesTeam: list$7,
  discounts: list$6,
  campaigns: list$5,
  sizecharts: list$4,
  vouchers: list$3,
  experiences: list$2,
  requests: list$1
};
let routes = [];
const settings = __zero.sections.find((x) => x.alias === __zero.alias.sections.settings);
const commerce = __zero.sections.find((x) => x.alias === "commerce");
if (settings) {
  routes.push({
    name: "laola-settings-layout",
    path: settings.url + "/laola-layout",
    component: () => __vitePreload(() => __import__("./layout.js"), true ? ["/zero/layout.js","/zero/layout.css","/zero/vendor.js"] : void 0),
    meta: {
      name: "@laola.settings.layout.name"
    }
  });
}
if (commerce) {
  routes.push({
    name: "commerce-sizecharts",
    path: settings.url + "/commerce-sizecharts",
    props: true,
    component: () => __vitePreload(() => __import__("./sizecharts.js"), true ? ["/zero/sizecharts.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Size charts"
    }
  });
  routes.push({
    name: "commerce-sizecharts-create",
    path: settings.url + "/commerce-sizecharts/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./sizechart.js"), true ? ["/zero/sizechart.js","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: "Size chart"
    }
  });
  routes.push({
    name: "commerce-sizecharts-edit",
    path: settings.url + "/commerce-sizecharts/edit/:id/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./sizechart.js"), true ? ["/zero/sizechart.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Size chart"
    }
  });
  routes.push({
    name: "commerce-vouchers",
    path: settings.url + "/commerce-vouchers",
    props: true,
    component: () => __vitePreload(() => __import__("./vouchers.js"), true ? ["/zero/vouchers.js","/zero/vouchers.css","/zero/vendor.js"] : void 0),
    meta: {
      name: "Vouchers"
    }
  });
  routes.push({
    name: "commerce-vouchers-create",
    path: settings.url + "/commerce-vouchers/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./voucher.js"), true ? ["/zero/voucher.js","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: "Voucher"
    }
  });
  routes.push({
    name: "commerce-vouchers-edit",
    path: settings.url + "/commerce-vouchers/edit/:id/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./voucher.js"), true ? ["/zero/voucher.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Voucher"
    }
  });
  routes.push({
    name: "commerce-discounts",
    path: "/commerce/discounts",
    props: true,
    component: () => __vitePreload(() => __import__("./discounts.js"), true ? ["/zero/discounts.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Discounts"
    }
  });
  routes.push({
    name: "commerce-discounts-create",
    path: "/commerce/discounts/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./discount.js"), true ? ["/zero/discount.js","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: "Discounts"
    }
  });
  routes.push({
    name: "commerce-discounts-edit",
    path: "/commerce/discounts/edit/:id/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./discount.js"), true ? ["/zero/discount.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Discounts"
    }
  });
  routes.push({
    name: "commerce-campaigns",
    path: "/commerce/campaigns",
    props: true,
    component: () => __vitePreload(() => __import__("./campaigns.js"), true ? ["/zero/campaigns.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Campaigns"
    }
  });
  routes.push({
    name: "commerce-campaigns-create",
    path: "/commerce/campaigns/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./campaign.js"), true ? ["/zero/campaign.js","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: "Campaigns"
    }
  });
  routes.push({
    name: "commerce-campaigns-edit",
    path: "/commerce/campaigns/edit/:id/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./campaign.js"), true ? ["/zero/campaign.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Campaigns"
    }
  });
  routes.push({
    name: "commerce-requests",
    path: "/commerce/requests",
    props: true,
    component: () => __vitePreload(() => __import__("./requests.js"), true ? ["/zero/requests.js","/zero/requests.css","/zero/vendor.js"] : void 0),
    meta: {
      name: "Requests"
    }
  });
  routes.push({
    name: "commerce-requests-edit",
    path: "/commerce/requests/edit/:id/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./request.js"), true ? ["/zero/request.js","/zero/request.css","/zero/order.js","/zero/order.css","/zero/vendor.js"] : void 0),
    meta: {
      name: "Request"
    }
  });
  routes.push({
    name: "commerce-experiences",
    path: settings.url + "/commerce-experiences",
    props: true,
    component: () => __vitePreload(() => __import__("./experiences.js"), true ? ["/zero/experiences.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Experiences"
    }
  });
  routes.push({
    name: "commerce-experiences-create",
    path: settings.url + "/commerce-experiences/create/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./experience.js"), true ? ["/zero/experience.js","/zero/vendor.js"] : void 0),
    meta: {
      create: true,
      name: "Experience"
    }
  });
  routes.push({
    name: "commerce-experiences-edit",
    path: settings.url + "/commerce-experiences/edit/:id/:scope?",
    props: true,
    component: () => __vitePreload(() => __import__("./experience.js"), true ? ["/zero/experience.js","/zero/vendor.js"] : void 0),
    meta: {
      name: "Experience"
    }
  });
}
var render$1k = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-add-button"}, [!_vm.hasDropdown ? _c("ui-button", {attrs: {type: _vm.type, label: _vm.label, disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.onClick(false);
  }}}) : _c("ui-dropdown", {ref: "dropdown", attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {label: _vm.label, type: _vm.type, disabled: _vm.disabled}})];
  }, proxy: true}])}, [_c("div", {staticClass: "laola-add-button-items"}, [_c("button", {staticClass: "laola-add-button-item", attrs: {type: "button", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.onClick(true);
  }}}, [_c("ui-icon", {attrs: {symbol: "fth-cloud", size: 20}}), _c("span", {staticClass: "-text"}, [_vm._v("Blueprint")])], 1), _c("span", {staticClass: "laola-add-button-items-line"}), _c("button", {staticClass: "laola-add-button-item", attrs: {type: "button", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.onClick(false);
  }}}, [_c("ui-icon", {attrs: {symbol: "fth-arrow-right", size: 20}}), _c("span", {staticClass: "-text"}, [_vm._v("For " + _vm._s(_vm.application.name))])], 1)])])], 1);
};
var staticRenderFns$1k = [];
var addButton_vue_vue_type_style_index_0_lang$1 = ".laola-add-button {\n  display: flex;\n}\n\n/*.laola-add-button .ui-dropdown-button\n{\n  font-weight: 700;\n}*/\n.laola-add-button-items {\n  display: grid;\n  grid-template-columns: 1fr 1px 1fr;\n}\n.laola-add-button-item {\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  padding: 20px 10px;\n  font-size: var(--font-size);\n  border-radius: var(--radius);\n}\n.laola-add-button-item .ui-icon {\n  margin-bottom: 12px;\n}\n.laola-add-button-item .is-primary {\n  font-size: 24px;\n  color: var(--color-text);\n}\n.laola-add-button-item:hover {\n  background: var(--color-dropdown-selected);\n}\n.laola-add-button-items-line {\n  display: block;\n  height: 100%;\n  background: rgba(255, 255, 255, 0.05);\n}";
const appAwareEntities = [
  "countries",
  "languages",
  "translations",
  "mailTemplates",
  "commerce-currencies",
  "commerce-numbers",
  "commerce-orderstates",
  "commerce-taxes",
  "commerce-manufacturers",
  "commerce-properties",
  "commerce-products"
];
const script$1k = {
  overrides: "add-button",
  props: {
    label: {
      type: String,
      default: "@ui.add"
    },
    type: {
      type: String,
      default: "primary"
    },
    route: {
      type: [String, Object],
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    application() {
      return find(zero.applications, (x) => x.id === zero.appId);
    },
    routeName() {
      return this.route && typeof this.route === "object" ? this.route.name : this.route;
    },
    hasDropdown() {
      if (this.routeName && appAwareEntities.find((x) => this.routeName.indexOf(x + "-") === 0)) {
        return true;
      }
      return false;
    }
  },
  methods: {
    onClick(isShared) {
      if (this.$refs.dropdown) {
        this.$refs.dropdown.hide();
      }
      if (!!this.route) {
        let routeObj = typeof this.route === "object" ? this.route : {name: this.route};
        routeObj.params = routeObj.params || {};
        routeObj.query = routeObj.query || {};
        if (isShared) {
          routeObj.query.scope = "shared";
        }
        this.$router.push(routeObj);
      }
      this.$emit("click", false);
    }
  }
};
const __cssModules$1k = {};
var component$1k = normalizeComponent(script$1k, render$1k, staticRenderFns$1k, false, injectStyles$1k, null, null, null);
function injectStyles$1k(context) {
  for (let o in __cssModules$1k) {
    this[o] = __cssModules$1k[o];
  }
}
component$1k.options.__file = "../../Laola/Laola.Backoffice/Plugin/components/add-button.vue";
var addButton = component$1k.exports;
var componentOverrides = {
  addButton
};
function application(zero2) {
  const prefix2 = "@laola.settings.application.fields.";
  const editor2 = zero2.getEditor("application");
  const documents = editor2.tab("documents", "@laola.settings.application.tab_documents");
  documents.field("documentSettings.imageId", {label: prefix2 + "imageId", description: prefix2 + "imageId_text"}).image();
  documents.field("documentSettings.address", {label: prefix2 + "address", description: prefix2 + "address_text"}).text(100).required();
  documents.field("documentSettings.footerText", {label: prefix2 + "footerText", description: prefix2 + "footerText_text"}).text(140);
}
const editor$2 = new Editor("laola.channel.reminder", "@laola.channel.reminder.fields.");
editor$2.field("scheduledForDate").datePicker().required();
editor$2.field("email").text().required();
editor$2.field("text").textarea().required();
const partEditor = new Editor("laola.channel.setpart", "@laola.channel.setpart.fields.");
partEditor.field("name").text(30).required();
partEditor.field("categoryIds").custom(CategoryPicker, {limit: 10, channel: (x, meta) => meta.parentModel.parentModel.id}).required();
const editor$1 = new Editor("laola.channel.set", "@laola.channel.set.fields.");
editor$1.field("name").text(30).required();
editor$1.field("isManfacturerScoped").toggle();
editor$1.field("parts").nested(partEditor, {
  title: "@laola.channel.setpart.title",
  limit: 3,
  itemIcon: "fth-box",
  itemLabel: (x) => x.name,
  itemDescription: (x) => Localization.localize(x.categoryIds.length === 1 ? "@laola.channel.setpart.categories_1" : "@laola.channel.setpart.categories_x", {tokens: {count: x.categoryIds.length}}),
  template: {
    name: null,
    categoryIds: []
  }
}).required();
function channel(zero2) {
  const prefix2 = "@laola.channel.fields.";
  const editor2 = zero2.getEditor("commerce.channel");
  const general2 = editor2.tab("general");
  const settings2 = editor2.tab("settings");
  const theme = editor2.tab("theme", "@laola.channel.theme_tab");
  const documentSettings = editor2.tab("documents", "@laola.channel.documents_tab");
  const CHECKOUT_TYPE_VALUES = [
    {value: prefix2 + "checkoutTypes.orders", key: "orders"},
    {value: prefix2 + "checkoutTypes.requests", key: "requests"},
    {value: prefix2 + "checkoutTypes.ordersAndRequests", key: "ordersAndRequests"},
    {value: prefix2 + "checkoutTypes.collectiveRequests", key: "collectiveRequests"},
    {value: prefix2 + "checkoutTypes.none", key: "none"}
  ];
  general2.field("shortName", {label: prefix2 + "shortName", description: prefix2 + "shortName_text"}).text(30);
  general2.field("clientNo", {label: prefix2 + "clientNo", description: prefix2 + "clientNo_text"}).text(30);
  general2.field("content.isPrivate", {label: prefix2 + "content.isPrivate", description: prefix2 + "content.isPrivate_text"}).toggle();
  general2.field("checkoutType", {label: prefix2 + "checkoutType", description: prefix2 + "checkoutType"}).select(CHECKOUT_TYPE_VALUES).required();
  theme.field("theme.title", {label: prefix2 + "title", description: prefix2 + "title_text"}).text(30, (x) => x.name);
  theme.field("theme.hideSubtitle", {label: prefix2 + "hideSubtitle", description: prefix2 + "hideSubtitle_text"}).toggle();
  theme.field("theme.subtitle", {label: prefix2 + "subtitle", description: prefix2 + "subtitle_text"}).when((x) => !x.theme.hideSubtitle).text(30, "Vereinsshop");
  theme.field("theme.colorAccent", {label: prefix2 + "colorAccent", description: prefix2 + "colorAccent_text"}).colorPicker().required();
  theme.field("theme.colorAccentText", {label: prefix2 + "colorAccentText", description: prefix2 + "colorAccentText_text"}).colorPicker();
  settings2.field("reminders", {label: prefix2 + "reminders", description: prefix2 + "reminders_text"}).nested(editor$2, {
    title: "@laola.channel.reminder.title",
    limit: 5,
    itemIcon: "fth-mail",
    itemLabel: (x) => Strings.date(x.scheduledForDate, "short"),
    itemDescription: (x) => x.text,
    template: {
      id: null,
      scheduledForDate: null,
      text: null,
      email: null
    }
  });
  settings2.field("content.sets", {label: prefix2 + "content.sets", description: prefix2 + "content.sets_text"}).nested(editor$1, {
    title: "@laola.channel.set.title",
    limit: 100,
    itemIcon: "fth-box",
    itemLabel: (x) => x.name,
    itemDescription: (x) => x.parts.filter((p2) => !!p2.name).map((p2) => p2.name).join(", "),
    template: {
      id: null,
      alias: null,
      name: null,
      parts: []
    }
  });
  documentSettings.field("showChannelInDocumentAddressField", {label: prefix2 + "showChannelInDocumentAddressField", description: prefix2 + "showChannelInDocumentAddressField_text"}).toggle();
}
const editor = new Editor("laola.product-label", "@laola.product.fields.labelling.");
const prefix = "@laola.product.fields.labelling.";
const LABEL_TYPE_VALUES = [
  {value: prefix + "types.output", key: "output"},
  {value: prefix + "types.text", key: "text"},
  {value: prefix + "types.toggle", key: "toggle"},
  {value: prefix + "types.select", key: "select"}
];
editor.field("name").text(40).required();
editor.field("type").select(LABEL_TYPE_VALUES).required();
editor.field("description").text(40);
editor.field("isRequired").toggle().when((x) => x.type !== "output" && x.type !== "toggle");
editor.field("price").currency().required().when((x) => x.type !== "output");
editor.field("maxInputLength").number(2).when((x) => x.type === "text" || x.type === "number");
editor.field("options").inputList(100, 40, prefix + "options_add").when((x) => x.type === "select");
var render$1j = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-sizechartpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$1j = [];
const script$1j = {
  name: "laolaSizeChartPicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "sizechart",
      items: SizeChartsApi.getForPicker,
      previews: SizeChartsApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$1j = {};
var component$1j = normalizeComponent(script$1j, render$1j, staticRenderFns$1j, false, injectStyles$1j, null, null, null);
function injectStyles$1j(context) {
  for (let o in __cssModules$1j) {
    this[o] = __cssModules$1j[o];
  }
}
component$1j.options.__file = "../../Laola/Laola.Backoffice/Plugin/components/sizechartpicker.vue";
var SizeChartPicker = component$1j.exports;
function product(zero2) {
  const prefix2 = "@laola.product.fields.";
  let opts = (key) => {
    return {label: prefix2 + key, description: prefix2 + key + "_text"};
  };
  const labelTypeMap = {
    text: "fth-edit",
    output: "fth-type",
    number: "fth-edit",
    toggle: "fth-toggle-right",
    select: "fth-check-square"
  };
  const LABEL_TYPE_VALUES2 = [
    {value: prefix2 + "labelling.types.output", key: "output"},
    {value: prefix2 + "labelling.types.text", key: "text"},
    {value: prefix2 + "labelling.types.number", key: "number"},
    {value: prefix2 + "labelling.types.toggle", key: "toggle"},
    {value: prefix2 + "labelling.types.select", key: "select"}
  ];
  const requiredText = Localization.localize("@ui.required");
  const editor$12 = zero2.getEditor("commerce.product");
  const relations2 = editor$12.tab("relations");
  const general2 = editor$12.tab("general");
  const availability2 = editor$12.tab("availability");
  const labelling = editor$12.tab("labelling", "@laola.product.labelling_tab", (x) => x.labelling.isEnabled ? x.labelling.items.length : 0, (x) => x.type !== "physical");
  const seo2 = editor$12.tab("seo", "@laola.tabs.meta");
  partials.addSeo(seo2, "seo.");
  general2.field("voucherImageId", opts("voucherImageId")).when((x) => x.type === "giftCard").image();
  relations2.field("sizeChartIds", opts("sizeChartIds")).custom(SizeChartPicker, {limit: 3});
  availability2.field("runtimeDate", opts("runtimeDate")).when((x) => x.type === "physical").datePicker();
  labelling.field("labelling.isEnabled", opts("labelling.isEnabled")).toggle();
  labelling.field("labelling.items", opts("labelling.items")).when((x) => x.labelling.isEnabled).nested(editor, {
    limit: 10,
    title: "@laola.product.fields.labelling.title",
    itemLabel: (x) => x.name,
    itemDescription: (x) => {
      let value = LABEL_TYPE_VALUES2.find((v) => v.key === x.type);
      return Localization.localize(value.value) + (x.price > 0 && x.type !== "output" ? " - " + Strings.currency(x.price) : "") + (x.isRequired ? " - " + requiredText : "");
    },
    itemIcon: (x) => labelTypeMap[x.type],
    template: {id: null, name: null, description: null, type: "text", isRequired: false, price: 0, maxInputLength: null, options: []}
  }).required();
}
function filter(zero2) {
  let opts = (key) => {
    return {label: "@laola.filter.fields." + key, description: "@laola.filter.fields." + key + "_text"};
  };
  const editor2 = zero2.getEditor("commerce.filter");
  const pageTitle = editor2.tab("pageTitle", "@laola.filter.pageTitle_tab");
  pageTitle.field("pageTitle.isHidden", opts("pageTitle.isHidden")).toggle();
  pageTitle.field("pageTitle.valueLimit", opts("pageTitle.valueLimit")).when((x) => !x.pageTitle.isHidden).number(2);
  pageTitle.field("pageTitle.prefixPropertyName", opts("pageTitle.prefixPropertyName")).when((x) => !x.pageTitle.isHidden).toggle();
  pageTitle.field("pageTitle.lowercaseValues", opts("pageTitle.lowercaseValues")).when((x) => !x.pageTitle.isHidden).toggle();
  pageTitle.field("pageTitle.prefix", opts("pageTitle.prefix")).when((x) => !x.pageTitle.isHidden).text(10);
  pageTitle.field("pageTitle.isBeforeCategory", opts("pageTitle.isBeforeCategory")).when((x) => !x.pageTitle.isHidden).toggle();
  pageTitle.field("pageTitle.position", opts("pageTitle.position")).when((x) => !x.pageTitle.isHidden).number(3);
}
function category(zero2) {
  const prefix2 = "@laola.category.fields.";
  const editor2 = zero2.getEditor("commerce.category");
  const content2 = editor2.tab("general");
  content2.field("hideInNavigation", {label: prefix2 + "hideInNavigation", description: prefix2 + "hideInNavigation_text"}).toggle();
  content2.field("productPreviewImageId", {label: prefix2 + "productPreviewImageId", description: prefix2 + "productPreviewImageId_text"}).image();
  const seo2 = editor2.tab("seo", "@laola.tabs.meta");
  partials.addSeo(seo2, "seo.");
}
const list = new List("commerce.customer-requests");
list.templateLabel = (x) => "@laola.request.fields." + x;
list.link = "commerce-requests-edit";
list.query.pageSize = 10;
list.onFetch((filter2) => RequestsApi.getByQuery(filter2));
list.column("number", {class: "is-bold"}).custom((value, model) => model.number);
list.column("isCompleted", {label: "@laola.request.fields.isCompleted"}).boolean();
list.column("countOffers", {label: "@laola.request.fields.countOffers"}).boolean((x) => x > 0);
list.column("price").custom((value, model) => Strings.currency(value), true);
list.column("createdDate").created();
var render$1i = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-customer-orders"}, [_c("ui-table", {ref: "table", attrs: {config: _vm.list, inline: true}, on: {loaded: _vm.onTableChange}})], 1);
};
var staticRenderFns$1i = [];
const script$1i = {
  name: "laolaCustomerRequests",
  props: {
    value: {
      type: String,
      default: null
    },
    entity: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    list
  }),
  watch: {
    $route(to, from) {
      this.rebuild();
    }
  },
  created() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.list.onFetch((filter2) => RequestsApi.getListByQuery(filter2, true, this.entity.id));
      if (this.$refs.table) {
        this.$refs.table.initialize();
      }
    },
    onTableChange(result) {
      let parent = this.$parent;
      while (parent != null && parent.$options.name !== "uiTab") {
        parent = parent.$parent;
      }
      if (parent != null) {
        parent.setCount(result.totalItems);
      }
    }
  }
};
const __cssModules$1i = {};
var component$1i = normalizeComponent(script$1i, render$1i, staticRenderFns$1i, false, injectStyles$1i, null, null, null);
function injectStyles$1i(context) {
  for (let o in __cssModules$1i) {
    this[o] = __cssModules$1i[o];
  }
}
component$1i.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/requests/partials/customer-requests.vue";
var RequestsList = component$1i.exports;
function customer(zero2) {
  const editor2 = zero2.getEditor("commerce.customer");
  const requests = editor2.tab("requests", "@laola.customer.requests_tab");
  requests.field("key", {hideLabel: true}).when((x) => x.id).custom(RequestsList);
}
function propertyValueCreator(zero2) {
}
var overrides = [application, channel, product, filter, category, customer, propertyValueCreator];
var render$1h = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-block-products"}, [_vm.previews.length < 1 ? _c("product-picker", {attrs: {limit: 4}, model: {value: _vm.vals, callback: function($$v) {
    _vm.vals = $$v;
  }, expression: "vals"}}) : _vm._e(), _vm.previews.length > 0 ? _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated}, expression: "{ onUpdate: onSortingUpdated }"}], staticClass: "ui-block-products-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("figure", {key: preview.sortId, staticClass: "ui-block-products-preview"}, [_c("div", {staticClass: "ui-block-products-preview-image-outer"}, [preview.image ? _c("img", {staticClass: "ui-block-products-preview-image", attrs: {src: preview.image}}) : _vm._e()]), _c("figcaption", {staticClass: "ui-block-products-preview-caption"}, [_c("strong", {staticClass: "ui-block-products-preview-name"}, [_vm._v(_vm._s(preview.name))]), _c("span", {staticClass: "ui-block-products-preview-text"}, [_vm._v(_vm._s(preview.categories[0].name))])])]);
  }), 0) : _vm._e()], 1);
};
var staticRenderFns$1h = [];
var products_vue_vue_type_style_index_0_lang = ".ui-block-content.ui-block-products {\n  margin: 0 16px;\n}\n.ui-block-products-add {\n  width: 100%;\n  height: 300px;\n  /*background: var(--color-box-nested);*/\n  border: 1px dashed var(--color-line-dashed);\n  border-radius: var(--radius);\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n}\n.ui-block-products-previews {\n  display: grid;\n  grid-template-columns: repeat(3, minmax(0, 1fr));\n  grid-gap: var(--padding-s);\n}\n.ui-block-products-preview {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  border: 1px solid var(--color-line);\n  border-radius: var(--radius);\n  width: 100%;\n  margin: 0;\n  background: var(--color-box);\n}\n.ui-block-products-preview-caption {\n  margin: 0;\n  background: var(--color-box-nested);\n  width: 100%;\n  padding: var(--padding-s);\n  border-top: 1px solid var(--color-line);\n}\n.ui-block-products-preview-image-outer {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  height: 100px;\n  margin: var(--padding-m) 0;\n}\n.ui-block-products-preview-image {\n  max-width: 80px;\n  max-height: 80px;\n}\n.ui-block-products-preview-text {\n  display: block;\n  color: var(--color-text-dim);\n  margin-top: 6px;\n}";
const script$1h = {
  name: "uiBlockProducts",
  components: {ProductPicker},
  props: {
    value: {
      type: Object,
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  watch: {
    "value.items": function() {
      this.loadPreviews();
    }
  },
  data: () => ({
    previews: [],
    vals: []
  }),
  mounted() {
    this.loadPreviews();
  },
  methods: {
    loadPreviews() {
      Promise.all(this.value.items.map((x) => ProductsApi.getPreview(x.productId, x.variantId))).then((values2) => {
        this.previews = values2.map((x) => {
          x.sortId = Strings.guid();
          x.image = x.image ? MediaApi.getImageSource(x.image, false) : null;
          return x;
        });
      });
    },
    onSortingUpdated(ev) {
    }
  }
};
const __cssModules$1h = {};
var component$1h = normalizeComponent(script$1h, render$1h, staticRenderFns$1h, false, injectStyles$1h, null, null, null);
function injectStyles$1h(context) {
  for (let o in __cssModules$1h) {
    this[o] = __cssModules$1h[o];
  }
}
component$1h.options.__file = "../../Laola/Laola.Backoffice/Plugin/components/storyblocks/products.vue";
var StoriesBlockProducts = component$1h.exports;
var _lists = ":root {\n  --color-synced: #ffac00;\n}\n\n.ui-table-field-synced {\n  margin-left: 8px;\n  color: var(--color-text-dim-one);\n  font-size: 11px;\n  margin-top: -2px;\n}\n.ui-table-field-synced:hover {\n  color: var(--color-text);\n}\n\n.space-list[data-space=team] img.ui-table-field-image {\n  border-radius: 50px;\n}\n\n.space-list[data-space=team] .ui-table-cell[table-field=imageId] {\n  padding: 12px;\n  padding-left: 20px;\n}";
var _modules = '[data-module="page.headline"] .ui-module-preview-text .-text,\n[data-module="page.image"] .ui-module-preview-figure .-text {\n  font-weight: bold;\n  font-size: var(--font-size-l);\n}\n\n[data-module="page.line"] .ui-module-item-header {\n  display: none;\n}\n\n[data-module="page.line"] hr {\n  border-bottom-width: 3px;\n  border-bottom-color: var(--color-line-onbg);\n}\n\n[data-module="page.line"] .ui-module-item-content {\n  padding-top: 5px;\n  padding-bottom: 5px;\n}\n\n[data-module="page.line"] .ui-module-preview-inner {\n  padding-top: 0 !important;\n}';
const base = "blueprint/";
var BlueprintApi = {
  getById: async (id) => await get$1(base + "getById", {params: {id}})
};
var Notification = new Vue({
  data: () => ({
    instances: []
  }),
  methods: {
    success(label, text2, options2) {
      this.show(extend({
        type: "success",
        label,
        text: text2
      }, options2 || {}));
    },
    error(label, text2, options2) {
      this.show(extend({
        type: "error",
        label,
        text: text2
      }, options2 || {}));
    },
    show(options2, text2) {
      let me = this;
      if (typeof options2 === "string") {
        options2 = {
          label: options2,
          text: text2
        };
      }
      options2 = extend({
        id: Strings.guid(),
        type: "default",
        label: null,
        text: null,
        persistent: false,
        duration: 3e3,
        timeout: null,
        close: () => {
          clearTimeout(options2.timeout);
          me.close(this.instances.indexOf(options2));
        }
      }, options2);
      if (!options2.label && !options2.text) {
        return;
      }
      options2.timeout = setTimeout(() => {
        options2.close();
      }, options2.duration);
      this.instances.push(options2);
    },
    close(index) {
      if (index > -1) {
        this.instances.splice(index, 1);
      } else {
        this.instances = [];
      }
    }
  }
});
var render$1g = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "blueprint-settings", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "Synchronisation", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.confirm", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {staticClass: "blueprint-settings-text"}, [_vm._v("By default all properties of your entity are synced with its blueprint."), _c("br"), _vm._v("You can disable synchronisation per property so it won't be overridden on changes.")]), _c("div", {staticClass: "ui-box"}, _vm._l(_vm.items, function(item2, index) {
    return _c("ui-property", {key: index, class: {"not-synced": !item2.synced}, attrs: {label: item2.label, vertical: false}}, [_c("ui-toggle", {attrs: {value: item2.synced}, on: {input: function($event) {
      return _vm.onChange(item2);
    }}})], 1);
  }), 1)]);
};
var staticRenderFns$1g = [];
var settings_vue_vue_type_style_index_0_lang = `.blueprint-settings-text {
  margin: 0 0 var(--padding);
  line-height: 1.5;
}
.blueprint-settings-headline {
  margin: 0 0 var(--padding) !important;
}
.blueprint-settings .ui-property {
  display: flex;
  justify-content: space-between;
}
.blueprint-settings .ui-property + .ui-property {
  margin-top: 20px;
  border-top: none;
  padding-top: 0;
}
.blueprint-settings .ui-property-content {
  display: inline;
  flex: 0 0 auto;
}
.blueprint-settings .ui-property-label {
  padding-top: 1px;
}
.blueprint-settings .ui-property.not-synced .ui-property-label {
  font-weight: 400;
}

/*.blueprint-settings .ui-property.not-synced .ui-property-label:before
{
  content: "\\e929";
  font-family: 'Feather';
  margin-right: 0.8em;
  color: var(--color-text-dim);
  font-weight: 400;
}

.blueprint-settings .ui-property:not(.not-synced) .ui-property-label:before
{
  content: "\\e8f8";
  font-family: 'Feather';
  margin-right: 0.8em;
  color: var(--color-primary);
  font-weight: 400;
}*/`;
const script$1g = {
  props: {
    isCopy: {
      type: Boolean,
      default: false
    },
    model: Object,
    config: Object
  },
  data: () => ({
    desync: [],
    items: [],
    state: "default",
    editor: null
  }),
  mounted() {
    this.desync = JSON.parse(JSON.stringify(this.model.desync));
    this.editor = this.config.editor;
    let processed = ["blueprint"];
    this.editor.fields.forEach((field) => {
      if (processed.indexOf(field.path) < 0) {
        processed.push(field.path);
        this.items.push({
          path: field.path,
          label: field.options.label || this.editor.templateLabel(field.path),
          synced: this.model.desync.indexOf(field.path) < 0
        });
      }
    });
  },
  methods: {
    onChange(item2) {
      item2.synced = !item2.synced;
      if (item2.synced) {
        Arrays.remove(this.model.desync, item2.path);
      } else {
        this.model.desync.push(item2.path);
      }
    },
    onSave() {
      this.state = "loading";
      let resync = [];
      this.desync.forEach((path) => {
        if (this.model.desync.indexOf(path) < 0) {
          resync.push(path);
        }
      });
      if (resync.length > 0) {
        BlueprintApi.getById(this.model.id).then((blueprint) => {
          this.config.confirm({
            blueprint: this.model,
            update: (entity) => {
              resync.forEach((path) => {
                entity[path] = blueprint[path];
              });
            }
          });
        });
      } else {
        this.config.confirm({
          blueprint: this.model
        });
      }
    }
  }
};
const __cssModules$1g = {};
var component$1g = normalizeComponent(script$1g, render$1g, staticRenderFns$1g, false, injectStyles$1g, null, null, null);
function injectStyles$1g(context) {
  for (let o in __cssModules$1g) {
    this[o] = __cssModules$1g[o];
  }
}
component$1g.options.__file = "../../Laola/Laola.Backoffice/Plugin/blueprints/settings.vue";
var SettingsOverlay = component$1g.exports;
var render$1f = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "blueprint-block", on: {click: _vm.onClick}});
};
var staticRenderFns$1f = [];
var block_vue_vue_type_style_index_0_lang = ".blueprint-block {\n  position: absolute;\n  left: 0;\n  right: 0;\n  top: -10px;\n  bottom: -10px;\n  background: transparent;\n  z-index: 3;\n}";
const script$1f = {
  props: {
    config: {
      type: Object,
      required: true
    },
    editor: {
      type: Object,
      required: true
    },
    value: {
      type: Object,
      required: true
    },
    model: {}
  },
  methods: {
    onClick() {
      Overlay.confirm({
        title: "Unlock property",
        text: "Unlock this property to override the value passed by the blueprint",
        confirmLabel: "Confirm",
        closeLabel: "Cancel"
      }).then(() => {
        this.value.blueprint.desync.push(this.config.path);
        this.$parent.$parent.setDisabled(false);
        this.$parent.$parent.setBlock(null);
      }, () => {
      });
    }
  }
};
const __cssModules$1f = {};
var component$1f = normalizeComponent(script$1f, render$1f, staticRenderFns$1f, false, injectStyles$1f, null, null, null);
function injectStyles$1f(context) {
  for (let o in __cssModules$1f) {
    this[o] = __cssModules$1f[o];
  }
}
component$1f.options.__file = "../../Laola/Laola.Backoffice/Plugin/blueprints/block.vue";
var render$1e = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "blueprint", class: {"is-active": _vm.entity.isActive, "is-shared": _vm.isBlueprint}}, [_vm.hasBlueprint && !_vm.isBlueprint ? [_c("div", {staticClass: "blueprint-inner"}, [_c("ui-icon", {attrs: {symbol: "fth-cloud", size: 20}}), _vm._m(0)], 1), _c("aside", [_c("router-link", {staticClass: "ui-button type-light type-small", attrs: {replace: "", to: _vm.switchLink}}, [_vm._v("Blueprint")]), _c("ui-button", {attrs: {type: "light small", label: _vm.value.desync.length > 0 ? _vm.value.desync.length + " unlocked" : "Settings"}, on: {click: _vm.openSettings}})], 1)] : _vm._e(), _vm.isBlueprint ? [_c("div", {staticClass: "blueprint-inner"}, [_c("ui-icon", {attrs: {symbol: "fth-cloud", size: 20}}), !_vm.isNewBlueprint ? _c("p", [_vm._v("This blueprint will "), _c("b", [_vm._v("automatically pass changes")]), _vm._v(" to its children.")]) : _vm._e(), _vm.isNewBlueprint ? _c("p", [_vm._v("This blueprint will create children in all registered apps and automatically pass changes to them.")]) : _vm._e()], 1), !_vm.isNewBlueprint ? _c("aside", [_c("router-link", {staticClass: "ui-button type-light type-small", attrs: {replace: "", to: _vm.switchLink}}, [_vm._v("View child")])], 1) : _vm._e()] : _vm._e()], 2);
};
var staticRenderFns$1e = [function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("p", [_c("b", [_vm._v("This entity is based on a blueprint")]), _vm._v(" and automatically synchronised."), _c("br")]);
}];
var property_vue_vue_type_style_index_0_lang$1 = `.ui-property.is-property-locked {
  pointer-events: none;
  opacity: 0.8;
}

/*.language .ui-property.has-block:after,
.mails .ui-property.has-block:after
{
  position: absolute;
  left: -13px;
  top: -7px;
  display: inline-flex;
  justify-content: center;
  align-items: center;
  width: 32px;
  height: 32px;
  border-radius: 16px;
  background: var(--color-box);
  content: "\\e887";
  font-family: 'Feather';
  font-size: 15px;
  font-weight: 400;
  color: var(--color-text-dim);
}*/
.blueprint {
  position: relative;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.blueprint-inner {
  display: grid;
  grid-template-columns: auto minmax(0, 1fr);
  grid-gap: 16px;
  align-items: center;
  font-size: var(--font-size);
  line-height: 1.5;
  position: relative;
}
.blueprint-inner p {
  margin: 0;
  color: var(--color-text);
}
.blueprint-inner .ui-icon {
  color: var(--color-synchronized);
  margin-top: -2px;
}`;
const script$1e = {
  props: {
    value: {
      type: Object
    },
    entity: {
      type: Object,
      required: true
    },
    meta: {
      type: Object,
      default: () => {
      }
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  inject: ["editor"],
  watch: {
    $route: function() {
      this.bind();
    }
  },
  computed: {
    hasBlueprint() {
      return !!this.value;
    },
    isBlueprint() {
      return this.entity && this.$route.query.scope === "shared";
    },
    isNewBlueprint() {
      return this.entity && !this.entity.id && this.$route.query.scope === "shared";
    },
    switchLink() {
      return {
        name: this.$route.name,
        params: this.$route.params,
        query: __assign(__assign({}, this.$route.query || {}), {
          scope: this.isBlueprint ? void 0 : "shared"
        })
      };
    }
  },
  mounted() {
    this.bind();
  },
  methods: {
    openSettings() {
      const editor2 = typeof this.editor === "string" ? this.zero.getEditor(this.editor) : this.editor;
      return Overlay.open({
        component: SettingsOverlay,
        display: "editor",
        model: this.value,
        editor: editor2
      }).then((res) => {
        this.$emit("input", res.blueprint);
        if (typeof res.update === "function") {
          res.update(this.entity);
        }
      });
    },
    bind() {
      const form = this.getForm();
      const onLoaded = () => {
        this.$nextTick(() => {
          this.setupSync(form);
        });
      };
      if (form.loadingState === "default") {
        onLoaded();
      } else {
        form.$on("loaded", onLoaded);
      }
    },
    setupSync(form) {
      const meta = form.$parent.meta;
      if (meta) {
        meta.canDelete = this.isBlueprint;
        meta.canEdit = this.isBlueprint;
      }
      if (this.hasBlueprint) {
        const properties = this.getProperties(form);
        properties.forEach((property) => {
          if (property.config.path !== "blueprint")
            ;
        });
      }
    },
    getForm() {
      let component2 = this.$parent;
      do {
        if (component2.$options.name === "uiForm") {
          return component2;
        }
      } while (component2 = component2.$parent);
      return null;
    },
    getProperties(form) {
      let find2 = "uiEditorComponent";
      let components2 = [];
      let traverseChildren = (parent) => {
        parent.$children.forEach((component2) => {
          if (component2.$options.name === find2) {
            components2.push(component2);
          } else {
            traverseChildren(component2);
          }
        });
      };
      traverseChildren(form);
      return components2;
    }
  }
};
const __cssModules$1e = {};
var component$1e = normalizeComponent(script$1e, render$1e, staticRenderFns$1e, false, injectStyles$1e, null, null, null);
function injectStyles$1e(context) {
  for (let o in __cssModules$1e) {
    this[o] = __cssModules$1e[o];
  }
}
component$1e.options.__file = "../../Laola/Laola.Backoffice/Plugin/blueprints/property.vue";
var BlueprintProperty = component$1e.exports;
const plugin = new Plugin("laola");
plugin.addEditors(editors);
plugin.addLists(lists);
plugin.addRoutes(routes);
plugin.install = (vue, zero2) => {
  const spaces = __zero.sections.find((x) => x.alias === "spaces");
  if (spaces) {
    __zero.sections.splice(__zero.sections.indexOf(spaces), 1);
  }
  const commerce2 = __zero.sections.find((x) => x.alias === "commerce");
  if (commerce2) {
    commerce2.children.splice(1, 0, {
      alias: "requests",
      name: "@laola.request.list",
      url: "/commerce/requests"
    });
    commerce2.children.push({
      alias: "discounts",
      name: "@laola.discounts.list",
      url: "/commerce/discounts"
    });
    commerce2.children.push({
      alias: "campaigns",
      name: "@laola.campaign.list",
      url: "/commerce/campaigns"
    });
    var promotions = commerce2.children.find((x) => x.alias === "promotions");
    if (promotions) {
      commerce2.children.splice(commerce2.children.indexOf(promotions), 1);
    }
  }
  zero2.stories.modules.push({
    alias: "stories.products",
    component: StoriesBlockProducts
  });
  Object.entries(componentOverrides).forEach((cmp) => {
    const component2 = cmp[1].default || cmp[1];
    if (component2.overrides) {
      window.zero.overrides[component2.overrides] = component2;
    } else {
      vue.component(cmp[0], component2);
    }
  });
  overrides.forEach((override) => {
    override(zero2);
  });
  var blueprintEditors = [
    "language",
    "country",
    "mailTemplate",
    "commerce.category",
    "commerce.currency",
    "commerce.manufacturer",
    "commerce.number",
    "commerce.orderstate",
    "commerce.property",
    "commerce.tax"
  ];
  blueprintEditors.forEach((alias2) => {
    const editor2 = zero2.getEditor(alias2);
    if (editor2) {
      editor2.fields.splice(0, 0, new EditorField("blueprint", {
        allTabs: true,
        hideLabel: true,
        class: "blueprint-property"
      }).when((x, cmp) => x.blueprint || cmp.$route.query.scope == "shared").custom(BlueprintProperty));
    }
  });
};
var plugins = [plugin$3, plugin$2, plugin$1, plugin];
var mediaConfig = {
  defaults: {
    images: [".jpg", ".jpeg", ".png", ".webp", ".svg"],
    images_natural: [".jpg", ".jpeg", ".webp"],
    images_artificial: [".png", ".webp", ".svg"]
  }
};
var linksConfig = {
  areas: []
};
const beforeEach = (to, from, next) => {
  let isGuarded = false;
  let callback = () => {
    let title = Localization.localize("@zero.name");
    let name = to.meta.name;
    if (!name && to.matched.length > 1) {
      to.matched.forEach((route) => {
        if (!name && route.meta.name) {
          name = route.meta.name;
        }
      });
    }
    if (!name || to.meta.alias === __zero.alias.sections.dashboard) {
      document.title = title;
      next();
      return;
    }
    let nameParts = isArray(name) ? name : [name];
    let translations = [];
    nameParts.forEach((part) => {
      if (part) {
        translations.push(Localization.localize(part));
      }
    });
    title += ": " + translations.join(" - ");
    document.title = title;
    next();
  };
  if (from.matched.length && from.matched[0].instances) {
    let instance = from.matched[0].instances.default;
    if (instance.$refs["form"] && typeof instance.$refs.form.beforeRouteLeave === "function") {
      isGuarded = true;
      instance.$refs.form.beforeRouteLeave(to, from, (res) => {
        if (res === false) {
          next(false);
        } else {
          callback();
        }
      });
    }
  }
  if (!isGuarded) {
    callback();
  }
};
var routerConfig = {
  mode: "history",
  base: __zero.path,
  linkActiveClass: "is-active",
  linkExactActiveClass: "is-active-exact",
  beforeEach,
  scrollBehavior(to, from, savedPosition) {
    return savedPosition ? savedPosition : {x: 0, y: 0};
  }
};
class Zero {
  constructor(vue, opts) {
    this.config = {};
    this._vue = null;
    this._plugins = [];
    this._editors = [];
    this._lists = [];
    this._routes = [];
    this._router = null;
    this._setupDone = false;
    let initialConfig = JSON.parse(document.getElementById("zeroconfig").innerHTML);
    this.config = __assign(__assign(__assign({}, initialConfig), {
      media: mediaConfig,
      linkPicker: linksConfig
    }), opts || {});
    console.info(this.config);
    this._vue = vue;
    this.use(plugin$4);
  }
  get version() {
    return this.config.version;
  }
  get plugins() {
    return this._plugins;
  }
  get router() {
    return this._router;
  }
  reloadConfig(config2) {
    return axios.get("zerovue/config").then((res) => {
      this.config = __assign(__assign(__assign({}, res.data), {
        media: mediaConfig,
        linkPicker: linksConfig
      }), config2 || {});
    });
  }
  setup() {
    this._router = new VueRouter(__assign(__assign({}, routerConfig), {
      routes: this._routes
    }));
    this._router.beforeEach(routerConfig.beforeEach);
    this._setupDone = true;
  }
  useByPath(pluginPath) {
  }
  use(plugin2) {
    if (typeof plugin2.install === "function") {
      plugin2.install(this._vue, this);
    }
    this._plugins.push(plugin2);
    if (this._setupDone) {
      this._router.addRoutes(plugin2.routes);
    } else {
      plugin2.routes.forEach((x) => this._routes.push(x));
    }
    plugin2.editors.forEach((x) => this.addOrReplace(this._editors, x, "alias"));
    plugin2.lists.forEach((x) => this.addOrReplace(this._lists, x, "alias"));
    console.log(`[zero] Installed %c${plugin2.name}%cplugin`, "font-style:italic;");
  }
  getEditor(alias2) {
    const renderer = this._editors.find((x) => x.alias === alias2);
    if (!renderer) {
      console.warn(`[zero] Could not find editor renderer ${alias2}`);
    }
    return renderer;
  }
  getList(alias2) {
    const renderer = this._lists.find((x) => x.alias === alias2);
    if (!renderer) {
      console.warn(`[zero] Could not find list renderer ${alias2}`);
    }
    return renderer;
  }
  addOrReplace(array, item2, byKey) {
    const existingRenderer = array.find((x) => x[byKey] === item2[byKey]);
    if (existingRenderer) {
      const index = array.indexOf(existingRenderer);
      array.splice(index, 1, item2);
    } else {
      array.push(item2);
    }
  }
}
Zero.install = (vue, opts) => {
  const zero2 = new Zero(vue, opts);
  Zero.instance = zero2;
  Object.defineProperty(vue.prototype, "zero", {
    get: () => zero2
  });
  zero2.setup();
  console.log("[zero] Setup completed");
  plugins.forEach((plugin2) => {
    zero2.use(plugin2);
  });
};
var _typography = '@font-face {\n  font-family: "Feather";\n  font-weight: 400;\n  font-style: normal;\n  src: url("__VITE_ASSET__4e554e4b__") format("woff2"), url("__VITE_ASSET__ef3c47cb__") format("woff");\n}\n@font-face {\n  font-family: "Lato";\n  font-weight: 100;\n  font-style: normal;\n  src: url("__VITE_ASSET__e7f20acf__") format("woff2"), url("__VITE_ASSET__7ff2abf1__") format("woff");\n}\n@font-face {\n  font-family: "Lato";\n  font-weight: 400;\n  font-style: normal;\n  src: url("__VITE_ASSET__ddd4ef7f__") format("woff2"), url("__VITE_ASSET__9c46f792__") format("woff");\n}\n@font-face {\n  font-family: "Lato";\n  font-weight: 700;\n  font-style: normal;\n  src: url("__VITE_ASSET__27640163__") format("woff2"), url("__VITE_ASSET__7cebe978__") format("woff");\n}\n@font-face {\n  font-family: "Lato";\n  font-weight: 900;\n  font-style: normal;\n  src: url("__VITE_ASSET__e2c997ab__") format("woff2"), url("__VITE_ASSET__ee422b6f__") format("woff");\n}\n@font-face {\n  font-family: "Inter";\n  font-weight: 100 900;\n  font-display: swap;\n  font-style: normal;\n  font-named-instance: "Regular";\n  src: url("__VITE_ASSET__e75452ed__$_?v=3.17__") format("woff2");\n}\n@font-face {\n  font-family: "Inter";\n  font-weight: 100 900;\n  font-display: swap;\n  font-style: italic;\n  font-named-instance: "Italic";\n  src: url("__VITE_ASSET__f2928fe1__$_?v=3.17__") format("woff2");\n}\n:root {\n  --font: "Lato", "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;\n  --font-icon: "Feather";\n}';
var _core = `*, *:before, *:after {
  box-sizing: border-box;
  -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
  -webkit-tap-highlight-color: transparent;
}

p {
  margin: 0;
}

p + p {
  margin-top: 0.5em;
}

img {
  display: inline-block;
  vertical-align: middle;
}

img, object, embed {
  max-width: 100%;
  height: auto;
}

object, embed {
  height: 100%;
}

a, a:visited, a:link {
  /*color: var(--color-text);*/
  text-decoration: none;
}

button {
  -webkit-appearance: none;
  border: none;
  background: transparent;
  cursor: pointer;
  padding: 0;
  margin: 0;
  text-align: left;
}

h1 {
  font-size: calc(1.4rem + 0.1vmin);
  font-weight: 400;
  margin: -0.3rem 0 3rem;
}

h2 {
  font-size: calc(1.4rem + 0.1vmin);
  font-weight: 400;
  margin: 0 0 2rem;
}

ul {
  margin: 0;
  padding: 0;
}

ul li {
  list-style: none inside;
  margin-bottom: 0;
  padding-left: 16px;
  position: relative;
}

ul li:last-child {
  margin-bottom: 0;
}

ul li:before {
  content: " ";
  display: inline-block;
  width: 4px;
  height: 4px;
  border-radius: 12px;
  background: var(--color-text);
  position: absolute;
  left: 1px;
  top: 9px;
}

ol {
  margin: 0;
  padding: 0;
}

ol li {
  list-style-position: outside;
  margin-left: 1em;
  margin-bottom: 20px;
  position: relative;
}

ol li:last-child {
  margin-bottom: 0;
}

li {
  line-height: 1.8;
}

li > * {
  margin: 0 !important;
  padding: 0 !important;
  display: inline;
}

a[href]:focus, button:focus {
  outline: var(--color-button-focus-outline);
  outline-offset: 2px;
  outline: none;
}

/*a[href], button
{
  position: relative;
  overflow: visible !important;

  &:focus
  {
    outline: none;
  }

  &:focus:after
  {
    content: '';
    position: absolute;
    z-index: 10;
    left: -4px;
    top: -4px;
    right: -4px;
    bottom: -4px;
    border-radius: 7px;
    border: var(--color-button-focus-outline);
  }
}*/
table {
  border-collapse: collapse;
}

hr {
  margin: 20px 0;
  border: none;
  border-bottom: 1px solid var(--color-line);
}

sup {
  margin-left: 5px;
  font-size: 0.7em;
}

iframe, video {
  margin-top: 20px;
}

code {
  border-radius: 3px;
  padding: 1px 4px;
  border: 1px dashed var(--color-line-dashed);
  background: var(--color-bg-shade-2);
  color: var(--color-text);
}

.sr-only {
  height: 1px;
  width: 1px;
  clip: rect(1px, 1px, 1px, 1px);
  overflow: hidden;
  position: absolute;
  opacity: 0.01;
}

* {
  scrollbar-width: thin;
}

::-webkit-scrollbar {
  width: 7px;
  height: 7px;
  background-color: transparent;
}

/*::-webkit-scrollbar-thumb
{
  background: var(--color-bg-mid);
}*/
.ui-resizing {
  user-select: none;
}

.media-pattern {
  position: relative;
  z-index: 0;
  overflow: hidden;
  background: transparent;
  border-radius: var(--radius);
}
.media-pattern:before {
  content: "";
  position: absolute;
  width: 100%;
  height: 100%;
  left: 0;
  top: 0;
  background-position: 0px 0px, 5px 5px;
  background-size: 10px 10px;
  background-image: linear-gradient(45deg, #aaa 25%, transparent 25%, transparent 75%, #aaa 75%, #aaa 100%), linear-gradient(45deg, #aaa 25%, #ddd 25%, #ddd 75%, #aaa 75%, #aaa 100%);
  border-radius: var(--radius);
}

.ui-editor-overlay > content,
.ui-overlay-editor > content {
  position: relative;
  padding-top: 0 !important;
}
.ui-editor-overlay .ui-box,
.ui-overlay-editor .ui-box {
  margin: 0;
}
.ui-editor-overlay .ui-box + .ui-box:not(.ui-tab),
.ui-overlay-editor .ui-box + .ui-box:not(.ui-tab) {
  margin-top: var(--padding-s);
}
.ui-editor-overlay .ui-tabs-list, .ui-editor-overlay .editor,
.ui-overlay-editor .ui-tabs-list,
.ui-overlay-editor .editor {
  padding: 0;
}
.ui-editor-overlay .ui-property.ui-modules,
.ui-overlay-editor .ui-property.ui-modules {
  margin: 0;
  padding: 0;
}
.ui-editor-overlay .ui-loading,
.ui-overlay-editor .ui-loading {
  position: absolute;
  left: 50%;
  top: 50%;
  margin: -14px 0 0 -14px;
}

.show-dark, .hide-light,
.theme-dark .show-light, .theme-dark .hide-dark {
  display: none !important;
}

.theme-dark .show-dark, .theme-dark .hide-light {
  display: initial !important;
}

.feather {
  width: 24px;
  height: 24px;
  stroke: currentColor;
  stroke-width: 2;
  stroke-linecap: round;
  stroke-linejoin: round;
  fill: none;
}

.ui-maxlines {
  line-height: 1.5;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: normal;
}

.ui-maxlines.is-expanded {
  -webkit-line-clamp: inherit !important;
}`;
var _flags = ".ui-icon[data-symbol^=flag-] {\n  width: 22px;\n  height: 15px;\n  display: inline-block;\n  background: url(__VITE_ASSET__c74f6f56__) no-repeat;\n  background-size: 100%;\n}\n\n.ui-icon[data-symbol=flag-ad] {\n  background-position: 0 0.413223%;\n}\n\n.ui-icon[data-symbol=flag-ae] {\n  background-position: 0 0.826446%;\n}\n\n.ui-icon[data-symbol=flag-af] {\n  background-position: 0 1.239669%;\n}\n\n.ui-icon[data-symbol=flag-ag] {\n  background-position: 0 1.652893%;\n}\n\n.ui-icon[data-symbol=flag-ai] {\n  background-position: 0 2.066116%;\n}\n\n.ui-icon[data-symbol=flag-al] {\n  background-position: 0 2.479339%;\n}\n\n.ui-icon[data-symbol=flag-am] {\n  background-position: 0 2.892562%;\n}\n\n.ui-icon[data-symbol=flag-an] {\n  background-position: 0 3.305785%;\n}\n\n.ui-icon[data-symbol=flag-ao] {\n  background-position: 0 3.719008%;\n}\n\n.ui-icon[data-symbol=flag-aq] {\n  background-position: 0 4.132231%;\n}\n\n.ui-icon[data-symbol=flag-ar] {\n  background-position: 0 4.545455%;\n}\n\n.ui-icon[data-symbol=flag-as] {\n  background-position: 0 4.958678%;\n}\n\n.ui-icon[data-symbol=flag-at] {\n  background-position: 0 5.371901%;\n}\n\n.ui-icon[data-symbol=flag-au] {\n  background-position: 0 5.785124%;\n}\n\n.ui-icon[data-symbol=flag-aw] {\n  background-position: 0 6.198347%;\n}\n\n.ui-icon[data-symbol=flag-az] {\n  background-position: 0 6.61157%;\n}\n\n.ui-icon[data-symbol=flag-ba] {\n  background-position: 0 7.024793%;\n}\n\n.ui-icon[data-symbol=flag-bb] {\n  background-position: 0 7.438017%;\n}\n\n.ui-icon[data-symbol=flag-bd] {\n  background-position: 0 7.85124%;\n}\n\n.ui-icon[data-symbol=flag-be] {\n  background-position: 0 8.264463%;\n}\n\n.ui-icon[data-symbol=flag-bf] {\n  background-position: 0 8.677686%;\n}\n\n.ui-icon[data-symbol=flag-bg] {\n  background-position: 0 9.090909%;\n}\n\n.ui-icon[data-symbol=flag-bh] {\n  background-position: 0 9.504132%;\n}\n\n.ui-icon[data-symbol=flag-bi] {\n  background-position: 0 9.917355%;\n}\n\n.ui-icon[data-symbol=flag-bj] {\n  background-position: 0 10.330579%;\n}\n\n.ui-icon[data-symbol=flag-bm] {\n  background-position: 0 10.743802%;\n}\n\n.ui-icon[data-symbol=flag-bn] {\n  background-position: 0 11.157025%;\n}\n\n.ui-icon[data-symbol=flag-bo] {\n  background-position: 0 11.570248%;\n}\n\n.ui-icon[data-symbol=flag-br] {\n  background-position: 0 11.983471%;\n}\n\n.ui-icon[data-symbol=flag-bs] {\n  background-position: 0 12.396694%;\n}\n\n.ui-icon[data-symbol=flag-bt] {\n  background-position: 0 12.809917%;\n}\n\n.ui-icon[data-symbol=flag-bv] {\n  background-position: 0 13.22314%;\n}\n\n.ui-icon[data-symbol=flag-bw] {\n  background-position: 0 13.636364%;\n}\n\n.ui-icon[data-symbol=flag-by] {\n  background-position: 0 14.049587%;\n}\n\n.ui-icon[data-symbol=flag-bz] {\n  background-position: 0 14.46281%;\n}\n\n.ui-icon[data-symbol=flag-ca] {\n  background-position: 0 14.876033%;\n}\n\n.ui-icon[data-symbol=flag-cc] {\n  background-position: 0 15.289256%;\n}\n\n.ui-icon[data-symbol=flag-cd] {\n  background-position: 0 15.702479%;\n}\n\n.ui-icon[data-symbol=flag-cf] {\n  background-position: 0 16.115702%;\n}\n\n.ui-icon[data-symbol=flag-cg] {\n  background-position: 0 16.528926%;\n}\n\n.ui-icon[data-symbol=flag-ch] {\n  background-position: 0 16.942149%;\n}\n\n.ui-icon[data-symbol=flag-ci] {\n  background-position: 0 17.355372%;\n}\n\n.ui-icon[data-symbol=flag-ck] {\n  background-position: 0 17.768595%;\n}\n\n.ui-icon[data-symbol=flag-cl] {\n  background-position: 0 18.181818%;\n}\n\n.ui-icon[data-symbol=flag-cm] {\n  background-position: 0 18.595041%;\n}\n\n.ui-icon[data-symbol=flag-cn] {\n  background-position: 0 19.008264%;\n}\n\n.ui-icon[data-symbol=flag-co] {\n  background-position: 0 19.421488%;\n}\n\n.ui-icon[data-symbol=flag-cr] {\n  background-position: 0 19.834711%;\n}\n\n.ui-icon[data-symbol=flag-cu] {\n  background-position: 0 20.247934%;\n}\n\n.ui-icon[data-symbol=flag-cv] {\n  background-position: 0 20.661157%;\n}\n\n.ui-icon[data-symbol=flag-cx] {\n  background-position: 0 21.07438%;\n}\n\n.ui-icon[data-symbol=flag-cy] {\n  background-position: 0 21.487603%;\n}\n\n.ui-icon[data-symbol=flag-cz] {\n  background-position: 0 21.900826%;\n}\n\n.ui-icon[data-symbol=flag-de] {\n  background-position: 0 22.31405%;\n}\n\n.ui-icon[data-symbol=flag-dj] {\n  background-position: 0 22.727273%;\n}\n\n.ui-icon[data-symbol=flag-dk] {\n  background-position: 0 23.140496%;\n}\n\n.ui-icon[data-symbol=flag-dm] {\n  background-position: 0 23.553719%;\n}\n\n.ui-icon[data-symbol=flag-do] {\n  background-position: 0 23.966942%;\n}\n\n.ui-icon[data-symbol=flag-dz] {\n  background-position: 0 24.380165%;\n}\n\n.ui-icon[data-symbol=flag-ec] {\n  background-position: 0 24.793388%;\n}\n\n.ui-icon[data-symbol=flag-ee] {\n  background-position: 0 25.206612%;\n}\n\n.ui-icon[data-symbol=flag-eg] {\n  background-position: 0 25.619835%;\n}\n\n.ui-icon[data-symbol=flag-eh] {\n  background-position: 0 26.033058%;\n}\n\n.ui-icon[data-symbol=flag-er] {\n  background-position: 0 26.446281%;\n}\n\n.ui-icon[data-symbol=flag-es] {\n  background-position: 0 26.859504%;\n}\n\n.ui-icon[data-symbol=flag-et] {\n  background-position: 0 27.272727%;\n}\n\n.ui-icon[data-symbol=flag-fi] {\n  background-position: 0 27.68595%;\n}\n\n.ui-icon[data-symbol=flag-fj] {\n  background-position: 0 28.099174%;\n}\n\n.ui-icon[data-symbol=flag-fk] {\n  background-position: 0 28.512397%;\n}\n\n.ui-icon[data-symbol=flag-fm] {\n  background-position: 0 28.92562%;\n}\n\n.ui-icon[data-symbol=flag-fo] {\n  background-position: 0 29.338843%;\n}\n\n.ui-icon[data-symbol=flag-fr] {\n  background-position: 0 29.752066%;\n}\n\n.ui-icon[data-symbol=flag-ga] {\n  background-position: 0 30.165289%;\n}\n\n.ui-icon[data-symbol=flag-gd] {\n  background-position: 0 30.578512%;\n}\n\n.ui-icon[data-symbol=flag-ge] {\n  background-position: 0 30.991736%;\n}\n\n.ui-icon[data-symbol=flag-gf] {\n  background-position: 0 31.404959%;\n}\n\n.ui-icon[data-symbol=flag-gh] {\n  background-position: 0 31.818182%;\n}\n\n.ui-icon[data-symbol=flag-gi] {\n  background-position: 0 32.231405%;\n}\n\n.ui-icon[data-symbol=flag-gl] {\n  background-position: 0 32.644628%;\n}\n\n.ui-icon[data-symbol=flag-gm] {\n  background-position: 0 33.057851%;\n}\n\n.ui-icon[data-symbol=flag-gn] {\n  background-position: 0 33.471074%;\n}\n\n.ui-icon[data-symbol=flag-gp] {\n  background-position: 0 33.884298%;\n}\n\n.ui-icon[data-symbol=flag-gq] {\n  background-position: 0 34.297521%;\n}\n\n.ui-icon[data-symbol=flag-gr] {\n  background-position: 0 34.710744%;\n}\n\n.ui-icon[data-symbol=flag-gs] {\n  background-position: 0 35.123967%;\n}\n\n.ui-icon[data-symbol=flag-gt] {\n  background-position: 0 35.53719%;\n}\n\n.ui-icon[data-symbol=flag-gu] {\n  background-position: 0 35.950413%;\n}\n\n.ui-icon[data-symbol=flag-gw] {\n  background-position: 0 36.363636%;\n}\n\n.ui-icon[data-symbol=flag-gy] {\n  background-position: 0 36.77686%;\n}\n\n.ui-icon[data-symbol=flag-hk] {\n  background-position: 0 37.190083%;\n}\n\n.ui-icon[data-symbol=flag-hm] {\n  background-position: 0 37.603306%;\n}\n\n.ui-icon[data-symbol=flag-hn] {\n  background-position: 0 38.016529%;\n}\n\n.ui-icon[data-symbol=flag-hr] {\n  background-position: 0 38.429752%;\n}\n\n.ui-icon[data-symbol=flag-ht] {\n  background-position: 0 38.842975%;\n}\n\n.ui-icon[data-symbol=flag-hu] {\n  background-position: 0 39.256198%;\n}\n\n.ui-icon[data-symbol=flag-id] {\n  background-position: 0 39.669421%;\n}\n\n.ui-icon[data-symbol=flag-ie] {\n  background-position: 0 40.082645%;\n}\n\n.ui-icon[data-symbol=flag-il] {\n  background-position: 0 40.495868%;\n}\n\n.ui-icon[data-symbol=flag-in] {\n  background-position: 0 40.909091%;\n}\n\n.ui-icon[data-symbol=flag-io] {\n  background-position: 0 41.322314%;\n}\n\n.ui-icon[data-symbol=flag-iq] {\n  background-position: 0 41.735537%;\n}\n\n.ui-icon[data-symbol=flag-ir] {\n  background-position: 0 42.14876%;\n}\n\n.ui-icon[data-symbol=flag-is] {\n  background-position: 0 42.561983%;\n}\n\n.ui-icon[data-symbol=flag-it] {\n  background-position: 0 42.975207%;\n}\n\n.ui-icon[data-symbol=flag-jm] {\n  background-position: 0 43.38843%;\n}\n\n.ui-icon[data-symbol=flag-jo] {\n  background-position: 0 43.801653%;\n}\n\n.ui-icon[data-symbol=flag-jp] {\n  background-position: 0 44.214876%;\n}\n\n.ui-icon[data-symbol=flag-ke] {\n  background-position: 0 44.628099%;\n}\n\n.ui-icon[data-symbol=flag-kg] {\n  background-position: 0 45.041322%;\n}\n\n.ui-icon[data-symbol=flag-kh] {\n  background-position: 0 45.454545%;\n}\n\n.ui-icon[data-symbol=flag-ki] {\n  background-position: 0 45.867769%;\n}\n\n.ui-icon[data-symbol=flag-km] {\n  background-position: 0 46.280992%;\n}\n\n.ui-icon[data-symbol=flag-kn] {\n  background-position: 0 46.694215%;\n}\n\n.ui-icon[data-symbol=flag-kp] {\n  background-position: 0 47.107438%;\n}\n\n.ui-icon[data-symbol=flag-kr] {\n  background-position: 0 47.520661%;\n}\n\n.ui-icon[data-symbol=flag-kw] {\n  background-position: 0 47.933884%;\n}\n\n.ui-icon[data-symbol=flag-ky] {\n  background-position: 0 48.347107%;\n}\n\n.ui-icon[data-symbol=flag-kz] {\n  background-position: 0 48.760331%;\n}\n\n.ui-icon[data-symbol=flag-la] {\n  background-position: 0 49.173554%;\n}\n\n.ui-icon[data-symbol=flag-lb] {\n  background-position: 0 49.586777%;\n}\n\n.ui-icon[data-symbol=flag-lc] {\n  background-position: 0 50%;\n}\n\n.ui-icon[data-symbol=flag-li] {\n  background-position: 0 50.413223%;\n}\n\n.ui-icon[data-symbol=flag-lk] {\n  background-position: 0 50.826446%;\n}\n\n.ui-icon[data-symbol=flag-lr] {\n  background-position: 0 51.239669%;\n}\n\n.ui-icon[data-symbol=flag-ls] {\n  background-position: 0 51.652893%;\n}\n\n.ui-icon[data-symbol=flag-lt] {\n  background-position: 0 52.066116%;\n}\n\n.ui-icon[data-symbol=flag-lu] {\n  background-position: 0 52.479339%;\n}\n\n.ui-icon[data-symbol=flag-lv] {\n  background-position: 0 52.892562%;\n}\n\n.ui-icon[data-symbol=flag-ly] {\n  background-position: 0 53.305785%;\n}\n\n.ui-icon[data-symbol=flag-ma] {\n  background-position: 0 53.719008%;\n}\n\n.ui-icon[data-symbol=flag-mc] {\n  background-position: 0 54.132231%;\n}\n\n.ui-icon[data-symbol=flag-md] {\n  background-position: 0 54.545455%;\n}\n\n.ui-icon[data-symbol=flag-me] {\n  background-position: 0 54.958678%;\n}\n\n.ui-icon[data-symbol=flag-mg] {\n  background-position: 0 55.371901%;\n}\n\n.ui-icon[data-symbol=flag-mh] {\n  background-position: 0 55.785124%;\n}\n\n.ui-icon[data-symbol=flag-mk] {\n  background-position: 0 56.198347%;\n}\n\n.ui-icon[data-symbol=flag-ml] {\n  background-position: 0 56.61157%;\n}\n\n.ui-icon[data-symbol=flag-mm] {\n  background-position: 0 57.024793%;\n}\n\n.ui-icon[data-symbol=flag-mn] {\n  background-position: 0 57.438017%;\n}\n\n.ui-icon[data-symbol=flag-mo] {\n  background-position: 0 57.85124%;\n}\n\n.ui-icon[data-symbol=flag-mp] {\n  background-position: 0 58.264463%;\n}\n\n.ui-icon[data-symbol=flag-mq] {\n  background-position: 0 58.677686%;\n}\n\n.ui-icon[data-symbol=flag-mr] {\n  background-position: 0 59.090909%;\n}\n\n.ui-icon[data-symbol=flag-ms] {\n  background-position: 0 59.504132%;\n}\n\n.ui-icon[data-symbol=flag-mt] {\n  background-position: 0 59.917355%;\n}\n\n.ui-icon[data-symbol=flag-mu] {\n  background-position: 0 60.330579%;\n}\n\n.ui-icon[data-symbol=flag-mv] {\n  background-position: 0 60.743802%;\n}\n\n.ui-icon[data-symbol=flag-mw] {\n  background-position: 0 61.157025%;\n}\n\n.ui-icon[data-symbol=flag-mx] {\n  background-position: 0 61.570248%;\n}\n\n.ui-icon[data-symbol=flag-my] {\n  background-position: 0 61.983471%;\n}\n\n.ui-icon[data-symbol=flag-mz] {\n  background-position: 0 62.396694%;\n}\n\n.ui-icon[data-symbol=flag-na] {\n  background-position: 0 62.809917%;\n}\n\n.ui-icon[data-symbol=flag-nc] {\n  background-position: 0 63.22314%;\n}\n\n.ui-icon[data-symbol=flag-ne] {\n  background-position: 0 63.636364%;\n}\n\n.ui-icon[data-symbol=flag-nf] {\n  background-position: 0 64.049587%;\n}\n\n.ui-icon[data-symbol=flag-ng] {\n  background-position: 0 64.46281%;\n}\n\n.ui-icon[data-symbol=flag-ni] {\n  background-position: 0 64.876033%;\n}\n\n.ui-icon[data-symbol=flag-nl] {\n  background-position: 0 65.289256%;\n}\n\n.ui-icon[data-symbol=flag-no] {\n  background-position: 0 65.702479%;\n}\n\n.ui-icon[data-symbol=flag-np] {\n  background-position: 0 66.115702%;\n}\n\n.ui-icon[data-symbol=flag-nr] {\n  background-position: 0 66.528926%;\n}\n\n.ui-icon[data-symbol=flag-nu] {\n  background-position: 0 66.942149%;\n}\n\n.ui-icon[data-symbol=flag-nz] {\n  background-position: 0 67.355372%;\n}\n\n.ui-icon[data-symbol=flag-om] {\n  background-position: 0 67.768595%;\n}\n\n.ui-icon[data-symbol=flag-pa] {\n  background-position: 0 68.181818%;\n}\n\n.ui-icon[data-symbol=flag-pe] {\n  background-position: 0 68.595041%;\n}\n\n.ui-icon[data-symbol=flag-pf] {\n  background-position: 0 69.008264%;\n}\n\n.ui-icon[data-symbol=flag-pg] {\n  background-position: 0 69.421488%;\n}\n\n.ui-icon[data-symbol=flag-ph] {\n  background-position: 0 69.834711%;\n}\n\n.ui-icon[data-symbol=flag-pk] {\n  background-position: 0 70.247934%;\n}\n\n.ui-icon[data-symbol=flag-pl] {\n  background-position: 0 70.661157%;\n}\n\n.ui-icon[data-symbol=flag-pm] {\n  background-position: 0 71.07438%;\n}\n\n.ui-icon[data-symbol=flag-pn] {\n  background-position: 0 71.487603%;\n}\n\n.ui-icon[data-symbol=flag-pr] {\n  background-position: 0 71.900826%;\n}\n\n.ui-icon[data-symbol=flag-pt] {\n  background-position: 0 72.31405%;\n}\n\n.ui-icon[data-symbol=flag-pw] {\n  background-position: 0 72.727273%;\n}\n\n.ui-icon[data-symbol=flag-py] {\n  background-position: 0 73.140496%;\n}\n\n.ui-icon[data-symbol=flag-qa] {\n  background-position: 0 73.553719%;\n}\n\n.ui-icon[data-symbol=flag-re] {\n  background-position: 0 73.966942%;\n}\n\n.ui-icon[data-symbol=flag-ro] {\n  background-position: 0 74.380165%;\n}\n\n.ui-icon[data-symbol=flag-rs] {\n  background-position: 0 74.793388%;\n}\n\n.ui-icon[data-symbol=flag-ru] {\n  background-position: 0 75.206612%;\n}\n\n.ui-icon[data-symbol=flag-rw] {\n  background-position: 0 75.619835%;\n}\n\n.ui-icon[data-symbol=flag-sa] {\n  background-position: 0 76.033058%;\n}\n\n.ui-icon[data-symbol=flag-sb] {\n  background-position: 0 76.446281%;\n}\n\n.ui-icon[data-symbol=flag-sc] {\n  background-position: 0 76.859504%;\n}\n\n.ui-icon[data-symbol=flag-sd] {\n  background-position: 0 77.272727%;\n}\n\n.ui-icon[data-symbol=flag-se] {\n  background-position: 0 77.68595%;\n}\n\n.ui-icon[data-symbol=flag-sg] {\n  background-position: 0 78.099174%;\n}\n\n.ui-icon[data-symbol=flag-sh] {\n  background-position: 0 78.512397%;\n}\n\n.ui-icon[data-symbol=flag-si] {\n  background-position: 0 78.92562%;\n}\n\n.ui-icon[data-symbol=flag-sj] {\n  background-position: 0 79.338843%;\n}\n\n.ui-icon[data-symbol=flag-sk] {\n  background-position: 0 79.752066%;\n}\n\n.ui-icon[data-symbol=flag-sl] {\n  background-position: 0 80.165289%;\n}\n\n.ui-icon[data-symbol=flag-sm] {\n  background-position: 0 80.578512%;\n}\n\n.ui-icon[data-symbol=flag-sn] {\n  background-position: 0 80.991736%;\n}\n\n.ui-icon[data-symbol=flag-so] {\n  background-position: 0 81.404959%;\n}\n\n.ui-icon[data-symbol=flag-sr] {\n  background-position: 0 81.818182%;\n}\n\n.ui-icon[data-symbol=flag-ss] {\n  background-position: 0 82.231405%;\n}\n\n.ui-icon[data-symbol=flag-st] {\n  background-position: 0 82.644628%;\n}\n\n.ui-icon[data-symbol=flag-sv] {\n  background-position: 0 83.057851%;\n}\n\n.ui-icon[data-symbol=flag-sy] {\n  background-position: 0 83.471074%;\n}\n\n.ui-icon[data-symbol=flag-sz] {\n  background-position: 0 83.884298%;\n}\n\n.ui-icon[data-symbol=flag-tc] {\n  background-position: 0 84.297521%;\n}\n\n.ui-icon[data-symbol=flag-td] {\n  background-position: 0 84.710744%;\n}\n\n.ui-icon[data-symbol=flag-tf] {\n  background-position: 0 85.123967%;\n}\n\n.ui-icon[data-symbol=flag-tg] {\n  background-position: 0 85.53719%;\n}\n\n.ui-icon[data-symbol=flag-th] {\n  background-position: 0 85.950413%;\n}\n\n.ui-icon[data-symbol=flag-tj] {\n  background-position: 0 86.363636%;\n}\n\n.ui-icon[data-symbol=flag-tk] {\n  background-position: 0 86.77686%;\n}\n\n.ui-icon[data-symbol=flag-tl] {\n  background-position: 0 87.190083%;\n}\n\n.ui-icon[data-symbol=flag-tm] {\n  background-position: 0 87.603306%;\n}\n\n.ui-icon[data-symbol=flag-tn] {\n  background-position: 0 88.016529%;\n}\n\n.ui-icon[data-symbol=flag-to] {\n  background-position: 0 88.429752%;\n}\n\n.ui-icon[data-symbol=flag-tp] {\n  background-position: 0 88.842975%;\n}\n\n.ui-icon[data-symbol=flag-tr] {\n  background-position: 0 89.256198%;\n}\n\n.ui-icon[data-symbol=flag-tt] {\n  background-position: 0 89.669421%;\n}\n\n.ui-icon[data-symbol=flag-tv] {\n  background-position: 0 90.082645%;\n}\n\n.ui-icon[data-symbol=flag-tw] {\n  background-position: 0 90.495868%;\n}\n\n.ui-icon[data-symbol=flag-ty] {\n  background-position: 0 90.909091%;\n}\n\n.ui-icon[data-symbol=flag-tz] {\n  background-position: 0 91.322314%;\n}\n\n.ui-icon[data-symbol=flag-ua] {\n  background-position: 0 91.735537%;\n}\n\n.ui-icon[data-symbol=flag-ug] {\n  background-position: 0 92.14876%;\n}\n\n.ui-icon[data-symbol=flag-gb], .ui-icon[data-symbol=flag-uk] {\n  background-position: 0 92.561983%;\n}\n\n.ui-icon[data-symbol=flag-um] {\n  background-position: 0 92.975207%;\n}\n\n.ui-icon[data-symbol=flag-us] {\n  background-position: 0 93.38843%;\n}\n\n.ui-icon[data-symbol=flag-uy] {\n  background-position: 0 93.801653%;\n}\n\n.ui-icon[data-symbol=flag-uz] {\n  background-position: 0 94.214876%;\n}\n\n.ui-icon[data-symbol=flag-va] {\n  background-position: 0 94.628099%;\n}\n\n.ui-icon[data-symbol=flag-vc] {\n  background-position: 0 95.041322%;\n}\n\n.ui-icon[data-symbol=flag-ve] {\n  background-position: 0 95.454545%;\n}\n\n.ui-icon[data-symbol=flag-vg] {\n  background-position: 0 95.867769%;\n}\n\n.ui-icon[data-symbol=flag-vi] {\n  background-position: 0 96.280992%;\n}\n\n.ui-icon[data-symbol=flag-vn] {\n  background-position: 0 96.694215%;\n}\n\n.ui-icon[data-symbol=flag-vu] {\n  background-position: 0 97.107438%;\n}\n\n.ui-icon[data-symbol=flag-wf] {\n  background-position: 0 97.520661%;\n}\n\n.ui-icon[data-symbol=flag-ws] {\n  background-position: 0 97.933884%;\n}\n\n.ui-icon[data-symbol=flag-ye] {\n  background-position: 0 98.347107%;\n}\n\n.ui-icon[data-symbol=flag-za] {\n  background-position: 0 98.760331%;\n}\n\n.ui-icon[data-symbol=flag-zm] {\n  background-position: 0 99.173554%;\n}\n\n.ui-icon[data-symbol=flag-zr] {\n  background-position: 0 99.586777%;\n}\n\n.ui-icon[data-symbol=flag-zw] {\n  background-position: 0 100%;\n}";
var _forms = '@charset "UTF-8";\n::placeholder {\n  color: var(--color-input-placeholder);\n}\n\ninput[type=text], input[type=color], input[type=date], input[type=datetime], input[type=email], input[type=file], input[type=month], input[type=number],\ninput[type=password], input[type=range], input[type=search], input[type=tel], input[type=time], input[type=url], input[type=week],\ntextarea, .ui-native-select, .ui-rte-input, .flatpickr-current-month {\n  font-family: var(--font);\n  background: var(--color-input);\n  border: var(--color-input-border);\n  font-size: var(--font-size);\n  display: inline-block;\n  height: 48px;\n  padding: 6px 16px;\n  line-height: 1.5;\n  color: var(--color-text);\n  border-radius: var(--radius-inner);\n  vertical-align: middle;\n  box-sizing: border-box;\n  width: 100%;\n}\ninput[type=text]:not(:disabled):not([readonly]):focus, input[type=color]:not(:disabled):not([readonly]):focus, input[type=date]:not(:disabled):not([readonly]):focus, input[type=datetime]:not(:disabled):not([readonly]):focus, input[type=email]:not(:disabled):not([readonly]):focus, input[type=file]:not(:disabled):not([readonly]):focus, input[type=month]:not(:disabled):not([readonly]):focus, input[type=number]:not(:disabled):not([readonly]):focus,\ninput[type=password]:not(:disabled):not([readonly]):focus, input[type=range]:not(:disabled):not([readonly]):focus, input[type=search]:not(:disabled):not([readonly]):focus, input[type=tel]:not(:disabled):not([readonly]):focus, input[type=time]:not(:disabled):not([readonly]):focus, input[type=url]:not(:disabled):not([readonly]):focus, input[type=week]:not(:disabled):not([readonly]):focus,\ntextarea:not(:disabled):not([readonly]):focus, .ui-native-select:not(:disabled):not([readonly]):focus, .ui-rte-input:not(:disabled):not([readonly]):focus, .flatpickr-current-month:not(:disabled):not([readonly]):focus {\n  background-color: var(--color-input-focus-bg);\n  border: var(--color-input-focus-border);\n  box-shadow: var(--color-input-focus-shadow);\n  outline: none;\n}\n\nselect:focus {\n  /*background-color: #fff;\n  border: 1px solid var(--color-line-onbg);\n  box-shadow: 0 0 0 4px var(--color-input);*/\n  outline: none;\n}\n\ntextarea {\n  resize: vertical;\n  min-height: 80px;\n}\n\n.ui-native-select, .flatpickr-current-month {\n  position: relative;\n  font-size: var(--font-size);\n}\n.ui-native-select select, .flatpickr-current-month select {\n  font-family: var(--font);\n  position: absolute;\n  left: 0;\n  right: 0;\n  top: 0;\n  bottom: 0;\n  width: 100%;\n  padding-left: 9px;\n  -webkit-appearance: none;\n  border: none;\n  font-size: var(--font-size);\n  background: none;\n  border: 1px solid transparent;\n  color: var(--color-text);\n  border-radius: var(--radius-inner);\n}\n.ui-native-select select:focus, .flatpickr-current-month select:focus {\n  background-color: var(--color-input-focus-bg);\n  border: var(--color-input-focus-border);\n  box-shadow: var(--color-input-focus-shadow);\n  outline: none;\n}\n.ui-native-select:after, .flatpickr-current-month:after {\n  font-family: var(--font-icon);\n  content: "\uE842";\n  position: absolute;\n  right: 12px;\n  top: 0;\n  height: 48px;\n  line-height: 46px;\n}\n.ui-native-select[disabled], .ui-native-select[readonly], .flatpickr-current-month[disabled], .flatpickr-current-month[readonly] {\n  min-height: 23px;\n  /*select\n  {\n    text-indent: -2px;\n    padding-left: 0;\n  }*/\n}\n.ui-native-select[disabled]:after, .ui-native-select[readonly]:after, .flatpickr-current-month[disabled]:after, .flatpickr-current-month[readonly]:after {\n  display: none;\n}\n\n.ui-native-check {\n  position: relative;\n  top: 1px;\n  font-size: var(--font-size);\n  cursor: pointer;\n  display: flex;\n  align-items: center;\n  line-height: 1;\n}\n.ui-native-check input {\n  width: 0;\n  height: 0;\n  opacity: 0;\n  position: absolute;\n  left: 0;\n  overflow: hidden;\n  visibility: hidden;\n  -webkit-appearance: none;\n  border: none;\n}\n.is-disabled .ui-native-check {\n  cursor: default;\n}\n\n.ui-native-check-toggle {\n  display: inline-block;\n  width: 20px;\n  height: 20px;\n  line-height: 20px;\n  text-align: center;\n  border-radius: 3px;\n  border: 1px solid transparent;\n  background: var(--color-check);\n  margin-right: 10px;\n  position: relative;\n  top: -1px;\n}\n.onbg :not(input:checked) ~ .ui-native-check-toggle {\n  background: var(--color-bg-shade-1);\n  box-shadow: var(--shadow-short);\n}\n.ui-native-check-toggle:before {\n  content: "\uE83F";\n  font-family: var(--font-icon);\n  color: transparent;\n  font-size: var(--font-size-s);\n}\ninput:focus ~ .ui-native-check-toggle {\n  background: var(--color-checked) !important;\n  border: var(--color-input-focus-border);\n  box-shadow: var(--color-input-focus-shadow);\n  outline: none;\n}\ninput:checked ~ .ui-native-check-toggle {\n  background: var(--color-checked);\n}\ninput:checked ~ .ui-native-check-toggle:before {\n  color: var(--color-checked-fg);\n}';
var _icons = '@charset "UTF-8";\n[class^=fth-], [class*=" fth-"] {\n  font-family: var(--font-icon);\n}\n\n.feather {\n  width: 24px;\n  height: 24px;\n  stroke: currentColor;\n  stroke-width: 2;\n  stroke-linecap: round;\n  stroke-linejoin: round;\n  fill: none;\n}\n\n.fth-alert-octagon:before {\n  content: "\uE81B";\n}\n\n.fth-alert-circle:before {\n  content: "\uE81C";\n}\n\n.fth-activity:before {\n  content: "\uE81D";\n}\n\n.fth-alert-triangle:before {\n  content: "\uE81E";\n}\n\n.fth-align-center:before {\n  content: "\uE81F";\n}\n\n.fth-airplay:before {\n  content: "\uE820";\n}\n\n.fth-align-justify:before {\n  content: "\uE821";\n}\n\n.fth-align-left:before {\n  content: "\uE822";\n}\n\n.fth-align-right:before {\n  content: "\uE823";\n}\n\n.fth-arrow-down-left:before {\n  content: "\uE824";\n}\n\n.fth-arrow-down-right:before {\n  content: "\uE825";\n}\n\n.fth-anchor:before {\n  content: "\uE826";\n}\n\n.fth-aperture:before {\n  content: "\uE827";\n}\n\n.fth-arrow-left:before {\n  content: "\uE828";\n}\n\n.fth-arrow-right:before {\n  content: "\uE829";\n}\n\n.fth-arrow-down:before {\n  content: "\uE82A";\n}\n\n.fth-arrow-up-left:before {\n  content: "\uE82B";\n}\n\n.fth-arrow-up-right:before {\n  content: "\uE82C";\n}\n\n.fth-arrow-up:before {\n  content: "\uE82D";\n}\n\n.fth-award:before {\n  content: "\uE82E";\n}\n\n.fth-bar-chart:before {\n  content: "\uE82F";\n}\n\n.fth-at-sign:before {\n  content: "\uE830";\n}\n\n.fth-bar-chart-2:before {\n  content: "\uE831";\n}\n\n.fth-battery-charging:before {\n  content: "\uE832";\n}\n\n.fth-bell-off:before {\n  content: "\uE833";\n}\n\n.fth-battery:before {\n  content: "\uE834";\n}\n\n.fth-bluetooth:before {\n  content: "\uE835";\n}\n\n.fth-bell:before {\n  content: "\uE836";\n}\n\n.fth-book:before {\n  content: "\uE837";\n}\n\n.fth-briefcase:before {\n  content: "\uE838";\n}\n\n.fth-camera-off:before {\n  content: "\uE839";\n}\n\n.fth-calendar:before {\n  content: "\uE83A";\n}\n\n.fth-bookmark:before {\n  content: "\uE83B";\n}\n\n.fth-box:before {\n  content: "\uE83C";\n}\n\n.fth-camera:before {\n  content: "\uE83D";\n}\n\n.fth-check-circle:before {\n  content: "\uE83E";\n}\n\n.fth-check:before {\n  content: "\uE83F";\n}\n\n.fth-check-square:before {\n  content: "\uE840";\n}\n\n.fth-cast:before {\n  content: "\uE841";\n}\n\n.fth-chevron-down:before {\n  content: "\uE842";\n}\n\n.fth-chevron-left:before {\n  content: "\uE843";\n}\n\n.fth-chevron-right:before {\n  content: "\uE844";\n}\n\n.fth-chevron-up:before {\n  content: "\uE845";\n}\n\n.fth-chevrons-down:before {\n  content: "\uE846";\n}\n\n.fth-chevrons-right:before {\n  content: "\uE847";\n}\n\n.fth-chevrons-up:before {\n  content: "\uE848";\n}\n\n.fth-chevrons-left:before {\n  content: "\uE849";\n}\n\n.fth-circle:before {\n  content: "\uE84A";\n}\n\n.fth-clipboard:before {\n  content: "\uE84B";\n}\n\n.fth-chrome:before {\n  content: "\uE84C";\n}\n\n.fth-clock:before {\n  content: "\uE84D";\n}\n\n.fth-cloud-lightning:before {\n  content: "\uE84E";\n}\n\n.fth-cloud-drizzle:before {\n  content: "\uE84F";\n}\n\n.fth-cloud-rain:before {\n  content: "\uE850";\n}\n\n.fth-cloud-off:before {\n  content: "\uE851";\n}\n\n.fth-codepen:before {\n  content: "\uE852";\n}\n\n.fth-cloud-snow:before {\n  content: "\uE853";\n}\n\n.fth-compass:before {\n  content: "\uE854";\n}\n\n.fth-copy:before {\n  content: "\uE855";\n}\n\n.fth-corner-down-right:before {\n  content: "\uE856";\n}\n\n.fth-corner-down-left:before {\n  content: "\uE857";\n}\n\n.fth-corner-left-down:before {\n  content: "\uE858";\n}\n\n.fth-corner-left-up:before {\n  content: "\uE859";\n}\n\n.fth-corner-up-left:before {\n  content: "\uE85A";\n}\n\n.fth-corner-up-right:before {\n  content: "\uE85B";\n}\n\n.fth-corner-right-down:before {\n  content: "\uE85C";\n}\n\n.fth-corner-right-up:before {\n  content: "\uE85D";\n}\n\n.fth-cpu:before {\n  content: "\uE85E";\n}\n\n.fth-credit-card:before {\n  content: "\uE85F";\n}\n\n.fth-crosshair:before {\n  content: "\uE860";\n}\n\n.fth-disc:before {\n  content: "\uE861";\n}\n\n.fth-delete:before {\n  content: "\uE862";\n}\n\n.fth-download-cloud:before {\n  content: "\uE863";\n}\n\n.fth-download:before {\n  content: "\uE864";\n}\n\n.fth-droplet:before {\n  content: "\uE865";\n}\n\n.fth-edit-2:before {\n  content: "\uE866";\n}\n\n.fth-edit:before {\n  content: "\uE867";\n}\n\n.fth-edit-1:before {\n  content: "\uE868";\n}\n\n.fth-external-link:before {\n  content: "\uE869";\n}\n\n.fth-eye:before {\n  content: "\uE86A";\n}\n\n.fth-feather:before {\n  content: "\uE86B";\n}\n\n.fth-facebook:before {\n  content: "\uE86C";\n}\n\n.fth-file-minus:before {\n  content: "\uE86D";\n}\n\n.fth-eye-off:before {\n  content: "\uE86E";\n}\n\n.fth-fast-forward:before {\n  content: "\uE86F";\n}\n\n.fth-file-text:before {\n  content: "\uE870";\n}\n\n.fth-film:before {\n  content: "\uE871";\n}\n\n.fth-file:before {\n  content: "\uE872";\n}\n\n.fth-file-plus:before {\n  content: "\uE873";\n}\n\n.fth-folder:before {\n  content: "\uE874";\n}\n\n.fth-filter:before {\n  content: "\uE875";\n}\n\n.fth-flag:before {\n  content: "\uE876";\n}\n\n.fth-globe:before {\n  content: "\uE877";\n}\n\n.fth-grid:before {\n  content: "\uE878";\n}\n\n.fth-heart:before {\n  content: "\uE879";\n}\n\n.fth-home:before {\n  content: "\uE87A";\n}\n\n.fth-github:before {\n  content: "\uE87B";\n}\n\n.fth-image:before {\n  content: "\uE87C";\n}\n\n.fth-inbox:before {\n  content: "\uE87D";\n}\n\n.fth-layers:before {\n  content: "\uE87E";\n}\n\n.fth-info:before {\n  content: "\uE87F";\n}\n\n.fth-instagram:before {\n  content: "\uE880";\n}\n\n.fth-layout:before {\n  content: "\uE881";\n}\n\n.fth-link-2:before {\n  content: "\uE882";\n}\n\n.fth-life-buoy:before {\n  content: "\uE883";\n}\n\n.fth-link:before {\n  content: "\uE884";\n}\n\n.fth-log-in:before {\n  content: "\uE885";\n}\n\n.fth-list:before {\n  content: "\uE886";\n}\n\n.fth-lock:before {\n  content: "\uE887";\n}\n\n.fth-log-out:before {\n  content: "\uE888";\n}\n\n.fth-loader:before {\n  content: "\uE889";\n}\n\n.fth-mail:before {\n  content: "\uE88A";\n}\n\n.fth-maximize-2:before {\n  content: "\uE88B";\n}\n\n.fth-map:before {\n  content: "\uE88C";\n}\n\n.fth-map-pin:before {\n  content: "\uE88E";\n}\n\n.fth-menu:before {\n  content: "\uE88F";\n}\n\n.fth-message-circle:before {\n  content: "\uE890";\n}\n\n.fth-message-square:before {\n  content: "\uE891";\n}\n\n.fth-minimize-2:before {\n  content: "\uE892";\n}\n\n.fth-mic-off:before {\n  content: "\uE893";\n}\n\n.fth-minus-circle:before {\n  content: "\uE894";\n}\n\n.fth-mic:before {\n  content: "\uE895";\n}\n\n.fth-minus-square:before {\n  content: "\uE896";\n}\n\n.fth-minus:before {\n  content: "\uE897";\n}\n\n.fth-moon:before {\n  content: "\uE898";\n}\n\n.fth-monitor:before {\n  content: "\uE899";\n}\n\n.fth-more-vertical:before {\n  content: "\uE89A";\n}\n\n.fth-more-horizontal:before {\n  content: "\uE89B";\n}\n\n.fth-move:before {\n  content: "\uE89C";\n}\n\n.fth-music:before {\n  content: "\uE89D";\n}\n\n.fth-navigation-2:before {\n  content: "\uE89E";\n}\n\n.fth-navigation:before {\n  content: "\uE89F";\n}\n\n.fth-octagon:before {\n  content: "\uE8A0";\n}\n\n.fth-package:before {\n  content: "\uE8A1";\n}\n\n.fth-pause-circle:before {\n  content: "\uE8A2";\n}\n\n.fth-pause:before {\n  content: "\uE8A3";\n}\n\n.fth-percent:before {\n  content: "\uE8A4";\n}\n\n.fth-phone-call:before {\n  content: "\uE8A5";\n}\n\n.fth-phone-forwarded:before {\n  content: "\uE8A6";\n}\n\n.fth-phone-missed:before {\n  content: "\uE8A7";\n}\n\n.fth-phone-off:before {\n  content: "\uE8A8";\n}\n\n.fth-phone-incoming:before {\n  content: "\uE8A9";\n}\n\n.fth-phone:before {\n  content: "\uE8AA";\n}\n\n.fth-phone-outgoing:before {\n  content: "\uE8AB";\n}\n\n.fth-pie-chart:before {\n  content: "\uE8AC";\n}\n\n.fth-play-circle:before {\n  content: "\uE8AD";\n}\n\n.fth-play:before {\n  content: "\uE8AE";\n}\n\n.fth-plus-square:before {\n  content: "\uE8AF";\n}\n\n.fth-plus-circle:before {\n  content: "\uE8B0";\n}\n\n.fth-plus:before {\n  content: "\uE8B1";\n}\n\n.fth-pocket:before {\n  content: "\uE8B2";\n}\n\n.fth-printer:before {\n  content: "\uE8B3";\n}\n\n.fth-power:before {\n  content: "\uE8B4";\n}\n\n.fth-radio:before {\n  content: "\uE8B5";\n}\n\n.fth-repeat:before {\n  content: "\uE8B6";\n}\n\n.fth-refresh-ccw:before {\n  content: "\uE8B7";\n}\n\n.fth-rewind:before {\n  content: "\uE8B8";\n}\n\n.fth-rotate-ccw:before {\n  content: "\uE8B9";\n}\n\n.fth-refresh-cw:before {\n  content: "\uE8BA";\n}\n\n.fth-rotate-cw:before {\n  content: "\uE8BB";\n}\n\n.fth-save:before {\n  content: "\uE8BC";\n}\n\n.fth-search:before {\n  content: "\uE8BD";\n}\n\n.fth-server:before {\n  content: "\uE8BE";\n}\n\n.fth-scissors:before {\n  content: "\uE8BF";\n}\n\n.fth-share-2:before {\n  content: "\uE8C0";\n}\n\n.fth-share:before {\n  content: "\uE8C1";\n}\n\n.fth-shield:before {\n  content: "\uE8C2";\n}\n\n.fth-settings:before {\n  content: "\uE8C3";\n}\n\n.fth-skip-back:before {\n  content: "\uE8C4";\n}\n\n.fth-shuffle:before {\n  content: "\uE8C5";\n}\n\n.fth-sidebar:before {\n  content: "\uE8C6";\n}\n\n.fth-skip-forward:before {\n  content: "\uE8C7";\n}\n\n.fth-slack:before {\n  content: "\uE8C8";\n}\n\n.fth-slash:before {\n  content: "\uE8C9";\n}\n\n.fth-smartphone:before {\n  content: "\uE8CA";\n}\n\n.fth-square:before {\n  content: "\uE8CB";\n}\n\n.fth-speaker:before {\n  content: "\uE8CC";\n}\n\n.fth-star:before {\n  content: "\uE8CD";\n}\n\n.fth-stop-circle:before {\n  content: "\uE8CE";\n}\n\n.fth-sun:before {\n  content: "\uE8CF";\n}\n\n.fth-sunrise:before {\n  content: "\uE8D0";\n}\n\n.fth-tablet:before {\n  content: "\uE8D1";\n}\n\n.fth-tag:before {\n  content: "\uE8D2";\n}\n\n.fth-sunset:before {\n  content: "\uE8D3";\n}\n\n.fth-target:before {\n  content: "\uE8D4";\n}\n\n.fth-thermometer:before {\n  content: "\uE8D5";\n}\n\n.fth-thumbs-up:before {\n  content: "\uE8D6";\n}\n\n.fth-thumbs-down:before {\n  content: "\uE8D7";\n}\n\n.fth-toggle-left:before {\n  content: "\uE8D8";\n}\n\n.fth-toggle-right:before {\n  content: "\uE8D9";\n}\n\n.fth-trash-2:before {\n  content: "\uE8DA";\n}\n\n.fth-trash:before {\n  content: "\uE8DB";\n}\n\n.fth-trending-up:before {\n  content: "\uE8DC";\n}\n\n.fth-trending-down:before {\n  content: "\uE8DD";\n}\n\n.fth-triangle:before {\n  content: "\uE8DE";\n}\n\n.fth-type:before {\n  content: "\uE8DF";\n}\n\n.fth-twitter:before {\n  content: "\uE8E0";\n}\n\n.fth-upload:before {\n  content: "\uE8E1";\n}\n\n.fth-umbrella:before {\n  content: "\uE8E2";\n}\n\n.fth-upload-cloud:before {\n  content: "\uE8E3";\n}\n\n.fth-unlock:before {\n  content: "\uE8E4";\n}\n\n.fth-user-check:before {\n  content: "\uE8E5";\n}\n\n.fth-user-minus:before {\n  content: "\uE8E6";\n}\n\n.fth-user-plus:before {\n  content: "\uE8E7";\n}\n\n.fth-user-x:before {\n  content: "\uE8E8";\n}\n\n.fth-user:before {\n  content: "\uE8E9";\n}\n\n.fth-users:before {\n  content: "\uE8EA";\n}\n\n.fth-video-off:before {\n  content: "\uE8EB";\n}\n\n.fth-video:before {\n  content: "\uE8EC";\n}\n\n.fth-voicemail:before {\n  content: "\uE8ED";\n}\n\n.fth-volume-x:before {\n  content: "\uE8EE";\n}\n\n.fth-volume-2:before {\n  content: "\uE8EF";\n}\n\n.fth-volume-1:before {\n  content: "\uE8F0";\n}\n\n.fth-volume:before {\n  content: "\uE8F1";\n}\n\n.fth-watch:before {\n  content: "\uE8F2";\n}\n\n.fth-wifi:before {\n  content: "\uE8F3";\n}\n\n.fth-x-square:before {\n  content: "\uE8F4";\n}\n\n.fth-wind:before {\n  content: "\uE8F5";\n}\n\n.fth-x:before {\n  content: "\uE8F6";\n}\n\n.fth-x-circle:before {\n  content: "\uE8F7";\n}\n\n.fth-zap:before {\n  content: "\uE8F8";\n}\n\n.fth-zoom-in:before {\n  content: "\uE8F9";\n}\n\n.fth-zoom-out:before {\n  content: "\uE8FA";\n}\n\n.fth-command:before {\n  content: "\uE8FB";\n}\n\n.fth-cloud:before {\n  content: "\uE8FC";\n}\n\n.fth-hash:before {\n  content: "\uE8FD";\n}\n\n.fth-headphones:before {\n  content: "\uE8FE";\n}\n\n.fth-underline:before {\n  content: "\uE8FF";\n}\n\n.fth-italic:before {\n  content: "\uE900";\n}\n\n.fth-bold:before {\n  content: "\uE901";\n}\n\n.fth-crop:before {\n  content: "\uE902";\n}\n\n.fth-help-circle:before {\n  content: "\uE903";\n}\n\n.fth-paperclip:before {\n  content: "\uE904";\n}\n\n.fth-shopping-cart:before {\n  content: "\uE905";\n}\n\n.fth-tv:before {\n  content: "\uE906";\n}\n\n.fth-wifi-off:before {\n  content: "\uE907";\n}\n\n.fth-minimize:before {\n  content: "\uE88D";\n}\n\n.fth-maximize:before {\n  content: "\uE908";\n}\n\n.fth-gitlab:before {\n  content: "\uE909";\n}\n\n.fth-sliders:before {\n  content: "\uE90A";\n}\n\n.fth-star-on:before {\n  content: "\uE90B";\n}\n\n.fth-heart-on:before {\n  content: "\uE90C";\n}\n\n.fth-archive:before {\n  content: "\uE90D";\n}\n\n.fth-arrow-down-circle:before {\n  content: "\uE90E";\n}\n\n.fth-arrow-up-circle:before {\n  content: "\uE90F";\n}\n\n.fth-arrow-left-circle:before {\n  content: "\uE910";\n}\n\n.fth-arrow-right-circle:before {\n  content: "\uE911";\n}\n\n.fth-bar-chart-line-:before {\n  content: "\uE912";\n}\n\n.fth-bar-chart-line:before {\n  content: "\uE913";\n}\n\n.fth-book-open:before {\n  content: "\uE914";\n}\n\n.fth-code:before {\n  content: "\uE915";\n}\n\n.fth-database:before {\n  content: "\uE916";\n}\n\n.fth-dollar-sign:before {\n  content: "\uE917";\n}\n\n.fth-folder-plus:before {\n  content: "\uE918";\n}\n\n.fth-gift:before {\n  content: "\uE919";\n}\n\n.fth-folder-minus:before {\n  content: "\uE91A";\n}\n\n.fth-git-commit:before {\n  content: "\uE91B";\n}\n\n.fth-git-branch:before {\n  content: "\uE91C";\n}\n\n.fth-git-pull-request:before {\n  content: "\uE91D";\n}\n\n.fth-git-merge:before {\n  content: "\uE91E";\n}\n\n.fth-linkedin:before {\n  content: "\uE91F";\n}\n\n.fth-hard-drive:before {\n  content: "\uE920";\n}\n\n.fth-more-vertical:before {\n  content: "\uE921";\n}\n\n.fth-more-horizontal:before {\n  content: "\uE922";\n}\n\n.fth-rss:before {\n  content: "\uE923";\n}\n\n.fth-send:before {\n  content: "\uE924";\n}\n\n.fth-shield-off:before {\n  content: "\uE925";\n}\n\n.fth-shopping-bag:before {\n  content: "\uE926";\n}\n\n.fth-terminal:before {\n  content: "\uE927";\n}\n\n.fth-truck:before {\n  content: "\uE928";\n}\n\n.fth-zap-off:before {\n  content: "\uE929";\n}\n\n.fth-youtube:before {\n  content: "\uE92A";\n}\n\n.fth-google:before {\n  content: "\uE926";\n}\n\n.arrow-down, .arrow-up {\n  width: 0;\n  height: 0;\n  font-size: 0;\n  line-height: 0;\n  border: 4px solid transparent;\n  border-top-color: var(--color-primary);\n  position: relative;\n  top: 2px;\n}\n\n.arrow-up {\n  border-top-color: transparent;\n  border-bottom-color: var(--color-primary);\n  position: relative;\n  top: -2px;\n}';
var _themeLight = ":root, .theme-light {\n  --color-primary: #22272e;\n  --color-primary-low: rgba(34, 39, 46, 0.2);\n  --color-primary-text: #fff;\n  --color-secondary: #aaa;\n  --color-secondary-text: #222;\n  --color-tertiary: #c4e4de;\n  --color-tertiary-text: #2c3036;\n  --color-bg: #f2f2f2;\n  --color-bg-shade-1: #fff;\n  --color-bg-shade-2: #f8f8f8;\n  --color-bg-shade-3: #f5f5f5;\n  --color-bg-shade-4: #efefef;\n  --color-bg-shade-5: #eaeaea;\n  --color-text: #2c3036;\n  --color-text-dim: #85888c;\n  --color-text-dim-one: #a8acb0;\n  --color-page: #f2f2f2;\n  --color-box: #fff;\n  --color-box-light: #f8f8f8;\n  --color-box-nested: #f8f8f8;\n  --color-button: #22272e;\n  --color-button-text: #fff;\n  --color-button-light: #f5f5f5;\n  --color-button-light-onbg: #fff;\n  --color-button-focus-outline: 1px dotted #878787;\n  --color-input: #f8f8f8;\n  --color-input-border: 1px solid transparent;\n  --color-input-placeholder: #85888c;\n  --color-check: #f5f5f5;\n  --color-checked: #22272e;\n  --color-checked-fg: #fff;\n  --color-toggle: #f5f5f5;\n  --color-toggle-fg: #2c3036;\n  --color-toggled: #22272e;\n  --color-toggled-fg: #fff;\n  --color-input-focus-bg: #fff;\n  --color-input-focus-border: 1px solid #eaeaea;\n  --color-input-focus-shadow: 0 0 0 4px #f8f8f8;\n  --color-line: #f5f5f5;\n  --color-line-onbg: #efefef;\n  --color-line-dashed: #eaeaea;\n  --color-line-dashed-onbg: #c2c2c2;\n  --color-tree: #fff;\n  --color-tree-selected: #f8f8f8;\n  --color-tree-selected-line: #22272e;\n  --color-dropdown: #fff;\n  --color-dropdown-line: #f5f5f5;\n  --color-dropdown-border: #efefef;\n  --color-dropdown-selected: #f8f8f8;\n  --color-overlay-shade: rgba(0, 0, 0, 0.2);\n  --color-overlay: #fff;\n  --color-overlay-editor: #f2f2f2;\n  --color-overlay-footer: #f8f8f8;\n  --color-table: #fff;\n  --color-table-line-horizontal: #f5f5f5;\n  --color-table-line-vertical: transparent;\n  --color-table-head: #f8f8f8;\n  --color-table-hover: #f8f8f8;\n  --color-table-highlight: #f5f5f5;\n  --color-required-marker: #22272e;\n  --color-synchronized: #aaa;\n}";
var _themeDark = ".theme-dark {\n  /*$color-primary: #eee; //#0d83f0; #22272e\n  $color-primary-fg: #222;\n  $color-primary-low: rgba(44, 48, 54, 0.2);\n  $color-secondary: #94c4bb;\n  $color-secondary-fg: #fff;\n  $color-tertiary: #c4e4de;\n  $color-tertiary-fg: #2c3036;\n\n  // foreground colors\n  $color-fg: #eee;\n  $color-fg-shade-1: #838383; // dim\n  $color-fg-shade-2: #8c9094; // dim-two\n\n  // background colors  \n  $color-bg: #1d1d1d;\n  $color-bg-shade-0: #232323;\n  $color-bg-shade-1: #282828; // bright\n  $color-bg-shade-2: #333333; // dim\n  $color-bg-shade-3: #3b3b3b; // bright-two\n  $color-bg-shade-4: #4a4a4a; // bright-three\n  $color-bg-shade-5: #43484f;\n  $color-bg-shade-6: #43484f;*/\n  --color-primary: #fff;\n  --color-primary-low: rgba(255, 255, 255, 0.2);\n  --color-primary-text: #000;\n  --color-secondary: #94c4bb;\n  --color-secondary-text: #fff;\n  --color-tertiary: #c4e4de;\n  --color-tertiary-text: #2c3036;\n  --color-bg: #181c22;\n  --color-bg-shade-1: #22272e;\n  --color-bg-shade-2: #1c2026;\n  --color-bg-shade-3: #2d323b;\n  --color-bg-shade-4: #363c45;\n  --color-bg-shade-5: #43484f;\n  --color-text: #fafafa;\n  --color-text-dim: #aaacaf;\n  --color-text-dim-one: #8c9094;\n  --color-page: #181c22;\n  --color-box: #22272e;\n  --color-box-light: #1c2026;\n  --color-box-nested: #1c2026;\n  --color-button: #fff;\n  --color-button-text: #000;\n  --color-button-light: #2d323b;\n  --color-button-light-onbg: #2d323b;\n  --color-input: #1c2026;\n  --color-input-border: 1px solid #43484f;\n  --color-input-placeholder: #aaacaf;\n  --color-check: #2d323b;\n  --color-checked: #fff;\n  --color-checked-fg: #000;\n  --color-toggle: #2d323b;\n  --color-toggle-fg: #fafafa;\n  --color-toggled: #fff;\n  --color-toggled-fg: #000;\n  --color-input-focus-bg: #22272e;\n  --color-input-focus-border: 1px solid #43484f;\n  --color-input-focus-shadow: 0 0 0 4px #1c2026;\n  --color-line: #2d323b;\n  --color-line-onbg: #2d323b;\n  --color-line-dashed: #43484f;\n  --color-line-dashed-onbg: #43484f;\n  --color-tree: #22272e;\n  --color-tree-selected: #1d2229;\n  --color-tree-selected-line: #fff;\n  --color-dropdown: #2d323b;\n  --color-dropdown-line: #363c45;\n  --color-dropdown-border: transparent;\n  --color-dropdown-selected: #363c45;\n  --color-overlay-shade: rgba(0, 0, 0, 0.3);\n  --color-overlay: #22272e;\n  --color-overlay-editor: #181c22;\n  --color-overlay-footer: #1c2026;\n  --color-table: #22272e;\n  --color-table-line-horizontal: #181c22;\n  --color-table-line-vertical: transparent;\n  --color-table-head: #22272e;\n  --color-table-hover: #1c2026;\n  --color-table-highlight: #2d323b;\n  --color-required-marker: #aaacaf;\n  --color-synchronized: #fff;\n}";
var _accent = ":root {\n  --color-accent: #F9AA19;\n  --color-accent-fg: #ffffff;\n  --color-negative: rgb(216, 40, 83);\n  --color-image-bg: rgb(255, 255, 255);\n  --color-accent-info: rgb(44, 162, 212);\n  --color-accent-info-dim: rgba(44, 162, 212, 0.3);\n  --color-accent-info-bg: rgba(228, 242, 249, 0.6);\n  --color-accent-success: rgb(47, 187, 152);\n  --color-accent-success-bg: rgba(47, 187, 152, 0.1);\n  --color-accent-warn: rgb(247, 125, 5);\n  --color-accent-warn-bg: rgba(247, 125, 5, 0.1);\n  --color-accent-error: rgb(216, 40, 83);\n  --color-accent-error-bg: rgba(216, 40, 83, 0.08);\n}\n\n:root {\n  --color-accent-blue-gray: #527281;\n  --color-accent-gray: #8a9598;\n  --color-accent-brown: #795548;\n  --color-accent-blue: #429be2;\n  --color-accent-purple: #9f3f9d;\n  --color-accent-teal: #3bb1c0;\n  --color-accent-green: #2fbb98;\n  --color-accent-lime: #a0db61;\n  --color-accent-yellow: #fdd330;\n  --color-accent-orange: #ff662b;\n  --color-accent-red: rgb(216, 40, 83);\n}\n\n.color-blue-gray {\n  color: var(--color-accent-blue-gray) !important;\n}\n\n.bg-color-blue-gray {\n  background: var(--color-accent-blue-gray);\n}\n\n.color-gray {\n  color: var(--color-accent-gray) !important;\n}\n\n.bg-color-gray {\n  background: var(--color-accent-gray);\n}\n\n.color-brown {\n  color: var(--color-accent-brown) !important;\n}\n\n.bg-color-brown {\n  background: var(--color-accent-brown);\n}\n\n.color-blue {\n  color: var(--color-accent-blue) !important;\n}\n\n.bg-color-blue {\n  background: var(--color-accent-blue);\n}\n\n.color-purple {\n  color: var(--color-accent-purple) !important;\n}\n\n.bg-color-purple {\n  background: var(--color-accent-purple);\n}\n\n.color-teal {\n  color: var(--color-accent-teal) !important;\n}\n\n.bg-color-teal {\n  background: var(--color-accent-teal);\n}\n\n.color-green {\n  color: var(--color-accent-green) !important;\n}\n\n.bg-color-green {\n  background: var(--color-accent-green);\n}\n\n.color-lime {\n  color: var(--color-accent-lime) !important;\n}\n\n.bg-color-lime {\n  background: var(--color-accent-lime);\n}\n\n.color-yellow {\n  color: var(--color-accent-yellow) !important;\n}\n\n.bg-color-yellow {\n  background: var(--color-accent-yellow);\n}\n\n.color-orange {\n  color: var(--color-accent-orange) !important;\n}\n\n.bg-color-orange {\n  background: var(--color-accent-orange);\n}\n\n.color-red {\n  color: var(--color-accent-red) !important;\n}\n\n.bg-color-red {\n  background: var(--color-accent-red);\n}\n\n.color-default {\n  color: var(--color-text);\n}\n\n.bg-color-default {\n  background: var(--color-text);\n}\n\n.color-primary {\n  color: var(--color-primary);\n}\n\n.color-negative {\n  color: var(--color-negative);\n}";
var _dimensions = ":root {\n  --padding: 32px;\n  --padding-m: 24px;\n  --padding-s: 16px;\n  --padding-xs: 12px;\n  --padding-xxs: 8px;\n  --padding-l: 64px;\n  --height-top: 74px;\n  --radius: 9px;\n  --radius-inner: 6px;\n}";
var _fontSizes = ":root {\n  --font-size-factor: 1;\n  --font-size-base: calc(10px * ((9 + var(--font-size-factor)) / 10));\n  --font-size-2xs: calc(1.1 * var(--font-size-base));\n  --font-size-xs: calc(1.2 * var(--font-size-base));\n  --font-size-s: calc(1.3 * var(--font-size-base));\n  --font-size: calc(1.4 * var(--font-size-base));\n  --font-size-m: calc(1.5 * var(--font-size-base));\n  --font-size-l: calc(1.6 * var(--font-size-base));\n  --font-size-xl: calc(1.8 * var(--font-size-base));\n}";
var _shadows = ":root {\n  --shadow: 0 3px 20px rgba(0, 0, 0, 0.07);\n  --shadow-mid: 0 1px 12px rgba(0, 0, 0, 0.03);\n  --shadow-short: 0px 0.5px 2px 0 rgba(0, 0, 0, 0.05);\n  --shadow-dropdown: 3px 10px 32px rgba(0, 0, 0, 0.07);\n  --shadow-overlay: -30px 0 40px rgba(0, 0, 0, 0.07);\n  --shadow-overlay-dialog: 0 0 20px rgba(0, 0, 0, 0.07);\n}";
var _canvas = "body, html {\n  width: 100%;\n  margin: 0;\n  padding: 0;\n  font-size: 16px;\n  -webkit-font-smoothing: antialiased;\n  -moz-osx-font-smoothing: grayscale;\n  background: var(--color-page);\n}\n\nhtml {\n  height: 100vh;\n  -webkit-text-size-adjust: 100%;\n}\n\nbody {\n  height: 100vh;\n  background: var(--color-page);\n  color: var(--color-text);\n  font-size: 16px;\n  font-family: var(--font);\n}\n\n.clear {\n  display: block;\n  clear: both;\n  float: none;\n}\n\n.app {\n  height: 100vh;\n  display: grid;\n  grid-template-columns: auto 1fr;\n  justify-content: stretch;\n}\n\n.theme-rounded .app {\n  grid-gap: 0;\n}\n\n.app-main {\n  overflow-y: auto;\n}\n\n.fade-enter-active, .fade-leave-active {\n  transition: opacity 0.5s;\n}\n\n.fade-enter, .fade-leave-to {\n  opacity: 0;\n}\n\n.ui-split, .ui-split-three, .ui-split-four {\n  display: grid;\n  grid-template-columns: 1fr 1fr;\n  gap: var(--padding);\n  align-items: stretch;\n  justify-content: stretch;\n}\n.ui-split + .ui-split, .ui-split-three + .ui-split, .ui-split-four + .ui-split {\n  margin-top: 50px;\n}\n.ui-split .ui-property + .ui-property, .ui-split-three .ui-property + .ui-property, .ui-split-four .ui-property + .ui-property {\n  margin-top: 0;\n  padding-top: 0;\n  border-top: none;\n}\n\n.ui-split-three {\n  grid-template-columns: 1fr 1fr 1fr;\n}\n\n.ui-split-four {\n  grid-template-columns: 1fr 1fr 1fr 1fr;\n}";
var _navigation = '.app-nav-apps {\n  position: absolute;\n  left: 100%;\n  top: 0;\n  bottom: 0;\n  width: 360px;\n  background: var(--color-bg-shade-3);\n  z-index: -1;\n  margin-left: 0;\n  display: flex;\n  flex-direction: column;\n  display: none;\n}\n.app-nav-apps .ui-header-bar {\n  height: 92px;\n}\n\n.app-nav-app {\n  display: grid;\n  grid-template-columns: auto minmax(auto, 1fr) auto;\n  grid-gap: var(--padding-s);\n  align-items: center;\n  margin: 0 var(--padding-m);\n  padding: var(--padding-s) var(--padding-s);\n  border-radius: var(--radius);\n}\n.app-nav-app.is-active {\n  background: var(--color-bg-shade-4);\n  font-weight: 700;\n}\n\n.app-nav-app-icon {\n  height: 32px;\n}\n\n.app-nav {\n  position: relative;\n  background: var(--color-box);\n  width: 260px;\n  color: var(--color-text);\n  height: 100vh;\n  display: grid;\n  grid-template-rows: auto auto 1fr auto;\n  box-shadow: var(--shadow-short);\n  margin-right: 1px;\n  z-index: 5;\n}\n.theme-rounded .app-nav {\n  height: calc(100vh - 20px);\n  margin: 10px;\n  margin-right: 0;\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n}\n.app-nav-boxed {\n  height: 90px;\n  display: flex;\n  align-items: center;\n  justify-content: space-between;\n  padding: 0 var(--padding-xs) 0 0;\n}\n\n.app-nav-inner {\n  overflow-y: auto;\n  overflow-x: hidden;\n}\n\n.app-nav-headline {\n  display: flex;\n  align-items: center;\n  padding: 0 var(--padding-m);\n  margin: 0;\n}\n.theme-rounded .app-nav-headline {\n  margin: 0;\n}\n.app-nav-headline img {\n  height: 15px;\n}\n\n.app-nav-logo-circle {\n  display: inline-block;\n  width: 22px;\n  height: 22px;\n  border-radius: 20px;\n  border: 4px solid var(--color-accent);\n  margin-right: 12px;\n}\n\n.app-nav-switch {\n  margin-bottom: var(--padding-s);\n  background: var(--color-bg-shade-3);\n}\n.app-nav-switch .ui-button.type-light {\n  padding: 0 24px;\n  height: 70px;\n  background: transparent;\n  border-radius: 0;\n  font-size: var(--font-size-m);\n}\n.app-nav-switch .ui-dropdown-button-icon {\n  max-height: 20px;\n  max-width: 20px;\n}\n\na.app-nav-item, button.app-nav-item {\n  display: grid;\n  grid-template-columns: 28px 1fr auto;\n  gap: 6px;\n  align-items: center;\n  font-size: var(--font-size);\n  padding: 0 var(--padding-m);\n  height: 50px;\n  color: var(--color-text);\n  position: relative;\n}\na.app-nav-item + .app-nav-item, button.app-nav-item + .app-nav-item {\n  margin-top: 1px;\n}\na.app-nav-item:hover, button.app-nav-item:hover {\n  color: var(--color-text);\n  background: var(--color-tree-selected);\n}\na.app-nav-item:hover .app-nav-item-icon, button.app-nav-item:hover .app-nav-item-icon {\n  color: var(--color-text);\n}\na.app-nav-item.is-active:not([alias=dashboard]), a.app-nav-item.is-active-exact, button.app-nav-item.is-active:not([alias=dashboard]), button.app-nav-item.is-active-exact {\n  color: var(--color-text);\n  background: var(--color-tree-selected);\n  font-weight: 700;\n  border-right: 3px solid var(--color-accent);\n}\na.app-nav-item.is-active:not([alias=dashboard]) .app-nav-item-icon, a.app-nav-item.is-active-exact .app-nav-item-icon, button.app-nav-item.is-active:not([alias=dashboard]) .app-nav-item-icon, button.app-nav-item.is-active-exact .app-nav-item-icon {\n  color: var(--color-text);\n}\na.app-nav-item.is-active:not([alias=dashboard]) .app-nav-item-arrow, a.app-nav-item.is-active-exact .app-nav-item-arrow, button.app-nav-item.is-active:not([alias=dashboard]) .app-nav-item-arrow, button.app-nav-item.is-active-exact .app-nav-item-arrow {\n  transform: rotate(180deg);\n}\na.app-nav-item.is-active:not([alias=dashboard]) .app-nav-item-text, a.app-nav-item.is-active-exact .app-nav-item-text, button.app-nav-item.is-active:not([alias=dashboard]) .app-nav-item-text, button.app-nav-item.is-active-exact .app-nav-item-text {\n  color: var(--color-text);\n}\na.app-nav-item.is-active:not([alias=dashboard]):before, a.app-nav-item.is-active-exact:before, button.app-nav-item.is-active:not([alias=dashboard]):before, button.app-nav-item.is-active-exact:before {\n  content: "";\n  position: absolute;\n  left: 0;\n  top: 0;\n  bottom: 0;\n  display: inline-block;\n  background: var(--color-tree-selected-line);\n}\n\n.app-nav-item-text {\n  transition: color 0.2s ease;\n}\n\n.app-nav-item-icon {\n  font-size: 18px;\n  line-height: 1;\n  font-weight: 400;\n  position: relative;\n  top: -1px;\n  color: var(--color-text);\n  transition: color 0.2s ease;\n}\n\n.app-nav-item-arrow {\n  color: var(--color-text-dim);\n}\n\n.app-nav-children {\n  padding: 5px 0 10px;\n}\n\na.app-nav-child {\n  display: flex;\n  align-items: center;\n  font-size: var(--font-size);\n  padding: 0 var(--padding) 0 calc(var(--padding) + 26px);\n  height: 36px;\n  color: var(--color-text-dim);\n}\na.app-nav-child:hover, a.app-nav-child.is-active {\n  color: var(--color-text);\n}\na.app-nav-child.is-active {\n  font-weight: 700;\n}\n\n.app-nav-children-enter-active {\n  transition: all 0.3s ease;\n}\n\n.app-nav-children-leave-active {\n  transition: all 0;\n}\n\n.app-nav-children-enter, .app-nav-children-leave-to {\n  transform: translateX(10px);\n  opacity: 0;\n}\n\n.app-nav-account {\n  border-top: 1px solid var(--color-line-onbg);\n}\n\n.app-nav-account-button {\n  display: grid;\n  width: 100%;\n  grid-template-columns: auto minmax(auto, 1fr) auto;\n  gap: 16px;\n  color: var(--color-text-dim);\n  align-items: center;\n  padding: var(--padding-m);\n}\n.app-nav-account-button:hover {\n  background: var(--color-bg-shade-2);\n}\n.app-nav-account-button .-image {\n  height: 32px;\n  width: 32px;\n  border-radius: 18px;\n  position: relative;\n  top: -1px;\n  background: var(--color-bg-shade-3);\n  text-align: center;\n  line-height: 33px;\n  font-size: 16px;\n  overflow: hidden;\n  color: transparent;\n}\n.app-nav-account-button .-text {\n  font-weight: 400;\n  margin: 0;\n}\n.app-nav-account-button .-text strong {\n  font-weight: 700;\n  color: var(--color-text);\n}\n\n/* COMPACT MODE */\n.app-nav.is-compact {\n  width: 82px;\n}\n.app-nav.is-compact .app-nav-headline {\n  width: 100%;\n  overflow: hidden;\n  padding-left: 0;\n  padding-right: 0;\n}\n.app-nav.is-compact .app-nav-headline img {\n  margin-left: 29px;\n  clip-path: circle(23.78% at 13px 14px);\n  min-width: 118px;\n}\n.app-nav.is-compact .app-nav-boxed {\n  width: 100%;\n  overflow: hidden;\n}\n.app-nav.is-compact .app-nav-switch {\n  visibility: hidden;\n  pointer-events: none;\n  opacity: 0;\n  width: 100%;\n  overflow: hidden;\n  padding: 16px 0 0;\n}\n.app-nav.is-compact a.app-nav-item, .app-nav.is-compact button.app-nav-item {\n  display: flex;\n  padding-left: var(--padding);\n  width: 100%;\n}\n.app-nav.is-compact a.app-nav-item:hover + .app-nav-children, .app-nav.is-compact button.app-nav-item:hover + .app-nav-children {\n  display: block;\n}\n.app-nav.is-compact a.app-nav-item:before, .app-nav.is-compact button.app-nav-item:before {\n  display: none;\n}\n.app-nav.is-compact .app-nav-item-text, .app-nav.is-compact .app-nav-item-arrow {\n  display: none;\n}\n.app-nav.is-compact .app-nav-children {\n  display: none;\n  position: absolute;\n  z-index: 8;\n  min-width: 240px;\n  min-height: 20px;\n  background: var(--color-dropdown);\n  border-radius: var(--radius);\n  border-top-left-radius: 0;\n  border-bottom-left-radius: 0;\n  border: 1px solid var(--color-dropdown-border);\n  box-shadow: 6px 1px 8px rgba(0, 0, 0, 0.02);\n  padding: 5px;\n  color: var(--color-text);\n  margin-left: 82px;\n  margin-top: -55px;\n}\n.app-nav.is-compact .app-nav-children:hover {\n  display: block;\n}\n.app-nav.is-compact a.app-nav-child {\n  padding: 0 var(--padding);\n  display: grid;\n  width: 100%;\n  grid-template-columns: minmax(0, 1fr) auto;\n  gap: 6px;\n  align-items: center;\n  font-size: var(--font-size);\n  padding: 0 16px;\n  height: 48px;\n  color: var(--color-text-dim);\n  border-radius: var(--radius);\n  white-space: nowrap;\n  text-overflow: ellipsis;\n  overflow: hidden;\n  max-width: 100%;\n}\n.app-nav.is-compact a.app-nav-child:not([disabled]):hover, .app-nav.is-compact a.app-nav-child:focus {\n  background: var(--color-dropdown-selected);\n}\n.app-nav.is-compact a.app-nav-child.is-active {\n  color: var(--color-text);\n  font-weight: 700;\n}\n.app-nav.is-compact .app-nav-account {\n  padding: 0;\n  margin-bottom: var(--padding);\n  margin-left: 25px;\n}\n.app-nav.is-compact .app-nav-account-button {\n  display: block;\n  width: 32px;\n}\n.app-nav.is-compact .app-nav-account-button .-text, .app-nav.is-compact .app-nav-account-button .-arrow {\n  display: none;\n}';
var _button = 'button {\n  font-family: var(--font);\n  color: var(--color-text);\n  display: inline-block;\n  font-size: var(--font-size);\n}\nbutton:visited, button:link {\n  text-decoration: none;\n}\nbutton[disabled] {\n  cursor: default;\n  pointer-events: none;\n}\n\nbutton::-moz-focus-inner {\n  border: 0;\n}\n\n.ui-button,\n.ui-dot-button,\n.ui-icon-button {\n  position: relative;\n  display: inline-flex;\n  justify-content: space-between;\n  align-items: center;\n  background: var(--color-button);\n  color: var(--color-button-text);\n  padding: 0 21px;\n  height: 48px;\n  border-radius: var(--radius-inner);\n  font-size: var(--font-size);\n  font-weight: 700;\n  margin: 0;\n  -webkit-backface-visibility: hidden;\n  -webkit-appearance: none;\n  cursor: pointer;\n  user-select: none;\n  border: none;\n  overflow: hidden;\n  font-family: var(--font);\n}\n.ui-button:hover,\n.ui-dot-button:hover,\n.ui-icon-button:hover {\n  opacity: 0.9;\n}\n.ui-button:active,\n.ui-dot-button:active,\n.ui-icon-button:active {\n  opacity: 1;\n}\n.ui-button[disabled],\n.ui-dot-button[disabled],\n.ui-icon-button[disabled] {\n  cursor: default;\n  opacity: 0.7;\n  pointer-events: none;\n}\n.ui-button.type-big,\n.ui-dot-button.type-big,\n.ui-icon-button.type-big {\n  height: 50px;\n  padding: 0 30px;\n}\n.ui-button.type-small,\n.ui-dot-button.type-small,\n.ui-icon-button.type-small {\n  height: 38px;\n  font-size: var(--font-size-s);\n  padding: 0 16px;\n}\n.ui-button.type-light, .ui-button.type-action,\n.ui-dot-button.type-light,\n.ui-dot-button.type-action,\n.ui-icon-button.type-light,\n.ui-icon-button.type-action {\n  background: var(--color-button-light);\n  color: var(--color-text);\n  border: 1px solid transparent;\n}\n.ui-button.type-primary,\n.ui-dot-button.type-primary,\n.ui-icon-button.type-primary {\n  background: var(--color-accent);\n  color: var(--color-accent-fg);\n  min-width: 90px;\n  justify-content: center;\n}\n.ui-button.type-accent,\n.ui-dot-button.type-accent,\n.ui-icon-button.type-accent {\n  min-width: 90px;\n  justify-content: center;\n  background: var(--color-accent);\n  color: var(--color-accent-fg);\n  border: 1px solid transparent;\n}\n.ui-button.type-secondary,\n.ui-dot-button.type-secondary,\n.ui-icon-button.type-secondary {\n  background: var(--color-secondary);\n  color: var(--color-secondary-text);\n  border: 1px solid transparent;\n}\n.ui-button.type-primary.type-small,\n.ui-dot-button.type-primary.type-small,\n.ui-icon-button.type-primary.type-small {\n  min-width: 0;\n}\n.ui-button.type-light.type-onbg, .ui-button.type-action.type-onbg,\n.ui-dot-button.type-light.type-onbg,\n.ui-dot-button.type-action.type-onbg,\n.ui-icon-button.type-light.type-onbg,\n.ui-icon-button.type-action.type-onbg {\n  background: var(--color-button-light-onbg);\n  color: var(--color-text);\n  box-shadow: var(--shadow-short);\n}\n.ui-button.type-danger,\n.ui-dot-button.type-danger,\n.ui-icon-button.type-danger {\n  background: var(--color-accent-error);\n  color: white;\n}\n.ui-button.type-blank,\n.ui-dot-button.type-blank,\n.ui-icon-button.type-blank {\n  background: transparent;\n  color: var(--color-text);\n  padding: 0 16px;\n}\n.ui-button.type-block,\n.ui-dot-button.type-block,\n.ui-icon-button.type-block {\n  display: flex;\n  width: 100%;\n}\n.ui-button + .ui-button,\n.ui-dot-button + .ui-button,\n.ui-icon-button + .ui-button {\n  margin-left: 15px;\n}\n.ui-button.is-attached,\n.ui-dot-button.is-attached,\n.ui-icon-button.is-attached {\n  border-top-right-radius: 0;\n  border-bottom-right-radius: 0;\n}\n.ui-button.is-attached + .ui-button, .ui-button.is-attached + .ui-dropdown-container .ui-button,\n.ui-dot-button.is-attached + .ui-button,\n.ui-dot-button.is-attached + .ui-dropdown-container .ui-button,\n.ui-icon-button.is-attached + .ui-button,\n.ui-icon-button.is-attached + .ui-dropdown-container .ui-button {\n  border-top-left-radius: 0;\n  border-bottom-left-radius: 0;\n  margin-left: -1px;\n  border-left: 1px solid rgba(0, 0, 0, 0.1);\n  padding: 0 9px 0 8px;\n}\n.ui-button.is-attached + .ui-button .ui-button-icon, .ui-button.is-attached + .ui-dropdown-container .ui-button .ui-button-icon,\n.ui-dot-button.is-attached + .ui-button .ui-button-icon,\n.ui-dot-button.is-attached + .ui-dropdown-container .ui-button .ui-button-icon,\n.ui-icon-button.is-attached + .ui-button .ui-button-icon,\n.ui-icon-button.is-attached + .ui-dropdown-container .ui-button .ui-button-icon {\n  margin: 0 !important;\n}\n.ui-button .ui-button-icon, .ui-button .ui-button-caret,\n.ui-dot-button .ui-button-icon,\n.ui-dot-button .ui-button-caret,\n.ui-icon-button .ui-button-icon,\n.ui-icon-button .ui-button-caret {\n  margin-left: 18px;\n  margin-right: -3px;\n  position: relative;\n  font-size: var(--font-size-l);\n  top: -1px;\n  /*color: var(--color-text-dim);*/\n}\n.ui-button.type-small .ui-button-icon,\n.ui-dot-button.type-small .ui-button-icon,\n.ui-icon-button.type-small .ui-button-icon {\n  margin-left: 10px;\n  margin-right: -2px;\n  font-size: var(--font-size);\n}\n.ui-button.no-label .ui-button-icon,\n.ui-dot-button.no-label .ui-button-icon,\n.ui-icon-button.no-label .ui-button-icon {\n  margin: 0 -5px !important;\n}\n.ui-button.caret-left,\n.ui-dot-button.caret-left,\n.ui-icon-button.caret-left {\n  flex-direction: row-reverse;\n}\n.ui-button.caret-left .ui-button-icon, .ui-button.caret-left .ui-button-caret,\n.ui-dot-button.caret-left .ui-button-icon,\n.ui-dot-button.caret-left .ui-button-caret,\n.ui-icon-button.caret-left .ui-button-icon,\n.ui-icon-button.caret-left .ui-button-caret {\n  margin-right: 12px;\n  margin-left: -7px;\n}\n.ui-button.has-ellipsis .ui-button-text:after,\n.ui-dot-button.has-ellipsis .ui-button-text:after,\n.ui-icon-button.has-ellipsis .ui-button-text:after {\n  content: "...";\n}\n\n.ui-button-text span {\n  font-weight: 400;\n}\n\n.ui-button-state {\n  position: absolute;\n  left: 50%;\n  top: 50%;\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  width: 16px;\n  height: 16px;\n  margin-left: -8px;\n  margin-top: -8px;\n  text-align: center;\n}\n\n.ui-button-progress {\n  width: 100%;\n  height: 100%;\n  z-index: 2;\n  border-radius: 40px;\n  border: 2px solid rgba(255, 255, 255, 0.2);\n  border-left-color: var(--color-button-text);\n  opacity: 1;\n  will-change: transform;\n  animation: rotating 0.5s linear infinite;\n  transition: opacity 0.25s ease;\n}\n.type-light .ui-button-progress, .type-blank .ui-button-progress {\n  border-left-color: var(--color-text);\n}\n\n@keyframes rotating {\n  from {\n    -webkit-transform: rotate(0);\n    transform: rotate(0);\n  }\n  to {\n    -webkit-transform: rotate(1turn);\n    transform: rotate(1turn);\n  }\n}\n.ui-button.has-state:not(.state-default) > :not(.ui-button-state) {\n  visibility: hidden;\n}\n\n.ui-dot-button {\n  background: transparent;\n  padding: 0;\n  height: 28px;\n  width: 36px;\n  text-align: center;\n  justify-content: center;\n  color: var(--color-text);\n}\n.ui-dot-button:hover {\n  background: var(--color-button-light);\n}\n.ui-dot-button .ui-button-icon {\n  font-size: var(--font-size-xl);\n  margin: 0;\n}\n\n.ui-icon-button {\n  background: var(--color-button-light);\n  padding: 0 !important;\n  height: 32px;\n  width: 32px;\n  border-radius: 16px;\n  text-align: center;\n  justify-content: center;\n  font-size: var(--font-size-l);\n}\n.ui-icon-button:hover {\n  opacity: 1;\n}\n.ui-icon-button .ui-button-icon {\n  color: var(--color-text);\n  margin: 0;\n  top: 0;\n}\n.ui-icon-button.type-small {\n  height: 24px;\n  width: 24px;\n}\n.ui-icon-button.type-small .ui-button-icon {\n  font-size: 13px;\n  margin: 0 !important;\n}\n\n.ui-select-button {\n  color: var(--color-text);\n  display: inline-flex;\n  align-items: center;\n}\n\n.ui-select-button-icon {\n  width: 48px;\n  height: 48px;\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  border-radius: var(--radius-inner);\n  background: var(--color-button-light);\n  border: 1px solid transparent;\n  color: var(--color-text);\n  text-align: center;\n  font-size: 16px;\n  flex-shrink: 0;\n  overflow: hidden;\n}\n.ui-select-button-icon.is-image {\n  padding: 5px;\n  font-size: 0;\n  color: transparent;\n  background: var(--color-image-bg);\n  border-radius: var(--radius-inner);\n  border: 1px solid var(--color-line-onbg);\n}\n.ui-select-button-icon.is-image img {\n  object-fit: contain;\n  width: 100%;\n  height: 100%;\n  border-radius: var(--radius-inner);\n}\n\n.ui-select-button-content {\n  display: inline-flex;\n  flex-direction: column;\n  margin: 0 16px;\n}\n\nstrong.ui-select-button-label {\n  font-weight: 400;\n}\nstrong.ui-select-button-label b, strong.ui-select-button-label strong {\n  font-weight: 400;\n}\n\n.ui-select-button-description {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n  font-size: var(--font-size-xs);\n  display: -webkit-box;\n  -webkit-box-orient: vertical;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  white-space: normal;\n  -webkit-line-clamp: 1;\n  max-height: 16px;\n}\n.ui-select-button-description p {\n  margin: 0;\n}';
var _headlines = "h2.ui-headline {\n  font-family: var(--font);\n  margin: 0;\n  font-size: var(--font-size-l);\n  font-weight: 700;\n}\nh2.ui-headline.xl {\n  font-size: var(--font-size-xl);\n}\nh2.ui-headline .-minor {\n  font-size: var(--font-size);\n  color: var(--color-text-dim);\n  font-weight: 400;\n}\n\nh3.ui-headline {\n  font-family: var(--font);\n  margin: 0;\n  font-size: var(--font-size-m);\n  font-weight: 700;\n}\nh3.ui-headline .-minor {\n  display: block;\n  margin-top: 4px;\n  font-size: var(--font-size);\n  color: var(--color-text-dim);\n  font-weight: 400;\n}";
var _box = ".ui-box {\n  margin: var(--padding);\n  padding: var(--padding);\n  background: var(--color-box);\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n  /*max-width: 1104px;*/\n}\n.ui-box h2, .ui-box h3 {\n  margin: -5px 0 25px;\n  padding: 0;\n}\n.ui-property-content .ui-box {\n  margin: 0;\n  padding: 0;\n  background: transparent;\n  box-shadow: none;\n}\n.ui-box.is-light {\n  background: var(--color-box-light);\n}\n.ui-box.is-blank {\n  background: none;\n  margin: 0;\n  padding: 0;\n  border-radius: 0;\n  box-shadow: none;\n}\n\n.ui-box + .ui-box.is-connected,\n.ui-tabs + .ui-box.is-connected {\n  margin-top: -32px;\n  border-top: 1px solid var(--color-line-onbg);\n  border-top-left-radius: 0;\n  border-top-right-radius: 0;\n}\n\n.ui-blank-box {\n  margin: var(--padding);\n  border-radius: var(--radius);\n  /*max-width: 1104px;*/\n}\n\n.ui-view-box {\n  padding: var(--padding);\n  /*max-width: 1104px + 32px + 320px + 64px;*/\n}\n.ui-view-box.has-sidebar {\n  display: grid;\n  grid-template-columns: 1fr auto;\n  gap: 16px;\n}\n.ui-view-box .ui-box, .ui-view-box .ui-blank-box {\n  margin: 0;\n}\n.ui-view-box .ui-box + .ui-box {\n  margin-top: 16px;\n}\n\n.ui-view-box-aside {\n  width: 340px;\n  max-width: 100%;\n  padding: var(--padding);\n}\n.ui-view-box-aside.ui-box {\n  margin-top: 0 !important;\n}\n.ui-view-box-aside .ui-property + .ui-property {\n  padding-top: 0;\n  margin-top: 25px;\n}\n\n.ui-view-split {\n  display: grid;\n  grid-template-columns: 1fr 360px;\n}\n.ui-view-split > :last-child {\n  margin-top: 90px;\n}";
var _link = ".ui-link, a.ui-link {\n  color: var(--color-text);\n  text-decoration: underline dotted var(--color-text) !important;\n  text-underline-offset: 3px;\n}\n.ui-link.is-primary, a.ui-link.is-primary {\n  color: var(--color-primary);\n  text-decoration: underline dotted var(--color-primary) !important;\n}\n\n.ui-silent-link {\n  color: var(--color-text);\n}\n\n.ui-silent-link:hover {\n  color: var(--color-text);\n  text-decoration: underline dotted var(--color-text) !important;\n  text-underline-offset: 3px;\n}";
var _flatpickr = '@charset "UTF-8";\n.flatpickr-calendar {\n  width: 400px;\n}\n\n.flatpickr-months {\n  display: flex;\n  justify-content: space-between;\n  padding: 15px 10px;\n}\n\n.flatpickr-months .flatpickr-prev-month svg,\n.flatpickr-months .flatpickr-next-month svg {\n  width: 14px;\n  height: 14px;\n  fill: var(--color-text);\n}\n\n.flatpickr-prev-month,\n.flatpickr-next-month {\n  width: 32px;\n  height: 40px;\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  cursor: pointer;\n}\n\n.flatpickr-month {\n  position: relative;\n  width: 260px;\n  padding-right: 110px;\n}\n\n.flatpickr-current-month {\n  border: none;\n  background: var(--color-button-light);\n}\n.flatpickr-current-month select {\n  background: transparent;\n  color: var(--color-text);\n}\n.flatpickr-current-month:after {\n  opacity: 0.2;\n  transition: opacity 0.2s ease;\n}\n.flatpickr-current-month:hover:after {\n  opacity: 1;\n}\n.flatpickr-current-month .numInputWrapper {\n  position: absolute !important;\n  top: 0;\n  right: -110px;\n  width: 100px;\n}\n.flatpickr-current-month .numInputWrapper input {\n  background: var(--color-button-light);\n  border: none;\n}\n\n.flatpickr-innerContainer {\n  overflow: hidden;\n  padding: 15px 5px;\n}\n\n.flatpickr-weekdaycontainer,\n.flatpickr-days .dayContainer {\n  display: grid;\n  width: 100%;\n  grid-template-columns: repeat(7, 1fr);\n  text-align: center;\n}\n\n.flatpickr-weekday {\n  color: var(--color-text-dim-one);\n  font-size: var(--font-size-xs);\n  margin-bottom: 20px;\n}\n\n.flatpickr-day {\n  min-height: 40px;\n  line-height: 40px;\n  cursor: pointer;\n  position: relative;\n  font-size: var(--font-size-s);\n}\n.flatpickr-day:after {\n  content: "";\n  width: 30px;\n  height: 30px;\n  margin-left: -15px;\n  margin-top: -16px;\n  border-radius: 30px;\n  background: transparent;\n  position: absolute;\n  left: 50%;\n  top: 50%;\n  z-index: -1;\n  transition: background 0.2s ease;\n}\n.flatpickr-day:hover:after {\n  background: var(--color-button-light);\n}\n.flatpickr-day.today {\n  font-weight: 700;\n}\n.flatpickr-day.today:after {\n  background: var(--color-button-light);\n}\n.flatpickr-day.selected {\n  font-weight: 700;\n  color: var(--color-primary-text) !important;\n}\n.flatpickr-day.selected:after {\n  background: var(--color-primary);\n}\n.flatpickr-day.nextMonthDay, .flatpickr-day.prevMonthDay {\n  color: var(--color-text-dim-one);\n}\n\n.flatpickr-calendar .numInputWrapper {\n  position: relative;\n}\n.flatpickr-calendar .numInputWrapper input {\n  -webkit-appearance: textfield;\n  -moz-appearance: textfield;\n  appearance: textfield;\n}\n.flatpickr-calendar .numInputWrapper .arrowUp, .flatpickr-calendar .numInputWrapper .arrowDown {\n  position: absolute;\n  right: 5px;\n  top: 3px;\n  width: 21px;\n  height: 21px;\n  line-height: 21px;\n  text-align: center;\n  cursor: pointer;\n  opacity: 0.2;\n  transition: opacity 0.2s ease;\n}\n.flatpickr-calendar .numInputWrapper .arrowUp:before, .flatpickr-calendar .numInputWrapper .arrowDown:before {\n  font-family: var(--font-icon);\n  content: "\uE845";\n}\n.flatpickr-calendar .numInputWrapper .arrowDown {\n  top: auto;\n  bottom: 1px;\n}\n.flatpickr-calendar .numInputWrapper .arrowDown:before {\n  content: "\uE842";\n}\n.flatpickr-calendar .numInputWrapper:hover .arrowUp, .flatpickr-calendar .numInputWrapper:hover .arrowDown {\n  opacity: 1;\n}\n\n.flatpickr-time {\n  border-top: 1px solid var(--color-line);\n  background: var(--color-button-light);\n  padding: 20px 15px;\n  display: flex;\n  align-items: center;\n}\n.flatpickr-time input[type=number] {\n  padding-left: 20px;\n  border: none;\n  background: var(--color-dropdown);\n  box-shadow: var(--shadow-short);\n}\n\n.flatpickr-time-separator {\n  margin: 0 15px;\n}';
var _tree = ".app-tree {\n  width: 340px;\n  background: var(--color-tree);\n  padding: 0;\n  position: relative;\n  overflow-y: auto;\n  height: 100vh;\n  box-shadow: var(--shadow-short);\n}\n.theme-rounded .app-tree {\n  height: calc(100vh - 20px);\n  margin: 10px 0 10px 10px;\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n}\n.app-tree .ui-header-bar + .ui-tree {\n  margin-top: -10px;\n}\n.app-tree .ui-dot-button {\n  margin-right: -8px;\n}\n.app-tree .ui-tree-item.is-active:before {\n  background: var(--color-tree-selected);\n}\n\n.app-tree-resizable {\n  position: absolute;\n  top: 0;\n  bottom: 0;\n  background: var(--color-text);\n  opacity: 0;\n  right: 0;\n  width: 6px;\n  cursor: ew-resize;\n  transition: opacity 0.15s ease 0s;\n}\n.app-tree-resizable:hover {\n  transition-delay: 0.2s;\n  opacity: 0.04;\n}";
var _tag = ".ui-tag {\n  align-self: center;\n  display: inline-block;\n  font-size: var(--font-size-2xs);\n  font-weight: 500;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n}\n\n.ui-tag + .ui-tag {\n  margin-left: 8px;\n}";
var _list = ".ui-list-item {\n  display: grid;\n  width: 100%;\n  grid-template-columns: 1fr 32px;\n  gap: 6px;\n  align-items: center;\n  position: relative;\n  padding: 0 32px;\n}\n.ui-list-item.has-icon {\n  grid-template-columns: 32px 1fr 32px;\n}\n.ui-list-item:hover, .ui-list-item.is-selected {\n  background: var(--color-tree-selected);\n}\n\n.ui-list-item-icon {\n  font-size: 18px;\n  line-height: 1;\n  font-weight: 400;\n  position: relative;\n  top: -2px;\n  color: var(--color-text-dim);\n  align-self: center;\n}\n\n.ui-list-item-image {\n  max-width: 32px;\n  max-height: 34px;\n  justify-self: center;\n}\n\n.ui-list-item-content {\n  display: block;\n  overflow: hidden;\n  white-space: nowrap;\n  text-overflow: ellipsis;\n}\n\n.ui-list-item-text {\n  display: block;\n}\n\n.ui-list-item.is-selected .ui-list-item-text {\n  font-weight: 600;\n}\n\n.ui-list-item-description {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-xs);\n}\n\n.ui-list-item-selected-icon {\n  color: var(--color-primary);\n  justify-self: center;\n}";
var Auth = new Vue({
  data: () => ({
    isAuthenticated: false,
    rejectReason: null,
    user: {
      name: null,
      email: null
    }
  }),
  watch: {
    isAuthenticated(value) {
      this.$emit("authenticated", value);
    },
    user(value) {
      this.$emit("user", value);
    }
  },
  methods: {
    loadUser() {
      axios.get("authentication/getUser").then((res) => {
        this.isAuthenticated = res.data.success && res.data.model;
        if (res.data.success) {
          this.user = res.data.model;
        }
      });
    },
    rejectUser(reason) {
      this.rejectReason = reason;
      this.isAuthenticated = false;
      this.user = null;
    },
    setUser(user) {
      if (!user) {
        this.rejectUser();
        return;
      }
      this.isAuthenticated = true;
      this.user = user;
    },
    login(model) {
      return axios.post("authentication/loginUser", model).then((res) => {
        return new Promise((resolve, reject) => {
          if (res.data.success) {
            this.setUser(res.data.model);
            resolve(res.data);
          } else {
            this.rejectUser();
            reject(res.data.errors);
          }
        });
      });
    },
    logout() {
      let promise = axios.post("authentication/logoutUser");
      this.rejectUser("@login.rejectReasons.logout");
      return promise;
    },
    switchApp(appId) {
      return axios.post("authentication/switchApp", null, {params: {appId}}).then((res) => {
        this.$emit("appswitch", res.data);
        return Promise.resolve(res.data.success);
      });
    },
    openPasswordOverlay() {
      return Overlay.open({
        title: "@changepasswordoverlay.title",
        closeLabel: "@ui.close",
        confirmLabel: "@changepasswordoverlay.confirm",
        component: PasswordChangeOverlay
      });
    }
  }
});
var render$1d = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "ui-iconpicker-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.config.closeLabel}, on: {click: _vm.config.hide}})];
  }, proxy: true}])}, [_c("ui-search", {staticClass: "ui-iconpicker-overlay-search onbg", model: {value: _vm.query, callback: function($$v) {
    _vm.query = $$v;
  }, expression: "query"}}), _vm.config.colors ? _c("div", {staticClass: "ui-iconpicker-overlay-colors"}, _vm._l(_vm.colors, function(col) {
    var _obj;
    return _c("i", {class: (_obj = {"is-active": "color-" + col === _vm.color || col === "default" && !_vm.color}, _obj["bg-color-" + col] = true, _obj), attrs: {title: col}, on: {click: function($event) {
      return _vm.selectColor(col);
    }}});
  }), 0) : _vm._e(), _c("div", {staticClass: "ui-iconpicker-overlay-items", style: {"grid-template-columns": "repeat(" + _vm.columns + ", 1fr)"}}, _vm._l(_vm.items, function(item2) {
    var _obj;
    return _c("button", {staticClass: "ui-iconpicker-overlay-item", class: (_obj = {"is-active": item2 === _vm.icon}, _obj[_vm.color] = item2 === _vm.icon, _obj), attrs: {type: "button", title: item2}, on: {click: function($event) {
      return _vm.select(item2);
    }}}, [_c("ui-icon", {attrs: {symbol: item2, size: _vm.size}})], 1);
  }), 0)], 1);
};
var staticRenderFns$1d = [];
var overlay_vue_vue_type_style_index_0_lang$5 = ".ui-iconpicker-overlay content {\n  padding-top: 0;\n}\n.ui-iconpicker-overlay-items {\n  display: grid;\n  grid-template-columns: repeat(auto-fill, 61px);\n  grid-gap: 8px;\n  align-items: stretch;\n}\n.ui-iconpicker-overlay-item {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  padding: 22px 0;\n  border-radius: var(--radius);\n}\n.ui-iconpicker-overlay-item:hover {\n  background: var(--color-box);\n  box-shadow: var(--shadow-short);\n}\n.ui-iconpicker-overlay-item.is-active {\n  background: var(--color-primary);\n  color: var(--color-primary-text);\n  box-shadow: var(--shadow-short);\n}\n.ui-iconpicker-overlay-search {\n  margin-bottom: 20px;\n}\n.ui-iconpicker-overlay-colors {\n  text-align: center;\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  margin: 0 20px;\n}\n.ui-iconpicker-overlay-colors i {\n  display: inline-block;\n  width: 16px;\n  height: 16px;\n  border-radius: 20px;\n  cursor: pointer;\n  transition: transform 0.2s ease;\n}\n.ui-iconpicker-overlay-colors i.is-active {\n  transform: scale(1.4);\n}";
const script$1d = {
  props: {
    model: String,
    config: Object
  },
  data: () => ({
    file: null,
    colors: ["default", "gray", "blue-gray", "blue", "teal", "green", "lime", "yellow", "orange", "red", "purple", "brown"],
    icon: null,
    color: null,
    query: "",
    set: null,
    items: [],
    size: 20
  }),
  watch: {
    model() {
      this.init();
    },
    query() {
      this.debouncedSearch();
    }
  },
  computed: {
    columns() {
      return ~~(580 / (this.size + 45));
    }
  },
  created() {
    this.debouncedSearch = debounce(this.search, 100);
    this.set = this.config.set;
    this.items = this.set.icons;
    this.init();
  },
  mounted() {
    let $selected = this.$el.querySelector(".ui-iconpicker-overlay-item.is-active");
    if ($selected) {
      let $scrollable = this.$el.querySelector("content");
      const offset = $selected.offsetTop - $scrollable.clientHeight * 0.5 - 30;
      $scrollable.scrollTop = offset < 0 ? 0 : offset;
    }
  },
  methods: {
    confirm() {
      const result = (this.icon || "") + " " + (this.color || "").trim();
      this.config.confirm(result);
    },
    select(item2) {
      this.icon = item2;
      this.confirm();
    },
    selectColor(color) {
      this.color = color === "default" ? null : "color-" + color;
    },
    init() {
      if (!this.model) {
        this.icon = null;
        this.color = null;
      } else {
        const parts2 = this.model.split(" ");
        this.icon = parts2[0];
        this.color = parts2.length > 1 ? parts2[1] : null;
      }
    },
    search() {
      const query = this.query;
      if (!query) {
        this.items = this.set.icons;
      } else {
        this.items = filter$1(this.set.icons, function(item2) {
          return item2.toLowerCase().indexOf(query) > -1;
        });
      }
    }
  }
};
const __cssModules$1d = {};
var component$1d = normalizeComponent(script$1d, render$1d, staticRenderFns$1d, false, injectStyles$1d, null, null, null);
function injectStyles$1d(context) {
  for (let o in __cssModules$1d) {
    this[o] = __cssModules$1d[o];
  }
}
component$1d.options.__file = "app/components/pickers/iconPicker/overlay.vue";
var PickIconOverlay = component$1d.exports;
var render$1c = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.output ? _c("div", {staticClass: "ui-iconpicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _c("ui-select-button", {attrs: {icon: _vm.previewIcon, label: "@ui.icon", description: _vm.buttonDescription, disabled: _vm.disabled}, on: {click: _vm.pick}})], 1) : _vm._e();
};
var staticRenderFns$1c = [];
const script$1c = {
  name: "uiIconpicker",
  props: {
    value: {
      type: String,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    colors: {
      type: Boolean,
      default: false
    },
    output: {
      type: Boolean,
      default: true
    },
    set: {
      type: String,
      default: "feather"
    },
    options: {
      type: Object,
      default: () => {
        return {};
      }
    }
  },
  data: () => ({
    iconSet: null
  }),
  watch: {
    set() {
      this.loadSet();
    }
  },
  created() {
    this.loadSet();
  },
  computed: {
    buttonDescription() {
      return this.value ? this.value.split(" ")[0] : "@ui.icon_select";
    },
    previewIcon() {
      return this.value || "fth-plus";
    }
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    loadSet() {
      this.iconSet = __zero.icons.find((x) => x.alias === this.set);
    },
    pick() {
      if (this.disabled) {
        return;
      }
      let options2 = extend({
        title: "@iconpicker.title",
        closeLabel: "@ui.close",
        component: PickIconOverlay,
        display: "editor",
        set: this.iconSet,
        model: this.value,
        colors: this.colors,
        width: 660
      }, typeof this.options === "object" ? this.options : {});
      return Overlay.open(options2).then((value) => {
        this.onChange(value);
      });
    }
  }
};
const __cssModules$1c = {};
var component$1c = normalizeComponent(script$1c, render$1c, staticRenderFns$1c, false, injectStyles$1c, null, null, null);
function injectStyles$1c(context) {
  for (let o in __cssModules$1c) {
    this[o] = __cssModules$1c[o];
  }
}
component$1c.options.__file = "app/components/pickers/iconPicker/iconpicker.vue";
var uiIconpicker = component$1c.exports;
let hub = new Vue();
hub.name = "tobi";
hub.count = 7;
var __$_require_ssets_zero_svg__ = "/zero/assets/zero.1da33506.svg";
var __$_require_ssets_zero_dark_svg__ = "/zero/assets/zero-dark.f92cf530.svg";
var render$1b = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-nav", class: {"is-compact": _vm.compact}}, [_c("div", {staticClass: "app-nav-apps theme-light"}, [_c("ui-header-bar", {staticClass: "ui-tree-header", attrs: {title: "Applications", "back-button": false}}), _vm._l(_vm.applications, function(app2) {
    return _c("button", {key: app2.id, staticClass: "app-nav-app", class: {"is-active": app2.id == _vm.appId}, attrs: {type: "button"}, on: {click: function($event) {
      return _vm.applicationChanged(app2);
    }}}, [_c("img", {staticClass: "app-nav-app-icon", attrs: {src: app2.image, alt: app2.name}}), _c("ui-localize", {attrs: {value: app2.name}}), app2.id == _vm.appId ? _c("ui-icon", {staticClass: "app-nav-app-selected", attrs: {symbol: "fth-check"}}) : _vm._e()], 1);
  })], 2), _c("div", {staticClass: "app-nav-boxed"}, [_c("h1", {staticClass: "app-nav-headline"}, [_c("span", {staticClass: "app-nav-logo-circle"}), _c("img", {directives: [{name: "localize", rawName: "v-localize:alt", value: "@zero.name", expression: "'@zero.name'", arg: "alt"}], staticClass: "show-light", attrs: {src: __$_require_ssets_zero_svg__}}), _c("img", {directives: [{name: "localize", rawName: "v-localize:alt", value: "@zero.name", expression: "'@zero.name'", arg: "alt"}], staticClass: "show-dark", attrs: {src: __$_require_ssets_zero_dark_svg__}})]), _c("ui-button", {staticClass: "app-nav-search", attrs: {icon: "fth-search", stroke: 2.5, type: "blank"}, on: {click: _vm.openSearch}})], 1), _vm.applications.length > 0 ? _c("ui-dropdown", {staticClass: "app-nav-switch", scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light block", label: _vm.currentApplication.name, caret: "right"}})];
  }, proxy: true}], null, false, 2828153927)}, [_vm._l(_vm.applications, function(app2) {
    return _c("button", {key: app2.id, staticClass: "ui-dropdown-button has-icon", class: {"is-active": app2.id == _vm.appId}, attrs: {type: "button"}, on: {click: function($event) {
      return _vm.applicationChanged(app2);
    }}}, [_c("img", {staticClass: "ui-dropdown-button-icon", attrs: {src: app2.image, alt: app2.name}}), _c("ui-localize", {attrs: {value: app2.name}}), app2.id == _vm.appId ? _c("ui-icon", {staticClass: "ui-dropdown-button-selected", attrs: {symbol: "check"}}) : _vm._e()], 1);
  }), _c("ui-dropdown-separator"), _c("ui-dropdown-button", {attrs: {label: "Add new application", icon: "fth-plus"}, on: {click: _vm.addApplication}}), _c("ui-dropdown-button", {attrs: {label: "Manage apps", icon: "fth-edit-2"}, on: {click: _vm.manageApplications}})], 2) : _vm._e(), _c("nav", {staticClass: "app-nav-inner"}, [_vm._l(_vm.sections, function(section2) {
    return [_c("router-link", {staticClass: "app-nav-item", class: {"has-children": _vm.hasChildren(section2)}, attrs: {to: section2.url, alias: section2.alias}}, [_c("ui-icon", {staticClass: "app-nav-item-icon", attrs: {symbol: section2.icon, size: 18}}), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: section2.name, expression: "section.name"}], staticClass: "app-nav-item-text"}), _vm.hasChildren(section2) ? _c("ui-icon", {staticClass: "app-nav-item-arrow", attrs: {symbol: "fth-chevron-down"}}) : _vm._e()], 1), _c("transition", {attrs: {name: "app-nav-children"}}, [_vm.hasChildren(section2) && _vm.$route.path.indexOf("/" + section2.alias) === 0 ? _c("div", {staticClass: "app-nav-children"}, _vm._l(section2.children, function(child) {
      return _c("router-link", {key: child.alias, staticClass: "app-nav-child", attrs: {to: child.url}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: child.name, expression: "child.name"}], staticClass: "app-nav-child-text"})]);
    }), 1) : _vm._e()])];
  })], 2), _vm.user ? _c("ui-dropdown", {staticClass: "app-nav-account", attrs: {align: "left bottom"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("button", {staticClass: "app-nav-account-button", attrs: {type: "button"}}, [_vm.userAvatar ? _c("img", {staticClass: "-image", attrs: {src: _vm.userAvatar, alt: _vm.user.name}}) : _vm._e(), !_vm.userAvatar ? _c("span", {staticClass: "-image"}, [_c("i", {staticClass: "fth-user"})]) : _vm._e(), _c("p", {staticClass: "-text"}, [_c("strong", [_vm._v(_vm._s(_vm.user.name))])]), _c("ui-icon", {staticClass: "-arrow", attrs: {symbol: "fth-more-horizontal"}})], 1)];
  }, proxy: true}], null, false, 3368758236)}, [_c("ui-dropdown-button", {attrs: {label: "Edit", icon: "fth-edit-2"}, on: {click: _vm.editUser}}), _c("ui-dropdown-button", {attrs: {label: "Change password", icon: "fth-lock"}, on: {click: _vm.changePassword}}), !_vm.darkTheme ? _c("ui-dropdown-button", {attrs: {label: "Dark theme", icon: "fth-moon"}, on: {click: _vm.toggleDarkTheme}}) : _vm._e(), _vm.darkTheme ? _c("ui-dropdown-button", {attrs: {label: "Light theme", icon: "fth-sun"}, on: {click: _vm.toggleDarkTheme}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "Logout", icon: "fth-log-out"}, on: {click: _vm.logout}})], 1) : _vm._e()], 1);
};
var staticRenderFns$1b = [];
const compactCacheKey = "zero.navigation.compact";
const themeCacheKey = "zero.theme";
const script$1b = {
  name: "app-navigation",
  data: () => ({
    appId: zero.appId,
    applications: zero.applications,
    sections: zero.sections,
    user: null,
    userAvatar: null,
    compact: false,
    darkTheme: false,
    currentApplication: null
  }),
  components: {IconPicker: uiIconpicker},
  created() {
    this.currentApplication = find(this.applications, (x) => x.id === zero.appId);
    this.compact = localStorage.getItem(compactCacheKey) === "true";
    this.darkTheme = localStorage.getItem(themeCacheKey) === "dark";
    this.buildUser(Auth.user);
    Auth.$on("user", (user) => {
      this.buildUser(user);
    });
  },
  methods: {
    buildUser(user) {
      this.user = user;
      if (user && user.avatarId) {
        this.userAvatar = MediaApi.getImageSource(user.avatarId, false, true);
      } else {
        this.userAvatar = null;
      }
    },
    hasChildren(section2) {
      return section2.children && section2.children.length > 0;
    },
    editUser(item2, opts) {
      opts.hide();
      this.$router.push({
        name: zero.alias.settings.users + "-edit",
        params: {id: this.user.id}
      });
    },
    changePassword(item2, opts) {
      Auth.openPasswordOverlay();
      opts.hide();
    },
    logout(item2, opts) {
      Auth.logout();
      opts.hide();
    },
    addApplication(item2, opts) {
      opts.hide();
      this.$router.push({
        name: zero.alias.settings.applications + "-create"
      });
    },
    manageApplications(item2, opts) {
      opts.hide();
      this.$router.push({
        name: zero.alias.settings.applications
      });
    },
    applicationChanged(item2, opts) {
      Auth.switchApp(item2.id).then((success) => {
      });
    },
    toggleSidebar() {
      this.compact = !this.compact;
      localStorage.setItem(compactCacheKey, this.compact.toString());
    },
    toggleDarkTheme() {
      this.darkTheme = !this.darkTheme;
      hub.$emit("app.theme", this.darkTheme ? "dark" : "light");
      localStorage.setItem(themeCacheKey, this.darkTheme ? "dark" : "light");
      document.body.classList.toggle("theme-light", !this.darkTheme);
      document.body.classList.toggle("theme-dark", this.darkTheme);
    },
    openSearch() {
      hub.$emit("app.search.open");
    }
  }
};
const __cssModules$1b = {};
var component$1b = normalizeComponent(script$1b, render$1b, staticRenderFns$1b, false, injectStyles$1b, null, null, null);
function injectStyles$1b(context) {
  for (let o in __cssModules$1b) {
    this[o] = __cssModules$1b[o];
  }
}
component$1b.options.__file = "app/navigation.vue";
var AppNavigation = component$1b.exports;
var __$_require_ssets_zero_2_light_png__ = "/zero/assets/zero-2-light.9a87f092.png";
var __$_require_ssets_zero_2_png__ = "/zero/assets/zero-2.173ee257.png";
var render$1a = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-bar theme-dark"}, [_c("h1", {staticClass: "app-nav-headline"}, [_c("img", {directives: [{name: "localize", rawName: "v-localize:alt", value: "@zero.name", expression: "'@zero.name'", arg: "alt"}], staticClass: "show-light", attrs: {src: __$_require_ssets_zero_2_light_png__}}), _c("img", {directives: [{name: "localize", rawName: "v-localize:alt", value: "@zero.name", expression: "'@zero.name'", arg: "alt"}], staticClass: "show-dark", attrs: {src: __$_require_ssets_zero_2_png__}})]), _vm.applications.length > 0 ? _c("ui-dropdown", {staticClass: "app-nav-switch", scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light block", label: _vm.currentApplication.name, caret: "down"}})];
  }, proxy: true}], null, false, 1559454485)}, [_vm._l(_vm.applications, function(app2) {
    return _c("ui-dropdown-button", {key: app2.id, attrs: {value: app2, label: app2.name, selected: app2.id === _vm.appId, prevent: true}, on: {click: _vm.applicationChanged}});
  }), _c("ui-dropdown-separator"), _c("ui-dropdown-button", {attrs: {label: "Add new application", icon: "fth-plus"}, on: {click: _vm.addApplication}}), _c("ui-dropdown-button", {attrs: {label: "Manage apps", icon: "fth-edit-2"}, on: {click: _vm.manageApplications}})], 2) : _vm._e(), _c("div"), _vm.user ? _c("footer", {staticClass: "app-nav-account"}, [_c("ui-dropdown", {attrs: {align: "right top"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("button", {staticClass: "app-nav-account-button", attrs: {type: "button"}}, [_vm.userAvatar ? _c("img", {staticClass: "-image", attrs: {src: _vm.userAvatar, alt: _vm.user.name}}) : _vm._e(), !_vm.userAvatar ? _c("span", {staticClass: "-image"}, [_c("i", {staticClass: "fth-user"})]) : _vm._e(), _c("p", {staticClass: "-text"}, [_c("strong", [_vm._v(_vm._s(_vm.user.name))])]), _c("ui-icon", {staticClass: "-arrow", attrs: {symbol: "fth-chevron-down"}})], 1)];
  }, proxy: true}], null, false, 1539566190)}, [_c("ui-dropdown-button", {attrs: {label: "Edit", icon: "fth-edit-2"}, on: {click: _vm.editUser}}), _c("ui-dropdown-button", {attrs: {label: "Change password", icon: "fth-lock"}, on: {click: _vm.changePassword}}), _c("ui-dropdown-button", {attrs: {label: "Logout", icon: "fth-log-out"}, on: {click: _vm.logout}})], 1)], 1) : _vm._e()], 1);
};
var staticRenderFns$1a = [];
var bar_vue_vue_type_style_index_0_lang = ".app-bar {\n  grid-column: span 2/auto;\n  background: var(--color-bg-shade-3);\n  height: 70px;\n  display: grid;\n  align-items: center;\n  grid-template-columns: auto auto 1fr auto;\n  padding: 0 24px;\n}";
const script$1a = {
  name: "app-bar",
  data: () => ({
    appId: zero.appId,
    applications: zero.applications,
    user: null,
    userAvatar: null,
    currentApplication: null
  }),
  created() {
    this.currentApplication = find(this.applications, (x) => x.id === zero.appId);
    this.buildUser(Auth.user);
    Auth.$on("user", (user) => {
      this.buildUser(user);
    });
  },
  methods: {
    buildUser(user) {
      this.user = user;
      if (user && user.avatarId) {
        this.userAvatar = MediaApi.getImageSource(user.avatarId, false, true);
      } else {
        this.userAvatar = null;
      }
    },
    editUser(item2, opts) {
      opts.hide();
      this.$router.push({
        name: zero.alias.settings.users + "-edit",
        params: {id: this.user.id}
      });
    },
    changePassword(item2, opts) {
      Auth.openPasswordOverlay();
      opts.hide();
    },
    logout(item2, opts) {
      Auth.logout();
      opts.hide();
    },
    addApplication(item2, opts) {
      opts.hide();
      this.$router.push({
        name: zero.alias.settings.applications + "-create"
      });
    },
    manageApplications(item2, opts) {
      opts.hide();
      this.$router.push({
        name: zero.alias.settings.applications
      });
    },
    applicationChanged(item2, opts) {
      opts.loading(true);
      Auth.switchApp(item2.id).then((success) => {
        opts.loading(false);
      });
    }
  }
};
const __cssModules$1a = {};
var component$1a = normalizeComponent(script$1a, render$1a, staticRenderFns$1a, false, injectStyles$1a, null, null, null);
function injectStyles$1a(context) {
  for (let o in __cssModules$1a) {
    this[o] = __cssModules$1a[o];
  }
}
component$1a.options.__file = "app/bar.vue";
var AppBar = component$1a.exports;
var SearchApi = {
  query: async (query) => await get$1("search/query", {params: {query}})
};
var render$19 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-search"}, [_c("form", {staticClass: "app-search-form", on: {submit: function($event) {
    $event.preventDefault();
    return _vm.onSubmit($event);
  }}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.query, expression: "query"}], ref: "input", staticClass: "app-search-form-input", attrs: {type: "search", placeholder: "Search..."}, domProps: {value: _vm.query}, on: {input: function($event) {
    if ($event.target.composing) {
      return;
    }
    _vm.query = $event.target.value;
  }}}), _c("ui-button", {staticClass: "app-search-submit", attrs: {submit: true, type: "blank", icon: "fth-search"}})], 1), _vm.list.items.length ? _c("div", {staticClass: "app-search-items"}, _vm._l(_vm.list.items, function(item2) {
    return _c("router-link", {key: item2.id, staticClass: "app-search-item", attrs: {to: item2.url}, nativeOn: {click: function($event) {
      return _vm.config.close($event);
    }}}, [_c("ui-icon", {staticClass: "app-search-item-icon", attrs: {symbol: item2.icon, size: 18}}), _c("span", {staticClass: "app-search-item-text"}, [_c("span", {staticClass: "app-search-item-name"}, [_vm._v(_vm._s(item2.name) + " "), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item2.group, expression: "item.group"}], staticClass: "app-search-item-group"})]), item2.description ? _c("span", {staticClass: "app-search-item-description"}, [_vm._v(_vm._s(item2.description))]) : _vm._e()])], 1);
  }), 1) : _vm._e()]);
};
var staticRenderFns$19 = [];
var search_vue_vue_type_style_index_0_lang$1 = ".app-search-overlay .app-overlay {\n  padding: var(--padding);\n}\n.app-search-form {\n  position: relative;\n}\ninput.app-search-form-input {\n  padding-right: 48px;\n}\n.app-search-submit {\n  position: absolute;\n  right: 0;\n  height: 100%;\n  width: 48px;\n  justify-content: center;\n}\n.app-search-items {\n  margin-top: var(--padding-s);\n  font-size: var(--font-size);\n  max-height: 490px;\n  overflow-y: auto;\n}\n.app-search-item {\n  display: grid;\n  width: 100%;\n  grid-template-columns: 26px 1fr auto;\n  gap: 12px;\n  align-items: center;\n  position: relative;\n  color: var(--color-text);\n  padding: var(--padding-xs);\n  border-radius: var(--radius);\n}\n.app-search-item:hover, .app-search-item:focus {\n  background: var(--color-tree-selected);\n}\n.app-search-item + .app-search-item {\n  margin-top: 2px;\n}\n.app-search-item-text {\n  display: flex;\n  flex-direction: column;\n  position: relative;\n  top: 1px;\n}\n.app-search-item-name {\n  font-size: var(--font-size);\n  /*display: flex;\n  flex-direction: row;\n  align-items: center;\n  flex-wrap: wrap;*/\n}\n.app-search-item-description {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}\n.app-search-item-group {\n  display: block;\n  font-size: var(--font-size-xs);\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}\n.app-search-item-icon {\n  position: relative;\n  top: -1px;\n  left: 4px;\n  color: var(--color-text);\n}";
const script$19 = {
  props: {
    model: Object,
    config: Object
  },
  name: "app-search",
  data: () => ({
    open: false,
    query: null,
    list: {
      page: 1,
      totalPages: 1,
      items: []
    }
  }),
  mounted() {
    this.$nextTick(() => {
      this.$refs.input.focus();
    });
  },
  methods: {
    async onSubmit() {
      this.list = await SearchApi.query(this.query);
      console.info(this.list);
    }
  }
};
const __cssModules$19 = {};
var component$19 = normalizeComponent(script$19, render$19, staticRenderFns$19, false, injectStyles$19, null, null, null);
function injectStyles$19(context) {
  for (let o in __cssModules$19) {
    this[o] = __cssModules$19[o];
  }
}
component$19.options.__file = "app/search.vue";
var AppSearch = component$19.exports;
var render$18 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-auth"}, [_c("i", {staticClass: "fth-home app-auth-font-trigger"}), _c("span"), _c("ui-form", {staticClass: "app-auth-inner", on: {submit: _vm.onSubmit}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("div", [_c("div", {staticClass: "app-auth-logo"}, [_c("span", {staticClass: "app-nav-logo-circle"}), _c("img", {directives: [{name: "localize", rawName: "v-localize:alt", value: "@zero.name", expression: "'@zero.name'", arg: "alt"}], staticClass: "app-auth-image show-light", attrs: {src: __$_require_ssets_zero_svg__}}), _c("img", {directives: [{name: "localize", rawName: "v-localize:alt", value: "@zero.name", expression: "'@zero.name'", arg: "alt"}], staticClass: "app-auth-image show-dark", attrs: {src: __$_require_ssets_zero_dark_svg__}})]), _c("ui-error", {attrs: {"catch-remaining": true}}), _vm.rejectReason ? _c("ui-message", {attrs: {type: "info", text: _vm.rejectReason}}) : _vm._e(), _c("ui-property", {attrs: {field: "email", label: "@login.fields.email", vertical: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.model.email, expression: "model.email"}, {name: "localize", rawName: "v-localize:placeholder", value: "@login.fields.email_placeholder", expression: "'@login.fields.email_placeholder'", arg: "placeholder"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "120"}, domProps: {value: _vm.model.email}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.model, "email", $event.target.value);
    }}})]), _c("ui-property", {attrs: {field: "password", label: "@login.fields.password", vertical: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.model.password, expression: "model.password"}, {name: "localize", rawName: "v-localize:placeholder", value: "@login.fields.password_placeholder", expression: "'@login.fields.password_placeholder'", arg: "placeholder"}], staticClass: "ui-input", attrs: {type: "password", maxlength: "1024"}, domProps: {value: _vm.model.password}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.model, "password", $event.target.value);
    }}})])], 1), _c("div", {staticClass: "app-auth-bottom"}, [_c("ui-button", {staticClass: "app-auth-confirm", attrs: {type: "accent", submit: true, label: "@login.button", state: form.state}}), _c("ui-button", {attrs: {type: "blank", label: "@login.button_forgot"}})], 1)];
  }}])})], 1);
};
var staticRenderFns$18 = [];
var login_vue_vue_type_style_index_0_lang = ".app-auth {\n  grid-column: span 2/auto;\n  align-self: stretch;\n  justify-self: stretch;\n  display: grid;\n  grid-template-rows: 1fr auto 1fr;\n  align-items: center;\n  justify-content: center;\n  background: var(--color-page);\n  /*&:before\n  {\n    content: 'login';\n    font-family: var(--font-headline);\n    color: rgba(black, 0.008);\n    position: fixed;\n    left: -10vw;\n    bottom: -30vh;\n    font-size: 150vh;\n    line-height: 150vh;\n    font-weight: bold;\n  }*/\n}\n.app-auth-font-trigger {\n  position: absolute;\n  pointer-events: none;\n  opacity: 0.001;\n  bottom: 0;\n  right: 0;\n}\n.app-auth-inner {\n  display: grid;\n  grid-template-rows: 1fr auto;\n  align-items: stretch;\n  max-width: 100%;\n  width: 520px;\n  background: var(--color-box);\n  box-shadow: var(--shadow-short);\n  border-radius: var(--radius);\n  /*border: 1px solid var(--color-line);*/\n  position: relative;\n  z-index: 2;\n  padding: var(--padding);\n  color: var(--color-text);\n  /*box-shadow: 0 0 60px var(--color-shadow);*/\n}\n.app-auth-logo {\n  display: flex;\n  align-items: center;\n  margin: 0 0 3rem 0;\n}\n.app-auth-image {\n  height: 15px;\n}\n.app-auth .ui-property + .ui-property {\n  padding-top: 0;\n  margin-top: var(--padding);\n  border-top: none;\n}\n.app-auth-bottom {\n  margin-top: 3rem;\n}\n.app-auth .ui-message {\n  margin: -16px 0 var(--padding);\n}";
const script$18 = {
  name: "app-login",
  data: () => ({
    rejectReason: null,
    model: {
      email: null,
      password: null,
      isPersistent: true
    }
  }),
  created() {
    this.rejectReason = Auth.rejectReason;
  },
  methods: {
    onSubmit(form) {
      this.rejectReason = null;
      form.handle(Auth.login(this.model)).then((res) => {
        window.location.reload();
        Auth.$emit("apprebuild");
      }, (errors) => {
        console.info("login: error", errors);
      });
    }
  }
};
const __cssModules$18 = {};
var component$18 = normalizeComponent(script$18, render$18, staticRenderFns$18, false, injectStyles$18, null, null, null);
function injectStyles$18(context) {
  for (let o in __cssModules$18) {
    this[o] = __cssModules$18[o];
  }
}
component$18.options.__file = "app/pages/login/login.vue";
var AppLogin = component$18.exports;
var render$17 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-overlays", class: {"has-multiple": _vm.instances.length > 1}}, [_c("transition-group", {attrs: {name: "overlay", duration: 600}}, _vm._l(_vm.instances, function(instance, index) {
    return _c("div", {key: instance.id, staticClass: "app-overlay-outer", class: instance.class || "", style: {transform: instance.display !== "editor" ? null : "translateX(" + (_vm.editorLength - index - 1) * -120 + "px)"}, attrs: {display: instance.display}}, [_c("div", {staticClass: "app-overlay-bg", on: {click: function($event) {
      return _vm.close(instance);
    }}}), _c("div", {staticClass: "app-overlay", class: "theme-" + instance.theme, style: {width: instance.width ? instance.width + "px" : null}, attrs: {open: "", "data-alias": instance.alias, display: instance.display}}, [_c(instance.component, _vm._b({tag: "component", attrs: {model: instance.model, config: instance, title: ""}, on: {"update:model": function($event) {
      return _vm.$set(instance, "model", $event);
    }}}, "component", instance, false))], 1)]);
  }), 0)], 1);
};
var staticRenderFns$17 = [];
var overlayHolder_vue_vue_type_style_index_0_lang = ".app-overlays {\n  grid-column: span 2/auto;\n}\n.app-overlay-outer {\n  display: flex;\n  position: fixed;\n  left: 0;\n  top: 0;\n  right: 0;\n  bottom: 0;\n  z-index: 5;\n  justify-content: center;\n  align-items: flex-start;\n  transition: transform 0.55s ease-out;\n}\n.app-overlay-bg {\n  position: absolute;\n  left: 0;\n  top: 0;\n  right: 0;\n  bottom: 0;\n  background: var(--color-overlay-shade);\n  z-index: 2;\n  opacity: 1;\n}\n.app-overlay {\n  width: auto;\n  height: auto;\n  background: var(--color-overlay);\n  border-bottom-right-radius: var(--radius);\n  border-bottom-left-radius: var(--radius);\n  border: none !important;\n  box-shadow: var(--shadow-overlay-dialog);\n  padding: var(--padding);\n  padding-top: 40px;\n  text-align: left;\n  position: relative;\n  -webkit-backface-visibility: hidden;\n  z-index: 3;\n  color: var(--color-text);\n  font-size: var(--font-size);\n}\n.app-overlay .ui-loading {\n  position: relative;\n  left: 50%;\n  margin-left: -16px;\n  margin-top: 20px;\n  margin-bottom: 20px;\n}\n.app-overlay .ui-headline {\n  margin-bottom: 0.4em;\n}\n.app-overlay[display=dialog] .ui-form-loading {\n  height: 200px;\n}\n.app-overlay[display=editor] {\n  width: auto;\n  position: absolute;\n  left: auto;\n  right: 0;\n  top: 0;\n  bottom: 0;\n  box-shadow: var(--shadow-overlay);\n  background: var(--color-overlay-editor);\n  text-align: left;\n  padding: 0;\n  height: 100vh;\n  max-width: 100%;\n  border-bottom-right-radius: 0;\n  border-top-left-radius: var(--radius);\n}\n.overlay-enter-active .app-overlay-bg, .overlay-leave-active .app-overlay-bg {\n  transition: opacity 0.3s ease;\n}\n.overlay-enter-active .app-overlay, .overlay-leave-active .app-overlay {\n  transition: transform 0.3s ease, opacity 0.3s ease;\n}\n.overlay-enter-active .app-overlay[display=editor], .overlay-leave-active .app-overlay[display=editor] {\n  transition: transform 0.6s ease;\n}\n.overlay-enter .app-overlay-bg, .overlay-leave-to .app-overlay-bg {\n  opacity: 0;\n}\n.overlay-enter .app-overlay, .overlay-leave-to .app-overlay {\n  opacity: 0;\n  transform: translateY(-20px);\n}\n.overlay-enter .app-overlay[display=editor], .overlay-leave-to .app-overlay[display=editor] {\n  opacity: 1;\n  transform: scale(1) translateX(100%);\n}";
const script$17 = {
  data: () => ({
    instances: Overlay.instances
  }),
  computed: {
    editorLength() {
      return this.instances.filter((x) => x.display === "editor").length;
    }
  },
  mounted() {
    this.$el.addEventListener("keyup", (e) => {
      if (e.key === "Escape" && this.instances.length) {
        let instance = this.instances[this.instances.length - 1];
        instance.close();
      }
    });
  },
  beforeDestroy() {
    this.$el.removeEventListener("keyup");
  },
  methods: {
    close(instance) {
      if (instance.softdismiss !== false) {
        instance.close();
      }
    }
  }
};
const __cssModules$17 = {};
var component$17 = normalizeComponent(script$17, render$17, staticRenderFns$17, false, injectStyles$17, null, null, null);
function injectStyles$17(context) {
  for (let o in __cssModules$17) {
    this[o] = __cssModules$17[o];
  }
}
component$17.options.__file = "app/components/overlays/overlay-holder.vue";
var AppOverlays = component$17.exports;
var render$16 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "app-notifications", class: {"has-multiple": _vm.instances.length > 1}}, [_c("transition-group", {attrs: {name: "app-notifications", duration: 400}}, _vm._l(_vm.instances, function(instance) {
    return _c("div", {key: instance.id, staticClass: "app-notification", attrs: {type: instance.type}}, [_c("p", {staticClass: "-text"}, [_c("b", {directives: [{name: "localize", rawName: "v-localize", value: instance.label, expression: "instance.label"}]}), instance.text ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: instance.text, expression: "instance.text"}]}) : _vm._e()]), _c("ui-icon-button", {attrs: {type: "action small", icon: "fth-x", title: "@ui.close"}, on: {click: function($event) {
      return _vm.close(instance);
    }}})], 1);
  }), 0)], 1);
};
var staticRenderFns$16 = [];
var notificationHolder_vue_vue_type_style_index_0_lang = ".app-notifications {\n  position: fixed;\n  right: var(--padding);\n  bottom: var(--padding);\n  width: 420px;\n  max-width: 100%;\n  z-index: 6;\n}\n.app-notification {\n  display: grid;\n  grid-template-columns: 1fr auto;\n  gap: 20px;\n  align-items: center;\n  background: var(--color-primary);\n  color: var(--color-primary-text);\n  border-radius: var(--radius);\n  padding: 10px 12px;\n  font-size: var(--font-size);\n  transition: transform 0.3s ease, opacity 0.3s ease;\n}\n.app-notification .-text {\n  margin: 0;\n}\n.app-notification .-text span {\n  font-size: var(--font-size-s);\n  line-height: 1.3;\n}\n.app-notification .-text b {\n  display: block;\n  margin: 3px 0;\n}\n.app-notification .ui-icon-button {\n  background: none;\n  margin-right: -5px;\n}\n.app-notification .ui-button-icon {\n  color: var(--color-primary-text);\n}\n.app-notification + .app-notification {\n  margin-top: 10px;\n}\n.app-notification[type=success], .app-notification[type=primary] {\n  background: var(--color-primary);\n  color: var(--color-primary-text);\n}\n.app-notification[type=success] .ui-button-icon, .app-notification[type=primary] .ui-button-icon {\n  color: var(--color-primary-text);\n}\n.app-notification[type=error] {\n  background: var(--color-accent-error);\n  color: white;\n}\n.app-notification[type=error] .ui-button-icon {\n  color: white;\n}\n.app-notification.app-notifications-enter,\n.app-notification.app-notifications-leave-to {\n  opacity: 0;\n  transform: translateY(20px);\n}";
const script$16 = {
  data: () => ({
    instances: Notification.instances
  }),
  methods: {
    close(instance) {
      instance.close();
    }
  }
};
const __cssModules$16 = {};
var component$16 = normalizeComponent(script$16, render$16, staticRenderFns$16, false, injectStyles$16, null, null, null);
function injectStyles$16(context) {
  for (let o in __cssModules$16) {
    this[o] = __cssModules$16[o];
  }
}
component$16.options.__file = "app/components/notifications/notification-holder.vue";
var AppNotifications = component$16.exports;
var ConfigApi = {
  getConfig: async () => await get$1("zerovue/config")
};
var render$15 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {key: _vm.appKey, staticClass: "app", class: _vm.getClassList()}, [_vm.isAuthenticated ? [_c("app-navigation"), _c("div", {staticClass: "app-main"}, [_c("router-view")], 1), _c("app-overlays"), _c("app-notifications")] : _c("app-login")], 2);
};
var staticRenderFns$15 = [];
const script$15 = {
  name: "app",
  components: {AppNavigation, AppBar, AppSearch, AppOverlays, AppLogin, AppNotifications},
  data: () => ({
    isAuthenticated: false,
    keyIndex: 0
  }),
  computed: {
    appKey() {
      return "appkey-" + this.keyIndex;
    }
  },
  created() {
    Auth.setUser(__zero.user);
    Auth.$on("authenticated", (isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
    });
    Auth.$on("appswitch", (data) => {
      location.reload();
      this.rerender();
    });
    Auth.$on("apprebuild", (data) => {
      this.rerender();
    });
    hub.$on("app.search.open", () => {
      this.search();
    });
    if (localStorage.getItem("zero.theme") === "dark") {
      document.body.classList.remove("theme-light");
      document.body.classList.add("theme-dark");
    }
  },
  methods: {
    getClassList() {
      return {
        "is-preview": false
      };
    },
    rerender() {
      ConfigApi.getConfig().then((res) => {
        window.__zero = res;
        window.zero = res;
        Auth.setUser(res.user);
        this.keyIndex += 1;
      });
    },
    search() {
      Overlay.open({
        component: AppSearch,
        autoclose: false,
        softdismiss: true,
        width: 780,
        class: "app-search-overlay"
      });
    }
  }
};
const __cssModules$15 = {};
var component$15 = normalizeComponent(script$15, render$15, staticRenderFns$15, false, injectStyles$15, null, null, null);
function injectStyles$15(context) {
  for (let o in __cssModules$15) {
    this[o] = __cssModules$15[o];
  }
}
component$15.options.__file = "app/app.vue";
var App = component$15.exports;
var render$14 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-add-button"}, [!_vm.component ? _c("ui-button", {attrs: {type: _vm.type, label: _vm.label, disabled: _vm.disabled}, on: {click: _vm.onClick}}) : _c(_vm.component, {tag: "component", attrs: {type: _vm.type, label: _vm.label, disabled: _vm.disabled, route: _vm.route}, on: {click: _vm.onClick}})], 1);
};
var staticRenderFns$14 = [];
var addButton_vue_vue_type_style_index_0_lang = "\n.ui-add-button\n{\n  display: flex;\n}\n.ui-add-button-items\n{\n  display: grid;\n  grid-template-columns: 1fr 1px 1fr;\n}\n.ui-add-button-item\n{\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  padding: 20px 10px;\n  font-size: var(--font-size);\n  border-radius: var(--radius);\n}\n.ui-add-button-item i\n{\n  font-size: 20px;\n  line-height: 24px;\n  margin-bottom: 12px;\n}\n.ui-add-button-item .is-primary\n{\n  font-size: 24px;\n  color: var(--color-primary);\n}\n.ui-add-button-item:hover\n{\n  background: var(--color-button-light);\n}\n.ui-add-button-items-line\n{\n  display: block;\n  height: 100%;\n  background: var(--color-line);\n}\n";
const script$14 = {
  props: {
    label: {
      type: String,
      default: "@ui.add"
    },
    type: {
      type: String,
      default: "primary"
    },
    route: {
      type: [String, Object],
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    component: null
  }),
  created() {
    this.component = zero.overrides["add-button"] || null;
  },
  methods: {
    onClick() {
      if (this.$refs.dropdown) {
        this.$refs.dropdown.hide();
      }
      if (!!this.route) {
        let routeObj = typeof this.route === "object" ? this.route : {name: this.route};
        this.$router.push(routeObj);
      }
      this.$emit("click", false);
    }
  }
};
const __cssModules$14 = {};
var component$14 = normalizeComponent(script$14, render$14, staticRenderFns$14, false, injectStyles$14, null, null, null);
function injectStyles$14(context) {
  for (let o in __cssModules$14) {
    this[o] = __cssModules$14[o];
  }
}
component$14.options.__file = "app/components/buttons/add-button.vue";
var uiAddButton = component$14.exports;
var render$13 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("button", {staticClass: "ui-button has-state", class: _vm.buttonClass, attrs: {type: _vm.buttonType, disabled: _vm.disabled || _vm.state == "loading"}, on: {click: _vm.tryClick}}, [_vm.label ? _c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: _vm.label, expression: "label", arg: "html"}], staticClass: "ui-button-text"}) : _vm._e(), _vm.caret ? _c("ui-icon", {staticClass: "ui-button-caret", attrs: {symbol: _vm.caretSymbol}}) : _vm._e(), _vm.icon ? _c("ui-icon", {staticClass: "ui-button-icon", attrs: {symbol: _vm.icon, stroke: _vm.stroke}}) : _vm._e(), !_vm.isDefaultState ? _c("span", {staticClass: "ui-button-state"}, [_vm.stateDisplay == "loading" ? _c("i", {staticClass: "ui-button-progress"}) : _vm._e(), _vm.stateDisplay == "success" ? _c("ui-icon", {attrs: {symbol: "fth-check"}}) : _vm._e(), _vm.stateDisplay == "error" ? _c("ui-icon", {attrs: {symbol: "fth-x"}}) : _vm._e()], 1) : _vm._e()], 1);
};
var staticRenderFns$13 = [];
const STATE_DEFAULT = "default";
const STATE_LOADING = "loading";
const STATE_SUCCESS = "success";
const STATE_ERROR = "error";
const STATES = [STATE_DEFAULT, STATE_LOADING, STATE_SUCCESS, STATE_ERROR];
const script$13 = {
  name: "uiButton",
  props: {
    label: {
      type: String
    },
    state: {
      type: String,
      default: STATE_DEFAULT
    },
    submit: {
      type: Boolean,
      default: false
    },
    type: {
      type: String,
      default: "action"
    },
    icon: {
      type: String
    },
    stroke: {
      type: Number,
      default: 2
    },
    caret: String,
    caretPosition: {
      type: String,
      default: "right"
    },
    disabled: Boolean,
    stateDuration: {
      type: Number,
      default: 2e3
    },
    ellipsis: {
      type: Boolean,
      default: false
    },
    attach: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    stateDisplay: null,
    stateTimeout: null
  }),
  created() {
    this.stateDisplay = this.state;
  },
  computed: {
    buttonType() {
      return this.submit ? "submit" : "button";
    },
    buttonClass() {
      let classes = [];
      classes.push("type-" + this.type.split(" ").join(" type-"));
      classes.push("state-" + (this.isDefaultState ? STATE_DEFAULT : this.stateDisplay));
      if (this.caret) {
        classes.push("caret-" + this.caretPosition);
      }
      if (this.icon) {
        classes.push("has-icon");
      }
      if (this.ellipsis) {
        classes.push("has-ellipsis");
      }
      if (!this.label) {
        classes.push("no-label");
      }
      if (this.attach) {
        classes.push("is-attached");
      }
      return classes;
    },
    caretSymbol() {
      return "fth-chevron-" + this.caret;
    },
    isDefaultState() {
      return !this.stateDisplay || this.stateDisplay === STATE_DEFAULT || STATES.indexOf(this.stateDisplay) < 0;
    }
  },
  watch: {
    state(value) {
      this.stateDisplay = value;
      clearTimeout(this.stateTimeout);
      if (value && STATES.indexOf(value) < 0) {
        console.warn(`ui-button: Supported states are "${STATES.join('", "')}"`);
      }
      if (value === STATE_SUCCESS || value === STATE_ERROR) {
        this.stateTimeout = setTimeout(() => {
          this.stateDisplay = STATE_DEFAULT;
        }, this.stateDuration);
      }
    }
  },
  methods: {
    tryClick(ev) {
      if (this.isDefaultState) {
        this.$emit("click", ev);
      }
    }
  }
};
const __cssModules$13 = {};
var component$13 = normalizeComponent(script$13, render$13, staticRenderFns$13, false, injectStyles$13, null, null, null);
function injectStyles$13(context) {
  for (let o in __cssModules$13) {
    this[o] = __cssModules$13[o];
  }
}
component$13.options.__file = "app/components/buttons/button.vue";
var uiButton = component$13.exports;
var render$12 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: _vm.title, expression: "title", arg: "title"}], staticClass: "ui-dot-button", attrs: {type: "button", disabled: _vm.disabled}, on: {click: _vm.tryClick}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.title, expression: "title"}], staticClass: "sr-only"}), _c("ui-icon", {staticClass: "ui-button-icon", attrs: {symbol: "fth-more-horizontal"}})], 1);
};
var staticRenderFns$12 = [];
const script$12 = {
  name: "uiDotButton",
  props: {
    state: {
      type: String,
      default: "default"
    },
    title: {
      type: String,
      default: "@ui.actions"
    },
    disabled: Boolean
  },
  mounted() {
  },
  methods: {
    tryClick(ev) {
      this.$emit("click", ev);
    }
  }
};
const __cssModules$12 = {};
var component$12 = normalizeComponent(script$12, render$12, staticRenderFns$12, false, injectStyles$12, null, null, null);
function injectStyles$12(context) {
  for (let o in __cssModules$12) {
    this[o] = __cssModules$12[o];
  }
}
component$12.options.__file = "app/components/buttons/dot-button.vue";
var uiDotButton = component$12.exports;
var render$11 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: _vm.title, expression: "title", arg: "title"}], staticClass: "ui-icon-button", class: "type-" + _vm.type.split(" ").join(" type-"), attrs: {type: "button", disabled: _vm.disabled}, on: {click: _vm.tryClick}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.title, expression: "title"}], staticClass: "sr-only"}), _c("ui-icon", {staticClass: "ui-button-icon", attrs: {symbol: _vm.icon, size: _vm.size, stroke: _vm.stroke}})], 1);
};
var staticRenderFns$11 = [];
const script$11 = {
  name: "uiIconButton",
  props: {
    state: {
      type: String,
      default: "default"
    },
    icon: {
      type: String,
      default: "fth-arrow-left"
    },
    stroke: {
      type: Number,
      default: 2
    },
    type: {
      type: String,
      default: "action"
    },
    title: {
      type: String,
      default: ""
    },
    size: {
      type: Number,
      default: 14
    },
    disabled: Boolean
  },
  mounted() {
  },
  methods: {
    tryClick(ev) {
      this.$emit("click", ev);
    }
  }
};
const __cssModules$11 = {};
var component$11 = normalizeComponent(script$11, render$11, staticRenderFns$11, false, injectStyles$11, null, null, null);
function injectStyles$11(context) {
  for (let o in __cssModules$11) {
    this[o] = __cssModules$11[o];
  }
}
component$11.options.__file = "app/components/buttons/icon-button.vue";
var uiIconButton = component$11.exports;
var render$10 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("button", {staticClass: "ui-select-button type-light", attrs: {type: "button", disabled: _vm.disabled}, on: {click: _vm.tryClick}}, [!_vm.isImage ? _c("span", {staticClass: "ui-select-button-icon"}, [_c("ui-icon", {attrs: {symbol: _vm.icon}})], 1) : _vm._e(), _vm.isImage ? _c("span", {staticClass: "ui-select-button-icon is-image"}, [_c("img", {attrs: {src: _vm.source, alt: _vm.icon}})]) : _vm._e(), _vm.label || _vm.description ? _c("content", {staticClass: "ui-select-button-content"}, [_c("strong", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: _vm.label, tokens: _vm.tokens}, expression: "{ key: label, tokens: tokens }", arg: "html"}], staticClass: "ui-select-button-label"}), _vm.description ? _c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: _vm.description, tokens: _vm.tokens}, expression: "{ key: description, tokens: tokens }", arg: "html"}], staticClass: "ui-select-button-description"}) : _vm._e()]) : _vm._e(), _vm._t("default")], 2);
};
var staticRenderFns$10 = [];
const script$10 = {
  name: "uiSelectButton",
  props: {
    icon: {
      type: String,
      default: "fth-box"
    },
    iconAsImage: {
      type: Boolean,
      default: false
    },
    label: {
      type: String,
      default: null
    },
    description: {
      type: String,
      default: null
    },
    tokens: {
      type: Object,
      default: () => {
      }
    },
    disabled: Boolean
  },
  computed: {
    isImage() {
      return this.iconAsImage && this.icon.indexOf("fth-") !== 0;
    },
    source() {
      return this.iconAsImage ? this.icon.indexOf("/") === 0 ? this.icon : MediaApi.getImageSource(this.icon) : null;
    }
  },
  methods: {
    tryClick(ev) {
      this.$emit("click", ev);
    }
  }
};
const __cssModules$10 = {};
var component$10 = normalizeComponent(script$10, render$10, staticRenderFns$10, false, injectStyles$10, null, null, null);
function injectStyles$10(context) {
  for (let o in __cssModules$10) {
    this[o] = __cssModules$10[o];
  }
}
component$10.options.__file = "app/components/buttons/select-button.vue";
var uiSelectButton = component$10.exports;
var render$$ = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-state-button", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _vm._l(_vm.items, function(item2) {
    return _c("button", {directives: [{name: "localize", rawName: "v-localize", value: item2.label, expression: "item.label"}], staticClass: "ui-state-button-item", class: {"is-active": _vm.value === item2.value}, attrs: {type: "button", disabled: _vm.disabled || item2.disabled}, on: {click: function($event) {
      return _vm.select(item2.value);
    }}});
  })], 2);
};
var staticRenderFns$$ = [];
var stateButton_vue_vue_type_style_index_0_lang = "\n.ui-state-button\n{\n  display: inline-flex;\n}\n.ui-state-button.is-disabled button\n{\n  cursor: default;\n}\nbutton.ui-state-button-item\n{\n  background: var(--color-input);\n  padding: 10px 16px;\n  color: var(--color-text-dim);\n}\nbutton.ui-state-button-item + .ui-state-button-item\n{\n  border-left: none;\n}\nbutton.ui-state-button-item:first-of-type\n{\n  border-top-left-radius: var(--radius);\n  border-bottom-left-radius: var(--radius);\n}\nbutton.ui-state-button-item:last-child\n{\n  border-top-right-radius: var(--radius);\n  border-bottom-right-radius: var(--radius);\n}\nbutton.ui-state-button-item.is-active\n{\n  color: var(--color-text);\n  font-weight: bold;\n}\n";
const script$$ = {
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    items: {
      type: Array,
      default: []
    },
    value: {
      type: [String, Number],
      default: null
    }
  },
  methods: {
    select(value) {
      if (!this.disabled) {
        this.$emit("input", value);
      }
    }
  }
};
const __cssModules$$ = {};
var component$$ = normalizeComponent(script$$, render$$, staticRenderFns$$, false, injectStyles$$, null, null, null);
function injectStyles$$(context) {
  for (let o in __cssModules$$) {
    this[o] = __cssModules$$[o];
  }
}
component$$.options.__file = "app/components/buttons/state-button.vue";
var uiStateButton = component$$.exports;
var UtilsApi = {};
var render$_ = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-alias", class: {"is-disabled": _vm.disabled, "is-locked": _vm.locked}}, [_c("button", {staticClass: "ui-alias-lock", attrs: {type: "button"}, on: {click: _vm.toggleLock}}, [_c("i", {class: _vm.locked ? "fth-lock" : "fth-unlock"})]), _c("input", {ref: "input", staticClass: "ui-input", attrs: {type: "text", maxlength: _vm.maxLength, readonly: _vm.locked, disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: _vm.onChange, blur: function($event) {
    _vm.locked = true;
  }}})]);
};
var staticRenderFns$_ = [];
var alias_vue_vue_type_style_index_0_lang = "\n.ui-alias\n{\n  display: flex;\n  align-items: center;\n}\n.ui-alias button\n{\n  width: 24px;\n  height: 24px;\n  border-radius: var(--radius);\n  background: var(--color-box-nested);\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  font-size: 13px;\n  margin-right: 10px;\n}\n.ui-alias input\n{\n  background: transparent !important;\n  border: none !important;\n  box-shadow: none !important;\n  height: 24px !important;\n  padding: 0 !important;\n  outline: none !important;\n  min-width: 10px !important;\n  width: auto !important;\n}\n.ui-alias:not(.is-locked) input\n{\n  font-weight: bold;\n}\n";
const script$_ = {
  name: "uiAlias",
  props: {
    value: {
      type: String,
      default: null
    },
    name: {
      type: String,
      required: true,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    maxLength: {
      type: Number,
      default: 200
    }
  },
  data: () => ({
    custom: false,
    locked: true
  }),
  watch: {
    name: function(val) {
      this.updateAlias(val);
    }
  },
  mounted() {
    this.updateAlias(this.name);
  },
  methods: {
    onChange(ev) {
      let alias2 = ev.target.value;
      this.custom = alias2 !== this.value;
      this.updateAlias(alias2);
      this.$emit("input", alias2);
    },
    toggleLock() {
      this.locked = !this.locked;
      if (!this.locked) {
        this.$nextTick(() => {
          this.$refs.input.focus();
          this.$refs.input.select();
        });
      }
    },
    updateAlias(value) {
      UtilsApi.generateAlias(value).then((alias2) => {
        this.custom = alias2 !== this.value;
        this.$emit("input", alias2);
      });
    }
  }
};
const __cssModules$_ = {};
var component$_ = normalizeComponent(script$_, render$_, staticRenderFns$_, false, injectStyles$_, null, null, null);
function injectStyles$_(context) {
  for (let o in __cssModules$_) {
    this[o] = __cssModules$_[o];
  }
}
component$_.options.__file = "app/components/forms/alias.vue";
var uiAlias = component$_.exports;
var render$Z = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-check-list", class: {"is-disabled": _vm.disabled, "is-inline": _vm.inline}}, _vm._l(_vm.list, function(item2) {
    return _c("label", {staticClass: "ui-native-check ui-check-list-item"}, [_c("input", {attrs: {type: "checkbox", disabled: _vm.disabled}, domProps: {checked: _vm.isChecked(item2)}, on: {input: function($event) {
      return _vm.onChange(item2);
    }}}), _c("span", {staticClass: "ui-native-check-toggle"}), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item2[_vm.labelKey], expression: "item[labelKey]"}]})]);
  }), 0);
};
var staticRenderFns$Z = [];
var checkList_vue_vue_type_style_index_0_lang = ".ui-check-list-item {\n  display: block;\n}\n.ui-check-list .ui-check-list-item + .ui-check-list-item {\n  margin-top: 8px;\n}\n.ui-alias + .ui-check-list-item {\n  margin-top: 14px;\n}\n.ui-check-list.is-inline .ui-check-list-item {\n  display: inline-block;\n}\n.ui-check-list.is-inline .ui-check-list-item + .ui-check-list-item {\n  margin-top: 0;\n  margin-left: 30px;\n}\n.ui-check-list.is-inline .ui-check-list-item .ui-native-check-toggle {\n  margin-right: 6px;\n}";
const script$Z = {
  name: "uiCheckList",
  props: {
    value: {
      type: Array,
      default: () => []
    },
    items: {
      type: [Array, Function, Promise],
      required: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    inline: {
      type: Boolean,
      default: false
    },
    limit: {
      type: Number,
      default: 100
    },
    reverse: Boolean,
    labelKey: {
      type: String,
      default: "value"
    },
    idKey: {
      type: String,
      default: "key"
    }
  },
  data: () => ({
    list: []
  }),
  watch: {
    items() {
      this.init();
    }
  },
  mounted() {
    this.init();
  },
  methods: {
    init() {
      if (typeof this.items === "function") {
        this.items().then((res) => {
          this.list = res;
        });
      } else {
        this.list = JSON.parse(JSON.stringify(this.items));
      }
    },
    isChecked(item2) {
      let index = this.value.indexOf(item2[this.idKey]);
      return !this.reverse && index > -1 || this.reverse && index < 0;
    },
    onChange(item2) {
      let index = this.value.indexOf(item2[this.idKey]);
      let value = JSON.parse(JSON.stringify(this.value));
      if (index < 0) {
        value.push(item2[this.idKey]);
      } else {
        value.splice(index, 1);
      }
      this.$emit("input", value);
    }
  }
};
const __cssModules$Z = {};
var component$Z = normalizeComponent(script$Z, render$Z, staticRenderFns$Z, false, injectStyles$Z, null, null, null);
function injectStyles$Z(context) {
  for (let o in __cssModules$Z) {
    this[o] = __cssModules$Z[o];
  }
}
component$Z.options.__file = "app/components/forms/check-list.vue";
var uiCheckList = component$Z.exports;
var render$Y = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "page page-error", class: {"theme-dark": _vm.dark}}, [_c("i", {staticClass: "page-error-icon fth-cloud-snow"}), _c("p", {staticClass: "page-error-text"}, [_c("strong", {directives: [{name: "localize", rawName: "v-localize", value: {key: _vm.errorDetails.headline, tokens: _vm.tokens}, expression: "{ key: errorDetails.headline, tokens: tokens }"}], staticClass: "page-error-headline"}), _c("br"), _c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: _vm.errorDetails.text, tokens: _vm.tokens}, expression: "{ key: errorDetails.text, tokens: tokens }", arg: "html"}]})]), _vm.errorDetails.code === 404 ? _c("ui-button", {staticClass: "page-error-button", attrs: {type: "light onbg", label: _vm.detailsText}, on: {click: function($event) {
    _vm.details = !_vm.details;
  }}}) : _vm._e(), _vm.errorDetails.code !== 404 && _vm.errorDetails.category === 4 ? _c("ui-button", {staticClass: "page-error-button", attrs: {type: "light onbg", label: "@ui.back"}, on: {click: function($event) {
    return _vm.$router.go(-1);
  }}}) : _vm._e(), _vm.errorDetails.code === 404 && _vm.details ? [_c("br"), _c("br"), _c("div", {staticClass: "page-error-routes"}, [_c("span", [_vm._v("#")]), _c("span", [_vm._v("Name")]), _c("span", [_vm._v("Path")]), _vm._l(_vm.routes, function(route, index) {
    return [_c("span", [_vm._v(_vm._s(index + 1) + ".")]), _c("span", [_vm._v(_vm._s(route.name))]), _c("span", [_vm._v(_vm._s(route.path))])];
  })], 2)] : _vm._e()], 2);
};
var staticRenderFns$Y = [];
var errorView_vue_vue_type_style_index_0_lang = ".page-error {\n  width: 100%;\n  min-height: 100vh;\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  color: var(--color-text);\n  text-align: center;\n  padding: var(--padding);\n  overflow-y: auto;\n}\n.page-error-icon {\n  font-size: 82px;\n  color: var(--color-text);\n  margin-bottom: 20px;\n}\n.page-error-text {\n  font-size: var(--font-size);\n  color: var(--color-text-dim);\n  line-height: 1.4em;\n}\n.page-error-headline {\n  display: inline-block;\n  margin-bottom: 10px;\n  font-size: var(--font-size-l);\n  color: var(--color-text);\n}\n.page-error-button {\n  margin-top: 10px;\n}\n.page-error-routes {\n  display: grid;\n  grid-template-columns: auto auto 1fr;\n  width: 100%;\n  max-width: 100%;\n  text-align: left;\n  border-top: 1px solid var(--color-line);\n  border-left: 1px solid var(--color-line);\n  margin-top: 30px;\n}\n.page-error-routes span {\n  border: 1px solid var(--color-line);\n  border-left: none;\n  border-top: none;\n  padding: 8px 10px 6px;\n  font-family: Consolas;\n  font-size: 12px;\n}\n.page-error-routes span:nth-child(3n+1) {\n  color: var(--color-text-dim);\n}";
const KNOWN_ERRORS = [403, 404, 409, 504];
const script$Y = {
  props: {
    dark: {
      type: Boolean,
      default: false
    },
    error: {
      type: Error,
      default: null
    }
  },
  data: () => ({
    path: null,
    routes: [],
    details: false,
    errorDetails: {
      category: 0,
      code: null,
      headline: null,
      text: null
    }
  }),
  watch: {
    error: function(val) {
      this.rebuild();
    },
    $route: function(val) {
      this.rebuild();
    }
  },
  computed: {
    detailsText() {
      return this.details ? "Hide" : "Defined routes";
    },
    tokens() {
      return {
        code: this.errorDetails.code,
        path: this.path
      };
    }
  },
  mounted() {
    console.info(this.error.response);
    this.rebuild();
  },
  methods: {
    rebuild() {
      if (this.error && this.error.response) {
        let errorKey = null;
        const errorCode = this.error.response.status;
        const errorCategory = Math.round(errorCode / 100);
        if (KNOWN_ERRORS.indexOf(errorCode) > -1) {
          errorKey = "@errors.http." + errorCode;
        } else if (errorCategory === 4) {
          errorKey = "@errors.http.4xx";
        } else if (errorCategory === 5) {
          errorKey = "@errors.http.5xx";
        }
        this.errorDetails.category = errorCategory;
        this.errorDetails.code = errorCode;
        this.errorDetails.headline = errorKey;
        this.errorDetails.text = errorKey + "_text";
      }
      this.path = this.$router.options.base + this.$route.path.substring(1);
      this.routes = [];
      this.$router.options.routes.forEach((route) => {
        this.routes.push({
          path: route.path,
          name: route.name
        });
        if (route.children) {
          route.children.forEach((child) => {
            this.routes.push({
              path: route.path + "/" + child.path,
              name: child.name
            });
          });
        }
      });
    }
  }
};
const __cssModules$Y = {};
var component$Y = normalizeComponent(script$Y, render$Y, staticRenderFns$Y, false, injectStyles$Y, null, null, null);
function injectStyles$Y(context) {
  for (let o in __cssModules$Y) {
    this[o] = __cssModules$Y[o];
  }
}
component$Y.options.__file = "app/components/forms/error-view.vue";
var uiErrorView = component$Y.exports;
var render$X = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.visible ? _c("div", {staticClass: "ui-error"}, _vm._l(_vm.errors, function(error) {
    return _c("ui-message", {key: error.id, attrs: {type: "error", text: error.message, title: error.field}});
  }), 1) : _vm._e();
};
var staticRenderFns$X = [];
const script$X = {
  name: "uiError",
  props: {
    field: {
      type: String,
      default: ""
    },
    catchRemaining: {
      type: Boolean,
      default: false
    },
    catchAll: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    errors: []
  }),
  mounted() {
  },
  computed: {
    visible() {
      return this.errors && this.errors.length;
    }
  },
  methods: {
    setErrors(errors, append) {
      if (!errors) {
        return this.clearErrors();
      }
      if (!isArray(errors)) {
        errors = [errors];
      }
      errors.forEach((error) => {
        error.id = Strings.guid();
      });
      if (append) {
        errors.forEach((error) => {
          this.errors.push(error);
        });
      } else {
        this.errors = errors;
      }
    },
    clearErrors() {
      this.errors = [];
    }
  }
};
const __cssModules$X = {};
var component$X = normalizeComponent(script$X, render$X, staticRenderFns$X, false, injectStyles$X, null, null, null);
function injectStyles$X(context) {
  for (let o in __cssModules$X) {
    this[o] = __cssModules$X[o];
  }
}
component$X.options.__file = "app/components/forms/error.vue";
var uiError = component$X.exports;
var render$W = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-header-bar", {staticClass: "ui-form-header", attrs: {"back-button": true}, scopedSlots: _vm._u([{key: "title", fn: function() {
    return [_c("h2", {staticClass: "ui-header-bar-title", class: {"is-empty": _vm.title && !_vm.value.name && !_vm.titleDisabled}}, [_vm._l(_vm.prefixes, function(prefix2) {
      return [_c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: prefix2, expression: "prefix", arg: "html"}], staticClass: "-minor -prefix"}), _c("ui-icon", {staticClass: "-chevron", attrs: {symbol: "fth-chevron-right", size: 14}})];
    }), !_vm.titleDisabled ? _c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.value.name, expression: "value.name"}, {name: "localize", rawName: "v-localize:placeholder", value: _vm.title, expression: "title", arg: "placeholder"}], staticClass: "ui-form-header-title-input", attrs: {type: "text", readonly: _vm.titleDisabled || _vm.disabled}, domProps: {value: _vm.value.name}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.value, "name", $event.target.value);
    }}}) : _vm._e(), _vm.titleDisabled ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.forceTitle ? _vm.title : _vm.value.name || _vm.title, expression: "forceTitle ? title : (value.name || title)"}]}) : _vm._e()], 2)];
  }, proxy: true}])}, [_c("div", {staticClass: "ui-form-header-aside"}, [_vm._t("default"), !_vm.activeDisabled && typeof _vm.value.isActive !== "undefined" ? _c("div", {staticClass: "ui-form-header-toggle"}, [_c("ui-toggle", {staticClass: "is-accent", attrs: {"off-content": "@ui.inactive", "off-warning": true, "on-content": "@ui.active", "content-left": true, disabled: _vm.disabled}, model: {value: _vm.value.isActive, callback: function($$v) {
    _vm.$set(_vm.value, "isActive", $$v);
  }, expression: "value.isActive"}})], 1) : _vm._e(), _vm._t("buttons"), _vm.actionsDefined && !_vm.disabled ? _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.actions", caret: "down"}})];
  }, proxy: true}], null, false, 1918546237)}, [_vm._t("actions"), _vm.canDelete ? _c("ui-dropdown-button", {attrs: {label: "@ui.delete", icon: "fth-trash", disabled: _vm.disabled}, on: {click: _vm.onDelete}}) : _vm._e()], 2) : _vm._e(), !_vm.disabled ? _c("ui-button", {staticClass: "ui-form-header-primary-button", attrs: {submit: true, type: "primary", label: "@ui.save", state: _vm.state}}) : _vm._e()], 2)]);
};
var staticRenderFns$W = [];
var formHeader_vue_vue_type_style_index_0_lang = ".ui-form-header {\n  /*    width: 100%;\n      max-width: 1320px;\n      margin: 0 auto;*/\n}\n.ui-form-header-aside {\n  display: flex;\n  align-items: center;\n  justify-content: flex-end;\n}\n.ui-form-header-aside > * + * {\n  margin-left: var(--padding-s);\n}\n.ui-form-header-toggle {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  position: relative;\n  top: -1px;\n  margin-left: var(--padding-s);\n  margin-right: var(--padding-s);\n}\n.ui-form-header-toggle .ui-toggle-off-warning {\n  display: none;\n  color: var(--color-accent-red);\n}\n.ui-form-header-toggle .ui-toggle-switch {\n  background: var(--color-button-light-onbg);\n  box-shadow: var(--shadow-short) !important;\n}\n.ui-form-header-toggle .ui-toggle-switch.is-active {\n  background: var(--color-toggled);\n  box-shadow: none !important;\n}\n.ui-form-header-toggle input:focus + .ui-toggle-switch {\n  border-color: transparent;\n}\n.ui-header-bar-title {\n  position: relative;\n}\ninput[type=text].ui-form-header-title-input {\n  font-family: var(--font);\n  color: var(--color-text);\n  font-size: var(--font-size-l);\n  font-weight: 700;\n  background: none;\n  border: 1px dashed var(--color-line-dashed-onbg);\n  /*&:hover, &:focus, .ui-header-bar-title.is-empty &\n  {\n    border: 1px dashed var(--color-text-dim-one);\n  }*/\n}\n.-prefix + input[type=text].ui-form-header-title-input {\n  margin-left: 5px;\n}\n.ui-form-header-title-alias {\n  position: absolute;\n  right: 10px;\n  top: 11px;\n  z-index: 2;\n}\n.ui-form-header-title-alias .ui-alias-lock {\n  background: none;\n}\n.ui-form-header-info-button {\n  padding: 0;\n  justify-content: center;\n  width: 48px;\n  text-align: center;\n}\n.ui-form-header-info-button .ui-button-icon {\n  margin: 0 !important;\n  font-size: 18px;\n}";
const script$W = {
  name: "uiFormHeader",
  props: {
    title: {
      type: String,
      default: null
    },
    forceTitle: {
      type: Boolean,
      default: false
    },
    prefix: {
      type: [String, Array]
    },
    value: {
      type: Object
    },
    canDelete: {
      type: Boolean,
      default: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    titleDisabled: {
      type: Boolean,
      default: false
    },
    activeDisabled: {
      type: Boolean,
      default: false
    },
    state: {
      type: String
    },
    isCreate: {
      type: Boolean,
      default: false
    },
    hasAlias: {
      type: Boolean,
      default: true
    }
  },
  computed: {
    actionsDefined() {
      return !this.isCreate && (this.canDelete || this.$scopedSlots.hasOwnProperty("actions"));
    },
    prefixes() {
      let items = Array.isArray(this.prefix) ? this.prefix : [this.prefix];
      return items.filter((x) => !!x);
    }
  },
  methods: {
    onDelete(item2, opts) {
      this.$emit("delete", item2, opts);
    }
  }
};
const __cssModules$W = {};
var component$W = normalizeComponent(script$W, render$W, staticRenderFns$W, false, injectStyles$W, null, null, null);
function injectStyles$W(context) {
  for (let o in __cssModules$W) {
    this[o] = __cssModules$W[o];
  }
}
component$W.options.__file = "app/components/forms/form-header.vue";
var uiFormHeader = component$W.exports;
var render$V = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("form", {staticClass: "ui-form", on: {keydown: _vm.onKeydown, submit: function($event) {
    $event.preventDefault();
    return _vm.onSubmit($event);
  }, change: _vm.onChange}}, [_vm.loadingState === "default" ? _vm._t("default", null, null, _vm.slotProps) : _vm._e(), _vm.loadingState == "loading" ? _c("div", {staticClass: "ui-form-loading"}, [_c("i", {staticClass: "ui-form-loading-progress"})]) : _vm._e(), _vm.loadingState === "error" ? _c("ui-error-view", {attrs: {error: _vm.loadingError}}) : _vm._e()], 2);
};
var staticRenderFns$V = [];
var form_vue_vue_type_style_index_0_lang = ".ui-form {\n  min-height: 100%;\n  font-size: var(--font-size);\n}\n.ui-form-loading {\n  display: flex;\n  width: 100%;\n  height: 100vh;\n  align-items: center;\n  justify-content: center;\n}\n.ui-form-loading-progress {\n  width: 32px;\n  height: 32px;\n  z-index: 2;\n  border-radius: 40px;\n  border: 2px solid var(--color-bg-shade-3);\n  border-left-color: var(--color-text);\n  opacity: 1;\n  will-change: transform;\n  animation: rotating 0.5s linear infinite;\n  transition: opacity 0.25s ease;\n}\n@keyframes rotating {\nfrom {\n    -webkit-transform: rotate(0);\n    transform: rotate(0);\n}\nto {\n    -webkit-transform: rotate(1turn);\n    transform: rotate(1turn);\n}\n}";
const script$V = {
  name: "uiForm",
  props: {
    errorComponents: {
      type: Array,
      default: () => ["uiError"]
    },
    inputComponents: {
      type: Array,
      default: () => ["uiProperty"]
    },
    route: {
      type: [String, Object],
      default: null
    }
  },
  data: () => ({
    dirty: false,
    loadingState: "default",
    loadingError: null,
    state: "default",
    errors: [],
    canEdit: true,
    slotProps: {
      state: null
    },
    submitBlocked: false
  }),
  watch: {
    $route: function() {
      this.$emit("load", this);
    },
    state(val) {
      this.slotProps.state = val;
    },
    canEdit(val) {
      this.$nextTick(() => {
        this.setCanEdit(val);
      });
    }
  },
  created() {
    this.slotProps.state = this.state;
    this.$emit("load", this);
  },
  methods: {
    beforeRouteLeave(to, from, next) {
      if (this.dirty) {
        Overlay.confirm({
          title: "@unsavedchanges.title",
          text: "@unsavedchanges.text",
          confirmLabel: "@unsavedchanges.confirm",
          closeLabel: "@unsavedchanges.close"
        }).then(() => next(false), () => {
          this.dirty = false;
          next();
        });
      } else {
        next();
      }
    },
    load(promise) {
      this.loadingState = "loading";
      return new Promise((resolve, reject) => {
        promise.then((response) => {
          if (response.meta && typeof response.meta.canEdit === "boolean") {
            this.canEdit = response.meta.canEdit;
          }
          resolve(response);
          this.loadingState = "default";
          this.$nextTick(() => {
            this.$emit("loaded", this);
          });
        }, (error) => {
          this.loadingState = "error";
          this.loadingError = error;
          reject(error);
        }).catch((exception) => {
          this.loadingState = "error";
          this.loadingError = exception;
        });
      });
    },
    handle(promise, isCreate) {
      this.setState("loading");
      let handleError = (errors, reject) => {
        this.setState("error");
        this.setErrors(errors);
        reject(errors);
      };
      return new Promise((resolve, reject) => {
        promise.then((response) => {
          this.clearErrors();
          if (response.success) {
            this.setState("success");
            this.setDirty(false);
            resolve(response);
            if (response.model && this.route && this.$route.name !== this.route) {
              let routeObj = typeof this.route === "object" ? this.route : {name: this.route};
              routeObj.params = routeObj.params || {};
              routeObj.query = this.$route.query || {};
              routeObj.params.id = response.model.id;
              this.$router.replace(routeObj);
            }
          } else {
            handleError(response.errors, reject);
          }
        }, (errors) => {
          handleError(errors, reject);
        }).catch((exception) => {
          this.setState("error");
          throw exception;
        });
      });
    },
    onSubmit(e) {
      if (!this.submitBlocked) {
        this.$emit("submit", this, e);
      }
    },
    onChange(e) {
      this.dirty = true;
    },
    onDelete(promise) {
      Overlay.confirmDelete().then((opts) => {
        opts.state("loading");
        promise().then((response) => {
          if (response.success) {
            opts.state("success");
            opts.hide();
            this.$router.go(-1);
            Notification.success("@deleteoverlay.success", "@deleteoverlay.success_text");
          } else {
            opts.errors(response.errors);
          }
        });
      });
    },
    onKeydown(e) {
      if (e.keyCode === 13) {
        this.submitBlocked = true;
        clearTimeout(this.submitBlockedTimeout);
        this.submitBlockedTimeout = setTimeout(() => this.submitBlocked = false, 300);
      }
    },
    setDirty(dirty) {
      this.dirty = dirty;
    },
    setState(state) {
      this.state = state;
    },
    clearErrors() {
      this.getErrorComponents().forEach((component2) => {
        component2.clearErrors();
        if (component2.tab) {
          component2.tab.clearErrors();
        }
      });
    },
    setErrors(errors) {
      if (typeof errors === "undefined" || !errors) {
        this.errors = [];
      } else {
        this.errors = !isArray(errors) ? [errors] : errors;
      }
      let errorComponents = this.getErrorComponents();
      let errorGroups = groupBy(this.errors, "property");
      let handledGroups = [];
      errorComponents.forEach((component2) => {
        let field = component2.field;
        if (field && errorGroups[field]) {
          handledGroups.push(field);
          component2.setErrors(errorGroups[field]);
          if (component2.tab) {
            component2.tab.setErrors(true);
          }
        }
      });
      each(errorGroups, (errorGroup, field) => {
        if (handledGroups.indexOf(field) < 0) {
          errorComponents.forEach((component2) => {
            if (component2.catchRemaining || component2.catchAll) {
              component2.setErrors(errorGroup, true);
              if (component2.tab) {
                component2.tab.setErrors(true);
              }
            }
          });
        }
      });
    },
    getErrorComponents() {
      let errorComponents = [];
      let traverseChildren = (parent, tab) => {
        parent.$children.forEach((component2) => {
          if (this.errorComponents.indexOf(component2.$options.name) > -1) {
            errorComponents.push({
              name: component2.$options.name,
              field: component2.field,
              catchAll: component2.catchAll,
              catchRemaining: component2.catchRemaining,
              component: component2,
              setErrors: component2.setErrors,
              clearErrors: component2.clearErrors,
              tab
            });
          } else {
            traverseChildren(component2, tab || (component2.$options.name === "uiTab" ? component2 : null));
          }
        });
      };
      traverseChildren(this);
      return errorComponents;
    },
    setCanEdit(canEdit) {
    }
  }
};
const __cssModules$V = {};
var component$V = normalizeComponent(script$V, render$V, staticRenderFns$V, false, injectStyles$V, null, null, null);
function injectStyles$V(context) {
  for (let o in __cssModules$V) {
    this[o] = __cssModules$V[o];
  }
}
component$V.options.__file = "app/components/forms/form.vue";
var uiForm = component$V.exports;
var render$U = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-input-list", class: {"is-disabled": _vm.disabled}}, [_vm._l(_vm.items, function(item2) {
    return _c("div", {staticClass: "ui-input-list-item"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: item2.value, expression: "item.value"}], staticClass: "ui-input", attrs: {type: "text", maxlength: _vm.maxLength, readonly: _vm.disabled, size: "5"}, domProps: {value: item2.value}, on: {input: [function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(item2, "value", $event.target.value);
    }, _vm.onChange]}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {type: "light", icon: "fth-x"}, on: {click: function($event) {
      return _vm.removeItem(item2);
    }}}) : _vm._e()], 1);
  }), !_vm.disabled && !_vm.plusButton ? _c("ui-button", {attrs: {type: "light", label: _vm.addLabel}, on: {click: _vm.addItem}}) : _vm._e(), !_vm.disabled && _vm.plusButton ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.items.length > 0 ? null : _vm.addLabel}, on: {click: _vm.addItem}}) : _vm._e()], 2);
};
var staticRenderFns$U = [];
var inputList_vue_vue_type_style_index_0_lang = ".ui-input-list-item {\n  display: grid;\n  grid-template-columns: 1fr auto auto;\n  background: var(--color-input);\n  border-radius: var(--radius);\n  margin-bottom: 6px;\n}\n.ui-input-list.is-disabled .ui-input-list-item {\n  background: transparent;\n}\n.ui-input-list-item .ui-input {\n  border-radius: var(--radius) 0 0 var(--radius);\n}\n.ui-input-list-item .ui-icon-button {\n  border-radius: 0 var(--radius) var(--radius) 0;\n  height: 48px;\n  width: 48px;\n  border-left: none;\n  background: transparent !important;\n  box-shadow: none;\n}\n.ui-input-list-item .ui-icon-button + .ui-icon-button {\n  margin-left: 0;\n}";
const script$U = {
  name: "uiInputList",
  props: {
    addLabel: {
      type: String,
      default: "@ui.add"
    },
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    maxItems: {
      type: Number,
      default: 100
    },
    maxLength: {
      type: Number,
      default: 200
    },
    plusButton: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    items: []
  }),
  watch: {
    value(value) {
      this.items = map(value, (item2) => {
        return {value: item2};
      });
    }
  },
  mounted() {
    this.items = map(this.value, (item2) => {
      return {value: item2};
    });
  },
  methods: {
    onChange() {
      this.$emit("input", map(this.items, (item2) => item2.value));
    },
    addItem() {
      this.items.push({
        value: null
      });
      this.onChange();
      this.$nextTick(() => {
        this.$el.querySelector(".ui-input-list-item:last-of-type input").focus();
      });
    },
    removeItem(item2) {
      const index = this.items.indexOf(item2);
      this.items.splice(index, 1);
      this.onChange();
    }
  }
};
const __cssModules$U = {};
var component$U = normalizeComponent(script$U, render$U, staticRenderFns$U, false, injectStyles$U, null, null, null);
function injectStyles$U(context) {
  for (let o in __cssModules$U) {
    this[o] = __cssModules$U[o];
  }
}
component$U.options.__file = "app/components/forms/input-list.vue";
var uiInputList = component$U.exports;
var render$T = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-property", class: {"is-vertical": _vm.vertical, "is-text": _vm.isText, "hide-label": _vm.hideLabel, "is-disabled": _vm.disabled}}, [_vm.label && !_vm.hideLabel ? _c("label", {staticClass: "ui-property-label", attrs: {for: _vm.field}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.label, expression: "label"}]}), _vm.required ? _c("strong", {staticClass: "ui-property-required"}, [_vm._v("*")]) : _vm._e(), _vm._t("label-after"), _vm.description ? _c("small", {directives: [{name: "localize", rawName: "v-localize", value: _vm.description, expression: "description"}]}) : _vm._e()], 2) : _vm._e(), _c("div", {staticClass: "ui-property-content"}, [_vm._t("default"), _vm.field ? _c("ui-error", {attrs: {field: _vm.field}}) : _vm._e()], 2), _vm._t("after")], 2);
};
var staticRenderFns$T = [];
var property_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.ui-property {\n  position: relative;\n  display: grid;\n  grid-gap: 40px;\n  grid-template-columns: minmax(auto, 1fr) auto;\n  margin: 0 -32px 0;\n  padding: 0 32px 0;\n}\n.ui-property.is-disabled .ui-property-content {\n  pointer-events: none;\n}\n.ui-property.is-disabled {\n  cursor: not-allowed;\n}\n.ui-property + .ui-split,\n.ui-split + .ui-property,\n.ui-property + .ui-property {\n  padding-top: 30px;\n  margin-top: 30px;\n  border-top: 1px dashed var(--color-line-dashed);\n}\n.is-narrow .ui-property + .ui-split,\n.is-narrow .ui-split + .ui-property,\n.is-narrow .ui-property + .ui-property {\n  margin-top: 30px;\n  padding-top: 0;\n  border-top: none;\n}\n.ui-property.is-vertical {\n  grid-template-columns: minmax(auto, 1fr);\n  grid-gap: 12px;\n  flex-direction: column;\n}\n.ui-property.is-vertical > .ui-property-label {\n  width: 100%;\n  padding-right: 0;\n}\n.ui-property.is-vertical > .ui-property-label + .ui-property-content {\n  margin-top: 0;\n}\n.ui-property.is-vertical > .ui-property-content {\n  display: block;\n}\n.ui-property.is-text {\n  grid-gap: 2px;\n}\n.ui-property.full-width > .ui-property-content,\n.ui-property.hide-label > .ui-property-content,\n.ui-property.is-static > .ui-property-content {\n  max-width: 100%;\n}\n.ui-property.hide-label > .ui-property-label,\n.ui-property.is-static > .ui-property-label {\n  display: none;\n}\n.ui-property.is-static,\n.ui-property.is-static + .ui-property {\n  margin-top: 0 !important;\n}\n.ui-property-label {\n  display: block;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n  font-weight: 700;\n  flex-basis: 100%;\n  /*.ui-property:focus-within &\n  {\n    color: var(--color-accent-info);\n  }*/\n}\n.ui-property-label small {\n  display: block;\n  padding-top: 2px;\n  font-size: var(--font-size-xs);\n  font-weight: 400;\n  line-height: 1.3;\n  text-decoration: none;\n  color: var(--color-text-dim);\n}\n.ui-property-label small:empty {\n  display: none;\n}\n.ui-property-required {\n  color: var(--color-required-marker);\n  margin-left: 0.2em;\n  font-weight: 400;\n}\n.ui-property-content {\n  flex: 1;\n  max-width: 932px;\n  font-size: var(--font-size);\n  display: flex;\n  align-items: center;\n}\n.ui-property-help {\n  max-width: 932px;\n  font-size: var(--font-size-xs);\n  color: var(--color-text-dim);\n  margin: 15px 0 0;\n  letter-spacing: 0.3px;\n}\n.ui-property-help:before {\n  content: "\uE87F";\n  font-family: var(--font-icon);\n  color: var(--color-primary);\n  font-size: var(--font-size-l);\n  float: left;\n  position: relative;\n  top: -1px;\n  margin-right: 6px;\n}\n.ui-properties-floating .ui-property {\n  display: inline-flex;\n  min-height: 52px;\n}\n.ui-properties-floating .ui-property + .ui-property {\n  padding-top: 0;\n  margin-top: 0;\n  border-left: 1px solid var(--color-line);\n  padding-left: 50px;\n  margin-left: 50px;\n}\n.ui-property.ui-property-parent {\n  display: grid;\n  grid-template-columns: repeat(12, minmax(0, 1fr));\n  grid-gap: var(--padding) var(--padding-s);\n}\n.ui-property.ui-property-parent > .ui-property {\n  grid-column-start: 1;\n  grid-column-end: 13;\n  margin-left: 0;\n  margin-right: 0;\n  padding-left: 0;\n  padding-right: 0;\n}\n.ui-property.ui-property-parent:empty:first-child + .ui-property,\n.ui-property.ui-property-parent > .ui-property[data-cols] + .ui-property[data-cols] {\n  margin-top: 0;\n  border-top: none;\n  padding-top: 0;\n}\n.ui-property.ui-property-parent:empty {\n  display: none;\n}';
const script$T = {
  name: "uiProperty",
  props: {
    field: String,
    label: String,
    hideLabel: Boolean,
    description: String,
    required: Boolean,
    vertical: {
      type: Boolean,
      default: true
    },
    isText: Boolean,
    disabled: {
      type: Boolean,
      default: false
    }
  }
};
const __cssModules$T = {};
var component$T = normalizeComponent(script$T, render$T, staticRenderFns$T, false, injectStyles$T, null, null, null);
function injectStyles$T(context) {
  for (let o in __cssModules$T) {
    this[o] = __cssModules$T[o];
  }
}
component$T.options.__file = "app/components/forms/property.vue";
var uiProperty = component$T.exports;
class Menu {
  constructor({options: options2}) {
    this.options = options2;
    this.preventHide = false;
    this.mousedownHandler = this.handleClick.bind(this);
    this.options.element.addEventListener("mousedown", this.mousedownHandler, {capture: true});
    this.blurHandler = () => {
      if (this.preventHide) {
        this.preventHide = false;
        return;
      }
      this.options.editor.emit("menubar:focusUpdate", false);
    };
    this.options.editor.on("blur", this.blurHandler);
  }
  handleClick() {
    this.preventHide = true;
  }
  destroy() {
    this.options.element.removeEventListener("mousedown", this.mousedownHandler);
    this.options.editor.off("blur", this.blurHandler);
  }
}
const MenuBar = function(options2) {
  return new Plugin$1({
    key: new PluginKey("menu_bar"),
    view(editorView) {
      return new Menu({editorView, options: options2});
    }
  });
};
var EditorMenuBar = {
  props: {
    editor: {
      default: null,
      type: Object
    }
  },
  data() {
    return {
      focused: false
    };
  },
  watch: {
    editor: {
      immediate: true,
      handler(editor2) {
        if (editor2) {
          this.$nextTick(() => {
            var menubar = MenuBar({
              editor: editor2,
              element: this.$el
            });
            editor2.registerPlugin(menubar);
            this.focused = editor2.focused;
            editor2.on("focus", () => {
              this.focused = true;
            });
            editor2.on("menubar:focusUpdate", (focused) => {
              this.focused = focused;
            });
          });
        }
      }
    }
  },
  render() {
    if (!this.editor) {
      return null;
    }
    return this.$scopedSlots.default({
      focused: this.focused,
      focus: this.editor.focus,
      commands: this.editor.commands,
      isActive: this.editor.isActive,
      getMarkAttrs: this.editor.getMarkAttrs.bind(this.editor),
      getNodeAttrs: this.editor.getNodeAttrs.bind(this.editor)
    });
  }
};
class MaxSize extends Extension {
  get name() {
    return "maxSize";
  }
  get defaultOptions() {
    return {
      maxSize: null
    };
  }
  get plugins() {
    return [
      new Plugin$1({
        appendTransaction: (transactions, oldState, newState) => {
          const max2 = this.options.maxSize;
          const oldLength = oldState.doc.content.size;
          const newLength = newState.doc.content.size;
          if (max2 && newLength > max2 && newLength > oldLength) {
            let newTr = newState.tr;
            newTr.insertText("", max2 + 1, newLength);
            return newTr;
          }
        }
      })
    ];
  }
}
function createConfig() {
  return {
    removeExtension(name) {
      var ext = this.extensions.find((x) => x.name === name);
      ext && this.extensions.splice(this.extensions.indexOf(ext), 1);
    },
    removeCommand(alias2) {
      var cmd = this.commands.find((x) => x.alias === alias2);
      cmd && this.commands.splice(this.commands.indexOf(cmd), 1);
    },
    extensions: [
      new History(),
      new HardBreak(),
      new Link$1({
        target: "_blank"
      }),
      new Bold(),
      new Code(),
      new Italic(),
      new Strike(),
      new Underline(),
      new ListItem(),
      new BulletList(),
      new OrderedList(),
      new HorizontalRule(),
      new Heading({
        levels: [2, 3, 4]
      })
    ],
    commands: [
      {
        alias: "bold",
        title: "@rte.bold",
        symbol: "fth-bold",
        symbolSize: 14,
        isActive: (active) => active.bold(),
        onClick: (ev, cmd) => cmd.bold(ev),
        bubble: true
      },
      {
        alias: "italic",
        title: "@rte.italic",
        symbol: "fth-italic",
        symbolSize: 14,
        isActive: (active) => active.italic(),
        onClick: (ev, cmd) => cmd.italic(ev),
        bubble: true
      },
      {
        alias: "underline",
        title: "@rte.underline",
        symbol: "fth-underline",
        symbolSize: 14,
        isActive: (active) => active.underline(),
        onClick: (ev, cmd) => cmd.underline(ev),
        bubble: true
      },
      {
        alias: "code",
        title: "@rte.code",
        symbol: "fth-code",
        symbolSize: 14,
        isActive: (active) => active.code(),
        onClick: (ev, cmd) => cmd.code(ev),
        bubble: true
      },
      {
        alias: "line",
        title: "@rte.line",
        symbol: "fth-minus",
        symbolSize: 15,
        onClick: (ev, cmd) => cmd.horizontal_rule(ev)
      },
      {
        alias: "list",
        title: "@rte.list",
        symbol: "fth-list",
        symbolSize: 14,
        children: [
          {
            alias: "ulist",
            title: "@rte.ulist",
            isActive: (active) => active.bullet_list(),
            onClick: (ev, cmd) => cmd.bullet_list(ev)
          },
          {
            alias: "olist",
            title: "@rte.olist",
            isActive: (active) => active.ordered_list(),
            onClick: (ev, cmd) => cmd.ordered_list(ev)
          }
        ]
      },
      {
        alias: "heading",
        title: "@rte.heading",
        symbol: "fth-type",
        symbolSize: 14,
        children: [
          {
            alias: "h2",
            title: "@rte.heading2",
            isActive: (active) => active.heading({level: 2}),
            onClick: (ev, cmd) => cmd.heading({level: 2})
          },
          {
            alias: "h3",
            title: "@rte.heading3",
            isActive: (active) => active.heading({level: 3}),
            onClick: (ev, cmd) => cmd.heading({level: 3})
          },
          {
            alias: "h4",
            title: "@rte.heading4",
            isActive: (active) => active.heading({level: 4}),
            onClick: (ev, cmd) => cmd.heading({level: 4})
          }
        ]
      }
    ]
  };
}
var render$S = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-rte", attrs: {id: _vm.id, disabled: _vm.disabled}}, [_vm.hasBubble ? _c("editor-menu-bubble", {attrs: {editor: _vm.editor, "keep-in-bounds": true}, scopedSlots: _vm._u([{key: "default", fn: function(ref) {
    var commands = ref.commands;
    var isActive = ref.isActive;
    var menu = ref.menu;
    return [_c("div", {staticClass: "ui-rte-overlay-controls theme-dark", class: {"is-active": menu.isActive}, style: "left: " + menu.left + "px; bottom: " + menu.bottom + "px;"}, _vm._l(_vm.cmds, function(cmd) {
      return cmd.bubble && !cmd.isParent ? _c("div", {key: cmd.id, staticClass: "ui-rte-overlay-control-outer"}, [_c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: cmd.title, expression: "cmd.title", arg: "title"}], staticClass: "ui-rte-overlay-control", class: {"is-active": cmd.isActive(isActive)}, attrs: {type: "button", "data-alias": cmd.alias}, on: {click: function($event) {
        return cmd.onClick($event, commands);
      }}}, [_c("ui-icon", {attrs: {symbol: cmd.symbol, size: cmd.symbolSize}})], 1)]) : _vm._e();
    }), 0)];
  }}], null, false, 661682183)}) : _vm._e(), _vm.cmds.length > 0 && _vm.editor ? _c("editor-menu-bar", {attrs: {editor: _vm.editor}, scopedSlots: _vm._u([{key: "default", fn: function(ref) {
    var commands = ref.commands;
    var isActive = ref.isActive;
    return [_c("div", {staticClass: "ui-rte-controls"}, _vm._l(_vm.cmds, function(cmd) {
      return _c("div", {key: cmd.id, staticClass: "ui-rte-control-outer"}, [!cmd.isParent ? _c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: cmd.title, expression: "cmd.title", arg: "title"}], staticClass: "ui-rte-control", class: {"is-active": cmd.isActive(isActive)}, attrs: {type: "button", "data-alias": cmd.alias}, on: {click: function($event) {
        return cmd.onClick($event, commands);
      }}}, [_c("ui-icon", {attrs: {symbol: cmd.symbol, size: cmd.symbolSize}})], 1) : _vm._e(), cmd.isParent ? _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
        return [_c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: cmd.title, expression: "cmd.title", arg: "title"}], staticClass: "ui-rte-control", class: {"is-active": cmd.isActive(isActive)}, attrs: {"data-alias": cmd.alias, type: "button"}}, [_c("ui-icon", {attrs: {symbol: cmd.symbol, size: cmd.symbolSize}})], 1)];
      }, proxy: true}], null, true)}, _vm._l(cmd.children, function(child) {
        return _c("ui-dropdown-button", {key: child.id, attrs: {label: child.title}, on: {click: function($event) {
          return child.onClick($event, commands);
        }}});
      }), 1) : _vm._e()], 1);
    }), 0)];
  }}], null, false, 931166325)}) : _vm._e(), _c("editor-content", {staticClass: "ui-rte-input", attrs: {editor: _vm.editor}})], 1);
};
var staticRenderFns$S = [];
var rte_vue_vue_type_style_index_0_lang = '.ui-rte {\n  background: var(--color-input);\n  border-radius: var(--radius);\n  border: var(--color-input-border);\n}\n.ui-rte:focus-within {\n  background-color: var(--color-input-focus-bg);\n  border: var(--color-input-focus-border);\n  box-shadow: var(--color-input-focus-shadow);\n  outline: none;\n}\n.ui-rte-input {\n  height: auto;\n  min-height: 48px;\n  padding-top: 9px;\n  max-height: 420px;\n  overflow-y: auto;\n  border: none !important;\n}\n.ui-rte-input .ProseMirror:focus {\n  outline: none;\n}\n.ui-rte-input p {\n  margin: 0;\n}\n.ui-rte-input p + p {\n  margin-top: 1em;\n}\n.ui-rte-input p.is-editor-empty:first-child:before {\n  content: attr(data-empty-text);\n  float: left;\n  color: var(--color-input-placeholder);\n  opacity: 0.7;\n  pointer-events: none;\n  height: 0;\n}\n.ui-rte-input a {\n  color: var(--color-primary);\n  text-decoration: underline;\n  cursor: pointer;\n}\n.ui-rte-overlay-controls {\n  position: absolute;\n  display: flex;\n  z-index: 20;\n  background: var(--color-bg);\n  border-radius: var(--radius);\n  padding: 5px;\n  margin-bottom: 8px;\n  transform: translateX(-50%);\n  visibility: hidden;\n  opacity: 0;\n  transition: opacity 0.2s, visibility 0.2s;\n  pointer-events: none;\n}\n.ui-rte-overlay-controls:after {\n  content: "";\n  position: absolute;\n  left: 50%;\n  top: 100%;\n  width: 0;\n  height: 0;\n  border: 8px solid transparent;\n  margin-left: -7px;\n  border-top-color: var(--color-bg);\n}\n.ui-rte-overlay-controls.is-active {\n  opacity: 1;\n  visibility: visible;\n  pointer-events: auto;\n}\n.ui-rte-overlay-control {\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  background: transparent;\n  border: 0;\n  color: var(--color-text);\n  height: 32px;\n  width: 32px;\n  border-radius: var(--radius);\n  cursor: pointer;\n}\n.ui-rte-overlay-control + .ui-rte-overlay-control {\n  margin-left: 4px;\n}\n.ui-rte-overlay-control:hover {\n  background-color: var(--color-bg-shade-1);\n}\n.ui-rte-overlay-control.is-active {\n  background-color: var(--color-bg-shade-1);\n  color: var(--color-text-dim);\n}\n.ui-rte-controls {\n  background: none;\n  padding: 5px 5px 0;\n  display: flex;\n  align-items: center;\n  justify-content: flex-start;\n  border-bottom: var(--color-input-border);\n}\n.ui-rte-controls-label {\n  color: var(--color-text-dim);\n  margin: 0 14px 0 8px;\n  font-size: 10px;\n  text-transform: uppercase;\n  pointer-events: none;\n  user-select: none;\n}\n.ui-rte-control {\n  background-color: transparent;\n  cursor: pointer;\n  height: 30px;\n  width: 30px;\n  text-align: center;\n  border-radius: 5px;\n  font-size: 14px;\n  vertical-align: bottom;\n}\n.ui-rte-control[disabled] {\n  opacity: 0.5;\n}\n.ui-rte-control-outer + .ui-rte-control-outer {\n  margin-left: 5px;\n}\n.ui-rte-control:hover, .ui-rte-control.is-active {\n  background: var(--color-box-nested);\n  color: var(--color-text);\n}\n.ui-rte-input {\n  background: none !important;\n  border-radius: 0;\n}\n.ui-rte-input .ProseMirror > * {\n  margin: 1rem 0;\n}\n.ui-rte-input .ProseMirror > :first-child {\n  margin-top: 0;\n}\n.ui-rte-input .ProseMirror > :last-child {\n  margin-bottom: 0;\n}\n.ui-rte-input .ProseMirror h1, .ui-rte-input .ProseMirror h2, .ui-rte-input .ProseMirror h3, .ui-rte-input .ProseMirror h4, .ui-rte-input .ProseMirror h5, .ui-rte-input .ProseMirror h6 {\n  margin-bottom: 0.5rem;\n  font-weight: bold;\n}\n.ui-rte-input .ProseMirror h1 {\n  font-size: 1.4em;\n}\n.ui-rte-input .ProseMirror h2 {\n  font-size: 1.2em;\n}\n.ui-rte-input .ProseMirror h3 {\n  font-size: 1.1em;\n}\n.ui-rte-input .ProseMirror h4, .ui-rte-input .ProseMirror h5, .ui-rte-input .ProseMirror h6 {\n  font-size: 1em;\n}\n.ui-rte-input .ProseMirror hr {\n  display: block;\n  border-bottom-style: dashed;\n  border-bottom-color: var(--color-line-dashed);\n}\n.ui-rte-input .ProseMirror li p {\n  display: block;\n  line-height: 1.5;\n}';
const script$S = {
  name: "uiRte",
  components: {EditorContent, EditorMenuBubble, EditorMenuBar},
  props: {
    value: {
      type: String,
      default: ""
    },
    disabled: {
      type: Boolean,
      default: false
    },
    maxLength: {
      type: Number,
      default: null
    },
    placeholder: {
      type: String,
      default: null
    },
    setup: {
      type: Function,
      default: () => {
      }
    }
  },
  data: () => ({
    id: "rte-" + Strings.guid(),
    blocked: false,
    onDebouncedChange: null,
    editor: null,
    extensions: [],
    cmds: []
  }),
  watch: {
    value() {
      if (!this.blocked) {
        this.init();
      }
    },
    disabled(value) {
      this.editor.setOptions({
        editable: !value
      });
    }
  },
  computed: {
    hasBubble() {
      return this.cmds.find((x) => x.bubble) != null;
    }
  },
  created() {
    this.onDebouncedChange = debounce(this.onChange, 350);
  },
  mounted() {
    var config2 = createConfig();
    if (typeof this.setup === "function") {
      this.setup(config2);
    }
    this.extensions = config2.extensions;
    this.cmds = config2.commands.map((cmd) => {
      let params = this.mapCommand(cmd);
      if (cmd.children && cmd.children.length) {
        params.isParent = true;
        params.children = cmd.children.map((child) => this.mapCommand(child));
      }
      return params;
    });
    if (this.placeholder) {
      this.extensions.push(new Placeholder({
        emptyEditorClass: "is-editor-empty",
        emptyNodeClass: "is-empty",
        emptyNodeText: this.placeholder,
        showOnlyWhenEditable: true,
        showOnlyCurrent: true
      }));
    }
    if (this.maxLength > 0) {
      this.extensions.push(new MaxSize({maxSize: this.maxLength}));
    }
    this.editor = new Editor$1({
      editable: !this.disabled,
      extensions: this.extensions,
      onUpdate: (opts) => this.onDebouncedChange(opts.getHTML())
    });
    this.init();
  },
  methods: {
    init() {
      this.editor.setContent(this.value);
    },
    onChange(content2) {
      this.blocked = true;
      this.$emit("input", content2);
      this.$nextTick(() => this.blocked = false);
    },
    mapCommand(cmd) {
      return {
        id: Strings.guid(),
        alias: cmd.alias,
        title: cmd.title,
        symbol: cmd.symbol,
        symbolSize: cmd.symbolSize || 17,
        isActive: typeof cmd.isActive === "function" ? cmd.isActive : () => false,
        disabled: typeof cmd.disabled === "function" ? cmd.disabled : () => false,
        onClick: typeof cmd.onClick === "function" ? cmd.onClick : () => {
        },
        bubble: cmd.bubble || false
      };
    }
  },
  beforeDestroy() {
    this.editor.destroy();
  }
};
const __cssModules$S = {};
var component$S = normalizeComponent(script$S, render$S, staticRenderFns$S, false, injectStyles$S, null, null, null);
function injectStyles$S(context) {
  for (let o in __cssModules$S) {
    this[o] = __cssModules$S[o];
  }
}
component$S.options.__file = "app/components/forms/rte.vue";
var uiRte = component$S.exports;
var render$R = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-searchinput"}, [_c("input", {directives: [{name: "localize", rawName: "v-localize:placeholder", value: _vm.placeholder, expression: "placeholder", arg: "placeholder"}], ref: "input", staticClass: "ui-input", attrs: {type: "search"}, domProps: {value: _vm.value}, on: {input: _vm.onChange, keyup: function($event) {
    if (!$event.type.indexOf("key") && _vm._k($event.keyCode, "enter", 13, $event.key, "Enter")) {
      return null;
    }
    return _vm.onSubmit($event);
  }}}), _vm._t("button", [_c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.search.button", expression: "'@ui.search.button'", arg: "title"}], staticClass: "ui-searchinput-button", attrs: {type: "button"}, on: {click: _vm.onSubmit}}, [_c("ui-icon", {attrs: {symbol: "fth-search"}})], 1)], null, {onSubmit: _vm.onSubmit})], 2);
};
var staticRenderFns$R = [];
var search_vue_vue_type_style_index_0_lang = ".ui-searchinput {\n  position: relative;\n}\n.ui-searchinput .ui-input {\n  display: block;\n  min-width: 320px;\n  padding-right: 40px;\n  border: var(--color-input-border);\n  background: var(--color-input);\n}\n.ui-searchinput.onbg .ui-input:not(:focus) {\n  /*box-shadow: var(--shadow-short);\n  background: var(--color-button-light-onbg);*/\n  background: transparent;\n  border: 1px dashed var(--color-line-dashed-onbg);\n}\n.ui-searchinput .ui-searchinput-button {\n  position: absolute;\n  right: 0;\n  top: 0;\n  height: 100%;\n  width: 45px;\n  text-align: center;\n  font-size: var(--font-size);\n  padding-top: 1px;\n  border: 1px solid transparent;\n  border-radius: var(--radius);\n}\n.ui-searchinput .ui-input:focus {\n  background-color: var(--color-input-focus-bg);\n  border: var(--color-input-focus-border);\n  box-shadow: var(--color-input-focus-shadow);\n  outline: none;\n}";
const script$R = {
  name: "uiSearch",
  props: {
    value: {
      type: String,
      default: ""
    },
    placeholder: {
      type: String,
      default: "@ui.search.placeholder"
    }
  },
  computed: {},
  methods: {
    onChange(ev) {
      this.$emit("change", ev.target.value);
      this.$emit("input", ev.target.value);
    },
    onSubmit(ev) {
      this.$emit("submit", this.$refs.input.value);
    },
    focus() {
      this.$refs.input.focus();
    }
  }
};
const __cssModules$R = {};
var component$R = normalizeComponent(script$R, render$R, staticRenderFns$R, false, injectStyles$R, null, null, null);
function injectStyles$R(context) {
  for (let o in __cssModules$R) {
    this[o] = __cssModules$R[o];
  }
}
component$R.options.__file = "app/components/forms/search.vue";
var uiSearch = component$R.exports;
var render$Q = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-tags", class: {"is-disabled": _vm.disabled}}, [_c("ui-input-list", {attrs: {value: _vm.value, "add-label": _vm.addLabel, disabled: _vm.disabled, "max-items": _vm.maxItems, "max-length": _vm.maxLength, "plus-button": true}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}})], 1);
};
var staticRenderFns$Q = [];
var tags_vue_vue_type_style_index_0_lang = ".ui-tags .ui-input-list {\n  display: flex;\n  align-items: flex-start;\n  flex-wrap: wrap;\n}\n.ui-tags .ui-input-list-item {\n  display: inline-grid;\n  min-width: 0;\n}\n.ui-tags .ui-input-list-item .ui-input {\n  min-width: 120px;\n  width: auto;\n}\n.ui-tags .ui-input-list-item + .ui-input-list-item, .ui-tags .ui-input-list-item + .ui-button, .ui-tags .ui-input-list-item + .ui-select-button {\n  margin-top: 0;\n  margin-left: 6px;\n  margin-bottom: 8px;\n}";
const script$Q = {
  name: "uiTags",
  props: {
    addLabel: {
      type: String,
      default: "@ui.add"
    },
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    maxItems: {
      type: Number,
      default: 100
    },
    maxLength: {
      type: Number,
      default: 200
    }
  }
};
const __cssModules$Q = {};
var component$Q = normalizeComponent(script$Q, render$Q, staticRenderFns$Q, false, injectStyles$Q, null, null, null);
function injectStyles$Q(context) {
  for (let o in __cssModules$Q) {
    this[o] = __cssModules$Q[o];
  }
}
component$Q.options.__file = "app/components/forms/tags.vue";
var uiTags = component$Q.exports;
var render$P = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-toggle", class: {"is-disabled": _vm.disabled, "is-negative": _vm.negative, "is-active": _vm.value, "is-content-left": _vm.contentLeft}}, [_c("input", {attrs: {type: "checkbox", disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: _vm.onChange}}), _c("span", {staticClass: "ui-toggle-switch", class: {"is-active": _vm.value}}, [_c("i")]), _vm.offContent && !_vm.value && _vm.offWarning ? _c("i", {staticClass: "fth-minus-circle ui-toggle-off-warning"}) : _vm._e(), _vm.onContent && _vm.value ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.onContent, expression: "onContent"}], staticClass: "ui-toggle-text"}) : _vm._e(), _vm.offContent && !_vm.value ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.offContent, expression: "offContent"}], staticClass: "ui-toggle-text"}) : _vm._e()]);
};
var staticRenderFns$P = [];
var toggle_vue_vue_type_style_index_0_lang = ".ui-toggle {\n  display: inline-flex;\n  align-items: center;\n  position: relative;\n  height: 22px;\n}\n.ui-toggle input {\n  position: absolute;\n  top: 0;\n  left: 0;\n  right: 0;\n  bottom: 0;\n  width: 100%;\n  height: 100%;\n  margin: 0;\n  z-index: 2;\n  opacity: 0;\n  cursor: pointer;\n}\n.ui-toggle.is-disabled input {\n  cursor: default;\n}\n.ui-toggle.is-content-left {\n  flex-direction: row-reverse;\n}\n.ui-toggle.is-content-left .ui-toggle-text {\n  margin-left: 0;\n  margin-right: 12px;\n}\n.ui-toggle-text {\n  margin-top: 1px;\n  margin-left: 12px;\n}\n.ui-toggle.is-active .ui-toggle-text {\n  font-weight: 600;\n}\n.ui-toggle-switch {\n  display: inline-block;\n  height: 22px;\n  width: 36px;\n  background: var(--color-toggle);\n  border-radius: 20px;\n  border: 1px solid transparent;\n  transition: all 0.2s ease;\n  z-index: 1;\n  pointer-events: none;\n}\n.ui-toggle-switch i {\n  display: inline-block;\n  height: 16px;\n  width: 16px;\n  border-radius: 20px;\n  margin: 2px;\n  background: var(--color-toggle-fg);\n  transition: all 0.2s ease;\n}\n.ui-toggle-switch.is-active {\n  background: var(--color-toggled);\n}\n.ui-toggle-switch.is-active i {\n  background: var(--color-toggled-fg);\n  transform: translateX(14px);\n}\ninput:focus + .ui-toggle-switch {\n  border: var(--color-input-focus-border);\n  box-shadow: var(--color-input-focus-shadow);\n  outline: none;\n}\ninput:focus + .ui-toggle-switch.is-active {\n  background-color: var(--color-toggled);\n}\n.ui-toggle.onbg .ui-toggle-switch:not(.is-active) {\n  background: var(--color-bg);\n}\n.ui-toggle.is-negative .ui-toggle-switch.is-active {\n  background: var(--color-negative);\n  border-color: transparent !important;\n}\n.ui-toggle.is-negative .ui-toggle-switch.is-active i {\n  background: white;\n}\n.ui-toggle.is-accent .ui-toggle-switch.is-active {\n  background: var(--color-accent);\n  border-color: transparent !important;\n}\n.ui-toggle.is-accent .ui-toggle-switch.is-active i {\n  background: var(--color-accent-fg);\n}\n.ui-toggle-off-warning {\n  margin: 0 10px 0 -5px;\n}";
const script$P = {
  name: "uiToggle",
  props: {
    value: {
      type: Boolean,
      default: false
    },
    disabled: {
      type: Boolean,
      default: false
    },
    negative: {
      type: Boolean,
      default: false
    },
    onContent: {
      type: String,
      default: null
    },
    offContent: {
      type: String,
      default: null
    },
    offWarning: {
      type: Boolean,
      default: false
    },
    contentLeft: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    onChange(ev) {
      this.$emit("input", !this.value);
    }
  }
};
const __cssModules$P = {};
var component$P = normalizeComponent(script$P, render$P, staticRenderFns$P, false, injectStyles$P, null, null, null);
function injectStyles$P(context) {
  for (let o in __cssModules$P) {
    this[o] = __cssModules$P[o];
  }
}
component$P.options.__file = "app/components/forms/toggle.vue";
var uiToggle = component$P.exports;
var render$O = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-native-select", attrs: {disabled: _vm.disabled}}, [_c("select", {attrs: {disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: _vm.onChange}}, [_vm.emptyOption ? _c("option") : _vm._e(), _vm._l(_vm.options, function(option) {
    return _c("option", {directives: [{name: "localize", rawName: "v-localize", value: option.value, expression: "option.value"}], domProps: {value: option.key}});
  })], 2)]);
};
var staticRenderFns$O = [];
const script$O = {
  name: "uiSelect",
  props: {
    value: [String, Number, Object],
    items: [Array, Function],
    entity: Object,
    disabled: Boolean,
    emptyOption: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    options: []
  }),
  created() {
    this.rebuild();
  },
  watch: {
    items: {
      deep: true,
      handler() {
        this.rebuild();
        this.onChange({target: {value: this.value}});
      }
    },
    entity: {
      deep: true,
      handler() {
        this.rebuild();
        this.onChange({target: {value: this.value}});
      }
    }
  },
  methods: {
    rebuild() {
      if (this.entity && typeof this.items === "function") {
        this.options = this.items(this.entity);
      } else if (typeof this.items !== "function" && this.items) {
        this.options = [...this.items];
      } else {
        this.options = [];
      }
    },
    onChange(ev) {
      this.$emit("input", ev.target.value ? ev.target.value : null);
    }
  }
};
const __cssModules$O = {};
var component$O = normalizeComponent(script$O, render$O, staticRenderFns$O, false, injectStyles$O, null, null, null);
function injectStyles$O(context) {
  for (let o in __cssModules$O) {
    this[o] = __cssModules$O[o];
  }
}
component$O.options.__file = "app/components/forms/select.vue";
var uiSelect = component$O.exports;
var render$N = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("figure", {staticClass: "ui-module-preview-figure", class: {"has-image": _vm.imageSource != null}}, [_vm.imageSource ? _c("img", {staticClass: "-image", attrs: {src: _vm.imageSource}}) : _vm._e(), _c("figcaption", [_c("p", [_vm.text ? _c("span", {staticClass: "-text", domProps: {innerHTML: _vm._s(_vm.textContent)}}) : _vm._e(), _vm.subline ? _c("span", {staticClass: "-subline", domProps: {innerHTML: _vm._s(_vm.sublineContent)}}) : _vm._e()])])]);
};
var staticRenderFns$N = [];
var modulePreviewFigure_vue_vue_type_style_index_0_lang = ".ui-module-preview-figure {\n  font-size: var(--font-size);\n  line-height: 1.5;\n  margin: 0;\n  padding: 0;\n}\n.ui-module-preview-figure.has-image {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 20px;\n  align-items: center;\n}\n.ui-module-preview-figure .-subline {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-s);\n}\n.ui-module-preview-figure .-text, .ui-module-preview-figure .-subline {\n  overflow: hidden;\n  -webkit-box-orient: vertical;\n  -webkit-line-clamp: 2;\n  display: -webkit-box;\n}\n.ui-module-preview-figure .-image {\n  border-radius: var(--radius);\n  max-width: 128px;\n  max-height: 64px;\n}";
const script$N = {
  name: "uiModulePreviewFigure",
  props: {
    text: {
      type: String
    },
    subline: {
      type: String
    },
    image: {
      type: String
    },
    html: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    imageSource: null
  }),
  computed: {
    textContent() {
      return this.html ? this.text : Strings.htmlToText(this.text);
    },
    sublineContent() {
      return this.html ? this.subline : Strings.htmlToText(this.subline);
    }
  },
  watch: {
    image(val) {
      this.loadImageSource();
    }
  },
  mounted() {
    this.loadImageSource();
  },
  methods: {
    loadImageSource() {
      if (!this.image) {
        this.imageSource = null;
        return;
      }
      this.imageSource = MediaApi.getImageSource(this.image);
    }
  }
};
const __cssModules$N = {};
var component$N = normalizeComponent(script$N, render$N, staticRenderFns$N, false, injectStyles$N, null, null, null);
function injectStyles$N(context) {
  for (let o in __cssModules$N) {
    this[o] = __cssModules$N[o];
  }
}
component$N.options.__file = "app/components/modules/predefined/module-preview-figure.vue";
var uiModulePreviewFigure = component$N.exports;
var render$M = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-module-preview-headline"}, [_vm.text ? _c("article", {staticClass: "-text", domProps: {innerHTML: _vm._s(_vm.text)}}) : _vm._e(), _vm.subline ? _c("article", {staticClass: "-subline", domProps: {innerHTML: _vm._s(_vm.subline)}}) : _vm._e()]);
};
var staticRenderFns$M = [];
var modulePreviewHeadline_vue_vue_type_style_index_0_lang = ".ui-module-preview-headline {\n  font-size: var(--font-size);\n  line-height: 1.5;\n}\n.ui-module-preview-headline .-text {\n  font-size: var(--font-size-xl);\n  font-weight: 600;\n}\n.ui-module-preview-headline .-subline {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-s);\n}";
const script$M = {
  name: "uiModulePreviewHeadline",
  props: {
    text: {
      type: String
    },
    subline: {
      type: String
    }
  }
};
const __cssModules$M = {};
var component$M = normalizeComponent(script$M, render$M, staticRenderFns$M, false, injectStyles$M, null, null, null);
function injectStyles$M(context) {
  for (let o in __cssModules$M) {
    this[o] = __cssModules$M[o];
  }
}
component$M.options.__file = "app/components/modules/predefined/module-preview-headline.vue";
var uiModulePreviewHeadline = component$M.exports;
var render$L = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-module-preview-text"}, [_vm.text ? _c("p", {staticClass: "-text", domProps: {innerHTML: _vm._s(_vm.textContent)}}) : _vm._e(), _vm.subline ? _c("p", {staticClass: "-subline", domProps: {innerHTML: _vm._s(_vm.sublineContent)}}) : _vm._e()]);
};
var staticRenderFns$L = [];
var modulePreviewText_vue_vue_type_style_index_0_lang = ".ui-module-preview-text {\n  font-size: var(--font-size);\n  line-height: 1.5;\n}\n.ui-module-preview-text p.-subline {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-s);\n  margin-top: 0.2em;\n}\n.ui-module-preview-text .-text, .ui-module-preview-text .-subline {\n  overflow: hidden;\n  -webkit-box-orient: vertical;\n  -webkit-line-clamp: 3;\n  display: -webkit-box;\n}";
const script$L = {
  name: "uiModulePreviewText",
  props: {
    text: {
      type: String
    },
    subline: {
      type: String
    },
    html: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    textContent() {
      return this.html ? this.text : Strings.htmlToText(this.text);
    },
    sublineContent() {
      return this.html ? this.subline : Strings.htmlToText(this.subline);
    }
  }
};
const __cssModules$L = {};
var component$L = normalizeComponent(script$L, render$L, staticRenderFns$L, false, injectStyles$L, null, null, null);
function injectStyles$L(context) {
  for (let o in __cssModules$L) {
    this[o] = __cssModules$L[o];
  }
}
component$L.options.__file = "app/components/modules/predefined/module-preview-text.vue";
var uiModulePreviewText = component$L.exports;
var render$K = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-module-preview-button"}, [_c("div", {staticClass: "ui-button type-light", domProps: {textContent: _vm._s(_vm.text)}})]);
};
var staticRenderFns$K = [];
const script$K = {
  name: "uiModulePreviewButton",
  props: {
    text: {
      type: String
    }
  }
};
const __cssModules$K = {};
var component$K = normalizeComponent(script$K, render$K, staticRenderFns$K, false, injectStyles$K, null, null, null);
function injectStyles$K(context) {
  for (let o in __cssModules$K) {
    this[o] = __cssModules$K[o];
  }
}
component$K.options.__file = "app/components/modules/predefined/module-preview-button.vue";
var uiModulePreviewButton = component$K.exports;
var render$J = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-module-preview-tags"}, _vm._l(_vm.items, function(item2) {
    return _c("div", {staticClass: "ui-module-preview-tag"}, [_vm._v(_vm._s(!!_vm.property ? item2[_vm.property] : item2))]);
  }), 0);
};
var staticRenderFns$J = [];
var modulePreviewTags_vue_vue_type_style_index_0_lang = ".ui-module-preview-tags {\n  display: flex;\n  gap: 8px;\n}\n.ui-module-preview-tag {\n  display: inline-block;\n  border: 1px solid var(--color-line);\n  border-radius: var(--radius);\n  padding: 6px 8px 4px;\n}";
const script$J = {
  name: "uiModulePreviewTags",
  props: {
    items: {
      type: Array
    },
    property: {
      type: String,
      default: null
    }
  }
};
const __cssModules$J = {};
var component$J = normalizeComponent(script$J, render$J, staticRenderFns$J, false, injectStyles$J, null, null, null);
function injectStyles$J(context) {
  for (let o in __cssModules$J) {
    this[o] = __cssModules$J[o];
  }
}
component$J.options.__file = "app/components/modules/predefined/module-preview-tags.vue";
var uiModulePreviewTags = component$J.exports;
var render$I = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-module-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: _vm.config.module.name, "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), !_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, label: "Confirm", state: form.state, disabled: _vm.loading}}) : _vm._e()];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-module-overlay-editor"}, [_c("ui-editor", {attrs: {config: _vm.editor, meta: _vm.meta, "is-page": false, infos: "none", disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$I = [];
var editModule_vue_vue_type_style_index_0_lang = ".ui-module-overlay > content {\n  position: relative;\n  padding-top: 0 !important;\n}\n.ui-module-overlay .ui-box {\n  margin: 0;\n}\n.ui-module-overlay .ui-tabs-list {\n  padding: 0;\n  padding-bottom: 32px;\n}\n.ui-module-overlay .ui-property.ui-modules {\n  margin: 0;\n  padding: 0;\n}\n.ui-module-overlay .editor-outer.-infos-aside:not(.is-page) {\n  display: block;\n}\n.ui-module-overlay .ui-loading {\n  position: absolute;\n  left: 50%;\n  top: 50%;\n  margin: -14px 0 0 -14px;\n}";
const script$I = {
  props: {
    config: Object
  },
  components: {UiEditor},
  data: () => ({
    isAdd: true,
    disabled: false,
    id: null,
    loading: true,
    state: "default",
    meta: {},
    editor: null,
    model: {}
  }),
  created() {
    this.editor = this.config.editor;
  },
  methods: {
    onLoad(form) {
      if (this.config.model) {
        this.isAdd = false;
        this.model = JSON.parse(JSON.stringify(this.config.model));
      }
      form.load(ModulesApi.getEmpty(this.config.module.alias)).then((response) => {
        this.disabled = !response.meta.canEdit || this.config.disabled;
        this.meta = response.meta;
        if (this.isAdd) {
          this.model = response.entity;
        }
        this.loading = false;
      });
    },
    onSubmit(form) {
      this.config.confirm(this.model);
    }
  }
};
const __cssModules$I = {};
var component$I = normalizeComponent(script$I, render$I, staticRenderFns$I, false, injectStyles$I, null, null, null);
function injectStyles$I(context) {
  for (let o in __cssModules$I) {
    this[o] = __cssModules$I[o];
  }
}
component$I.options.__file = "app/components/modules/edit-module.vue";
var EditModuleOverlay = component$I.exports;
const script$H = {
  name: "uiModulePreviewInner",
  props: {
    value: {
      type: Object,
      default: () => {
      }
    },
    template: {
      type: String,
      default: null
    }
  },
  render: function(h) {
    if (!this.template) {
      return;
    }
    return h({
      template: '<div class="ui-module-preview-inner">' + this.template + "</div>",
      props: {
        model: {
          type: Object,
          default: () => {
          }
        }
      }
    }, {
      props: {
        model: this.value
      }
    });
  }
};
let render$H, staticRenderFns$H;
const __cssModules$H = {};
var component$H = normalizeComponent(script$H, render$H, staticRenderFns$H, false, injectStyles$H, null, null, null);
function injectStyles$H(context) {
  for (let o in __cssModules$H) {
    this[o] = __cssModules$H[o];
  }
}
component$H.options.__file = "app/components/modules/module-preview-inner.vue";
var ModulePreviewInner = component$H.exports;
var render$G = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("div", {staticClass: "ui-module-item", class: {"can-edit": _vm.canEdit}, attrs: {"data-module": _vm.alias}}, [_vm.module ? _c("div", {staticClass: "ui-module-item-content", on: {click: function($event) {
    return _vm.emit("edit");
  }}}, [!_vm.preview || !_vm.preview.hideLabel ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.module.name, expression: "module.name"}], staticClass: "ui-module-item-header"}) : _vm._e(), _vm.tryRender && typeof _vm.preview.template === "string" ? _c("module-preview-inner", {attrs: {template: _vm.preview.template, value: _vm.value}}) : _vm._e(), _vm.tryRender && typeof _vm.preview.template !== "string" ? _c("div", {staticClass: "ui-module-preview-inner"}, [_c(_vm.preview.template, {tag: "component", attrs: {model: _vm.value}})], 1) : _vm._e()], 1) : _c("div", {staticClass: "ui-module-item-content"}, [_c("span", {staticClass: "ui-module-item-header is-error"}, [_c("i", {staticClass: "fth-alert-circle"}), _vm._v(" " + _vm._s(_vm.alias))]), _c("p", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: "@modules.notfound", tokens: {alias: _vm.alias}}, expression: "{ key: '@modules.notfound', tokens: { alias: alias } }", arg: "html"}], staticClass: "ui-module-item-error"})]), !_vm.value.isActive ? _c("div", {staticClass: "ui-module-item-disabled"}) : _vm._e(), !_vm.disabled ? _c("div", {staticClass: "ui-module-item-actions"}, [_c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [!_vm.value.isActive ? _c("ui-icon-button", {staticClass: "ui-module-item-disabled-icon", attrs: {icon: "fth-lock", title: "Disabled"}}) : _c("ui-icon-button", {attrs: {icon: "fth-more-horizontal", title: "Actions"}})];
  }, proxy: true}], null, false, 3824822733)}, [_vm.canEdit ? _c("ui-dropdown-button", {attrs: {label: "Edit", icon: "fth-edit-2"}, on: {click: function($event) {
    return _vm.emit("edit");
  }}}) : _vm._e(), _vm.value.isActive ? _c("ui-dropdown-button", {attrs: {label: "Disable", icon: "fth-lock"}, on: {click: function($event) {
    return _vm.toggleStatus(false);
  }}}) : _c("ui-dropdown-button", {attrs: {label: "Enable", icon: "fth-unlock"}, on: {click: function($event) {
    return _vm.toggleStatus(true);
  }}}), _c("ui-dropdown-button", {attrs: {label: "Remove", icon: "fth-trash"}, on: {click: function($event) {
    return _vm.emit("remove");
  }}})], 1)], 1) : _vm._e()]) : _vm._e();
};
var staticRenderFns$G = [];
var modulePreview_vue_vue_type_style_index_0_lang = ".ui-module-item {\n  display: grid !important;\n  grid-template-columns: 1fr auto 0;\n  grid-column-gap: var(--padding);\n  position: relative;\n  margin: 0 -32px;\n  padding: 0;\n  /*margin-top: var(--padding);\n  padding-top: var(--padding);*/\n  border-bottom: 1px solid var(--color-line);\n  /*&:first-child\n  {\n    border-top: none;\n  }*/\n}\n.ui-module-item.can-edit .ui-module-item-content {\n  cursor: pointer;\n}\n.ui-module-item:not(.can-edit) .ui-module-item-header {\n  cursor: default;\n}\n.ui-module-item-content {\n  padding: var(--padding);\n  padding-right: 0;\n}\n.ui-module-item-header {\n  display: flex;\n  align-items: center;\n  color: var(--color-text-dim);\n  font-size: var(--font-size-s);\n  width: 100%;\n}\n.ui-module-item-header i {\n  font-size: var(--font-size-l);\n  margin-right: 10px;\n  position: relative;\n  top: -1px;\n}\n.ui-module-item-header.is-error {\n  color: var(--color-accent-error);\n}\n.ui-module-item-disabled {\n  background: var(--color-box);\n  border-radius: var(--radius);\n  opacity: 0.5;\n  position: absolute;\n  left: 0;\n  right: 0;\n  top: 0;\n  bottom: 0;\n  z-index: 1;\n}\n.ui-module-item-disabled-icon {\n  position: relative;\n  z-index: 1;\n}\n.ui-module-item-disabled-icon .ui-button-icon {\n  color: var(--color-accent-error);\n}\n.ui-module-preview-inner {\n  max-width: 1060px;\n}\n.ui-module-item-header + .ui-module-preview-inner {\n  padding-top: 6px;\n}\n.ui-module-preview-inner p {\n  margin: 0;\n}\n.ui-module-preview-inner p + p {\n  margin-top: 10px;\n}\n.ui-module-preview-inner a {\n  color: var(--color-text);\n  text-decoration: underline dotted var(--color-text-dim);\n  text-underline-offset: 3px;\n}\n.ui-module-item-actions {\n  display: flex;\n  grid-column: 2;\n  align-self: center;\n  margin: -10px 0;\n  opacity: 0;\n  transition: opacity 0.2s ease;\n}\n.ui-module-item-actions > * + * {\n  margin-left: 10px;\n}\n.ui-module-item:hover .ui-module-item-actions {\n  opacity: 1;\n}\n.ui-module-item-error {\n  margin: 0;\n  grid-column: 1;\n  margin-top: 12px;\n}";
const script$G = {
  name: "uiModulePreview",
  components: {ModulePreviewInner},
  props: {
    value: {
      type: Object,
      default: () => {
      }
    },
    types: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: Object
  },
  data: () => ({
    loading: true,
    module: {},
    renderer: {},
    preview: null
  }),
  watch: {
    value(val) {
      this.render(val);
    }
  },
  computed: {
    alias() {
      return this.value.moduleTypeAlias;
    },
    tryRender() {
      return this.preview && this.preview.template;
    },
    canEdit() {
      return this.module && this.renderer && this.renderer.fields && this.renderer.fields.length > 0;
    }
  },
  mounted() {
    this.render(this.value);
  },
  methods: {
    render(value) {
      this.loading = true;
      this.module = find(this.types, (x) => x.alias == this.alias);
      this.renderer = this.zero.getEditor("module." + this.alias);
      if (this.renderer) {
        this.preview = this.renderer.previewOptions;
      }
      this.$nextTick(() => this.loading = false);
    },
    emit(ev) {
      if (!this.canEdit && ev === "edit") {
        return;
      }
      this.$emit(ev, this.module, this.value);
    },
    toggleStatus(isActive) {
      this.value.isActive = isActive;
      this.emit("isActive");
    }
  }
};
const __cssModules$G = {};
var component$G = normalizeComponent(script$G, render$G, staticRenderFns$G, false, injectStyles$G, null, null, null);
function injectStyles$G(context) {
  for (let o in __cssModules$G) {
    this[o] = __cssModules$G[o];
  }
}
component$G.options.__file = "app/components/modules/module-preview.vue";
var ModulePreview = component$G.exports;
var render$F = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-modules-select"}, [_c("h2", {staticClass: "ui-headline"}, [_vm._v("Add module")]), !_vm.loading ? _c("div", [_c("div", {staticClass: "ui-modules-select-items"}, _vm._l(_vm.types, function(item2) {
    return _c("button", {staticClass: "ui-modules-select-item", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.onSelect(item2);
    }}}, [_c("div", {staticClass: "ui-modules-select-item-top"}, [_c("ui-icon", {staticClass: "ui-modules-select-item-icon", attrs: {symbol: item2.icon, size: 22}})], 1), _c("span", {staticClass: "ui-modules-select-item-text"}, [_c("ui-localize", {staticClass: "-headline", attrs: {value: item2.name}}), item2.description ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item2.description, expression: "item.description"}], staticClass: "-desc"}) : _vm._e()], 1)]);
  }), 0), !_vm.types.length ? _c("ui-message", {attrs: {type: "error", text: "@page.create.nonavailable"}}) : _vm._e(), _c("div", {staticClass: "app-confirm-buttons"}, [_c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel}, on: {click: _vm.config.close}})], 1)], 1) : _vm._e()]);
};
var staticRenderFns$F = [];
var moduleSelect_vue_vue_type_style_index_0_lang = ".app-overlay[data-alias=modules-select] {\n  width: calc(100vw - 40px);\n  max-width: 1080px;\n  height: calc(100vh - 40px);\n  max-height: 750px;\n}\n.ui-modules-select {\n  text-align: left;\n}\n.ui-modules-select .ui-message {\n  margin: 0;\n}\n.ui-modules-select-parent {\n  margin: 30px 0 -10px 0;\n  border-radius: var(--radius);\n  /*border: 1px solid var(--color-line-light);*/\n  background: var(--color-box-nested);\n  line-height: 1.4;\n  color: var(--color-text-dim);\n  padding: 14px 16px;\n  font-size: var(--font-size);\n}\n.ui-modules-select-parent strong {\n  color: var(--color-text);\n}\n.ui-modules-select-items {\n  display: grid;\n  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));\n  grid-gap: var(--padding-m);\n  margin: 0 -16px;\n  padding: 0 16px;\n  margin-top: var(--padding);\n  max-height: 550px;\n  overflow-y: auto;\n}\n.ui-modules-select-item {\n  display: inline-flex;\n  flex-direction: column;\n  width: 100%;\n  grid-template-columns: 40px 1fr auto;\n  gap: 12px;\n  align-items: stretch;\n  position: relative;\n  color: var(--color-text);\n  /*&:hover, &:focus\n  {\n    background: var(--color-tree-selected);\n  }*/\n}\n.ui-modules-select-item:hover .ui-modules-select-item-top {\n  border: 1px solid var(--color-bg-shade-5);\n  background: var(--color-bg-shade-1);\n  outline: none;\n}\n.ui-modules-select-item-top {\n  background: var(--color-bg-shade-2);\n  height: 100px;\n  border-radius: var(--radius);\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  border: 1px solid var(--color-line);\n}\n.ui-modules-select-item-text {\n  display: flex;\n  flex-direction: column;\n}\n.ui-modules-select-item-text .-desc {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n  font-size: var(--font-size-s);\n  line-height: 1.4;\n}\n.ui-modules-select-item-text .-headline {\n  color: var(--color-text);\n  font-weight: 700;\n}\n.ui-modules-select-item-icon {\n  position: relative;\n  font-size: var(--size-xl);\n  color: var(--color-text);\n}";
const script$F = {
  props: {
    config: Object
  },
  data: () => ({
    model: {
      name: null,
      parentId: null,
      pageTypeAlias: null
    },
    loading: false,
    item: {},
    disabled: false,
    types: []
  }),
  created() {
    this.types = this.config.types;
    this.model.parentId = this.config.parent ? this.config.parent.id : null;
  },
  methods: {
    onSelect(item2) {
      this.config.confirm(item2);
    }
  }
};
const __cssModules$F = {};
var component$F = normalizeComponent(script$F, render$F, staticRenderFns$F, false, injectStyles$F, null, null, null);
function injectStyles$F(context) {
  for (let o in __cssModules$F) {
    this[o] = __cssModules$F[o];
  }
}
component$F.options.__file = "app/components/modules/module-select.vue";
var ModuleSelectOverlay = component$F.exports;
var render$E = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.loaded ? _c("div", {staticClass: "ui-modules-inner"}, [_vm.items.length ? _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated}, expression: "{ onUpdate: onSortingUpdated }"}], staticClass: "ui-modules-inner-sortable"}, _vm._l(_vm.items, function(item2) {
    return _c("module-preview", {key: item2.id, attrs: {types: _vm.moduleTypes, value: item2, disabled: _vm.disabled}, on: {edit: _vm.edit, remove: _vm.remove, isActive: _vm.onChange}});
  }), 1) : _vm._e(), _vm.canAdd ? _c("button", {staticClass: "ui-modules-start-button", attrs: {type: "button"}, on: {click: _vm.selectModule}}, [_c("span", {staticClass: "ui-modules-start-button-icon"}, [_c("ui-icon", {attrs: {symbol: "fth-plus", size: 19}})], 1), _vm._m(0)]) : _vm._e()]) : _vm._e();
};
var staticRenderFns$E = [function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("p", {staticClass: "ui-modules-start-button-text"}, [_c("strong", [_vm._v("Add content")])]);
}];
var modules_vue_vue_type_style_index_0_lang = ".ui-modules-inner-sortable {\n  margin-top: -32px;\n}\n.ui-modules-start-button {\n  color: var(--color-primary);\n  font-size: var(--font-size);\n  display: inline-grid;\n  grid-template-columns: auto 1fr;\n  gap: 25px;\n  align-items: center;\n}\n.ui-modules-inner-sortable + .ui-modules-start-button {\n  margin-top: var(--padding);\n}\n.ui-modules-start-button-icon {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  width: 52px;\n  height: 52px;\n  background: var(--color-button-light);\n  border-radius: var(--radius);\n}\n.ui-modules-start-button-text {\n  line-height: 1.3;\n  color: var(--color-text-dim);\n  margin: 0;\n  font-size: var(--font-size-s);\n}\n.ui-modules-start-button-text strong {\n  display: inline-block;\n  margin-bottom: 2px;\n  color: var(--color-text);\n  font-size: var(--font-size);\n}";
const script$E = {
  name: "uiModules",
  components: {ModulePreview},
  props: {
    value: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    tags: {
      type: Array,
      default: () => []
    },
    config: Object
  },
  data: () => ({
    loaded: false,
    items: [],
    moduleTypes: []
  }),
  watch: {
    value(val) {
      this.setup(val);
    }
  },
  computed: {
    canAdd() {
      return !this.disabled;
    }
  },
  created() {
    ModulesApi.getModuleTypes(this.tags).then((res) => {
      this.moduleTypes = res;
      this.setup(this.value);
      this.loaded = true;
    });
  },
  methods: {
    setup(value) {
      this.items = JSON.parse(JSON.stringify(value || []));
    },
    selectModule() {
      Overlay.open({
        alias: "modules-select",
        component: ModuleSelectOverlay,
        types: this.moduleTypes,
        width: null
      }).then((module) => this.onAdd(module), () => {
      });
    },
    onAdd(module) {
      this.edit(module, null, true);
    },
    onSortingUpdated(ev) {
      this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
      let sort = 0;
      this.items.forEach((x) => x.sort = sort++);
      this.onChange();
    },
    edit(module, model, isAdd) {
      const alias2 = "module." + module.alias;
      const editor2 = this.zero.getEditor(alias2);
      if (!editor2.fields || editor2.fields.length < 1) {
        return ModulesApi.getEmpty(module.alias).then((res) => {
          this.items.push(res.entity);
          this.onChange();
        });
      }
      return Overlay.open({
        component: EditModuleOverlay,
        display: "editor",
        module,
        editor: editor2,
        model,
        width: 820
      }).then((value) => {
        if (isAdd) {
          this.items.push(value);
        } else {
          Arrays.replace(this.items, model, value);
        }
        this.onChange();
      });
    },
    remove(module, model) {
      Arrays.remove(this.items, model);
      this.onChange();
    },
    onChange() {
      this.$emit("input", this.items);
    }
  }
};
const __cssModules$E = {};
var component$E = normalizeComponent(script$E, render$E, staticRenderFns$E, false, injectStyles$E, null, null, null);
function injectStyles$E(context) {
  for (let o in __cssModules$E) {
    this[o] = __cssModules$E[o];
  }
}
component$E.options.__file = "app/components/modules/modules.vue";
var uiModules = component$E.exports;
var render$D = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {on: {click: _vm.onClickShim}}, [!_vm.confirming ? _c("button", {staticClass: "ui-dropdown-button", class: {"has-icon": _vm.icon, "is-active": _vm.selected, "is-multiline": _vm.multiline}, attrs: {disabled: _vm.disabled, type: "button"}, on: {click: _vm.onClick}}, [_vm.icon ? _c("ui-icon", {staticClass: "ui-dropdown-button-icon", attrs: {symbol: _vm.icon}}) : _vm._e(), _c("span", [_c("ui-localize", {attrs: {value: _vm.label}}), _vm._e(), _vm._e()], 1), _vm.selected ? _c("ui-icon", {staticClass: "ui-dropdown-button-selected", attrs: {symbol: "check"}}) : _vm._e(), _vm.loading ? _c("i", {staticClass: "ui-dropdown-button-progress"}) : _vm._e()], 1) : _vm._e(), _vm.confirming ? _c("div", {staticClass: "ui-dropdown-button-confirmation"}, [_vm.icon ? _c("ui-icon", {staticClass: "ui-dropdown-button-icon", attrs: {symbol: _vm.icon}}) : _vm._e(), _c("ui-localize", {attrs: {value: _vm.label}}), _c("ui-button", {attrs: {type: "small light", icon: "fth-x", title: "Cancel"}, on: {click: function($event) {
    _vm.confirming = false;
  }}}), _c("ui-button", {attrs: {type: _vm.negative ? "small danger" : "small primary", icon: "fth-check", title: "OK"}, on: {click: function($event) {
    return _vm.onClick($event, true);
  }}})], 1) : _vm._e()]);
};
var staticRenderFns$D = [];
var dropdownButton_vue_vue_type_style_index_0_lang = "button.ui-dropdown-button {\n  display: grid;\n  width: 100%;\n  grid-template-columns: minmax(0, 1fr) auto;\n  gap: 6px;\n  align-items: center;\n  font-size: var(--font-size);\n  font-weight: 500;\n  padding: 0 16px;\n  height: 48px;\n  color: var(--color-text);\n  border-radius: var(--radius);\n  white-space: nowrap;\n  text-overflow: ellipsis;\n  overflow: hidden;\n  max-width: 100%;\n}\nbutton.ui-dropdown-button.has-icon {\n  grid-template-columns: 30px minmax(0, 1fr) auto;\n}\nbutton.ui-dropdown-button.has-icon:not([disabled]):hover .ui-dropdown-button-icon {\n  color: var(--color-text);\n}\nbutton.ui-dropdown-button.has-icon.is-negative:not([disabled]):hover .ui-dropdown-button-icon {\n  color: var(--color-accent-error);\n}\nbutton.ui-dropdown-button.is-multiline {\n  white-space: normal;\n  overflow: visible;\n}\nbutton.ui-dropdown-button:not([disabled]):hover, button.ui-dropdown-button.is-active, button.ui-dropdown-button:focus {\n  background: var(--color-dropdown-selected);\n  color: var(--color-text);\n}\nbutton.ui-dropdown-button:not([disabled]):hover .ui-dropdown-list-item-icon, button.ui-dropdown-button.is-active .ui-dropdown-list-item-icon, button.ui-dropdown-button:focus .ui-dropdown-list-item-icon {\n  color: var(--color-text);\n}\nbutton.ui-dropdown-button.is-active {\n  font-weight: 700;\n}\nbutton.ui-dropdown-button.is-active .ui-dropdown-button-icon {\n  color: var(--color-text);\n}\nbutton.ui-dropdown-button[disabled] {\n  color: var(--color-text-dim);\n  cursor: default;\n  pointer-events: none;\n}\nbutton.ui-dropdown-button[disabled] .ui-dropdown-list-item-icon,\nbutton.ui-dropdown-button[disabled] .ui-dropdown-button-icon {\n  color: var(--color-text-dim);\n}\nbutton.ui-dropdown-button + .ui-dropdown-button {\n  margin-top: 4px;\n}\n.ui-dropdown-button-icon {\n  font-size: 18px;\n  line-height: 1;\n  font-weight: 400;\n  position: relative;\n  top: -1px;\n  color: var(--color-text);\n}\n.ui-dropdown-button-progress {\n  width: 16px;\n  height: 16px;\n  z-index: 2;\n  border-radius: 40px;\n  border: 2px solid transparent;\n  border-left-color: var(--color-text);\n  opacity: 1;\n  will-change: transform;\n  animation: rotating 0.5s linear infinite;\n  transition: opacity 0.25s ease;\n}\n.ui-dropdown-button-confirmation {\n  display: grid;\n  flex-grow: 0;\n  width: 100%;\n  grid-template-columns: 30px minmax(0, 1fr) auto auto;\n  gap: 6px;\n  align-items: center;\n  font-size: var(--font-size);\n  padding: 0 6px 0 16px;\n  height: 48px;\n  color: var(--color-text);\n  border-radius: var(--radius);\n  white-space: nowrap;\n  text-overflow: ellipsis;\n  overflow: hidden;\n  max-width: 300px;\n}\n.ui-dropdown-button-confirmation .ui-button + .ui-button {\n  margin-left: 2px;\n}\n.ui-dropdown-button-confirmation .ui-button {\n  width: 40px;\n}\n@keyframes rotating {\nfrom {\n    -webkit-transform: rotate(0);\n    transform: rotate(0);\n}\nto {\n    -webkit-transform: rotate(1turn);\n    transform: rotate(1turn);\n}\n}";
const script$D = {
  name: "uiDropdownButton",
  props: {
    value: {
      default: null
    },
    multiline: {
      type: Boolean,
      default: false
    },
    label: {
      type: String,
      required: true
    },
    icon: {
      type: String
    },
    selected: {
      type: Boolean,
      default: false
    },
    confirm: {
      type: Boolean,
      default: false
    },
    disabled: {
      type: Boolean,
      default: false
    },
    negative: {
      type: Boolean,
      default: false
    },
    prevent: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    dropdown: null,
    loading: false,
    confirming: false
  }),
  mounted() {
    let current = this;
    do {
      if (current.$options.name === "uiDropdown") {
        this.dropdown = current;
        break;
      }
    } while (current = current.$parent);
    if (!this.dropdown)
      ;
  },
  methods: {
    onClick(e, confirmed) {
      var instance = this;
      if (!this.loading && !this.disabled) {
        if (this.confirm && !confirmed) {
          this.confirming = true;
          return;
        } else {
          this.confirming = false;
        }
        if (!this.prevent && this.dropdown) {
          this.dropdown.hide();
        }
        this.$emit("click", this.value, {
          dropdown: this.dropdown,
          hide() {
            if (this.dropdown) {
              this.dropdown.hide();
            }
            instance.$emit("hide");
          },
          loading(isLoading) {
            instance.loading = isLoading;
          }
        });
      }
    },
    onClickShim(e) {
      e.preventDefault();
      e.stopPropagation();
    }
  }
};
const __cssModules$D = {};
var component$D = normalizeComponent(script$D, render$D, staticRenderFns$D, false, injectStyles$D, null, null, null);
function injectStyles$D(context) {
  for (let o in __cssModules$D) {
    this[o] = __cssModules$D[o];
  }
}
component$D.options.__file = "app/components/overlays/dropdown-button.vue";
var uiDropdownButton = component$D.exports;
var render$C = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("hr", {staticClass: "ui-dropdown-separator"});
};
var staticRenderFns$C = [];
var dropdownSeparator_vue_vue_type_style_index_0_lang = ".ui-dropdown-separator {\n  border: none;\n  border-bottom: 1px solid var(--color-dropdown-line);\n  margin: 5px 0;\n}";
const script$C = {
  name: "uiDropdownSeparator"
};
const __cssModules$C = {};
var component$C = normalizeComponent(script$C, render$C, staticRenderFns$C, false, injectStyles$C, null, null, null);
function injectStyles$C(context) {
  for (let o in __cssModules$C) {
    this[o] = __cssModules$C[o];
  }
}
component$C.options.__file = "app/components/overlays/dropdown-separator.vue";
var uiDropdownSeparator = component$C.exports;
var render$B = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-dropdown-container"}, [_vm.hasButton ? _c("div", {ref: "trigger", staticClass: "ui-dropdown-toggle", on: {click: function($event) {
    $event.preventDefault();
    $event.stopPropagation();
    return _vm.toggle($event);
  }}}, [_vm._t("button")], 2) : _vm._e(), _vm.open ? _c("div", {directives: [{name: "click-outside", rawName: "v-click-outside", value: _vm.hide, expression: "hide"}], ref: "overlay", staticClass: "ui-dropdown", class: _vm.dropdownClasses, attrs: {role: "dialog"}}, [_vm._t("default")], 2) : _vm._e()]);
};
var staticRenderFns$B = [];
var dropdown_vue_vue_type_style_index_0_lang = ".ui-dropdown-container {\n  position: relative;\n}\n.ui-dropdown {\n  position: absolute;\n  min-width: 300px;\n  min-height: 20px;\n  background: var(--color-dropdown);\n  border-radius: var(--radius);\n  border: 1px solid var(--color-dropdown-border);\n  box-shadow: var(--shadow-dropdown);\n  z-index: 8;\n  top: calc(100% + 5px);\n  padding: 5px;\n  color: var(--color-text);\n}\n.ui-dropdown.align-right {\n  right: 0;\n}\n.ui-dropdown.align-top {\n  top: calc(100% + 5px);\n  bottom: auto;\n}\n.ui-dropdown.align-bottom {\n  bottom: calc(100% + 5px);\n  top: auto;\n}";
const script$B = {
  name: "uiDropdown",
  props: {
    align: {
      type: String,
      default: "left"
    },
    theme: {
      type: String,
      default: "dark"
    },
    locked: {
      type: Boolean,
      default: false
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    hasButton() {
      return this.$scopedSlots.hasOwnProperty("button");
    },
    dropdownClasses() {
      let classes = "align-" + this.align.split(" ").join(" align-");
      if (!!this.theme) {
        classes += " theme-" + this.theme;
      }
      return classes;
    }
  },
  data: () => ({
    open: false
  }),
  methods: {
    toggle() {
      if (this.open) {
        this.hide();
      } else if (!this.disabled) {
        this.show();
      }
    },
    show() {
      if (this.disabled) {
        return;
      }
      Overlay.setDropdown(this);
      this.open = true;
      this.$emit("opened");
    },
    hide() {
      if (this.locked) {
        return;
      }
      this.open = false;
      this.$emit("closed");
    },
    position() {
      this.$nextTick(() => {
        const reference = this.$refs.trigger;
        const triggerBoundingBox = reference.getBoundingClientRect();
        this.$refs.overlay.getBoundingClientRect();
        const windowBox = {width: window.innerWidth, height: window.innerHeight};
        const windowOffset = 32;
        let availableSpace = {
          left: triggerBoundingBox.left + triggerBoundingBox.width - windowOffset,
          right: windowBox.width - triggerBoundingBox.left - windowOffset,
          top: triggerBoundingBox.top + triggerBoundingBox.height - windowOffset,
          bottom: windowBox.height - triggerBoundingBox.top - windowOffset
        };
        console.table(availableSpace);
      });
    }
  }
};
const __cssModules$B = {};
var component$B = normalizeComponent(script$B, render$B, staticRenderFns$B, false, injectStyles$B, null, null, null);
function injectStyles$B(context) {
  for (let o in __cssModules$B) {
    this[o] = __cssModules$B[o];
  }
}
component$B.options.__file = "app/components/overlays/dropdown.vue";
var uiDropdown = component$B.exports;
var render$A = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-overlay-editor"}, [_c("header", [_vm._t("header")], 2), _c("content", [_vm._t("default")], 2), _c("footer", [_vm._t("footer")], 2)]);
};
var staticRenderFns$A = [];
var overlayEditor_vue_vue_type_style_index_0_lang = ".ui-overlay-editor {\n  display: grid;\n  grid-template-rows: auto 1fr auto;\n  height: 100vh;\n}\n.ui-overlay-editor > header {\n  display: block;\n}\n.ui-overlay-editor > footer {\n  display: flex;\n  justify-content: flex-end;\n  padding: 20px var(--padding);\n}\n.ui-overlay-editor > content {\n  display: block;\n  padding: var(--padding);\n  height: 100%;\n  overflow-y: auto;\n}";
const script$A = {};
const __cssModules$A = {};
var component$A = normalizeComponent(script$A, render$A, staticRenderFns$A, false, injectStyles$A, null, null, null);
function injectStyles$A(context) {
  for (let o in __cssModules$A) {
    this[o] = __cssModules$A[o];
  }
}
component$A.options.__file = "app/components/overlays/overlay-editor.vue";
var uiOverlayEditor = component$A.exports;
var render$z = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-colorpicker", class: {"is-disabled": _vm.disabled}}, [_c("label", {staticClass: "ui-colorpicker-color", attrs: {for: _vm.id}}, [_c("span", {staticClass: "ui-colorpicker-color-preview", style: {"background-color": _vm.value || "var(--color-box)"}}), _c("input", {attrs: {id: _vm.id, type: "color", disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: _vm.onChange}})]), _c("input", {directives: [{name: "localize", rawName: "v-localize:placeholder", value: "@colorpicker.placeholder", expression: "'@colorpicker.placeholder'", arg: "placeholder"}], staticClass: "ui-colorpicker-input", attrs: {type: "text", maxlength: "7", disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: _vm.onChange}})]);
};
var staticRenderFns$z = [];
var colorpicker_vue_vue_type_style_index_0_lang = ".ui-colorpicker {\n  position: relative;\n}\n.ui-colorpicker-color {\n  display: inline-block;\n  position: absolute;\n  overflow: hidden;\n  width: 32px;\n  height: 100%;\n  border-radius: 3px;\n  left: 0;\n  top: -1px;\n  padding: 0 !important;\n  cursor: pointer;\n}\n.ui-colorpicker-color-preview {\n  position: absolute;\n  left: 12px;\n  top: 50%;\n  margin-top: -8px;\n  width: 16px;\n  height: 16px;\n  border-radius: 2px;\n  box-shadow: var(--shadow-short);\n}\n.ui-colorpicker-color input {\n  opacity: 0 !important;\n  visibility: hidden !important;\n  position: absolute;\n  left: 0;\n  top: 0;\n  bottom: 0;\n  right: 0;\n}\ninput[type=text].ui-colorpicker-input {\n  padding-left: 40px;\n  max-width: 322px;\n}\n.ui-colorpicker-preview {\n  display: inline-block;\n  margin-top: -11px;\n  border: 1px solid #eceaea;\n  cursor: pointer;\n}";
const script$z = {
  name: "uiColorpicker",
  props: {
    value: {
      type: String,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
        return {};
      }
    }
  },
  data: () => ({
    id: null
  }),
  created() {
    this.id = "colorpicker-" + Strings.guid();
  },
  methods: {
    onChange(ev) {
      this.$emit("change", ev.target.value);
      this.$emit("input", ev.target.value);
    }
  }
};
const __cssModules$z = {};
var component$z = normalizeComponent(script$z, render$z, staticRenderFns$z, false, injectStyles$z, null, null, null);
function injectStyles$z(context) {
  for (let o in __cssModules$z) {
    this[o] = __cssModules$z[o];
  }
}
component$z.options.__file = "app/components/pickers/colorPicker/colorpicker.vue";
var uiColorpicker = component$z.exports;
var render$y = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-countrypicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$y = [];
const script$y = {
  name: "uiCountrypicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "country",
      items: countriesApi.getForPicker,
      previews: countriesApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {}
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$y = {};
var component$y = normalizeComponent(script$y, render$y, staticRenderFns$y, false, injectStyles$y, null, null, null);
function injectStyles$y(context) {
  for (let o in __cssModules$y) {
    this[o] = __cssModules$y[o];
  }
}
component$y.options.__file = "app/components/pickers/countryPicker/countrypicker.vue";
var uiCountrypicker = component$y.exports;
var render$x = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {ref: "calendar", staticClass: "ui-datepicker-overlay"});
};
var staticRenderFns$x = [];
var overlay_vue_vue_type_style_index_0_lang$4 = "";
const script$x = {
  props: {
    value: String,
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    date: null,
    initialized: false
  }),
  mounted() {
    let vm = this;
    if (this.initialized) {
      return;
    }
    flatpickr(this.$refs.calendar, extend({
      inline: true,
      enableTime: true,
      time_24hr: true,
      defaultDate: this.value,
      minuteIncrement: 1,
      onChange(dates) {
        vm.$emit("change", dates[0]);
      }
    }, this.options));
    this.initialized = true;
  }
};
const __cssModules$x = {};
var component$x = normalizeComponent(script$x, render$x, staticRenderFns$x, false, injectStyles$x, null, null, null);
function injectStyles$x(context) {
  for (let o in __cssModules$x) {
    this[o] = __cssModules$x[o];
  }
}
component$x.options.__file = "app/components/pickers/datePicker/overlay.vue";
var DatepickerOverlay = component$x.exports;
var render$w = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-datepicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {directives: [{name: "localize", rawName: "v-localize:placeholder", value: _vm.placeholder, expression: "placeholder", arg: "placeholder"}], staticClass: "ui-input ui-datepicker-input", attrs: {type: "text", disabled: _vm.disabled}, domProps: {value: _vm.output}, on: {input: _vm.onChange, focus: _vm.onFocus, blur: _vm.onBlur}}), !_vm.clear || !_vm.value ? _c("ui-icon", {staticClass: "ui-datepicker-icon", attrs: {symbol: "fth-calendar", size: 17}}) : _vm._e(), _vm.clear && _vm.value ? _c("button", {staticClass: "ui-datepicker-input-button", attrs: {type: "button"}, on: {click: _vm.clearInput}}, [_c("i", {staticClass: "fth-x"})]) : _vm._e(), _c("ui-dropdown", {ref: "overlay", staticClass: "ui-datepicker-overlay", on: {opened: _vm.overlayOpened}}, [_c("datepicker-overlay", {attrs: {value: _vm.value, options: _vm.pickerOptions}, on: {change: _vm.onSelect}})], 1)], 1);
};
var staticRenderFns$w = [];
var datepicker_vue_vue_type_style_index_0_lang = ".ui-datepicker {\n  position: relative;\n  max-width: 260px;\n}\ninput[type=text].ui-datepicker-input {\n  padding-right: 36px;\n}\n.ui-datepicker-icon {\n  position: absolute;\n  right: 13px;\n  top: 50%;\n  margin-top: -8px;\n}\n.ui-datepicker-overlay .ui-dropdown {\n  padding: 0;\n}\n.ui-datepicker-input-button {\n  position: absolute;\n  right: 0;\n  top: 0;\n  height: 100%;\n  width: 40px;\n  text-align: center;\n  font-size: var(--font-size);\n  padding-top: 1px;\n}";
const DATETIME_FORMAT$1 = "DD.MM.YY HH:mm";
const DATE_FORMAT$1 = "DD.MM.YY";
const script$w = {
  name: "uiDatepicker",
  components: {DatepickerOverlay},
  props: {
    value: {
      type: String,
      default: null
    },
    placeholder: {
      type: String,
      default: "@ui.date.select"
    },
    clear: {
      type: Boolean,
      default: true
    },
    disabled: {
      type: Boolean,
      default: false
    },
    format: {
      type: String,
      default: null
    },
    time: {
      type: Boolean,
      default: false
    },
    maxDate: {
      type: [String, Date],
      default: null
    },
    minDate: {
      type: [String, Date],
      default: null
    },
    amPm: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    id: null,
    output: null,
    pickerOptions: {}
  }),
  watch: {
    value() {
      this.updateOutput();
    },
    format() {
      this.updateOutput();
    }
  },
  mounted() {
    this.id = "datepicker-" + Strings.guid();
    this.updateOptions();
    this.updateOutput();
  },
  methods: {
    updateOutput() {
      this.output = Strings.date(this.value, this.format || (this.time ? DATETIME_FORMAT$1 : DATE_FORMAT$1));
    },
    updateOptions() {
      this.pickerOptions = {
        enableTime: this.time,
        maxDate: this.maxDate,
        minDate: this.minDate,
        time_24hr: !this.amPm
      };
    },
    onSelect(date2) {
      let dateStr = dayjs(date2).toISOString();
      this.setValue(dateStr);
      this.$refs.overlay.hide();
      document.activeElement.blur();
    },
    onChange(ev) {
      this.setValue(ev.target.value);
    },
    setValue(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    onFocus() {
      this.$refs.overlay.show();
    },
    onBlur() {
      if (!this.$refs.overlay.open) {
        this.$refs.overlay.hide();
      }
    },
    clearInput() {
      this.setValue(null);
    },
    overlayOpened() {
    }
  }
};
const __cssModules$w = {};
var component$w = normalizeComponent(script$w, render$w, staticRenderFns$w, false, injectStyles$w, null, null, null);
function injectStyles$w(context) {
  for (let o in __cssModules$w) {
    this[o] = __cssModules$w[o];
  }
}
component$w.options.__file = "app/components/pickers/datePicker/datepicker.vue";
var uiDatepicker = component$w.exports;
var render$v = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "ui-daterangepicker", scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("div", {staticClass: "ui-daterangepicker-items"}, [_c("div", {class: {"ui-split": _vm.config.options.rangeEnd}}, [_c("div", {staticClass: "ui-daterangepicker-group"}, [_c("ui-property", {attrs: {label: _vm.config.options.fromText, vertical: true}}, [_c("ui-datepicker", {attrs: {time: true}, model: {value: _vm.fromDate, callback: function($$v) {
      _vm.fromDate = $$v;
    }, expression: "fromDate"}})], 1)], 1), _vm.config.options.rangeEnd ? _c("div", {staticClass: "ui-daterangepicker-group"}, [_c("ui-property", {attrs: {label: _vm.config.options.toText, vertical: true}}, [_c("ui-datepicker", {attrs: {time: true}, model: {value: _vm.toDate, callback: function($$v) {
      _vm.toDate = $$v;
    }, expression: "toDate"}})], 1)], 1) : _vm._e()])]), _c("div", {staticClass: "app-confirm-buttons"}, [_c("ui-button", {attrs: {type: "primary", label: "@ui.confirm"}, on: {click: _vm.confirm}}), _c("ui-button", {attrs: {type: "light", label: "@ui.close"}, on: {click: _vm.config.close}})], 1)];
  }}])});
};
var staticRenderFns$v = [];
var overlay_vue_vue_type_style_index_0_lang$3 = ".ui-daterangepicker {\n  text-align: left;\n}\nh3.ui-daterangepicker-group-header {\n  font-weight: 400;\n  font-size: var(--font-size);\n}";
const script$v = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    disabled: false,
    fromDate: null,
    toDate: null
  }),
  mounted() {
    this.fromDate = this.model.from;
    this.toDate = this.model.to;
  },
  methods: {
    confirm() {
      this.config.confirm({
        from: this.fromDate,
        to: this.toDate
      }, this.config);
    }
  }
};
const __cssModules$v = {};
var component$v = normalizeComponent(script$v, render$v, staticRenderFns$v, false, injectStyles$v, null, null, null);
function injectStyles$v(context) {
  for (let o in __cssModules$v) {
    this[o] = __cssModules$v[o];
  }
}
component$v.options.__file = "app/components/pickers/dateRangePicker/overlay.vue";
var DaterangepickerOverlay = component$v.exports;
var render$u = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-daterangepicker", class: {"is-disabled": _vm.disabled}}, [!_vm.inline ? _c("button", {directives: [{name: "localize", rawName: "v-localize", value: _vm.scheduleLocalize, expression: "scheduleLocalize"}], staticClass: "ui-link", attrs: {type: "button", disabled: _vm.disabled}, on: {click: _vm.schedule}}) : _vm._e(), _vm.inline ? _c("div", {staticClass: "ui-daterangepicker-inline"}, [_c("div", {staticClass: "ui-daterangepicker-group"}, [_c("ui-property", {attrs: {vertical: true}}, [_c("ui-datepicker", {attrs: {time: _vm.time}, model: {value: _vm.value.from, callback: function($$v) {
    _vm.$set(_vm.value, "from", $$v);
  }, expression: "value.from"}})], 1)], 1), _vm.rangeEnd ? _c("div", {staticClass: "ui-daterangepicker-group"}, [_c("ui-property", {attrs: {vertical: true}}, [_c("ui-datepicker", {attrs: {time: _vm.time}, model: {value: _vm.value.to, callback: function($$v) {
    _vm.$set(_vm.value, "to", $$v);
  }, expression: "value.to"}})], 1)], 1) : _vm._e()]) : _vm._e()]);
};
var staticRenderFns$u = [];
var daterangepicker_vue_vue_type_style_index_0_lang = ".ui-daterangepicker.is-primary .ui-link {\n  color: var(--color-primary);\n  font-weight: 700;\n  text-decoration-color: var(--color-primary) !important;\n}\n.ui-daterangepicker-inline {\n  display: flex;\n  gap: var(--padding-s);\n}";
const DATETIME_FORMAT = "DD.MM.YY HH:mm";
const DATE_FORMAT = "DD.MM.YY";
const script$u = {
  name: "uiDaterangepicker",
  components: {DaterangepickerOverlay},
  props: {
    value: {
      type: Object,
      default: {
        from: null,
        to: null
      }
    },
    disabled: {
      type: Boolean,
      default: false
    },
    format: {
      type: String,
      default: null
    },
    time: {
      type: Boolean,
      default: false
    },
    maxDate: {
      type: [String, Date],
      default: null
    },
    minDate: {
      type: [String, Date],
      default: null
    },
    fromLabel: {
      type: String,
      default: "@ui.date.range_from"
    },
    toLabel: {
      type: String,
      default: "@ui.date.range_to"
    },
    amPm: {
      type: Boolean,
      default: false
    },
    inline: {
      type: Boolean,
      default: false
    },
    rangeEnd: {
      type: Boolean,
      default: true
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    id: null,
    output: null,
    pickerOptions: {}
  }),
  computed: {
    scheduleLocalize() {
      return {
        key: !this.value || !this.value.from && !this.value.to ? "@ui.date.set" : this.value.from && !this.value.to ? "@ui.date.x" : !this.value.from && this.value.to ? "@ui.date.y" : "@ui.date.xtoy",
        tokens: {
          x: this.value ? Strings.date(this.value.from, this.format || (this.time ? DATETIME_FORMAT : DATE_FORMAT)) : null,
          y: this.value ? Strings.date(this.value.to, this.format || (this.time ? DATETIME_FORMAT : DATE_FORMAT)) : null
        }
      };
    }
  },
  created() {
    this.id = "daterangepicker-" + Strings.guid();
  },
  methods: {
    schedule() {
      Overlay.open({
        component: DaterangepickerOverlay,
        options: {
          format: this.format,
          time: this.time,
          max: this.maxDate,
          min: this.minDate,
          fromText: this.fromLabel,
          toText: this.toLabel,
          amPm: this.amPm,
          rangeEnd: this.rangeEnd
        },
        model: {
          from: this.value.from,
          to: this.value.to
        }
      }).then((res) => {
        this.$emit("change", res);
        this.$emit("input", res);
      }, () => {
      });
    }
  }
};
const __cssModules$u = {};
var component$u = normalizeComponent(script$u, render$u, staticRenderFns$u, false, injectStyles$u, null, null, null);
function injectStyles$u(context) {
  for (let o in __cssModules$u) {
    this[o] = __cssModules$u[o];
  }
}
component$u.options.__file = "app/components/pickers/dateRangePicker/daterangepicker.vue";
var uiDaterangepicker = component$u.exports;
var render$t = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "mediafolder", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: _vm.model.id ? "@media.editfolder" : "@media.addfolder", expression: "model.id ? '@media.editfolder' : '@media.addfolder'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "mediafolder-items"}, [_c("ui-property", {attrs: {required: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.item.name, expression: "item.name"}, {name: "localize", rawName: "v-localize:placeholder", value: "@media.fields.foldername_placeholder", expression: "'@media.fields.foldername_placeholder'", arg: "placeholder"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "80", disabled: _vm.disabled}, domProps: {value: _vm.item.name}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.item, "name", $event.target.value);
    }}}), _c("ui-error", {attrs: {"catch-all": true}})], 1)], 1), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, state: form.state, label: _vm.model.id ? "@ui.save" : "@ui.create"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}}), !_vm.disabled && _vm.model.id ? _c("ui-button", {staticStyle: {float: "right"}, attrs: {type: "light", label: "@ui.delete"}, on: {click: _vm.onDelete}}) : _vm._e()], 1)];
  }}], null, false, 3879353949)}) : _vm._e();
};
var staticRenderFns$t = [];
var folder_vue_vue_type_style_index_0_lang = ".mediafolder {\n  text-align: left;\n}\n.mediafolder-items {\n  margin-top: var(--padding);\n}";
const script$t = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false,
    item: {},
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(!this.model.id ? MediaFolderApi.getEmpty() : MediaFolderApi.getById(this.model.id)).then((response) => {
        this.disabled = false;
        //!response.canEdit;
        this.item = response.entity;
        this.item.parentId = this.model.parentId;
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(MediaFolderApi.save(this.item)).then((response) => {
        this.config.confirm(response, this.config);
      });
    },
    onDelete() {
      this.config.confirm({deleted: true});
    }
  }
};
const __cssModules$t = {};
var component$t = normalizeComponent(script$t, render$t, staticRenderFns$t, false, injectStyles$t, null, null, null);
function injectStyles$t(context) {
  for (let o in __cssModules$t) {
    this[o] = __cssModules$t[o];
  }
}
component$t.options.__file = "app/pages/media/overlays/folder.vue";
var FolderOverlay = component$t.exports;
var render$s = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-media-upload"}, [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "Upload status", expression: "'Upload status'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "ui-media-upload-items"}, _vm._l(_vm.items, function(item2) {
    return _c("button", {staticClass: "ui-media-upload-item", attrs: {type: "button"}}, [item2.isImage ? _c("img", {staticClass: "-preview", attrs: {src: item2.source}}) : _vm._e(), !item2.isImage ? _c("span", {staticClass: "-preview"}, [_c("i", {staticClass: "fth-file"})]) : _vm._e(), _c("p", {staticClass: "ui-media-upload-item-text"}, [_vm._v(" " + _vm._s(item2.name) + " "), _c("span", {staticClass: "-minor"}, [_c("br"), item2.progress < 100 && !item2.error && !item2.success ? _c("span", {staticClass: "ui-media-upload-item-progress"}, [_c("span", {staticClass: "-inner", style: {width: item2.progress + "%"}})]) : _vm._e(), item2.success ? _c("span", [_vm._v("Completed")]) : _vm._e()])])]);
  }), 0), _c("div", {staticClass: "app-confirm-buttons"}, [_c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}})], 1)]);
};
var staticRenderFns$s = [];
var uploadStatus_vue_vue_type_style_index_0_lang = ".ui-media-upload {\n  text-align: left;\n}\n.ui-media-upload {\n  /*height: 200px;\n  background: var(--color-bg-mid);\n  border-radius: var(--radius) var(--radius) 0 0;\n  margin-top: var(--padding);\n  padding: 10px;\n  overflow: hidden;*/\n}\n.ui-media-upload-items {\n  margin-top: 25px;\n  max-height: 495px;\n  overflow-y: auto;\n}\n.ui-media-upload-item {\n  width: 100%;\n  height: 70px;\n  display: grid;\n  grid-template-columns: 70px 1fr;\n  gap: 15px;\n  align-items: center;\n  line-height: 1.4;\n  /*&.is-upload .-preview\n  {\n    background: var(--color-primary);\n    color: var(--color-primary-text);\n  }*/\n}\n.ui-media-upload-item + .ui-media-upload-item {\n  margin-top: 15px;\n}\n.ui-media-upload-item .-preview {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  flex-direction: column;\n  height: 70px;\n  width: 70px;\n  object-fit: cover;\n  background: var(--color-bg-mid);\n  border-radius: var(--radius);\n  position: relative;\n  text-align: center;\n  font-size: 22px;\n}\n.ui-media-upload-item .-extension {\n  font-size: 12px;\n  font-style: normal;\n  margin-top: 8px;\n}\n.ui-media-upload-item .-minor {\n  color: var(--color-text-light);\n  font-size: var(--font-size-s);\n}\n.ui-media-upload-item-progress {\n  display: block;\n  width: 50%;\n  height: 8px;\n  margin-top: 6px;\n  border-radius: 2px;\n  background: var(--color-bg-mid);\n  position: relative;\n}\n.ui-media-upload-item-progress .-inner {\n  position: absolute;\n  left: 0;\n  top: 0;\n  height: 100%;\n  border-radius: 2px;\n  background: var(--color-primary);\n}";
const script$s = {
  props: {
    model: FileList,
    config: Object
  },
  data: () => ({
    loading: false,
    items: []
  }),
  mounted() {
    const files = this.model;
    if (!files || files.length < 1) {
      return;
    }
    for (var i = 0; i < files.length; i++) {
      let file = files[i];
      this.items.push({
        id: "upload:" + Strings.guid(),
        name: file.name,
        size: file.size,
        mimeType: file.type,
        source: null,
        progress: 0,
        file,
        isImage: false,
        success: false,
        error: null
      });
    }
    for (var i = 0; i < this.items.length; i++) {
      let item2 = this.items[i];
      MediaApi.upload(item2.file, this.config.folderId, (progress) => {
        item2.progress = progress;
      }).then((res) => {
        if (res.success) {
          item2.source = res.model.thumbnailSource || res.model.source;
          item2.isImage = res.model.type === "image";
          item2.success = true;
        } else {
          item2.success = false;
        }
      });
    }
  },
  methods: {
    onSubmit() {
    }
  }
};
const __cssModules$s = {};
var component$s = normalizeComponent(script$s, render$s, staticRenderFns$s, false, injectStyles$s, null, null, null);
function injectStyles$s(context) {
  for (let o in __cssModules$s) {
    this[o] = __cssModules$s[o];
  }
}
component$s.options.__file = "app/pages/media/overlays/upload-status.vue";
var UploadStatusOverlay = component$s.exports;
var render$r = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "ui-mediapicker-overlay", scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {staticClass: "ui-headline"}, [_vm._v("Select or upload media")]), _c("ui-search", {staticClass: "ui-mediapicker-overlay-search", model: {value: _vm.query, callback: function($$v) {
      _vm.query = $$v;
    }, expression: "query"}}), _c("nav", {staticClass: "ui-mediapicker-overlay-hierarchy"}, [_c("button", {staticClass: "ui-mediapicker-overlay-hierarchy-item", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.selectFolder(null);
    }}}, [_c("i", {staticClass: "fth-home"})]), _vm._l(_vm.hierarchy, function(item2) {
      return _c("button", {directives: [{name: "localize", rawName: "v-localize", value: item2.name, expression: "item.name"}], staticClass: "ui-mediapicker-overlay-hierarchy-item", attrs: {type: "button"}, on: {click: function($event) {
        return _vm.selectFolder(item2.id);
      }}});
    }), _c("button", {staticClass: "ui-mediapicker-overlay-hierarchy-item", attrs: {type: "button"}, on: {click: _vm.addFolder}}, [_c("i", {staticClass: "fth-plus"})])], 2), _c("div", {staticClass: "ui-mediapicker-overlay-items"}, [_vm.folderId ? _c("div", {staticClass: "ui-mediapicker-overlay-item is-upload"}, [_c("input", {staticClass: "ui-mediapicker-overlay-upload", attrs: {type: "file", multiple: ""}, on: {change: _vm.onUpload}}), _c("span", {staticClass: "-preview"}, [_c("i", {staticClass: "fth-plus"})]), _c("p", {staticClass: "ui-mediapicker-overlay-item-text"}, [_vm._v(" Upload media ")])]) : _vm._e(), _vm._l(_vm.items, function(item2) {
      return _c("button", {staticClass: "ui-mediapicker-overlay-item", attrs: {type: "button"}, on: {click: function($event) {
        return _vm.select(item2);
      }}}, [item2.image ? _c("img", {staticClass: "-preview", attrs: {src: item2.image}}) : _vm._e(), !item2.image ? _c("span", {staticClass: "-preview"}, [_c("i", {class: item2.isFolder ? "fth-folder" : "fth-file"})]) : _vm._e(), _c("p", {staticClass: "ui-mediapicker-overlay-item-text"}, [_vm._v(" " + _vm._s(item2.name) + " "), item2.size ? _c("span", {staticClass: "-minor"}, [_c("br"), _c("span", {directives: [{name: "filesize", rawName: "v-filesize", value: item2.size, expression: "item.size"}]})]) : _vm._e()])]);
    })], 2)];
  }}])});
};
var staticRenderFns$r = [];
var overlay_vue_vue_type_style_index_0_lang$2 = '.ui-mediapicker-overlay {\n  text-align: left;\n}\n.ui-mediapicker-overlay-search.ui-searchinput .ui-input {\n  margin-top: 25px;\n}\n.ui-mediapicker-overlay-hierarchy {\n  text-align: left;\n  margin-top: 20px;\n}\n.ui-mediapicker-overlay-hierarchy-item {\n  line-height: 1.4;\n}\n.ui-mediapicker-overlay-hierarchy-item + .ui-mediapicker-overlay-hierarchy-item:before {\n  content: "/";\n  margin: 0 0.5em;\n  color: var(--color-text-dim);\n}\n.ui-mediapicker-overlay-items {\n  margin-top: 25px;\n  max-height: 495px;\n  overflow-y: auto;\n}\n.ui-mediapicker-overlay-item {\n  width: 100%;\n  height: 70px;\n  display: grid;\n  grid-template-columns: 70px 1fr;\n  gap: 15px;\n  align-items: center;\n  line-height: 1.4;\n}\n.ui-mediapicker-overlay-item + .ui-mediapicker-overlay-item {\n  margin-top: 15px;\n}\n.ui-mediapicker-overlay-item .-preview {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  flex-direction: column;\n  height: 70px;\n  width: 70px;\n  object-fit: cover;\n  background: var(--color-box-nested);\n  border-radius: var(--radius);\n  position: relative;\n  text-align: center;\n  font-size: 20px;\n}\n.ui-mediapicker-overlay-item.is-upload {\n  position: relative;\n}\n.ui-mediapicker-overlay-item .-extension {\n  font-size: 12px;\n  font-style: normal;\n  margin-top: 8px;\n}\n.ui-mediapicker-overlay-item .-minor {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-s);\n}\n.ui-mediapicker-overlay-item-text {\n  white-space: nowrap;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  margin: 0;\n}\ninput[type=file].ui-mediapicker-overlay-upload {\n  position: absolute;\n  height: 100%;\n  top: 0;\n  left: 0;\n  width: 100%;\n  z-index: 1;\n  bottom: 0;\n  opacity: 0.001;\n  cursor: pointer;\n}';
const script$r = {
  props: {
    model: String,
    config: Object
  },
  data: () => ({
    folderId: null,
    icon: null,
    query: "",
    items: [],
    hierarchy: []
  }),
  watch: {
    "config.folderId": function(id) {
      this.folderId = id;
    },
    model() {
    },
    query() {
      this.debouncedSearch();
    }
  },
  created() {
    this.folderId = this.config.folderId;
    this.debouncedSearch = debounce(this.search, 100);
  },
  mounted() {
    this.getItems();
  },
  methods: {
    getItems(query) {
      if (!query) {
        query = {};
      }
      query.folderId = this.folderId;
      query.pageSize = 100;
      this.getFolderHierarchy(query.folderId);
      return MediaApi.getListByQuery(query).then((response) => {
        this.items = response.items;
      });
    },
    getFolderHierarchy(id) {
      MediaFolderApi.getHierarchy(id).then((res) => {
        this.hierarchy = res;
        this.current = res[res.length - 1];
      });
    },
    selectFolder(id) {
      this.folderId = id;
      this.getItems();
    },
    addFolder() {
      Overlay.open({
        component: FolderOverlay,
        model: {parentId: this.folderId},
        theme: "dark"
      }).then(() => {
      }, () => {
        setTimeout(() => {
          this.selectFolder(item.model.id);
        }, 1e3);
      });
    },
    onUpload(event) {
      let options2 = {
        title: "Upload status",
        closeLabel: "@ui.close",
        component: UploadStatusOverlay,
        isCreate: true,
        model: event.target.files,
        folderId: this.folderId,
        theme: "dark",
        width: 520
      };
      return Overlay.open(options2).then((value) => {
        setTimeout(() => {
          this.getItems();
        }, 500);
      }, () => {
      });
    },
    select(item2) {
      if (item2.isFolder) {
        return this.selectFolder(item2.id);
      } else {
        this.config.confirm(item2, this.config);
      }
    }
  }
};
const __cssModules$r = {};
var component$r = normalizeComponent(script$r, render$r, staticRenderFns$r, false, injectStyles$r, null, null, null);
function injectStyles$r(context) {
  for (let o in __cssModules$r) {
    this[o] = __cssModules$r[o];
  }
}
component$r.options.__file = "app/components/pickers/mediaPicker/overlay.vue";
var PickMediaOverlay = component$r.exports;
var render$q = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-mediapicker"}, [_vm.previews.length > 0 ? _c("div", {staticClass: "ui-mediapicker-previews"}, _vm._l(_vm.previews, function(item2) {
    return _c("div", {staticClass: "ui-mediapicker-preview"}, [!item2.error ? _c("div", {staticClass: "ui-mediapicker-preview-image", class: {"media-pattern": item2.thumbnailSource}}, [item2.thumbnailSource ? _c("img", {attrs: {src: item2.thumbnailSource, alt: item2.name}}) : _vm._e(), !_vm.disabled ? _c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.remove", expression: "'@ui.remove'", arg: "title"}], staticClass: "ui-mediapicker-preview-image-delete", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.remove(item2);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-x", size: 12}})], 1) : _vm._e(), !_vm.disabled ? _c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.edit", expression: "'@ui.edit'", arg: "title"}], staticClass: "ui-mediapicker-preview-image-edit", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.edit(item2);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-edit-2", size: 12}})], 1) : _vm._e()]) : _vm._e(), !item2.error ? _c("div", {staticClass: "ui-mediapicker-preview-text"}, [_c("b", {attrs: {title: item2.name}}, [_vm._v(_vm._s(_vm.getFilename(item2.name)))]), _c("span", {staticClass: "is-filesize"}, [_vm._v(_vm._s(_vm.getFileinfo(item2)))])]) : _vm._e(), item2.error ? _c("div", [_c("ui-select-button", {attrs: {icon: "fth-alert-circle color-red", label: "@errors.preview.notfound", description: "@errors.preview.notfound_text", tokens: {id: item2.id}}, on: {click: function($event) {
      return _vm.remove({id: item2.id});
    }}})], 1) : _vm._e()]);
  }), 0) : _vm._e(), _vm.canAdd ? _c("div", {staticClass: "ui-mediapicker-select", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _c("ui-select-button", {attrs: {icon: "fth-plus", label: "@mediapicker.select_text", description: "@mediapicker.select_description", disabled: _vm.disabled}, on: {click: _vm.pick}})], 1) : _vm._e()]);
};
var staticRenderFns$q = [];
var mediapicker_vue_vue_type_style_index_0_lang = ".display-grid .ui-mediapicker-previews {\n  display: flex;\n  gap: 10px;\n}\n.display-grid .ui-mediapicker-previews .ui-mediapicker-preview + .ui-mediapicker-preview {\n  margin-top: 0;\n}\n.ui-mediapicker-previews + .ui-mediapicker-select,\n.ui-mediapicker-preview + .ui-mediapicker-preview {\n  margin-top: 10px;\n}\n.ui-mediapicker:not(.display-grid) .ui-mediapicker-previews + .ui-mediapicker-add {\n  display: flex;\n  flex-direction: column;\n}\n.ui-mediapicker:not(.display-grid) .ui-mediapicker-previews + .ui-mediapicker-add .ui-select-button + .ui-mediapicker-add-upload {\n  margin-left: 0;\n  margin-top: 10px;\n}\n.ui-mediapicker-preview {\n  display: flex;\n  align-items: center;\n}\n.ui-mediapicker-preview-image {\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  width: 80px;\n  height: 80px;\n  /*background: var(--color-bg);*/\n  border: 1px solid var(--color-line);\n  padding: 0;\n  border-radius: var(--radius);\n  color: var(--color-text);\n  position: relative;\n  overflow: hidden;\n}\n.ui-mediapicker-preview-image img {\n  border-radius: 3px;\n  max-width: 100%;\n  max-height: 100%;\n  margin: auto;\n  display: block;\n  color: transprent;\n  overflow: hidden;\n  font-size: 0;\n  position: relative;\n  z-index: 2;\n}\n.ui-mediapicker-preview-image.is-icon {\n  width: 48px;\n  height: 48px;\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  border-radius: var(--radius);\n  background: var(--color-box);\n  color: var(--color-text);\n  text-align: center;\n  font-size: 16px;\n}\n.ui-mediapicker-preview-image:hover .ui-mediapicker-preview-image-delete, .ui-mediapicker-preview-image:hover .ui-mediapicker-preview-image-edit {\n  opacity: 1;\n  transition-delay: 0.1s;\n}\n.display-big .ui-mediapicker-preview-image, .display-grid .ui-mediapicker-preview-image {\n  width: 100px;\n  height: 100px;\n}\n.ui-mediapicker-preview-text {\n  display: flex;\n  flex-direction: column;\n  margin-left: 16px;\n  font-size: var(--font-size);\n}\n.ui-mediapicker-preview-text .is-filesize {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n  font-size: var(--font-size-xs);\n}\n.display-grid .ui-mediapicker-preview-text {\n  display: none;\n}\n.ui-mediapicker-preview-image-delete,\n.ui-mediapicker-preview-image-edit {\n  opacity: 0;\n  transition: opacity 0.15s ease;\n  position: absolute;\n  display: inline-block;\n  right: 3px;\n  bottom: 3px;\n  width: 24px;\n  height: 24px;\n  line-height: 26px;\n  border-radius: 20px;\n  background: var(--color-negative);\n  color: var(--color-primary-text);\n  z-index: 2;\n  text-align: center;\n  font-size: 13px;\n}\n.ui-media.display-default .ui-mediapicker-preview-image-delete,\n.ui-media.display-default .ui-mediapicker-preview-image-edit {\n  width: 20px;\n  height: 20px;\n  line-height: 22px;\n}\n.ui-mediapicker-preview-image-edit {\n  right: 30px;\n  background: var(--color-box);\n  color: var(--color-text);\n}\n.ui-media.display-default .ui-mediapicker-preview-image-edit {\n  right: 24px;\n}";
const TYPES = {
  ALL: "all",
  IMAGE: "image",
  VIDEO: "video",
  DOCUMENT: "document",
  OTHER: "other"
};
const DISPLAY = {
  DEFAULT: "default",
  BIG: "big",
  GRID: "grid"
};
const MAX_FILENAME_LENGTH = 32;
const defaultConfig$2 = {
  limit: 1,
  type: TYPES.ALL,
  display: DISPLAY.DEFAULT,
  disallowSelect: false,
  disallowUpload: false,
  fileExtensions: [],
  maxFileSize: 10
};
const script$q = {
  name: "uiMediapicker",
  props: {
    value: {
      type: [Array, String],
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: {
      type: Object,
      default: () => {
        return defaultConfig$2;
      }
    }
  },
  data: () => ({
    configuration: null,
    previews: []
  }),
  watch: {
    value() {
      this.updatePreviews();
    }
  },
  created() {
    this.configuration = JSON.parse(JSON.stringify(extend(defaultConfig$2, this.config)));
  },
  mounted() {
    this.updatePreviews();
  },
  computed: {
    multiple() {
      return this.configuration.limit > 1;
    },
    canAdd() {
      return !this.disabled && this.configuration.limit - this.previews.length > 0;
    }
  },
  methods: {
    updatePreviews() {
      let ids = [];
      if (typeof this.value === "string") {
        ids = [this.value];
      } else if (isArray(this.value)) {
        ids = this.value;
      }
      this.previews = [];
      if (!ids || ids.length < 1) {
        this.$emit("previews", this.multiple ? [] : null);
        return;
      }
      MediaApi.getByIds(ids).then((res) => {
        each(ids, (id) => {
          let value = res[id];
          if (!value) {
            this.previews.push({
              id,
              error: true
            });
          } else {
            this.previews.push(value);
          }
        });
        this.$emit("previews", this.multiple ? this.previews : this.previews[0]);
      });
    },
    remove(item2) {
      let newValue = this.value;
      if (typeof this.value === "string") {
        newValue = null;
      } else if (isArray(this.value)) {
        const index = this.value.indexOf(item2.id);
        newValue.splice(index, 1);
      }
      this.onChange(newValue);
    },
    getFilename(name) {
      if (name.length < MAX_FILENAME_LENGTH) {
        return name;
      }
      const parts2 = name.split(".");
      const extension = parts2.pop();
      return parts2.join(".").substring(0, MAX_FILENAME_LENGTH - 6) + "..." + extension;
    },
    getFileinfo(item2) {
      if (item2.dimension) {
        return `${Strings.filesize(item2.size)} \u2013 ${item2.dimension.width} \xD7 ${item2.dimension.height}`;
      }
      return Strings.filesize(item2.size);
    },
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    pick() {
      if (this.disabled) {
        return;
      }
      let options2 = extend({
        title: "@iconpicker.title",
        closeLabel: "@ui.close",
        component: PickMediaOverlay,
        model: this.configuration.limit > 1 ? this.value[0] : this.value,
        folderId: null,
        width: 520
      }, typeof this.config === "object" ? this.config : {});
      options2.display = "dialog";
      return Overlay.open(options2).then((value) => {
        let newValue = this.value;
        if (this.multiple) {
          if (!newValue) {
            newValue = [];
          }
          newValue.push(value.id);
        } else {
          newValue = value.id;
        }
        this.onChange(newValue);
        return new Promise((resolve) => resolve(newValue));
      });
    }
  }
};
const __cssModules$q = {};
var component$q = normalizeComponent(script$q, render$q, staticRenderFns$q, false, injectStyles$q, null, null, null);
function injectStyles$q(context) {
  for (let o in __cssModules$q) {
    this[o] = __cssModules$q[o];
  }
}
component$q.options.__file = "app/components/pickers/mediaPicker/mediapicker.vue";
var uiMediapicker = component$q.exports;
var render$p = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "ui-pagepicker-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.config.closeLabel, parent: _vm.config.rootId}, on: {click: _vm.config.hide}})];
  }, proxy: true}])}, [_vm.opened ? _c("div", {staticClass: "ui-box ui-pagepicker-overlay-items"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems, parent: _vm.config.rootId}, on: {select: _vm.onSelect}})], 1) : _vm._e()]);
};
var staticRenderFns$p = [];
var overlay_vue_vue_type_style_index_0_lang$1 = '@charset "UTF-8";\n.ui-pagepicker-overlay content {\n  padding-top: 0;\n}\n.ui-box.ui-pagepicker-overlay-items {\n  margin: 0;\n  padding: 20px 0;\n}\n.ui-box.ui-pagepicker-overlay-items .ui-tree-item.is-selected, .ui-box.ui-pagepicker-overlay-items .ui-tree-item:hover:not(.is-disabled) {\n  background: var(--color-bg-xxlight);\n}\n.ui-box.ui-pagepicker-overlay-items + .ui-box {\n  margin-top: var(--padding);\n}\n.ui-box.ui-pagepicker-overlay-items .ui-tree-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n}\n.ui-box.ui-pagepicker-overlay-items .ui-tree-item.is-selected .ui-tree-item-text {\n  font-weight: bold;\n}';
const script$p = {
  props: {
    model: String,
    config: Object
  },
  data: () => ({
    opened: false
  }),
  computed: {
    disabledIds() {
      return this.config.disabledIds || [];
    }
  },
  mounted() {
    setTimeout(() => this.opened = true, 300);
  },
  methods: {
    onSelect(item2) {
      this.config.confirm(item2);
    },
    getItems(parent) {
      return PageTreeApi.getChildren(parent, this.model).then((res) => {
        res = res.filter((x) => x.id !== "recyclebin");
        res.forEach((item2) => {
          if (item2.id === this.model) {
            item2.isSelected = true;
          }
          if (this.disabledIds.indexOf(item2.id) > -1) {
            item2.disabled = true;
          }
          item2.hasActions = false;
        });
        return res;
      });
    }
  }
};
const __cssModules$p = {};
var component$p = normalizeComponent(script$p, render$p, staticRenderFns$p, false, injectStyles$p, null, null, null);
function injectStyles$p(context) {
  for (let o in __cssModules$p) {
    this[o] = __cssModules$p[o];
  }
}
component$p.options.__file = "app/components/pickers/pagePicker/overlay.vue";
var PageOverlay = component$p.exports;
var render$o = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-pagepicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _vm.previews.length > 0 ? _c("div", {staticClass: "ui-pagepicker-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {staticClass: "ui-pagepicker-preview"}, [_c("ui-select-button", {attrs: {icon: preview.icon, label: preview.name, description: preview.text, disabled: _vm.disabled, tokens: {id: preview.id}}, on: {click: function($event) {
      return _vm.pick(preview.id);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: function($event) {
      return _vm.remove(preview.id);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.canAdd ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.limit > 1 ? "@ui.add" : "@ui.select", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}}) : _vm._e()], 1);
};
var staticRenderFns$o = [];
var pagepicker_vue_vue_type_style_index_0_lang = ".ui-pagepicker-preview {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.ui-pagepicker-preview .ui-icon-button {\n  height: 24px;\n  width: 24px;\n}\n.ui-pagepicker-preview .ui-icon-button i {\n  font-size: 13px;\n}\n.ui-pagepicker-previews + .ui-select-button,\n.ui-pagepicker-preview + .ui-pagepicker-preview {\n  margin-top: 10px;\n}";
const script$o = {
  name: "uiPagepicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    root: {
      type: String,
      default: null
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: []
  }),
  watch: {
    value() {
      this.updatePreviews();
    }
  },
  computed: {
    multiple() {
      return this.limit > 1;
    },
    canAdd() {
      let count = isArray(this.value) ? this.value.length : !this.value ? 0 : 1;
      return !this.disabled && count < this.limit;
    }
  },
  mounted() {
    this.updatePreviews();
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    updatePreviews() {
      if (!this.value || isEmpty(this.value)) {
        this.previews = [];
        return;
      }
      let ids = isArray(this.value) ? this.value : [this.value];
      PagesApi.getPreviews(ids).then((res) => {
        this.previews = res;
      });
    },
    remove(id) {
      if (isArray(this.value)) {
        let index = this.value.indexOf(id);
        this.value.splice(index, 1);
        this.onChange(this.value);
      } else {
        this.onChange(this.limit > 1 ? [] : null);
      }
    },
    pick(id) {
      if (this.disabled) {
        return;
      }
      let disabledIds = [];
      if (!!this.value && !isArray(this.value)) {
        disabledIds = [this.value];
      } else if (isArray(this.value)) {
        disabledIds = this.value;
      }
      let options2 = extend({
        title: "@page.picker.headline",
        closeLabel: "@ui.close",
        component: PageOverlay,
        display: "editor",
        model: this.multiple ? id : this.value,
        rootId: this.root,
        disabledIds
      }, typeof this.options === "object" ? this.options : {});
      return Overlay.open(options2).then((value) => {
        if (this.multiple) {
          if (!this.value || !isArray(this.value)) {
            this.onChange([value.id]);
          } else if (this.value.indexOf(value.id) < 0) {
            if (id) {
              this.remove(id);
            }
            this.value.push(value.id);
            this.onChange(this.value);
          }
        } else {
          this.onChange(value ? value.id : null);
        }
      });
    }
  }
};
const __cssModules$o = {};
var component$o = normalizeComponent(script$o, render$o, staticRenderFns$o, false, injectStyles$o, null, null, null);
function injectStyles$o(context) {
  for (let o in __cssModules$o) {
    this[o] = __cssModules$o[o];
  }
}
component$o.options.__file = "app/components/pickers/pagePicker/pagepicker.vue";
var uiPagepicker = component$o.exports;
var render$n = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-userpicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$n = [];
var userpicker_vue_vue_type_style_index_0_lang = ".ui-userpicker .ui-select-button-icon.is-image, .ui-userpicker .ui-select-button-icon {\n  padding: 0;\n  border-radius: 50px;\n}\n.ui-userpicker .ui-select-button-icon.is-image img, .ui-userpicker .ui-select-button-icon img {\n  border-radius: 50px;\n}";
const script$n = {
  name: "uiUserpicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "user",
      items: UsersApi.getForPicker,
      previews: UsersApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true,
        iconAsImage: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$n = {};
var component$n = normalizeComponent(script$n, render$n, staticRenderFns$n, false, injectStyles$n, null, null, null);
function injectStyles$n(context) {
  for (let o in __cssModules$n) {
    this[o] = __cssModules$n[o];
  }
}
component$n.options.__file = "app/components/pickers/userPicker/userpicker.vue";
var uiUserpicker = component$n.exports;
var render$m = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-mailtemplatespicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$m = [];
const script$m = {
  name: "uiMailtemplatespicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "mailtemplate",
      items: MailTemplatesApi.getForPicker,
      previews: MailTemplatesApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$m = {};
var component$m = normalizeComponent(script$m, render$m, staticRenderFns$m, false, injectStyles$m, null, null, null);
function injectStyles$m(context) {
  for (let o in __cssModules$m) {
    this[o] = __cssModules$m[o];
  }
}
component$m.options.__file = "app/components/pickers/mailPicker/mailpicker.vue";
var uiMailtemplatepicker = component$m.exports;
var render$l = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-spacepicker", class: {"is-disabled": _vm.disabled}}, [_vm.pickerConfig ? _c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}}) : _vm._e()], 1);
};
var staticRenderFns$l = [];
const script$l = {
  name: "uiSpacepicker",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: [],
    pickerConfig: null
  }),
  created() {
    SpacesApi.getAll().then((items) => {
      this.pickerConfig = extend({
        scope: "space",
        keys: {
          id: "alias",
          name: "name",
          description: "description",
          icon: "icon"
        },
        items,
        limit: this.limit,
        multiple: this.limit > 1
      }, this.options);
    });
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
    }
  }
};
const __cssModules$l = {};
var component$l = normalizeComponent(script$l, render$l, staticRenderFns$l, false, injectStyles$l, null, null, null);
function injectStyles$l(context) {
  for (let o in __cssModules$l) {
    this[o] = __cssModules$l[o];
  }
}
component$l.options.__file = "app/components/pickers/spacePicker/spacepicker.vue";
var uiSpacepicker = component$l.exports;
var render$k = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "ui-linkpicker-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.config.closeLabel, parent: _vm.config.rootId}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.confirm"}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_vm.opened ? _c("div", [_c("div", {staticClass: "ui-linkpicker-overlay-area"}, [_c("ui-property", {attrs: {vertical: true}}, [_c("ui-select", {attrs: {items: _vm.areaItems}, model: {value: _vm.current, callback: function($$v) {
    _vm.current = $$v;
  }, expression: "current"}})], 1)], 1), _c("div", {staticClass: "ui-box"}, [_vm.area && _vm.area.component ? _c(_vm.area.component, {tag: "component", attrs: {area: _vm.area}, model: {value: _vm.link, callback: function($$v) {
    _vm.link = $$v;
  }, expression: "link"}}) : _vm._e()], 1)]) : _vm._e()]);
};
var staticRenderFns$k = [];
var overlay_vue_vue_type_style_index_0_lang = "\n.ui-linkpicker-overlay-area\n{\n  margin-bottom: var(--padding-s);\n}\n.ui-linkpicker-overlay .ui-property + .ui-property\n{\n  margin-top: 15px;\n}\n.ui-linkpicker-overlay-area .ui-native-select\n{\n  background: var(--color-box);\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n  font-weight: bold;\n  border-color: transparent;\n}\n.ui-linkpicker-overlay-area .ui-native-select select\n{\n  font-weight: bold;\n}\n.ui-linkpicker-overlay-area .ui-native-select select option\n{\n  font-weight: normal;\n}\n.ui-linkpicker-overlay content\n{\n  padding-top: 0;\n}\n.ui-linkpicker-overlay-options .ui-property\n{\n  display: flex;\n  justify-content: space-between;\n}\n.ui-linkpicker-overlay-options .ui-property + .ui-property\n{\n  margin-top: var(--padding-m);\n}\n.ui-linkpicker-overlay-options .ui-property-content\n{\n  display: inline;\n  flex: 0 0 auto;\n}\n.ui-linkpicker-overlay-options .ui-property-label\n{\n  padding-top: 1px;\n}\n";
const script$k = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    opened: false,
    current: null,
    area: null,
    areas: [],
    areaItems: [],
    link: null,
    template: {
      area: null,
      target: "default",
      urlSuffix: null,
      title: null,
      values: {}
    },
    showOptions: false
  }),
  watch: {
    current() {
      this.reloadSelector();
    }
  },
  mounted() {
    this.areas = this.zero.config.linkPicker.areas;
    this.areaItems = this.areas.map((x) => {
      return {
        key: x.alias,
        value: x.name
      };
    });
    this.link = JSON.parse(JSON.stringify(this.model || this.template));
    if (this.model && this.model.area && this.areas.find((x) => x.alias === this.model.area)) {
      this.area = this.areas.find((x) => x.alias === this.model.area);
    } else {
      this.area = this.areas[0];
    }
    this.current = this.area.alias;
    this.link.area = this.current;
    setTimeout(() => this.opened = true, 300);
  },
  methods: {
    reloadSelector() {
      this.area = this.areas.find((x) => x.alias === this.current);
    },
    onSave() {
      this.link.area = this.current;
      this.config.confirm(this.link);
    },
    onTargetChange(ev) {
      this.link.target = ev ? "blank" : "default";
    }
  }
};
const __cssModules$k = {};
var component$k = normalizeComponent(script$k, render$k, staticRenderFns$k, false, injectStyles$k, null, null, null);
function injectStyles$k(context) {
  for (let o in __cssModules$k) {
    this[o] = __cssModules$k[o];
  }
}
component$k.options.__file = "app/components/pickers/linkPicker/overlay.vue";
var LinkpickerOverlay = component$k.exports;
var render$j = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-linkpicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _vm.previews.length > 0 ? _c("div", {staticClass: "ui-linkpicker-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {staticClass: "ui-linkpicker-preview"}, [_c("ui-select-button", {attrs: {icon: preview.icon, label: preview.name, description: preview.text, disabled: _vm.disabled, tokens: {id: preview.id}}, on: {click: function($event) {
      return _vm.pick(preview.id);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: function($event) {
      return _vm.remove(preview.id);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.canAdd ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.limit > 1 ? "@ui.add" : "@ui.select", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}}) : _vm._e()], 1);
};
var staticRenderFns$j = [];
var linkpicker_vue_vue_type_style_index_0_lang = ".ui-linkpicker-preview {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.ui-linkpicker-preview .ui-icon-button {\n  height: 24px;\n  width: 24px;\n}\n.ui-linkpicker-preview .ui-icon-button i {\n  font-size: 13px;\n}\n.ui-linkpicker-previews + .ui-select-button,\n.ui-linkpicker-preview + .ui-linkpicker-preview {\n  margin-top: 10px;\n}";
const script$j = {
  name: "uiLinkpicker",
  props: {
    value: {
      type: [Object, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    title: {
      type: Boolean,
      default: true
    },
    label: {
      type: Boolean,
      default: false
    },
    target: {
      type: Boolean,
      default: true
    },
    suffix: {
      type: Boolean,
      default: false
    },
    areas: {
      type: Array,
      default: () => []
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: []
  }),
  watch: {
    value() {
      this.updatePreviews();
    }
  },
  computed: {
    multiple() {
      return this.limit > 1;
    },
    canAdd() {
      let count = Array.isArray(this.value) ? this.value.length : !this.value ? 0 : 1;
      return !this.disabled && count < this.limit;
    }
  },
  mounted() {
    this.updatePreviews();
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    updatePreviews() {
      this.previews = [];
      if (!this.value || isEmpty(this.value)) {
        this.$emit("previews", this.multiple ? [] : null);
        this.previews = [];
        return;
      }
      let links = Array.isArray(this.value) ? this.value : [this.value];
      LinksApi.getPreviews(links).then((res) => {
        this.previews = res;
        this.$emit("previews", this.multiple ? this.previews : this.previews[0]);
      });
    },
    remove(id) {
      if (Array.isArray(this.value)) {
        let index = this.value.indexOf(id);
        this.value.splice(index, 1);
        this.onChange(this.value);
      } else {
        this.onChange(this.multiple ? [] : null);
      }
    },
    pick(id) {
      if (this.disabled) {
        return;
      }
      let options2 = extend({
        title: "Select a link",
        closeLabel: "@ui.close",
        component: LinkpickerOverlay,
        display: "editor",
        model: this.multiple ? id : this.value,
        options: {
          limit: this.limit,
          label: this.label,
          title: this.title,
          target: this.target,
          suffix: this.suffix,
          areas: this.areas
        }
      }, typeof this.options === "object" ? this.options : {});
      return Overlay.open(options2).then((res) => {
        if (this.multiple) {
          this.value.push(res);
          this.onChange(this.value);
        } else {
          this.onChange(res);
        }
        return new Promise((resolve) => resolve(res));
      });
    }
  }
};
const __cssModules$j = {};
var component$j = normalizeComponent(script$j, render$j, staticRenderFns$j, false, injectStyles$j, null, null, null);
function injectStyles$j(context) {
  for (let o in __cssModules$j) {
    this[o] = __cssModules$j[o];
  }
}
component$j.options.__file = "app/components/pickers/linkPicker/linkpicker.vue";
var uiLinkpicker = component$j.exports;
var render$i = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-pick", class: {"is-disabled": _vm.disabled, "is-combined": _vm.configuration.preview.combined}}, [_vm.configuration.preview.enabled && _vm.previews.length > 0 && !_vm.configuration.preview.combined ? _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated, enabled: _vm.configuration.sortable}, expression: "{ onUpdate: onSortingUpdated, enabled: configuration.sortable }"}], staticClass: "ui-pick-previews"}, _vm._l(_vm.previews, function(preview) {
    return _c("div", {key: preview[_vm.configuration.keys.id], staticClass: "ui-pick-preview"}, [_c("ui-select-button", {attrs: {icon: _vm.getPreviewIcon(preview), "icon-as-image": _vm.configuration.preview.iconAsImage, label: preview[_vm.configuration.keys.name], description: _vm.getPreviewDescription(preview), disabled: _vm.disabled, tokens: preview}, on: {click: function($event) {
      return _vm.pick(preview[_vm.configuration.keys.id]);
    }}}), !_vm.disabled && _vm.configuration.preview.delete ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close", size: 14}, on: {click: function($event) {
      return _vm.remove(preview[_vm.configuration.keys.id]);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.configuration.preview.enabled && _vm.configuration.preview.combined ? _c("div", {staticClass: "ui-pick-previews"}, [_c("div", {staticClass: "ui-pick-preview"}, [_c("ui-select-button", {attrs: {icon: "fth-check-circle", label: _vm.combinedTitle, disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}})], 1)]) : _vm._e(), _vm.canAdd && _vm.configuration.addButton.enabled && !_vm.configuration.preview.combined ? _c("div", {staticClass: "ui-pick-add"}, [_vm._t("add", [_c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.configuration.addButton.label, disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}})])], 2) : _vm._e(), _c("ui-dropdown", {ref: "overlay", staticClass: "ui-pick-overlay", on: {opened: _vm.overlayOpened}}, [_c("div", {staticClass: "ui-pick-overlay-head"}, [_c("div", {staticClass: "ui-pick-overlay-head-title"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.title, expression: "title"}], staticClass: "-name"}), _vm.configuration.limit > 1 ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: "@ui.pick.max", tokens: {count: _vm.configuration.limit}}, expression: "{ key: '@ui.pick.max', tokens: { count: configuration.limit }}"}], staticClass: "-max"}) : _vm._e()]), _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: _vm.hide}})], 1), _vm.configuration.search.enabled ? _c("ui-search", {ref: "search", staticClass: "ui-pick-overlay-search", attrs: {value: _vm.searchValue}, on: {input: _vm.onSearch, submit: _vm.onSearchSubmit}, scopedSlots: _vm._u([_vm.configuration.autocomplete ? {key: "button", fn: function(search) {
    return [_c("button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.search.button", expression: "'@ui.search.button'", arg: "title"}], staticClass: "ui-searchinput-button", attrs: {type: "button"}, on: {click: search.onSubmit}}, [_c("ui-icon", {attrs: {symbol: "fth-check"}})], 1)];
  }} : null], null, true)}) : _vm._e(), _c("div", {staticClass: "ui-pick-overlay-items"}, _vm._l(_vm.items, function(item2) {
    return _c("button", {key: item2[_vm.configuration.keys.id], staticClass: "ui-pick-overlay-item", class: {"is-selected": _vm.isSelected(item2)}, attrs: {type: "button"}, on: {click: function($event) {
      return _vm.select(item2);
    }}}, [item2[_vm.configuration.keys.icon] ? _c("ui-icon", {staticClass: "-icon", attrs: {symbol: item2[_vm.configuration.keys.icon]}}) : _vm._e(), _c("div", {staticClass: "ui-pick-overlay-item-title"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: item2[_vm.configuration.keys.name], expression: "item[configuration.keys.name]"}], staticClass: "-name"}), item2[_vm.configuration.keys.description] && _vm.configuration.list.description ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item2[_vm.configuration.keys.description], expression: "item[configuration.keys.description]"}], staticClass: "-text"}) : _vm._e()])], 1);
  }), 0), _vm.isLoading || !_vm.configuration.autocomplete && !_vm.items.length ? _c("div", {staticClass: "ui-pick-overlay-center"}, [!_vm.isLoading && !_vm.configuration.autocomplete && !_vm.items.length ? _c("div", {staticClass: "ui-pick-overlay-message"}, [_c("ui-icon", {staticClass: "ui-pick-overlay-message-icon", attrs: {symbol: "fth-list", size: 18}}), _vm._v(" No items found ")], 1) : _vm._e(), _vm.isLoading ? _c("ui-loading") : _vm._e()], 1) : _vm._e()], 1)], 1);
};
var staticRenderFns$i = [];
var pick_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.ui-pick-overlay-search {\n  margin: 15px 15px 5px;\n}\n.ui-pick-overlay-search .ui-input {\n  min-width: 0;\n}\n.ui-pick-overlay .ui-dropdown {\n  min-width: 0;\n  width: 100%;\n  max-width: 380px;\n}\n.ui-pick-overlay-center {\n  display: flex;\n  justify-content: center;\n  margin: 30px 0;\n}\n.ui-pick-overlay-head {\n  padding: 15px 15px 5px;\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.ui-pick-overlay-head .ui-icon-button {\n  background: none;\n}\n.ui-pick-overlay-head-title {\n  font-size: var(--font-size-l);\n  font-weight: 600;\n}\n.ui-pick-overlay-head-title .-max {\n  font-size: var(--font-size-s);\n  font-weight: 400;\n  color: var(--color-text-dim);\n  margin-left: 0.6em;\n}\n.ui-pick-overlay-message {\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  text-align: center;\n  padding: 0 15px;\n}\n.ui-pick-overlay-message-icon {\n  font-size: 18px;\n  margin-bottom: 10px;\n}\n.ui-pick-overlay-items {\n  margin: 15px 0;\n  max-height: 290px;\n  overflow-y: auto;\n}\n.ui-pick-overlay-item {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  padding: 10px 18px;\n  min-height: 48px;\n  width: 100%;\n  border-radius: var(--radius);\n  align-items: center;\n  transition: background 0.2s, transform 0.2s, opacity 0.2s;\n  position: relative;\n}\n.ui-pick-overlay-item:hover {\n  background: var(--color-tree-selected);\n}\n.ui-pick-overlay-item.is-selected {\n  padding-right: 32px;\n}\n.ui-pick-overlay-item.is-selected .-name {\n  font-weight: 600;\n}\n.ui-pick-overlay-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n  position: absolute;\n  right: 20px;\n}\n.ui-pick-overlay-item .-icon {\n  font-size: var(--font-size-l);\n  margin-right: var(--padding-s);\n}\n.ui-pick-overlay-item-title .-name {\n  display: block;\n}\n.ui-pick-overlay-item-title .-text {\n  display: block;\n  color: var(--color-text-dim);\n  font-size: var(--font-size-s);\n  margin-top: 3px;\n}\n.ui-pick-preview {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.ui-pick-preview + .ui-pick-preview,\n.ui-pick-previews + .ui-select-button,\n.ui-pick-previews + .ui-pick-add {\n  margin-top: 10px;\n}\n.ui-pick.is-combined .ui-select-button-label {\n  font-weight: 400;\n}\n.ui-pick-previews .ui-icon-button {\n  height: 24px;\n  width: 24px;\n  flex-shrink: 0;\n}\n.ui-pick-previews .ui-icon-button .ui-button-icon {\n  font-size: 13px;\n}';
const defaultConfig$1 = {
  items: [],
  previews: [],
  excludedIds: [],
  autocomplete: false,
  limit: 10,
  multiple: true,
  sortable: true,
  closeOnClick: true,
  title: null,
  autoOpen: false,
  paging: false,
  keys: {
    id: "id",
    name: "name",
    description: "text",
    icon: "icon"
  },
  addButton: {
    enabled: true,
    label: "@ui.select"
  },
  list: {
    description: false
  },
  preview: {
    enabled: true,
    combined: false,
    combinedTitle: null,
    defaultIcon: "fth-square",
    icon: true,
    delete: true,
    iconAsImage: false,
    description: true
  },
  search: {
    enabled: true,
    local: true,
    setDefaultValue: false,
    focus: false,
    placeholder: null
  }
};
const script$i = {
  name: "uiPick",
  props: {
    value: {
      type: [String, Array],
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: {
      type: Object,
      default: () => {
        return defaultConfig$1;
      }
    }
  },
  watch: {
    config: {
      deep: true,
      handler(newval, oldval) {
        if (JSON.stringify(newval) !== JSON.stringify(oldval)) {
          this.buildConfig();
          this.loaded = false;
        }
      }
    },
    value(val) {
      this.onValueChanged(val);
    }
  },
  computed: {
    multiple() {
      return this.configuration.multiple;
    },
    canAdd() {
      return !this.value && !this.multiple || (!this.value || this.value.length < this.configuration.limit);
    },
    isRemote() {
      return typeof this.configuration.items === "function";
    },
    title() {
      if (!!this.configuration.title)
        return this.configuration.title;
      if (this.configuration.autocomplete)
        return "@ui.pick.title_autocomplete";
      if (this.multiple)
        return "@ui.pick.title_multiple";
      return "@ui.pick.title";
    },
    combinedTitle() {
      let html = this.configuration.preview.combinedTitle ? this.configuration.preview.combinedTitle : "";
      if (this.previews.length > 0) {
        html += ": <b>" + map(this.previews, (p2) => p2[this.configuration.keys.name]).join(", ") + "</b>";
      }
      return html;
    }
  },
  data: () => ({
    configuration: {},
    previews: [],
    allItems: [],
    items: [],
    selected: [],
    loaded: false,
    isLoading: false,
    debouncedUpdate: null,
    searchValue: ""
  }),
  created() {
    this.buildConfig();
    this.debouncedUpdate = debounce(this.loadSuggestions, 300);
    this.onValueChanged(this.value);
  },
  mounted() {
    if (this.configuration.autoOpen) {
      this.pick();
    }
  },
  methods: {
    refresh() {
      this.buildConfig();
      this.loaded = false;
    },
    onValueChanged(val) {
      if (this.multiple) {
        this.selected = isArray(val) && val.length ? clone(val) : [];
      } else {
        this.selected = val ? [val] : [];
      }
      this.loadPreviews();
    },
    buildConfig() {
      var config2 = JSON.parse(JSON.stringify(defaultConfig$1));
      this.configuration = extend(JSON.parse(JSON.stringify(config2)), this.config);
      this.configuration.search = extend(config2.search, this.config.search || {});
      this.configuration.addButton = extend(config2.addButton, this.config.addButton || {});
      this.configuration.preview = extend(config2.preview, this.config.preview || {});
      this.configuration.list = extend(config2.list, this.config.list || {});
      this.configuration.keys = extend(config2.keys, this.config.keys || {});
    },
    overlayOpened() {
      if (!this.loaded) {
        this.load();
      }
      if (this.configuration.search.enabled && this.configuration.search.focus) {
        this.$nextTick(() => this.$refs.search.focus());
      }
    },
    pick() {
      this.$refs.overlay.toggle();
      if (!this.loaded) {
        this.load();
      }
    },
    hide() {
      this.$refs.overlay.hide();
    },
    remove(id) {
      let index = this.selected.indexOf(id);
      this.selected.splice(index, 1);
      this.onChange(this.multiple ? this.selected : null);
    },
    clear() {
      this.selected = [];
      this.onChange(this.multiple ? this.selected : null);
    },
    load() {
      if (this.isLoading) {
        return;
      }
      this.isLoading = true;
      this.allItems = [];
      let onLoaded = (items) => {
        if (this.configuration.paging && typeof items === "object" && !Array.isArray(items)) {
          items = items.items;
        }
        this.allItems = items;
        this.loadSuggestions();
        this.loaded = true;
        this.isLoading = false;
      };
      if (this.isRemote) {
        this.configuration.items().then(onLoaded);
      } else {
        onLoaded(this.configuration.items);
      }
    },
    loadSuggestions() {
      let items = [];
      let search = this.searchValue;
      let handleResult = (res) => {
        if (this.configuration.paging && typeof res === "object" && !Array.isArray(res)) {
          res = res.items;
        }
        if (this.configuration.excludedIds && this.configuration.excludedIds.length) {
          res = filter$1(res, (item2) => this.configuration.excludedIds.indexOf(item2[this.configuration.keys.id]) < 0);
        }
        this.items = res;
      };
      if (!search) {
        items = this.allItems;
      } else {
        if (!this.isRemote || this.configuration.search.local) {
          items = filter$1(this.allItems, (item2) => item2.name.toLowerCase().indexOf(search) > -1);
        } else if (this.isRemote) {
          this.configuration.items(search).then((res) => handleResult(res));
          return;
        }
      }
      handleResult(items);
    },
    loadPreviews() {
      let onLoaded = (items, needsFilter) => {
        if (this.configuration.paging && typeof items === "object" && !Array.isArray(items)) {
          items = items.items;
        }
        if (needsFilter) {
          this.previews = [];
          this.selected.forEach((id) => {
            let res = find(items, (item2) => item2[this.configuration.keys.id] === id);
            if (!res)
              ;
            else {
              this.previews.push(clone(res));
            }
          });
        } else {
          this.previews = items;
        }
      };
      if (this.configuration.autocomplete) {
        onLoaded(map(this.selected, (s) => {
          let item2 = {};
          item2[this.configuration.keys.id] = s;
          item2[this.configuration.keys.name] = s;
          return item2;
        }), false);
      } else {
        let promise = isArray(this.configuration.previews) && this.configuration.previews.length || typeof this.configuration.previews === "function" ? this.configuration.previews : this.configuration.items;
        let isFunc = typeof promise === "function";
        if (isFunc && this.selected.length > 0) {
          promise(this.selected).then(onLoaded);
        } else if (isFunc) {
          onLoaded([]);
        } else {
          onLoaded(promise, true);
        }
      }
    },
    onSearch(value) {
      this.searchValue = value;
      if (!this.isRemote || this.configuration.search.local) {
        this.loadSuggestions();
      } else {
        this.debouncedUpdate();
      }
    },
    onSearchSubmit() {
      let value = this.searchValue.trim();
      let item2 = {};
      item2[this.configuration.keys.id] = value;
      item2[this.configuration.keys.name] = value;
      this.select(item2);
    },
    select(item2) {
      let value = this.configuration.autocomplete ? item2[this.configuration.keys.name] : item2[this.configuration.keys.id];
      if (this.multiple) {
        if (!this.canAdd) {
          if (this.limit > 1) {
            return;
          }
          this.selected = [];
        }
        var index = this.selected.indexOf(value);
        if (index > -1) {
          this.selected.splice(index, 1);
          this.onDeselected(value, index);
        } else {
          this.selected.push(value);
          this.onSelected(value, item2);
        }
        this.onChange(this.selected);
      } else {
        this.selected = [value];
        this.onChange(value);
        this.onSelected(value, item2);
      }
    },
    onSelected(value, item2) {
      this.$emit("select", value, item2);
    },
    onDeselected(value, index) {
      this.$emit("deselect", value, index);
    },
    onChange(value) {
      this.$emit("input", value);
      this.$emit("change", value);
      if (this.configuration.closeOnClick) {
        this.$refs.overlay.hide();
      }
    },
    isSelected(item2) {
      let value = this.configuration.autocomplete ? item2[this.configuration.keys.name] : item2[this.configuration.keys.id];
      return this.selected.indexOf(value) > -1;
    },
    getPreviewIcon(preview) {
      return this.configuration.preview.icon ? preview[this.configuration.keys.icon] || this.configuration.preview.defaultIcon : null;
    },
    getPreviewDescription(preview) {
      return this.configuration.preview.description ? preview[this.configuration.keys.description] : null;
    },
    onSortingUpdated(ev) {
      this.selected = Arrays.move(this.selected, ev.oldIndex, ev.newIndex);
      this.onChange(this.multiple ? this.selected : this.selected[0]);
    }
  }
};
const __cssModules$i = {};
var component$i = normalizeComponent(script$i, render$i, staticRenderFns$i, false, injectStyles$i, null, null, null);
function injectStyles$i(context) {
  for (let o in __cssModules$i) {
    this[o] = __cssModules$i[o];
  }
}
component$i.options.__file = "app/components/pickers/pick.vue";
var uiPick = component$i.exports;
var table = '@charset "UTF-8";\n.ui-table {\n  display: flex;\n  -webkit-box-orient: vertical;\n  -webkit-box-direction: normal;\n  flex-direction: column;\n  position: relative;\n  background: var(--color-table);\n  flex-wrap: nowrap;\n  -webkit-box-pack: justify;\n  justify-content: space-between;\n  min-width: auto;\n  border-radius: var(--radius);\n  table-layout: fixed;\n  word-wrap: break-word;\n  font-size: var(--font-size);\n  width: 100%;\n  box-shadow: var(--shadow-short);\n}\n.ui-table.is-inline {\n  box-shadow: none;\n  border: 1px solid var(--color-line);\n}\n\n.ui-table-row {\n  display: flex;\n  -webkit-box-orient: horizontal;\n  -webkit-box-direction: normal;\n  flex-flow: row nowrap;\n  -webkit-box-align: center;\n  align-items: stretch;\n  width: 100%;\n  border-bottom: 1px solid var(--color-table-line-horizontal);\n  position: relative;\n  transition: outline 0.1s ease, box-shadow 0.1s ease;\n  color: var(--color-text);\n}\n.ui-table-row:last-child {\n  border-bottom: none;\n}\n.ui-table.is-inline .ui-table-row {\n  border-bottom-color: var(--color-line);\n}\n\na.ui-table-row:hover, button.ui-table-row:hover {\n  background: var(--color-table-hover);\n  z-index: 2;\n}\n\n.ui-table-head {\n  font-weight: 700;\n  border-radius: 5px 5px 0 0;\n  color: var(--color-text);\n  position: sticky;\n  top: 0;\n  z-index: 3;\n  background: var(--color-table-head);\n  /*border-bottom: 2px solid var(--color-bg);*/\n}\n.ui-table-head .ui-table-cell {\n  justify-content: space-between;\n  font-size: var(--font-size-s);\n}\n.ui-table.is-inline .ui-table-head {\n  background: var(--color-box);\n}\n\n.ui-table-cell {\n  display: inline-flex;\n  flex-direction: row;\n  justify-content: flex-start;\n  align-items: center;\n  flex: 1 1 5%;\n  position: relative;\n  text-align: left;\n  padding: 15px 20px 14px 20px;\n  border-left: 1px solid var(--color-table-line-vertical);\n  white-space: nowrap;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  min-height: 58px;\n  min-width: 20px;\n  line-height: 20px;\n  color: var(--color-text);\n}\n.ui-table-cell:first-child {\n  border-left: none;\n}\n.ui-table-cell.is-multiline {\n  white-space: normal;\n  overflow: auto;\n  text-overflow: initial;\n}\n.ui-table-cell.is-bold {\n  font-weight: bold;\n}\n.ui-table-cell.is-selectable {\n  font-size: var(--font-size-l);\n  flex: 0 1 40px;\n  text-align: center;\n  padding: 0;\n  justify-content: center;\n}\n\n.ui-table-row:not(.ui-table-head) .ui-table-cell.is-vertical {\n  flex-direction: column;\n  align-items: flex-start;\n  justify-content: center;\n}\n.ui-table-row:not(.ui-table-head) .ui-table-cell.is-name .ui-icon {\n  margin-left: 0.6em;\n  color: var(--color-synchronized);\n}\n\n.ui-table-row:not(.is-selected) .ui-table-cell:not(.is-head).is-selectable i {\n  color: var(--color-text);\n  margin-right: 1px;\n}\n.ui-table-row:not(.is-selected) .ui-table-cell:not(.is-head).is-selectable i:before {\n  content: "\uE8CB";\n}\n\n.ui-table-sort {\n  height: 20px;\n  width: 20px;\n  display: inline-flex;\n  align-items: center;\n  justify-content: center;\n  margin-right: -10px;\n  border-radius: 15px;\n  transition: background 0.2s ease;\n  /*&:hover\n  {\n    background: var(--color-bg);\n  }*/\n}\n.ui-table-sort[disabled] {\n  opacity: 0;\n  visibility: hidden;\n  pointer-events: none;\n}\n.ui-table-sort .arrow {\n  border-top-color: var(--color-text);\n  transition: opacity 0.2s ease, transform 0.3s ease;\n  opacity: 0.2;\n}\n.ui-table-sort:hover .arrow {\n  opacity: 0.5;\n}\n.ui-table-sort.sort-desc .arrow, .ui-table-sort.sort-asc .arrow {\n  opacity: 1;\n}\n.ui-table-sort.sort-asc .arrow {\n  transform: scaleY(-1) translateY(5px);\n}\n\n.ui-table-empty, .ui-table-loading {\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  height: 250px;\n  text-align: center;\n  padding: 0 20px;\n}\n\n.ui-table.is-inline .ui-table-empty,\n.ui-table.is-inline .ui-table-loading {\n  height: auto;\n  padding: var(--padding) 20px;\n}\n\n.ui-table-empty-icon {\n  margin-bottom: 20px;\n}\n\n/* special styling for display types */\n.ui-table-field-bool {\n  color: var(--color-text-dim-one);\n}\n\n.ui-table-field-bool[data-symbol=fth-check] {\n  color: var(--color-primary);\n}\n\n.ui-table-cell[field-type=datetime] {\n  display: inline;\n}\n.ui-table-cell[field-type=datetime] .-minor {\n  color: var(--color-text-dim);\n}\n\n.ui-table-cell .-minor {\n  color: var(--color-text-dim);\n  font-weight: 400;\n  font-size: var(--font-size-xs);\n}\n\n.ui-table-field-image {\n  border-radius: 3px;\n}';
var render$h = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.pages > 1 ? _c("div", {staticClass: "ui-pagination", class: {"is-inline": _vm.inline}}, [_c("ui-icon-button", {staticClass: "ui-pagination-prev ui-pagination-button", attrs: {type: _vm.buttonType, title: "@ui.pagination.previous", icon: "fth-chevron-left", disabled: _vm.page < 2}, on: {click: function($event) {
    return _vm.set(_vm.page - 1);
  }}}), _c("div", {staticClass: "ui-pagination-select"}, [_c("select", {domProps: {value: _vm.page}, on: {change: _vm.selectChanged}}, _vm._l(_vm.values, function(value) {
    return _c("option", {domProps: {value}}, [_vm._v(_vm._s(value))]);
  }), 0), _c("button", {staticClass: "ui-button type-blank caret-down", attrs: {type: "button"}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: "@ui.pagination.xofy", tokens: {x: _vm.page, y: _vm.pages}}, expression: "{ key: '@ui.pagination.xofy', tokens: { x: page, y: pages }}"}], staticClass: "ui-button-text"}), _c("ui-icon", {staticClass: "ui-button-caret", attrs: {symbol: "fth-chevron-down"}})], 1)]), _c("ui-icon-button", {staticClass: "ui-pagination-next ui-pagination-button", attrs: {type: _vm.buttonType, title: "@ui.pagination.next", icon: "fth-chevron-right", disabled: _vm.page >= _vm.pages}, on: {click: function($event) {
    return _vm.set(_vm.page + 1);
  }}})], 1) : _vm._e();
};
var staticRenderFns$h = [];
var pagination_vue_vue_type_style_index_0_lang = ".ui-pagination {\n  display: flex;\n  justify-content: center;\n  margin-top: var(--padding);\n  align-items: center;\n}\n.ui-pagination-select {\n  margin: 0 30px;\n  position: relative;\n}\n.ui-pagination-select .ui-button {\n  padding: 0 2px;\n}\n.ui-pagination-select select {\n  width: 100%;\n  position: absolute;\n  z-index: 1;\n  left: 0;\n  right: 0;\n  top: 0;\n  bottom: 0;\n  opacity: 0;\n}\n.ui-pagination-button[disabled] .ui-button-icon {\n  color: var(--color-text-dim);\n}";
const script$h = {
  name: "uiPagination",
  props: {
    pages: {
      type: Number,
      default: 1
    },
    page: {
      type: Number,
      default: 1
    },
    inline: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    values: function() {
      return Array.apply(null, Array(this.pages)).map(function(_, i) {
        return i + 1;
      });
    },
    buttonType() {
      return this.inline ? "light" : "light onbg";
    }
  },
  methods: {
    set(page) {
      this.$emit("update:page", page);
      this.$emit("change", page);
    },
    selectChanged(ev, a) {
      this.set(+ev.target.value);
    }
  }
};
const __cssModules$h = {};
var component$h = normalizeComponent(script$h, render$h, staticRenderFns$h, false, injectStyles$h, null, null, null);
function injectStyles$h(context) {
  for (let o in __cssModules$h) {
    this[o] = __cssModules$h[o];
  }
}
component$h.options.__file = "app/components/pagination.vue";
var uiPagination = component$h.exports;
function TableValue(el, binding) {
  const item2 = binding.value.item;
  const column = binding.value.column.column;
  const value = item2[column.path];
  if (column.isHtml) {
    el.innerHTML = column.render(value, item2);
  } else {
    el.innerText = column.render(value, item2);
  }
}
var render$g = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-table-outer"}, [_c("div", {staticClass: "ui-table", class: {"is-inline": _vm.inline}}, [_vm._t("top"), _c("header", {staticClass: "ui-table-row ui-table-head"}, [_vm._l(_vm.columns, function(column) {
    return _c("div", {key: column.path, staticClass: "ui-table-cell", class: column.options.class, style: column.flex, attrs: {"table-field": column.path}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: column.label, expression: "column.label", arg: "html"}]}), _c("button", {staticClass: "ui-table-sort", class: _vm.query.orderBy == column.path ? "sort-" + (_vm.query.orderIsDescending ? "desc" : "asc") : null, attrs: {disabled: !column.options.canSort, type: "button"}, on: {click: function($event) {
      return _vm.sort(column);
    }}}, [_c("i", {staticClass: "arrow arrow-down"})])]);
  }), _vm.listConfig.selectable ? _c("button", {staticClass: "ui-table-cell is-head is-selectable", attrs: {type: "button", "table-field": "table_selectable"}, on: {click: function($event) {
    return _vm.select();
  }}}, [_c("span", {staticClass: "ui-native-check"}, [_c("input", {attrs: {type: "checkbox"}, domProps: {checked: _vm.selected.length === _vm.items.length && _vm.selected.length > 0}}), _c("span", {staticClass: "ui-native-check-toggle"})])]) : _vm._e()], 2), _vm._t("topRow"), _vm._l(_vm.items, function(item2, index) {
    return _c(_vm.component, {key: index, tag: "component", staticClass: "ui-table-row", class: {"is-selected": _vm.selected.indexOf(item2) > -1}, attrs: {to: _vm.getLink(item2), type: "button"}, on: {click: function($event) {
      return _vm.onRowClick(item2);
    }}}, [_vm._l(_vm.columns, function(column) {
      return _c("div", {directives: [{name: "table-value", rawName: "v-table-value", value: {column, item: item2}, expression: "{ column, item }"}], key: column.path, staticClass: "ui-table-cell", class: column.options.class, style: column.flex, attrs: {"table-field": column.path, "field-type": column.type}});
    }), _vm.listConfig.selectable ? _c("button", {staticClass: "ui-table-cell is-selectable", attrs: {type: "button", "table-field": "table_selectable"}, on: {click: function($event) {
      $event.preventDefault();
      $event.stopPropagation();
      return _vm.select(item2);
    }}}, [_c("span", {staticClass: "ui-native-check"}, [_c("input", {attrs: {type: "checkbox"}, domProps: {checked: _vm.selected.indexOf(item2) > -1}}), _c("span", {staticClass: "ui-native-check-toggle"})])]) : _vm._e()], 2);
  }), !_vm.isLoading && _vm.items.length < 1 ? _c("div", {staticClass: "ui-table-empty"}, [_c("ui-icon", {staticClass: "ui-table-empty-icon", attrs: {symbol: "fth-list", size: 38}}), _vm._v(" There are no items to show in this list ")], 1) : _vm._e(), _vm.isLoading ? _c("div", {staticClass: "ui-table-loading"}, [_c("ui-loading")], 1) : _vm._e()], 2), _vm.pages > 1 ? _c("footer", {staticClass: "ui-table-pagination"}, [_c("ui-pagination", {attrs: {pages: _vm.pages, page: _vm.query.page, inline: _vm.inline}, on: {change: _vm.setPage}})], 1) : _vm._e()]);
};
var staticRenderFns$g = [];
const script$g = {
  name: "uiTable",
  props: {
    config: {
      type: [String, Object],
      required: true
    },
    inline: {
      type: Boolean,
      default: false
    }
  },
  directives: {
    "table-value": TableValue
  },
  components: {UiPagination: uiPagination},
  data: () => ({
    loaded: false,
    listConfig: {},
    columns: [],
    query: {},
    filter: null,
    items: [],
    component: "div",
    isLoading: true,
    pages: 1,
    count: 0,
    debouncedUpdate: null,
    selected: []
  }),
  mounted() {
    this.setup();
  },
  watch: {
    "query.search": function(val) {
      if (this.loaded)
        this.onChange();
    },
    $route(to, from) {
      this.setup();
    }
  },
  methods: {
    setup() {
      this.debouncedUpdate = debounce(this.update, 300);
      this.listConfig = typeof this.config === "string" ? this.zero.getList(this.config) : this.config;
      this.columns = this.listConfig.columns.map((column) => {
        return __assign(__assign({}, column), {
          column,
          label: column.options.hideLabel ? null : column.options.label || this.listConfig.templateLabel(column.path),
          flex: column.options.width ? {flex: "0 1 " + column.options.width + "px"} : {}
        });
      });
      this.query = __assign(__assign({}, this.listConfig.query), this.listConfig.queryToParams(this.$route.query));
      this.component = !!this.listConfig.link ? "router-link" : !!this.listConfig.onClick ? "button" : "div";
      this.filter = __assign({}, this.listConfig.filterOptions);
      this.$nextTick(() => {
        this.loaded = true;
        this.load(true);
      });
    },
    load(initial) {
      this.isLoading = true;
      this.listConfig.fetch(this.query).then((result) => {
        this.$emit("loaded", result);
        this.pages = result.totalPages;
        this.count = result.totalItems;
        this.$emit("count", this.count);
        this.isLoading = false;
        this.items = result.items;
        this.selected = [];
        if (!initial) {
          let container = document.querySelector(".app-main");
          if (container) {
            this.$nextTick(() => container.scrollTo({top: 0, behavior: "smooth"}));
          }
        }
      });
    },
    update() {
      if (!this.isLoading) {
        this.load();
      }
    },
    onChange() {
      const query = O__zero_zero_Web_UI_node_modules_qs_lib.stringify(this.listConfig.paramsToQuery(this.query));
      const path = __zero.path + this.$route.path.substring(1) + (query ? "?" + query : "");
      history.replaceState(null, null, path);
      this.debouncedUpdate();
    },
    getLink(item2) {
      if (!this.listConfig.link) {
        return null;
      } else if (typeof this.listConfig.link === "function") {
        return this.listConfig.link(item2);
      }
      return {
        name: this.listConfig.link,
        params: {
          id: item2.id
        }
      };
    },
    onRowClick(item2) {
      if (typeof this.listConfig.onClick === "function") {
        this.listConfig.onClick(item2);
      }
    },
    setPage(index) {
      this.query.page = index;
      this.onChange();
    },
    setFilter(filter2) {
      this.query.filter = filter2;
      this.onChange();
    },
    sort(column) {
      if (this.query.orderBy === column.path && this.query.orderIsDescending) {
        this.query.orderIsDescending = false;
      } else if (this.query.orderBy === column.path) {
        this.query.orderBy = null;
      } else {
        this.query.orderBy = column.path;
        this.query.orderIsDescending = true;
      }
      this.query.page = 1;
      this.onChange();
    },
    select(item2) {
      if (!item2) {
        if (this.selected.length >= this.items.length) {
          this.selected = [];
        } else {
          this.selected = [];
          this.items.forEach((item3) => {
            this.selected.push(item3);
          });
        }
      } else {
        const index = this.selected.indexOf(item2);
        if (index > -1) {
          this.selected.splice(index, 1);
        } else {
          this.selected.push(item2);
        }
      }
      this.$emit("select", this.selected, this);
    }
  }
};
const __cssModules$g = {};
var component$g = normalizeComponent(script$g, render$g, staticRenderFns$g, false, injectStyles$g, null, null, null);
function injectStyles$g(context) {
  for (let o in __cssModules$g) {
    this[o] = __cssModules$g[o];
  }
}
component$g.options.__file = "app/components/tables/table.vue";
var uiTable = component$g.exports;
var render$f = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "ui-table-filter-overlay", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-module-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), !_vm.config.isCreate ? _c("ui-button", {attrs: {type: "light onbg", label: "@ui.remove"}, on: {click: _vm.onRemove}}) : _vm._e(), _c("ui-button", {attrs: {type: "primary", submit: true, label: "@ui.confirm", state: form.state, disabled: _vm.loading || _vm.disabled}})];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-box"}, _vm._l(_vm.fields, function(field, index) {
      return _c("div", {staticClass: "ui-table-filter-overlay-group", class: {"is-open": _vm.activeFilter === index, "has-value": field.hasValue(_vm.model[field.path])}}, [_c("div", {staticClass: "ui-table-filter-overlay-group-head"}, [_c("ui-select-button", {attrs: {label: _vm.getFieldLabel(field.field), icon: field.icon, description: field.preview(_vm.model[field.path])}, on: {click: function($event) {
        return _vm.toggleFilter(field, index);
      }}}), _c("ui-button", {staticClass: "ui-table-filter-overlay-group-clear", attrs: {type: "blank", label: "Clear"}, on: {click: function($event) {
        return _vm.clearFilter(field, index);
      }}})], 1), _c("div", {directives: [{name: "show", rawName: "v-show", value: _vm.activeFilter === index, expression: "activeFilter === index"}], staticClass: "ui-table-filter-overlay-group-content"}, [_c("editor-component", {attrs: {config: field.field, editor: _vm.editor, disabled: _vm.disabled}, on: {input: _vm.onFieldChange}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})], 1)]);
    }), 0) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-box ui-table-filter-overlay-filtername"}, [_c("ui-property", {attrs: {label: "Save as...", description: "You can optionally save this filter for future reference", vertical: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.filterName, expression: "filterName"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "40"}, domProps: {value: _vm.filterName}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.filterName = $event.target.value;
    }}})])], 1) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$f = [];
var tableFilterOverlay_vue_vue_type_style_index_0_lang = ".ui-table-filter-overlay .ui-box + .ui-box {\n  margin-top: var(--padding-s);\n}\n.ui-table-filter-overlay-group:not(.is-open) + .ui-table-filter-overlay-group {\n  margin-top: var(--padding-s);\n  border-top: 1px solid var(--color-line);\n  padding-top: var(--padding-s);\n}\n.ui-table-filter-overlay-group:not(.has-value) .ui-table-filter-overlay-group-clear {\n  display: none;\n}\n.ui-table-filter-overlay-group-head {\n  display: grid;\n  grid-template-columns: minmax(0, 1fr) auto;\n  grid-gap: 20px;\n}\n.ui-table-filter-overlay-group-clear .ui-button-text {\n  font-weight: 400;\n  color: var(--color-text-dim);\n}\n.ui-table-filter-overlay-group-content {\n  background: var(--color-box);\n  margin: var(--padding-s) 0;\n  padding: var(--padding-m) 0;\n  border-top: 1px solid var(--color-line);\n  border-bottom: 1px solid var(--color-line);\n}\n.ui-table-filter-overlay-group-content > .ui-property > .ui-property-label {\n  display: none;\n}\n.ui-table-filter-overlay-filtername .ui-property-content {\n  margin-top: 12px !important;\n}\n.ui-table-filter-overlay .ui-select-button-description {\n  color: var(--color-primary);\n}";
const script$f = {
  props: {
    config: Object
  },
  provide: function() {
    return {
      meta: {},
      disabled: false
    };
  },
  data: () => ({
    loading: true,
    disabled: false,
    model: {},
    template: null,
    editor: null,
    fields: [],
    filterName: null,
    activeFilter: null
  }),
  components: {EditorComponent},
  methods: {
    toggleFilter(filter2, index) {
      this.activeFilter = this.activeFilter === index ? null : index;
    },
    onFieldChange(value) {
    },
    clearFilter(filter2, index) {
      this.model[filter2.path] = JSON.parse(JSON.stringify(this.template[filter2.path]));
    },
    getFieldLabel(field) {
      return field.options.label || this.editor.templateLabel(field.path);
    },
    onLoad() {
      this.loading = true;
      this.template = JSON.parse(JSON.stringify(this.config.template || {}));
      this.model = JSON.parse(JSON.stringify(this.config.model.filter || this.template));
      this.editor = this.config.editor;
      this.filterName = this.config.model.name;
      this.fields = this.editor.fields.map((x) => {
        return __assign({
          field: x,
          path: x.path
        }, x.previewOptions);
      });
      this.loading = false;
    },
    onSubmit() {
      this.config.confirm({
        model: this.model,
        filterName: this.filterName
      });
    },
    onRemove() {
      this.config.confirm({
        remove: true
      });
    }
  }
};
const __cssModules$f = {};
var component$f = normalizeComponent(script$f, render$f, staticRenderFns$f, false, injectStyles$f, null, null, null);
function injectStyles$f(context) {
  for (let o in __cssModules$f) {
    this[o] = __cssModules$f[o];
  }
}
component$f.options.__file = "app/components/tables/table-filter-overlay.vue";
var FilterOverlay = component$f.exports;
var render$e = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.loaded ? _c("div", {staticClass: "ui-table-filter"}, [!_vm.hideSearch ? _c("ui-search", {staticClass: "onbg", model: {value: _vm.attach.query.search, callback: function($$v) {
    _vm.$set(_vm.attach.query, "search", $$v);
  }, expression: "attach.query.search"}}) : _vm._e(), _vm.hasFilter && _vm.storedFilters.length ? _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: _vm.filterLabel, caret: "down"}})];
  }, proxy: true}], null, false, 179972004)}, [_vm._l(_vm.storedFilters, function(filter2, index) {
    return _vm.storedFilters.length ? _c("ui-dropdown-button", {key: index, attrs: {value: filter2, label: filter2.name}, on: {click: _vm.setFilter}}) : _vm._e();
  }), _vm.storedFilters.length ? _c("ui-dropdown-separator") : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@ui.add", icon: "fth-plus"}, on: {click: function($event) {
    return _vm.addOrEditFilter();
  }}}), _vm.currentFilter ? _c("ui-dropdown-button", {attrs: {label: "Edit filter", icon: "fth-edit-2"}, on: {click: function($event) {
    return _vm.addOrEditFilter(_vm.currentFilter.id);
  }}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "Clear filter", icon: "fth-x"}, on: {click: function($event) {
    return _vm.setFilter(null);
  }}})], 2) : _vm._e(), _vm.hasFilter && !_vm.storedFilters.length ? _c("ui-button", {attrs: {type: "light onbg", label: "Filter"}, on: {click: function($event) {
    return _vm.addOrEditFilter();
  }}}) : _vm._e(), _vm.actions && _vm.actions.length > 0 ? _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", icon: "fth-more-horizontal"}})];
  }, proxy: true}], null, false, 204167963)}, _vm._l(_vm.actions, function(action, index) {
    return _c("ui-dropdown-button", {key: index, attrs: {value: action, prevent: action.autoclose === false, label: action.label, icon: action.icon}, on: {click: _vm.onActionClicked}});
  }), 1) : _vm._e()], 1) : _vm._e();
};
var staticRenderFns$e = [];
var tableFilter_vue_vue_type_style_index_0_lang = ".ui-table-filter {\n  display: flex;\n  align-items: center;\n  height: 100%;\n}\n.ui-table-filter > * + * {\n  margin-left: 15px;\n}";
const KEY_PREFIX = "zero.ui-table-filter.";
const script$e = {
  name: "uiTableFilter",
  props: {
    attach: [Object, void 0],
    hideSearch: {
      type: Boolean,
      default: false
    }
  },
  watch: {
    attach: function(value) {
      this.setup();
    }
  },
  data: () => ({
    loaded: false,
    hasFilter: false,
    filterOptions: null,
    storedFilters: [],
    currentFilter: null,
    actions: []
  }),
  created() {
  },
  mounted() {
    this.setup();
  },
  computed: {
    filterLabel() {
      return this.currentFilter ? "Filter: <span>" + this.currentFilter.name + "</span>" : "Filter";
    }
  },
  methods: {
    setup() {
      if (!this.attach) {
        return;
      }
      this.filterOptions = __assign({}, this.attach.filter);
      this.hasFilter = this.filterOptions && this.filterOptions.editor;
      this.storedFilters = this.getStoredFilters();
      this.actions = this.attach.listConfig.actions;
      this.loaded = true;
    },
    onActionClicked(action, opts) {
      action.call(opts);
    },
    getStoredFilters() {
      if (!this.hasFilter) {
        return [];
      }
      return JSON.parse(localStorage.getItem(KEY_PREFIX + this.filterOptions.editor.alias) || "[]");
    },
    setFilter(value, two) {
      if (!value) {
        this.currentFilter = null;
        this.$emit("filter", null);
        this.attach.setFilter(null);
      } else {
        this.currentFilter = JSON.parse(JSON.stringify(value));
        this.currentFilter.filter.id = this.currentFilter.id;
        this.$emit("filter", this.currentFilter.filter);
        this.attach.setFilter(this.currentFilter.filter);
      }
    },
    removeFilter(id) {
      let savedFilter = this.storedFilters.find((x) => x.id === id);
      this.storedFilters.splice(this.storedFilters.indexOf(savedFilter), 1);
      localStorage.setItem(KEY_PREFIX + this.filterOptions.editor.alias, JSON.stringify(this.storedFilters));
      if (this.currentFilter && this.currentFilter.id === id) {
        this.setFilter(null);
      }
    },
    saveFilter(name, filter2, id) {
      id = id || Strings.guid(4);
      let savedFilter = this.storedFilters.find((x) => x.id === id);
      let index = this.storedFilters.indexOf(savedFilter);
      let model = {id, name, filter: filter2};
      if (index > -1) {
        this.storedFilters.splice(index, 1, model);
      } else {
        this.storedFilters.push(model);
      }
      localStorage.setItem(KEY_PREFIX + this.filterOptions.editor.alias, JSON.stringify(this.storedFilters));
      return id;
    },
    addOrEditFilter(id) {
      let model = {name: null, filter: this.filterOptions.template};
      if (id) {
        model = this.storedFilters.find((x) => x.id === id);
      }
      return Overlay.open({
        component: FilterOverlay,
        display: "editor",
        title: "Filter",
        editor: this.filterOptions.editor,
        template: this.filterOptions.template,
        model,
        isCreate: !id
      }).then((value) => {
        if (value.remove && id) {
          this.removeFilter(id);
          return;
        }
        if (value.filterName) {
          id = this.saveFilter(value.filterName, value.model, id);
        }
        this.setFilter({
          id,
          name: value.filterName,
          filter: value.model
        });
      });
    }
  }
};
const __cssModules$e = {};
var component$e = normalizeComponent(script$e, render$e, staticRenderFns$e, false, injectStyles$e, null, null, null);
function injectStyles$e(context) {
  for (let o in __cssModules$e) {
    this[o] = __cssModules$e[o];
  }
}
component$e.options.__file = "app/components/tables/table-filter.vue";
var uiTableFilter = component$e.exports;
var render$d = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-tabs"}, [_c("div", {staticClass: "ui-tabs-list", attrs: {role: "tablist"}}, _vm._l(_vm.tabs, function(tab, index) {
    return _c("button", {key: index, staticClass: "ui-tabs-list-item", class: {"is-active": tab.active, "has-errors": tab.hasErrors}, attrs: {type: "button", disabled: tab.disabled, "aria-selected": tab.active, role: "tab"}, on: {click: function($event) {
      return _vm.select(index);
    }}}, [tab.hasErrors ? _c("ui-icon", {staticClass: "ui-tabs-list-item-error", attrs: {size: 16, symbol: "fth-alert-circle"}}) : _vm._e(), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: tab.label, expression: "tab.label"}]}), tab.countOutput > 0 ? _c("i", {staticClass: "ui-tabs-list-item-count"}, [_vm._v(_vm._s(tab.countOutput))]) : _vm._e()], 1);
  }), 0), _c("div", {staticClass: "ui-tabs-items"}, [_vm._t("default")], 2)]);
};
var staticRenderFns$d = [];
var tabs_vue_vue_type_style_index_0_lang = '.ui-header-bar + .ui-tabs > .ui-tabs-list {\n  padding-top: 0;\n}\n.ui-tabs-list {\n  padding: var(--padding) var(--padding) 0;\n  margin-bottom: calc(var(--padding) * -1);\n  height: 58px;\n  display: flex;\n}\n\n/*.ui-tabs-items > .ui-tab:first-child.ui-box:not(.is-blank)\n{\n  border-top-left-radius: 0;\n}*/\n.ui-tabs-items {\n  position: relative;\n  z-index: 1;\n}\n.ui-tabs-list-item {\n  display: inline-flex;\n  align-items: center;\n  height: 58px;\n  padding: 0 var(--padding);\n  font-size: var(--font-size);\n  color: var(--color-text);\n  position: relative;\n  transition: color 0.2s ease;\n  border-radius: var(--radius) var(--radius) 0 0;\n  background: var(--color-box-light);\n}\n.ui-tabs-list-item + .ui-tabs-list-item {\n  margin-left: 4px;\n}\n.ui-tabs-list-item:hover {\n  color: var(--color-text);\n}\n.ui-tabs-list-item[disabled] {\n  cursor: default;\n  color: var(--color-text-dim);\n}\n.ui-tabs-list-item.is-active {\n  font-weight: 700;\n  color: var(--color-text);\n  background: var(--color-box);\n  box-shadow: var(--shadow-short);\n}\n.ui-tabs-list-item.is-active .ui-tabs-list-item-count {\n  background: var(--color-box-light);\n}\n.ui-tabs-list-item.has-errors {\n  color: var(--color-accent-error);\n}\n.ui-tabs-list-item.has-errors.is-active {\n  color: var(--color-accent-error);\n}\n.ui-tabs-list-item.has-errors.is-active:before {\n  background: var(--color-accent-error);\n}\n.ui-tabs-list-item:first-child:after {\n  content: "";\n  position: absolute;\n  left: 0;\n  width: 30px;\n  bottom: -30px;\n  height: 30px;\n  background: var(--color-box-light);\n  z-index: 0;\n}\n.ui-tabs-list-item-count {\n  font-style: normal;\n  font-size: 12px;\n  overflow: hidden;\n  float: right;\n  padding: 2px 6px;\n  background: var(--color-box);\n  border-radius: 10px;\n  margin-left: 8px;\n  margin-right: -4px;\n  margin-top: -1px;\n  font-weight: bold;\n  color: var(--color-text);\n}\n.ui-tabs-list-item-error {\n  display: inline-block;\n  float: left;\n  margin-right: 6px;\n  margin-left: -4px;\n  position: relative;\n  margin-top: -4px;\n  top: 1px;\n}\n.ui-tab.ui-box:first-child {\n  border-top-left-radius: 0;\n}';
const script$d = {
  name: "uiTabs",
  props: {
    cache: {
      type: String
    },
    active: {
      type: Number,
      default: 0
    }
  },
  data: () => ({
    storageKey: null,
    tabs: []
  }),
  mounted() {
    this.cacheKey = this.cache ? `zero.ui-tabs.cache.${this.cache}` : null;
    this.tabs = this.$children;
    if (this.cache) {
      const cachedActiveTab = localStorage.getItem(this.cacheKey);
      if (cachedActiveTab !== null) {
        this.select(+cachedActiveTab);
        return;
      }
    }
    this.select(this.active);
  },
  methods: {
    select(index, ev) {
      if (ev) {
        ev.preventDefault();
      }
      const currentTab = this.tabs[index];
      if (!currentTab || currentTab.disabled) {
        return;
      }
      this.tabs.forEach((tab, tabIndex) => {
        tab.active = index === tabIndex;
      });
      if (this.cache) {
        localStorage.setItem(this.cacheKey, index);
      }
    }
  }
};
const __cssModules$d = {};
var component$d = normalizeComponent(script$d, render$d, staticRenderFns$d, false, injectStyles$d, null, null, null);
function injectStyles$d(context) {
  for (let o in __cssModules$d) {
    this[o] = __cssModules$d[o];
  }
}
component$d.options.__file = "app/components/tabs/tabs.vue";
var uiTabs = component$d.exports;
var render$c = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("section", {directives: [{name: "show", rawName: "v-show", value: _vm.active, expression: "active"}], staticClass: "ui-tab", attrs: {"aria-hidden": !_vm.active, role: "tabpanel"}}, [_vm._t("default")], 2);
};
var staticRenderFns$c = [];
const script$c = {
  name: "uiTab",
  props: {
    label: {
      type: String
    },
    count: {
      type: Number,
      default: 0
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    id: null,
    loaded: false,
    active: false,
    hasErrors: false,
    countOutput: 0
  }),
  watch: {
    active(val) {
      this.loaded = true;
    },
    count(val) {
      this.countOutput = val;
    }
  },
  created() {
    this.id = Strings.guid();
    this.loaded = this.active;
    this.countOutput = this.count;
  },
  methods: {
    setCount(count) {
      this.countOutput = count;
    },
    setErrors(errors, append) {
      this.hasErrors = !!errors;
    },
    clearErrors() {
      this.hasErrors = false;
    }
  }
};
const __cssModules$c = {};
var component$c = normalizeComponent(script$c, render$c, staticRenderFns$c, false, injectStyles$c, null, null, null);
function injectStyles$c(context) {
  for (let o in __cssModules$c) {
    this[o] = __cssModules$c[o];
  }
}
component$c.options.__file = "app/components/tabs/tab.vue";
var uiTab = component$c.exports;
var render$b = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-inline-tabs"}, [_c("nav", {staticClass: "ui-inline-tabs-list", attrs: {role: "tablist"}}, _vm._l(_vm.tabs, function(tab, index) {
    return _c("button", {key: index, staticClass: "ui-inline-tabs-list-item", class: {"is-active": tab.active, "has-errors": tab.error}, attrs: {type: "button", disabled: tab.disabled, "aria-selected": tab.active, role: "tab"}, on: {click: function($event) {
      return _vm.select(index);
    }}}, [tab.error ? _c("i", {staticClass: "ui-inline-tabs-list-item-error fth-alert-circle"}) : _vm._e(), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: tab.label, expression: "tab.label"}]}), _vm.forceCount || tab.count > 0 ? _c("i", {staticClass: "ui-inline-tabs-list-item-count"}, [_vm._v(_vm._s(tab.count))]) : _vm._e()]);
  }), 0), _c("div", {staticClass: "ui-inline-tabs-items"}, [_vm._t("default")], 2)]);
};
var staticRenderFns$b = [];
var inlineTabs_vue_vue_type_style_index_0_lang = ".ui-inline-tabs-list {\n  display: flex;\n  flex-wrap: wrap;\n  gap: 16px;\n  margin-bottom: var(--padding);\n  margin-left: -2px;\n}\n.ui-inline-tabs-list-item {\n  display: inline-flex;\n  align-items: center;\n  padding: 6px 12px;\n  background: transparent;\n  border-radius: 30px;\n  font-size: var(--font-size);\n}\n.ui-inline-tabs-list-item .ui-inline-tabs-list-item-count {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  width: 20px;\n  height: 20px;\n  border-radius: 10px;\n  background: var(--color-button-light);\n  margin-left: 12px;\n  margin-right: -6px;\n  font-size: 11px;\n  line-height: 1;\n  color: var(--color-text-dim);\n  font-style: normal;\n}\n.ui-inline-tabs-list-item.is-active {\n  background: var(--color-button-light);\n  font-weight: 600;\n}\n.ui-inline-tabs-list-item.is-active .ui-inline-tabs-list-item-count {\n  background: var(--color-box);\n  color: var(--color-text);\n}\n.ui-inline-tabs-list-item-error {\n  display: inline-block;\n  float: left;\n  font-size: 16px;\n  margin-right: 6px;\n  margin-left: -4px;\n  position: relative;\n  margin-top: -4px;\n  top: 1px;\n}";
const script$b = {
  name: "uiTabs",
  props: {
    cache: {
      type: String
    },
    active: {
      type: Number,
      default: 0
    },
    forceCount: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    storageKey: null,
    tabs: []
  }),
  created() {
    this.cacheKey = this.cache ? `zero.ui-inline-tabs.cache.${this.cache}` : null;
    this.tabs = this.$children;
  },
  mounted() {
    if (this.cache) {
      const cachedActiveTab = localStorage.getItem(this.cacheKey);
      if (cachedActiveTab !== null) {
        this.select(+cachedActiveTab);
        return;
      }
    }
    this.select(this.active);
  },
  methods: {
    select(index, ev) {
      if (ev) {
        ev.preventDefault();
      }
      const currentTab = this.tabs[index];
      if (currentTab.disabled) {
        return;
      }
      this.tabs.forEach((tab, tabIndex) => {
        tab.active = index === tabIndex;
      });
      if (this.cache) {
        localStorage.setItem(this.cacheKey, index);
      }
    }
  }
};
const __cssModules$b = {};
var component$b = normalizeComponent(script$b, render$b, staticRenderFns$b, false, injectStyles$b, null, null, null);
function injectStyles$b(context) {
  for (let o in __cssModules$b) {
    this[o] = __cssModules$b[o];
  }
}
component$b.options.__file = "app/components/tabs/inline-tabs.vue";
var uiInlineTabs = component$b.exports;
var render$a = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-tree"}, [_vm.header ? _c("ui-header-bar", {staticClass: "ui-tree-header", attrs: {title: _vm.header, "back-button": false}}, [_c("ui-dot-button", {on: {click: function($event) {
    return _vm.onActionsClicked(null, $event);
  }}})], 1) : _vm._e(), _vm._t("default"), _vm.status === "loading" ? _c("span", {staticClass: "ui-tree-item-loading"}, [_c("i")]) : _vm._e(), _vm._l(_vm.items, function(item2) {
    return [_c("ui-tree-item", {attrs: {value: item2, "active-id": _vm.active, depth: _vm.depth, selected: _vm.selection.indexOf(item2.id) > -1}, on: {rightclick: _vm.onRightClicked, click: function($event) {
      return _vm.onSelect(item2, $event);
    }, actions: _vm.onActionsClicked, open: _vm.toggle, setactive: _vm.onActiveSet}}), item2.hasChildren && item2.isOpen && _vm.status != "loading" ? _c("ui-tree", _vm._b({on: {select: _vm.onChildSelect, setactive: _vm.onActiveSet}, scopedSlots: _vm._u([{key: "actions", fn: function(props) {
      return [_vm._t("actions", null, null, props)];
    }}], null, true)}, "ui-tree", {get: _vm.get, parent: item2.id, depth: _vm.depth + 1, active: _vm.active, mode: _vm.mode, selection: _vm.selection, selectionLimit: _vm.selectionLimit}, false)) : _vm._e()];
  }), _vm._t("bottom"), _c("ui-dropdown", {ref: "dropdown", staticClass: "ui-tree-dropdown", attrs: {align: "top", theme: "dark"}}, [_vm._t("actions", null, null, _vm.actionProps)], 2)], 2);
};
var staticRenderFns$a = [];
var tree_vue_vue_type_style_index_0_lang = ".ui-tree {\n  position: relative;\n  overflow-x: hidden;\n}\n.ui-tree-header + .ui-tree-item {\n  margin-top: -18px;\n}\n.ui-tree-dropdown .ui-dropdown {\n  position: fixed;\n  min-width: 200px;\n}";
const script$a = {
  name: "uiTree",
  props: {
    depth: {
      type: Number,
      default: 0
    },
    active: {
      type: String,
      default: null
    },
    parent: {
      type: String,
      default: null
    },
    header: {
      type: String,
      default: null
    },
    get: {
      type: Function,
      required: true
    },
    hasActions: {
      type: Function,
      default: null
    },
    mode: {
      type: String,
      default: "link"
    },
    selection: {
      type: Array,
      default: () => []
    },
    selectionLimit: {
      type: Number,
      default: 1
    }
  },
  data: () => ({
    items: [],
    status: "none",
    actionProps: {
      item: null
    }
  }),
  computed: {
    actionsDefined() {
      return this.$scopedSlots.hasOwnProperty("actions");
    }
  },
  mounted() {
    this.refresh();
  },
  methods: {
    onActiveSet(val) {
      this.$emit("setactive", val);
    },
    refresh() {
      this.load(this.parent);
    },
    load(parent) {
      this.setStatus("loading", this.items);
      let promise = this.get(parent, this.active);
      promise.then((response) => {
        this.items = response;
        this.setStatus("loaded", this.items);
      }).catch((error) => {
        console.error(error);
        this.setStatus("error", this.items, error);
      });
    },
    setStatus(status) {
      this.status = status;
      this.$emit("onStatusChange", status);
    },
    toggle(item2) {
      item2.isOpen = !item2.isOpen;
    },
    onSelect(item2, ev) {
      if (this.mode === "select") {
        let index = this.selection.indexOf(item2.id);
        if (index > -1) {
          this.selection.splice(index, 1);
        } else if (this.selectionLimit === 1) {
          this.selection.splice(0, this.selection.length);
          this.selection.push(item2.id);
        } else if (this.selection.length < this.selectionLimit) {
          this.selection.push(item2.id);
        }
        this.$emit("select", this.selectionLimit > 1 ? this.selection : this.selection.length > 0 ? this.selection[0] : null, ev);
      } else {
        this.$emit("select", item2, ev);
      }
    },
    onChildSelect(item2, ev) {
      this.$emit("select", item2, ev);
    },
    onRightClicked(item2, ev) {
      if (this.actionsDefined && (!item2 || item2.hasActions)) {
        ev.preventDefault();
        this.onActionsClicked(item2, ev);
      }
    },
    onActionsClicked(item2, ev) {
      let dropdown = this.$refs.dropdown;
      if (!this.actionsDefined || item2 && !item2.hasActions || typeof this.hasActions === "function" && !this.hasActions(item2)) {
        return;
      }
      this.actionProps.item = item2;
      this.actionProps.event = ev;
      dropdown.toggle();
      if (!dropdown.open) {
        return;
      }
      this.$nextTick(() => {
        let target = ev.target;
        do {
          if (target.classList.contains("ui-tree-item") || target.classList.contains("ui-tree-header")) {
            break;
          }
        } while (target = target.parentElement);
        target = target.querySelector(".ui-dot-button");
        var rect = target.getBoundingClientRect();
        var width = 240;
        var position = {
          x: rect.left - width + rect.width,
          y: rect.top + rect.height
        };
        let element = dropdown.$el.querySelector(".ui-dropdown");
        element.style.top = position.y + "px";
        element.style.left = position.x + "px";
        element.style.width = width + "px";
      });
    }
  }
};
const __cssModules$a = {};
var component$a = normalizeComponent(script$a, render$a, staticRenderFns$a, false, injectStyles$a, null, null, null);
function injectStyles$a(context) {
  for (let o in __cssModules$a) {
    this[o] = __cssModules$a[o];
  }
}
component$a.options.__file = "app/components/tree/tree.vue";
var uiTree = component$a.exports;
var render$9 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-tree-item", class: _vm.getClasses(_vm.value), on: {contextmenu: function($event) {
    return _vm.onRightClicked(_vm.value, $event);
  }}}, [_vm.value.hasChildren ? _c("button", {staticClass: "ui-tree-item-toggle", style: {width: _vm.depth * 15 + 32 + "px"}, attrs: {disabled: _vm.value.disabled, type: "button"}, on: {click: function($event) {
    return _vm.toggle(_vm.value);
  }}}, [_c("ui-icon", {staticClass: "ui-tree-item-arrow", attrs: {symbol: "fth-chevron-" + (_vm.value.isOpen ? "up" : "down"), size: 14}})], 1) : _vm._e(), !_vm.value.hasChildren ? _c("span", {staticClass: "ui-tree-item-toggle", style: {width: _vm.depth * 15 + 32 + "px"}}) : _vm._e(), _c(_vm.tag, {tag: "component", staticClass: "ui-tree-item-link", attrs: {disabled: _vm.value.disabled, type: "button", to: _vm.value.url}, on: {click: function($event) {
    return _vm.onClick(_vm.value, $event);
  }}}, [_vm.value.icon ? _c("ui-icon", {staticClass: "ui-tree-item-icon", class: {"is-dashed": _vm.value.isDashed}, attrs: {symbol: _vm.value.icon, size: 18}}) : _vm._e(), _vm.value.modifier ? _c("ui-icon", {staticClass: "ui-tree-item-modifier", class: _vm.modifierClass, attrs: {title: _vm.value.modifier.name, symbol: _vm.modifier, size: 10, stroke: 2.5}}) : _vm._e(), _c("span", {staticClass: "ui-tree-item-text"}, [_c("ui-localize", {attrs: {value: _vm.value.name}}), _vm.value.description ? _c("span", {staticClass: "ui-tree-item-description"}, [_c("br"), _c("ui-localize", {attrs: {value: _vm.value.description}})], 1) : _vm._e()], 1)], 1), _vm.value.hasActions ? _c("ui-dot-button", {staticClass: "ui-tree-item-actions", attrs: {disabled: _vm.value.disabled}, on: {click: function($event) {
    return _vm.onActionsClicked(_vm.value, $event);
  }}}) : _vm._e(), _vm.value.countOutput != null ? _c("span", {staticClass: "ui-tree-item-count"}, [_vm._v(_vm._s(_vm.value.countOutput))]) : _vm._e()], 1);
};
var staticRenderFns$9 = [];
var treeItem_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.ui-tree-item {\n  display: grid;\n  grid-template-columns: auto 1fr auto auto;\n  align-items: center;\n  font-size: var(--font-size);\n  padding: 0 var(--padding) 0 0;\n  height: 54px;\n  color: var(--color-text);\n  position: relative;\n  transition: color 0.2s ease;\n  position: relative;\n}\n.ui-tree-item:hover > .ui-tree-item-actions {\n  opacity: 1;\n}\n.ui-tree-item.is-disabled {\n  cursor: not-allowed;\n  opacity: 0.5;\n}\n.ui-tree-item.is-active:before, .ui-tree-item.is-selected:before, .ui-tree-item:hover:before {\n  content: " ";\n  position: absolute;\n  top: 0;\n  bottom: 0;\n  left: 0;\n  right: 0;\n  background: var(--color-tree-selected);\n}\n.ui-tree-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n  z-index: 2;\n}\n.ui-tree-item.is-selected .ui-tree-item-text {\n  font-weight: bold;\n}\n.ui-tree-item-link {\n  display: grid;\n  grid-template-columns: 28px 1fr auto;\n  gap: 6px;\n  height: 100%;\n  align-items: center;\n  position: relative;\n  color: var(--color-text);\n}\n.ui-tree-item-link:hover {\n  color: var(--color-text);\n}\n.ui-tree-item-link.is-active {\n  color: var(--color-text);\n  font-weight: bold;\n}\n.ui-tree-item-text {\n  display: block;\n  overflow: hidden;\n  white-space: nowrap;\n  text-overflow: ellipsis;\n}\n.ui-tree-item-description {\n  color: var(--color-text-dim);\n  font-size: var(--font-size-xs);\n}\n.ui-tree-item-toggle {\n  color: var(--color-text-dim);\n  height: 100%;\n  text-align: right;\n  padding-right: 4px;\n  transition: color 0.2s ease;\n  z-index: 1;\n}\n.ui-tree-item-toggle:hover {\n  color: var(--color-text);\n}\n.ui-tree-item-icon {\n  position: relative;\n  top: -2px;\n  color: var(--color-text-dim);\n  transition: color 0.2s ease;\n}\n.ui-tree-item-icon.is-dashed {\n  stroke-dasharray: 3.5px;\n}\n.ui-tree-item:hover .ui-tree-item-icon {\n  color: var(--color-text);\n}\n.ui-tree-item.is-active .ui-tree-item-icon {\n  color: var(--color-text);\n}\n.ui-tree-item-loading {\n  display: block;\n  overflow: hidden;\n  position: absolute;\n  left: 0;\n  right: 0;\n  height: 2px;\n}\n.ui-tree-item-loading i {\n  background-color: var(--color-bg-shade-4);\n  transform: translateX(-100%) scaleX(1);\n  animation: treeitemloading 1s linear infinite;\n  width: 100%;\n  height: 100%;\n  position: absolute;\n  left: 0;\n  top: 0;\n}\n@keyframes treeitemloading {\n0% {\n    transform: translateX(-100%);\n}\n100% {\n    transform: translateX(100%);\n}\n}\n.ui-tree-item-modifier {\n  position: absolute;\n  left: 10px;\n  bottom: 17px;\n  color: var(--color-text-dim);\n  background: var(--color-tree);\n  border-radius: 50%;\n  padding: 2px;\n  width: 14px;\n  height: 14px;\n  transition: color 0.2s ease;\n}\n.ui-tree-item.is-active .ui-tree-item-modifier, .ui-tree-item:hover .ui-tree-item-modifier {\n  color: var(--color-text);\n}\n.ui-tree-item.is-active .ui-tree-item-modifier {\n  background: var(--color-tree-selected);\n}\n.ui-tree-item-actions {\n  opacity: 0;\n  color: var(--color-text-dim);\n}\n.ui-tree-item-actions:focus {\n  opacity: 1;\n}\n.ui-tree-item-count {\n  display: inline-block;\n  font-size: 11px;\n  font-weight: 400;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  font-style: normal;\n  grid-column: 3;\n  margin-right: -4px;\n  margin-left: 8px;\n}';
const script$9 = {
  name: "uiTreeItem",
  props: {
    value: {
      type: Object,
      required: true
    },
    activeId: {
      type: String,
      default: null
    },
    selected: {
      type: Boolean,
      default: false
    },
    depth: {
      type: Number,
      default: 0
    }
  },
  data: () => ({
    _isActive: false
  }),
  computed: {
    isLink() {
      return this.value && this.value.url;
    },
    tag() {
      return this.isLink ? "router-link" : "button";
    },
    modifier() {
      return this.value && this.value.modifier ? this.value.modifier.icon.split(" ")[0] : null;
    },
    modifierClass() {
      return this.value && this.value.modifier ? this.value.modifier.icon.split(" ")[1] : null;
    },
    isActive() {
      return this.value && this.isLink && (!!this.value.id && this.value.id === this.activeId || this.value.id == this.$route.params.id || this.value.url && !this.value.url.params && this.value.url.name === this.$route.name);
    }
  },
  watch: {
    isActive(val) {
      if (val) {
        this.$emit("setactive", {
          $el: this.$el,
          model: this.value
        });
      }
    }
  },
  mounted() {
    if (this.isActive) {
      this.$emit("setactive", {
        $el: this.$el,
        model: this.value
      });
    }
  },
  methods: {
    onClick(item2, ev) {
      if (this.isLink) {
        return;
      } else {
        this.$emit("click", item2, ev);
      }
    },
    getClasses(item2) {
      return {
        "has-icon": !!item2.icon,
        "has-children": item2.hasChildren,
        "is-inactive": item2.isInactive,
        "is-open": item2.isOpen,
        "is-selected": this.selected || item2.isSelected,
        "is-disabled": item2.disabled,
        "is-active": this.isActive
      };
    },
    toggle(item2) {
      this.$emit("open", item2);
    },
    onRightClicked(item2, ev) {
      if (!item2.disabled) {
        this.$emit("rightclick", item2, ev);
      }
    },
    onActionsClicked(item2, ev) {
      if (!item2.disabled) {
        this.$emit("actions", item2, ev);
      }
    }
  }
};
const __cssModules$9 = {};
var component$9 = normalizeComponent(script$9, render$9, staticRenderFns$9, false, injectStyles$9, null, null, null);
function injectStyles$9(context) {
  for (let o in __cssModules$9) {
    this[o] = __cssModules$9[o];
  }
}
component$9.options.__file = "app/components/tree/tree-item.vue";
var uiTreeItem = component$9.exports;
var render$8 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-datagrid-outer"}, [_c("div", {staticClass: "ui-datagrid"}, [_c("div", {staticClass: "ui-datagrid-items", class: {"is-block": _vm.configuration.block}, style: "grid-template-columns: repeat(auto-fill, minmax(" + _vm.configuration.width + "px, 1fr))"}, [_vm._l(_vm.items, function(item2, index) {
    return _c("div", {key: index, staticClass: "ui-datagrid-item", on: {contextmenu: function($event) {
      return _vm.onRightClicked(item2, $event);
    }}}, [_vm.configuration.selectable && _vm.selected.length > 0 ? _c("button", {staticClass: "ui-datagrid-cell-select", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.select(item2);
    }}}) : _vm._e(), _c(_vm.configuration.component, {tag: "component", staticClass: "ui-datagrid-cell", class: {"is-selected": _vm.configuration.selectable && _vm.selected.indexOf(item2) > -1}, attrs: {value: item2}})], 1);
  }), _vm._t("below")], 2), !_vm.isLoading && _vm.items.length < 1 ? _c("div", {staticClass: "ui-datagrid-empty"}, [_c("i", {staticClass: "ui-datagrid-empty-icon fth-list"}), _vm._v(" There are no items to show in this list ")]) : _vm._e(), _vm.isLoading ? _c("div", {staticClass: "ui-datagrid-loading"}, [_c("ui-loading")], 1) : _vm._e()]), _vm.pages > 1 ? _c("footer", {staticClass: "ui-datagrid-pagination"}, [_c("ui-pagination", {attrs: {pages: _vm.pages, page: _vm.filter.page}, on: {change: _vm.setPage}})], 1) : _vm._e(), _c("ui-dropdown", {ref: "dropdown", staticClass: "ui-datagrid-dropdown", attrs: {align: "top", theme: "dark"}}, [_vm._t("actions", null, null, _vm.actionProps)], 2)], 1);
};
var staticRenderFns$8 = [];
var datagrid_vue_vue_type_style_index_0_lang = "\n.ui-datagrid-items\n{\n  display: grid;\n  gap: var(--padding-s);\n  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));\n  align-items: stretch;\n}\n.ui-datagrid-items.is-block\n{\n  display: block;\n}\n.ui-datagrid-item\n{\n  position: relative;\n}\n.ui-datagrid-empty, .ui-datagrid-loading\n{\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  height: 250px;\n  text-align: center;\n  padding: 0 20px;\n  font-size: var(--font-size);\n}\n.ui-datagrid-empty-icon\n{\n  font-size: 34px;\n  margin-bottom: 20px;\n}\n.ui-datagrid-dropdown .ui-dropdown\n{\n  position: fixed;\n  min-width: 200px;\n}\n.ui-datagrid-cell-select\n{\n  width: 100%;\n  position: absolute;\n  left: 0;\n  top: 0;\n  right: 0;\n  bottom: 0;\n  background: transparent;\n  z-index: 2;\n}\n";
const defaultConfig = {
  order: {
    enabled: true,
    by: "createdDate",
    isDescending: true
  },
  component: null,
  width: 280,
  scrollToTop: true,
  items: null,
  block: false,
  selectable: false
};
const script$8 = {
  name: "uiDatagrid",
  props: {
    value: {
      type: Object,
      required: true,
      default: () => {
        return defaultConfig;
      }
    }
  },
  components: {UiPagination: uiPagination},
  watch: {
    $route: function(val) {
      this.load(true);
    },
    "value.search": function(val) {
      this.filter.search = val;
    },
    "value.items": function(val) {
      this.initialize();
    },
    "filter.search": function(val) {
      this.filter.page = 1;
      this.debouncedUpdate();
    }
  },
  data: () => ({
    configuration: {},
    items: [],
    isLoading: true,
    pages: 1,
    count: 0,
    filter: {
      orderBy: null,
      orderIsDescending: true,
      page: 1,
      pageSize: 30,
      search: null
    },
    debouncedUpdate: null,
    actionProps: {
      item: null
    },
    selected: []
  }),
  computed: {
    actionsDefined() {
      return this.$scopedSlots.hasOwnProperty("actions");
    }
  },
  mounted() {
    this.initialize();
  },
  methods: {
    initialize() {
      this.debouncedUpdate = debounce(this.update, 300);
      this.configuration = extend(JSON.parse(JSON.stringify(defaultConfig)), this.value);
      if (this.configuration.order.enabled) {
        this.filter.orderBy = this.configuration.order.by;
        this.filter.orderIsDescending = this.configuration.order.isDescending;
      }
      this.load(true);
    },
    load(initial) {
      if (initial) {
        this.filter.page = 1;
        this.filter.search = null;
      }
      this.configuration.items(this.filter).then((result) => {
        this.pages = result.totalPages;
        this.count = result.totalItems;
        this.$emit("count", this.count);
        this.isLoading = false;
        this.items = result.items;
        if (!initial && this.configuration.scrollToTop) {
          let container = document.querySelector(".app-main");
          if (container) {
            this.$nextTick(() => container.scrollTo({top: 0, behavior: "smooth"}));
          }
        }
      });
    },
    update() {
      if (!this.isLoading) {
        this.load();
      }
    },
    setPage(index) {
      this.filter.page = index;
      this.debouncedUpdate();
    },
    sort(column) {
      if (this.filter.orderBy === column.field && this.filter.orderIsDescending) {
        this.filter.orderIsDescending = false;
      } else if (this.filter.orderBy === column.field) {
        this.filter.orderBy = null;
      } else {
        this.filter.orderBy = column.field;
        this.filter.orderIsDescending = true;
      }
      this.debouncedUpdate();
    },
    onRightClicked(item2, ev) {
      if (this.actionsDefined) {
        ev.preventDefault();
        this.onActionsClicked(item2, ev);
      }
    },
    onActionsClicked(item2, ev) {
      let dropdown = this.$refs.dropdown;
      if (!this.actionsDefined || typeof this.hasActions === "function" && !this.hasActions(item2)) {
        return;
      }
      this.actionProps.item = item2;
      this.actionProps.event = ev;
      dropdown.toggle();
      if (!dropdown.open) {
        return;
      }
      this.$nextTick(() => {
        let target = ev.target;
        do {
          if (target.classList.contains("ui-datagrid-cell")) {
            break;
          }
        } while (target = target.parentElement);
        var width = 240;
        var position = {
          x: ev.pageX,
          y: ev.pageY
        };
        let element = dropdown.$el.querySelector(".ui-dropdown");
        element.style.top = position.y + "px";
        element.style.left = position.x + "px";
        element.style.width = width + "px";
      });
    },
    select(item2) {
      if (!item2) {
        if (this.selected.length >= this.items.length) {
          this.selected = [];
        } else {
          this.selected = [];
          this.items.forEach((item3) => {
            this.selected.push(item3);
          });
        }
      } else {
        const index = this.selected.indexOf(item2);
        if (index > -1) {
          this.selected.splice(index, 1);
        } else {
          this.selected.push(item2);
        }
      }
      this.$emit("select", this.selected, this);
    },
    clearSelection() {
      this.selected = [];
      this.$emit("select", this.selected, this);
    }
  }
};
const __cssModules$8 = {};
var component$8 = normalizeComponent(script$8, render$8, staticRenderFns$8, false, injectStyles$8, null, null, null);
function injectStyles$8(context) {
  for (let o in __cssModules$8) {
    this[o] = __cssModules$8[o];
  }
}
component$8.options.__file = "app/components/datagrid/datagrid.vue";
var uiDatagrid = component$8.exports;
var render$7 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("p", {staticClass: "ui-message", class: _vm.messageClasses}, [_vm.iconClass ? _c("ui-icon", {staticClass: "ui-message-icon", attrs: {symbol: _vm.iconClass}}) : _vm._e(), _c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: _vm.text, expression: "text", arg: "html"}], staticClass: "ui-message-text"})], 1);
};
var staticRenderFns$7 = [];
var message_vue_vue_type_style_index_0_lang = ".ui-message {\n  font-size: var(--font-size-s);\n  background: var(--color-accent-info-bg);\n  color: var(--color-accent-info);\n  display: inline-grid;\n  padding: 8px 12px 7px 12px;\n  grid-template-columns: auto 1fr;\n  gap: 12px;\n  border-radius: var(--radius);\n  position: relative;\n  line-height: 20px;\n  text-align: left;\n}\n.ui-message.type-warn {\n  background: var(--color-accent-warn-bg);\n  color: var(--color-accent-warn);\n}\n.ui-message.type-error {\n  background: var(--color-accent-error-bg);\n  color: var(--color-accent-error);\n}\n.ui-message.type-success {\n  background: var(--color-accent-success-bg);\n  color: var(--color-accent-success);\n}\n.ui-message.type-neutral {\n  background: var(--color-box-nested);\n  color: var(--color-text);\n}\n.ui-message.type-primary {\n  background: var(--color-primary-low);\n  color: var(--color-primary);\n}\n.ui-message.block {\n  display: grid;\n}\n.ui-message-icon {\n  font-size: 1.3em;\n  position: relative;\n  top: 1px;\n}";
const TYPE_ICONS = {
  neutral: "fth-info",
  info: "fth-info",
  primary: "fth-info",
  warn: "fth-alert-circle",
  error: "fth-alert-circle",
  success: "fth-check-circle"
};
const script$7 = {
  name: "uiMessage",
  props: {
    type: {
      type: String,
      default: "info"
    },
    text: {
      type: String,
      required: true
    },
    icon: {
      type: String,
      default: "-1"
    },
    block: {
      type: Boolean,
      default: true
    }
  },
  computed: {
    iconClass() {
      return this.icon !== "-1" ? this.icon : TYPE_ICONS[this.type];
    },
    messageClasses() {
      return [
        "type-" + this.type,
        this.block ? "block" : null
      ];
    }
  },
  mounted() {
  },
  methods: {}
};
const __cssModules$7 = {};
var component$7 = normalizeComponent(script$7, render$7, staticRenderFns$7, false, injectStyles$7, null, null, null);
function injectStyles$7(context) {
  for (let o in __cssModules$7) {
    this[o] = __cssModules$7[o];
  }
}
component$7.options.__file = "app/components/messages/message.vue";
var uiMessage = component$7.exports;
var render$6 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("span", {staticClass: "ui-date", domProps: {innerHTML: _vm._s(_vm.output)}});
};
var staticRenderFns$6 = [];
var date_vue_vue_type_style_index_0_lang = ".ui-date .-minor {\n  color: var(--color-text-dim);\n}";
const script$6 = {
  name: "uiDate",
  props: {
    value: {
      type: String,
      default: null
    },
    format: {
      type: String,
      default: null
    },
    split: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    output: null
  }),
  watch: {
    value: function(value) {
      this.rebuild();
    }
  },
  mounted() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      if (!this.value) {
        this.output = "";
        return;
      }
      if (!this.split) {
        this.output = Strings.date(this.value, this.format);
      } else {
        this.output = Strings.date(this.value, "short") + ' <span class="-minor">' + Strings.date(this.value, "time") + "</span>";
      }
    }
  }
};
const __cssModules$6 = {};
var component$6 = normalizeComponent(script$6, render$6, staticRenderFns$6, false, injectStyles$6, null, null, null);
function injectStyles$6(context) {
  for (let o in __cssModules$6) {
    this[o] = __cssModules$6[o];
  }
}
component$6.options.__file = "app/components/date.vue";
var uiDate = component$6.exports;
var render$5 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-header-bar"}, [_c("div", {staticClass: "ui-header-bar-inner"}, [_c("div", {staticClass: "ui-header-bar-main"}, [_vm.backButton ? _c("ui-icon-button", {attrs: {type: "light onbg", size: 15}, on: {click: _vm.onBack}}) : _vm._e(), _c("div", {staticClass: "ui-header-bar-main-title"}, [_vm._t("title", [_c("h2", {staticClass: "ui-header-bar-title", class: {"is-empty": !_vm.title && _vm.titleEmpty}}, [_vm._l(_vm.prefixes, function(prefix2) {
    return [_c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: prefix2, expression: "prefix", arg: "html"}], staticClass: "-minor -prefix"}), _c("ui-icon", {staticClass: "-chevron", attrs: {symbol: "fth-chevron-right", size: 14}})];
  }), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.title || _vm.titleEmpty, expression: "title || titleEmpty"}]}), _vm.suffix ? _c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: _vm.suffix, expression: "suffix", arg: "html"}], staticClass: "-minor -suffix"}) : _vm._e(), _vm.count > 0 ? _c("span", {staticClass: "-minor -count"}, [_vm._v(_vm._s(_vm.count))]) : _vm._e()], 2)]), _vm.description ? _c("p", {directives: [{name: "localize", rawName: "v-localize", value: _vm.description, expression: "description"}], staticClass: "ui-header-bar-description"}) : _vm._e()], 2)], 1), _c("div", {staticClass: "ui-header-bar-aside"}, [_vm._t("default"), _vm.closeButton ? _c("ui-icon-button", {staticClass: "ui-header-bar-close", attrs: {icon: "fth-x", title: "@ui.close"}, on: {click: _vm.onClose}}) : _vm._e()], 2)])]);
};
var staticRenderFns$5 = [];
var headerBar_vue_vue_type_style_index_0_lang = ".ui-header-bar {\n  width: 100%;\n  height: 90px;\n  padding: 0 var(--padding) 0;\n}\n.ui-header-bar + .ui-blank-box, .ui-header-bar + .ui-box, .ui-header-bar + .ui-view-box {\n  margin-top: 0;\n}\n.ui-header-bar + .ui-view-box {\n  padding-top: 0;\n}\n.app-tree .ui-header-bar {\n  margin-bottom: 8px;\n}\n.ui-header-bar-inner {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  height: 100%;\n  /*max-width: 1104px;*/\n}\n.ui-header-bar-main {\n  display: flex;\n  align-items: center;\n  height: 100%;\n  flex: 1 0 auto;\n}\n.ui-header-bar-main .ui-icon-button {\n  margin-right: var(--padding-s);\n}\n.ui-header-bar-main-title {\n  flex: 1 0 auto;\n}\n.ui-header-bar-aside {\n  display: flex;\n  align-items: center;\n  height: 100%;\n  flex-shrink: 0;\n  padding-left: var(--padding-s);\n}\n.ui-header-bar-aside > * + * {\n  margin-left: var(--padding-s);\n}\n.ui-header-bar-title {\n  font-family: var(--font);\n  color: var(--color-text);\n  margin: 0;\n  font-size: var(--font-size-l);\n  font-weight: 700;\n  display: flex;\n  align-items: center;\n}\n.ui-header-bar-title.is-empty, .ui-header-bar-title .-minor {\n  color: var(--color-text-dim);\n  font-weight: 400;\n}\n.ui-header-bar-title .-prefix {\n  flex-shrink: 0;\n}\n.ui-header-bar-title .-chevron {\n  color: var(--color-text-dim);\n  position: relative;\n  top: -1px;\n  margin: 0 var(--padding-xxs);\n}\n.ui-header-bar-title .-count {\n  display: inline-block;\n  font-size: 11px;\n  font-weight: 700;\n  text-transform: uppercase;\n  background: var(--color-box);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  letter-spacing: 0.5px;\n  font-style: normal;\n  margin-left: 12px;\n  margin-top: 2px;\n  position: relative;\n  top: -1px;\n}\n.ui-header-bar-description {\n  font-size: var(--font-size-s);\n  color: var(--color-text-dim);\n  margin: 2px 0 0;\n}\n.ui-header-bar-close {\n  background: transparent !important;\n}";
const script$5 = {
  name: "uiHeaderBar",
  props: {
    title: {
      type: String
    },
    titleEmpty: {
      type: String
    },
    prefix: {
      type: [String, Array]
    },
    suffix: {
      type: String
    },
    count: {
      type: Number,
      default: 0
    },
    description: {
      type: String
    },
    backButton: {
      type: Boolean,
      default: false
    },
    closeButton: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    prefixes() {
      let items = Array.isArray(this.prefix) ? this.prefix : [this.prefix];
      return items.filter((x) => !!x);
    }
  },
  methods: {
    onBack() {
      this.$router.go(-1);
    },
    onClose() {
      Overlay.close();
    }
  }
};
const __cssModules$5 = {};
var component$5 = normalizeComponent(script$5, render$5, staticRenderFns$5, false, injectStyles$5, null, null, null);
function injectStyles$5(context) {
  for (let o in __cssModules$5) {
    this[o] = __cssModules$5[o];
  }
}
component$5.options.__file = "app/components/header-bar.vue";
var uiHeaderBar = component$5.exports;
var render$4 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("i", {staticClass: "ui-loading", class: {"is-big": _vm.isBig}});
};
var staticRenderFns$4 = [];
var loading_vue_vue_type_style_index_0_lang = ".ui-loading {\n  display: inline-block;\n  width: 28px;\n  height: 28px;\n  border-radius: 40px;\n  border: 2px solid var(--color-box);\n  border-left-color: var(--color-text);\n  will-change: transform;\n  animation: loadingRotation 0.8s linear infinite;\n}\n.ui-loading.is-big {\n  width: 36px;\n  height: 36px;\n  border-width: 2px;\n}\n@keyframes loadingRotation {\n0% {\n    transform: rotate(0);\n}\n100% {\n    transform: rotate(1turn);\n}\n}";
const script$4 = {
  name: "uiLoading",
  props: {
    isBig: {
      type: Boolean,
      default: false
    }
  }
};
const __cssModules$4 = {};
var component$4 = normalizeComponent(script$4, render$4, staticRenderFns$4, false, injectStyles$4, null, null, null);
function injectStyles$4(context) {
  for (let o in __cssModules$4) {
    this[o] = __cssModules$4[o];
  }
}
component$4.options.__file = "app/components/loading.vue";
var uiLoading = component$4.exports;
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-revisions", class: {"is-empty": _vm.items.length < 1}}, [_vm._l(_vm.items, function(revision) {
    return _c("div", {staticClass: "ui-revision"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@revisions.actions.updated", expression: "'@revisions.actions.updated'"}], staticClass: "ui-revision-action"}), _c("ui-date", {staticClass: "ui-revision-date", attrs: {format: "long", split: true}, model: {value: revision.date, callback: function($$v) {
      _vm.$set(revision, "date", $$v);
    }, expression: "revision.date"}}), revision.user ? _c("router-link", {staticClass: "ui-revision-user", attrs: {to: {name: _vm.userRoute, params: {id: revision.user.id}}}}, [revision.user ? _c("img", {staticClass: "ui-revision-user-image", attrs: {src: _vm.getImage(revision.user.avatarId), alt: revision.user.name}}) : _vm._e(), revision.user ? _c("span", {staticClass: "ui-revision-user-name"}, [_vm._v(_vm._s(revision.user.name))]) : _vm._e()]) : _c("div"), _c("button", {directives: [{name: "localize", rawName: "v-localize", value: "@revisions.view", expression: "'@revisions.view'"}], staticClass: "ui-link is-minor", attrs: {type: "button"}})], 1);
  }), _vm.pages > 1 ? _c("ui-pagination", {attrs: {pages: _vm.pages, page: _vm.page}, on: {change: _vm.setPage}}) : _vm._e()], 2);
};
var staticRenderFns$3 = [];
var revisions_vue_vue_type_style_index_0_lang = '.ui-revision {\n  display: grid;\n  grid-template-columns: auto 3fr 2fr auto;\n  gap: 20px;\n  align-items: center;\n  min-height: 28px;\n}\n.ui-revision + .ui-revision {\n  margin-top: 24px;\n}\n.ui-revision:first-child .ui-revision-action:before {\n  display: none;\n}\n.ui-revision-action {\n  align-self: center;\n  display: inline-block;\n  font-size: 9px;\n  font-weight: 700;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text-dim);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  letter-spacing: 0.5px;\n  position: relative;\n}\n.ui-revision-action:before {\n  content: "";\n  position: absolute;\n  left: calc(50% - 1.5px);\n  top: -30px;\n  height: 30px;\n  width: 3px;\n  background: var(--color-box-nested);\n}\n.ui-revision-user {\n  color: var(--color-text);\n}\n.ui-revision-user-image {\n  width: 28px;\n  height: 28px;\n  border-radius: 14px;\n  margin-right: 8px;\n}';
const script$3 = {
  name: "uiRevisions",
  props: {
    get: {
      type: Function,
      required: true
    }
  },
  data: () => ({
    userRoute: zero.alias.settings.users + "-edit",
    items: [],
    page: 1,
    pages: 0
  }),
  mounted() {
    this.setPage(this.page);
  },
  methods: {
    getImage(id) {
      return MediaApi.getImageSource(id);
    },
    setPage(page) {
      this.page = page;
      this.get(page).then((res) => {
        this.items = res.items;
        this.pages = res.totalPages;
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
component$3.options.__file = "app/components/revisions.vue";
var uiRevisions = component$3.exports;
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("svg", {staticClass: "ui-icon", class: _vm.classes, attrs: {width: _vm.size, height: _vm.size, "stroke-width": _vm.stroke, "data-symbol": _vm.symbolName}}, [_c("use", _vm._b({directives: [{name: "show", rawName: "v-show", value: !_vm.isFlag, expression: "!isFlag"}]}, "use", {"xlink:href": _vm.href}, false))]);
};
var staticRenderFns$2 = [];
var icon_vue_vue_type_style_index_0_lang = ".ui-icon {\n  stroke: currentColor;\n  stroke-linecap: round;\n  stroke-linejoin: round;\n  fill: none;\n}\n.ui-icon[data-symbol=fth-waffle] {\n  stroke: none;\n  fill: currentColor;\n}";
const script$2 = {
  name: "uiIcon",
  props: {
    symbol: {
      type: String,
      default: null,
      required: true
    },
    file: {
      type: String,
      default: null
    },
    size: {
      type: Number,
      default: 17
    },
    stroke: {
      type: Number,
      default: 2
    }
  },
  computed: {
    symbolName() {
      return this.symbol && this.symbol.split(" ")[0];
    },
    classes() {
      return this.symbol ? this.symbol.split(" ").slice(1) : [];
    },
    isFlag() {
      return this.symbol && this.symbol.indexOf("flag") === 0;
    },
    href() {
      return (this.file || "") + "#" + this.symbolName.trim();
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
component$2.options.__file = "app/components/icon.vue";
var uiIcon = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("span", [_vm._v(_vm._s(_vm.output))]);
};
var staticRenderFns$1 = [];
const script$1 = {
  name: "uiLocalize",
  props: {
    value: {
      type: String,
      default: null
    },
    force: {
      type: Boolean,
      default: false
    },
    tokens: {
      type: Object,
      default: null
    },
    hideEmpty: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    output() {
      return Localization.localize(this.value, {tokens: this.tokens, force: this.force, hideEmpty: this.hideEmpty});
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
component$1.options.__file = "app/components/localize.vue";
var uiLocalize = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.visible ? _c("div", {staticClass: "editor-infos"}, [_c("div", {staticClass: "ui-box is-light editor-infos-aside"}, [_vm._t("before"), _vm.value.id && _vm.value.lastModifiedDate ? _c("ui-property", {attrs: {field: "lastModifiedDate", label: "@ui.modifiedDate"}}, [_c("ui-date", {model: {value: _vm.value.lastModifiedDate, callback: function($$v) {
    _vm.$set(_vm.value, "lastModifiedDate", $$v);
  }, expression: "value.lastModifiedDate"}})], 1) : _vm._e(), _vm.value.id ? _c("ui-property", {attrs: {label: "@ui.createdDate", field: "createdDate"}}, [_c("ui-date", {model: {value: _vm.value.createdDate, callback: function($$v) {
    _vm.$set(_vm.value, "createdDate", $$v);
  }, expression: "value.createdDate"}})], 1) : _vm._e(), _vm._t("after")], 2)]) : _vm._e();
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: [Object, Array]
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    visible() {
      return this.value && this.value.id || this.$scopedSlots.hasOwnProperty("before") || this.$scopedSlots.hasOwnProperty("after");
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
component.options.__file = "app/editor/editor-infos.vue";
var uiEditorInfos = component.exports;
var editorInfos = /* @__PURE__ */ Object.freeze({
  __proto__: null,
  [Symbol.toStringTag]: "Module",
  default: uiEditorInfos
});
var components = {
  uiAddButton,
  uiButton,
  uiDotButton,
  uiIconButton,
  uiSelectButton,
  uiStateButton,
  uiAlias,
  uiCheckList,
  uiErrorView,
  uiError,
  uiFormHeader,
  uiForm,
  uiInputList,
  uiProperty,
  uiRte,
  uiSearch,
  uiTags,
  uiToggle,
  uiSelect,
  uiModulePreviewFigure,
  uiModulePreviewHeadline,
  uiModulePreviewText,
  uiModulePreviewButton,
  uiModulePreviewTags,
  uiModules,
  uiDropdownButton,
  uiDropdownSeparator,
  uiDropdown,
  uiOverlayEditor,
  uiColorpicker,
  uiCountrypicker,
  uiDatepicker,
  uiDaterangepicker,
  uiIconpicker,
  uiMediapicker,
  uiPagepicker,
  uiUserpicker,
  uiMailtemplatepicker,
  uiSpacepicker,
  uiLinkpicker,
  uiPick,
  uiTable,
  uiTableFilter,
  uiTabs,
  uiTab,
  uiInlineTabs,
  uiTree,
  uiTreeItem,
  uiDatagrid,
  uiMessage,
  uiDate,
  uiHeaderBar,
  uiLoading,
  uiPagination,
  uiPermissions,
  uiRevisions,
  uiIcon,
  uiLocalize,
  uiEditor: UiEditor,
  uiEditorInfos
};
var isServer = (vNode) => {
  return typeof vNode.componentInstance !== "undefined" && vNode.componentInstance.$isServer;
};
var isPopup = (popupItem, elements) => {
  if (!popupItem || !elements) {
    return false;
  }
  for (var i = 0, len = elements.length; i < len; i++) {
    try {
      if (popupItem.contains(elements[i])) {
        return true;
      }
      if (elements[i].contains(popupItem)) {
        return false;
      }
    } catch (e) {
      return false;
    }
  }
  return false;
};
var validate = (binding) => {
  if (typeof binding.value !== "function") {
    console.warn("v-click-outside: provided expression " + binding.expression + " is not a function.");
    return false;
  }
  return true;
};
var Resizable = function(element, params) {
  const prefix2 = "zero.ui-resizable.";
  const cacheKey = prefix2 + (params.save || "none");
  const resizingClass = "ui-resizing";
  let isVertical = ["Y", "y"].indexOf(params.axis) > -1;
  this.element = element;
  this.params = params;
  this.handle = this.element.querySelector(this.params.handle || ".handle");
  if (!this.handle) {
    throw "Set the [handle] parameter to a valid CSS selector within the attached element";
  }
  this.listen = () => {
    this.handle.addEventListener("mousedown", this.start);
    this.handle.addEventListener("dblclick", this.reset);
  };
  this.detach = () => {
    this.handle.removeEventListener("mousedown", this.start);
    this.handle.removeEventListener("dblclick", this.reset);
    document.removeEventListener("mousemove", this.resize);
    document.removeEventListener("mouseup", this.stop);
  };
  this.start = (e) => {
    this.coordinates = {
      X: e.pageX,
      Y: e.pageY,
      offsetX: 0,
      offsetY: 0
    };
    document.body.classList.add(resizingClass);
    document.addEventListener("mousemove", this.resize);
    document.addEventListener("mouseup", this.stop);
  };
  this.resize = (e) => {
    let deltaX = e.pageX - this.coordinates.X;
    let deltaY = e.pageY - this.coordinates.Y;
    let delta = isVertical ? deltaY : deltaX;
    let newValue = this.value + delta;
    if (params.min && params.min > newValue) {
      newValue = params.min;
    } else if (params.max && params.max < newValue) {
      newValue = params.max;
    }
    this.update(newValue);
  };
  this.stop = (e) => {
    document.body.classList.remove(resizingClass);
    document.removeEventListener("mousemove", this.resize);
    document.removeEventListener("mouseup", this.stop);
    this.value = getCurrentValue();
    localStorage.setItem(cacheKey, this.value);
  };
  this.update = (value) => {
    if (value > 0) {
      this.element.style[isVertical ? "height" : "width"] = value + "px";
    }
  };
  this.reset = () => {
    localStorage.removeItem(cacheKey);
    this.element.style[isVertical ? "height" : "width"] = "";
    this.value = getCurrentValue();
  };
  let getCurrentValue = () => {
    return this.element[isVertical ? "clientHeight" : "clientWidth"];
  };
  this.value = getCurrentValue();
  if (params.save) {
    const newValue = +localStorage.getItem(cacheKey);
    if (newValue > 0) {
      this.update(+localStorage.getItem(cacheKey));
      this.value = newValue;
    }
  }
};
var localize = (el, binding) => {
  if (binding.value !== binding.oldValue || !el.innerText) {
    const hasValue = !!binding.value;
    const isObject = typeof binding.value === "object";
    let key = hasValue ? isObject ? binding.value.key : binding.value : null;
    let options2 = hasValue && isObject ? binding.value : null;
    const result = hasValue ? Localization.localize(key, options2) : "";
    if (binding.arg === "html") {
      el.innerHTML = result;
    } else if (binding.arg === "title") {
      el.title = result;
    } else if (binding.arg) {
      el.setAttribute(binding.arg, result);
    } else {
      el.innerText = result;
    }
  }
};
var placeholder = (el, binding) => {
  if (binding.value !== binding.oldValue || !el.innerText) {
    const hasValue = !!binding.value;
    const isObject = typeof binding.value === "object";
    let value = hasValue ? isObject ? binding.value.placeholder : binding.value : null;
    let result = null;
    if (isObject && typeof value === "function") {
      if (typeof binding.value.model === "undefined") {
        return;
      }
      result = value(binding.value.model);
    } else {
      result = value;
    }
    result = hasValue ? Localization.localize(result) : "";
    el.setAttribute("placeholder", result);
  }
};
var date = (el, binding) => {
  if (binding.value !== binding.oldValue) {
    if (!binding.value) {
      el.innerHTML = "-";
      return;
    }
    el.innerHTML = Strings.date(binding.value);
  }
};
var sortable = (el, binding) => {
  if (binding.value === binding.oldValue) {
    return;
  }
  new Sortable(el, binding.value || {});
};
var filesize = (el, binding) => {
  if (binding.value !== binding.oldValue) {
    el.innerText = Strings.filesize(binding.value);
  }
};
var currency = (el, binding) => {
  if (binding.value !== binding.oldValue) {
    el.innerHTML = Strings.currency(binding.value);
  }
};
var resizable = {
  bind(el, binding) {
    let resizable2 = new Resizable(el, binding.value);
    resizable2.listen();
  }
};
var maxLines = (el, binding) => {
  if (binding.value !== binding.oldValue) {
    if (!el.__zero_maxlines) {
      el.__zero_maxlines = true;
      el.addEventListener("click", (e) => {
        el.classList.toggle("is-expanded");
      });
    }
    el.classList.add("ui-maxlines");
    el.style.webkitLineClamp = +binding.value > 0 ? +binding.value : null;
  }
};
var clickOutside = {
  bind(el, binding, vNode) {
    if (!validate(binding))
      return;
    function handler(e) {
      if (!vNode.context)
        return;
      var elements = e.path || e.composedPath && e.composedPath();
      elements && elements.length > 0 && elements.unshift(e.target);
      if (el.contains(e.target) || isPopup(vNode.context.popupItem, elements) || !el.__vueClickOutside__) {
        return;
      }
      el.__vueClickOutside__.callback(e);
    }
    el.__vueClickOutside__ = {
      handler,
      callback: binding.value
    };
    setTimeout(() => {
      !isServer(vNode) && document.addEventListener("click", handler);
    }, 200);
  },
  update(el, binding) {
    if (validate(binding)) {
      el.__vueClickOutside__.callback = binding.value;
    }
  },
  unbind(el, binding, vNode) {
    !isServer(vNode) && document.removeEventListener("click", el.__vueClickOutside__.handler);
    delete el.__vueClickOutside__;
  }
};
var directives = /* @__PURE__ */ Object.freeze({
  __proto__: null,
  [Symbol.toStringTag]: "Module",
  localize,
  placeholder,
  date,
  sortable,
  filesize,
  currency,
  resizable,
  maxLines,
  clickOutside
});
Vue.config.errorHandler = (err, vm, info) => {
  console.error(err);
};
Vue.config.warnHandler = (msg, vm, trace) => {
  console.error(`[zero warn]: ${msg}${trace}`);
};
if (!__zero || !__zero.apiPath) {
  throw Exception("window.zero and zero.apiPath (= base path to the backoffice API) have to be configured");
}
axios.defaults.baseURL = __zero.apiPath;
axios.defaults.withCredentials = true;
axios.defaults.paramsSerializer = (params) => {
  return O__zero_zero_Web_UI_node_modules_qs_lib.stringify(params, {allowDots: true});
};
axios.interceptors.response.use((response) => response, (error) => {
  if (error.response) {
    if (error.response.status === 401) {
      Auth.rejectUser("@login.rejectReasons.inactive");
    }
  }
  return Promise.reject(error);
});
axios.interceptors.request.use((config2) => {
  if (location.search) {
    var query = O__zero_zero_Web_UI_node_modules_qs_lib.parse(location.search.substring(1));
    if (query.scope) {
      if (!config2.params) {
        config2.params = {};
      }
      config2.params["scope"] = query.scope;
    }
  }
  return config2;
}, (error) => Promise.reject(error));
Object.entries(components).forEach((cmp) => {
  Vue.component(cmp[0], cmp[1].default || cmp[1]);
});
Object.entries(directives).forEach((cmp) => {
  Vue.directive(cmp[0], cmp[1]);
});
Vue.component("MyAddButton", () => __vitePreload(() => __import__("./add-button-2.js"), true ? ["/zero/add-button-2.js","/zero/add-button-2.css","/zero/vendor.js"] : void 0));
Vue.use(VueRouter);
Vue.use(Zero);
App.router = Zero.instance.router;
var app = new Vue(App);
app.$mount("#app");
export {NavigationEditor as $, Arrays as A, OrdersApi as B, CataloguesApi as C, DeliveryProducts as D, Editor as E, FolderOverlay as F, ProductPicker as G, CurrenciesApi as H, NumbersApi as I, OrderStatesApi as J, PropertiesApi as K, LanguagesApi as L, MediaFolderApi as M, Notification as N, Overlay as O, PageTreeApi as P, FiltersApi as Q, AuthorsApi as R, Strings as S, TaxesApi as T, UiEditor as U, TagsApi as V, StoriesApi as W, Authorpicker as X, FormsApi as Y, LinkpickerOverlay$1 as Z, __$_require_ssets_zero_2_png__ as _, PagesApi as a, SizeChartsApi as a0, VouchersApi as a1, DiscountsApi as a2, CampaignsApi as a3, list$1 as a4, RequestsApi as a5, ExperiencesApi as a6, TranslationsApi as a7, MediaApi as b, UploadStatusOverlay as c, del$1 as d, ApplicationsApi as e, countriesApi as f, get$1 as g, hub as h, __vitePreload as i, MailTemplatesApi as j, UserRolesApi as k, UsersApi as l, SpacesApi as m, normalizeComponent as n, List as o, post$1 as p, list$h as q, ProductsApi as r, CategoriesApi as s, ChannelsApi as t, CustomersApi as u, ManufacturersApi as v, ShippingOptionPicker as w, ShippingLines as x, ShippingOptionsApi as y, UiEditorOverlay as z};
