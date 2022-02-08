<template>
  <div class="ui-mediapicker" :class="{'is-disabled': disabled }" v-sortable="{ draggable: '.ui-media-preview', onUpdate: onSortingUpdated, filter: '.ui-mediapicker-add' }">
    <media-previews :value="value" :disabled="disabled" @remove="remove" />
    <button type="button" class="ui-mediapicker-add" :class="{ 'is-full': !this.value || this.value.length < 1 }" v-if="canAdd" @click="pick">
      <ui-icon symbol="fth-plus" :size="17" />
    </button>
  </div>
</template>


<script>
  import api from '../api';
  import { arrayMove } from '../../../utils';
  import * as overlays from '../../../services/overlay';
  import MediaPreviews from './media-previews.vue';

  export default {
    name: 'uiMediapicker',

    props: {
      value: {
        type: [String, Array],
        default: null
      },
      limit: {
        type: Number,
        default: 1
      },
      disabled: {
        type: Boolean,
        default: false
      },
      for: {
        type: String,
        default: 'all'
      },
      disableSelect: {
        type: Boolean,
        default: false
      },
      disableUpload: {
        type: Boolean,
        default: false
      },
      fileExtensions: {
        type: Array,
        default: []
      },
    },

    components: { MediaPreviews },

    data: () => ({
      pickerConfig: {}
    }),

    computed: {
      canAdd()
      {
        return this.limit > 1 ? (Array.isArray(this.value) ? this.limit > this.value.length : true) : (!this.value || this.value.length < 1);
      }
    },

    methods: {
      onChange(value)
      {
        if (this.limit < 2 && Array.isArray(value))
        {
          value = value.length > 0 ? value[0] : null;
        }

        this.$emit('input', value);
        this.$emit('update:value', value);
      },

      async onSortingUpdated(ev)
      {
        let value = this.value;

        if (Array.isArray(value))
        {
          value = arrayMove(value, ev.oldIndex, ev.newIndex);
        }

        this.onChange(value);
      },

      async pick()
      {
        const result = await overlays.open({
          component: () => import('../overlays/media.vue'),
          display: 'editor',
          width: 1240,
          model: {
            parentId: null
          }
        });

        if (result.eventType == 'confirm' && result.value)
        {
          this.add(result.value);
        }
      },


      add(item)
      {
        let value = this.value;

        if (Array.isArray(value))
        {
          value.push(item.id);
        }
        else if (this.limit > 1)
        {
          if (value)
          {
            value = [value];
          }
          value.push(item.id);
        }
        else
        {
          value = item.id;
        }

        this.onChange(value);
      },


      remove(item)
      {
        let value = this.value;

        if (Array.isArray(value))
        {
          const index = value.indexOf(item.id);
          value.splice(index, 1);
        }
        else
        {
          value = null;
        }

        this.onChange(value);
      }
    }
  }
</script>

<style lang="scss">

  .ui-mediapicker
  {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    gap: 10px;
  }

  .ui-mediapicker-add
  {
    width: 40px;
    height: 80px;
    display: inline-flex;
    justify-content: center;
    align-items: center;
    border-radius: var(--radius-inner);
    background: var(--color-button-light);
    border: 1px solid transparent;
    color: var(--color-text);
    text-align: center;
    font-size: 16px;
    flex-shrink: 0;
    overflow: hidden;

    &.is-full
    {
      width: 80px;
    }
  }

</style>