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

  fields: [
    {
      field: 'headline',
      display: 'text',
      label: 'Headline',
      required: true
    },
    {
      field: 'subline',
      display: 'text',
      label: 'Subline'
    },
  ]
};