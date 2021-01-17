<template>
  <div class="ui-dropdown-container">
    <div v-if="hasButton" ref="trigger" class="ui-dropdown-toggle" @click.stop="toggle">
      <slot name="button"></slot>
    </div>
    <div class="ui-dropdown" ref="overlay" role="dialog" v-if="open" v-click-outside="hide" :class="dropdownClasses">
      <slot></slot>
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/helpers/overlay.js';

  export default {
    name: 'uiDropdown',

    props: {
      align: {
        type: String,
        default: 'left'
      },
      theme: {
        type: String,
        default: 'dark'
      },
      locked: {
        type: Boolean,
        default: false
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    computed: {

      hasButton()
      {
        return this.$scopedSlots.hasOwnProperty('button');
      },

      dropdownClasses()
      {
        let classes = 'align-' + this.align.split(' ').join(' align-');

        if (!!this.theme)
        {
          classes += ' theme-' + this.theme;
        }

        return classes;
      }
    },

    data: () => ({
      open: false
    }),


    methods: {

      toggle()
      {
        if (this.open)
        {
          this.hide();
        }
        else if (!this.disabled)
        {
          this.show();
        }
      },

      show()
      {
        if (this.disabled)
        {
          return;
        }
        Overlay.setDropdown(this);
        this.open = true;
        this.position();
        this.$emit('opened');
      },

      hide()
      {
        if (this.locked)
        {
          return;
        }
        this.open = false;
        this.$emit('closed');
      },

      position()
      {
        return; 

        this.$nextTick(() =>
        {
          // the trigger which is the relative to the overlay
          const reference = this.$refs.trigger;

          // get bounding boxes both for the trigger and the overlay
          const triggerBoundingBox = reference.getBoundingClientRect();
          const overlayBoundingBox = this.$refs.overlay.getBoundingClientRect();
          const windowBox = { width: window.innerWidth, height: window.innerHeight };
          const windowOffset = 32;

          // calculate available space for the dropdown in all 4 directions areound the trigger element
          let availableSpace = {
            left: triggerBoundingBox.left + triggerBoundingBox.width - windowOffset,
            right: windowBox.width - triggerBoundingBox.left - windowOffset,
            top: triggerBoundingBox.top + triggerBoundingBox.height - windowOffset,
            bottom: windowBox.height - triggerBoundingBox.top - windowOffset 
          };

          console.table(availableSpace);

          //console.info(triggerBoundingBox, overlayBoundingBox, windowBox);

          //const resize_ob = new ResizeObserver(function (entries)
          //{
          //  // since we are observing only a single element, so we access the first element in entries array
          //  let rect = entries[0].contentRect;

          //  // current width & height
          //  let width = rect.width;
          //  let height = rect.height;

          //  console.log('Current Width : ' + width);
          //  console.log('Current Height : ' + height);
          //});

          //// start observing for resize
          //resize_ob.observe(document.querySelector("#demo-textarea"));

          //resize_ob.unobserve(document.querySelector("#demo-textarea"));
        });
      }

    }
  }
</script>


<style lang="scss">
  .ui-dropdown-container
  {
    position: relative;
  }

  .ui-dropdown
  {
    position: absolute;
    min-width: 300px;
    min-height: 20px;
    background: var(--color-dropdown);
    border-radius: var(--radius);
    border: 1px solid var(--color-dropdown-border);
    box-shadow: var(--shadow-dropdown);
    z-index: 8;
    top: calc(100% + 5px);
    padding: 5px;
    color: var(--color-text);

    &.align-right
    {
      right: 0;
    }

    &.align-top
    {
      top: calc(100% + 5px);
      bottom: auto;
    }

    &.align-bottom
    {
      bottom: calc(100% + 5px);
      top: auto;
    }
  }
</style>