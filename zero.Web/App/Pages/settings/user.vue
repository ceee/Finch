<template>
  <div class="user">
    <ui-header-bar title="Tobias Klika" :on-back="onBack">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-button type="light" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list :items="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button label="Save" />
    </ui-header-bar>
  </div>
</template>


<script>
  export default {
    name: 'app-settings-user',

    data: () => ({
      actions: []
    }),

    created()
    {
      this.actions.push({
        name: 'Disable',
        icon: 'fth-minus-circle'
      });
      this.actions.push({
        name: 'Change password',
        icon: 'fth-lock'
      });
      this.actions.push({
        name: 'Delete',
        icon: 'fth-x'
      });
    },


    methods: {

      onBack()
      {
        this.$router.go(-1);
      },

      actionSelected(item, dropdown)
      {
        dropdown.hide();
      },

    }
  }
</script>


<style lang="scss">
  
</style>