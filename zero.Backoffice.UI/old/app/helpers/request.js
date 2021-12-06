"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.download = exports.collection = exports.send = exports.patch = exports.put = exports.del = exports.post = exports.get = void 0;
var axios_1 = require("axios");
var getConfig = function (config) {
    config = config || {};
    if (config.scope) {
        config.params = config.params || {};
        config.params.scope = config.scope;
    }
    return config;
};
function get(url, config) {
    if (config === void 0) { config = null; }
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, send(__assign({ method: 'get', url: url }, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        });
    });
}
exports.get = get;
function post(url, data, config) {
    if (data === void 0) { data = null; }
    if (config === void 0) { config = null; }
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, send(__assign({ method: 'post', url: url, data: data }, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        });
    });
}
exports.post = post;
function del(url, config) {
    if (config === void 0) { config = null; }
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, send(__assign({ method: 'delete', url: url }, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        });
    });
}
exports.del = del;
function put(url, data, config) {
    if (data === void 0) { data = null; }
    if (config === void 0) { config = null; }
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, send(__assign({ method: 'put', url: url, data: data }, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        });
    });
}
exports.put = put;
function patch(url, data, config) {
    if (data === void 0) { data = null; }
    if (config === void 0) { config = null; }
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, send(__assign({ method: 'patch', url: url, data: data }, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        });
    });
}
exports.patch = patch;
function send(config) {
    return __awaiter(this, void 0, void 0, function () {
        var result, err_1;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    config = getConfig(config);
                    _a.label = 1;
                case 1:
                    _a.trys.push([1, 3, , 4]);
                    return [4 /*yield*/, axios_1.default(config)];
                case 2:
                    result = _a.sent();
                    return [2 /*return*/, result.data];
                case 3:
                    err_1 = _a.sent();
                    return [3 /*break*/, 4];
                case 4: return [2 /*return*/];
            }
        });
    });
}
exports.send = send;
function collection(base) {
    var _this = this;
    return {
        getById: function (id, config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, get(base + 'getById', __assign(__assign({}, config), { params: { id: id } }))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        getByIds: function (ids, config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, get(base + 'getByIds', __assign(__assign({}, config), { params: { ids: ids } }))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        getEmpty: function (config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, get(base + 'getEmpty', __assign({}, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        getByQuery: function (query, config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, get(base + 'getByQuery', __assign(__assign({}, config), { params: { query: query } }))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        getAll: function (config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, get(base + 'getAll', __assign({}, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        getPreviews: function (ids, config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, get(base + 'getPreviews', __assign(__assign({}, config), { params: { ids: ids } }))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        getForPicker: function (config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, get(base + 'getForPicker', __assign({}, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        save: function (model, config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, post(base + 'save', model, __assign({}, config))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); },
        delete: function (id, config) { return __awaiter(_this, void 0, void 0, function () { return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, del(base + 'delete', __assign(__assign({}, config), { params: { id: id } }))];
                case 1: return [2 /*return*/, _a.sent()];
            }
        }); }); }
    };
}
exports.collection = collection;
function download(response) {
    var filename = response.headers["zero-filename"] || 'download.bin';
    // code from: https://github.com/kennethjiang/js-file-download/blob/master/file-download.js
    var blob = response.data;
    if (typeof window.navigator.msSaveBlob !== 'undefined') {
        // IE workaround for "HTML7007: One or more blob URLs were
        // revoked by closing the blob for which they were created.
        // These URLs will no longer resolve as the data backing
        // the URL has been freed."
        window.navigator.msSaveBlob(blob, filename);
    }
    else {
        var blobURL = (window.URL && window.URL.createObjectURL) ? window.URL.createObjectURL(blob) : window.webkitURL.createObjectURL(blob);
        var tempLink = document.createElement('a');
        tempLink.style.display = 'none';
        tempLink.href = blobURL;
        tempLink.setAttribute('download', filename);
        // Safari thinks _blank anchor are pop ups. We only want to set _blank
        // target if the browser does not support the HTML5 download attribute.
        // This allows you to download files in desktop safari if pop up blocking
        // is enabled.
        if (typeof tempLink.download === 'undefined') {
            tempLink.setAttribute('target', '_blank');
        }
        document.body.appendChild(tempLink);
        tempLink.click();
        // Fixes "webkit blob resource error 1"
        setTimeout(function () {
            document.body.removeChild(tempLink);
            window.URL.revokeObjectURL(blobURL);
        }, 200);
    }
}
exports.download = download;
;
//# sourceMappingURL=request.js.map