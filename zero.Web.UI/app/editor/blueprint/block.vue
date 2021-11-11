<template>
  <div class="blueprint-block" @click="onClick">
   
  </div>
</template>


<script>
  import Overlay from 'zero/helpers/overlay.js';

  export default {
    props: {
      config: {
        type: Object,
        required: true
      },
      editor: {
        type: Object,
        required: true
      },
      value: {
        type: Object,
        required: true
      },
      model: {}
    },

    methods: {
      onClick()
      {
        Overlay.confirm({
          title: 'Unlock property',
          text: 'Unlock this property to override the value passed by the blueprint',
          confirmLabel: 'Confirm',
          closeLabel: 'Cancel'
        }).then(
          () =>
          {
            this.value.blueprint.desync.push(this.config.path);
            this.$parent.$parent.setDisabled(false);
            this.$parent.$parent.setBlock(null);
          },
          () =>
          {
            //this.dirty = false;
            //next();
          }
        );
      }
    }
  }
</script>

<style lang="scss">
 .blueprint-block
 {
   position: absolute;
   left: 0;
   right: 0;
   top: -10px;
   bottom: -10px;
   background: transparent;
   z-index: 3;
 }
</style>
