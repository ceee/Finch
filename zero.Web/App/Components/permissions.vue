<template>
  <div class="ui-permissions">
    <div v-for="permissionCollection in permissions" class="ui-box">
      <h2 class="ui-headline">
        {{ permissionCollection.label | localize }}
        <span v-if="permissionCollection.description" class="-minor"><br>{{ permissionCollection.description | localize }}</span>
      </h2>
      <ui-property v-for="permission in permissionCollection.items" class="role-permission-toggle" :label="permission.label" :description="permission.description">
        <ui-toggle v-if="permission.valueType === 'boolean'" v-model="permission.value" />
        <ui-state-button v-if="permission.valueType === 'readWrite'" :items="stateItems" v-model="permission.value" />
        <input v-if="permission.valueType === 'string'" v-model="permission.value" type="text" class="ui-input" />
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