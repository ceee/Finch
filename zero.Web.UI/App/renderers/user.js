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
      required: true
    },
    {
      field: 'email',
      display: 'text',
      required: true
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
  ]
};