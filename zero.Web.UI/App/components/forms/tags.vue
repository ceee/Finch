 <template>
  <div class="ui-tags" :class="{'is-disabled': disabled }">
    <ui-input-list :value="value" @input="$emit('input', $event)" :add-label="addLabel" :disabled="disabled" :max-items="maxItems" :max-length="maxLength" :plus-button="true" />
  </div>
</template>


<script>
  export default {
    name: 'uiTags',

    props: {
      addLabel: {
        type: String,
        default: '@ui.add'
      },
      value: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      },
      maxItems: {
        type: Number,
        default: 100
      },
      maxLength: {
        type: Number,
        default: 200
      }
    }
  }
</script>

<style lang="scss">
  .ui-tags
  {
    .ui-input-list
    {
      display: flex;
      align-items: flex-start;
      flex-wrap: wrap;
    }

    .ui-input-list-item
    {
      display: inline-grid;
      min-width: 0;

      .ui-input
      {
        min-width: 120px;
        width: auto;
      }

      & + .ui-input-list-item, & + .ui-button, & + .ui-select-button
      {
        margin-top: 0;
        margin-left: 6px;
        margin-bottom: 8px;
      }
    }

  }
</style>