export default {
  alias: 'language',

  labelTemplate(field)
  {
    return "@language.fields." + field;
  },

  descriptionTemplate(field)
  {
    return "@language.fields." + field + "_text";
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
      field: 'inheritedLanguageId',
      display: 'language'
    },
    {
      field: 'isDefault',
      display: 'toggle'
    },
    {
      field: 'isOptional',
      display: 'toggle'
    }
  ]
};