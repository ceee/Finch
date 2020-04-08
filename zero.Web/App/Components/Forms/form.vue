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
          Overlay.confirm('Confirm', 'Do you really want to leave this form?').then(() => next(), () => next(false));
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