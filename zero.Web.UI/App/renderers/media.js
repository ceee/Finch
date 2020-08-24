export default {
  alias: 'media',

  labelTemplate(field)
  {
    return "@media.fields." + field;
  },

  descriptionTemplate(field)
  {
    return "@media.fields." + field + "_text";
  },

  fields: [
    {
      field: 'source',
      display: 'custom',
      required: true,
      path: '@zero/pages/media/upload.vue'
    },
    {
      field: 'name',
      display: 'text',
      label: '@ui.name',
      required: true
    },
    {
      field: 'alternativeText',
      display: 'text'
    },
    {
      field: 'caption',
      display: 'textarea'
    }
  ]
};