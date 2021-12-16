<template>
  <div class="media-selection">
    <ui-button type="light onbg" label="@media.selection.clear" @click="clearSelection" />
    <ui-dropdown align="right">
      <template v-slot:button>
        <ui-button type="accent" :label="localize('@media.selection.selected', { tokens: { count: selected.length }})" caret="down" />
      </template>
      <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move" />
      <ui-dropdown-button label="@ui.delete" icon="fth-trash" @click="remove" />
    </ui-dropdown>
  </div>
</template>

<script>
  import { localize } from '../../../../services/localization';

  export default {
    name: 'mediaOverviewSelection',

    props: {
      selected: {
        type: Array,
        default: () => []
      }
    },

    methods: {

      localize,

      clearSelection()
      {
        this.$emit('clear');
      },


      move()
      {
        this.$emit('move', this.selected);
      },


      remove()
      {
        this.$emit('remove', this.selected);
      }
    }
  };
</script>

<style lang="scss">
  .media-selection
  {
    display: flex;
    gap: 10px;
  }
</style>
