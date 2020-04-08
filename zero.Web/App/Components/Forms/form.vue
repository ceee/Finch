<template>
  <form class="ui-form" @submit="submit($event)" @change="changed">
    <slot />
  </form>
</template>


<script>
  import Overlay from 'zeroservices/overlay.js'

  export default {
    name: 'uiForm',

    data: () => ({
      dirty: false,
      root: null
    }),

    created()
    {
      
    },

    mounted()
    {
      
    },

    methods: {

      beforeRouteLeave(to, from, next) 
      {        
        if (this.dirty)
        {
          Overlay.confirm({
            title: 'You have unsaved changes',
            text: 'Are you sure you want to navigate away from this page?',
            confirmLabel: '@unsavedchanges.confirm',
            closeLabel: '@unsavedchanges.close'
          }).then(
            () => next(false),
            () => next()
          );
        }
        else
        {
          next()
        }
      },


      changed()
      {
        this.dirty = true;
      },


      submit(event)
      {
        event.preventDefault();
        console.info('submit');
      }

    }
  }
</script>