
import { filter as _filter } from 'underscore';

export default {
  alias: 'user',

  tabs: [
    {
      name: 'general',
      label: '@ui.tab_general'
    },
    {
      name: 'permissions',
      label: '@user.tab_permissions',
      count: x =>
      {
        return _filter(x.claims || [], claim =>
        {
          const value = claim.value.split(':')[1];
          return value !== 'none' && value !== 'false' && !!value;
        }).length;
      }
    }
  ],

  labelTemplate(field)
  {
    return "@user.fields." + field;
  },

  descriptionTemplate(field)
  {
    return "@user.fields." + field + "_text";
  },

  fields: [
    {
      field: 'name',
      display: 'text',
      label: '@ui.name',
      required: true,
      maxLength: 80
    },
    {
      field: 'email',
      display: 'text',
      required: true,
      maxLength: 120
    },
    {
      field: 'languageId',
      display: 'culture',
      required: true
    },
    {
      field: 'avatarId',
      display: 'media',
      type: 'image'
    },
    {
      tab: 'permissions',
      field: 'claims',
      display: 'custom',
      hideLabel: true,
      path: '@zero/components/permissions.vue'
    }
  ],

  list: {
    labelPrefix: '@user.fields.',
    search: null,
    columns: {
      avatarId: {
        label: '',
        as: 'image',
        width: 70,
        link: item =>
        {
          return {
            name: zero.alias.sections.settings + '-' + zero.alias.settings.users + '-edit',
            params: { id: item.id }
          };
        },
        sort: false
      },
      name: {
        label: '@ui.name',
        as: 'text',
        bold: true,
        link: item =>
        {
          return {
            name: zero.alias.sections.settings + '-' + zero.alias.settings.users + '-edit',
            params: { id: item.id }
          };
        },
        shared: true
      },
      email: 'text',
      roles: {
        as: 'text'
      },
      isActive: {
        as: 'bool',
        width: 200
      }
    },
  }
};