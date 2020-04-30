<template>
  <ui-form ref="form" class="space-editor" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-header-bar :title="space.name" title-empty="Space">
      <!--<ui-dropdown align="right" v-if="!disabled">
        <template v-slot:button>
          <ui-button type="light" label="@ui.actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" />
      </ui-dropdown>-->
      <ui-button :submit="true" label="@ui.save" icon="fth-check" :state="form.state" v-if="!disabled" />
    </ui-header-bar>
    <ui-editor />
  </ui-form>
</template>


<script>
  import SpacesApi from 'zero/resources/spaces.js';
  import UiEditor from 'zero/editor/editor';

  export default {
    props: ['config', 'space'],

    components: { UiEditor },

    data: () => ({
      disabled: false,
      actions: []
    }),

    watch: {
      '$route': 'load'
    },

    created()
    {
      this.actions.push({
        name: 'Delete',
        icon: 'fth-trash'
      });
    },


    methods: {

      onLoad(form)
      {
        
      },


      onSubmit(form)
      {
        
      },

    }
  }
</script>