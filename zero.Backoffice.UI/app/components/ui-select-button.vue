<template>
  <button type="button" class="ui-select-button type-light" :disabled="disabled" @click="tryClick">
    <span class="ui-select-button-icon" v-if="!isImage">
      <ui-icon :symbol="icon" />
    </span>
    <span class="ui-select-button-icon is-image" v-if="isImage">
      <ui-thumbnail :media="icon" :alt="icon" />
    </span>
    <div class="ui-select-button-content" v-if="label || description">
      <strong class="ui-select-button-label" v-localize:html="{ key: label, tokens: tokens }"></strong>
      <span class="ui-select-button-description" v-if="description" v-localize:html="{ key: description, tokens: tokens }"></span>
    </div>
    <slot></slot>
  </button>
</template>


<script>
  //import MediaApi from 'zero/api/media.js'; // TODO vue

  export default {
    name: 'uiSelectButton',

    props: {
      icon: {
        type: String,
        default: 'fth-box'
      },
      iconAsImage: {
        type: Boolean,
        default: false
      },
      label: {
        type: String,
        default: null
      },
      description: {
        type: String,
        default: null
      },
      tokens: {
        type: Object,
        default: () => {}
      },
      disabled: Boolean
    },


    computed: {
      isImage()
      {
        return this.iconAsImage && (!this.icon || this.icon.indexOf('fth-') !== 0);
      }
    },


    methods: {

      tryClick(ev)
      {
        this.$emit('click', ev);
      }
    }
  }
</script>