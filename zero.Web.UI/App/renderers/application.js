export default {
  collection: 'applications',

  tabs: [
    {
      name: 'general',
      label: '@ui.tab_general'
    },
    {
      name: 'domains',
      label: '@application.tab_domains',
      count: x => x.domains.length
    },
    {
      name: 'features',
      label: '@application.tab_features',
      count: x => x.features.length
    }
  ],

  labelTemplate(field)
  {
    return "@application.fields." + field;
  },

  descriptionTemplate(field)
  {
    return "@application.fields." + field + "_text";
  },

  fields: [
    {
      field: 'name',
      display: 'text',
      label: '@ui.name',
      required: true
    },
    {
      field: 'fullName',
      display: 'text'
    },
    {
      field: 'email',
      display: 'text',
      required: true
    },
    {
      field: 'imageId',
      display: 'media',
      type: 'image',
      class: 'my-image'
    },
    {
      field: 'iconId',
      display: 'media',
      type: 'image'
    },
    {
      tab: 'domains',
      field: 'domains',
      display: 'inputList',
      limit: 10,
      addLabel: '@application.fields.domains_add',
      helpText: '@application.fields.domains_help'
    },
    {
      tab: 'features',
      field: 'features',
      display: 'custom',
      path: '@zero/pages/settings/application-features.vue'
    }
  ]
};