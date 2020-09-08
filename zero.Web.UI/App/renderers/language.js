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
  ],

  list: {
    labelPrefix: '@language.fields.',
    search: null,
    columns: {
      name: {
        label: '@ui.name',
        as: 'text',
        bold: true,
        link: item =>
        {
          return {
            name: zero.alias.sections.settings + '-' + zero.alias.settings.languages + '-edit',
            params: { id: item.id }
          };
        }
      },
      code: 'text',
      isDefault: {
        as: 'bool',
        width: 200
      }
    }
  }
};