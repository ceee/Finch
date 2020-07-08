import MetaFields from './meta.partial';

export default {
  alias: 'page.content',

  tabs: [
    {
      name: 'general',
      label: '@ui.tab_content'
    },
    {
      name: 'meta',
      label: 'Meta'
    }
  ],

  fields: [
    {
      field: 'name',
      display: 'text',
      label: '@ui.name',
      required: true
    }
  ].concat(MetaFields)
};