export default {
  alias: 'text.module',
  label: '@dbg.modules.text.label',
  description: '@dbg.modules.text.description',
  icon: 'fth-text',

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
    }
  ]
};