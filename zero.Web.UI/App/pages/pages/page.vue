<template>
  <ui-form ref="form" class="page page-editor" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-header-bar :title="title" :on-back="onBack">
      <ui-button type="white" icon="fth-eye" title="Preview" />
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-button type="white" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button :submit="true" label="Save" />
    </ui-header-bar>

    <ui-editor v-if="!loading" :config="renderer" v-model="model" :meta="meta" :is-page="true" infos="none" :on-configure="onEditorConfigure" :active-tab="2" />
  </ui-form>
</template>


<script>
  import UiEditor from 'zero/editor/editor';
  import PagesApi from 'zero/resources/pages';
  import InfoTab from './page-info';

  export default {

    props: ['id', 'type', 'parent'],

    components: { UiEditor },

    data: () => ({
      loading: true,
      renderer: null,
      actions: [],
      meta: {},
      pageType: {},
      model: {
        name: null,
        options: {
          hideInNavigation: false
        },
        link: null
      }
    }),


    computed: {
      isCreate()
      {
        return this.$route.name === 'page-create';
      },
      title()
      {
        return this.isCreate ? 'Create new page' : this.model.name;
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
        
      },

      actionSelected(item, dropdown)
      {
        dropdown.hide();
      },

      onBack()
      {
        this.$router.go(-1);
      },

      onLoad(form)
      {
        form.load(!this.id ? PagesApi.getEmpty(this.type, this.parent) : PagesApi.getById(this.id)).then(response =>
        {
          this.renderer = 'page.' + response.entity.pageTypeAlias;
          this.model = response.entity;
          this.meta = response.meta;
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        //this.fullModel.model = this.model;

       //console.info(JSON.parse(JSON.stringify(this.model)));
        form.handle(PagesApi.save(this.model)).then(response =>
        {
          console.info(response);
        });
      },


      onEditorConfigure(editor)
      {
        editor.tabs.push({
          label: 'Info',
          name: 'zero.info',
          class: 'is-info is-blank',
          fields: [],
          component: InfoTab,
          count: () => null
        });
      }

    }
  }
</script>

<style lang="scss">
  .page-editor .ui-tab.is-info
  {
    
  }

  .page-editor-info
  {
    .editor-infos
    {
      margin: 0;
      padding: 0;
    }
  }
</style>