<template>
  <ui-form ref="form" class="page page-editor" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@page.name" :disabled="disabled" :is-create="!id" :state="form.state" :active-toggle="true" :can-delete="meta.canDelete" @delete="onDelete">
      <template v-slot:actions>
        <ui-dropdown-button label="@page.preview.title" icon="fth-eye" :disabled="disabled" @click="openPreview" />
        <ui-dropdown-separator />
        <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move(model)" />
        <ui-dropdown-button label="@ui.copy.title" icon="fth-copy" @click="copy(model)" />
        <ui-dropdown-separator />
      </template>
    </ui-form-header>

    <div v-if="preview.open" class="page-editor-preview-message">
      <div class="-text">
        <span>To update the <b>preview</b> with your unsaved changes click the <b>Refresh</b> button.</span>
      </div>
      <div class="-buttons">
        <ui-button type="small light onbg" label="Exit" @click="exitPreview()" />
        <ui-button type="small light onbg" label="Open" @click="focusPreview" />
        <ui-button type="small" label="Refresh" icon="fth-rotate-cw" @click="refreshPreview" />
      </div>
    </div>

    <ui-editor v-if="!loading" :config="renderer" v-model="model" :meta="meta" :is-page="true" infos="none" :on-configure="onEditorConfigure" />
  </ui-form>
</template>


<script>
  import UiEditor from 'zero/editor/editor';
  import PagesApi from 'zero/resources/pages';
  import EventHub from 'zero/services/eventhub';
  import InfoTab from './page-info';
  import Overlay from 'zero/services/overlay.js'
  import MoveOverlay from './overlays/move'
  import CopyOverlay from './overlays/copy'
  import Strings from 'zero/services/strings';
  import { find as _find, debounce as _debounce } from 'underscore';

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
      },
      preview: {
        open: false,
        window: null
      },
      debouncedUpdatePreview: null
    }),


    computed: {
      isCreate()
      {
        return this.$route.name === 'page-create';
      }
    },


    watch: {
      model: {
        deep: true,
        handler(value)
        {
          this.debouncedUpdatePreview(value);
        }
      }
    },


    mounted()
    {
      this.debouncedUpdatePreview = _debounce(this.updatePreview, 1000);

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
          this.renderer = response.entity ? 'page.' + response.entity.pageTypeAlias : null;
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

            // store last edited page in localstorage
            localStorage.setItem('zero.last-page.' + response.model.appId, response.model.id);
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
      },


      move(item)
      {
        return Overlay.open({
          component: MoveOverlay,
          display: 'editor',
          model: item
        }).then(value =>
        {
          EventHub.$emit('page.move', value);
          EventHub.$emit('page.update');
        });
      },


      copy(item)
      {
        return Overlay.open({
          component: CopyOverlay,
          display: 'editor',
          model: item
        }).then(value =>
        {
          EventHub.$emit('page.update');
        });
      },


      openPreview()
      {
        if (this.preview.open)
        {
          return this.focusPreview();
        }

        const id = Strings.guid();

        this.preview.window = window.open(window.location.origin + '/zero/preview?id=' + id, 'blank');
        this.preview.window.focus();

        window.addEventListener("message", event =>
        {
          this.preview.window.postMessage({
            id: id,
            preview: true,
            model: this.model
          }, window.location.origin);
        }, false);

        this.preview.window.onbeforeunload = () => this.exitPreview(true);

        this.preview.open = true;
      },

      updatePreview(value)
      {
        if (!this.preview.open)
        {
          return;
        }

        this.preview.window.postMessage({
          preview: true,
          update: true,
          model: value
        }, window.location.origin);
      },

      focusPreview()
      {
        this.preview.window.focus();
      },

      refreshPreview()
      {
        this.focusPreview();
        //this.preview.window.location.reload();
      },

      exitPreview(external)
      {
        if (!external)
        {
          this.preview.window.close();
        }
        this.preview.window = null;
        this.preview.open = false;
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