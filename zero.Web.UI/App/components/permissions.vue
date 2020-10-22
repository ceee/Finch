<template>
  <div>
    <ui-inline-tabs v-if="permissions.length" class="ui-permissions" :force-count="true">
      <ui-tab v-for="(permissionCollection, index) in permissions" :key="index" :label="permissionCollection.label" :title="permissionCollection.description" :count="getCount(permissionCollection)">
        <ui-error field="Claims" />
        <ui-property v-for="(permission, index) in permissionCollection.items" :key="index" class="role-permission-toggle" :label="permission.label" :description="permission.description">
          <ui-toggle v-if="permission.valueType === 'boolean'" :disabled="disabled" v-model="permission.value" @input="onChange" />
          <div class="ui-permissions-crud" v-if="permission.valueType === 'crud'">
            <ui-toggle :value="permission.value != 'none'" :disabled="disabled" @input="onPermissionToggle($event, permission)" />
            <ui-check-list :value="permission.value.split(',')" :items="stateItems" v-if="permission.value != 'none'" :inline="true" @input="onPermissionCRUDChecked($event, permission)" :disabled="disabled" />
          </div>
          <!--<input v-if="permission.valueType === 'string'" :disabled="disabled" v-model="permission.value" type="text" class="ui-input" @input="onChange" />-->
        </ui-property>
      </ui-tab>
    </ui-inline-tabs>
  </div>
</template>


<script>
  import UserRolesApi from 'zero/resources/userRoles';
  import { filter as _filter } from 'underscore';

  export default {
    name: 'uiPermissions',

    props: {
      value: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    watch: {
      value(value)
      {
        this.rebuild();
      }
    },

    data: () => ({
      claims: [],
      stateItems: [
        { value: '@permission.states.read', key: 'read' },
        { value: '@permission.states.update', key: 'update' },
        { value: '@permission.states.create', key: 'create' }
      ],
      permissions: []
    }),

    created()
    {
      UserRolesApi.getAllPermissions().then(response =>
      {
        this.permissions = response;
        this.rebuild();
      });
    },

    methods: {

      rebuild()
      {
        if (!this.permissions.length)
        {
          return;
        }

        let claims = {};

        this.value.forEach(claim =>
        {
          const parts = claim.value.split(':');
          claims[parts[0]] = parts[1];
        });

        this.permissions.forEach(collection =>
        {
          collection.items.forEach(permission =>
          {
            permission.value = this.parsePermissionValue(claims[permission.key], permission.valueType);
          });
        });
      },


      getCount(permissionGroup)
      {
        return _filter(permissionGroup.items, claim =>
        {
          return claim.value !== 'none' && claim.value !== 'false' && !!claim.value;
        }).length;
      },


      onChange()
      {
        let claims = [];

        this.permissions.forEach(collection =>
        {
          collection.items.forEach(permission =>
          {
            if (
              (permission.valueType === 'boolean') ||
              (permission.valueType === 'crud') ||
              (permission.valueType === 'string'))
            {
              claims.push({
                //$type: 'zero.Core.Entities.UserClaim, zero.Core',
                type: 'zero.claim.permission',
                value: permission.key + ":" + permission.value
              });
            }
          });
        });

        //console.info(claims);

        this.$emit('input', claims);
      },


      onPermissionToggle(isOn, permission)
      {
        if (!isOn)
        {
          permission._oldValue = permission.value;
          permission.value = 'none';
        }
        else
        {
          permission.value = permission._oldValue || 'read,create,update,delete';
        }
        this.onChange();
      },


      onPermissionCRUDChecked(val, permission)
      {
        if (val.indexOf('read') < 0)
        {
          val.push('read');
        }
        permission.value = val.join(',').trim(',');
        this.onChange();
      },


      parsePermissionValue(value, type)
      {
        if (type === 'boolean')
        {
          return value === 'true';
        }
        else if (type === 'crud' && !value)
        {
          return 'none';
        }
        return value;
      }
    }
  }
</script>

<style lang="scss">
  .ui-permissions > .ui-property + .ui-property
  {
    border-top: 1px solid var(--color-line);
    padding-top: 40px;
    margin-top: 40px;
  }

  .ui-permissions > .ui-property > .ui-property-label
  {
    width: 300px;
  }

  .ui-permissions .ui-tab
  {
    border-top: 1px solid var(--color-line);
    padding-top: var(--padding);
    //background: var(--color-bg-dim);
    //border-radius: var(--radius);
    //padding: var(--padding);
  }

  .role-permission-toggle
  {
    border-top-width: 0 !important;
  }

  .ui-permissions-crud
  {
    display: flex;

    .ui-toggle
    {
      margin-right: var(--padding);
    }

    .ui-check-list
    {
      margin-top: 1px;
    }
  }
</style>