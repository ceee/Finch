<template>
  <ui-form ref="form" class="space-editor" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-header-bar :back-button="isList" :title="title" title-empty="Space">
      <ui-dropdown v-if="isList && !disabled" align="right">
        <template v-slot:button>
          <ui-button type="white" label="@ui.actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" />
      </ui-dropdown>
      <ui-button :submit="true" label="@ui.save" :state="form.state" v-if="!disabled" />
    </ui-header-bar>
    <ui-editor v-if="renderer" :config="renderer" v-model="model" />
  </ui-form>
</template>


<script>
  import SpacesApi from 'zero/resources/spaces.js';
  import UiEditor from 'zero/editor/editor';
  import Overlay from 'zero/services/overlay.js';

  export default {
    props: ['config', 'space'],

    components: { UiEditor },

    data: () => ({
      disabled: false,
      renderer: {},
      actions: [],
      model: null,
      fullModel: null
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
        icon: 'fth-trash',
        action: this.onDelete
      });
    },

    beforeRouteLeave(to, from, next) 
    {
      this.$refs.form.beforeRouteLeave(to, from, next);
    },

    methods: {

      onLoad(form)
      {
        form.load(SpacesApi.getContent(this.$route.params.alias, this.$route.params.id)).then(response =>
        {
          this.renderer = response.config;
          this.model = response.model;
          this.fullModel = response;
        });
      },


      onSubmit(form)
      {
        this.fullModel.model = this.model;

        form.handle(SpacesApi.save(this.fullModel)).then(response =>
        {
          console.info(response);
        });
      },


      onDelete(item, opts)
      {
        opts.hide();

        Overlay.confirmDelete().then((opts) =>
        {
          opts.state('loading');

          SpacesApi.delete(this.$route.params.alias, this.$route.params.id).then(response =>
          {
            if (response.success)
            {
              opts.state('success');
              opts.hide();
              this.$router.go(-1);
              // TODO show message
            }
            else
            {
              opts.errors(response.errors);
            }
          });
        });
      }
    }
  }
</script>

<style lang="scss">
  .space-editor .ui-header-bar + .editor > .ui-box
  {
    margin-top: 0;
  }
</style>