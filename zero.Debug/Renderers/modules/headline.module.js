import MetaFields from '../meta.partial';

export default {
  alias: 'module.headline',

  // define layouts users can switch between
  layouts: [
    {
      label: 'Default',
      preview: '/images/rte-layout1.png'
    },
    {
      label: 'Big',
      preview: '/images/rte-layout2.png'
    },
  ],

  // define a preview which is rendered in the overview
  preview: {
    template: '<div v-html="model.text"></div>'
  },

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