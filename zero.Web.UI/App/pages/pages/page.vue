<template>
  <div class="page">
    <ui-header-bar :title="title" :on-back="onBack">
      <ui-dropdown>
        <template v-slot:button>
          <ui-button type="white" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button type="white" label="Preview" icon="fth-eye" />
      <ui-button label="Save" />
    </ui-header-bar>
  </div>
</template>


<script>
  export default {

    props: [ 'id', 'type' ],

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


    computed: {
      isCreate()
      {
        return this.$route.name === 'page-create';
      },
      title()
      {
        return this.isCreate ? 'Create new page' : 'Page ' + this.id;
      }
    },


    watch: {
      '$route': function ()
      {
        this.initialize();
      }
    },


    mounted()
    {
      this.initialize();
    },


    methods: {

      initialize()
      {
        console.info(this.id, this.type);
      },

      actionSelected(item, dropdown)
      {
        dropdown.hide();
      },

      onBack()
      {
        this.$router.go(-1);
      }

    }
  }
</script>