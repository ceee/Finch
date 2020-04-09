<template>
  <form class="ui-form" @submit.prevent="submitted" @change="changed">
    <slot />
  </form>
</template>


<script>
  import Overlay from 'zeroservices/overlay.js'

  export default {
    name: 'uiForm',

    props: {
      submit: {
        type: Function,
        default: () => { }
      },
      change: {
        type: Function,
        default: () => { }
      }
    },

    data: () => ({
      dirty: false,
      errors: []
    }),

    created()
    {
      
    },

    mounted()
    {
      
    },

    methods: {

      // shows a confirmation dialog for dirty forms when the route tries to change
      // it only works when this method is attached to the route component
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


      // submits the form
      submitted(e)
      {
        this.submit(e, this);
      },


      // set the form to dirty when one of the fields changes
      changed(e)
      {
        this.dirty = true;
        this.change(e);
      },


      // tries to find matching fields for the given errors and displays them
      withErrors(errors)
      {
        if (typeof errors === 'undefined')
        {
          this.errors = [];
        }
        else if (typeof errors !== 'array')
        {
          this.errors = [errors];
        }
        else
        {
          this.errors = errors;
        }

        console.info(this.errors);
      }

    }
  }
</script>