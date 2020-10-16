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
      required: true,
      maxLength: 120
    },
    {
      field: 'alias',
      display: 'alias',
      label: '@ui.alias',
      required: true
    },
    {
      field: 'code',
      display: 'text',
      required: true,
      maxLength: 2
    },
    {
      field: 'isPreferred',
      display: 'toggle'
    }
  ],

  list: {
    labelPrefix: '@country.fields.',
    allowOrder: false,
    search: null,
    columns: {
      flag: {
        label: '',
        as: 'html',
        render: item => `<i class="flag flag-${item.code.toLowerCase()}"></i>`,
        width: 62,
        sort: false
      },
      name: {
        label: '@ui.name',
        as: 'text',
        bold: true,
        link: item =>
        {
          return {
            name: zero.alias.settings.countries + '-edit',
            params: { id: item.id }
          };
        },
        shared: true
      },
      code: 'text',
      isPreferred: {
        as: 'bool',
        width: 200
      },
      isActive: {
        as: 'bool',
        label: '@ui.active',
        width: 200
      }
    },
  }
};