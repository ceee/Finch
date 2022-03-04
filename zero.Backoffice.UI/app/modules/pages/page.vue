<template>
  <ui-form :key="id" ref="form" class="page page-editor" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model:value="model" title="@page.name" :disabled="disabled" :is-create="!id" :state="form.state" :active-toggle="!isFolder" :can-delete="meta.canDelete" @delete="onDelete" :sticky="true">
      <template v-slot:actions>
        <ui-form-header-links :value="model" :url="getUrl" :preview-enabled="true" />
        <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move(model)" />
        <ui-dropdown-button label="@ui.copy.title" icon="fth-copy" @click="copy(model)" />
      </template>
    </ui-form-header>

    <ui-editor v-if="!loading && editor" :config="editor" v-model="model" :is-page="true" infos="aside" :meta="meta" :disabled="disabled" :scope="true" />

    <div v-if="isFolder">
      <!-- // TODO list children -->
    </div>
  </ui-form>
</template>


<script lang="ts">
  import { defineComponent } from 'vue';
  import api from './api';
  import actions from './actions';
  import PageInfoTab from './partials/page-info.vue';

  export default defineComponent({

    name: 'page',

    props: ['id', 'flavor', 'parent'],

    data: () => ({
      loading: true,
      disabled: false,
      editor: null,
      actions: [],
      meta: {},
      pageType: {},
      route: 'pages-edit',
      model: {
        id: null,
        name: null,
        options: {
          hideInNavigation: false
        },
        link: null
      },
      isFolder: false,
      getUrl: null
    }),


    computed: {
      isCreate()
      {
        return this.$route.name === 'pages-create' || !this.id;
      }
    },


    mounted()
    {
      this.zero.events.on('page.sort', items =>
      {
        let item = items.find(x => x.id === this.id);
        if (item)
        {
          this.model.sort = item.sort;
        }
      });

      this.zero.events.on('page.move', item =>
      {
        if (item.id === this.id)
        {
          this.model.parentId = item.parentId;
        }
      });

      this.zero.events.on('page.delete', ids =>
      {
        if (ids.indexOf(this.id) > -1)
        {
          this.$router.replace({ name: 'pages' });
        }
      });

      this.getUrl = async (model: any) => await api.getUrl(model.id);
    },


    //beforeRouteUpdate(to, from, next)
    //{
    //  next();
    //  this.$nextTick(() =>
    //  {
    //    this.onLoad(this.$refs.form);
    //  });
    //},


    methods: {

      async onLoad(form)
      {
        this.loading = true;

        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = await form.load(() => !this.isCreate ? api.getById(this.id, undefined, config) : api.getEmpty(this.flavor, this.parent, config));
        this.model = response;

        if (this.model)
        {
          this.isFolder = this.model.flavor == 'zero.folder';
          this.editor = 'pages:' + this.model.flavor;
        }

        this.loading = false;
        //  this.disabled = !response.meta.canEdit;
        //  this.editor = response.entity ? this.zero.getEditor('pages.' + response.entity.pageTypeAlias) : null;
        //  this.model = response.entity;
        //  this.meta = response.meta;
        //  this.loading = false;
        //});
      },


      async onSubmit(form)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = !this.isCreate ? await api.update(this.model, config) : await api.create(this.model, config);
        await form.handle(response);

        if (response.success)
        {
          this.zero.events.emit('page.update', { action: 'save', create: this.isCreate, model: response.model });
          if (this.isCreate)
          {
            this.zero.events.emit('page.create', response.model);
          }
          this.model = response.data;
          localStorage.setItem('zero.last-page.hofbauer' /* // TODO v3 appid + response.model.appId */, response.data.id);
        }
      },


      async onDelete(item, opts)
      {
        opts.hide();
        await actions.remove(this.model);
      },


      async move(item)
      {
        await actions.move(item);
      },


      async copy(item)
      {
        await actions.copy(item);
      }
    }
  })
</script>

<style lang="scss">
  .page-editor
  {
    overflow-y: auto;
    height: 100%;
  }

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

  .page-editor-preview-message
  {
    margin: -10px var(--padding) var(--padding);
    background: var(--color-box-light);
    color: var(--color-primary);
    font-size: var(--font-size);
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 16px 20px;
    border-radius: var(--radius);
    position: relative;
    line-height: 20px;
    text-align: left;

    .-buttons
    {
      display: flex;
    }
  }
</style>