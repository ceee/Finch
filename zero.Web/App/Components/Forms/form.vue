<template>
  <form class="ui-form" @submit.prevent="submitted" @change="changed">
    <slot />
  </form>
</template>


<script>
  import Overlay from 'zeroservices/overlay.js'
  import { isArray as _isArray, filter as _filter, groupBy as _groupBy } from 'underscore'

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
      errors: [],
      errorFieldComponents: [ 'uiError' ]
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
      setErrors(errors)
      {
        if (typeof errors === 'undefined')
        {
          this.errors = [];
        }
        else if (!_isArray(errors))
        {
          this.errors = [errors];
        }
        else
        {
          this.errors = errors;
        }

        let errorGroups = _groupBy(this.errors, 'field');

        // find components which can output errors
        let traverseChildren = parent =>
        {
          parent.$children.forEach(component =>
          {
            const isErrorComponent = this.errorFieldComponents.indexOf(component.$options.name) > -1;
            const field = component.field;

            if (isErrorComponent && field)
            {
              let errorGroup = errorGroups[field];

              if (errorGroup)
              {
                component.set(errorGroup);
              }
            }
            else
            {
              traverseChildren(component);
            }
          });
        };

        traverseChildren(this);

        
      },

    }
  }
</script>