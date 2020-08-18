import MetaFields from './meta.partial';

export default {
  alias: 'page.content',

  tabs: [
    {
      name: 'general',
      label: '@ui.tab_content',
      class: 'is-blank'
    },
    {
      name: 'meta',
      label: 'Meta'
    }
  ],

  fields: [
    //{
    //  field: 'name',
    //  display: 'text',
    //  label: '@ui.name',
    //  required: true
    //},
    {
      field: 'name',
      display: 'modules',
      label: '@ui.name',
      required: true,
      hideLabel: true,
      class: 'ui-modules'
    }
  ].concat(MetaFields)
};