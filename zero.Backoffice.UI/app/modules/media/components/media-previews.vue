<template>
  <div v-for="item in items" :key="item.id" class="ui-media-preview" :class="{ 'media-pattern': !item.error, 'has-error': item.error }" v-localize:title="{ key: item.error ? item.error : item.name, tokens: { id: item.id } }">
    <template v-if="!item.error">
      <img class="-image" v-if="item.preview" :src="item.preview" />
      <span class="-icon" v-if="!item.preview"><ui-icon :symbol="(item.isFolder ? 'fth-folder' : 'fth-file')" :size="22" :stroke-width="2" /></span>
    </template>
    <ui-icon v-else symbol="fth-alert-circle" :size="22" />
    <ui-icon-button class="-remove" type="action" icon="fth-x" :stroke="2.5" @click="$emit('remove', item)" />
  </div> 
</template>


<script>
  import api from '../api';
  import { extendObject } from '../../../utils';
  import * as overlays from '../../../services/overlay';

  export default {
    name: 'uiMediaPreviews',

    props: {
      value: {
        type: [String, Array],
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      size: {
        type: String,
        default: 'big'
      }
    },

    data: () => ({
      items: [],
      cache: {}
    }),


    watch: {
      value: {
        deep: true,
        handler: async function ()
        {
          await this.reload();
        }
      }
    },


    mounted()
    {
      this.reload();
    },


    methods: {

      async reload()
      {
        const ids = this.value ? (Array.isArray(this.value) ? this.value : [this.value]) : [];

        if (!ids.length)
        {
          this.items = [];
          return;
        }

        let remoteIds = ids.filter(x => !this.cache[x]);
        let items = [];
        let result = { data: [] };

        if (remoteIds.length)
        {
          result = await api.getByQuery({ ids: remoteIds });
        }

        ids.forEach(id =>
        {
          let model = this.cache[id];

          if (!model)
          {
            model = result.data.find(x => x.id == id);
          }

          if (!model)
          {
            model = {
              id,
              error: '@media.notfound'
            };
          }
          else
          {
            this.cache[id] = model;
          }

          items.push(model);
        });


        this.items = items;
      }
    }
  }
</script>


<style lang="scss">

  .ui-media-previews
  {

  }

  .ui-media-preview
  {
    width: 80px;
    height: 80px;
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    background: var(--color-box);
    border-radius: var(--radius);
    overflow: hidden;
    position: relative;
    text-align: center;
    color: var(--color-text);
    position: relative;
  }

  .ui-media-preview.has-error
  {
    background: var(--color-box-nested);
    color: var(--color-negative);
  }

  .ui-media-preview .-image
  {
    width: 100%;
    height: 100%;
    object-fit: contain;
    position: relative;
    border-radius: var(--radius);
    z-index: 1;
  }

  .ui-media-preview .-icon
  {
    z-index: 1;
  }

  .ui-media-preview .-remove
  {
    position: absolute;
    right: 5px;
    bottom: 5px;
    width: 24px;
    height: 24px;
    z-index: 1;
    background: var(--color-box);
    box-shadow: var(--shadow-short);
    color: var(--color-negative);
    display: none;
  }

  .ui-media-preview:hover .-remove
  {
    display: inline-flex;
  }
</style>