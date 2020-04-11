<template>
  <div class="page">
    <ui-header-bar :title="'Page ' + $route.params.id" :on-back="onBack">
      <ui-dropdown>
        <template v-slot:button>
          <ui-button type="light" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list :items="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button type="light" label="Preview" icon="fth-eye" />
      <ui-button label="Save" />
    </ui-header-bar>
  </div>
</template>


<script>
  export default {

    data: () => ({
      actions: []
    }),

    created()
    {
      this.actions.push({
        name: 'Create',
        icon: 'fth-plus'
      });
      this.actions.push({
        name: 'Move',
        icon: 'fth-corner-down-right'
      });
      this.actions.push({
        name: 'Copy',
        icon: 'fth-copy',
        disabled: true
      });
      this.actions.push({
        name: 'Sort',
        icon: 'fth-arrow-down'
      });
      this.actions.push({
        type: 'separator'
      });
      this.actions.push({
        name: 'Delete',
        icon: 'fth-x',
        action(item, dropdown)
        {
          dropdown.hide();
        }
      });
    },


    methods: {

      actionSelected(item, dropdown)
      {
        dropdown.hide();
      },

      onBack()
      {
        console.info('back');
      }

    }
  }
</script>