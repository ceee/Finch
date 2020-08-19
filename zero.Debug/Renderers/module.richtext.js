export default {
  alias: 'module.richtext',

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

  fields: [
    {
      field: 'text',
      display: 'rte',
      label: 'Text',
      description: 'Enter rich-text',
      required: true
    },
    {
      field: 'isBigger',
      display: 'toggle',
      label: 'Is bigger'
    }
  ]
};