export default {
  alias: 'country',

  labelTemplate(field)
  {
    return "@country.fields." + field;
  },

  descriptionTemplate(field)
  {
    return "@country.fields." + field + "_text";
  },

  fields: [
    {
      field: 'name',
      display: 'text',
      label: '@ui.name',
      required: true
    },
    {
      field: 'code',
      display: 'text',
      required: true
    },
    {
      field: 'isPreferred',
      display: 'toggle'
    }
  ]
};