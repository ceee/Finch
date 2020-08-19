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
    {
      field: 'modules',
      display: 'modules',
      label: '@ui.name',
      required: true,
      hideLabel: true,
      class: 'ui-modules'
    }
  ].concat(MetaFields)
};