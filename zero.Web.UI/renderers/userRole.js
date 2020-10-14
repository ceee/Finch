import { filter as _filter } from 'underscore';

export default {
  alias: 'userRole',

  tabs: [
    {
      name: 'general',
      label: '@ui.tab_general'
    },
    {
      name: 'permissions',
      label: '@role.tab_permissions',
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
    return "@role.fields." + field;
  },

  descriptionTemplate(field)
  {
    return "@role.fields." + field + "_text";
  },

  fields: [
    {
      field: 'name',
      display: 'text',
      label: '@ui.name',
      required: true
    },
    {
      field: 'description',
      display: 'text'
    },
    {
      field: 'icon',
      display: 'iconpicker'
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