<template>
  <div class="ui-permissions ui-box">
    <ui-property v-for="(permissionCollection, index) in permissions" :key="index" :label="permissionCollection.label" :description="permissionCollection.description">
      <ui-error field="Claims" />
      <ui-property v-for="(permission, index) in permissionCollection.items" :key="index" class="role-permission-toggle" :label="permission.label" :description="permission.description">
        <ui-toggle v-if="permission.valueType === 'boolean'" :disabled="disabled" v-model="permission.value" @input="onChange" />
        <div class="ui-permissions-crud" v-if="permission.valueType === 'crud'">
          <ui-toggle :value="permission.value != 'none'" :disabled="disabled" @input="onPermissionToggle($event, permission)" />
          <ui-check-list :value="permission.value.split(',')" :items="stateItems" v-if="permission.value != 'none'" :inline="true" @input="onPermissionCRUDChecked($event, permission)" />
        </div>
        <!--<ui-state-button v-if="permission.valueType === 'crud'" :disabled="disabled" :items="stateItems" v-model="permission.value" @input="onChange" />-->
        <input v-if="permission.valueType === 'string'" :disabled="disabled" v-model="permission.value" type="text" class="ui-input" @input="onChange" />
      </ui-property>
    </ui-property>
  </div>
</template>


<script>
  import UserRolesApi from 'zero/resources/userRoles';

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
        { name: '@permission.states.read', id: 'read' },
        { name: '@permission.states.create', id: 'create' },
        { name: '@permission.states.update', id: 'update' },
        { name: '@permission.states.delete', id: 'delete' }
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
    border-top: 1px solid var(--color-line-light);
    padding-top: 40px;
    margin-top: 40px;
  }

  .ui-permissions > .ui-property > .ui-property-label
  {
    width: 300px;
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

    .ui-native-check:first-child
    {
      display: none;
    }
  }
</style>