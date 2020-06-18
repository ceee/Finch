<template>
  <button type="button" class="ui-select-button type-light" :disabled="disabled" @click="tryClick">
    <i class="ui-select-button-icon" :class="icon" v-if="!isImage"></i>
    <span class="ui-select-button-icon is-image" v-if="isImage">
      <img :src="source" :alt="icon" />
    </span>
    <content class="ui-select-button-content" v-if="label || description">
      <strong class="ui-select-button-label" v-localize:html="{ key: label, tokens: tokens }"></strong>
      <span class="ui-select-button-description" v-if="description" v-localize:html="{ key: description, tokens: tokens }"></span>
    </content>
  </button>
</template>


<script>
  import MediaApi from 'zero/resources/media';

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
        type: String
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
        return this.iconAsImage && this.icon.indexOf('fth-') !== 0;
      },
      source()
      {
        return this.iconAsImage ? MediaApi.getImageSource(this.icon) : null;
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