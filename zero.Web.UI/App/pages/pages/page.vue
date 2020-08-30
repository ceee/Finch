<template>
  <ui-form ref="form" class="page page-editor" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@page.name" :disabled="disabled" :is-create="!id" :state="form.state" :active-toggle="true" :can-delete="meta.canDelete" @delete="onDelete">
      <template v-slot:actions>
        <ui-dropdown-button label="@page.preview.title" icon="fth-eye" :disabled="disabled" />
      </template>
    </ui-form-header>

    <ui-editor v-if="!loading" :config="renderer" v-model="model" :meta="meta" :is-page="true" infos="none" :on-configure="onEditorConfigure" />
  </ui-form>
</template>


<script>
  import UiEditor from 'zero/editor/editor';
  import PagesApi from 'zero/resources/pages';
  import EventHub from 'zero/services/eventhub';
  import InfoTab from './page-info';
  import { find as _find } from 'underscore';

  export default {

    props: ['id', 'type', 'parent'],

    components: { UiEditor },

    data: () => ({
      loading: true,
      disabled: false,
      renderer: null,
      actions: [],
      meta: {},
      pageType: {},
      route: 'page',
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
      }
    },


    mounted()
    {
      EventHub.$on('page.sort', items =>
      {
        let item = _find(items, x => x.id === this.id);
        if (item)
        {
          this.model.sort = item.sort;
        }
      });

      EventHub.$on('page.move', item =>
      {
        if (item.id === this.id)
        {
          this.model.parentId = item.parentId;
        }
      });

      EventHub.$on('page.delete', ids =>
      {
        if (ids.indexOf(this.id) > -1)
        {
          this.$router.replace({ name: 'pages' });
        }
      });
    },


    methods: {

      onLoad(form)
      {
        this.loading = true;

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
        form.handle(PagesApi.save(this.model)).then(response =>
        {
          if (response.success)
          {
            EventHub.$emit('page.update', response.model);
            this.model = response.model;
          }
        });
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(PagesApi.delete.bind(this, this.id));
      },


      onEditorConfigure(editor)
      {
        editor.tabs.push({
          label: '@page.info_tab',
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