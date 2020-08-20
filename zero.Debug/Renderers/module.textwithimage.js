export default {
  alias: 'module.textWithImage',

  // define a preview which is rendered in the overview
  preview: {
    template: '<ui-module-preview-text :text="model.text" />'
  },

  fields: [
    {
      field: 'headline',
      display: 'text',
      label: 'Headline',
      required: true
    },
    {
      field: 'text',
      display: 'rte',
      label: 'Text',
      description: 'Enter a long-form text',
      required: true
    },
    {
      field: 'isLeftAligned',
      display: 'toggle',
      label: 'Is left aligned'
    },
    {
      field: 'imageId',
      display: 'media',
      label: 'Image'
    }
  ]
};