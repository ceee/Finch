import MetaFields from './meta.partial';

export default {
  alias: 'page.content',

  tabs: [
    {
      name: 'general',
      label: '@ui.tab_content',
      count: x => x.modules.length
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
      hideLabel: true
    }
  ].concat(MetaFields)
};