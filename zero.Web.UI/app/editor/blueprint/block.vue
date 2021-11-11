<template>
  <div v-if="blocked" class="blueprint-block" @click="onClick">
    <!--<button type="button" class="ui-property-lock"><ui-icon symbol="fth-lock"></ui-icon></button>-->
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


    computed: {
      blocked()
      {
        return this.value.blueprint && this.value.blueprint.id;
      }
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
            this.$parent.setDisabled(false);
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

  .ui-property-lock
  {
    display: inline-flex;
    width: 3px;
    border-radius: 5px;
    justify-content: center;
    align-items: center;
    font-size: 10px;
    position: absolute;
    left: -20px;
    top: 12px;
    bottom: 8px;
    //background: repeating-linear-gradient(-45deg, transparent, transparent 2px, var(--color-accent) 2px, var(--color-accent) 4px);
    background: var(--color-line-dashed);

    .ui-icon
    {
      display: none;
    }
  }
</style>
