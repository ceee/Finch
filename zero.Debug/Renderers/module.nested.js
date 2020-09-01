export default {
  alias: 'module.nested',

  // define a preview which is rendered in the overview
  preview: {
    template: '<b>Nested</b>'
  },

  fields: [
    {
      field: 'modules',
      display: 'modules',
      label: 'Modules',
      required: true,
      hideLabel: true,
      class: 'ui-modules'
    }
  ]
};