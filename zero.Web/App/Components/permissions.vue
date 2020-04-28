<template>
  <div class="ui-permissions">
    <div v-for="permissionCollection in permissions" class="ui-box">
      <h2 class="ui-headline">
        {{ permissionCollection.label | localize }}
        <span v-if="permissionCollection.description" class="-minor"><br>{{ permissionCollection.description | localize }}</span>
      </h2>
      <ui-property v-for="permission in permissionCollection.items" class="role-permission-toggle" :label="permission.label" :description="permission.description">
        <ui-toggle v-if="permission.valueType === 'boolean'" :disabled="disabled" v-model="permission.value" @input="onChange" />
        <ui-state-button v-if="permission.valueType === 'readWrite'" :disabled="disabled" :items="stateItems" v-model="permission.value" @input="onChange" />
        <input v-if="permission.valueType === 'string'" :disabled="disabled" v-model="permission.value" type="text" class="ui-input" @input="onChange" />
      </ui-property>
    </div>
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
        { label: '@permission.states.none', value: 'none' },
        { label: '@permission.states.read', value: 'read' },
        { label: '@permission.states.write', value: 'write' }
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
              (permission.valueType === 'boolean' && permission.value === true) ||
              (permission.valueType === 'readWrite' && permission.value !== 'none') ||
              (permission.valueType === 'string' && !!permission.value))
            {
              claims.push({
                //$type: 'zero.Core.Entities.UserClaim, zero.Core',
                type: 'zero.claim.permission',
                value: permission.key + ":" + permission.value
              });
            }
          });
        });

        this.$emit('input', claims);
      },

      parsePermissionValue(value, type)
      {
        if (type === 'boolean')
        {
          return value === 'true';
        }
        else if (type === 'readWrite' && !value)
        {
          return 'none';
        }
        return value;
      }
    }
  }
</script>

<style lang="scss">
  
</style>