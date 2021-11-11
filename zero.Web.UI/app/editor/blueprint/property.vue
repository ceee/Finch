<template>
  <div class="blueprint" :class="{'is-active': entity.isActive, 'is-shared': isBlueprint }">
    <div class="blueprint-box">
      <template v-if="hasBlueprint && !isBlueprint">
        <div class="blueprint-inner">
          <ui-icon symbol="fth-cloud" :size="22" />
          <p v-localize:html="'@blueprint.hint.childText'"></p>
        </div>
        <aside>
          <ui-button class="blueprint-button-settings" type="blank small" icon="fth-settings" :title="{ key: value.desync.length > 0 ? '@blueprint.hint.xUnlocked' : '@blueprint.hint.settingsButton', tokens: { count: value.desync.length }}" @click="openSettings" />
          <router-link replace :to="switchLink" class="ui-button type-small" v-localize="'@blueprint.hint.goToBlueprint'"></router-link>
        </aside>
      </template>
      <template v-if="isBlueprint">
        <div class="blueprint-inner">
          <ui-icon symbol="fth-cloud" :size="22" />
          <p v-if="!isNewBlueprint" v-localize:html="'@blueprint.hint.parentText'"></p>
          <p v-if="isNewBlueprint" v-localize:html="'@blueprint.hint.parentCreateText'"></p>
        </div>
        <aside v-if="!isNewBlueprint">
          <router-link replace :to="switchLink" class="ui-button type-small" v-localize="'@blueprint.hint.goToChild'"></router-link>
        </aside>
      </template>
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/helpers/overlay.js';
  import SettingsOverlay from './settings.vue';
  import BlueprintBlockComponent from './block.vue';

  export default {
    props: {
      value: {
        type: Object
      },
      entity: {
        type: Object,
        required: true
      },
      meta: {
        type: Object,
        default: () => { }
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    inject: ['editor'],

    watch: {
      '$route': function ()
      {
        this.bind();
      }
    },

    computed: {
      hasBlueprint()
      {
        return !!this.value;
      },
      isBlueprint()
      {
        return this.entity && this.$route.query.scope === 'shared';
      },
      isNewBlueprint()
      {
        return this.entity && !this.entity.id && this.$route.query.scope === 'shared';
      },
      switchLink()
      {
        return {
          name: this.$route.name,
          params: this.$route.params,
          query: {
            ...(this.$route.query || {}),
            scope: this.isBlueprint ? undefined : 'shared'
          }
        };
      }
    },

    mounted()
    {
      this.bind();
    },

    methods: {

      openSettings()
      {
        const editor = typeof this.editor === 'string' ? this.zero.getEditor(this.editor) : this.editor;

        return Overlay.open({
          component: SettingsOverlay,
          display: 'editor',
          model: this.value,
          editor
        }).then(res =>
        {
          this.$emit('input', res.blueprint);
          if (typeof res.update === 'function')
          {
            res.update(this.entity);
          }
          //EventHub.$emit('page.update');
        });
      },

      bind()
      {
        const form = this.getForm();
        const onLoaded = () =>
        {
          this.$nextTick(() =>
          {
            this.setupSync(form);
          });
        };

        if (form.loadingState === 'default')
        {
          onLoaded();
        }
        else
        {
          form.$on('loaded', onLoaded);
        }
      },

      setupSync(form)
      {
        const meta = form.$parent.meta;

        if (meta)
        {
          meta.canDelete = this.isBlueprint;
          meta.canEdit = this.isBlueprint;
        }

        if (this.hasBlueprint)
        {
          const properties = this.getProperties(form);

          properties.forEach(property =>
          {
            if (property.config.path !== 'blueprint')
            {
              //property.setBlock(BlueprintBlockComponent);
              //property.setDisabled(true);
            }
            //property.$el.classList.add('is-property-locked');
            //property.setLocked(true);
          });
        }
      },

      getForm()
      {
        let component = this.$parent;

        do
        {
          if (component.$options.name === 'uiForm')
          {
            return component;
          }
        }
        while (component = component.$parent);

        return null;
      },

      // find all form properties
      getProperties(form)
      {
        let find = 'uiEditorComponent';
        let components = [];

        // find components which can output errors
        let traverseChildren = (parent) =>
        {
          parent.$children.forEach(component =>
          {
            if (component.$options.name === find)
            {
              components.push(component);
            }
            else
            {
              traverseChildren(component);
            }
          });
        };

        traverseChildren(form);

        return components;
      }
    }
  }
</script>

<style lang="scss">
  .ui-property.is-property-locked
  {
    pointer-events: none;
    opacity: .8;
  }

  /*.language .ui-property.has-block:after,
  .mails .ui-property.has-block:after
  {
    position: absolute;
    left: -13px;
    top: -7px;
    display: inline-flex;
    justify-content: center;
    align-items: center;
    width: 32px;
    height: 32px;
    border-radius: 16px;
    background: var(--color-box);
    content: "\e887";
    font-family: 'Feather';
    font-size: 15px;
    font-weight: 400;
    color: var(--color-text-dim);
  }*/


  .blueprint
  {
    position: relative;
    margin: 0 -32px 0;
    padding: 0 32px 0;
    margin-bottom: 30px;
    //background: var(--color-box-nested);
    border-top-left-radius: var(--radius);
    border-top-right-radius: var(--radius);

    aside
    {
      display: flex;
    }

    &.is-shared
    {
      //border-bottom: 1px dotted var(--color-accent-error);
    }
  }

  .blueprint-box
  {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: var(--padding-s);
    padding-left: var(--padding-m);
    border-radius: var(--radius);
    border: 1px dashed var(--color-line-dashed);
    //background: repeating-linear-gradient(-45deg, transparent, transparent 2px, var(--color-bg-shade-2) 2px, var(--color-bg-shade-2) 4px);
  }

  .blueprint-button-settings
  {
    margin-right: -5px;
  }

  .blueprint-inner
  {
    display: grid;
    grid-template-columns: auto minmax(0, 1fr);
    grid-gap: var(--padding-m);
    align-items: center;
    font-size: var(--font-size);
    line-height: 1.4;
    position: relative;

    p
    {
      margin: 0;
      color: var(--color-text);
    }

    .ui-icon
    {
      color: var(--color-text);
      margin-top: -2px;
    }
  }
</style>

<!--

  <button type="button" class="ui-property-lock" v-if="locked" > <i class="fth-lock" > </i > </button >
  /*.ui-property.is-locked
  {

  }*/
  .ui-property-lock
  {
    display: inline-flex;
    width: 20px;
    height: 20px;
    border-radius: 10px;
    background: var(--color-button-light);
    color: var(--color-text);
    justify-content: center;
    align-items: center;
    font-size: 10px;
    position: relative;
    top: -1px;
    margin-right: 8px;
  }

-->
