
export default {
  alias: 'space.team',

  fields: [
    {
      field: 'name',
      display: 'text',
      label: 'Name',
      required: true
    },
    {
      field: 'position',
      display: 'text',
      label: 'Position'
    },
    {
      field: 'email',
      display: 'text',
      label: 'Email',
      required: true
    }
  ],

  list: {
    columns: {
      name: {
        as: 'text',
        label: '@ui.name',
        bold: true,
        link: item =>
        {
          return {
            name: 'space-item',
            params: { alias: 'team', id: item.id }
          };
        }
      },
      position: {
        as: 'text',
        label: 'Position'
      },
      email: {
        as: 'text',
        label: 'Email'
      },
      createdDate: {
        as: 'date',
        label: '@ui.createdDate'
      }
    }
  }
};