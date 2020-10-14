let imports = [
  // buttons
  () => import('./buttons/add-button.vue'),
  () => import('./buttons/button.vue'),
  () => import('./buttons/dot-button.vue'),
  () => import('./buttons/icon-button.vue'),
  () => import('./buttons/select-button.vue'),
  () => import('./buttons/state-button.vue'),

  // forms
  () => import('./forms/alias.vue'),
  () => import('./forms/check-list.vue'),
  () => import('./forms/error-view.vue'),
  () => import('./forms/error.vue'),
  () => import('./forms/form-header.vue'),
  () => import('./forms/form.vue'),
  () => import('./forms/input-list.vue'),
  () => import('./forms/media-old.vue'),
  () => import('./forms/property.vue'),
  () => import('./forms/rte.vue'),
  () => import('./forms/search.vue'),
  () => import('./forms/tags.vue'),
  () => import('./forms/toggle.vue'),

  // modules
  () => import('./modules/modules.vue'),

  // overlays
  () => import('./overlays/confirm.vue'),
  () => import('./overlays/dropdown-button.vue'),
  () => import('./overlays/dropdown-separator.vue'),
  () => import('./overlays/dropdown.vue'),
  () => import('./overlays/overlay-editor.vue'),

  // pickers
  () => import('./pickers/colorPicker/colorpicker.vue'),
  () => import('./pickers/countryPicker/countrypicker.vue'),
  () => import('./pickers/datePicker/datepicker.vue'),
  () => import('./pickers/dateRangePicker/daterangepicker.vue'),
  () => import('./pickers/iconPicker/iconpicker.vue'),
  () => import('./pickers/mediaPicker/mediapicker.vue'),
  () => import('./pickers/userPicker/userpicker.vue'),
  () => import('./pickers/pick.vue'),

  // tables
  () => import('./tables/table.vue'),
  () => import('./tables/table-filter.vue'),

  // tabs
  () => import('./tabs/tab.vue'),
  () => import('./tabs/tabs.vue'),
  () => import('./tabs/inline-tabs.vue'),

  // tabs
  () => import('./tree/tree.vue'),
  () => import('./tree/tree-item.vue'),

  // misc
  () => import('./datagrid/datagrid.vue'),
  () => import('./messages/message.vue'),
  () => import('./date.vue'),
  () => import('./header-bar.vue'),
  () => import('./loading.vue'),
  () => import('./pagination.vue'),
  () => import('./permissions.vue'),
  () => import('./revisions.vue')
];

const resolveFilename = path =>
{
  let pathParts = path.split('/');
  return 'ui' + pathParts[pathParts.length - 1]
    .replace(/^\.\/_/, '')
    .replace(/\.\w+$/, '')
    .split('-')
    .map((kebab) => kebab.charAt(0).toUpperCase() + kebab.slice(1))
    .join('');
};

export default function (app)
{
  imports.forEach(path =>
  {
    path().then(resolved =>
    {
      const config = resolved.default || resolved;
      const name = config.name || resolveFilename(config.__hmrId);
      app.component(name, config);
    });
  });
};