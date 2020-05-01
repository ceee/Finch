<template>
  <ui-form ref="form" class="space-editor" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-header-bar :back-button="isList" :title="title" title-empty="Space">
      <ui-dropdown v-if="isList && !disabled" align="right">
        <template v-slot:button>
          <ui-button type="light" label="@ui.actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" />
      </ui-dropdown>
      <ui-button :submit="true" label="@ui.save" icon="fth-check" :state="form.state" v-if="!disabled" />
    </ui-header-bar>
    <ui-editor v-if="renderer" :config="renderer" />
  </ui-form>
</template>


<script>
  import Axios from 'axios';
  import SpacesApi from 'zero/resources/spaces.js';
  import UiEditor from 'zero/editor/editor';

  export default {
    props: ['config', 'space'],

    components: { UiEditor },

    data: () => ({
      disabled: false,
      renderer: {},
      actions: []
    }),

    computed: {
      isList()
      {
        return !!this.$route.params.id;
      },
      title()
      {
        return this.isList ? 'My item' : this.space.name;
      }
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
        form.load(Axios.get('test/getRenderer', { params: { alias: this.$route.params.alias } })).then(response =>
        {
          this.renderer = response.data;
        });
      },


      onSubmit(form)
      {
        
      },

    }
  }
</script>